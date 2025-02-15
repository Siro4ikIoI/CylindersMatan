using Parabox.CSG;
using UnityEngine;

public class DynamicCSG : MonoBehaviour
{
    public GameObject cylinder1;
    public GameObject cylinder2;
    public GameObject cylinder3;
    public Material material;
    public GameObject spawn;

    private GameObject[] resultObjects = new GameObject[3];
    private MeshFilter[] meshFilters = new MeshFilter[3];
    private MeshRenderer[] meshRenderers = new MeshRenderer[3];

    private Camera mainCamera;
    private GameObject selectedObject;
    private Vector3 offset;

    void Start()
    {
        mainCamera = Camera.main;

        for (int i = 0; i < 3; i++)
        {
            resultObjects[i] = new GameObject($"ResultObject{i + 1}");
            meshFilters[i] = resultObjects[i].AddComponent<MeshFilter>();
            meshRenderers[i] = resultObjects[i].AddComponent<MeshRenderer>();
            meshRenderers[i].material = material;

            resultObjects[i].AddComponent<BoxCollider>();
        }
    }

    void Update()
    {
        try
        {
            SteinmetzsBody();
        }
        catch { }

        HandleSelection();
        HandleMovement();
        HandleRotation();
    }

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0)) // ֻÊּ הכÿ גûבמנא
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                for (int i = 0; i < resultObjects.Length; i++)
                {
                    if (hit.collider.gameObject == resultObjects[i])
                    {
                        selectedObject = resultObjects[i];
                        offset = selectedObject.transform.position - GetMouseWorldPosition();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedObject = null;
        }
    }

    private void HandleMovement()
    {
        if (selectedObject != null && Input.GetMouseButton(0))
        {
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            selectedObject.transform.position = new Vector3(newPosition.x, newPosition.y, selectedObject.transform.position.z);
        }
    }

    private void HandleRotation()
    {
        if (selectedObject != null)
        {
            float rotationSpeed = 100f;
            if (Input.GetKey(KeyCode.Q)) selectedObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.E)) selectedObject.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.W)) selectedObject.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.S)) selectedObject.transform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.A)) selectedObject.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.D)) selectedObject.transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private void SteinmetzsBody()
    {
        bool[] intersections = new bool[3];
        intersections[0] = AreCylindersIntersecting(cylinder1, cylinder2);
        intersections[1] = AreCylindersIntersecting(cylinder1, cylinder3);
        intersections[2] = AreCylindersIntersecting(cylinder2, cylinder3);

        if (intersections[0])
        {
            UpdateBicylinder(CSG.Intersect(cylinder1, cylinder2), 1);
        }
        else if (intersections[1])
        {
            UpdateBicylinder(CSG.Intersect(cylinder1, cylinder3), 1);
        }
        else if (intersections[2])
        {
            UpdateBicylinder(CSG.Intersect(cylinder2, cylinder3), 1);
        }
        else
        {
            resultObjects[1].SetActive(false);
        }

        if (intersections[0] && intersections[1] && intersections[2])
        {
            resultObjects[2].SetActive(true);
            resultObjects[1].SetActive(false);
            resultObjects[0].SetActive(true);
            resultObjects[2].transform.position = new Vector3(0, -3, -2);
            Model bicylinder = CSG.Intersect(cylinder1, cylinder2);
            meshFilters[0].sharedMesh = bicylinder.mesh;
            meshRenderers[0].sharedMaterials = bicylinder.materials.ToArray();

            Model tricylinder = CSG.Intersect(resultObjects[0], cylinder3);
            meshFilters[2].sharedMesh = tricylinder.mesh;
            meshRenderers[2].sharedMaterials = tricylinder.materials.ToArray();
        }
        else
        {
            resultObjects[2].SetActive(false);
            resultObjects[0].SetActive(false);
        }
    }

    private void UpdateBicylinder(Model bicylinder, int index)
    {
        resultObjects[index].SetActive(true);
        resultObjects[index].transform.position = new Vector3(0, -3, -2);
        meshFilters[index].sharedMesh = bicylinder.mesh;
        meshRenderers[index].sharedMaterials = bicylinder.materials.ToArray();
    }

    private bool AreCylindersIntersecting(GameObject cylinderA, GameObject cylinderB)
    {
        Collider colliderA = cylinderA.GetComponent<Collider>();
        Collider colliderB = cylinderB.GetComponent<Collider>();
        return colliderA.bounds.Intersects(colliderB.bounds);
    }
}
