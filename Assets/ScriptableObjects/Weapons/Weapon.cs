using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon/Basic Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    [ReadOnly]
    public Weapon identity;
    public bool initialized = false;
    public WeaponType weaponType;
    public bool randomDamage;
    public int damageMin, damageMax;
    public float range;
    public float attackSpeed;
    [ReadOnly]
    public GameObject owner;
    public GameObject prefab;


    public bool isPlayerWeapon() {
        
        if (owner != null && owner.GetComponent<PlayerController>()) {
            return true;
        }
        return false;
        
    }

    public static Weapon CreateInstance(Weapon template) {
        Weapon instance = (Weapon)ScriptableObject.Instantiate(template);

        //Unity will automatically call the instance's OnEnable during Instantiate

        instance.identity = template;

        return instance;
    }

    private void OnValidate() {
        Reset();
    }

    public virtual void Reset() {
        initialized = false;
    }

    public virtual void Initialize(GameObject setOwner) {
        owner = setOwner;
       
        initialized = true;
    }

    public int getDamage() {
        if (randomDamage) {
            int rand = Random.Range(damageMin, damageMax);
            return rand;
        }
        else {
            return damageMin;
        }
        
    }

    public virtual void Attack() {

    }

    public virtual void Attack(int attackBuff) {

    }

    public override bool Equals(System.Object obj) {
        if (obj == null)
            return false;
        Weapon c = obj as Weapon;
        if ((System.Object)c == null)
            return false;
        return identity == c.identity;
    }
    public bool Equals(Weapon c) {
        if ((object)c == null)
            return false;
        return identity == c.identity;
    }
}

public enum WeaponType
{
    MELEE,
    RANGED
}
