using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform target; 
    public float speed; 

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.X))
        {
            transform.RotateAround(target.position, Vector3.forward, speed * Time.deltaTime);
        }

        transform.LookAt(target);
    }
}
