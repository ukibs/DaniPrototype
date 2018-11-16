using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum WordStates { AVAILABLE, BLOCK, COMPLETE }

public class PanelWord : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Text text;
    private WordStates active = WordStates.AVAILABLE;
    public WordInfo info;

    private Type2 level;

    private float timeInScreen;
    private float timeSelected;
    private bool select = false;

    private GameManager gameManager;

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
        gameManager = GameManager.instance;
        level = FindObjectOfType<Type2>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeInScreen += Time.deltaTime;

		if(select)
        {
            timeSelected += Time.deltaTime;
        }
	}
    
    public void OnSelect(BaseEventData eventData)
    {
        if(active == WordStates.AVAILABLE)
        {
            level.selectedButton = this;
            text.color = Color.cyan;
            select = true;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = Color.black;
        select = false;
    }

    public float NewWord()
    {
        Debug.Log("Time: " + timeSelected);
        gameManager.TimeRespond = timeSelected;
        float point = Mathf.Max(3 - timeSelected, 0.1f) * info.difficulty;
        point += timeInScreen * (-info.difficulty * 2 / 100);

        timeInScreen = 0;
        timeSelected = 0;
        return point;
    }
}
