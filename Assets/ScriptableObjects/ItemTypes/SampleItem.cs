using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "SampleItem", menuName = "Items/Sample", order = 1)]
public class SampleItem : Item
{
    public Object sampleOf;
    public Color sampleColor;
    public float timeAquired;

    private void Awake() {

        TYPE = ItemType.SAMPLE;
        PREFAB = Resources.Load("objects/VialFull") as GameObject;
    }

    public SampleItem(int id, string name, Color color, Object source, float time, Sprite s) {
        ID = id;
        NAME = name;
        TYPE = ItemType.SAMPLE;
        sampleColor = color;
        sampleOf = source;
        timeAquired = time;
    }
}
