using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float turnSpeed = 4.0f;
    public float moveSpeed = 2.0f;
    public float smoothing;

    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 90.0f;
    private float rotX;
    public bool xLook;
    public bool yLook;
    private void Awake() {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.Rotate(Vector3.up, target.eulerAngles.y);
    }

    void Update() {
        MouseAiming();
        if(target != null) {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, smoothing);
           
        }
    }

    void MouseAiming() {
        // get the mouse inputs
        float y = Input.GetAxis("Mouse X") * turnSpeed;
        if (!yLook)
            y = transform.rotation.y;
        rotX += Input.GetAxis("Mouse Y") * turnSpeed;
        if (!xLook)
            rotX = transform.rotation.x;
        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

        float finY = transform.eulerAngles.y + y;
        if (!yLook)
            finY = target.transform.eulerAngles.y;
        // rotate the camera
        transform.eulerAngles = new Vector3(-rotX, finY, 0);
    }
}
