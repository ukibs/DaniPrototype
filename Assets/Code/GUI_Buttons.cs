using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Buttons : MonoBehaviour {

    public GridLayoutGroup[] buttonsLength3;
    public GridLayoutGroup[] buttonsLength2;

    // Use this for initialization
    void Start () {

        for (int i = 0; i < buttonsLength3.Length; i++)
        {
            float width = buttonsLength3[i].GetComponent<RectTransform>().rect.width;
            float height = buttonsLength3[i].GetComponent<RectTransform>().rect.height;

            buttonsLength3[i].cellSize = new Vector2((width - width * 0.5f) / 3, (height - height * 0.3f));
        }

        for (int i = 0; i < buttonsLength2.Length; i++)
        {
            float width = buttonsLength2[i].GetComponent<RectTransform>().rect.width;
            float height = buttonsLength2[i].GetComponent<RectTransform>().rect.height;

            buttonsLength2[i].cellSize = new Vector2((width - width * 0.4f) / 2, (height - height * 0.3f));
        }

	}
}
