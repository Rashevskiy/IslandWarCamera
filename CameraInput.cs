using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraInput
{
    public float InhertiaRotation { get; set; }

    public abstract float GetZoomFloat();
    public abstract float GetRotationFloat();
    public abstract Vector3 GetMoveVector();
}