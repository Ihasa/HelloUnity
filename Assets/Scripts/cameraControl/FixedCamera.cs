using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : ICameraMode
{
    public CameraControlData GetCameraControlData(TennisPlayer tennisPlayer, GameObject ball, Vector3 posOffset)
    {
        CameraControlData cData = new CameraControlData();
        cData.Position = new Vector3(0, 12, Mathf.Sign(tennisPlayer.transform.position.z) * 20);
        cData.LookAt = Vector3.zero;
        return cData;
    }
}
