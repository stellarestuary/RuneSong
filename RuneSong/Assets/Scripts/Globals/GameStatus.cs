using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour {

    // static protected int lives = 3;
    static protected int money = 0;
    static protected int health = 200;
    static protected int mana = 100;

    int addGold;

    void Start()
    {
        
    }

    // money modifiers
    public void AddMoney(int s)
    {
        money += s;
        if (money > 9999999)
        {
            money = 9999999;
        }
    }

    public void SubtractMoney(int s)
    {
        money -= s;
        if (money < 0)
        {
            money = 0;
        }
    }

    // health modifiers
    // health will be capped at 9999
    // as the player becomes stronger
    public void AddHealth(int s)
    {
        health += s;
        if (health > 100) 
        {
            health = 100;
        }
    }

    public void SubtractHealth(int s)
    {
        health -= s;
        if (health < 0)
        {
            health = 0;
        }
    }

    // mana modifiers
    // magic will be capped at 999
    // as the player becomes stronger
    public void AddMana(int s)
    {
        mana += s;
        if (mana > 100)
        {
            mana = 100;
        }
    }

    public void SubtractMana(int s)
    {
        mana -= s;
        if (mana < 0)
        {
            mana = 0;
        }
    }

    // objects to return protected variables
    public static int GetMoney()
    {
        return money;
    }

    public static int GetHealth()
    {
        return health;
    }
    
    public static int GetMana()
    {
        return mana;
    }

    //
    // Let the GoldTestNPC give the player gold
    //
    public void GiveGold()
    {
        addGold = 10;
        money += addGold;
    }
}

//public static int GetLives()
//{
//    return lives;
//}

// COMMENTED IMPLEMENATATIONS FROM TUTORIAL

// There are 3 ways to persist data between scene changes
// 1) Save info into something persistent (UserPrefs, a save file)
//       - This preserves data even between game executions, not just scene changes
//    ** This is a very, very minimalist implementation. Has the benefit of
//       true persistence. Also, you don't need a single, central class to store
//       data in this way. Each object could save/load its own info.
//
// 2) Static class data. Very Simple. Occassionaly leads to weirdness from
//    inside the Unity editor, but not actually breaking things. You COULD
//    do really messy "global" style variable with this, but you can also
//    do really nice encapsulation as well.
//          ** This is the ideal implementation if your persist data
//             is just that: Data. It doesn't DO anything. It can
//             be implemented as a purely static class.
//
// 3) DontDestroyOnLoad -- This flags a GameObject such that when we change
//    from one scene to another, it doesn't get destroyed. (i.e. it is still
//    present in the newly loaded scene. To use this to the best possible advantage,
//    you need to use the Unity Singleton Design Pattern.
//          ** this is the ideal implementation if this class needs to DO
//             things, not just store things. In other words, maybe you need
//             it to have an Update that modifies the game data regularly 
//             and will need to do so on every scene.
// 

//static GameStatus instance;

//public static GameStatus GetInstance()
//{
//    return instance;
//} 

/*

public void AddScore(int s)
{
    score += s;
    // we could take this opportunity to save the score in a file/UserPrefs
    // But depending on implementation, this could be slow/inefficient.
    // On the other hand, if we are constantly saving the data on
    // the fly, it means that we pretty much never lose anything to a crash.
}

// Use this for initialization
void Start () {

    // "Implementation #1"
    // Load data form PlayerPrefs -- this might be from the 
    // previous scene, or may even from the previous execution
    // (i.e. saved between quitting and reloading the game.)
    //score = PlayerPrefs.GetInt("score", 0);
    //numLives = PlayerPrefs.GetInt("lives", 3);

    // We want to be a Singleton (i.e. there should only ever be 
    // one GameStatus instance at any given time.)

    //if (instance != null)
    //{
        // Someone ELSE is the singleton already.
        // So let's just destroy ourselves before we cause trouble.
        //Destroy(this.gameObject);
        //return;
    //}

    // If we get here, then we are "the one". Let's act like it.
    //instance = this;              // We are a highlander
    //GameObject.DontDestroyOnLoad(this.gameObject);     // Become immortal
}
*/

/*
void OnDestroy()
{
    Debug.Log("GameStatus was destroyed.");

    // Before we get destroyed, let's save our data to our save file.
    // This is "Implementation #1".
    // This will happen whenever this objet is destroyed, which
    // includes scene changes as well as simply exiting the program.s
    //PlayerPrefs.SetInt("score", score);
    //PlayerPrefs.SetInt("lives", numLives); 
}
*/
