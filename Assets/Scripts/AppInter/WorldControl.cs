using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldControl : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5.0f;
    private float distanse = 8;
    public static Quaternion CameraRotation;

    private float yaw;
    private float pitch;

    void Start()
    {
        if (target == null)
        {
            enabled = false;
            return;
        }

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        HandleRotation();
        DistanseUpdate();
        transform.LookAt(target);
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            CameraRotation = rotation;
            transform.position = target.position - rotation * Vector3.forward * distanse;
        }
    }

    void DistanseUpdate()
    {
        if (Input.GetKey(KeyCode.Equals) && distanse >= 3f)
        {
            distanse -= 0.5f;
        }

        if (Input.GetKey(KeyCode.Minus) && distanse <= 16f)
        {
            distanse += 0.5f;
        }
    }
}
