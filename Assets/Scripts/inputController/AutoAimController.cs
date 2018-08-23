using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimController : MonoBehaviour, IAimController{
    private GameObject player;
    private Vector3 defaultAimVia;
    private Vector2 aimRangeX;
    private Vector2 aimRangeZ;
    public AutoAimController(GameObject player, Vector3 defaultAimVia, Vector2 aimRangeX, Vector2 aimRangeZ)
    {
        this.player = player;
        this.defaultAimVia = defaultAimVia;
        this.aimRangeX = aimRangeX;
        this.aimRangeZ = aimRangeZ;
    }
    public AimControllerState GetAim()
    {
        return new AimControllerState(
            new Vector3(Random.Range(aimRangeX.x, aimRangeX.y), 0, -Mathf.Sign(player.transform.position.z) * Random.Range(aimRangeZ.x, aimRangeZ.y)),
            defaultAimVia // + Vector3.up * Random.Range(-aimRangeY.x, aimRangeY.y)
        );
    }
}
