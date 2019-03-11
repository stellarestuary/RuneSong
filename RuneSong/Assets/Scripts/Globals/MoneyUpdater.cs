using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUpdater : MonoBehaviour {

    void MoneyUpdate()
    {
        GetComponent<Text>().text = "Money: " + GameStatus.GetMoney(); // + " Lives: " + GameStatus.GetLives();
    }

    void Update () {

        MoneyUpdate();
        //HealthManaUpdate();
    }
}

/*
// COMMENTED IMPLEMENATATIONS FROM TUTORIAL
// relatively slow/bad way to grab GameStatus
GameObject go = GameObject.Find("GameStatus");

if(go == null)
{
    Debug.LogError("Failed to find an object named 'GameStatus'");
    this.enabled = false; // this line prevents the debugger from spamming hte log
    return;
}

// it's the GameStatus script we actually care about
GameStatus gs = go.GetComponent<GameStatus>();

GetComponent<Text>().text = "Score: " + gs.score + " Lives: " + gs.lives;
*/
