using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGoblinBattle : MonoBehaviour {

    public GameObject guiObject;
    public string levelToLoad;
    public int maxAmountEnemies = 4;
    //public string BattleScene;
    public List<GameObject> possibleEnemies = new List<GameObject>();

    // Use this for initialization
    void Start () {
        guiObject.SetActive(false);
	}
	
	// Update is called once per frame
	void OnTriggerStay (Collider other) {

        if (other.gameObject.tag == "Player")
        {
            guiObject.SetActive(true);
            if (guiObject.activeInHierarchy == true && Input.GetButtonDown("Use"))
            {
                Application.LoadLevel(levelToLoad);
            }
        }
	}

    void OnTriggerExit()
    {
        guiObject.SetActive(false);
    }
}
