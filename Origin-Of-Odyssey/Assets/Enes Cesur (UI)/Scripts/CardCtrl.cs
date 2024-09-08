using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCtrl : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject CreateCardPrefab;
    public Transform parentTransform;
    public GameObject SelectedCartPanel;

    public Button ZeusBtn;
    public Button OdinsBtn;
    public Button AnubisBtn;
    public Button DaVinciBtn;
    public Button DustinBtn;
    public Button GenghisBtn;

    private Dictionary<string, List<Minion>> cardSets = new Dictionary<string, List<Minion>>();
    private Dictionary<string, List<Spell>> spellSets = new Dictionary<string, List<Spell>>();

    public Text HeroUpdate;
    public GameObject warningPanel;
    public Button confirmButton;
    public Button rejectButton;
    private void Awake()
    {
        InitializeCardSets();
        StandartCard();
    }
    public void ControlMainCard()
    {
        string playerName = AuthManager.playerDeckArray[0];
        switch (playerName)
        {
            case "Zeus":
                ConfirmZeus();
                break;
            case "Odin":
                ConfirmOdin();
                break;
            case "Anubis":
                ConfirmAnubis();
                break;
            case "Genghis":
                ConfirmGenghis();
                break;
            case "Leonardo Da Vinci":
                ConfirmDaVinci();
                break;
            case "Dustin":
                ConfirmDustin();
                break;
        }
    }
    public void UpdateText(string newText)
    {
        HeroUpdate.text = newText;
    }
    
    void InitializeCardSets()
    {
        cardSets.Add("Zeus", new ZeusCard().minions);
        cardSets.Add("Odin", new OdinCard().minions);
        cardSets.Add("Anubis", new AnubisCard().minions);
        cardSets.Add("Genghis", new GenghisCard().minions);
        cardSets.Add("Leonardo Da Vinci", new LeonardoCard().minions);
        cardSets.Add("Dustin", new DustinCard().minions);
        spellSets.Add("Zeus", new ZeusCard().spells);
        spellSets.Add("Odin", new OdinCard().spells);
        spellSets.Add("Anubis", new AnubisCard().spells);
        spellSets.Add("Genghis", new GenghisCard().spells);
        spellSets.Add("Leonardo Da Vinci", new LeonardoCard().spells);
        spellSets.Add("Dustin", new DustinCard().spells);

    }

    void SetCardImage(CardDisplay cardDisplay)
    {
        string cardName = cardDisplay.cardNameText.text;
        string imagePath = "CardImages/" + cardName ;
        
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
        {
            cardDisplay.cardImage.sprite = sprite;
        }
    }
    void SetCArdBorderImg(CardDisplay cardDisplay)
    {
        string cardRarity = cardDisplay.cardRarityText.text;
        string imagePath1 = "CardsBorder/" + cardRarity;
        Sprite sprite1 = Resources.Load<Sprite>(imagePath1);
        if (sprite1 != null)
        {
            cardDisplay.cardBorderImage.sprite = sprite1;
        }
        
    }
    
    public void AddCardSet(string cardSetName)
    {
        ClearCards();              
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
                cardDisplay.cardType = cardSetName;
                SetCardImage(cardDisplay);
                SetCArdBorderImg(cardDisplay);
            }
        }
        if (spellSets.ContainsKey(cardSetName))
        {
            List<Spell> spells = spellSets[cardSetName];
            foreach (Spell spell in spells)
            {
                GameObject newCard = Instantiate(cardPrefab, parentTransform);
                CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
                cardDisplay.cardNameText.text = spell.name;
                cardDisplay.cardRarityText.text = spell.rarity.ToString();
                cardDisplay.cardAttackText.text = "";
                cardDisplay.cardHealthText.text = "";
                cardDisplay.cardManaText.text = spell.mana.ToString();
                cardDisplay.cardType = cardSetName;
                SetCardImage(cardDisplay);
                SetCArdBorderImg(cardDisplay);

            }
        }
        
        else
        {
            Debug.LogError("Card set not found: " + cardSetName);
        }
    }
    public void StandartCard()
    {
        StandartCards standartcart = new StandartCards();



        foreach (StandartCard card in standartcart.standartcards)

        {
            GameObject newCard = Instantiate(cardPrefab, parentTransform);
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            cardDisplay.cardNameText.text = card.name;
            cardDisplay.cardRarityText.text = card.rarity.ToString();
            cardDisplay.cardAttackText.text = card.attack.ToString();
            cardDisplay.cardHealthText.text = card.health.ToString();
            cardDisplay.cardManaText.text = card.mana.ToString();
            cardDisplay.cardType = "Standart";
            SetCardImage(cardDisplay);
            SetCArdBorderImg(cardDisplay);
        }
    }

    void ClearCards()
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowWarningPanel(string message, UnityEngine.Events.UnityAction confirmAction, UnityEngine.Events.UnityAction rejectAction)
    {
        warningPanel.SetActive(true);
        warningPanel.GetComponentInChildren<Text>().text = message;
        confirmButton.onClick.AddListener(confirmAction);
        rejectButton.onClick.AddListener(rejectAction);
    }

    public void HideWarningPanel()
    {
        warningPanel.SetActive(false);
        confirmButton.onClick.RemoveAllListeners();
        rejectButton.onClick.RemoveAllListeners();
    }
    public void SpawnCard(string cardName)
    {
        GameObject panel = GameObject.FindWithTag("CardContainerTag");
        if (panel != null)
        {
            GameObject newCard = Instantiate(CreateCardPrefab, panel.transform);
            newCard.GetComponentInChildren<Text>().text = cardName;
            newCard.GetComponent<SelectedCard>().cardType =cardName;

        }

    }
    public void ConfirmZeus()
    {
        
        ZeusBtn.interactable = false;
        OdinsBtn.interactable = true;
        AnubisBtn.interactable = true;
        GenghisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        DustinBtn.interactable = true;
        
        AddCardSet("Zeus");
        StandartCard();
        ResetButtonColors();
        ZeusBtn.image.color = Color.blue;
        UpdateText("Zeus Cards");
        
        HideWarningPanel();
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
        SpawnCard("Zeus");

    }

    public void ConfirmOdin()
    {
        OdinsBtn.interactable = false;
        ZeusBtn.interactable = true;
        AnubisBtn.interactable = true;
        GenghisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        DustinBtn.interactable = true;
        AddCardSet("Odin");
        StandartCard();
        ResetButtonColors();
        OdinsBtn.image.color = Color.blue;
        UpdateText("Odin Cards");
        HideWarningPanel();
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
        SpawnCard("Odin");
    }

    public void ConfirmAnubis()
    {
        AnubisBtn.interactable = false;
        ZeusBtn.interactable = true;
        OdinsBtn.interactable = true;
        GenghisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        DustinBtn.interactable = true;
        AddCardSet("Anubis");
        StandartCard();
        ResetButtonColors();
        AnubisBtn.image.color = Color.blue;
        UpdateText("Anubis Cards");
        HideWarningPanel();
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
        SpawnCard("Anubis");
    }

    public void ConfirmGenghis()
    {
        GenghisBtn.interactable = false;
        ZeusBtn.interactable = true;
        OdinsBtn.interactable = true;
        AnubisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        DustinBtn.interactable = true;
        AddCardSet("Genghis");
        StandartCard();
        ResetButtonColors();
        GenghisBtn.image.color = Color.blue;
        UpdateText("Genghis Cards");
        HideWarningPanel();
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
        SpawnCard("Genghis");
    }

    public void ConfirmDaVinci()
    {
        DaVinciBtn.interactable = false;
        ZeusBtn.interactable = true;
        OdinsBtn.interactable = true;
        AnubisBtn.interactable = true;
        GenghisBtn.interactable = true;
        DustinBtn.interactable = true;
        AddCardSet("Leonardo Da Vinci");
        StandartCard();
        ResetButtonColors();
        DaVinciBtn.image.color = Color.blue;
        UpdateText("DaVinci Cards");
        HideWarningPanel();
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
        SpawnCard("Leonardo Da Vinci");
    }

    public void ConfirmDustin()
    {
        DustinBtn.interactable = false;
        ZeusBtn.interactable = true;
        OdinsBtn.interactable = true;
        AnubisBtn.interactable = true;
        GenghisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        AddCardSet("Dustin");
        StandartCard();
        ResetButtonColors();
        DustinBtn.image.color = Color.blue;
        UpdateText("Dustin Cards");
        HideWarningPanel();
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
        SpawnCard("Dustin");
    }
     
    public void Reject()
    {
        HideWarningPanel();
    }
    public void Zeus()
    {
        ShowWarningPanel("You can only select cards belonging to one hero. If you choose Zeus's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmZeus, Reject);
    }

    public void Odin()
    {
        ShowWarningPanel("You can only select cards belonging to one hero. If you choose Odin's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmOdin, Reject);
    }

    public void Anubis()
    {
        ShowWarningPanel("You can only select cards belonging to one hero. If you choose Anubis's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmAnubis, Reject);
    }

    public void Genghis()
    {
        ShowWarningPanel("You can only select cards belonging to one hero. If you choose Genghis's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmGenghis, Reject);
    }

    public void DaVinci()
    {
        ShowWarningPanel("You can only select cards belonging to one hero. If you choose DaVinci's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmDaVinci, Reject);
    }

    public void Dustin()
    {
        ShowWarningPanel("You can only select cards belonging to one hero. If you choose Dustin's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmDustin, Reject);
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