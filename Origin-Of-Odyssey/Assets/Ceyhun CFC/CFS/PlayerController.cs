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
    GameObject CompetitorPV = null;

    public GameObject CardPrefabSolo; // Tek kart
    


    string OwnName = "";
    string[] OwnDeck;
    string OwnMainCard = "";
    int OwnHealth = 0;


    string CompetitorName = "";
    string[] CompetitorDeck;
    string CompetitorMainCard = "";
    int CompetitorHealth = 0;

    public Text OwnNameText;
    public Text OwnDeckText;
    public Text OwnMainCardText;
    public Text OwnHealthText;


    public Text CompetitorNameText;
    public Text CompetitorDeckText;
    public Text CompetitorMainCardText;
    public Text CompetitorHealthText;

    public Text ManaCountText;
    
    int DeckCardCount = 0;
    int Mana = 3;

    GameObject selectedCard;
    GameObject lastHoveredCard = null;

    private Vector3 initialCardPosition;

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

    private void Update()
    {
        if (!PV.IsMine)
            return;


        /*if (Input.GetMouseButtonDown(0) && PV.Owner.IsMasterClient && _GameManager.Turn==false)
        {
            SelectAndUseCard();
          
        }
        else if (Input.GetMouseButtonDown(0) && !PV.Owner.IsMasterClient && _GameManager.Turn == true)
        {
            SelectAndUseCard();
           
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Debug.LogError("IT IS NOT YOUR TURN!");
        }      */

        if (Input.GetMouseButtonDown(0) && PV.Owner.IsMasterClient && _GameManager.Turn == false)
        {
            selectCard();

        }
        else if (Input.GetMouseButtonDown(0) && !PV.Owner.IsMasterClient && _GameManager.Turn == true)
        {
            selectCard();

        }
        else if (Input.GetMouseButtonDown(0))
        {
            Debug.LogError("IT IS NOT YOUR TURN!");
        }

        if (Input.GetMouseButton(0) && PV.Owner.IsMasterClient && _GameManager.Turn == false)
        {
            if(selectedCard!=null)
            {
                DragCard();
            }
            

        }
        else if (Input.GetMouseButton(0) && !PV.Owner.IsMasterClient && _GameManager.Turn == true)
        {
            if (selectedCard != null)
            {
                DragCard();
            }

        }
        else if (Input.GetMouseButton(0))
        {
            Debug.LogError("IT IS NOT YOUR TURN!");
        }

        if (Input.GetMouseButtonUp(0) && PV.Owner.IsMasterClient && _GameManager.Turn == false)
        {
            if (selectedCard != null)
            {
                ReleaseCard();
            }

        }
        else if (Input.GetMouseButtonUp(0) && !PV.Owner.IsMasterClient && _GameManager.Turn == true)
        {
            if (selectedCard != null)
            {
                ReleaseCard();
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.LogError("IT IS NOT YOUR TURN!");
        }



        if (PV.Owner.IsMasterClient && _GameManager.Turn == false && selectedCard == null)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                scaleCard();
            }

        }
        else if (!PV.Owner.IsMasterClient && _GameManager.Turn == true && selectedCard == null)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                scaleCard();
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.LogError("IT IS NOT YOUR TURN!");
        }  


    }

    void scaleCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Card"))
            {
                
                if (lastHoveredCard != hit.collider.gameObject)
                {
                    
                    if (lastHoveredCard != null)
                    {
                        StopCoroutine("ChangeScale");
                        StartCoroutine(ChangeScale(lastHoveredCard.transform, new Vector3(0.7f, 1f, 0.04f), 0.2f));
                    }

                    lastHoveredCard = hit.collider.gameObject;
                    StartCoroutine(ChangeScale(lastHoveredCard.transform, new Vector3(0.9f, 1.2f, 0.04f), 0.2f));
                }
            }
            else 
            {
                if (lastHoveredCard != null)
                {
                    StopCoroutine("ChangeScale");
                    StartCoroutine(ChangeScale(lastHoveredCard.transform, new Vector3(0.7f, 1f, 0.04f), 0.2f));
                    lastHoveredCard = null; 
                }
            }
        }
        else 
        {
            if (lastHoveredCard != null)
            {
                StopCoroutine("ChangeScale");
                StartCoroutine(ChangeScale(lastHoveredCard.transform, new Vector3(0.7f, 1f, 0.04f), 0.2f));
                lastHoveredCard = null; 
            }
        }
    }


    IEnumerator ChangeScale(Transform target, Vector3 targetScale, float duration)
    {
        float time = 0f;
        Vector3 startScale = target.localScale;

        while (time < duration)
        {
            target.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        target.localScale = targetScale;
    }

    void selectCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Card")
            {

                if (hit.collider.gameObject.GetComponent<CardInformation>().CardMana <= Mana)
                {
                    selectedCard = hit.collider.gameObject;
                    selectedCard.GetComponent<Renderer>().material.color = Color.green;

                    initialCardPosition = selectedCard.transform.position;
                }

            }

        }
    }
    void DragCard()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;       //kartın ekrana uzaklığı
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float followSpeed = 5f; // Kartın takip hızı
        selectedCard.transform.position = Vector3.Lerp(selectedCard.transform.position, targetPosition, Time.deltaTime * followSpeed);
    }

    void ReleaseCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "AreaBox")
            {
                Transform objectBelow = hit.transform;

                foreach (Transform child in objectBelow)
                {
                    if (child.CompareTag("UsedCard"))
                    {
                        selectedCard.GetComponent<Renderer>().material.color = Color.white;
                        selectedCard.transform.position = initialCardPosition;
                        selectedCard.transform.localScale = new Vector3(0.7f, 1f, 0.04f);
                        selectedCard = null;
                        return;
                    }
                }
                selectedCard.GetComponent<Renderer>().material.color = Color.white;

                Transform transformBox = hit.collider.gameObject.transform;
                StartCoroutine(MoveAndRotateCard(selectedCard, transformBox.position, 0.3f));
                selectedCard.transform.SetParent(transformBox);

                selectedCard.transform.localScale = Vector3.one;
                selectedCard.transform.localEulerAngles = new Vector3(90, 0, 180);
                selectedCard.transform.localPosition = Vector3.zero;

                Mana -= selectedCard.GetComponent<CardInformation>().CardMana;
                ManaCountText.text = Mana.ToString();

                selectedCard.GetComponent<CardController>().UsedCard(selectedCard.GetComponent<CardInformation>().CardDamage, PV.Owner.IsMasterClient);

                selectedCard.tag = "UsedCard";
                DeckCardCount--;

                StackDeck();
                selectedCard = null;
                lastHoveredCard = null;

                if (PV.IsMine)
                {
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);

                    PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                }

                return; 
            }
        }
        if (selectedCard != null)
        {
            selectedCard.GetComponent<Renderer>().material.color = Color.white;
            selectedCard.transform.position = initialCardPosition;
            selectedCard.transform.localScale=new Vector3 (0.7f, 1f, 0.04f);
            selectedCard = null;
        }
    }

    IEnumerator MoveAndRotateCard(GameObject card, Vector3 targetPosition,  float duration)
    {
        Vector3 startPosition = card.transform.position;
        float time = 0f;

        Vector3 targetPlus = targetPosition + new Vector3(0f, 0.8f, 0f);

        while (time < duration)
        {
            card.transform.position = Vector3.Lerp(startPosition, targetPlus, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        time = 0f;
        while (time < duration)
        {
            card.transform.position = Vector3.Lerp(targetPlus, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }             

        card.transform.position = targetPosition;
        selectedCard = null;
    }

    void SelectAndUseCard()
    {
        if (selectedCard != null) // CARDIN MASAYA EKLENMESİ VE KULLANILMASI
        {
            // Fare konumunda bir ışın oluştur
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Işının çarptığı nesneyi kontrol et
            if (Physics.Raycast(ray, out hit))
            {
                selectedCard.GetComponent<Renderer>().material.color = Color.white;

                if (hit.collider.gameObject.tag == "AreaBox")
                {
                    Transform transformBox = hit.collider.gameObject.transform;
                    selectedCard.transform.SetParent(transformBox);

                    selectedCard.transform.localScale = Vector3.one;
                    selectedCard.transform.localEulerAngles = new Vector3(90, 0, 180);
                    selectedCard.transform.localPosition = Vector3.zero;


                    Mana -= selectedCard.GetComponent<CardInformation>().CardMana;
                    ManaCountText.text = Mana.ToString();


                    selectedCard.GetComponent<Renderer>().material.color = Color.white;
                    selectedCard.GetComponent<CardController>().UsedCard(selectedCard.GetComponent<CardInformation>().CardDamage, PV.Owner.IsMasterClient);



                    selectedCard.tag = "UsedCard";
                    DeckCardCount--;

                    StackDeck();
                    selectedCard = null;

                   

                    if (PV.IsMine)
                    {
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);

                        PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                    }

                }
                else
                {
                    selectedCard.GetComponent<Renderer>().material.color = Color.white;
                    selectedCard = null;
                }
            }
        }
        else // KULLANILACAK KARTIN SEÇİLMESİ
        {

            // Fare konumunda bir ışın oluştur
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Işının çarptığı nesneyi kontrol et
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Card")
                {

                    if (hit.collider.gameObject.GetComponent<CardInformation>().CardMana <= Mana)
                    {
                        selectedCard = hit.collider.gameObject;
                        selectedCard.GetComponent<Renderer>().material.color = Color.green;
                    }

                }

            }
        }
    }

    [PunRPC]
    public void RefreshPlayersInformation()
    {
        StartCoroutine(Refreshcard());
    }

    IEnumerator Refreshcard()
    {
        yield return new WaitForSeconds(1);

        if (PV.IsMine)
        {
            if (PV.Owner.IsMasterClient)
            {
                OwnHealth = _GameManager.MasterHealth;
                CompetitorHealth = _GameManager.OtherHealth;

                OwnHealthText.text = OwnHealth.ToString();
                CompetitorHealthText.text = CompetitorHealth.ToString();
            }
            else
            {
                OwnHealth = _GameManager.OtherHealth;
                CompetitorHealth = _GameManager.MasterHealth;

                OwnHealthText.text = OwnHealth.ToString();
                CompetitorHealthText.text = CompetitorHealth.ToString();
            }
        }

    }

    public void FinishButton()
    {
        // Find all GameObjects with the specified name
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        CompetitorPV = null;

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
                else
                {
                    Debug.LogError("IT IS NOT YOUR TURN!");
                }

            }
            else
            {
                if (_GameManager.Turn == true)
                {
                    _GameManager.FinishTurn(false);
                    _GameManager.AddMana();
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("BeginerFunction", RpcTarget.All);
                }
                else
                {
                    Debug.LogError("IT IS NOT YOUR TURN!");
                }

            }
        }

     
    }

    [PunRPC]
    public void DeleteCompatitorDeckCard()
    {

        if (!PV.IsMine)
            return;

        Destroy(GameObject.Find("CompetitorDeck").transform.GetChild(GameObject.Find("CompetitorDeck").transform.childCount - 1).gameObject);

        for (int i = 0; i < GameObject.Find("CompetitorDeck").transform.childCount; i++)
        {
            float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz

            GameObject.Find("CompetitorDeck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
        }

        GameObject.Find("CompetitorDeck").transform.position = new Vector3(0.6f - GameObject.Find("Deck").transform.childCount * 0.2f, 0.31f, 4.52f);

    }


    [PunRPC]
    public void BeginerFunction() // Tur bize geçtiğinde çalışan fonksion
    {
       
        if (PV.IsMine)
        {
           
            if (_GameManager.TurnCount < 2 ) // ilk turun fon.
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject card = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                    float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                    card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                    CreateCard(card);
                    StackDeck();
                    StackCompetitorDeck();
                    DeckCardCount++;
                }

               
            }
            else // DIĞER TURLAR
            {
                GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);

                float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                CreateCard(CardCurrent);
                StackDeck();
                StackCompetitorDeck();
                DeckCardCount++;
            }

        }
        else
        {
            if (_GameManager.TurnCount < 1) // ilk turun fon.
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject card = Instantiate(CardPrefabSolo, GameObject.Find("CompetitorDeck").transform);

                    float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                    card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                    CreateCard(card);
                    StackDeck();
                    StackCompetitorDeck();
                    DeckCardCount++;
                }

              
            }
            else // DIĞER TURLAR
            {
                GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("CompetitorDeck").transform);

                float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                CreateCard(CardCurrent);
                StackDeck();
                StackCompetitorDeck();
                DeckCardCount++;
            }
        }
       

        Mana = _GameManager.ManaCount;
        ManaCountText.text = Mana.ToString();

    }

    void CreateCard(GameObject CardCurrent)
    {
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
                        CardCurrent.GetComponent<CardInformation>().CardDamage = zeusCard.minions[targetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = zeusCard.minions[targetIndex].mana;
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
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = zeusCard.spells[targetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                break;

                /*  case "GenghisCard":
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
                      break;*/
        }
    }
  

    void StackDeck()
    {
     
        for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++)
        {
            float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz

            GameObject.Find("Deck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
        }

        GameObject.Find("Deck").transform.position = new Vector3(0.6f - GameObject.Find("Deck").transform.childCount * 0.2f, 0.31f, -2.47f);

    }

    void StackCompetitorDeck()
    {

        for (int i = 0; i < GameObject.Find("CompetitorDeck").transform.childCount; i++)
        {
            float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz

            GameObject.Find("CompetitorDeck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
        }

        GameObject.Find("CompetitorDeck").transform.position = new Vector3(0.6f - GameObject.Find("CompetitorDeck").transform.childCount * 0.2f, 0.31f, 4.52f);

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
                      GameObject card = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                     
                      float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                      card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                      CreateCard(card);
                      StackDeck();
                      StackCompetitorDeck();
                      DeckCardCount++;

                        
                  }

                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject card = Instantiate(CardPrefabSolo, GameObject.Find("CompetitorDeck").transform);

                        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                        card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                        CreateCard(card);
                        StackDeck();
                        StackCompetitorDeck();
                        DeckCardCount++;
                        

                    }
                }
                
            }
        }
    }
}
