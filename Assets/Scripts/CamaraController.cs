﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour {

    public GameObject player;
    public GameObject ball;
    public float height;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void LateUpdate () {
        this.transform.position = new Vector3(player.transform.position.x, height, player.transform.position.z);
        this.transform.LookAt(new Vector3(ball.transform.position.x / 2, height, ball.transform.position.z/2));
	}
}
