using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsGenerator : MonoBehaviour {

    public GameObject prefabLevel;

	// Use this for initialization
	void Start () {
        for(int i = 0; i < 10; ++i)
        {
            Instantiate(prefabLevel, new Vector3(0, i * 10, 0), prefabLevel.transform.rotation);
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
