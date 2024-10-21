using UnityEngine;
using UnityEngine.UI;

public class EventSystem : MonoBehaviour
{
    public Text HowMuch;
    private int maxMoney = 350;
    private int nowMoney;
    public InventorySystem inventorySystem;
    public NftSystem nftSystem;
    public GameObject InformationCanvas,YouHaveMoreMoneyText;

    public Text Dustin, Odin, Genghis, Zeus, Leonardo, Anubis;

    public void Confirm()
    {
        Debug.Log("here");
        if (nowMoney == maxMoney)
        {
            for (int a = 0; a < inventorySystem.playerCards.Count; a++)
            {
                switch (inventorySystem.playerCards[a].id)
                {
                    case 0:
                        inventorySystem.playerCards[a].hasBeenBought += TextToInt(Genghis);
                        break;
                    case 1:
                        inventorySystem.playerCards[a].hasBeenBought += TextToInt(Zeus);
                        break;
                    case 8:
                        inventorySystem.playerCards[a].hasBeenBought += TextToInt(Odin);
                        break;
                    case 11:
                        inventorySystem.playerCards[a].hasBeenBought += TextToInt(Leonardo);
                        break;
                    case 9:
                        inventorySystem.playerCards[a].hasBeenBought += TextToInt(Dustin);
                        break;
                    case 10:
                        inventorySystem.playerCards[a].hasBeenBought += TextToInt(Anubis);
                        break;
                }
            }
            nftSystem.campaignset(true);
        }
        else
        {
            Debug.Log("You have more money?");
            YouHaveMoreMoneyText.SetActive(true);
        }

    }

    int TextToInt(Text text)
    {
        if (string.IsNullOrEmpty(text.text))
            return 0;

        int result;
        if (int.TryParse(text.text, out result))
        {
            return result;
        }
        else
        {
            Debug.LogError("TextToIntConverter: Metin '" + text.text + "' tamsay?ya d?n??t?r?lemedi.");
            return 0;
        }
    }

    public void CloseInformationCanvas()
    {
        InformationCanvas.SetActive(false);
    }

    public void DustinAdd()
    {
        int currentMoney = int.Parse(Dustin.text);
        int cardMoney = 70;
        if (nowMoney < maxMoney && (nowMoney + cardMoney) <= maxMoney)
        {
            currentMoney++;
            Dustin.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money += cardMoney;
            nowMoney += cardMoney;
            HowMuch.text = Money.ToString();
        }
    }
    public void DustinRemove()
    {
        int currentMoney = int.Parse(Dustin.text);
        int cardMoney = 70;
        if (currentMoney != 0)
        {
            currentMoney--;
            Dustin.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money -= cardMoney;
            nowMoney -= cardMoney;
            HowMuch.text = Money.ToString();
        }
    }

    public void OdinAdd()
    {
        int currentMoney = int.Parse(Odin.text);
        int cardMoney = 10;
        if (nowMoney < maxMoney && (nowMoney + cardMoney) <= maxMoney)
        {
            currentMoney++;
            Odin.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money += cardMoney;
            nowMoney += cardMoney;
            HowMuch.text = Money.ToString();
        }
    }
    public void OdinRemove()
    {
        int currentMoney = int.Parse(Odin.text);
        int cardMoney = 10;
        if (currentMoney != 0)
        {
            currentMoney--;
            Odin.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money -= cardMoney;
            nowMoney -= cardMoney;
            HowMuch.text = Money.ToString();
        }
    }

    public void GenghisAdd()
    {
        int currentMoney = int.Parse(Genghis.text);
        int cardMoney = 50;
        if (nowMoney < maxMoney && (nowMoney + cardMoney) <= maxMoney)
        {
            currentMoney++;
            Genghis.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money += cardMoney;
            nowMoney += cardMoney;
            HowMuch.text = Money.ToString();
        }
    }
    public void GenghisRemove()
    {
        int currentMoney = int.Parse(Genghis.text);
        int cardMoney = 50;
        if (currentMoney != 0)
        {
            currentMoney--;
            Genghis.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money -= cardMoney;
            nowMoney -= cardMoney;
            HowMuch.text = Money.ToString();
        }
    }

    public void ZeusAdd()
    {
        int currentMoney = int.Parse(Zeus.text);
        int cardMoney = 10;
        if (nowMoney < maxMoney && (nowMoney + cardMoney) <= maxMoney)
        {
            currentMoney++;
            Zeus.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money += cardMoney;
            nowMoney += cardMoney;
            HowMuch.text = Money.ToString();
        }
    }
    public void ZeusRemove()
    {
        int currentMoney = int.Parse(Zeus.text);
        int cardMoney = 10;
        if (currentMoney != 0)
        {
            currentMoney--;
            Zeus.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money -= cardMoney;
            nowMoney -= cardMoney;
            HowMuch.text = Money.ToString();
        }
    }

    public void LeonardoAdd()
    {
        int currentMoney = int.Parse(Leonardo.text);
        int cardMoney = 50;
        if (nowMoney < maxMoney && (nowMoney + cardMoney) <= maxMoney)
        {
            currentMoney++;
            Leonardo.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money += cardMoney;
            nowMoney += cardMoney;
            HowMuch.text = Money.ToString();
        }
    }
    public void LeonardoRemove()
    {
        int currentMoney = int.Parse(Leonardo.text);
        int cardMoney = 50;
        if (currentMoney != 0)
        {
            currentMoney--;
            Leonardo.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money -= cardMoney;
            nowMoney -= cardMoney;
            HowMuch.text = Money.ToString();
        }
    }

    public void AnubisAdd()
    {
        int currentMoney = int.Parse(Anubis.text);
        int cardMoney = 10;
        if (nowMoney < maxMoney && (nowMoney + cardMoney) <= maxMoney)
        {
            currentMoney++;
            Anubis.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money += cardMoney;
            nowMoney += cardMoney;
            HowMuch.text = Money.ToString();
        }
    }
    public void AnubisRemove()
    {
        int currentMoney = int.Parse(Anubis.text);
        int cardMoney = 10;
        if (currentMoney != 0)
        {
            currentMoney--;
            Anubis.text = currentMoney.ToString();
            int Money = int.Parse(HowMuch.text);
            Money -= cardMoney;
            nowMoney -= cardMoney;
            HowMuch.text = Money.ToString();
        }
    }

    
}
