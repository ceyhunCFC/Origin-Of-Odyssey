using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitorSlot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Stoper()
    {
        GetComponent<Animator>().SetBool("Stop",true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Stoper();
        }
    }
}
