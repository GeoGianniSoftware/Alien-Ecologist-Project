using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchDart : Interactable
{
    public SampleItem infoSource = null;
    bool activated = false;
    public GameObject gfx;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(infoSource == null)
        SampleParent();
        else if (gfx != null && infoSource != null) {
            gfx.transform.position = transform.position;
            gfx.transform.rotation = transform.rotation;
        }
    }

    

    void SampleParent() {
        if (transform.parent != null && !activated) {
            if(transform.GetComponentInParent<CreatureController>() != null) {
               


                FindObjectOfType<PlayerController>().spawnUIPing(this);
                IAnimal animalRef = transform.GetComponentInParent<CreatureController>().animalAI;
                string slug = animalRef.animalName;
                slug = slug.ToLower();

                Texture2D tex = Item.getItemSprite(slug);

                Color tempColor = Color.white;
                if (animalRef.creatureColor != null)
                    tempColor = animalRef.creatureColor;

                print(animalRef.animalName);
                SampleItem temp = Item.CreateSample(55, animalRef.animalName + " Blood Sample", tempColor, animalRef, Time.time, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * .5f));
                temp = SampleItem.Instantiate(temp);
                infoSource = temp;

            }
            activated = true;
        }
        if(activated) {
            if (infoSource == null)
                Die(0);

            if (gfx == null && transform.childCount > 0) {
                GameObject Temp = transform.GetChild(0).gameObject;
                Temp.transform.SetParent(null);
                Temp.transform.localScale = Vector3.one;
                gfx = Temp;
               
            }
            

        }
    }

    void Die(int time) {
        if(time != 0) {
            GetComponent<Collider>().isTrigger = false;
            transform.SetParent(null);
            gameObject.AddComponent<Rigidbody>();
        }
        if (gfx != null)
            Destroy(gfx);
        else {
            Destroy(transform.GetChild(0).gameObject);
        }

        Destroy(gameObject, time);
    }
  

    public override void Interact() {
        base.Interact();
        if(infoSource != null)
            FindObjectOfType<PlayerInventory>().addItem(infoSource);

        Die(10);
    }
}
