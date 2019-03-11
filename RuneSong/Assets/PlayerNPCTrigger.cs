using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCTrigger : MonoBehaviour {

    private GameObject triggeringNPC;
    private bool triggering;

    public GameObject textNPC;

	// Use this for initialization
	void Update () {

        textNPC.SetActive(false);

        if (triggering)
        {
            textNPC.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E))
            {
                print("You talked to the NPC");
                // Destroy(triggeringNPC);
                triggering = false;
            }
            else
            {
                textNPC.SetActive(true);
            }
        }
	}
	
	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NPC")
        {
            triggering = true;
            triggeringNPC = other.gameObject; 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC")
        {
            triggering = false;
            triggeringNPC = null;
        }
    }
}


