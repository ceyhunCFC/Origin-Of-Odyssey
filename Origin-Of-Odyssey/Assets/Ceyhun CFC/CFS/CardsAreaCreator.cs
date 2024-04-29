using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsAreaCreator : MonoBehaviour
{
    public GameObject[] FrontAreaCollisions;
    public GameObject[] BackAreaCollisions;

    // Start is called before the first frame update
    /* void Start()
     {
         for (int x = 0; x < 7; x++)
         {
             for (int z = 0; z < 7; z++)
             { 
                 float xPos = x * 0.35f; // Kartın X konumunu belirliyoruz
                 float zPos = z * 0.5f; // Kartın X konumunu belirliyoruz

                 GameObject card =  Instantiate(AreaCollision,gameObject.transform);
                 card.transform.localPosition = new Vector3(xPos, 0.5f, zPos);
             }

         }
     }*/


}
