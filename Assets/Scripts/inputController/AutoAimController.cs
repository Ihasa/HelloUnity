using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimController : MonoBehaviour, IAimController{
    public Vector3 defaultAimVia;
    public Vector2 aimRangeX;
    public Vector2 aimRangeZ;

    public AimControllerState GetAim()
    {
        return new AimControllerState(
            new Vector3(Random.Range(aimRangeX.x, aimRangeX.y), 0, -Mathf.Sign(gameObject.transform.position.z) * Random.Range(aimRangeZ.x, aimRangeZ.y)),
            defaultAimVia // + Vector3.up * Random.Range(-aimRangeY.x, aimRangeY.y)
        );
    }
}
