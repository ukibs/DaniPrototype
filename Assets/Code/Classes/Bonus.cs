using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/List", order = 1)]
public class Bonus : ScriptableObject {
    public Sprite image;
    public string name;
    public int price;
    public int amount;
    public bool active;
}
