using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHider : MonoBehaviour {

    public GameObject mapObj;
    public float desiredDistance = 8;
    public float maxDistance = 100;
    public bool doShow = true;
    public bool doScale = false;
	
	// Update is called once per frame
	void Update () {
        dissapearCheck();	
	}

    private void dissapearCheck()
    {
        foreach(Transform child in mapObj.transform)
        {
            float distance = Vector3.Distance(child.position, transform.position);

            if(distance < desiredDistance)
            {
                if (!doShow)//Are we showing or hiding the object at this desired distance, if false we are showing objects
                {
                    if (!child.gameObject.activeInHierarchy)//Childs not active, activate it
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                else//Childs is within desired distance but we want to hide it
                {
                    if (child.gameObject.activeInHierarchy && !doScale)//Childs active and we are not scaling, let's hide it
                    {
                        child.gameObject.SetActive(false);
                    }
                    else if (child.gameObject.activeInHierarchy && doScale)//Childs active and we are scaling, lets hide it
                    {
                        child.GetComponent<Scaler>().ToggleScale(false);
                    }
                }
            }
            else
            {
                if (!doShow)//Are we showing or hiding the child at this desired distance, if false we are hiding objects
                {
                    if (child.gameObject.activeInHierarchy)//Childs active, hide it
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (distance < maxDistance)//Are we past the max distance
                    {
                        if (!child.gameObject.activeInHierarchy && !doScale)//Childs not active and we are not scaling and is within max distance, let's show it
                        {
                            child.gameObject.SetActive(true);
                        }
                        else if (!child.gameObject.activeInHierarchy && doScale)//Childs not active and we are scaling and is within max distance, let's show it
                        {
                            child.gameObject.SetActive(true);
                            child.GetComponent<Scaler>().ToggleScale(true);
                        }
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
