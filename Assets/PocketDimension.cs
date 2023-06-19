using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PocketDimension : Interactable
{
    public IAnimal attachedCreature;
    GameObject animalGraphics;
    public bool filled = false;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<CreatureController>() != null && attachedCreature == null) {
            attachedCreature = GetComponentInParent<CreatureController>().animalAI;
            
            animalGraphics = Instantiate(transform.parent.GetChild(0).gameObject, transform.position, Quaternion.identity);
            animalGraphics.transform.GetChild(0).transform.localPosition -= Vector3.up*.85f;
        }

        if(attachedCreature == null) {
            Destroy(gameObject);
        }

        if (attachedCreature == true && !filled) {
            print(attachedCreature.maxHealth);
            GameObject parent = GetComponentInParent<CreatureController>().gameObject;
            transform.SetParent(null);
            Vector3 tempVelocity = parent.GetComponent<NavMeshAgent>().velocity;
            Destroy(parent);
            filled = true;
            Color temp = attachedCreature.creatureColor;
            temp.a = 0.2f;
            GetComponent<MeshRenderer>().material.color = temp;



            ParticleSystem PS = GetComponentInChildren<ParticleSystem>();
            ParticleSystem.MainModule ma = PS.main;
            temp.a = 1;
            ma.startColor = temp;
            GetComponent<Rigidbody>().velocity = tempVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(animalGraphics != null) {
            animalGraphics.transform.localScale = Vector3.one * .2f;
            animalGraphics.transform.position = transform.position + (Vector3.down*attachedCreature.heightOffset)*3f;
            animalGraphics.transform.rotation = transform.rotation;
        }
    }

    public override void Interact() {
        base.Interact();
        if (attachedCreature != null) {
            /*
            string slug = attachedCreature.animalName;
            slug = slug.ToLower();

            Texture2D tex = Item.getItemSprite(slug);

            Color tempColor = Color.white;
            if (attachedCreature.creatureColor != null)
                tempColor = attachedCreature.creatureColor;

            OrganismItem creatureSample = Item.CreateOrganism(55, attachedCreature.animalName + " Blood Sample", tempColor, attachedCreature, Time.time, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * .5f));
            FindObjectOfType<PlayerInventory>().addItem(creatureSample);
            */
            FindObjectOfType<CreatureSpawns>().SpawnCreature(attachedCreature, transform.position + Vector3.up, false);

            if(animalGraphics != null)
                Destroy(animalGraphics);    
            Destroy(this.gameObject);
            
        }
            
         
    }
}
