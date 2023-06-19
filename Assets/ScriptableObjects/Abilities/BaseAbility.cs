using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BaseAbility",menuName = "Buffs and Abilities/Ability", order =0)]
public class BaseAbility : ScriptableObject
{

    [ReadOnly]
    public bool initialized = false;
    [System.NonSerialized]
    public PlayerController PC;
    public float amount;
    public float cooldownTime;
    public Buff playerBuff;
    // Start is called before the first frame update
    public virtual void Initialize(PlayerController pc)
    {
        PC = pc;
        playerBuff.Initialize(amount, cooldownTime, this);
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Cast() {
        if(initialized && playerBuff != null) {
            PC.inventory.AddBuff(playerBuff);
        }
    }
}
