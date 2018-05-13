using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour {

    public float scaleSpeed = 5;
    public float maxSize = 5;
    private bool desiredState;
    private bool doScaling = false;

    // Update is called once per frame
    void Update()
    {
        if (doScaling)
        {
            if (!desiredState)
            {
                Vector3 desiredSize = new Vector3(transform.localScale.x - Time.deltaTime * scaleSpeed, transform.localScale.y - Time.deltaTime * scaleSpeed, transform.localScale.z - Time.deltaTime * scaleSpeed);
                transform.localScale = desiredSize;

                if (transform.localScale.x <= 0.1)
                {
                    gameObject.SetActive(false);
                    doScaling = false;
                }
            }
            else if (desiredState)
            {
                Vector3 desiredSize = new Vector3(transform.localScale.x + Time.deltaTime * scaleSpeed, transform.localScale.y + Time.deltaTime * scaleSpeed, transform.localScale.z + Time.deltaTime * scaleSpeed);
                transform.localScale = desiredSize;

                if (transform.localScale.x >= maxSize)
                {
                    transform.localScale = new Vector3(maxSize, maxSize, maxSize);
                    gameObject.SetActive(true);
                    doScaling = false;
                }
            }
        }
    }

    public void ToggleScale(bool setType)
    {
        desiredState = setType;
        doScaling = true;
    }
}
