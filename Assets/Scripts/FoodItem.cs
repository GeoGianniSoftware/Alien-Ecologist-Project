using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FoodItem : MonoBehaviour
{
    public IFoodSource source;
    public CreatureController creatureRef;
    public FoodState currentState;
    float timeInitiated;
    float timeToSpoil;

    

    public int foodAmount;

    private void Start() {
        if(source != null) {
            Initalize(source);
        }
    }

    public void Initalize(IFoodSource source) {
        if (source == null) {
            Debug.Log(name + " source is null.");
            Destroy(gameObject);
        }
        else {

            source = Instantiate(source);
            
        }
        foodAmount = source.foodAmount;
        timeInitiated = Time.deltaTime;
        timeToSpoil = timeInitiated + source.timeTillSpoiled;
        CreatureSpawns.addFood(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(foodAmount <= 0 || transform.position.y < -10) {
            CreatureSpawns.removeFood(this);

            Destroy(gameObject);
        }

        if(Time.time > timeToSpoil) {
            currentState = FoodState.SPOILED;
        }
        else if(currentState != FoodState.ALIVE){
            currentState = FoodState.EDIBLE;
        }
        else if (currentState == FoodState.ALIVE) {
            source.alive = true;
        }

    }

    public int Consumed(int amt) {
        foodAmount -= amt;
        return amt;
        
    }
}
