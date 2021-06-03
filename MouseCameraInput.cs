using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseCameraInput : CameraInput
{
    private float moveXDelta;
    private float moveYDelta;
    private float prevMouseX;
    public override float GetZoomFloat()
    {
		return Input.GetAxis("Mouse ScrollWheel");
	}

    public override float GetRotationFloat()
    {
        if (Input.GetMouseButton(0))
        {
            InhertiaRotation = 0;
            prevMouseX = Input.GetAxis("Mouse X");
            return prevMouseX;
        }else if (Input.GetMouseButtonUp(0))
        {

            InhertiaRotation = prevMouseX;
            return 0;
        }
        else
        {
            return 0;
        }

    }

    public override Vector3 GetMoveVector()
    {
        Vector3 offset = Vector2.zero;
        if (Input.GetMouseButtonDown(2))
        {
            moveXDelta = Input.GetAxis("Mouse X");
            moveYDelta = Input.GetAxis("Mouse Y");
        }
        else if(Input.GetMouseButton(2))
        {
            float newAxisX = Input.GetAxis("Mouse X");
            float newAxisY = Input.GetAxis("Mouse Y");
            offset.x = newAxisX - moveXDelta;
            offset.z = newAxisY - moveYDelta;
            moveXDelta = 0;
            moveYDelta = 0;
        }
        return offset;
    }
}