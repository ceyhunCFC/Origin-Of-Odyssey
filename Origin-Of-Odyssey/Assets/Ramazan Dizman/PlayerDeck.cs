using Proyecto26;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayerDeck : MonoBehaviour
{
    private readonly string databaseURL = "https://origin-of-odyssey-eee04-default-rtdb.firebaseio.com/Users";
    
    private List<string> cardNames = new List<string>();

    private string[] playerDeck;

    public string localId;
    public string idToken;

    public GameObject NotEnoughCard;
    public GameObject CardPrefab,cardsPanel;
    GameObject newButton;

    private void Awake()
    {
        localId = AuthManager.localId;
        idToken = AuthManager.idToken;
    }

    public void GetCardsData()
    {
        print(localId + " " + idToken);
        RestClient.Get(databaseURL + "/" + localId + "/PlayerDeck" + ".json?auth=" + idToken).Then(PlayerDeck =>
        {
            playerDeck = ParseJsonArray(PlayerDeck.Text);
            if (cardsPanel.transform.childCount == 0)
            {
                string cardName = playerDeck[0];
                newButton = Instantiate(CardPrefab, cardsPanel.transform);

                Text[] texts = newButton.GetComponentsInChildren<Text>();
                texts[0].text = cardName;
            }
            for (int i = 1; i < playerDeck.Length; i++)
            {
                string cardName = playerDeck[i];
                newButton = Instantiate(CardPrefab, cardsPanel.transform);

                Text[] texts = newButton.GetComponentsInChildren<Text>();
                texts[0].text = cardName;
                int mana = GetManaForCard(cardName);
                SetCardImage(cardName);
                if (mana != -1)
                {
                    texts[1].text = mana.ToString();
                }
            }
        }).Catch(error =>
        {
            Debug.LogError("Error retrieving playerdeck: " + error.Message);
        });
    }
    public void SaveButton()
    {
        if (cardsPanel != null)
        {
            foreach (Transform card in cardsPanel.transform)
            {
                Text cardText = card.GetComponentInChildren<Text>();
                if (cardText != null)
                {
                    cardNames.Add(cardText.text);
                }
            }
        }

        string jsonData = "[" + string.Join(",", cardNames.ConvertAll(name => "\"" + name + "\"").ToArray()) + "]";
        Debug.Log(cardNames.Count + jsonData);
        if (cardNames.Count == 40)
        {
            RestClient.Put(databaseURL + "/" + localId + "/PlayerDeck" + ".json?auth=" + idToken, jsonData)
            .Then(response =>
            {
                cardNames.Clear();
            })
            .Catch(error =>
            {
                Debug.LogError("Error for save cards " + error.Message);
            });
            AuthManager.playerDeckArray = ParseJsonArray(jsonData);
            NotEnoughCard.SetActive(false);
        }
        else
        {
            NotEnoughCard.SetActive(true);
            Text notEnoughCardText = NotEnoughCard.GetComponent<Text>();

            if (notEnoughCardText != null)
            {
                notEnoughCardText.text = "You have selected " + cardNames.Count + " cards; you need to select 40 cards.";
            }
            cardNames.Clear();
        }
    }

    private int GetManaForCard(string cardName)
    {
        int mana = -1;

        ZeusCard zeusCard = new ZeusCard();
        foreach (Minion minion in zeusCard.minions)
        {
            if (cardName == minion.name)
            {
                mana = minion.mana;
                break;
            }
        }
        foreach (Spell spell in zeusCard.spells)
        {
            if (cardName == spell.name)
            {
                mana = spell.mana;
                break;
            }
        }

        OdinCard odinCard = new OdinCard();
        foreach (Minion minion in odinCard.minions)
        {
            if (cardName == minion.name)
            {
                mana = minion.mana;
                break;
            }
        }
        foreach (Spell spell in odinCard.spells)
        {
            if (cardName == spell.name)
            {
                mana = spell.mana;
                break;
            }
        }

        AnubisCard anubisCard = new AnubisCard();
        foreach (Minion minion in anubisCard.minions)
        {
            if (cardName == minion.name)
            {
                mana = minion.mana;
                break;
            }
        }
        foreach (Spell spell in anubisCard.spells)
        {
            if (cardName == spell.name)
            {
                mana = spell.mana;
                break;
            }
        }

        GenghisCard genghisCard = new GenghisCard();
        foreach (Minion minion in genghisCard.minions)
        {
            if (cardName == minion.name)
            {
                mana = minion.mana;
                break;
            }
        }
        foreach (Spell spell in genghisCard.spells)
        {
            if (cardName == spell.name)
            {
                mana = spell.mana;
                break;
            }
        }

        LeonardoCard leonardoCard = new LeonardoCard();
        foreach (Minion minion in leonardoCard.minions)
        {
            if (cardName == minion.name)
            {
                mana = minion.mana;
                break;
            }
        }
        foreach (Spell spell in leonardoCard.spells)
        {
            if (cardName == spell.name)
            {
                mana = spell.mana;
                break;
            }
        }

        DustinCard dustinCard = new DustinCard();
        foreach (Minion minion in dustinCard.minions)
        {
            if (cardName == minion.name)
            {
                mana = minion.mana;
                break;
            }
        }
        foreach (Spell spell in dustinCard.spells)
        {
            if (cardName == spell.name)
            {
                mana = spell.mana;
                break;
            }
        }

        StandartCards standartCards = new StandartCards();
        foreach (StandartCard card in standartCards.standartcards)
        {
            if (cardName == card.name)
            {
                mana = card.mana;
                break;
            }
        }

        if (mana == -1)
        {
            Debug.LogError("Mana value not found for card: " + cardName);
        }

        return mana;
    }

    void SetCardImage(string name)
    {
        string cardName = name;
        string imagePath = "CardImages/" + cardName;

        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
        {
            Transform imageParent = newButton.transform.Find("Image"); 
            if (imageParent != null)
            {
                Transform cardImageTransform = imageParent.Find("CardsImage");
                if (cardImageTransform != null)
                {
                    Image cardsImage = cardImageTransform.GetComponent<Image>();
                    if (cardsImage != null)
                    {
                        cardsImage.sprite = sprite;
                    }
                }
                else
                {
                    Debug.LogWarning("CardsImage nesnesi bulunamadư!");
                }
            }
            else
            {
                Debug.LogWarning("ImageParent nesnesi bulunamadư!");
            }
        }
    }


    string[] ParseJsonArray(string jsonArray)
    {
        int startIndex = jsonArray.IndexOf('[') + 1;
        int endIndex = jsonArray.LastIndexOf(']');
        string elements = jsonArray.Substring(startIndex, endIndex - startIndex);

        string[] parts = elements.Split(',');

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i] = parts[i].Trim('\"');
        }

        return parts;
    }
}
