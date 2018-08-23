using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAimController : MonoBehaviour, IAimController{
    public Camera mainCamera;
    public Vector3 defaultAimVia;
    [Range(0.5f,3.0f)]public float minAimZ;
    private float aimViaHeight = 0;
    private Vector3 prevAim = new Vector3(0, 10, 0);

    public AimControllerState GetAim()
    {
        Vector3? result = null;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                result = hit.point;
                if (Mathf.Abs(hit.point.z) < minAimZ)
                {
                    result = new Vector3(hit.point.x, hit.point.y, Mathf.Sign(hit.point.z) * minAimZ);
                }
                prevAim = (Vector3)result;
                break;
            }
        }

        //0.1ずつ
        aimViaHeight += Input.GetAxis("Mouse ScrollWheel");

        return new AimControllerState(
            result ?? prevAim,
            defaultAimVia + new Vector3(0, aimViaHeight, 0)
        );
    }
}