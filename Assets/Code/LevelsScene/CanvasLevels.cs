using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLevels : Singleton<CanvasLevels>
{
    public GameObject enterLevelPanel;
    public Text startText;
    public Text coins;
    public GameObject cart;

    private void Start()
    {
        coins.text = GameManager.instance.coins+"";
    }
}
