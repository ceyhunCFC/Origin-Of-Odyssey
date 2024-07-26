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
    public string MaxHealth;
   // public Sprite CardVisual;
    public bool CardFreeze=false;
    public bool isItFirstRaound = true;
    public bool FirstTakeDamage = true;         //raunt ba��na
    public bool HaveShield = false;
    public bool DivineSelected = false;
    public bool FirstDamageTaken = true;       // kart konuldu�u andan beri       true damage almamis demek
    public bool isAttacked = false;
    public bool GerDefense = false;
    public bool MongolFury = false;
    public bool CanAttackBehind = false;
    public bool EternalShield = false;
    public bool Gallop = false;                    //tur sonunda eline geri verir kartı
    public bool Invulnerable = false;               //saldırırken yaralanmaz
    public bool SunDiskRadiance = false;
    public int ChargeBrokandSindri = 0;             


    public Text CardNameText;
    public Text CardDesText;
    public Text CardHealthText;
    public Text CardDamageText;
    public Text CardManaText;
    public Image CardVisualImage;


    public void SetInformation()
    {
        CardNameText.text = CardName;
        CardDesText.text = CardDes;
        CardHealthText.text = CardHealth;
        CardDamageText.text = CardDamage.ToString();
        CardManaText.text = CardMana.ToString();
        CardVisualImage.sprite = GetSpriteByName(CardName);
    }

    public Sprite GetSpriteByName(string spriteName)
    {
        // Resources klasöründe sprite'ı arar
        Sprite foundSprite = Resources.Load<Sprite>("CardImages/" + spriteName);

        if (foundSprite == null)
        {
            Debug.LogWarning("Sprite not found: " + spriteName);

            foundSprite = Resources.Load<Sprite>("CardImages/" + "Centaur Archer");
            return foundSprite;
        }

        return foundSprite;
    }

    public void SetMaxHealth()
    {
        MaxHealth = CardHealth;
    }

}
