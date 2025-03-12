using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class CylindController : MonoBehaviour
{
    public LayerMask selectableLayer;
    public LayerMask gizmoLayer;
    public GameObject gizmoPrefab;
    private GameObject activeGizmo;
    private Transform selectedObject;
    private Transform activeAxis;
    private Vector3 lastMousePosition;
    public GameObject referenceObject;
    public static bool isMuwment =  false;
    public Transform[] cylinders;

    public Slider xSlider;
    public Slider ySlider;
    public Slider zSlider;

    public InputField[] radius;
    public InputField[] heights;

    private Transform currentObject;

    private void Start()
    {
        xSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        ySlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        zSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    private void Update()
    {
        HandleSelection();
        HandleDragging();
        CylinderHeights();
        CylinderRadius();
    }

    void HandleSelection()
    {
        Rect allowedArea = new Rect(0, 0, Screen.width * 0.24f, Screen.height);

        if (allowedArea.Contains(Input.mousePosition))
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, gizmoLayer))
            {
                activeAxis = hit.transform;
                lastMousePosition = Input.mousePosition;
                return;
            }

            if (Physics.Raycast(ray, out hit, 100f, selectableLayer))
            {
                SelectObject(hit.transform);
            }
            else
            {
                DeselectObject();
            }
        }
    }


    public void GetCylindr(int cylinder)
    {
        SelectObject(cylinders[cylinder]);
        currentObject = cylinders[cylinder];
        xSlider.value = cylinders[cylinder].transform.rotation.x;
        ySlider.value = cylinders[cylinder].transform.rotation.y;
        zSlider.value = cylinders[cylinder].transform.rotation.z;
    }

    void HandleDragging()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 objectPosition = referenceObject.transform.position;
        Vector3 cameraToObject = (objectPosition - Camera.main.transform.position).normalized;

        float yawAngle = Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg;
        if (yawAngle < 0) yawAngle += 360f;

        Debug.Log(yawAngle);
        if (activeAxis == null || selectedObject == null) return;

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            float moveAmount = 0.01f * (activeAxis.name == "Z_Axis" ? mouseDelta.y : mouseDelta.x);

            if (activeAxis.name == "X_Axis" && yawAngle <= 270 && yawAngle >= 90) selectedObject.position += new Vector3(-moveAmount, 0, 0);
            if ((activeAxis.name == "X_Axis") && ((yawAngle >= 0 && yawAngle <= 90) || (yawAngle <= 360  && yawAngle >= 270))) selectedObject.position += new Vector3(moveAmount, 0, 0);
            if (activeAxis.name == "Z_Axis") selectedObject.position += new Vector3(0, moveAmount, 0);
            if (activeAxis.name == "Y_Axis" && yawAngle >= 0 && yawAngle <= 180) selectedObject.position += new Vector3(0, 0, -moveAmount);
            if (activeAxis.name == "Y_Axis" && yawAngle >= 180 && yawAngle <= 360) selectedObject.position += new Vector3(0, 0, moveAmount);

            lastMousePosition = Input.mousePosition;
            isMuwment = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            activeAxis = null;
            isMuwment = false;
        }
    }

    void SelectObject(Transform obj)
    {
        if (selectedObject == obj) return;

        DeselectObject();
        selectedObject = obj;
        currentObject = obj;
        activeGizmo = Instantiate(gizmoPrefab, obj.position, Quaternion.identity);
        activeGizmo.transform.SetParent(obj);
    }

    void DeselectObject()
    {
        selectedObject = null;
        if (activeGizmo) Destroy(activeGizmo);
        activeGizmo = null;
        activeAxis = null;
    }

    void OnSliderValueChanged()
    {
        if (currentObject != null)
        {
            Vector3 newRotation = new Vector3(xSlider.value, ySlider.value, zSlider.value);
            currentObject.rotation = Quaternion.Euler(newRotation);
        }
    }

    public void Bicylinder()
    {
        cylinders[0].position = new Vector3(4,  5.25f, 5);
        cylinders[1].position = new Vector3(4, 5.25f, 5);

        cylinders[0].rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        cylinders[1].rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

    public void Tricylinder()
    {
        cylinders[0].position = new Vector3(4, 5.25f, 5);
        cylinders[1].position = new Vector3(4, 5.25f, 5);
        cylinders[2].position = new Vector3(4, 5.25f, 5);

        cylinders[0].rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        cylinders[1].rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        cylinders[2].rotation = Quaternion.Euler(new Vector3(0, 0, 90));
    }

    public void CylinderHeights()
    {
        cylinders[0].transform.localScale = new Vector3(cylinders[0].transform.localScale.x, float.Parse(heights[0].text), cylinders[0].transform.localScale.z);
        cylinders[1].transform.localScale = new Vector3(cylinders[1].transform.localScale.x, float.Parse(heights[1].text), cylinders[1].transform.localScale.z);
        cylinders[2].transform.localScale = new Vector3(cylinders[2].transform.localScale.x, float.Parse(heights[2].text), cylinders[2].transform.localScale.z);
    }

    public void CylinderRadius()
    {
        cylinders[0].transform.localScale = new Vector3(float.Parse(radius[0].text) * 2, cylinders[0].transform.localScale.y, float.Parse(radius[0].text) * 2);
        cylinders[1].transform.localScale = new Vector3(float.Parse(radius[1].text) * 2, cylinders[1].transform.localScale.y, float.Parse(radius[1].text) * 2);
        cylinders[2].transform.localScale = new Vector3(float.Parse(radius[2].text) * 2, cylinders[2].transform.localScale.y, float.Parse(radius[2].text) * 2);
    }
}
