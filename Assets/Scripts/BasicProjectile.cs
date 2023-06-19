using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public bool live = false;
    public Projectile source;
    public Rigidbody RB;
    Collider COL;
    float hitDelay = .05f;
    float delay;
    public bool faceVelocity;

    bool playerProjectile = false;
    float forceOffset = 1;
    float life;
    private void Awake() {
        COL = GetComponent<Collider>();
        RB = GetComponent<Rigidbody>();
        COL.enabled = false;
        
    }

    public void setForceOffset(float set) {
        forceOffset = set;
    }
    public void Initialize(Projectile s, RangedWeapon w)
    {
        source = Instantiate(s);
        source.source = w;
        faceVelocity = s.faceVelocity;
        life = source.lifeTime;

        if (w.isPlayerWeapon()) {
            playerProjectile = true;
            this.gameObject.layer = 9;

            Physics.IgnoreCollision(COL, FindObjectOfType<PlayerController>().playerCollider);

        }
        if (source != null) {
            RB.AddForce(transform.forward * w.getSpeed()*forceOffset, ForceMode.VelocityChange);
        }
        live = true;
        COL.enabled = true;
    }


    void SpawnObjectOnHit(Transform t) {
        Transform onHitParent = null;
        if (source.parentObjectToHit)
            onHitParent = t.transform;

        if (t.tag == "Terrain")
            return;

        if ((source.onlyOneOnHitSpawn && t.Find(source.onHitSpawnName) != null)) {
            t.SendMessage("Die", 5, SendMessageOptions.DontRequireReceiver);
           
                Destroy(t.Find(source.onHitSpawnName).gameObject);
        }
           

        GameObject onhit = Instantiate(source.onHitPrefab, onHitParent);
        onhit.name = source.onHitSpawnName;
        onhit.transform.position = transform.position;
        onhit.transform.rotation = transform.rotation;
        
    }

    void CollisionRoutine(Collider col) {
        
        if (col != null && live && delay <= 0) {
            if (col.GetComponent<PlayerController>() && playerProjectile)
                return;

            source.glideFactor = 0;
            DealDamage(col.gameObject);
            if (source.createObjectOnHit) {
                SpawnObjectOnHit(col.transform);
            }
            source.hitCount--;
            if (source.hitCount <= 0) {
                if (source.destroyOnHit) {
                    foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>()) {
                        if (p.IsAlive())
                            p.transform.SetParent(null);
                    }

                    Destroy(gameObject);
                }

                live = false;
            }
            delay = hitDelay;
        }
    }

    private void OnCollisionEnter(Collision collision) {

        CollisionRoutine(collision.collider);
    }

    bool friendlyFire(GameObject obj) {
        return !(playerProjectile && obj.transform.GetComponent<PlayerController>());
    }

    private void Update() {
        life -= Time.deltaTime;
        if(life<= 0) {
            Destroy(gameObject);
        }
        if (faceVelocity)
            transform.GetChild(0).LookAt(transform.position+RB.velocity);
        delay -= Time.deltaTime;
        
    }
    private void FixedUpdate() {
        if (RB != null) {
            RB.AddForce(Vector3.down * (Physics.gravity.y * source.glideFactor));
        }

        Vector3 nextPos = transform.position + RB.velocity * Time.deltaTime;

        Ray ray = new Ray(transform.position, nextPos);
        RaycastHit hit;


        if (Physics.Linecast(transform.position, nextPos, out hit)) {
            if (hit.collider != null) {
                print("Ray");

                CollisionRoutine(hit.collider);
            }
        }
    }


    void DealDamage(GameObject target) {
        Vector3 hitPoint = target.transform.position - transform.position;

        target.SendMessage("TakeDamage", source.source.getDamage(), SendMessageOptions.DontRequireReceiver);
    }
}
