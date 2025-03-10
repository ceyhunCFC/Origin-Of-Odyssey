using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCard : MonoBehaviour
{

    public Text NameText;
    public Text ManaText;
    public string cardType;
   

    void Start()
    {
        
        Button button = GetComponent<Button>();
        if (button != null)
        {
            //button.onClick.AddListener(DeleteCard);
        }
    }

    public void DeleteCard()
    {
       
        string[] protectedCards = { "Zeus", "Odin", "Anubis", "Genghis", "Dustin", "DaVinci" };

        if (NameText != null)
        {
            
            string cardName = NameText.text;

            GameObject panel = GameObject.FindWithTag("CardContainerTag");
            PlayerDeck playerDeck = panel.transform.parent.parent.parent.GetComponent<PlayerDeck>();
            if (!Array.Exists(protectedCards, name => name == cardName))
            {
                gameObject.transform.parent = null;
                playerDeck.UpdateDeckCount();
                playerDeck.GetManaForCard(cardName, out var rarity);
                playerDeck.UpdateRarityCount(rarity, -1);
                playerDeck.RemoveSelected(cardName);
                Destroy(gameObject);
            }
            
        }
       
    }


    public void DeleteAllSelectedCards()
    {
        GameObject panel = GameObject.FindWithTag("CardContainerTag");
        PlayerDeck playerDeck = panel.transform.parent.parent.parent.GetComponent<PlayerDeck>();
        playerDeck.UpdateDeckCount(1);
        if (panel != null)
        {
            foreach (Transform child in panel.transform)
            {
                SelectedCard selectedCard = child.GetComponent<SelectedCard>();

                if (selectedCard != null)
                {
                    
                    Text nameText = selectedCard.NameText;
                    if (nameText != null)
                    {
                        
                        string cardName = nameText.text;

                       
                        string[] protectedCards = { "Zeus", "Odin", "Anubis", "Genghis", "Dustin", "DaVinci" };

                        
                        if (!Array.Exists(protectedCards, name => name == cardName))
                        {
                            playerDeck.GetManaForCard(cardName, out var rarity);
                            playerDeck.UpdateRarityCount(rarity, -1);
                            playerDeck.RemoveSelected(cardName);
                            Destroy(child.gameObject);
                        }
                        
                    }
                }
            }
        }
    }
}
