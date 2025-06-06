using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardDisplay : MonoBehaviour
{
    public Text cardNameText;
    public Text cardRarityText;
    public Text cardAttackText;
    public Text cardHealthText;
    public Text cardManaText;
    public string cardType;
    public Image cardImage;
    public Image cardBorderImage;
    public Image selectedCardImage;
    public Text description;

    public GameObject spellManaBg, spellTxtBg, minionTxtBg;
    
    public void OpenSpellBGs()
    {
        spellManaBg.SetActive(true);
        spellTxtBg.SetActive(true);
        minionTxtBg.SetActive(false);
    }
}
