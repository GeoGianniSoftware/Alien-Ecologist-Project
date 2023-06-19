using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class OrganismItem : Item
{
    public Object sampleOf;
    public Color sampleColor;
    public float timeAquired;

    private void Awake() {

        TYPE = ItemType.ORGANISM;
        PREFAB = Resources.Load("objects/VialFull") as GameObject;
    }

    public OrganismItem(int id, string name, Color color, Object source, float time, Sprite s) {
        ID = id;
        NAME = name;
        TYPE = ItemType.ORGANISM;
        sampleColor = color;
        sampleOf = source;
        timeAquired = time;
    }
}
