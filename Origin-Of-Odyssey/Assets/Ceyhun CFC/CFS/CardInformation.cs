using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInformation : MonoBehaviour
{
    public string CardName;
    public string CardDes;
    public string CardHealth;
    public int CardDamage;
    public int CardMana;

    public Text CardNameText;
    public Text CardDesText;
    public Text CardHealthText;
    public Text CardDamageText;
    public Text CardManaText;


    public void SetInformation()
    {
        CardNameText.text = CardName;
        CardDesText.text = CardDes;
        CardHealthText.text = CardHealth;
        CardDamageText.text = CardDamage.ToString();
        CardManaText.text = CardMana.ToString();
    }

}
