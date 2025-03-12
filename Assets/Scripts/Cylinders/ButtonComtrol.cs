using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonComtrol : MonoBehaviour
{
    public Transform[] cylinders;
    private Transform selectedObject;
    private GameObject activeGizmo;
    public GameObject gizmoPrefab;

    public void GetCylindr(int cylinder)
    {
        SelectObject(cylinders[cylinder]);
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
    }
}
