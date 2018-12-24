using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CameraControlData
{
    public Vector3 Position;
    public Vector3 LookAt;
}
public interface ICameraMode
{
    CameraControlData GetCameraControlData(TennisPlayer tennisPlayer, GameObject ball, Vector3 posOffset);
}
