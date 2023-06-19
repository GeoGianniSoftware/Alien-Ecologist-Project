using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BaseBuff", menuName = "Buffs and Abilities/Buff", order = 1)]
public class Buff : ScriptableObject
{
    [ReadOnly]
    public BaseAbility source;

    [ReadOnly]
    public float lifetime;

    [ReadOnly]
    public float amt;
    public BuffType buffType;

    public void Initialize(float a, float l, BaseAbility s) {
        amt = a;
        lifetime = l;
        source = Instantiate(s);
    }

    public void Tick(float deltaTime) {
        lifetime -= deltaTime;
    }
}
public enum BuffType
{
    JUMP,
    SPEED,
    HEALTH,
    DAMAGE
}
