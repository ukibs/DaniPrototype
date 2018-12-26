using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Type2 : Singleton<GUI_Type2> {

    public GridLayoutGroup panel;
    public Sprite[] buttonColors;

    // Use this for initialization
    void Start () {
        float width = panel.GetComponent<RectTransform>().rect.width;
        float height = panel.GetComponent<RectTransform>().rect.height;

        panel.cellSize = new Vector2((width - width * 0.1f) / 2, (height - height * 0.3f) / 4);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
