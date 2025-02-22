using Parabox.CSG;
using UnityEngine;

namespace Assets.Scripts.DinamicCSG
{
    class GenResultObj : DynamicCSG
    {
        public static void ResultObjects(GameObject resultObject,Model model)
        {
            resultObject.SetActive(true);
            resultObject.GetComponent<MeshFilter>().sharedMesh = model.mesh;
            resultObject.GetComponent<MeshRenderer>().sharedMaterials = model.materials.ToArray();

            MeshFilter meshFilter = resultObject.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.mesh;
                Vector3[] vertices = mesh.vertices;
                Vector3 center = mesh.bounds.center;

                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] -= center;
                }

                mesh.vertices = vertices;
                mesh.RecalculateBounds();
            }
        }

        public static bool AreCylindersIntersecting(GameObject cylinderA, GameObject cylinderB)
        {
            Collider colliderA = cylinderA.GetComponent<Collider>();
            Collider colliderB = cylinderB.GetComponent<Collider>();
            return colliderA.bounds.Intersects(colliderB.bounds);
        }

        public static string VolumeText(GameObject resultObject)
        {
            float volume = Volume.CalculateMeshVolume(resultObject.GetComponent<MeshFilter>().mesh);
            return volume.ToString();
        }
    }
}
