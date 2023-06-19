using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    BASIC,
    TOOL,
    SAMPLE,
    ORGANISM
}
[System.Serializable]

[CreateAssetMenu(fileName = "Item", menuName = "Items/Basic", order = 0)]
public class Item: ScriptableObject
{
    public int ID;
    public string NAME;
    public ItemType TYPE;
    public GameObject PREFAB;
    public long itemHash = -1;
    public Sprite SPRITE;

    public Item(int id, string name, ItemType t, Sprite s) {
        ID = id;
        NAME = name;
        TYPE = t;
        SPRITE = s;
        itemHash = -1;
}

    public Item(Item template) {
        ID = template.ID;
        NAME = template.NAME;
        TYPE = template.TYPE;
        SPRITE = template.SPRITE;
        itemHash = template.itemHash;
    }

    public void SetHash(long hash) {
        itemHash = hash;
    }

    public Item() {
        ID = -1;

    }

    public static Texture2D getItemSprite(string slug) {
        
        Texture2D tex = Resources.Load("Items/Sprites/" + slug) as Texture2D;
        return tex;
    }

    public static SampleItem CreateSample(int id, string s, Color c, IAnimal reff, float t, Sprite sprite) {
        SampleItem temp = SampleItem.CreateInstance<SampleItem>();
        temp.ID = id;
        temp.NAME = s;
        temp.name = s;
        temp.sampleColor = c;
        temp.sampleOf = reff;
        temp.timeAquired = t;
        temp.SPRITE = sprite;

        return temp;
    }

    public static OrganismItem CreateOrganism(int id, string s, Color c, IAnimal reff, float t, Sprite sprite) {
        OrganismItem temp = OrganismItem.CreateInstance<OrganismItem>();
        temp.ID = id;
        temp.NAME = s;
        temp.name = s;
        temp.sampleColor = c;
        temp.sampleOf = reff;
        temp.timeAquired = t;
        temp.SPRITE = sprite;

        return temp;
    }
    public override bool Equals(System.Object obj) {
        if (obj == null)
            return false;
        Item c = obj as Item;
        if ((System.Object)c == null)
            return false;
        return ID == c.ID;
    }
    public bool Equals(Item c) {
        if ((object)c == null)
            return false;
        return ID == c.ID;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public override string ToString() {
        return NAME;
    }
}
