using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedParticle : MonoBehaviour {

    public ParticleSystem particle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        particle.transform.position = collision.transform.position;
        particle.Emit(10);
    }
}
