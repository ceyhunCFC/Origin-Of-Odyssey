using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Ender.Scripts;
using TMPro;
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
    public Button StandardBtn;

    public Button nextBtn;
    public Button previousBtn;
    public TextMeshProUGUI pageNumberText;
    
    private Dictionary<string, List<Minion>> cardSets = new Dictionary<string, List<Minion>>();
    private Dictionary<string, List<Spell>> spellSets = new Dictionary<string, List<Spell>>();

    public Text HeroUpdate;
    public GameObject warningPanel;
    public Button confirmButton;
    public Button rejectButton;
    
    
    private int _currentPage = 0;
    private int _maxPage;
    private PlayerDeck _playerDeck;
    private void Awake()
    {
        _playerDeck = SelectedCartPanel.transform.parent.GetComponent<PlayerDeck>();
        InitializeCardSets();
        StandartCard();
        ControlMainCard();
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
            var minionsOrdered=minions.OrderByDescending(x => x.rarity);
            foreach (Minion minion in minionsOrdered)
            {
                GameObject newCard = Instantiate(cardPrefab, parentTransform);
                CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
                cardDisplay.cardNameText.text = minion.name;
                cardDisplay.cardRarityText.text = minion.rarity.ToString();
                cardDisplay.cardAttackText.text = minion.attack.ToString();
                cardDisplay.cardHealthText.text = minion.health.ToString();
                cardDisplay.cardManaText.text = minion.mana.ToString();
                cardDisplay.cardType = cardSetName;
                cardDisplay.description.text = minion.description.ToString();
                SetCardImage(cardDisplay);
                SetCArdBorderImg(cardDisplay);
            }
        }
        if (spellSets.ContainsKey(cardSetName))
        {
            List<Spell> spells = spellSets[cardSetName];
            var spellsOrdered = spells.OrderByDescending(x => x.rarity);
            foreach (Spell spell in spellsOrdered)
            {
                GameObject newCard = Instantiate(cardPrefab, parentTransform);
                CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
                cardDisplay.cardNameText.text = spell.name;
                cardDisplay.cardRarityText.text = spell.rarity.ToString();
                cardDisplay.cardAttackText.text = "";
                cardDisplay.cardHealthText.text = "";
                cardDisplay.cardManaText.text = spell.mana.ToString();
                cardDisplay.cardType = cardSetName;
                cardDisplay.description.text = spell.description.ToString();
                SetCardImage(cardDisplay);
                SetCArdBorderImg(cardDisplay);

            }
            
        }
        
        else
        {
            Debug.LogError("Card set not found: " + cardSetName);
        }
        
        ResetPage();
        CalculatePageNumber(cardSets[cardSetName].Count+spellSets[cardSetName].Count);
        UpdatePageNumberText();
    }

    private void ResetPage()
    {
        _currentPage = 0;
        transform.localPosition = new Vector2(0, 0);
    }
    
    private void UpdatePageNumberText()
    {
        pageNumberText.text = (_currentPage+1) + "/" + (_maxPage+1);
    }

    private void CalculatePageNumber(int totalCards)
    {
        _maxPage = totalCards / 18;
        if(_maxPage>0)
            nextBtn.interactable = true;
    }
    
    public void NextPage()
    {
        if (_currentPage < _maxPage)
        {
            _currentPage++;
            UpdatePageNumberText();
            previousBtn.interactable = true;
            transform.DOLocalMoveX(_currentPage * -2400, 0.5f);
        }

        if (_currentPage >= _maxPage)
        {
            nextBtn.interactable = false;
        }
    }
    
    public void PreviousPage()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            UpdatePageNumberText();
            nextBtn.interactable = true;
            transform.DOLocalMoveX(_currentPage * -2400, 0.5f);
            if (transform.position.x>0)
            {
                transform.position=new Vector2(0,0);
            }
        }
        if (_currentPage <= 0)
        {
            previousBtn.interactable = false;
        }
    }
    
    public void StandartCard()
    {
        ZeusBtn.interactable = true;
        OdinsBtn.interactable = true;
        AnubisBtn.interactable = true;
        GenghisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        DustinBtn.interactable = true;
        StandardBtn.interactable = false;
        ReturnOtherButtonsToNormal(StandardBtn.gameObject);
        ResetButtonColors();
        //StandardBtn.image.color = Color.blue;
        UpdateText("Standard Cards");
        ClearCards();
        HideWarningPanel();
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
            cardDisplay.description.text = card.description.ToString();
            SetCardImage(cardDisplay);
            SetCArdBorderImg(cardDisplay);
        }
        ResetPage();
        CalculatePageNumber(standartcart.standartcards.Count);
        UpdatePageNumberText();
        OpenSelectedCards();
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
            _playerDeck.UpdateRarityCount(Rarity.Legendary,1);
        }

    }

    private void ReturnOtherButtonsToNormal(GameObject open)
    {
        ZeusBtn.gameObject.GetComponent<HeroDeckSelectButton>().isSelected=false;
        ZeusBtn.gameObject.GetComponent<HeroDeckSelectButton>().OpenNormal();
        OdinsBtn.gameObject.GetComponent<HeroDeckSelectButton>().isSelected=false;
        OdinsBtn.gameObject.GetComponent<HeroDeckSelectButton>().OpenNormal();
        GenghisBtn.gameObject.GetComponent<HeroDeckSelectButton>().isSelected=false;
        GenghisBtn.gameObject.GetComponent<HeroDeckSelectButton>().OpenNormal();
        AnubisBtn.gameObject.GetComponent<HeroDeckSelectButton>().isSelected=false;
        AnubisBtn.gameObject.GetComponent<HeroDeckSelectButton>().OpenNormal();
        DaVinciBtn.gameObject.GetComponent<HeroDeckSelectButton>().isSelected=false;
        DaVinciBtn.gameObject.GetComponent<HeroDeckSelectButton>().OpenNormal();
        DustinBtn.gameObject.GetComponent<HeroDeckSelectButton>().isSelected=false;
        DustinBtn.gameObject.GetComponent<HeroDeckSelectButton>().OpenNormal();
        StandardBtn.gameObject.GetComponent<HeroDeckSelectButton>().isSelected=false;
        StandardBtn.gameObject.GetComponent<HeroDeckSelectButton>().OpenNormal();
        open.GetComponent<HeroDeckSelectButton>().OpenSelected();
    }
    
    public void ConfirmZeus()
    {
        
        ZeusBtn.interactable = false;
        OdinsBtn.interactable = true;
        AnubisBtn.interactable = true;
        GenghisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        DustinBtn.interactable = true;
        StandardBtn.interactable = true;
        ReturnOtherButtonsToNormal(ZeusBtn.gameObject);
        
        AddCardSet("Zeus");
        //StandartCard();
        ResetButtonColors();
        //ZeusBtn.image.color = Color.blue;
        UpdateText("Zeus Cards");
        HideWarningPanel();
        OpenSelectedCards();
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Zeus")
        {
            DeleteDeck();
            SpawnCard("Zeus");
        }
    }

    public void ConfirmOdin()
    {
        OdinsBtn.interactable = false;
        ZeusBtn.interactable = true;
        AnubisBtn.interactable = true;
        GenghisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        DustinBtn.interactable = true;
        StandardBtn.interactable = true;
        ReturnOtherButtonsToNormal(OdinsBtn.gameObject);
        AddCardSet("Odin");
        //StandartCard();
        ResetButtonColors();
        //OdinsBtn.image.color = Color.blue;
        UpdateText("Odin Cards");
        HideWarningPanel();
        OpenSelectedCards();
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Odin")
        {
            DeleteDeck();
            SpawnCard("Odin");
        }
    }

    public void ConfirmAnubis()
    {
        AnubisBtn.interactable = false;
        ZeusBtn.interactable = true;
        OdinsBtn.interactable = true;
        GenghisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        DustinBtn.interactable = true;
        StandardBtn.interactable = true;
        ReturnOtherButtonsToNormal(AnubisBtn.gameObject);
        AddCardSet("Anubis");
        //StandartCard();
        ResetButtonColors();
        //AnubisBtn.image.color = Color.blue;
        UpdateText("Anubis Cards");
        HideWarningPanel();
        OpenSelectedCards();
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Anubis")
        {
            DeleteDeck();
            SpawnCard("Anubis");
        }
    }

    public void ConfirmGenghis()
    {
        GenghisBtn.interactable = false;
        ZeusBtn.interactable = true;
        OdinsBtn.interactable = true;
        AnubisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        DustinBtn.interactable = true;
        StandardBtn.interactable = true;
        ReturnOtherButtonsToNormal(GenghisBtn.gameObject);
        AddCardSet("Genghis");
        //StandartCard();
        ResetButtonColors();
        //GenghisBtn.image.color = Color.blue;
        UpdateText("Genghis Cards");
        HideWarningPanel();
        OpenSelectedCards();
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Genghis")
        {
            DeleteDeck();
            SpawnCard("Genghis");
        }
    }

    public void ConfirmDaVinci()
    {
        DaVinciBtn.interactable = false;
        ZeusBtn.interactable = true;
        OdinsBtn.interactable = true;
        AnubisBtn.interactable = true;
        GenghisBtn.interactable = true;
        DustinBtn.interactable = true;
        StandardBtn.interactable = true;
        ReturnOtherButtonsToNormal(DaVinciBtn.gameObject);
        AddCardSet("Leonardo Da Vinci");
        //StandartCard();
        ResetButtonColors();
        //DaVinciBtn.image.color = Color.blue;
        UpdateText("DaVinci Cards");
        HideWarningPanel();
        OpenSelectedCards();
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Leonardo Da Vinci")
        {
            DeleteDeck();
            SpawnCard("Leonardo Da Vinci");
        }
    }

    public void ConfirmDustin()
    {
        DustinBtn.interactable = false;
        ZeusBtn.interactable = true;
        OdinsBtn.interactable = true;
        AnubisBtn.interactable = true;
        GenghisBtn.interactable = true;
        DaVinciBtn.interactable = true;
        StandardBtn.interactable = true;
        ReturnOtherButtonsToNormal(DustinBtn.gameObject);
        AddCardSet("Dustin");
        //StandartCard();
        ResetButtonColors();
        //DustinBtn.image.color = Color.blue;
        UpdateText("Dustin Cards");
        HideWarningPanel();
        OpenSelectedCards();
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Dustin")
        {
            DeleteDeck();
            SpawnCard("Dustin");
        }
    }

    public void DeleteDeck()
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
        _playerDeck.UpdateDeckCount(1);
        _playerDeck.ResetRarityCount();
    }

    private void OpenSelectedCards()
    {
        GameObject panel = GameObject.FindWithTag("CardContainerTag");

        if (panel != null)
        {

            foreach (Transform child in panel.transform)
            {
                SelectedCard selectedCard = child.GetComponent<SelectedCard>();

                if (selectedCard != null)
                {
                    OpenSelected(selectedCard.NameText.text);
                }
            }
        }
    }
     
    public void Reject()
    {
        HideWarningPanel();
    }
    public void Zeus()
    {
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Zeus")
        {
            ShowWarningPanel("You can only select cards belonging to one hero. If you choose Zeus's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmZeus, Reject);
        }
        else
        {
            ConfirmZeus();
        }
    }

    public void Odin()
    {
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Odin")
        {
            ShowWarningPanel("You can only select cards belonging to one hero. If you choose Odin's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmOdin, Reject);
        }
        else
        {
            ConfirmOdin();
        }
        
    }

    public void Anubis()
    {
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Anubis")
        {
            ShowWarningPanel("You can only select cards belonging to one hero. If you choose Anubis's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmAnubis, Reject);
        }
        else
        {
            ConfirmAnubis();
        }
    }

    public void Genghis()
    {
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Genghis")
        {
            ShowWarningPanel("You can only select cards belonging to one hero. If you choose Genghis's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmGenghis, Reject);
        }
        else
        {
            ConfirmGenghis();
        }
    }

    public void DaVinci()
    {
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Leonardo Da Vinci")
        {
            ShowWarningPanel("You can only select cards belonging to one hero. If you choose DaVinci's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmDaVinci, Reject);
        }
        else
        {
            ConfirmDaVinci();
        }
    }

    public void Dustin()
    {
        if (_playerDeck.cardsPanel.transform.GetChild(0).GetComponentInChildren<Text>().text != "Dustin")
        {
            ShowWarningPanel("You can only select cards belonging to one hero. If you choose Dustin's cards, your entire deck will be wiped. Do you agree to this condition?", ConfirmDustin, Reject);
        }
        else
        {
            ConfirmDustin();
        }
    }

    public void Standard()
    {
        StandartCard();
    }
    
    

    void ResetButtonColors()
    {
        ZeusBtn.image.color = Color.white;
        OdinsBtn.image.color = Color.white;
        AnubisBtn.image.color = Color.white;
        GenghisBtn.image.color = Color.white;
        DaVinciBtn.image.color = Color.white;
        DustinBtn.image.color = Color.white;
        StandardBtn.image.color = Color.white;
    }

    public void RemoveSelected(string cardName)
    {
        foreach (Transform child in parentTransform)
        {
            CardDisplay cardDisplay = child.GetComponent<CardDisplay>();
            if (cardDisplay.cardNameText.text == cardName)
            {
                cardDisplay.selectedCardImage.gameObject.SetActive(false);
            }
        }
    }

    public void OpenSelected(string cardName)
    {
        foreach (Transform child in parentTransform)
        {
            CardDisplay cardDisplay = child.GetComponent<CardDisplay>();
            if (cardDisplay.cardNameText.text == cardName)
            {
                cardDisplay.selectedCardImage.gameObject.SetActive(true);
            }
        }
    }
}