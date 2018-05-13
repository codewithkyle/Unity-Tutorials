using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraMovement : MonoBehaviour {

    public float speed = 100f;
    Vector3 velocity = new Vector3();
    private float axisH;
    private float axisV;

    //World bounds
    public int maxX;
    public int minX;
    public int maxY;
    public int minY;

    // Update is called once per frame
    void Update () {
        axisH = Input.GetAxis("Horizontal");
        axisV = Input.GetAxis("Vertical");

        velocity = transform.forward * axisV + transform.right * axisH;

        if (velocity.magnitude > 1) velocity.Normalize();
        transform.position += velocity * Time.deltaTime * speed;

        velocity *= 0.5f;

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        if (transform.position.z < minY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, minY);
        }
        if (transform.position.z > maxY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxY);
        }
    }
}
