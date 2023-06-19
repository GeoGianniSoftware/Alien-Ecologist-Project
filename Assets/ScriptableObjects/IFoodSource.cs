using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FoodState
{
    ALIVE,
    FROZEN,
    SPOILED,
    EDIBLE
}

public enum FoodType
{
    VEGETARIAN,
    CARNIVOROUS,
    PREPARED
}

[CreateAssetMenu(fileName = "prefabFoodSource", menuName = "Scriptable/Food Source", order = 0)]
[System.Serializable]
public class IFoodSource : ScriptableObject
{
    public int foodAmount;
    public bool alive = false;
    public float timeTillSpoiled;
    public FoodType foodType;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
