using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefab : MonoBehaviour
{
    public string Card1;
    public Text card1;
    // Start is called before the first frame update
    public void SetInformation()
    {
        card1.text = Card1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
