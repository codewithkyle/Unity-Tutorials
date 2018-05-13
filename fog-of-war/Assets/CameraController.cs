using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This class takes the users input and controls the cameras interactions.
/// The users can rotate the camera or move its position to a new target location.
/// </summary>
public class CameraController : MonoBehaviour
{

    /// <summary>
    /// This is the public info for the cameras rotation.
    /// </summary>
    public Transform camYaw;
    public Transform camPitch;
    private bool invertLookY = true;

    /// <summary>
    /// lerpSpeed is the multiplier for deltaTime. 
    /// </summary>
    private float lerpSpeed;

    /// <summary>
    /// This is the public sensitivity info. It should be updated in the inspector.
    /// </summary>
    private float MouseSensitivityX = 4;
    private float MouseSensitivityY = 4;
    private float MouseSensitivityScroll = 6;
    private float MobileSensitivity = 12;
    private float MobileSensitivityScroll = 60;

    /// <summary>
    /// This is our max and min zoom values. It should be updated in the inspector.
    /// </summary>
    public float maxZoom = 30;
    public float minZoom = 10;

    /// <summary>
    /// This is the private info such as the moues starting position, the pitch, yaw, and scroll amounts.
    /// </summary>
    float camPitchAmount;
    float yaw;
    float scroll;

    /// <summary>
    /// This is the desired yaw, pitch, and zoom that is set when a user updated the POI.
    /// </summary>
    private float desiredYaw;
    private float desiredPitch;
    private float desiredZoom;

    /// <summary>
    /// This is the previous distance between the users fingers on mobile
    /// </summary>
    private float prevDistance = 0;

    /// <summary>
    /// In this start function we set our max and min zoom to the negative version of the public version.
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        maxZoom *= -1;
        minZoom *= -1;
    }

    /// <summary>
    /// In this update funciton we call the MouseInput function every update and the UpdateCamera funciton if we are currently allowing camera updates.
    /// </summary>
    void Update()
    {
        if(Input.GetMouseButton(0)) MouseInput();
        HandleZoom();
    }

    void MouseInput()
    {
        yaw = Input.GetAxis("Mouse X");
        camPitchAmount = Input.GetAxis("Mouse Y") * MouseSensitivityY * (invertLookY ? -1 : 1);

        HandleOrbit(yaw, camPitchAmount);

        LimitOrbit();
    }

    void HandleZoom()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");
        //print(scroll);
        if (scroll != 0)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + scroll * MouseSensitivityScroll);
            LimitZoom();
        }
    }

    /// <summary>
    /// This funciton will limit the minimum and maximum zoom.
    /// </summary>
    void LimitZoom()
    {
        if (transform.localPosition.z > minZoom)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, minZoom);
        }

        if (transform.localPosition.z < maxZoom)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, maxZoom);
        }
    }

    void MobileInput()
    {
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            yaw = Input.GetTouch(0).deltaPosition.x / MobileSensitivity * -1;
            camPitchAmount = Input.GetTouch(0).deltaPosition.y / MobileSensitivity * MouseSensitivityY * (invertLookY ? -1 : 1) * -1;
            
            HandleOrbit(yaw, camPitchAmount);
        }

        LimitOrbit();
    }


    void MobileZoom()
    {
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 point1 = Input.GetTouch(0).position;
            Vector2 point2 = Input.GetTouch(1).position;

            //Calculate the difference between the positions
            float difference = Vector2.Distance(point1, point2);

            if(prevDistance == 0)
            {
                prevDistance = difference;
                return;
            }else
            {
                float zoomAmount = difference - prevDistance;
                zoomAmount = zoomAmount / MobileSensitivityScroll;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + zoomAmount);
                LimitZoom();
            }

            prevDistance = difference;
        }else
        {
            prevDistance = 0;
        }
    }

    void HandleOrbit(float yaw, float pitchAmount)
    {

        pitchAmount = Mathf.Clamp(pitchAmount, -10, 80);

        camYaw.Rotate(0, yaw * MouseSensitivityX, 0);
        camPitch.Rotate(pitchAmount, 0, 0);
    }

    void LimitOrbit()
    {
        //This limits the pitch to a minimum preset angel
        if (camPitch.rotation.eulerAngles.x < 15 || camPitch.rotation.eulerAngles.x > 300)
        {
            camPitch.eulerAngles = new Vector3(1, camPitch.rotation.eulerAngles.y, camPitch.rotation.eulerAngles.z);
        }

        //This limits the pitch to a preset angle
        if (camPitch.rotation.eulerAngles.x > 75)
        {
            camPitch.eulerAngles = new Vector3(75, camPitch.rotation.eulerAngles.y, camPitch.rotation.eulerAngles.z);
        }
        else if(camPitch.rotation.eulerAngles.x < 15)
        {
            camPitch.eulerAngles = new Vector3(15, camPitch.rotation.eulerAngles.y, camPitch.rotation.eulerAngles.z);
        }

        //This will prevent the camera from breaking and going past the max angle pitch angle
        if (camPitch.rotation.eulerAngles.z != 0)
        {
            camPitch.eulerAngles = new Vector3(camPitch.rotation.eulerAngles.x, camPitch.rotation.eulerAngles.y, 0);
        }
    }

    /// <summary>
    /// In this MoveToPosition function we take in a new yaw, pitch, and zoom value. We then set our desired yaw, pitch, and zoom floats. We also set
    /// allowCameraUpdate to true, this will allow the UpdateCamera function to run.
    /// </summary>
    /// <param name="yaw">This value (float) is the desired yaw position.</param>
    /// <param name="pitch">This value (float) is the desired pitch position.</param>
    /// <param name="zoom">This value (float) is the desired zoom position.</param>
    public void MoveToPosition(float yaw, float pitch,float zoom)
    {
        desiredYaw = yaw;
        desiredPitch = pitch;
        desiredZoom = zoom * -1;
    }

    /// <summary>
    /// This function will lerp the camera into the correct position.
    /// </summary>
    void UpdateCamera()
    {
        camYaw.rotation = Quaternion.Slerp(Quaternion.Euler(0, camYaw.rotation.eulerAngles.y, 0), Quaternion.Euler(0, desiredYaw, 0), Time.deltaTime * lerpSpeed);
        camPitch.rotation = Quaternion.Slerp(Quaternion.Euler(camPitch.rotation.eulerAngles.x, 0, 0), Quaternion.Euler(desiredPitch, 0, 0), Time.deltaTime * lerpSpeed);
        camPitch.localEulerAngles = new Vector3(camPitch.localEulerAngles.x, 0, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, desiredZoom), Time.deltaTime * lerpSpeed);
    }
}
