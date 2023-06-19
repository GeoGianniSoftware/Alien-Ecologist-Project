using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public float moveSpeed = 2.0f;

    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 90.0f;
    private float rotX;
    public bool xLook;
    public bool yLook;
    void Update()
    {
        MouseAiming();
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

        // rotate the camera
        transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);
    }
}
