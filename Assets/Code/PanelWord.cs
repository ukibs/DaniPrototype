using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum WordStates { AVAILABLE, BLOCK, COMPLETE }

public class PanelWord : MonoBehaviour, ISelectHandler, IDeselectHandler {
    public Text text;
    private WordStates active = WordStates.AVAILABLE;
    public WordInfo info;

    private Type2 level;

    public WordStates Active
    {
        get { return active; }
        set { active = value;
            if (active == WordStates.BLOCK) text.color = Color.red;
            else if (active == WordStates.COMPLETE) text.color = Color.green;
            else text.color = Color.black;
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
        if(active == WordStates.AVAILABLE)
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
