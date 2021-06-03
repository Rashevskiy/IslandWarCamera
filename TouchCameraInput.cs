using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCameraInput : CameraInput
{
    private float oldDistance;
    private float deltaAngle;
    private Vector2 firstPoint;
    private Vector2 secondPoint;

    private Vector3 oldPoint;
    private Mode mode = Mode.Null;


    public override Vector3 GetMoveVector()
    {
        if (Input.touchCount == 0)
        {
            mode = Mode.Null;
            oldDistance = 0;
        }

        if (Input.touchCount > 1 && mode != Mode.Zooming)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                oldPoint = GetPinchMiddlePosition();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                var newPosTouch = GetPinchMiddlePosition();
                var newMoveDelta = (new Vector3(newPosTouch.x, 0, newPosTouch.y) - new Vector3(oldPoint.x, 0, oldPoint.y));
                oldPoint = newPosTouch;
                var result = newMoveDelta / 20;
                if (result.magnitude > 0.34f)
                {
                    mode = Mode.Moving;
                }

                return result;
            }

        }
        return Vector3.zero;
    }

    public override float GetRotationFloat()
    {

        if (Input.touchCount == 1 && mode == Mode.Null)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstPoint = Input.GetTouch(0).position;
                InhertiaRotation = 0f;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondPoint = Input.GetTouch(0).position;
                deltaAngle = (secondPoint.x - firstPoint.x) * 4 / Screen.width;
                return deltaAngle;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                InhertiaRotation = deltaAngle;
            }
        }

        if (Input.touchCount > 0)
        {
            firstPoint = Input.GetTouch(0).position;
        //    InhertiaRotation = 0f;
        }
        return 0;
    }

    public override float GetZoomFloat()
    {
        var delta = 0f;

        if (Input.touchCount > 1 && mode != Mode.Moving)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                oldDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                var distance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

                delta = (distance - oldDistance) / Screen.width;

                oldDistance = distance;
                if(Mathf.Abs(delta) * 25 > 0.2f)
                {
                    mode = Mode.Zooming;
                }
            }
        }
        return delta * 25;
    }

    private Vector3 GetPinchMiddlePosition()
    {
        var touch0 = Input.GetTouch(0).position;
        var touch1 = Input.GetTouch(1).position;
        return (touch0 + touch1) / 2;
    }

    private Vector3 GetWorldPoint(Vector2 screenPoint)
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        float distance = 0f;
        plane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }


    internal enum Mode
    {
        Moving,
        Zooming,
        Null
    }
}