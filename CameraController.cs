using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{


    [Header("Îáúåêò êîòîðûé áóäåò öåíòðîì âñåé ëîêàöèè")]
    [SerializeField] public Transform globalCentr;

    [Header("Radius")]
    [SerializeField] private float globalRadius = 22;
    [SerializeField] private float localRadius = 6;

    [Header("Zoom")]
    [SerializeField] public float zoomSpeed = 2;
    [SerializeField] public float zoomMin = 1.0f;
    [SerializeField] public float zoomMax = 10.0f;
    [Header("Rotation")]
    [SerializeField] public float rotationSpeed = 3;
    [Range(0.01f, 0.1f)]
    [SerializeField] public float inhertiaDecrease = 0.0192f;
    [Header("Move")]
    [SerializeField] public float moveSpeed = 0.3f;

    public float ZoomInput { get; private set; }

    private CameraInput input;

    private Vector3 localCentrPos;
    private Vector3 oldOffset;


    private void Start()
    {
        localCentrPos = transform.position + transform.forward * localRadius * (transform.position.y * 0.2f);

#if UNITY_EDITOR
        input = new MouseCameraInput();
#elif UNITY_IOS
        input = new TouchCameraInput();
#elif UNITY_ANDROID
        input = new TouchCameraInput();
#elif UNITY_STANDALONE_WIN
        input = new MouseCameraInput();
#endif
    }

    public void LateUpdate()
    {
        ZoomInput = input.GetZoomFloat();
        Scrolling(ZoomInput);
        Move(input.GetMoveVector());
        RotateAround(input.GetRotationFloat());
        RotateAround(input.InhertiaRotation);

        if(input.InhertiaRotation != 0)
        {
            DecreaseInhertia();
        }
    }

    private void Scrolling(float input)
    {
        Vector3 pos = transform.position;
        pos += (transform.forward * input * zoomSpeed);

        if (pos.y < zoomMin)
        {
            return;
        }else if (pos.y > zoomMax)
        {
            return;
        }
        else
        {
            transform.position = pos;
        }
    }
    private void RotateAround(float rotationX)
    {

         localCentrPos = transform.position + transform.forward * localRadius * (transform.position.y * 0.2f);
       //  transform.RotateAround(localCentrPos, Vector3.up, rotationX * rotationSpeed);

        var oldRot = transform.rotation.eulerAngles;
        transform.RotateAround(globalCentr.position, new Vector3(0, 1, 0), rotationX * rotationSpeed);
        var parentRot = transform.rotation.eulerAngles;
        transform.rotation *= Quaternion.Euler(new Vector3(parentRot.x - oldRot.x, parentRot.y - parentRot.y, 0));
    }

    private void Move(Vector3 offset)
    {
        if(offset == Vector3.zero && oldOffset.magnitude > 0)
        {
            oldOffset = Vector3.Lerp(oldOffset, Vector3.zero, Time.deltaTime * 5f);
            offset = oldOffset;
        }
        else
        {
            oldOffset = offset;
        }
        var worldPos = transform.TransformPoint(-offset);
        worldPos = new Vector3(worldPos.x, transform.position.y, worldPos.z);
        float distance = Vector3.Distance(localCentrPos, globalCentr.position);

        if (distance < globalRadius)
        {
            transform.position = Vector3.Lerp(transform.position, worldPos, Time.deltaTime * moveSpeed);
        }
        else
        {
            var target = new Vector3(globalCentr.position.x, transform.position.y, globalCentr.position.z);
            transform.position = Vector3.Lerp(transform.position, worldPos, Time.deltaTime * moveSpeed);
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * ((-globalRadius + distance)/2));
        }
    }
    private void DecreaseInhertia()
    {
        float inhertia = input.InhertiaRotation;
        if (inhertia > 0)
        {
            inhertia -= inhertiaDecrease;
            if(inhertia < 0)
            {
                inhertia = 0;
            }
        }
        else if(inhertia < 0)
        {
            inhertia += inhertiaDecrease;
            if(inhertia > 0)
            {
                inhertia = 0;
            }
        }
        input.InhertiaRotation = inhertia;
    }

    void OnDrawGizmosSelected()
    {
        var target = transform.position + transform.forward * localRadius * (transform.position.y * 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(globalCentr.position, globalRadius);
        Gizmos.color = Color.red - new Color(0, 0, 0, 0.98f);
        Gizmos.DrawSphere(globalCentr.position, globalRadius);

        Gizmos.color = new Color(1, 1, 0, 0.05F);
        Gizmos.DrawSphere(target, (transform.position - target).magnitude);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, target);
   //     Gizmos.DrawWireSphere(target, (transform.position - target).magnitude);
    }


}