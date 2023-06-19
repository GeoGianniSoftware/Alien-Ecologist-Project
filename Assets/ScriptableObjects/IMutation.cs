using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MutationType
{
    //TODO; Concept Mutation Types

    SIZE,
    SPEED,
    STRENGTH
}
public enum MutationRarity
{
    //TODO; Concept Mutation Rarities?
    COMMON,
    UNCOMMON,
    RARE

}

[CreateAssetMenu(fileName = "mutationTemplate", menuName = "Scriptable/Mutation", order = 0)]
[System.Serializable]
public class IMutation : ScriptableObject
{
    public string NAME;
    public float MutationModifier;
    public bool Dormant = false;
    public MutationType TYPE;
    public MutationRarity RARITY;

    public IMutation InitializeMutation(string nam, float mod, bool dormant, MutationType t, MutationRarity rare) {
        IMutation m = ScriptableObject.CreateInstance<IMutation>();
        m.NAME = nam;
        m.MutationModifier = mod;
        m.Dormant = dormant;
        m.TYPE = t;
        m.RARITY = rare;
        return m;
    }

    public override bool Equals(System.Object obj) {
        if (obj == null)
            return false;
        IMutation c = obj as IMutation;
        if ((System.Object)c == null)
            return false;
        return TYPE == c.TYPE;
    }
    public bool Equals(IMutation c) {
        if ((object)c == null)
            return false;
        return TYPE == c.TYPE;
    }

}
