using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstCardPrefab : MonoBehaviour
{
    public string Card1, Card2, Card3;
    public Text card1, card2, card3;

    public void SetInformation()
    {
        card1.text = Card1;
        card2.text = Card2;
        card3.text = Card3;
    }

    public void AcceptButton()
    {
        transform.position = new Vector3(transform.position.x, -1.64f,transform.position.z);
    }
}
