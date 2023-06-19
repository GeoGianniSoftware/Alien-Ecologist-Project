using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RangedWeapon", menuName = "Item/Weapon/Ranged Weapon", order = 0)]
public class RangedWeapon : Weapon
{
    
    public Projectile projectile;
    public bool randomSpeed;
    public float speedMin, speedMax;
    public float shotCount;
    public float shotDelay;
    [ReadOnly]
    public bool targetedFire;
    [System.NonSerialized]
    public float fireOffset;
    [System.NonSerialized]
    public Transform projectileSpawn;


    public override void Reset() {
        base.Reset();
        weaponType = WeaponType.RANGED;
    }

    public void setProjectileSpawn (Transform projSpawn){
        projectileSpawn = projSpawn;
    }

    public void setFireOffset(float offset) {
        fireOffset = offset;
    }

    public void SpawnProjectile() {
        BasicProjectile proj = Instantiate(projectile.projectilePrefab, projectileSpawn.position, projectileSpawn.rotation).GetComponent<BasicProjectile>();

        proj.Initialize(projectile, Instantiate(this));
    }

    

    public void SpawnProjectile(int attackBuff) {
        if (projectileSpawn == null)
            return;
        BasicProjectile proj = Instantiate(projectile.projectilePrefab, projectileSpawn.position, projectileSpawn.rotation).GetComponent<BasicProjectile>();
        if (proj == null)
            return;
        if (targetedFire)
            proj.setForceOffset(fireOffset);
        proj.Initialize(projectile, Instantiate(this));
        if (isPlayerWeapon()) {
            Physics.IgnoreCollision(proj.GetComponent<Collider>(), FindObjectOfType<PlayerController>().playerCollider);
        }
        
        proj.source.source.damageMin += attackBuff;
        proj.source.source.damageMax += attackBuff;
    }

    public float getSpeed() {
        float result = speedMin;
        if (randomSpeed) {
            result = Random.Range(speedMin, speedMax);
        }

        return result;
    }

    public override void Attack() {

        Attack(0);
    }

    public override void Attack(int attackBuff) {
        base.Attack();
        if (shotCount > 1) {

            FindObjectOfType<EventManager>().StartFireCoroutine(this, attackBuff);
        }
        else {
            SpawnProjectile(attackBuff);
        }
    }
    public IEnumerator fireRoutine(int attackBuff) {
        for (int i = 0; i < shotCount; i++) {
            SpawnProjectile(attackBuff);
            yield return new WaitForSeconds(shotDelay);
        }
        yield return null;
    }

}
