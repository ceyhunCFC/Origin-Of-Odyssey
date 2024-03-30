using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    PhotonView PV;
    GameManager _GameManager;
    public GameObject CardPrefab; // üçlü kart 
    public GameObject CardPrefabSolo; // Tek kart
    float gapBetweenCards = 0.8f; // Kartlar arasındaki boşluk
    


    string OwnName = "";
    string[] OwnDeck;
    string OwnMainCard = "";


    string CompetitorName = "";
    string[] CompetitorDeck;
    string CompetitorMainCard = "";

    public Text OwnNameText;
    public Text OwnDeckText;
    public Text OwnMainCardText;
    public Text CompetitorNameText;
    public Text CompetitorDeckText;
    public Text CompetitorMainCardText;

    bool isFirstTurn = true;
    



    void Start()
    {
        PV = GetComponent<PhotonView>();
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        if (!PV.IsMine)
        {
            GetComponent<PlayerController>().transform.GetChild(0).gameObject.SetActive(false);
        }

        GetDataForUI();
    }



    public void FinishButton()
    {
        // Find all GameObjects with the specified name
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        GameObject CompetitorPV = null;

        foreach (GameObject obj in objects)
        {
            if (obj.name == "PlayerController(Clone)")
            {

                if (obj.GetComponent<PlayerController>().PV!=this.PV)
                {
                    CompetitorPV = obj;
                }
            }
        }

        if (PV.IsMine)
        {
            if (PV.Owner.IsMasterClient)
            {
                if (_GameManager.Turn==false)
                {
                    _GameManager.FinishTurn(true);
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("BeginerFunction", RpcTarget.All);
                }

            }
            else
            {
                if (_GameManager.Turn == true)
                {
                    _GameManager.FinishTurn(false);
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("BeginerFunction", RpcTarget.All);

                }

            }
        }

    }



    [PunRPC]
    public void BeginerFunction() // Tur bize geçtiğinde çalışan fonksion
    {

        print(PV.ViewID);

        if (!PV.IsMine)
            return;


        if (isFirstTurn==true) // ilk turun fon.
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject card = Instantiate(CardPrefabSolo);
                float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                card.transform.position = new Vector3(xPos, -2.85f, CardPrefabSolo.transform.position.z); // Kartın pozisyonunu ayarlıyoruz
                card.GetComponent<CardPrefab>().Card1 = OwnDeck[Random.Range(1, OwnDeck.Length)];
                card.GetComponent<CardPrefab>().SetInformation();
            }
            isFirstTurn = false;
        }
        else
        {
            GameObject CardCurrent = Instantiate(CardPrefabSolo);

            switch (OwnMainCard)
            {
                case "ZeusCard":
                    ZeusCard zeusCard = new ZeusCard();
                    int CardIndex = Random.Range(1, OwnDeck.Length);
                    string targetCardName = OwnDeck[CardIndex]; // Deste içinden gelen kart isminin miniyon mu buyu mu olduğunu belirle daha sonra özelliklerini getir.

                    int targetIndex = -1;

                    for (int i = 0; i < zeusCard.minions.Count; i++)
                    {
                        if (zeusCard.minions[i].name == targetCardName)
                        {
                            targetIndex = i;
                            CardCurrent.GetComponent<CardInformation>().CardName = zeusCard.minions[targetIndex].name;
                            CardCurrent.GetComponent<CardInformation>().CardDes = zeusCard.minions[targetIndex].name + " POWWERRRRR!!!";
                            CardCurrent.GetComponent<CardInformation>().CardHealth = zeusCard.minions[targetIndex].health.ToString();
                            CardCurrent.GetComponent<CardInformation>().CardDamage = zeusCard.minions[targetIndex].attack.ToString();
                            CardCurrent.GetComponent<CardInformation>().CardMana = zeusCard.minions[targetIndex].mana.ToString();
                            CardCurrent.GetComponent<CardInformation>().SetInformation();
                            break;
                        }
                    }

                    for (int i = 0; i < zeusCard.spells.Count; i++)
                    {
                        if (zeusCard.spells[i].name == targetCardName)
                        {
                            targetIndex = i;
                            CardCurrent.GetComponent<CardInformation>().CardName = zeusCard.spells[targetIndex].name;
                            CardCurrent.GetComponent<CardInformation>().CardDes = zeusCard.spells[targetIndex].name + " POWWERRRRR!!!";
                            CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                            CardCurrent.GetComponent<CardInformation>().CardDamage = "";
                            CardCurrent.GetComponent<CardInformation>().CardMana = zeusCard.spells[targetIndex].mana.ToString();
                            CardCurrent.GetComponent<CardInformation>().SetInformation();
                            break;
                        }
                    }
                    break;

                case "GenghisCard":
                    GenghisCard genghisCard = new GenghisCard();
                    int CardIndex1 = Random.Range(1, OwnDeck.Length);
                    string targetCardName1 = OwnDeck[CardIndex1];
                   
                    int targetIndex1 = -1;


                    for (int i = 0; i < genghisCard.minions.Count; i++)
                    {
                        if (genghisCard.minions[i].name == targetCardName1)
                        {
                            targetIndex = i;
                            CardCurrent.GetComponent<CardInformation>().CardName = genghisCard.minions[targetIndex].name;
                            CardCurrent.GetComponent<CardInformation>().CardDes = genghisCard.minions[targetIndex].name + " POWWERRRRR!!!";
                            CardCurrent.GetComponent<CardInformation>().CardHealth = genghisCard.minions[targetIndex].health.ToString();
                            CardCurrent.GetComponent<CardInformation>().CardDamage = genghisCard.minions[targetIndex].attack.ToString();
                            CardCurrent.GetComponent<CardInformation>().CardMana = genghisCard.minions[targetIndex].mana.ToString();
                            CardCurrent.GetComponent<CardInformation>().SetInformation();
                            break;
                        }
                    }

                    for (int i = 0; i < genghisCard.spells.Count; i++)
                    {
                        if (genghisCard.spells[i].name == targetCardName1)
                        {
                            targetIndex = i;
                            CardCurrent.GetComponent<CardInformation>().CardName = genghisCard.spells[targetIndex].name;
                            CardCurrent.GetComponent<CardInformation>().CardDes = genghisCard.spells[targetIndex].name + " POWWERRRRR!!!";
                            CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                            CardCurrent.GetComponent<CardInformation>().CardDamage = "";
                            CardCurrent.GetComponent<CardInformation>().CardMana = genghisCard.spells[targetIndex].mana.ToString();
                            CardCurrent.GetComponent<CardInformation>().SetInformation();
                            break;
                        }
                    }
                    break;
            }

        }
    }

    void GetDataForUI()
    {
        if (OwnName == "" || OwnDeck == null || CompetitorName == "" || CompetitorDeck == null)
        {
            if (PV.IsMine)
            {
                if (PV.Owner.IsMasterClient)
                {
                    OwnName = _GameManager.MasterPlayerName;
                    OwnDeck = _GameManager.MasterDeck;
                    OwnMainCard = _GameManager.MasterMainCard;


                    OwnNameText.text = OwnName;
                    OwnMainCardText.text = OwnMainCard;

                    for (int i = 0; i < OwnDeck.Length; i++)
                    {
                        OwnDeckText.text += ", " + OwnDeck[i];
                    }
                   




                    CompetitorName = _GameManager.OtherPlayerName;
                    CompetitorDeck = _GameManager.OtherDeck;
                    CompetitorMainCard = _GameManager.OtherrMainCard;



                    CompetitorNameText.text = CompetitorName;
                    CompetitorMainCardText.text = CompetitorMainCard;

                    for (int i = 0; i < CompetitorDeck.Length; i++)
                    {
                        CompetitorDeckText.text += ", " + CompetitorDeck[i];
                    }


                    print("Im MasterClient");
                }
                else
                {
                    OwnName = _GameManager.OtherPlayerName;
                    OwnDeck = _GameManager.OtherDeck;
                    OwnMainCard = _GameManager.OtherrMainCard;


                    OwnNameText.text = OwnName;
                    OwnMainCardText.text = OwnMainCard;

                    for (int i = 0; i < OwnDeck.Length; i++)
                    {
                        OwnDeckText.text += ", " +  OwnDeck[i];
                    }




                    CompetitorName = _GameManager.MasterPlayerName;
                    CompetitorDeck = _GameManager.MasterDeck;
                    CompetitorMainCard = _GameManager.MasterMainCard;
                    
                    CompetitorNameText.text = CompetitorName;
                    CompetitorMainCardText.text = CompetitorMainCard;

                    for (int i = 0; i < CompetitorDeck.Length; i++)
                    {
                        CompetitorDeckText.text += ", " + CompetitorDeck[i];
                    }

                    print("Im OtherClient");
                }
            }

            Invoke("GetDataForUI", 1);
        }
        else // GAME STARTER WİTH MASTER CLIENT
        {
            if (PV.IsMine)
            {
                if (PV.Owner.IsMasterClient)
                {
                  for (int i = 0; i < 3; i++)
                  {
                      GameObject card = Instantiate(CardPrefabSolo);
                      float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                      card.transform.position = new Vector3(xPos, -2.85f, -1.92f); // Kartın pozisyonunu ayarlıyoruz
                      card.GetComponent<CardPrefab>().Card1 = OwnDeck[Random.Range(1, OwnDeck.Length)];
                      card.GetComponent<CardPrefab>().SetInformation();
                  }
                  isFirstTurn = false;
                }
            }
        }
    }
}
