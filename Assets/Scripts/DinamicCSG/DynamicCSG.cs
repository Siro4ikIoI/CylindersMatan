using Assets.Scripts.DinamicCSG;
using Parabox.CSG;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCSG : MonoBehaviour
{
    public GameObject[] cylinders;

    public static GameObject[] resultObjects = new GameObject[3];
    private MeshFilter[] meshFilters = new MeshFilter[3];
    private MeshRenderer[] meshRenderers = new MeshRenderer[3];

    public GameObject volumeText;
    public Material[] materials;

    private string indexDisplay;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            resultObjects[i] = new GameObject($"ResultObject{i + 1}");
            meshFilters[i] = resultObjects[i].AddComponent<MeshFilter>();
            meshRenderers[i] = resultObjects[i].AddComponent<MeshRenderer>();
            meshRenderers[i].material = materials[1];
        }

        resultObjects[1].transform.position = gameObject.transform.position;
        resultObjects[2].transform.position = gameObject.transform.position;

        resultObjects[1].AddComponent<MultiPlaneProjection>();
        resultObjects[2].AddComponent<MultiPlaneProjection>();
    }

    [System.Obsolete]
    void Update()
    {
        bool[] intersections = new bool[3];
        intersections[0] = GenResultObj.AreCylindersIntersecting(cylinders[0], cylinders[1]);
        intersections[1] = GenResultObj.AreCylindersIntersecting(cylinders[0], cylinders[2]);
        intersections[2] = GenResultObj.AreCylindersIntersecting(cylinders[1], cylinders[2]);
        SetDisplay();

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
            Model bicylinder = CSG.Intersect(cylinders[0], cylinders[1]);
            GenResultObj.Intersect(resultObjects[0], bicylinder);
            GenResultObj.ResultObjects(resultObjects[1], bicylinder);
            VolumeText(1);
        }
        else if (in2)
        {
            Model bicylinder = CSG.Intersect(cylinders[0], cylinders[2]);
            GenResultObj.Intersect(resultObjects[0], bicylinder);
            GenResultObj.ResultObjects(resultObjects[1], bicylinder);
            VolumeText(1);
        }
        else if (in3)
        {
            Model bicylinder = CSG.Intersect(cylinders[1], cylinders[2]);
            GenResultObj.Intersect(resultObjects[0], bicylinder);
            GenResultObj.ResultObjects(resultObjects[1], bicylinder);
            VolumeText(1);
        }
        else
        {
            resultObjects[0].SetActive(false);
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
            Model tricylinder = CSG.Intersect(resultObjects[0], cylinders[2]);
            GenResultObj.Intersect(resultObjects[0], tricylinder);
            GenResultObj.ResultObjects(resultObjects[2], tricylinder);
            VolumeText(2);
        }
        else
        {
            resultObjects[2].SetActive(false);
        }
    }

    private void VolumeText(int index)
    {
        volumeText.SetActive(true);
        volumeText.GetComponent<Text>().text = "V = " + GenResultObj.VolumeText(resultObjects[index]) + "...";
    }

    public void SetDisplayButon(string ID)
    {
        switch (ID)
        {
            case "Transparent":
                indexDisplay = "Transparent";
                break;
            case "Main":
                indexDisplay = "Main";
                break;
            case "1":

                break;
        }
    }

    public void SetDisplay()
    {
        switch (indexDisplay)
        {
            case "Transparent":
                CylinderDisplay.Transparent(cylinders, meshRenderers, materials);
                break;
            case "Main":
                CylinderDisplay.MainMaterial(cylinders, meshRenderers, materials);
                break;
            case "1":

                break;
        }
    }
}
