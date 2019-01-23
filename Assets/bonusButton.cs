using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bonusButton : MonoBehaviour
{
    public Image deactive;
    bool active;
    GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.instance;
        active = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeState(int i)
    {
        if(active)
        {
            active = false;
            deactive.enabled = true;
            gameManager.bonusList[i].active = false;
        }
        else
        {
            if (gameManager.bonusList[i].amount > 0)
            {
                active = true;
                deactive.enabled = false;
                gameManager.bonusList[i].active = true;
            }
        }
    }
}
