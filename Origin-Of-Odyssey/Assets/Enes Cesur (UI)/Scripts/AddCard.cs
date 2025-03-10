using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCard : MonoBehaviour
{
    public GameObject prefab;
    public Text warningMessage;
    public void OnButtonPress()
    {
        CardDisplay cardDisplay = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<CardDisplay>();

        if (cardDisplay != null)
        {
            GameObject panel = GameObject.FindWithTag("CardContainerTag");

            if (panel != null)
            {
                bool shouldAddCard = true;

                
                foreach (Transform child in panel.transform)
                {
                    SelectedCard existingCard = child.GetComponent<SelectedCard>();

                    
                    if (existingCard != null && existingCard.NameText.text == cardDisplay.cardNameText.text)
                    {
                        shouldAddCard = false;
                        break;
                    }

                    
                    if ((cardDisplay.cardType == "Zeus" || cardDisplay.cardType == "Odin"|| cardDisplay.cardType == "Anubis" || cardDisplay.cardType == "DaVinci"|| cardDisplay.cardType == "Dustin"|| cardDisplay.cardType == "Genghis") &&
                        (existingCard != null && existingCard.cardType != cardDisplay.cardType && existingCard.cardType != "Standart"))
                    {
                        shouldAddCard = false;
                        break;
                    }
                }

                if (shouldAddCard)
                {
                    if (panel.transform.childCount >= 40)
                    {
         
                        return;
                    }

                    GameObject yeniButon = Instantiate(prefab, panel.transform);
                    yeniButon.transform.localPosition = new Vector3(0, 0, 0);
                    PlayerDeck playerDeck = panel.transform.parent.parent.parent.GetComponent<PlayerDeck>();
                    playerDeck.UpdateDeckCount();
                    Rarity rarity;
                    Enum.TryParse(cardDisplay.cardRarityText.text, out rarity);
                    playerDeck.UpdateRarityCount(rarity, 1);
                    cardDisplay.selectedCardImage.gameObject.SetActive(true);
                    SelectedCard selectedCard = yeniButon.GetComponent<SelectedCard>();
                    if (selectedCard != null)
                    {
                        selectedCard.NameText.text = cardDisplay.cardNameText.text;
                        selectedCard.ManaText.text = cardDisplay.cardManaText.text;
                        selectedCard.cardType = cardDisplay.cardType;
                    }
                    Transform imageParent = yeniButon.transform.Find("Image"); 
                    if (imageParent != null)
                    {
                        Transform cardImageTransform = imageParent.Find("CardsImage");
                        if (cardImageTransform != null)
                        {
                            Image cardsImage = cardImageTransform.GetComponent<Image>();
                            if (cardsImage != null)
                            {
                                cardsImage.sprite = cardDisplay.cardImage.sprite; 
                            }
                        }
                        else
                        {
                            Debug.LogWarning("CardsImage nesnesi bulunamad�!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("ImageParent nesnesi bulunamad�!");
                    }
                }
                
            }
        }
    }



}