﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroToMap : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Application.LoadLevel("WorldMap");
        }
    }
}
