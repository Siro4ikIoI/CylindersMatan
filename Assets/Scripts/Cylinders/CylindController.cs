using UnityEngine;
using TMPro;

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

    private void Update()
    {
        HandleSelection();
        HandleDragging();
    }

    void HandleSelection()
    {
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
        }

        if (Input.GetMouseButtonUp(0)) activeAxis = null;
    }

    void SelectObject(Transform obj)
    {
        if (selectedObject == obj) return;

        DeselectObject();
        selectedObject = obj;
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
}
