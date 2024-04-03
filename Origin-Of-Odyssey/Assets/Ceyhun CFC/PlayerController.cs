using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.IO;

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


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
            GameObject CompetitorPV = null;

            foreach (GameObject obj in objects)
            {
                if (obj.name == "PlayerController(Clone)")
                {

                    if (obj.GetComponent<PlayerController>().PV != this.PV)
                    {
                        CompetitorPV = obj;
                    }
                }
            }

            if (PV.IsMine)
            {
                if (PV.Owner.IsMasterClient)
                {
                    if (_GameManager.Turn == false)
                    {
                        CreateCard();

                    }

                }
                else
                {
                    if (_GameManager.Turn == true)
                    {
                        CreateCard();
                    }

                }
            }
        }
        
    }

    private void CreateCard()      //tıklanan kartın bilgilerini alıp ekranda oluşturuyor
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Card"))
            {
                foreach (Transform child in hit.collider.transform)
                {
                    if (child.name == "CardInfo")
                    {
                        Vector3 position = new Vector3(0.14f, 0.3f, -1.85f);
                        Quaternion rotation = Quaternion.Euler(42f, 0f, 180f);

                        GameObject CardCurrent = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Card_Prefab"), position, rotation);     //boyutunu değiştirme yok ondan diğerinde büyük oluyor


                        CardCurrent.transform.localScale = new Vector3(0.35f, 0.5f, 0.02f);


                        CardCurrent.transform.position = position;
                        CardCurrent.transform.rotation = rotation;
                        foreach (Transform grandChild in child.transform)
                        {
                            switch (grandChild.name)
                            {
                                case "CardName_Txt":
                                    CardCurrent.GetComponent<CardInformation>().CardName = grandChild.GetComponent<Text>().text;
                                    break;
                                case "CardDes_Txt":
                                    CardCurrent.GetComponent<CardInformation>().CardDes = grandChild.GetComponent<Text>().text;
                                    break;
                                case "CardHealth_Txt":
                                    CardCurrent.GetComponent<CardInformation>().CardHealth = grandChild.GetComponent<Text>().text;
                                    break;
                                case "CardDamage_Txt":
                                    CardCurrent.GetComponent<CardInformation>().CardDamage = grandChild.GetComponent<Text>().text;
                                    break;
                                case "CardMana_Txt":
                                    CardCurrent.GetComponent<CardInformation>().CardMana = grandChild.GetComponent<Text>().text;
                                    break;
                            }
                        }
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
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

        /*
        Vector3 position = new Vector3(0.14f, 0.3f, -1.85f);
        Quaternion rotation = Quaternion.Euler(42f, 0f, 180f);
        
        GameObject CardCurrent = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Card_Prefab"), position, rotation);
        
        CardCurrent.transform.localScale = new Vector3(0.35f, 0.5f, 0.02f);
        
        CardCurrent.transform.position = position;
        CardCurrent.transform.rotation = rotation;
        
        
        /
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
        }    */
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
                }
                if(!PV.Owner.IsMasterClient) 
                {

                    for (int i = 0; i < 3; i++)
                    {
                        GameObject card = Instantiate(CardPrefabSolo);
                        float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                        card.transform.position = new Vector3(xPos, -2.85f, CardPrefabSolo.transform.position.z); // Kartın pozisyonunu ayarlıyoruz
                        card.GetComponent<CardPrefab>().Card1 = OwnDeck[Random.Range(1, OwnDeck.Length)];
                        card.GetComponent<CardPrefab>().SetInformation();
                    }
                }
            }
        }
    }
}
