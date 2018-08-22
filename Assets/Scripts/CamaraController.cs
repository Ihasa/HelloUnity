using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour {

    public GameObject player;
    public float height;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void LateUpdate () {
        GameObject ball = player.GetComponent<TennisPlayer>().ballObject;
        this.transform.position = new Vector3(player.transform.position.x, height, player.transform.position.z);
	}
}
