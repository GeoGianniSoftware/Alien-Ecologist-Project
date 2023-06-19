using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class CreatureController : MonoBehaviour
{
    
    public bool isInstantiated = false;
    public AnimalState currentState;
    public int currentHealth;
    public IAnimal animalAI;

    public float currentHunger;
    

    public NavMeshAgent NMA;
    float idleTime = 0f;

    public FoodItem foodTarget;
    public GameObject gfxRef;
    bool randomize = true;

    // Start is called before the first frame update
    void Start()
    {
        //Initalize
        if (animalAI == null) {
            print(name + " Animal AI NUll");
            Destroy(gameObject);
        }
        else {

            currentHealth = animalAI.maxHealth;
            animalAI.Initialize(gameObject, randomize);
            animalAI.creatureIsFood = GetComponent<FoodItem>();
            animalAI.creatureIsFood.creatureRef = this;
        }
        //Instantiate
        if (!isInstantiated) {
            GameObject modelFromResource = Resources.Load("animalModelPrefabs/" + animalAI.prefabSlug) as GameObject;
            GameObject modelPrefab = Instantiate(modelFromResource, transform.GetChild(0).position, Quaternion.identity);
            modelPrefab.transform.localPosition = Vector3.zero;

            //Mesh Stuff

            MeshRenderer modelMesh = null;

            if (modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() != null) {
                SkinnedMeshRenderer tempRend = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
                

                Material tempMat = tempRend.material;
                tempMat.color = animalAI.creatureColor;
                tempRend.sharedMaterial = tempMat;
            }
            else {
                modelMesh = modelPrefab.GetComponent<MeshRenderer>();
                if (modelMesh != null) {
                    Material tempMat = modelMesh.material;
                    tempMat.color = animalAI.creatureColor;
                    modelMesh.sharedMaterial = tempMat;
                }

            }

           



            modelPrefab.transform.SetParent(transform.GetChild(0), false);
            if (modelPrefab.GetComponent<MeshCollider>()) {
                MeshCollider c = modelPrefab.GetComponent<MeshCollider>();
                MeshCollider temp = gameObject.AddComponent<MeshCollider>();
                temp.sharedMesh = c.sharedMesh;
                Destroy(c);
            }else if (modelPrefab.GetComponent<BoxCollider>()) {
                BoxCollider c = modelPrefab.GetComponent<BoxCollider>();
                BoxCollider temp = gameObject.AddComponent<BoxCollider>();
                temp.contactOffset = c.contactOffset;
                temp.size = c.size;
                temp.center = c.center;
                Destroy(c);
            }


            gfxRef = modelPrefab;
            name = animalAI.animalName + " ("+animalAI.animalGender+")";


            //TEMP Mutations!
            foreach (IMutation M in animalAI.mutations) {
                switch (M.TYPE) {
                    case MutationType.SIZE:
                        transform.localScale *= M.MutationModifier;
                        transform.localScale *= Random.Range(.85f, 1.15f);
                        break;
                    case MutationType.SPEED:
                        animalAI.speed *= M.MutationModifier;
                        break;
                    case MutationType.STRENGTH:
                        break;
                }
            }

            

            isInstantiated = true;
            preformanceMode = true;
        }

        
        NMA = GetComponent<NavMeshAgent>();
        //NMA.enabled = true;
        foreach (int i in animalAI.areaMask) {
            NMA.areaMask |= (1 << i);
        }

        NMA.speed = animalAI.speed;
        NMA.angularSpeed = animalAI.turningSpeed;
        NMA.radius = animalAI.obstacleRadius;
        NMA.height = animalAI.obstacleHeight;
        NMA.baseOffset = animalAI.heightOffset;
        currentHunger = animalAI.maxHunger;

        currentHealth = animalAI.maxHealth;


        StartCoroutine(waitForNMA());
        GM = FindObjectOfType<GameMaster>();
    }

    IEnumerator waitForNMA() {
        yield return new WaitForSeconds(.1f);

        //NMA.Warp(transform.position + (Vector3.up * transform.localScale.y*2));
        NMA.enabled = true;
    }

    // Update is called once per frame
    //[System.NonSerialized]
    public bool preformanceMode;
    GameMaster GM;
    void Update()
    {
        if(Time.timeScale == 0 && !NMA.isStopped) {
            NMA.isStopped = true;
            return;
        }
        else if(NMA.isOnNavMesh && NMA.isStopped) {
            NMA.isStopped = false;
        }

        


        if (preformanceMode) {
            Wander();
            Health();
            Idle();
            return;
        }

        if(currentState != animalAI.currentState)
        currentState = animalAI.currentState;


        if (NMA != null && NMA.isActiveAndEnabled) {


            Wander();
            Hunger();
            Eat();
            Health();
            Idle();
        }
        


    }


    void Idle() {
        idleTime -= Time.deltaTime;

        if(idleTime > 0 || Time.timeScale == 0) {
            NMA.speed = 0;
        }
        else {
            NMA.speed = animalAI.speed;
        }

        if(currentState == AnimalState.IDLE) {
            NMA.destination = transform.position;
            
        }

        
        
    }

    List<FoodItem> lastListOfFood = new List<FoodItem>();
    void Hunger() {
        if (currentHunger > 0) {
            currentHunger -= (Time.deltaTime / 60) * animalAI.hungerRate;
        }
        else if (currentHunger > animalAI.maxHunger) {
            currentHunger = animalAI.maxHunger;
        }
        else if (currentHunger <= 0f) {
            NMA.speed = animalAI.speed / 3f;

            NMA.angularSpeed = animalAI.turningSpeed / 3;
        }
        else
            currentHunger = 0;

        if (currentHunger <= animalAI.hungerPoint) {
            if (lastListOfFood.Count == 0 || (lastListOfFood != CreatureSpawns.currentActiveFoods) || foodTarget == null) {

                SearchForFood();

                lastListOfFood = CreatureSpawns.currentActiveFoods;
            }

        }
    }

    void Eat() {
        if(foodTarget != null) {
            
            if (NMA.isActiveAndEnabled) {

                NMA.SetDestination( foodTarget.transform.position);
            }

            //In Range and ready to attack
            if (NMA.isActiveAndEnabled && canInteractWithObject(foodTarget.gameObject) && idleTime <= 0) {
                transform.LookAt(foodTarget.transform);
                AttackSlashConsume();
            }
        }
        
    }

    void Health() {
        if (animalAI.maxHealth <= 0) {

            animalAI.maxHealth = animalAI.source.maxHealth;
            currentHealth = animalAI.maxHealth;
        }

        if (currentHealth <= .5f) {
            NMA.speed = animalAI.speed / 2;

            NMA.angularSpeed = animalAI.turningSpeed/2;
        }
        if (currentHealth <= 0) {
            animalAI.Die(transform);
            print("dead");
            print(animalAI.maxHealth);
            Destroy(GetComponent<NavMeshAgent>());
        }
    }

    bool isFoodNearby() {

        if (animalAI.getNearestFoodSource(transform) != null) {
           
            return true;

        }
        else {
            return false;
        }
    }




    void SearchForFood() {
        

        if (currentHunger <= animalAI.hungerPoint) {
            if (isFoodNearby()) {
                if(animalAI.animalDiet == Diet.CARNIVORE || animalAI.animalDiet == Diet.OMNIVORE) {
                    animalAI.currentState = AnimalState.HUNTING;
                }
                else {
                    animalAI.currentState = AnimalState.FORAGING;
                }


                if (animalAI.currentState == AnimalState.HUNTING || animalAI.currentState == AnimalState.FORAGING) {

                    //Find Nearest FoodSource and set as target
                    //if(foodTarget == null)
                    
                    foodTarget = animalAI.getNearestFoodSource(transform);

                    



                }
            }
            else if (isFoodNearby() == false) {
                
                if (NMA.isActiveAndEnabled) {
                    Wander();
                }
            }
        }
        
        
    }

    void AttackSlashConsume() {
        if (idleTime > 0)
            return;

        if (foodTarget.source.alive) {
            //print("Attacking "+ foodTarget.creatureRef);

            foodTarget.SendMessage("TakeDamage", animalAI.damage, SendMessageOptions.DontRequireReceiver);

            idleTime = animalAI.attackCooldown;
        }
        else {

            //print("Consuming " + foodTarget.source);
            currentHunger += foodTarget.GetComponent<FoodItem>().Consumed((int)(animalAI.damage));

            idleTime = animalAI.attackCooldown;
        }
        

    }

    void Wander() {
        if (currentHunger > animalAI.hungerPoint && (animalAI.currentState != AnimalState.FORAGING || animalAI.currentState != AnimalState.HUNTING)) {
            animalAI.currentState = AnimalState.WANDER;
        }


        if (NMA.isActiveAndEnabled && NMA.remainingDistance <= 1f && animalAI.currentState == AnimalState.WANDER) {

            NMA.SetDestination(transform.position + animalAI.getWanderDif());
            idleTime += Random.Range(.1f, 2f);
        }
    }

    void TakeDamage(int amt) {
        print("Taking Damage");
        currentHealth -= amt;
    }

    bool canInteractWithObject(GameObject targetToInteractWith) {
        Ray ray = new Ray(gfxRef.transform.GetChild(0).position, targetToInteractWith.transform.position - gfxRef.transform.GetChild(0).position);
        Debug.DrawRay(ray.origin, ray.direction * animalAI.attackRange);
        List<RaycastHit> hits = new List<RaycastHit>();
        hits.AddRange(Physics.RaycastAll(ray, animalAI.attackRange));
        foreach(RaycastHit hit in hits) {
            if(hit.transform.gameObject == targetToInteractWith) {

                return true;
            }
        }

        return false;
    }

    float onMeshThreshold = 3;

    public bool IsAgentOnNavMesh(GameObject agentObject) {
        Vector3 agentPosition = agentObject.transform.position;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas)) {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z)) {
                // Lastly, check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }

    public void disableRandom() {
        randomize = false;
    }
}
