using UnityEngine;

public class KeyControll : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedObject;

    public GameObject SelectedObject => selectedObject;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Cylinder"))
            {
                selectedObject = hit.collider.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedObject = null;
        }
    }
}
