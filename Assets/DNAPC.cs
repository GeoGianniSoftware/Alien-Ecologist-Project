using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DNAPC : ItemContainer
{
    public Canvas screen;
    public Text infoText;
    public float calculationTime = 2f;
    float t;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        t = calculationTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (storedItems[0] != null) {

            t -= Time.deltaTime;
        }
        else {

            t = calculationTime;
        }

        if (storedItems[0] == null) {
                    infoText.text = "Input Sample\n|\nV";
                }
        else if (storedItems[0] != null && t > 0) {
                    infoText.text = "Analyzing";
                    if (t / calculationTime < .3f)
                        infoText.text += ".";
                    if (t / calculationTime < .6f)
                        infoText.text += ".";
                    if (t / calculationTime < .9f)
                        infoText.text += ".";

            }
            else if (t <= 0) {
                SampleItem temp = (SampleItem)storedItems[0];
                if (temp.sampleOf.GetType() == typeof(IAnimal)) {
                    IAnimal animal = (IAnimal)temp.sampleOf;
                    infoText.text = "Blood Sample:" + "\n";
                
                    infoText.text += "<b><i>" + (animal.animalName).ToUpper() + "</i></b>" + "\n\n";
                    infoText.text += "Speed: " + Mathf.Round(animal.speed * 2.237f) + " MPH\n";
                    infoText.text += "Hunger: " + animal.hungerRate + "\n";
                    infoText.text += "Diet: " + animal.animalDiet + "\n";
                    infoText.text += "Sex: " + animal.animalGender+"\n" ;
                    if(animal.mutations.Count > 0) {
                    infoText.text += "Mutations: \n";
                    foreach (IMutation M in animal.mutations) {
                        infoText.text += M.NAME + "\n";
                    }
                }
                }


            }
    }

    public override bool canStore() {
        if (storedItems[0] == null)
            return true;
        return false;
    }
}
