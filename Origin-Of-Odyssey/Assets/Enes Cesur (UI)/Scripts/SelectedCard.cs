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
            button.onClick.AddListener(DeleteCard);
        }
    }

    public void DeleteCard()
    {
        Destroy(gameObject);
    }

    public void DeleteAllSelectedCards()
    {
        GameObject panel = GameObject.FindWithTag("CardContainerTag");

        if (panel != null)
        {
            
            foreach (Transform child in panel.transform)
            {
                SelectedCard selectedCard = child.GetComponent<SelectedCard>();

                if (selectedCard != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
