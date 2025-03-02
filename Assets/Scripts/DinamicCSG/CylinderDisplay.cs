using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderDisplay : MonoBehaviour
{
    public static void MainMaterial(GameObject[] cylinders, MeshRenderer[] resObj, Material[] materials)
    {
        for(int i = 0;i < 3; i++)
        {
            cylinders[i].GetComponent<MeshRenderer>().material = materials[1];
            resObj[i].sharedMaterial = materials[1];
        }
    }
    public static void Transparent(GameObject[] cylinders, MeshRenderer[] resObj, Material[] materials)
    {
        for (int i = 0; i < 3; i++)
        {
            cylinders[i].GetComponent<MeshRenderer>().material = materials[0];
            resObj[i].materials = materials;
        }
    }
}
