using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour {

    public GameObject player;
    public GameObject ball;
    public float height;
    public Projectiler projectiler;
    private Camera cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                projectiler.testAim = hit.point;
                projectiler.aimedPoint.transform.position = hit.point;
            }
        }
        
    }

    // Update is called once per frame
    void LateUpdate () {
        this.transform.position = new Vector3(player.transform.position.x, height, player.transform.position.z);
        this.transform.LookAt(new Vector3(ball.transform.position.x / 2, height, ball.transform.position.z/2));
	}
}
