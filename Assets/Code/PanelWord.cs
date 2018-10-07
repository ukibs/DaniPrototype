using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelWord : MonoBehaviour, ISelectHandler, IDeselectHandler {
    public Text text;
    private bool active = true;
    public WordInfo info;

    private Type2 level;

    public bool Active
    {
        get { return active; }
        set { active = value;
            if (!active) text.color = Color.red;
        }
    }

    // Use this for initialization
    void Start () {
        level = FindObjectOfType<Type2>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void OnSelect(BaseEventData eventData)
    {
        if(active)
        {
            level.selectedButton = this;
            text.color = Color.cyan;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = Color.black;
    }
}
