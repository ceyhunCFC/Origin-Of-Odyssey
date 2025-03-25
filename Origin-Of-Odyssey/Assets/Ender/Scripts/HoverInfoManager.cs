using System.Collections;
using TMPro;
using UnityEngine;

namespace Ender.Scripts
{
    public class HoverInfoManager : MonoBehaviour
    {
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private GameObject cardParent;
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private string cardName;
        [SerializeField] private string cardDescription;

        [SerializeField] private GameObject uiItemInfoItem, cardUiPrefab;

        public static HoverInfoManager Instance;
        private Coroutine _openInfoCoroutine;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
            DontDestroyOnLoad(this);
        }
        
        public void OpenInfo(HoverInfoType hoverInfoType, string infoTxt, CardDisplay cardDisplay, CardInformation cardInformation)
        {
            _openInfoCoroutine = StartCoroutine(OpenInfoCoroutine(hoverInfoType, infoTxt, cardDisplay, cardInformation));
        }
        
        IEnumerator OpenInfoCoroutine(HoverInfoType hoverInfoType,string infoTxt,CardDisplay cardDisplay, CardInformation cardInformation)
        {
            if (_openInfoCoroutine!=null)
            {
                StopCoroutine(_openInfoCoroutine);
            }
            yield return new WaitForSeconds(3f);
            infoPanel.SetActive(true);
            switch (hoverInfoType)
            {
                case HoverInfoType.UIItem:
                    UIItemInfo(infoTxt);
                    break;
                case HoverInfoType.CardUI:
                    CardUIInfo(cardDisplay);
                    break;
                case HoverInfoType.CardInGame:
                    CardInGameInfo(cardInformation);
                    break;
            }
        }

        private void UIItemInfo(string infoTxt)
        {
            infoText.text = infoTxt;
            uiItemInfoItem.SetActive(true);
            var pos = Input.mousePosition;
            pos.z = 10;
            if (pos.x>Screen.width/2)
            {
                pos.x -= 150;
            }
            else
            {
                pos.x += 150;
            }
            if (pos.y>Screen.height/2)
            {
                pos.y -= 60;
            }
            else
            {
                pos.y += 60;
            }
            print(pos);
            
            uiItemInfoItem.transform.position = pos;
        }
        
        private void CardUIInfo(CardDisplay minion)
        {
            CardDisplay cardDisplay = Instantiate(cardUiPrefab, cardParent.transform).GetComponent<CardDisplay>();
            cardDisplay.cardNameText.text = minion.cardNameText.text;
            cardDisplay.cardRarityText.text = minion.cardRarityText.text;
            cardDisplay.cardAttackText.text = minion.cardAttackText.text;
            cardDisplay.cardHealthText.text = minion.cardHealthText.text;
            cardDisplay.cardManaText.text = minion.cardManaText.text;
            cardDisplay.description.text = minion.description.text;
            string cardNameTxt = cardDisplay.cardNameText.text;
            string imagePath = "CardImages/" + cardNameTxt ;
        
            Sprite sprite = Resources.Load<Sprite>(imagePath);
            if (sprite != null)
            {
                cardDisplay.cardImage.sprite = sprite;
            }
            
            string cardRarity = cardDisplay.cardRarityText.text;
            string imagePath1 = "CardsBorder/" + cardRarity;
            Sprite sprite1 = Resources.Load<Sprite>(imagePath1);
            if (sprite1 != null)
            {
                cardDisplay.cardBorderImage.sprite = sprite1;
            }
            var pos = Input.mousePosition;
            pos.z = 10;
            if (pos.x>Screen.width/2)
            {
                pos.x -= 150;
            }
            else
            {
                pos.x += 150;
            }
            if (pos.y>Screen.height/2)
            {
                pos.y -= 150;
            }
            else
            {
                pos.y += 150;
            }
            print(pos);
            
            cardDisplay.transform.position = pos;
            /*SetCardImage(cardDisplay);
            SetCArdBorderImg(cardDisplay);*/
        }

        private void CardInGameInfo(CardInformation cardInformation)
        {
            CardDisplay cardDisplay = Instantiate(cardUiPrefab, cardParent.transform).GetComponent<CardDisplay>();
            cardDisplay.cardNameText.text = cardInformation.CardName;
            cardDisplay.cardRarityText.text = cardInformation.rarity.ToString();
            cardDisplay.cardAttackText.text = cardInformation.CardDamage.ToString();
            cardDisplay.cardHealthText.text = cardInformation.CardHealth;
            cardDisplay.cardManaText.text = cardInformation.CardMana.ToString();
            cardDisplay.description.text = cardInformation.CardDes;
            string cardNameTxt = cardDisplay.cardNameText.text;
            string imagePath = "CardImages/" + cardNameTxt ;
        
            Sprite sprite = Resources.Load<Sprite>(imagePath);
            if (sprite != null)
            {
                cardDisplay.cardImage.sprite = sprite;
            }
            
            string cardRarity = cardDisplay.cardRarityText.text;
            string imagePath1 = "CardsBorder/" + cardRarity;
            Sprite sprite1 = Resources.Load<Sprite>(imagePath1);
            if (sprite1 != null)
            {
                cardDisplay.cardBorderImage.sprite = sprite1;
            }
            var pos = Input.mousePosition;
            pos.z = 10;
            if (pos.x>Screen.width/2)
            {
                pos.x -= 150;
            }
            else
            {
                pos.x += 150;
            }
            if (pos.y>Screen.height/2)
            {
                pos.y -= 150;
            }
            else
            {
                pos.y += 150;
            }
            print(pos);
            
            cardDisplay.transform.position = pos;
        }
        
        public void CloseInfo()
        {
            if (_openInfoCoroutine!=null)
            {
                StopCoroutine(_openInfoCoroutine);
            }
            uiItemInfoItem.SetActive(false);
            foreach (Transform child in cardParent.transform)
            {
                DestroyImmediate(child.gameObject);
            }

            infoPanel.SetActive(false);
        }
    }
    
    public enum HoverInfoType
    {
        UIItem,
        CardUI,
        CardInGame,
    }
}