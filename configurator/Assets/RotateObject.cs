using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    float rotationSpeed = 20f;

    private void Start()
    {
        ResetRotation();
    }
    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * 0.1f * Mathf.Deg2Rad;
        transform.RotateAround(Vector3.up, -rotX);
       // transform.RotateAround(Vector3.right, rotY);

    }

    public void ResetRotation()
    {
        GameObject.FindObjectOfType<RotateObject>().transform.rotation = Quaternion.Euler(0f, 30f, 0f);
    }
}
