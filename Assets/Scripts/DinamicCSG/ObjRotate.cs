using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjRotate : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5.0f;
    private float distanse = 3;
    public GameObject mainCamera;
    public GameObject uiCanvas;
    private bool isHolding = false;

    private float yaw;
    private float pitch;

    public Animator animator;

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

    [System.Obsolete]
    void Update()
    {
        HandleRotation();
        transform.LookAt(target);
    }

    [System.Obsolete]
    void HandleRotation()
    {
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0) && IsPointerOverUIWithTag("Intersect") && (DynamicCSG.resultObjects[1].active == true || DynamicCSG.resultObjects[2].active == true))
        {
            DistanseUpdate();

            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            transform.position = target.position - rotation * Vector3.forward * distanse;
        }

        else if (DynamicCSG.resultObjects[1].active == false && DynamicCSG.resultObjects[2].active == false)
        {
            transform.position = new Vector3(16,0,100) - Quaternion.Euler(0, -90, 0) * Vector3.forward;
        }
    }

    bool IsPointerOverUIWithTag(string tag)
    {
        string currentTag = tag;
        if (Input.GetMouseButtonDown(0))
        {
            bool hit = PerformRaycast(tag);
            isHolding = hit;
            currentTag = hit ? tag : "";
            return hit;
        }
        else if (Input.GetMouseButton(0))
        {
            return isHolding && currentTag == tag;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            bool wasHolding = isHolding;
            isHolding = false;
            currentTag = "";
            return wasHolding && currentTag == tag;
        }

        return false;
    }

    private bool PerformRaycast(string tag)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = uiCanvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    void DistanseUpdate()
    {
        if (Input.GetKey(KeyCode.Equals) && distanse >= 2f)
        {
            distanse -= 0.5f;
        }

        if (Input.GetKey(KeyCode.Minus) && distanse <= 5f)
        {
            distanse += 0.5f;
        }
    }

    public void MaxObj()
    {
        animator.SetBool("", true);
    }
}
