using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLookCamera : PlayerCamera
{
    public override CameraControlData GetCameraControlData(TennisPlayer tennisPlayer, GameObject ball, Vector3 offset)
    {
        CameraControlData data = base.GetCameraControlData(tennisPlayer, ball, offset);
        data.LookAt = ball.transform.position;
        return data;
    }
}
