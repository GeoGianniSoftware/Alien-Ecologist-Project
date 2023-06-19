using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CreatureSpawns : MonoBehaviour
{
    public List<IAnimal> animalTemplates;

    public List<Transform> animalSpawns;

    public List<CreatureController> livingAnimals = new List<CreatureController>();
    public Text creatureAmounts;

    public static List<FoodItem> currentActiveFoods = new List<FoodItem>();


    //Temp Variables
    public List<IMutation> globalMutations = new List<IMutation>();
    public float globalMutationChance;

    // Start is called before the first frame update
    int index = 0;
    void Awake()
    {
        print("Spawning Creatures");
        InitializeMutations();

        SpawnCreatures(animalTemplates[0], 15, animalSpawns[0].position);

        SpawnCreatures(animalTemplates[1], 20, animalSpawns[1].position);

        SpawnCreatures(animalTemplates[2], 25, animalSpawns[2].position);

        SpawnCreatures(animalTemplates[3], 50, animalSpawns[3].position);

        SpawnCreatures(animalTemplates[4], 20, animalSpawns[4].position);

        SpawnCreatures(animalTemplates[5], 20, animalSpawns[5].position);

    }

    void InitializeMutations() {
        List<IMutation> tempGlobal = new List<IMutation>();
        foreach (IMutation M in globalMutations) {
            tempGlobal.Add(M.InitializeMutation(M.NAME, M.MutationModifier, M.Dormant, M.TYPE, M.RARITY));
        }
        globalMutations = tempGlobal;
    }

    public static void addFood(FoodItem itemToAdd) {
        currentActiveFoods.Add(itemToAdd);
    }

    public static void removeFood(FoodItem itemToRemove) {
        if (currentActiveFoods.Contains(itemToRemove)) {
            currentActiveFoods.Remove(itemToRemove);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateList();


    }

    void UpdateList() {
        List<CreatureController> temp = new List<CreatureController>();
        int[] lines = new int[animalTemplates.Count];
        foreach (CreatureController creature in livingAnimals) {
            if (creature.currentHealth > 0) {
                temp.Add(creature);
            }

            for (int i = 0; i < animalTemplates.Count; i++) {
                
                if(creature.animalAI.animalName.Contains(animalTemplates[i].animalName)) {
                    lines[i]++;
                }
            }
        }


        livingAnimals = temp;
        creatureAmounts.text = "";
        for (int i = 0; i < lines.Length; i++) {

            creatureAmounts.text += animalTemplates[i].animalName + ": " + lines[i] + "\n";
        }

    }

    public void SpawnCreature(IAnimal template, Vector3 pos, bool alterCreature) {

        int mutationCount = 0;




        GameObject animalToSpawn = Resources.Load("animalPlaceholder") as GameObject;

        if(alterCreature)
        pos = new Vector3(pos.x + Random.Range(-5.25f, 5.25f), pos.y, pos.z + Random.Range(-5.25f, 5.25f));

        GameObject spawnObject = Instantiate(animalToSpawn, pos, Quaternion.identity);
        spawnObject.GetComponent<CreatureController>().enabled = false;
        if(!alterCreature)
            spawnObject.GetComponent<CreatureController>().disableRandom();
        IAnimal temp = template;


        spawnObject.GetComponent<CreatureController>().currentHealth = template.maxHealth;

        if (alterCreature) {
            float random = Random.Range(0.0f, 1.0f);
            while (random > globalMutationChance && mutationCount <= 2) {
                mutationCount++;
                random = Random.Range(0.0f, 1.0f);
            }

            temp = Instantiate(template);
            Vector3 randomColor = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            Color _creatureColor = new Color(randomColor.x, randomColor.y, randomColor.z, 1f);
            temp.creatureColor = _creatureColor;
        }
        else {
            temp.creatureColor = template.creatureColor;
        }
        




        spawnObject.GetComponent<CreatureController>().animalAI = temp;
        if (alterCreature) {
            //Add Random mutation
            for (int i = 0; i < mutationCount; i++) {
                IMutation tempMutation = globalMutations[Random.Range(0, globalMutations.Count)];
                if (!spawnObject.GetComponent<CreatureController>().animalAI.mutations.Contains(tempMutation))
                    spawnObject.GetComponent<CreatureController>().animalAI.mutations.Add(tempMutation);
            }
        }

        
        livingAnimals.Add(spawnObject.GetComponent<CreatureController>());
        index++;
        spawnObject.GetComponent<CreatureController>().enabled = true;
    }
    public void SpawnCreatures(IAnimal template, int amt, Vector3 pos) {
        for (int i = 0; i < amt; i++) {
            SpawnCreature(template, pos, true);
        }

    }
}
