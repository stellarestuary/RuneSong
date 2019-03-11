using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerLoadScene : MonoBehaviour {

    public GameObject guiObject;
    //public string levelToLoad;
    //public string spawnPointName;

    // Use this for initialization
    void Start () {
        guiObject.SetActive(false);
	}
	
	// Update is called once per frame
	void OnTriggerStay2D (Collider2D other) {
		
        if (other.gameObject.tag == "Player")
        {
            guiObject.SetActive(true);
            /*
            if (guiObject.activeInHierarchy == true && Input.GetButtonDown("Use"))
            {
                Application.LoadLevel(levelToLoad);
            }
            */
        }
	}

    void OnTriggerExit2D()
    {
        guiObject.SetActive(false);
    }
}
