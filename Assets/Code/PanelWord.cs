using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum WordStates { AVAILABLE, BLOCK, COMPLETE, SELECTED }

public class PanelWord : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Text text;
    public WordInfo info;

    private Type2 level;
    private WordStates active = WordStates.AVAILABLE;
    private float timeInScreen;
    private float timeSelected;
    private Image image;

    private GameManager gameManager;

    public WordStates Active
    {
        get { return active; }
        set { active = value;
            if (active == WordStates.BLOCK) image.sprite = GUI_Type2.Instance.buttonColors[1];
            else if (active == WordStates.COMPLETE) image.sprite = GUI_Type2.Instance.buttonColors[2];
            else image.sprite = GUI_Type2.Instance.buttonColors[0];
        }
    }

    // Use this for initialization
    void Awake () {
        gameManager = GameManager.instance;
        level = FindObjectOfType<Type2>();
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeInScreen += Time.deltaTime;

		if(active == WordStates.SELECTED)
        {
            timeSelected += Time.deltaTime;
        }
	}
    
    public void OnSelect(BaseEventData eventData)
    {
        if(active == WordStates.AVAILABLE)
        {
            level.selectedButton = this;
            image.sprite = GUI_Type2.Instance.buttonColors[3];
            active = WordStates.SELECTED;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(active == WordStates.SELECTED)
            Active = WordStates.AVAILABLE;
    }

    public float NewWord()
    {
        gameManager.TimeRespond(timeSelected);
        float point = Mathf.Max(3 - timeSelected, 0.1f) * info.difficulty;
        point += timeInScreen * (info.difficulty * 2 / 100);
        timeInScreen = 0;
        timeSelected = 0;
        return point;
    }
}
