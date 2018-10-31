using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLevelSelectorMovement : MonoBehaviour {

    private float previousMouseX;
    private float previousMouseY;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //
        if (Input.GetMouseButtonDown(0))
        {
            previousMouseX = Input.mousePosition.x;
            previousMouseY = Input.mousePosition.y;
        }
        //
        if (Input.GetMouseButton(0))
        {
            Vector3 newPosition = transform.position;

            newPosition.x += (previousMouseX - Input.mousePosition.x) * Time.deltaTime;
            newPosition.y += (previousMouseY - Input.mousePosition.y) * Time.deltaTime;

            transform.position = newPosition;

            previousMouseX = Input.mousePosition.x;
            previousMouseY = Input.mousePosition.y;
        }
	}

    
}
