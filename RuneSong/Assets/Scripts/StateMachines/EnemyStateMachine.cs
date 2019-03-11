using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public BaseEnemy enemy;
    public int ID { get; set; }

    public enum TurnState {

        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;
    // for the ProgressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 10f;
    // this gameobject
    private Vector3 startposition;
    public GameObject Selector;
    // timeforaction check
    private bool actionStarted = false;
    public GameObject HeroToAttack;
    private float animSpeed = 10f;

    // alive
    private bool alive = true;

    // Use this for initialization
    void Start () {

        currentState = TurnState.PROCESSING;
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine> ();
        startposition = transform.position;

        ID = 0;
    }

    // Update is called once per frame
    void Update () {

        // Debug.Log(currentState);
        switch (currentState)
        {

            case (TurnState.PROCESSING):
                UpgradeProgressBar ();
                break;

            case (TurnState.CHOOSEACTION):
                ChooseAction ();
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                //idle state
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
                    // change tag of enemy
                    this.gameObject.tag = "DeadEnemy";
                    // not attackable by heroes
                    BSM.EnemiesInBattle.Remove(this.gameObject);
                    // disable the selector
                    Selector.SetActive(false);
                    // remove all inputs hero attacks
                    if(BSM.EnemiesInBattle.Count > 0)
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
                                    BSM.PerformList[i].AttackersTarget = BSM.EnemiesInBattle[Random.Range(0, BSM.EnemiesInBattle.Count)];
                                }
                            }
                        }
                    }
                    // change the color to gray / play dead animation
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
                    // set alive false
                    alive = false;
                    // reset enemy buttons
                    BSM.EnemyButtons();
                    // check alive
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
                }
            break;
        }
    }

    void UpgradeProgressBar () {

        cur_cooldown = cur_cooldown + Time.deltaTime;

        if (cur_cooldown >= max_cooldown)
        {

            currentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction ()
    {
        HandleTurn myAttack = new HandleTurn ();
        myAttack.Attacker = enemy.theName;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];

        int num = Random.Range(0, enemy.attacks.Count);
        myAttack.chooseAnAttack = enemy.attacks [num];
        Debug.Log(this.gameObject.name + " has chosen " + myAttack.chooseAnAttack.attackName + " and does " + myAttack.chooseAnAttack.attackDamage + " damage!");

        BSM.CollectActions (myAttack);
    }

    private IEnumerator TimerForAction ()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // animate the enemy near the hero to attack
        Vector3 heroPosition = new Vector3(HeroToAttack.transform.position.x-1.5f,HeroToAttack.transform.position.y,HeroToAttack.transform.position.z);
        while(MoveTowardsEnemy(heroPosition)) { yield return null; }

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
        BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
        // end coroutine
        actionStarted = false;
        // reset this enemy state
        cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;
    }
    
    private bool MoveTowardsEnemy (Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    void DoDamage()
    {
        float calc_damage = enemy.curATK + BSM.PerformList[0].chooseAnAttack.attackDamage;
        HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage);
    }

    public void TakeDamage(float getDamageAmount)
    {
        enemy.curHP -= getDamageAmount;
        if (enemy.curHP <= 0)
        {
            enemy.curHP = 0;
            currentState = TurnState.DEAD;
        }
    }
}