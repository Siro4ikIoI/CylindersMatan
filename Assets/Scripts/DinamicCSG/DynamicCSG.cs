using Assets.Scripts.DinamicCSG;
using Parabox.CSG;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCSG : MonoBehaviour
{
    public GameObject cylinder1;
    public GameObject cylinder2;
    public GameObject cylinder3;

    private GameObject[] resultObjects = new GameObject[3];
    private MeshFilter[] meshFilters = new MeshFilter[3];
    private MeshRenderer[] meshRenderers = new MeshRenderer[3];

    public GameObject volumeText;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            resultObjects[i] = new GameObject($"ResultObject{i + 1}");
            meshFilters[i] = resultObjects[i].AddComponent<MeshFilter>();
            meshRenderers[i] = resultObjects[i].AddComponent<MeshRenderer>();
        }

        resultObjects[1].transform.position = gameObject.transform.position;
        resultObjects[2].transform.position = gameObject.transform.position;
    }

    [System.Obsolete]
    void Update()
    {
        bool[] intersections = new bool[3];
        intersections[0] = GenResultObj.AreCylindersIntersecting(cylinder1, cylinder2);
        intersections[1] = GenResultObj.AreCylindersIntersecting(cylinder1, cylinder3);
        intersections[2] = GenResultObj.AreCylindersIntersecting(cylinder2, cylinder3);

        try
        {
            Bicylinder(intersections[0], intersections[1], intersections[2]);
            Tricylinder(intersections[0], intersections[1], intersections[2]);
        }
        catch { }
    }

    private void Bicylinder(bool in1, bool in2, bool in3)
    {
        if (in1)
        {
            Model bicylinder = CSG.Intersect(cylinder1, cylinder2);
            GenResultObj.ResultObjects(resultObjects[1], bicylinder);
            VolumeText(1);
        }
        else if (in2)
        {
            Model bicylinder = CSG.Intersect(cylinder1, cylinder3);
            GenResultObj.ResultObjects(resultObjects[1], bicylinder);
            VolumeText(1);
        }
        else if (in3)
        {
            Model bicylinder = CSG.Intersect(cylinder2, cylinder3);
            GenResultObj.ResultObjects(resultObjects[1], bicylinder);
            VolumeText(1);
        }
        else
        {
            resultObjects[1].SetActive(false);
            volumeText.SetActive(false);
        }
    }

    private void Tricylinder(bool in1, bool in2, bool in3)
    {
        if (in1 && in2 && in3)
        {
            volumeText.SetActive(true);
            resultObjects[1].SetActive(false);
            resultObjects[0].SetActive(true);
            Model bicylinder = CSG.Intersect(cylinder1, cylinder2);
            meshFilters[0].sharedMesh = bicylinder.mesh;
            meshRenderers[0].sharedMaterials = bicylinder.materials.ToArray();

            Model tricylinder = CSG.Intersect(resultObjects[0], cylinder3);
            GenResultObj.ResultObjects(resultObjects[2], tricylinder);
            VolumeText(2);
        }
        else
        {
            resultObjects[2].SetActive(false);
            resultObjects[0].SetActive(false);
        }
    }

    private void VolumeText(int index)
    {
        volumeText.SetActive(true);
        volumeText.GetComponent<Text>().text = GenResultObj.VolumeText(resultObjects[index]);
    }

    //Venomus
}
