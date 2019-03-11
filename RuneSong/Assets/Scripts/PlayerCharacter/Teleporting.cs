using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporting : MonoBehaviour {

    public Transform TeleportTarget;
    public GameObject thePlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        thePlayer.transform.position = TeleportTarget.transform.position;
    }
}
