using UnityEngine;

public class CylinderController : MonoBehaviour
{
    private SelectionManager selectionManager;
    private Vector3 offset;

    void Start()
    {
        selectionManager = FindObjectOfType<SelectionManager>();
    }

    void Update()
    {
        if (selectionManager.SelectedObject != null)
        {
            HandleMovement();
            HandleRotation();
            HandleScaling();
        }
    }

    void HandleMovement()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = GetMouseWorldPosition();
            selectionManager.SelectedObject.transform.position = mousePos + offset;
        }
    }

    void HandleRotation()
    {
        float rotationSpeed = 100f;
        Transform objTransform = selectionManager.SelectedObject.transform;

        if (Input.GetKey(KeyCode.Q)) objTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E)) objTransform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.W)) objTransform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) objTransform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) objTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) objTransform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
    }

    void HandleScaling()
    {
        Transform objTransform = selectionManager.SelectedObject.transform;
        float scaleStep = 1.001f; // Используем множитель

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.W)) objTransform.localScale += new Vector3(1, scaleStep, 1);
            if (Input.GetKey(KeyCode.S)) objTransform.localScale += new Vector3(1, 1 / scaleStep, 1);
            if (Input.GetKey(KeyCode.A)) objTransform.localScale += new Vector3(scaleStep, 1, scaleStep);
            if (Input.GetKey(KeyCode.D)) objTransform.localScale += new Vector3(1 / scaleStep, 1, 1 / scaleStep);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 8.7f;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
