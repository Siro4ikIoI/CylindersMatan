using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public static float CalculateMeshVolume(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        float volume = 0f;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = vertices[triangles[i]];
            Vector3 v1 = vertices[triangles[i + 1]];
            Vector3 v2 = vertices[triangles[i + 2]];

            volume += SignedVolumeOfTriangle(v0, v1, v2);
        }

        return Mathf.Abs(volume);
    }

    public static float SignedVolumeOfTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        return Vector3.Dot(v0, Vector3.Cross(v1, v2)) / 6.0f; 
    }
}
