﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public BaseHero hero;

    public enum TurnState {

        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState;
    // for the ProgressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar;
    public GameObject Selector;
    //IeNumerator
    public GameObject EnemyToAttack;
    private bool actionStarted = false;
    private Vector3 startposition;
    private float animSpeed = 10f;
    private bool alive = true;
    // heroPanel
    private HeroPanelStats stats;
    public GameObject HeroPanel;
    private Transform HeroPanelSpacer;

	// Use this for initialization
	void Start () {

        // find spacer 
        HeroPanelSpacer = GameObject.Find("BattleCanvas").transform.FindChild("HeroPanel").transform.FindChild("HeroPanelSpacer");
        // create panel , fill in info
        CreateHeroPanel();

        startposition = transform.position;
        cur_cooldown = Random.Range(0, 2.5f);
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.PROCESSING;
	}
	
	// Update is called once per frame
	void Update () {

        // Debug.Log(currentState);
        switch (currentState) {

            case (TurnState.PROCESSING):
                UpgradeProgressBar();

            break;
            case (TurnState.ADDTOLIST):
                BSM.HeroesToManage.Add (this.gameObject);
                currentState = TurnState.WAITING;
            break;
            case (TurnState.WAITING):
                // idle
            break;
            case (TurnState.ACTION):
                StartCoroutine(TimerForAction());
            break;
            case (TurnState.DEAD):
                if (!alive)
                {
                    return;
                }
                else
                {
                    // change tag
                    this.gameObject.tag = "DeadHero";
                    // not attackable by enemy
                    BSM.HeroesInBattle.Remove(this.gameObject);
                    // not manageable
                    BSM.HeroesToManage.Remove(this.gameObject);
                    // deactivate the selector
                    Selector.SetActive(false);
                    // reset GUI
                    BSM.AttackPanel.SetActive(false);
                    BSM.EnemySelectPanel.SetActive(false);
                    // remove item from PerformList
                    if (BSM.HeroesInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PerformList.Count; i++)
                        {
                            if (i != 0)
                            {
                                if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                                {
                                    BSM.PerformList.Remove(BSM.PerformList[i]);
                                }

                                else if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                                {
                                    BSM.PerformList[i].AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
                                }
                            }
                        }
                    }
                    // change color / play death animation
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
                    // reset HeroInput
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
                    alive = false;
                }
            break;
        }
	}

    void UpgradeProgressBar () {

        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if(cur_cooldown >= max_cooldown) {

            currentState = TurnState.ADDTOLIST;
        }
    }

    private IEnumerator TimerForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // animate the enemy near the hero to attack
        Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x + 1.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
        while (MoveTowardsEnemy(enemyPosition)) { yield return null; }

        // wait 
        yield return new WaitForSeconds(0.5f);

        // do damage
        DoDamage();

        // animate back to startposition
        Vector3 firstPosition = startposition;
        while (MoveTowardsStart(firstPosition)) { yield return null; }

        // remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);
        
        // reset BSM -> Wait
        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
        {
            BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
            // reset this enemy state
            cur_cooldown = 0f;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }
        // end coroutine
        actionStarted = false;
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    public void TakeDamage(float getDamageAmount)
    {
        hero.curHP -= getDamageAmount;
        if (hero.curHP <= 0)
        {
            hero.curHP = 0;
            currentState = TurnState.DEAD;
        }
        UpdateHeroPanel();
    }

    // do damage
    void DoDamage()
    {
        float calc_damage = hero.curATK + BSM.PerformList[0].chooseAnAttack.attackDamage;
        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage);
    }


    // create a hero panel
    void CreateHeroPanel()
    {
        HeroPanel = Instantiate(HeroPanel) as GameObject;
        stats = HeroPanel.GetComponent<HeroPanelStats>();
        stats.HeroName.text = hero.theName;
        stats.HeroHP.text = "HP: " + hero.curHP;
        stats.HeroMP.text = "MP: " + hero.curMP;

        ProgressBar = stats.ProgressBar;
        HeroPanel.transform.SetParent(HeroPanelSpacer, false);
    }

    // update stats on damage / heal
    void UpdateHeroPanel()
    {
        stats.HeroHP.text = "HP: " + hero.curHP;
        stats.HeroMP.text = "MP: " + hero.curMP;
    }
}
