using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Projectile", menuName ="Projectiles/Basic", order =1)]
public class Projectile : ScriptableObject
{
    [ReadOnly]
    public Weapon source;
    public bool destroyOnHit;
    public int hitCount = 1;
    public GameObject projectilePrefab;
    public float glideFactor;
    public float scanDistance = .5f;
    public float lifeTime = 1f;
    public bool faceVelocity;

    [Header("SpawnObjectOnHit")]
    public bool createObjectOnHit;
    public GameObject onHitPrefab;
    public bool parentObjectToHit;
    public bool onlyOneOnHitSpawn;
    public string onHitSpawnName = "**";


}
