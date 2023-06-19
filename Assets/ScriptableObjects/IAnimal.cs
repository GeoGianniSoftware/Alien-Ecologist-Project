using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum AnimalState
{
    STOPPED,
    IDLE,
    WANDER,
    FORAGING,
    HUNTING,
    EATING,
    DEAD
}

public enum Diet
{
    CARNIVORE,
    VEGETARIAN,
    OMNIVORE,
    PREPARED
}
public enum Gender
{
    MALE,
    FEMALE
}

[CreateAssetMenu(fileName = "prefabAnimal", menuName = "Scriptable/Behaviours/Animal ", order = 0)]
[System.Serializable]
public class IAnimal : ScriptableObject
{
    public IAnimal source;
    public string animalName;
    public int maxHealth = 1;

    

    public int damage;
    public float attackRange;
    public float forageRange;
    [Range(0f,1f)]
    public float attackSuccessChance;

    public float attackCooldown;

    public float speed;
    public float turningSpeed = 120;
    public float wanderRange;
    public float obstacleRadius = 1f;
    public float obstacleHeight = 1f;
    public float heightOffset = 0f;

    public Gender animalGender;
    public AnimalState currentState;
    public Diet animalDiet;
    public IFoodSource creatureFoodType;
    [System.NonSerialized]
    public FoodItem creatureIsFood;
    public List<IMutation> mutations = new List<IMutation>();

    public bool onlyEatsAlive;
    public bool sexualDimorphism;
    public Color creatureColor;
    public List<int> areaMask;


    public float maxHunger;
    public float hungerPoint;
    public float hungerRate;
    public float foodValue;
    public string prefabSlug;
    [System.NonSerialized]
    public GameObject host;

    // Tick Call once per frame
    

    public void Initialize(GameObject _host, bool randomize)
    {
        source = this;

        
            MakeUnique(randomize);

        if (creatureFoodType == null) {
            
            Debug.Log(name + "Initalizing failed!");
            return;
        }
        else {
            host = _host;
            creatureFoodType = Instantiate(creatureFoodType);
            FoodItem creatureIsFood = host.AddComponent<FoodItem>();
            creatureIsFood.Initalize(creatureFoodType);
            creatureIsFood.source = creatureFoodType;
            creatureIsFood.currentState = FoodState.ALIVE;



            if (creatureIsFood != null) {

            }
        }



    }

    public void MakeUnique(bool alter) {
        if (alter) {
            setGender(Random.Range(0, 2));
            Vector3 randomColor = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            //Random Genes. TODO REWORK THIS WHOLE SYSTEM
            speed *= Random.Range(.85f, 1.25f);
            damage *= (int)(Random.Range(.85f, 1.25f));
            attackCooldown *= Random.Range(.85f, 1.25f);
            foodValue *= Random.Range(.85f, 1.25f);

            creatureColor = new Color(randomColor.x, randomColor.y, randomColor.z, 1f);
        }
        

        


       



        string temp = name.Replace("(Clone)", "");
        animalName = temp;


        temp = temp.ToLower();
        if (sexualDimorphism) {

            prefabSlug = temp + animalGender;
        }
        else {
            prefabSlug = temp;
        }
    }

    public void setGender(int gender) {
        //0 == female, 1 == male
        switch (gender) {
            case 0:
                animalGender = Gender.FEMALE;
                break;
            case 1:
                animalGender = Gender.MALE;
                break;
            default:
                return;
        }
    }
    

    public Vector3 getWanderDif() {
        float getRandomX = Random.Range(-wanderRange, wanderRange);
        float getRandomZ = Random.Range(-wanderRange, wanderRange);

        Vector3 vectorToReturn;

        vectorToReturn = new Vector3(getRandomX, 0, getRandomZ);
        return vectorToReturn;
    }


    HashSet<FoodItem> cachedFoods = new HashSet<FoodItem>();
    public FoodItem getNearestFoodSource(Transform callingTransform) {
        Collider[] nearby = Physics.OverlapSphere(callingTransform.position, forageRange, 1<<8, QueryTriggerInteraction.Collide);



        foreach (Collider c in nearby) {
            if (c.GetComponent<FoodItem>()) {
                FoodItem temp = c.GetComponent<FoodItem>();

                
                cachedFoods.Add(temp);
            }
        }

        List<FoodItem> edibleFoods = new List<FoodItem>();
        foreach(FoodItem item in cachedFoods) {
            if (item == null)
                continue;
            bool canAcessItem = false;
            if(item.creatureRef != null) {
                foreach (int i in areaMask) {
                    if (item.creatureRef.animalAI.areaMask.Contains(i))
                        canAcessItem = true;
                }
            }
            else {
                NavMeshPath p = new NavMeshPath();
                NavMesh.CalculatePath(host.transform.position, item.transform.position, host.GetComponent<NavMeshAgent>().areaMask, p);
                if(p.status == NavMeshPathStatus.PathComplete) {
                    canAcessItem = true;
                }
                
            }
            


            if (item.currentState == FoodState.SPOILED || callingTransform.GetComponent<FoodItem>() == item || !canAcessItem)
                continue;

            switch (animalDiet) {
                case (Diet.CARNIVORE):
                    if (item.source.foodType == FoodType.CARNIVOROUS) {
                        if (onlyEatsAlive && !item.source.alive) {
                            
                            continue;
                        }
                        edibleFoods.Add(item);
                    }
                    break;
                case (Diet.VEGETARIAN):
                    if (item.source.foodType == FoodType.VEGETARIAN) {
                        edibleFoods.Add(item);
                    }
                    break;
                case (Diet.PREPARED):
                    if (item.source.foodType == FoodType.PREPARED) {
                        edibleFoods.Add(item);
                    }
                    break;
                case (Diet.OMNIVORE):
                    if (item.source.foodType == FoodType.VEGETARIAN || item.source.foodType == FoodType.CARNIVOROUS) {
                        if (onlyEatsAlive && !item.source.alive)
                            continue;
                        edibleFoods.Add(item);
                    }
                    break;

            }

        }

        float min = float.MaxValue;
        FoodItem closest = null;
        if(edibleFoods.Count == 0) {
            return null;
        }

        foreach(FoodItem food in edibleFoods) {
            float effortIndex = CalculateEffort(food, callingTransform);
            
            if (effortIndex < 0)
                continue;
            else if (effortIndex < min) {
                closest = food;
                min = effortIndex;
            }
        }
        return closest;
        

    }

    float CalculateEffort(FoodItem food, Transform callingTransform) {
        float effortIndex = 0;

        Vector3 dir = callingTransform.position - food.transform.position;
        float length = dir.sqrMagnitude;

        if (food.source.alive) {
            if (food.creatureRef != null && food.creatureRef.animalAI.animalName == animalName) {

                return 100;
            }
            int maxHealth = food.creatureRef.animalAI.maxHealth;
            if (maxHealth <= 0) {
                maxHealth = 1;
            }

            effortIndex += (food.creatureRef.animalAI.maxHealth + ((food.creatureRef.currentHealth / maxHealth) * 10)+food.creatureRef.animalAI.damage) / 10;

        }

        effortIndex += length / 5;

        return effortIndex;
        
    }
  

    public int Consume(int damageAmt) {
        host.SendMessage("TakeDamage", damageAmt, SendMessageOptions.DontRequireReceiver);
        return creatureIsFood.Consumed(damageAmt);
    }

    public int ConsumeTarget(FoodItem target, int damageAmt) {
        target.SendMessage("TakeDamage", damageAmt, SendMessageOptions.DontRequireReceiver);
        return target.Consumed(damageAmt);
    }

    public void Die(Transform host) {
        FoodItem temp = Instantiate(creatureIsFood.source.prefab, host.transform.position + Vector3.up, Quaternion.identity).GetComponent<FoodItem>();
        temp.source.foodAmount = maxHealth;

        Destroy(host.gameObject);
    }
}
