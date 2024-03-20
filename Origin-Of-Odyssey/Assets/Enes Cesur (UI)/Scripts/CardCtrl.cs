using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCtrl : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform parentTransform;
    public GameObject SelectedCartPanel;

    public Button ZeusBtn;
    public Button OdinsBtn;
    public Button AnubisBtn;
    public Button DaVinciBtn;
    public Button DustinBtn;
    public Button GenghisBtn;

    private Dictionary<string, List<Minion>> cardSets = new Dictionary<string, List<Minion>>();

    void Start()
    {
        InitializeCardSets();
        StandartCard();
        Zeus();
    }


    void InitializeCardSets()
    {
        cardSets.Add("Zeus", new ZeusCard().minions);
        cardSets.Add("Odin", new OdinCard().minions);
        cardSets.Add("Anubis", new AnubisCard().minions);
        cardSets.Add("Genghis", new GenghisCard().minions);
        cardSets.Add("DaVinci", new LeonardoCard().minions);
        cardSets.Add("Dustin", new DustinCard().minions);
    }


    public void AddCardSet(string cardSetName)
    {
        ClearCards();
        GridLayoutGroup gridLayout = parentTransform.GetComponent<GridLayoutGroup>();
        gridLayout.spacing = new Vector2(10, 10);

        if (cardSets.ContainsKey(cardSetName))
        {
            List<Minion> minions = cardSets[cardSetName];
            foreach (Minion minion in minions)
            {
                GameObject newCard = Instantiate(cardPrefab, parentTransform);
                CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();

                cardDisplay.cardNameText.text = minion.name;
                cardDisplay.cardRarityText.text = minion.rarity.ToString();
                cardDisplay.cardAttackText.text = minion.attack.ToString();
                cardDisplay.cardHealthText.text = minion.health.ToString();
                cardDisplay.cardManaText.text = minion.mana.ToString();
            }
        }
        else
        {
            Debug.LogError("Card set not found: " + cardSetName);
        }
    }


    void ClearCards()
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }


    public void Zeus()
    {
        AddCardSet("Zeus");
        StandartCard();
        ResetButtonColors();
        ZeusBtn.image.color = Color.blue;
    }

    public void Odin()
    {
        AddCardSet("Odin");
        StandartCard();
        ResetButtonColors();
        OdinsBtn.image.color = Color.blue;
    }

    public void Anubis()
    {
        AddCardSet("Anubis");
        StandartCard();
        ResetButtonColors();
        AnubisBtn.image.color = Color.blue;
    }

    public void Genghis()
    {
        AddCardSet("Genghis");
        StandartCard();
        ResetButtonColors();
        GenghisBtn.image.color = Color.blue;
    }

    public void DaVinci()
    {
        AddCardSet("DaVinci");
        StandartCard();
        ResetButtonColors();
        DaVinciBtn.image.color = Color.blue;
    }

    public void Dustin()
    {
        AddCardSet("Dustin");
        StandartCard();
        ResetButtonColors();
        DustinBtn.image.color = Color.blue;
    }

    public void StandartCard()
    {
        StandartCards standartcart = new StandartCards();


        GridLayoutGroup gridLayout = parentTransform.GetComponent<GridLayoutGroup>();
        gridLayout.spacing = new Vector2(10, 10);

        foreach (StandartCard card in standartcart.standartcards)

        {
            GameObject newCard = Instantiate(cardPrefab, parentTransform);
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();

            cardDisplay.cardNameText.text = card.name;
            cardDisplay.cardRarityText.text = card.rarity.ToString();
            cardDisplay.cardAttackText.text = card.attack.ToString();
            cardDisplay.cardHealthText.text = card.health.ToString();
            cardDisplay.cardManaText.text = card.mana.ToString();
        }
    }

    void ResetButtonColors()
    {
        ZeusBtn.image.color = Color.white;
        OdinsBtn.image.color = Color.white;
        AnubisBtn.image.color = Color.white;
        GenghisBtn.image.color = Color.white;
        DaVinciBtn.image.color = Color.white;
        DustinBtn.image.color = Color.white;
    }
}
