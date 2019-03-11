using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    // CLASS RANDOM MONSTER

    public RegionData curRegion;
    public RegionData2D curRegion2D;

    public int curRegions;

    // public List<RegionData> Regions = new List<RegionData>();

    // SPAWN POINTS
    public string nextSpawnPoint;

    // HERO
    public GameObject heroCharacter;

    // POSITIONS
    public Vector3 nextHeroPosition;
    public Vector2 lastHeroPosition; // BATTLE

    // SCENES
    public string sceneToLoad;
    public string lastScene; // BATTLE

    // BOOLS
    public bool isWalking = false;
    public bool canGetEncounter = false;
    public bool gotAttacked = false;

    // ENUM
    public enum GameStates
    {
        WORLD_STATE,
        TOWN_STATE,
        BATTLE_STATE,
        IDLE
    }

    // BATTLE
    public int enemyAmount;
    public List<GameObject> enemiesToBattle = new List<GameObject>();

    public GameStates gameState;

	// Use this for initialization
	void Awake () {
		
        // check if instance exists
        if (instance == null)
        {
            // if not set the instance to this
            instance = this;
        }
        // if it exists but is not this instance
        else if (instance != this)
        {
            // destroy it 
            Destroy(gameObject);
        }
        // set this to be not destroyable
        DontDestroyOnLoad(gameObject);
        if (!GameObject.Find("HeroCharacter"))
        {
            GameObject Hero = Instantiate(heroCharacter, nextHeroPosition, Quaternion.identity) as GameObject;
            Hero.name = "HeroCharacter";
        }
	}

    void Update()
    {
        switch (gameState)
        {
            case (GameStates.WORLD_STATE):
                if (isWalking)
                {
                    RandomEncounter();
                }
                if (gotAttacked)
                {
                    gameState = GameStates.BATTLE_STATE;
                }

            break;
            case (GameStates.TOWN_STATE):

            break;
            case (GameStates.BATTLE_STATE):
                // LOAD BATTLE SCENE
                StartBattle();
                gameState = GameStates.IDLE;
                // GO TO IDLE
            break;
            case (GameStates.IDLE):

            break;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene);
    }

    void RandomEncounter()
    {
        if (isWalking && canGetEncounter)
        {
            if (Random.Range(0, 1000) < 2)
            {
                // Debug.Log("I got attacked");
                gotAttacked = true;
            }
        }
    }

    void StartBattle()
    {
        // AMOUNT OF ENEMIES
        enemyAmount = Random.Range(1,curRegion.maxAmountEnemies+1);
        // WHICH ENEMIES
        for (int i = 0; i < enemyAmount; i++)
        {
            enemiesToBattle.Add(curRegion.possibleEnemies[Random.Range(0,curRegion.possibleEnemies.Count)]);
        }
        // HERO
        lastHeroPosition = GameObject.Find("HeroCharacter").gameObject.transform.position;
        nextHeroPosition = lastHeroPosition;
        lastScene = SceneManager.GetActiveScene().name;
        // LOAD LEVEL
        SceneManager.LoadScene(curRegion.BattleScene);
        // RESET HERO
        isWalking = false;
        gotAttacked = false;
        canGetEncounter = false;
    }
}
