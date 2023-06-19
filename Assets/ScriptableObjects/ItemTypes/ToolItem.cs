using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "ToolItem", menuName = "Items/Tools", order = 1)]
public class ToolItem : Item
{
    public Weapon weaponRef;

    private void Awake() {

        TYPE = ItemType.TOOL;
        PREFAB = weaponRef.prefab;
    }

    public ToolItem(int id, string name, Weapon reff) {
        ID = id;
        NAME = name;
        TYPE = ItemType.TOOL;
        weaponRef = reff;
    }
}
