using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitorCardSize : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        print(GameObject.Find("Arrow").transform.position.y - transform.position.y);

        float value = 1 - (1 * (Mathf.Abs(GameObject.Find("Arrow").transform.position.y - transform.position.y)/5) / 100);

        transform.localScale = new Vector3(value, value, value);
            
    }
}
