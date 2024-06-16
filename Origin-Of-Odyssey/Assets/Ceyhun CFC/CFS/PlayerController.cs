using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.IO;
using System;

public class PlayerController : MonoBehaviour
{
    public PhotonView PV;
    GameManager _GameManager;
    public GameObject CompetitorPV = null;
    CardsFunction _CardFunction;

    public GameObject CardPrefabSolo; // Tek kart
    


    string OwnName = "";
    string[] OwnDeck;
    string OwnMainCard = "";
    float OwnHealth = 0;
    float OwnHeroAttackDamage = 0;


    string CompetitorName = "";
    string[] CompetitorDeck;
    string CompetitorMainCard = "";
    float CompetitorHealth = 0;
    float CompetitorHeroAttackDamage = 0;

    public Text OwnNameText;
    public Text OwnDeckText;
    public Text OwnMainCardText;
    public Text OwnHealthText;
    public Text ManaCountText;

    public Text CompetitorNameText;
    public Text CompetitorDeckText;
    public Text CompetitorMainCardText;
    public Text CompetitorHealthText;
    public Text CompetitorManaCountText;

    public Image OwnHealthBar;
    public Image OwnManaBar;

    public Image CompetitorHealthBar;
    public Image CompetitorManaBar;

    public int DeadMonsterCound = 0;

    int DeckCardCount = 0;
    float Mana = 3;

    GameObject selectedCard;
    GameObject lastHoveredCard = null;
    CardProgress _CardProgress;

    private Vector3 initialCardPosition;

    public Text OwnHeroAttackDamageText;
    public Text CompetitorHeroAttackDamageText;

    public int OlympiaKillCount = 0;
    public int SpellsExtraDamage = 0;

    void Start()
    {
        //_CardFunction = GameObject.Find("GameManager").GetComponent<CardsFunction>();

        PV = GetComponent<PhotonView>();
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _CardProgress = GetComponent<CardProgress>();

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

        if (Input.GetMouseButtonDown(0) && PV.Owner.IsMasterClient && _GameManager.Turn == false)
        {
            SelectCardFromDeck();

        }
        else if (Input.GetMouseButtonDown(0) && !PV.Owner.IsMasterClient && _GameManager.Turn == true)
        {
            SelectCardFromDeck();

        }
        else if (Input.GetMouseButtonDown(0))
        {
            Debug.LogError("IT IS NOT YOUR TURN!");
        }

        if (Input.GetMouseButton(0) && PV.Owner.IsMasterClient && _GameManager.Turn == false)
        {
            if(selectedCard!=null)
            {
                DragCardAfterSelect();
            }
            

        }
        else if (Input.GetMouseButton(0) && !PV.Owner.IsMasterClient && _GameManager.Turn == true)
        {
            if (selectedCard != null)
            {
                DragCardAfterSelect();
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
                ScaleDeckCard();
            }

        }
        else if (!PV.Owner.IsMasterClient && _GameManager.Turn == true && selectedCard == null)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                ScaleDeckCard();
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.LogError("IT IS NOT YOUR TURN!");
        }  


    }

    void ScaleDeckCard()
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

    void SelectCardFromDeck()
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
            else if (hit.collider.gameObject.tag == "UsedCard" && _CardProgress.ForMyCard==false)
            {
                // _CardFunction.SelectFirstCard(hit.collider.gameObject);
                Debug.LogError(hit.collider.gameObject.transform.parent);
                if(hit.collider.gameObject.GetComponent<CardInformation>().CardFreeze == false && hit.collider.gameObject.GetComponent<CardInformation>().isItFirstRaound == false && hit.collider.gameObject.GetComponent<CardInformation>().isAttacked==false)
                {
                    _CardProgress.SetAttackerCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, hit.collider.gameObject.transform.parent.gameObject));
                }
                else
                {
                    Debug.Log("CardFreeze or firstraund or isattacked");
                }
            }

        }
    }

    void DragCardAfterSelect()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0.5f;       //kartın ekrana uzaklığı
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

                int Boxindex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, hit.transform.gameObject);

                

                if(selectedCard.GetComponent<CardInformation>().CardHealth!="")
                {
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
                }

                selectedCard.GetComponent<Renderer>().material.color = Color.white;

                Transform transformBox = hit.collider.gameObject.transform;
                StartCoroutine(MoveAndRotateCard(selectedCard, transformBox.position, 0.3f));
                selectedCard.transform.SetParent(transformBox);

                selectedCard.transform.localScale = Vector3.one;
                selectedCard.transform.localEulerAngles = new Vector3(90, 0, 180);
                selectedCard.transform.localPosition = Vector3.zero;

                Mana -= selectedCard.GetComponent<CardInformation>().CardMana;

                //////////////////////////////////// DESTEDEN BİR KART MASYA EKLENDİĞİ ZAMAN ///////////////////////////////
                
                if (selectedCard.GetComponent<CardInformation>().CardName == "Heracles") // MASAYA EKLENEN KART NEDİR
                {
                    selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + (2 * DeadMonsterCound)).ToString();
                    selectedCard.GetComponent<CardInformation>().CardDamage += (2 * DeadMonsterCound);
                    selectedCard.GetComponent<CardInformation>().SetInformation();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Stormcaller")
                {
                  
                    for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++) // KENDİ DESTEMİZDEKİ KARTLARI TEK TEK ÇAĞIR
                    {
                       
                        if (GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().CardHealth=="")  // ÇAĞIRILAN KARTIN BÜYÜ MÜ OLDUĞU KONTROL ET
                        {
                            SpellsExtraDamage = 1;
                        }
                    }


                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Odyssean Navigator")
                {
                    Debug.LogError("ODYYYYYSEAAANN");

                    //  CreateRandomCard();

                    GameObject spawnedObject = SpawnAndReturnGameObject();

                    if (spawnedObject.GetComponent<CardInformation>().CardHealth=="")
                    {
                        Debug.LogError("ODYYYYYSEAAANN SPEEEELLL YARATTTI ");
                        spawnedObject.GetComponent<CardInformation>().CardMana--;

                    }
                    else
                    {
                        Debug.LogError("ODYYYYYSEAAANN MİNNYOONNNN YARATTTI ");
                    }

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Oracle's Emissary")
                {
                    List<GameObject> OwnSpellCards = new List<GameObject>();

                    for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++) // KENDİ DESTEMİZDEKİ KARTLARI TEK TEK ÇAĞIR
                    {
                        if (GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().CardHealth == "")  // ÇAĞIRILAN KARTIN BÜYÜ MÜ OLDUĞU KONTROL ET
                        {
                            OwnSpellCards.Add(GameObject.Find("Deck").transform.GetChild(i).gameObject);
                        }
                    }

                    
                    if (OwnSpellCards.Count>0)
                    {
                        /*int randomspell = UnityEngine.Random.Range(0, OwnSpellCards.Count);

                       // OwnSpellCards[randomspell].transform.eulerAngles = new Vector3(0,90,0);
                        Debug.LogError(OwnSpellCards[randomspell]);


                       // StartCoroutine(RotateAndRevert(OwnSpellCards[randomspell],randomspell));

                        CallRotateAndRevert(OwnSpellCards[randomspell]);  */

                        GameObject spawnedObject = CreateRandomCard();

                        //CallRotateAndRevert(spawnedObject);   burda nul hatası alıyor bazen bazen almıyor bakılacak


                    }


                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Lightning Forger")
                {

                    if (PV.IsMine)
                    {
                        if (PV.Owner.IsMasterClient)
                        {
                          
                           
                            _GameManager.MasterAddAttackDamage(3);
                            Debug.LogError(_GameManager.MasterAttackDamage);
                          
                        }
                        else
                        {
                          
                         
                            _GameManager.OtherAddAttackDamage(3);
                            Debug.LogError(_GameManager.OtherAttackDamage);

                        }
                    }
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Lightning Bolt")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;
                    CompetitorManaCountText.text = (_GameManager.ManaCount) + "/10".ToString();
                    DeckCardCount--;

                    StackDeck();

                    if (PV.IsMine)
                    {
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                        PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                    }

                    GetComponent<CardProgress>().SecoundTargetCard = true;
                    GetComponent<CardProgress>().AttackerCard = selectedCard;


                    selectedCard.SetActive(false);

                   
                    selectedCard = null;
                    lastHoveredCard = null;
                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                    return;

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Gorgon")
                {

                    GetComponent<CardProgress>().FreezeAllEnemyMinions();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Chimera")
                {

                    GetComponent<CardProgress>().DamageToAlLOtherMinions();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Athena")
                {

                    GetComponent<CardProgress>().FillWithHoplites();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Lightning Storm")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;
                    CompetitorManaCountText.text = (_GameManager.ManaCount) + "/10".ToString();
                    DeckCardCount--;

                    StackDeck();

                    if (PV.IsMine)
                    {
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                        PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                    }

                    GetComponent<CardProgress>().LightningStorm();

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Olympian Favor") 
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;
                    CompetitorManaCountText.text = (_GameManager.ManaCount) + "/10".ToString();
                    DeckCardCount--;

                    StackDeck();

                    if (PV.IsMine)
                    {
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                        PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                    }

                    //  GetComponent<CardProgress>().AttackerCard = selectedCard;

                    GetComponent<CardProgress>().SecoundTargetCard = true;
                    GetComponent<CardProgress>().AttackerCard = selectedCard;
                    GetComponent<CardProgress>().ForMyCard = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Aegis Shield") 
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;
                    CompetitorManaCountText.text = (_GameManager.ManaCount) + "/10".ToString();
                    DeckCardCount--;

                    StackDeck();

                    if (PV.IsMine)
                    {
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                        PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                    }

                    // GetComponent<CardProgress>().LightningStorm();

                    GetComponent<CardProgress>().SecoundTargetCard = true;
                    GetComponent<CardProgress>().AttackerCard = selectedCard;
                    GetComponent<CardProgress>().ForMyCard = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Golden Fleece") 
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;
                    CompetitorManaCountText.text = (_GameManager.ManaCount) + "/10".ToString();
                    DeckCardCount--;

                    StackDeck();

                    if (PV.IsMine)
                    {
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                        PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                    }

                    // GetComponent<CardProgress>().LightningStorm();

                    GetComponent<CardProgress>().SecoundTargetCard = true;
                    GetComponent<CardProgress>().AttackerCard = selectedCard;
                    GetComponent<CardProgress>().ForMyCard = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Labyrinth Maze") 
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;
                    CompetitorManaCountText.text = (_GameManager.ManaCount) + "/10".ToString();
                    DeckCardCount--;

                    StackDeck();

                    if (PV.IsMine)
                    {
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                        PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                    }

                    LabyrinthMaze();

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Divine Ascention") 
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;
                    CompetitorManaCountText.text = (_GameManager.ManaCount) + "/10".ToString();
                    DeckCardCount--;

                    StackDeck();

                    if (PV.IsMine)
                    {
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                        PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                    }

                    // GetComponent<CardProgress>().LightningStorm();

                    GetComponent<CardProgress>().SecoundTargetCard = true;
                    GetComponent<CardProgress>().AttackerCard = selectedCard;
                    GetComponent<CardProgress>().ForMyCard = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Centaur Archer" || selectedCard.GetComponent<CardInformation>().CardName == "Minotaur Warrior" || selectedCard.GetComponent<CardInformation>().CardName == "Pegasus Rider" || selectedCard.GetComponent<CardInformation>().CardName == "Greek Hoplite" || selectedCard.GetComponent<CardInformation>().CardName =="Siren")
                {
                    selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName== "Mongol Messenger")
                {
                    SpawnAndReturnGameObject();
                }




                ManaCountText.text = Mana.ToString() + "/10";
                OwnManaBar.fillAmount = Mana / 10f;
                CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;
                CompetitorManaCountText.text = (_GameManager.ManaCount) + "/10".ToString();

//                selectedCard.GetComponent<CardController>().UsedCard(selectedCard.GetComponent<CardInformation>().CardDamage, PV.Owner.IsMasterClient); // HERO YA DAMAGE VURMA

                selectedCard.tag = "UsedCard";
                DeckCardCount--;

                StackDeck();
               
                if (PV.IsMine)
                {
                    Debug.LogError(Boxindex);
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);

                    if (selectedCard.GetComponent<CardInformation>().CardName == "Heracles")
                    {
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("CreateUsedCard", RpcTarget.All, Boxindex,
                      selectedCard.GetComponent<CardInformation>().CardName,
                      selectedCard.GetComponent<CardInformation>().CardDes,
                     (int.Parse( selectedCard.GetComponent<CardInformation>().CardHealth) + (2 * DeadMonsterCound)).ToString(),
                      selectedCard.GetComponent<CardInformation>().CardDamage + (2 * DeadMonsterCound),
                      selectedCard.GetComponent<CardInformation>().CardMana);
                    }
                    else
                    {
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("CreateUsedCard", RpcTarget.All, Boxindex,
                      selectedCard.GetComponent<CardInformation>().CardName,
                      selectedCard.GetComponent<CardInformation>().CardDes,
                      selectedCard.GetComponent<CardInformation>().CardHealth,
                      selectedCard.GetComponent<CardInformation>().CardDamage,
                      selectedCard.GetComponent<CardInformation>().CardMana);
                    }
                      


                    PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                }

                selectedCard = null;
                lastHoveredCard = null;
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


    
    void CallRotateAndRevert(GameObject RotateCard)
    {

        if (PV.IsMine)
        {

            StartCoroutine(RotateAndRevert(RotateCard));

            if (RotateCard.GetComponent<CardInformation>().CardMana>=4)
            {
                RotateCard.GetComponent<CardController>().AddHealCard(4, !PV.Owner.IsMasterClient);
            }

            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CallRotateAndRevert", RpcTarget.All, RotateCard.GetComponent<CardInformation>().CardName, RotateCard.GetComponent<CardInformation>().CardDes, RotateCard.GetComponent<CardInformation>().CardHealth, RotateCard.GetComponent<CardInformation>().CardDamage, RotateCard.GetComponent<CardInformation>().CardMana);

        }

    }

    [PunRPC]
    void RPC_CallRotateAndRevert(string name, string des, string heatlh, int damage, int mana)
    {
        if (!PV.IsMine)
            return;


        int randomcard = UnityEngine.Random.Range(0, GameObject.Find("CompetitorDeck").transform.childCount);

        GameObject.Find("CompetitorDeck").transform.GetChild(randomcard).GetComponent<CardInformation>().CardName = name;
        GameObject.Find("CompetitorDeck").transform.GetChild(randomcard).GetComponent<CardInformation>().CardHealth = heatlh;
        GameObject.Find("CompetitorDeck").transform.GetChild(randomcard).GetComponent<CardInformation>().CardDamage = damage;
        GameObject.Find("CompetitorDeck").transform.GetChild(randomcard).GetComponent<CardInformation>().CardDes = des;
        GameObject.Find("CompetitorDeck").transform.GetChild(randomcard).GetComponent<CardInformation>().CardMana = mana;
        GameObject.Find("CompetitorDeck").transform.GetChild(randomcard).GetComponent<CardInformation>().SetInformation();


       
        StartCoroutine(RotateAndRevert(GameObject.Find("CompetitorDeck").transform.GetChild(randomcard).gameObject));


    }

    IEnumerator RotateAndRevert(GameObject RotateCard)
    {

        Quaternion originalRotation = RotateCard.transform.rotation;

        // 180 derece döndür
        RotateCard.transform.Rotate(0, 180, 0);

       

        // 2 saniye bekle
        yield return new WaitForSeconds(2);

        // Eski rotasyona geri döndür
        RotateCard.transform.rotation = originalRotation;

    }

    IEnumerator MoveAndRotateCard(GameObject card, Vector3 targetPosition,  float duration)
    {
        Vector3 startPosition = card.transform.position;
        float time = 0f;

        Vector3 targetPlus = targetPosition + new Vector3(0f, 0.4f, 0.8f);

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
                    ManaCountText.text = Mana.ToString() + "/10";


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
    public void CreateUsedCard(int boxindex, string name, string des, string heatlh, int damage, int mana)
    {

        if (PV.IsMine)
        {
            GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[boxindex].transform);
            CardCurrent.tag = "CompetitorCard";

          //  CardCurrent.GetComponent<PhotonView>().ViewID = OwnDeck.Length;
            CardCurrent.transform.localScale = Vector3.one;
            CardCurrent.transform.eulerAngles = new Vector3(90,0,180);

            CardCurrent.GetComponent<CardInformation>().CardName = name;
            CardCurrent.GetComponent<CardInformation>().CardDes = des;
            CardCurrent.GetComponent<CardInformation>().CardHealth = heatlh;
            CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
            CardCurrent.GetComponent<CardInformation>().CardMana = mana;
            CardCurrent.GetComponent<CardInformation>().SetInformation();

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

                OwnHeroAttackDamage = _GameManager.MasterAttackDamage;
                CompetitorHeroAttackDamage = _GameManager.OtherAttackDamage;

                /*OwnHealthText.text = OwnHealth.ToString() + "/30";
                OwnHealthBar.fillAmount = OwnHealth / 30;

                CompetitorHealthText.text = CompetitorHealth.ToString() + "/30";
                CompetitorHealthBar.fillAmount = CompetitorHealth / 30;*/

                RefreshUI(OwnHealth,CompetitorHealth, OwnHeroAttackDamage, CompetitorHeroAttackDamage);
            }
            else
            {
                OwnHealth = _GameManager.OtherHealth;
                CompetitorHealth = _GameManager.MasterHealth;

                OwnHeroAttackDamage = _GameManager.OtherAttackDamage;
                CompetitorHeroAttackDamage = _GameManager.MasterAttackDamage;

                /* OwnHealthText.text = OwnHealth.ToString() + "/30";
                 OwnHealthBar.fillAmount = OwnHealth / 30;

                 CompetitorHealthText.text = CompetitorHealth.ToString() + "/30";
                 CompetitorHealthBar.fillAmount = CompetitorHealth / 30;*/

                RefreshUI(OwnHealth, CompetitorHealth, OwnHeroAttackDamage, CompetitorHeroAttackDamage);
            }
        }

    }


    public void RefreshUI(float OwnHealth, float CompetitorHealth, float OwnAttackDamage, float CompetitiorAttackDamage)
    {
        OwnHealthText.text = OwnHealth.ToString() + "/15";
        OwnHealthBar.fillAmount = OwnHealth / 15;

        OwnHeroAttackDamageText.text = OwnAttackDamage.ToString();



        CompetitorHealthText.text = CompetitorHealth.ToString() + "/15";
        CompetitorHealthBar.fillAmount = CompetitorHealth / 15;

        CompetitorHeroAttackDamageText.text = CompetitiorAttackDamage.ToString();


        CompetitorManaCountText.text = _GameManager.ManaCount + "/10".ToString();
        CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;






        OwnManaBar.fillAmount = Mana / 10;

    }

    public void FinishButton()
    {
        // Find all GameObjects with the specified name
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("UsedCard");
        CompetitorPV = null;

        foreach (var card in AllOwnCards)
        {
            card.GetComponent<CardInformation>().isItFirstRaound = false;
            card.GetComponent<CardInformation>().CardFreeze = false;
            card.GetComponent<CardInformation>().isAttacked = false;
        }

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

        GetComponent<CardProgress>().AttackerCard = null;
        GetComponent<CardProgress>().TargetCard = null;
        GetComponent<CardProgress>().TargetCardIndex = -1;
        GetComponent<CardProgress>().SecoundTargetCard = false;
        GetComponent<CardProgress>().ForMyCard = false;


    }

    public GameObject SpawnAndReturnGameObject()
    {

        if (PV.IsMine)
        {
            if (GameObject.Find("Deck").transform.childCount < 10)
            {
                GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);

                float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                CreateCard(CardCurrent);
                StackDeck();
                StackCompetitorDeck();
                DeckCardCount++;

                CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateRandomCard", RpcTarget.All);

                return CardCurrent;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }


    }

    public GameObject CreateRandomCard() /// BOŞTA
    {
        /*if (PV.IsMine)
        {
            GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);

            float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
            CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
            CreateCard(CardCurrent);
            StackDeck();
            StackCompetitorDeck();
            DeckCardCount++;

            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateRandomCard", RpcTarget.All);
        } */
        if (PV.IsMine)
        {
            if (GameObject.Find("Deck").transform.childCount < 10)
            {
                GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);

                float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                CreateZeusSpell(CardCurrent);
                StackDeck();
                StackCompetitorDeck();
                DeckCardCount++;

                CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateRandomCard", RpcTarget.All);

                return CardCurrent;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    [PunRPC]
    void RPC_CreateRandomCard()
    {
        if (!PV.IsMine)
            return;

        GameObject card = Instantiate(Resources.Load<GameObject>("CompetitorCard"), GameObject.Find("CompetitorDeck").transform);

        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
        card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
       
        StackDeck();
        StackCompetitorDeck();
        DeckCardCount++;


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

        GameObject.Find("CompetitorDeck").transform.position = new Vector3(0.6f - GameObject.Find("Deck").transform.childCount * 0.2f, 1.95f, -0.86f);

    }

    public void DeleteAreaCard(int TargetCardIndex)
    {
        if (PV.IsMine)
        {
           

            GameObject DeadCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[TargetCardIndex].transform.GetChild(0).transform.gameObject;
            Destroy(DeadCard);

            string DeadCardName = DeadCard.GetComponent<CardInformation>().CardName;

            if (DeadCardName == "Centaur Archer" || DeadCardName == "Minotaur Warrior" || DeadCardName == "Siren" || DeadCardName == "Gorgon" || DeadCardName == "Nemean Lion" || DeadCardName == "Chimera") // ÖLEN KART MONSTER MI?
            {
                DeadMonsterCound++;
                Debug.LogError(DeadMonsterCound + " TANE MONSTER CARD ÖLDÜ");
            }
            else if(DeadCardName== "Keshik Cavalry")
            {
                
                Debug.LogError("keshikdead");
            }
            if(GetComponent<CardProgress>().AttackerCard != null)
            {
                if (GetComponent<CardProgress>().AttackerCard.GetComponent<CardInformation>().CardName == "Zeus")
                {
                    OlympiaKillCount++;
                    if (OlympiaKillCount > 4)
                    {
                        GameObject[] AllEnemyCards = GameObject.FindGameObjectsWithTag("UsedCard");

                        if (AllEnemyCards != null && AllEnemyCards.Length > 0)
                        {

                            int randomIndex = UnityEngine.Random.Range(0, AllEnemyCards.Length);
                            GameObject randomCard = AllEnemyCards[randomIndex];
                            randomCard.GetComponent<CardInformation>().DivineSelected = true;
                            int OwnCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, randomCard.transform.parent.gameObject);
                            RefreshMyCard(OwnCardIndex,
                                randomCard.GetComponent<CardInformation>().CardHealth,
                                randomCard.GetComponent<CardInformation>().HaveShield,
                                randomCard.GetComponent<CardInformation>().CardDamage,
                                randomCard.GetComponent<CardInformation>().DivineSelected,
                                randomCard.GetComponent<CardInformation>().FirstTakeDamage,
                                randomCard.GetComponent<CardInformation>().FirstDamageTaken);
                        }

                    }
                }
            }
            
              

            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_DeleteAreaCard", RpcTarget.All, TargetCardIndex);
        }
    }

    [PunRPC]
    void RPC_DeleteAreaCard(int TargetCardIndex)
    {
        if (!PV.IsMine)
            return;


         Destroy(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[TargetCardIndex].transform.GetChild(0).transform.gameObject);

    }

    public void CreateHoplitesCard(int CreateCardIndex)
    {
        if (PV.IsMine)
        {
         
         
            GameObject CardCurrent = Instantiate(Resources.Load<GameObject>("HoplitesCard_Prefab"), GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[CreateCardIndex].transform);
          
            CardCurrent.transform.localScale = Vector3.one;
            CardCurrent.transform.eulerAngles = new Vector3(90, 0, 180);

            CardCurrent.GetComponent<CardInformation>().CardName = "Hoplite";
            CardCurrent.GetComponent<CardInformation>().CardDes = "Hoplitesssss";
            CardCurrent.GetComponent<CardInformation>().CardHealth = 1.ToString();
            CardCurrent.GetComponent<CardInformation>().CardDamage = 1;
            CardCurrent.GetComponent<CardInformation>().CardMana = 1;
            CardCurrent.GetComponent<CardInformation>().SetInformation();


            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateHoplitesCard", RpcTarget.All, CreateCardIndex);
        }
    }

    [PunRPC]
    void RPC_CreateHoplitesCard(int CreateCardIndex)
    {
        if (!PV.IsMine)
            return;


        GameObject CardCurrent = Instantiate(Resources.Load<GameObject>("HoplitesCard_Prefab"), GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[CreateCardIndex].transform);
        CardCurrent.tag = "CompetitorCard";
        CardCurrent.transform.localScale = Vector3.one;
        CardCurrent.transform.eulerAngles = new Vector3(90, 0, 180);

        CardCurrent.GetComponent<CardInformation>().CardName = "Hoplite";
        CardCurrent.GetComponent<CardInformation>().CardDes = "Hoplitesssss";
        CardCurrent.GetComponent<CardInformation>().CardHealth = 1.ToString();
        CardCurrent.GetComponent<CardInformation>().CardDamage = 1;
        CardCurrent.GetComponent<CardInformation>().CardMana = 1;
        CardCurrent.GetComponent<CardInformation>().SetInformation();
    }


    public void RefreshUsedCard(int boxindex, string heatlh)
    {
        if (PV.IsMine)
        {

            GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject.GetComponent<CardInformation>().SetInformation();

            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_RefreshUsedCard", RpcTarget.All, boxindex, heatlh);
        }


    }

    [PunRPC]
    public void RPC_RefreshUsedCard(int boxindex, string heatlh)
    {

        if (PV.IsMine)
        {
            GameObject CardCurrent = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject;
         
            CardCurrent.GetComponent<CardInformation>().CardHealth = heatlh;
            CardCurrent.GetComponent<CardInformation>().SetInformation();

        }
    }

    public void RefreshCompotitorCard(int boxindex, bool firstdamage,bool freeze)
    {
        if (PV.IsMine)
        {

            GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject.GetComponent<CardInformation>().SetInformation();            //ilk damageleri truelama

            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_RefreshCompotitorCard", RpcTarget.All, boxindex, firstdamage,freeze);
        }


    }

    [PunRPC]
    public void RPC_RefreshCompotitorCard(int boxindex, bool firstdamage,bool freeze)
    {

        if (PV.IsMine)
        {
            GameObject CardCurrent = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject;

            CardCurrent.GetComponent<CardInformation>().FirstTakeDamage = firstdamage;
            CardCurrent.GetComponent <CardInformation>().CardFreeze = freeze;
            CardCurrent.GetComponent<CardInformation>().SetInformation();

        }
    }

    public void RefreshMyCard(int boxindex, string heatlh,bool haveshield,int damage,bool divineselected,bool firstdamage,bool firstdamagetaken)
    {
        if (PV.IsMine)
        {

            GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject.GetComponent<CardInformation>().SetInformation();       //kendi kart bilgimi güncelleme

            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_RefreshMyCard", RpcTarget.All, boxindex, heatlh,haveshield,damage,divineselected,firstdamage,firstdamagetaken);
        }
    }

    [PunRPC]
    public void RPC_RefreshMyCard(int boxindex, string heatlh,bool haveshield,int damage,bool divineselected,bool firstdamage,bool firstdamagetaken)
    {
        if (PV.IsMine)
        {
            GameObject CardCurrent = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject;

            CardCurrent.GetComponent<CardInformation>().CardHealth = heatlh;
            CardCurrent.GetComponent<CardInformation>().HaveShield = haveshield;
            CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
            CardCurrent.GetComponent<CardInformation>().DivineSelected=divineselected;
            CardCurrent.GetComponent<CardInformation>().FirstTakeDamage = firstdamage;
            CardCurrent.GetComponent<CardInformation>().FirstDamageTaken = firstdamagetaken;
            CardCurrent.GetComponent<CardInformation>().SetInformation();

        }
    }

    public void LabyrinthMaze()
    {
        CardsAreaCreator _cardsAreaCreator;
        _cardsAreaCreator = GameObject.Find("Area").GetComponent<CardsAreaCreator>();
        for (int i = 7; i < 14; i++)
        {
            GameObject areaCollision = _cardsAreaCreator.BackAreaCollisions[i];
            int childCount = areaCollision.transform.childCount;
            if (childCount > 0)
            {
                if(PV.IsMine) 
                {
                    DeleteAreaCard(i);

                    GameObject deckObject = GameObject.Find("CompetitorDeck");
                    if(deckObject.transform.childCount < 10)
                    {
                        string cardName = areaCollision.transform.GetChild(0).gameObject.GetComponent<CardInformation>().CardName;
                        string cardDes = cardName + " POWWERRRRR!!!";
                        string cardHealth = areaCollision.transform.GetChild(0).gameObject.GetComponent<CardInformation>().CardHealth.ToString();
                        int cardDamage = areaCollision.transform.GetChild(0).gameObject.GetComponent<CardInformation>().CardDamage;
                        int cardMana = areaCollision.transform.GetChild(0).gameObject.GetComponent<CardInformation>().CardMana;


                        GameObject CardCurrent = Instantiate(Resources.Load<GameObject>("CompetitorCard"), GameObject.Find("CompetitorDeck").transform);

                        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz


                        CardCurrent.GetComponent<CardInformation>().CardName = cardName;
                        CardCurrent.GetComponent<CardInformation>().CardDes = cardDes;
                        CardCurrent.GetComponent<CardInformation>().CardHealth = cardHealth;
                        CardCurrent.GetComponent<CardInformation>().CardDamage = cardDamage;
                        CardCurrent.GetComponent<CardInformation>().CardMana = cardMana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();


                        StackDeck();
                        StackCompetitorDeck();
                        DeckCardCount++;

                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_LabyrinthMaze", RpcTarget.Others, cardName, cardDes, cardHealth, cardDamage, cardMana);
                    }
                }
            }
        }
    }

    [PunRPC]
    public void RPC_LabyrinthMaze(string cardName, string cardDes, string cardHealth, int cardDamage, int cardMana)
    {

        if (!PV.IsMine)
            return;

        GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);


        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz

        CardCurrent.GetComponent<CardInformation>().CardName = cardName;
        CardCurrent.GetComponent<CardInformation>().CardDes = cardDes;
        CardCurrent.GetComponent<CardInformation>().CardHealth = cardHealth;
        CardCurrent.GetComponent<CardInformation>().CardDamage = cardDamage;
        CardCurrent.GetComponent<CardInformation>().CardMana = cardMana;
        CardCurrent.GetComponent<CardInformation>().SetInformation();

        StackDeck();
        StackCompetitorDeck();
        DeckCardCount++;
    }

    public void MainCardSpecial()
    {
        if (PV.IsMine)
        {
            GameObject existingObject = GameObject.Find("OwnMainCardGameObject");
            if (existingObject == null)
            {
                GameObject OwnMainCardGameObject = new GameObject("OwnMainCardGameObject");
                OwnMainCardGameObject.AddComponent<CardInformation>();
                OwnMainCardGameObject.AddComponent<CardController>();
                existingObject = OwnMainCardGameObject;
            }

            switch (OwnMainCard)
            {
                
                case "Zeus":
                    if (Mana >= 2)
                    {
                        existingObject.GetComponent<CardInformation>().CardName = "Zeus";
                        existingObject.GetComponent<CardInformation>().CardDamage = _GameManager.OtherAttackDamage;

                        GetComponent<CardProgress>().AttackerCard = existingObject;
                    }
                    break;
                case "Genghis":
                    if (Mana >= 2)
                    {
                        existingObject.GetComponent<CardInformation>().CardName = "Genghis";
                        existingObject.GetComponent<CardInformation>().CardDamage = _GameManager.OtherAttackDamage;

                        GetComponent<CardProgress>().AttackerCard = existingObject;
                    }
                    break;
            }
        }
    }





    [PunRPC]
    public void BeginerFunction() // Tur bize geçtiğinde çalışan fonksion
    {
        GameObject[] AllEnemyCards = GameObject.FindGameObjectsWithTag("UsedCard");

        if (PV.IsMine)
        {
           
            if (_GameManager.TurnCount < 2 ) // ilk turun fon.
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject card = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                    card.tag = "Card";
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

                if(GameObject.Find("Deck").transform.childCount<10)
                {
                    GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                    CardCurrent.tag = "Card";
                    float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                    CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                    CreateCard(CardCurrent);
                    DeckCardCount++;
                }
                
                StackDeck();
                StackCompetitorDeck();
                
                foreach (var card in AllEnemyCards)
                {
                    if (card.GetComponent<CardInformation>().DivineSelected == true)
                    {
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) * 2).ToString();
                        card.GetComponent<CardInformation>().SetInformation();
                        card.GetComponent<CardInformation>().DivineSelected = false;
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);           //seçili kart varsa divine ile ve yaşadıysa bu tur 2x can alır
                        RefreshMyCard(TargetCardIndex, 
                            card.GetComponent<CardInformation>().CardHealth, 
                            card.GetComponent<CardInformation>().HaveShield, 
                            card.GetComponent<CardInformation>().CardDamage, 
                            card.GetComponent<CardInformation>().DivineSelected,
                            card.GetComponent<CardInformation>().FirstTakeDamage,
                            card.GetComponent<CardInformation>().FirstDamageTaken);
                    }
                    if(card.GetComponent<CardInformation>().FirstTakeDamage==false)
                    {
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);            //ilk damage güncellemesi true yapıyor aegis için
                        card.GetComponent<CardInformation>().FirstTakeDamage =true;
                        RefreshMyCard(TargetCardIndex, 
                            card.GetComponent<CardInformation>().CardHealth, 
                            card.GetComponent<CardInformation>().HaveShield, 
                            card.GetComponent<CardInformation>().CardDamage, 
                            card.GetComponent<CardInformation>().DivineSelected,
                            card.GetComponent<CardInformation>().FirstTakeDamage,
                            card.GetComponent<CardInformation>().FirstDamageTaken);
                    }
                }

                if (OlympiaKillCount >= 4)
                {
                    foreach (var Cards in AllEnemyCards)
                    {
                        if (Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Centaur Archer" || Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Minotaur Warrior" || Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Siren" || Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Gorgon" || Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Nemean Lion" || Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Chimera")
                        {
                            Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardHealth = (int.Parse(Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardHealth) + 2).ToString();
                            int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, Cards.transform.parent.gameObject);
                            RefreshMyCard(TargetCardIndex, 
                                Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardHealth,
                                Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().HaveShield, 
                                Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardDamage,
                                Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().DivineSelected,
                                Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().FirstTakeDamage,
                                Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().FirstDamageTaken);
                        }
                    }
                }
                bool stormcallerExists = false;

                foreach (var card in AllEnemyCards)
                {
                    var cardInfo = card.GetComponent<CardInformation>();
                    if (cardInfo != null)
                    {
                        if (cardInfo.CardName == "Stormcaller")
                        {
                            stormcallerExists = true;
                            break;
                        }
                    }
                }
                PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
                if (stormcallerExists)
                {
                    foreach (PlayerController obj in objects)
                    {
                        if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV == this.PV)
                        {
                            // RPC çağrısı yap
                            obj.GetComponent<PlayerController>().PV.RPC("RPC_SetSpellsExtraDamage", RpcTarget.All, 1);
                        }
                    }
                }
                else
                {
                    foreach (PlayerController obj in objects)
                    {
                        if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV != this.PV)
                        {
                            // RPC çağrısı yap
                            obj.GetComponent<PlayerController>().PV.RPC("RPC_SetSpellsExtraDamage", RpcTarget.All,0);
                        }
                    }
                }

            }

        }
        else
        {
            if (_GameManager.TurnCount < 1) // ilk turun fon.
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject card = Instantiate(Resources.Load<GameObject>("CompetitorCard"), GameObject.Find("CompetitorDeck").transform);
                    card.tag = "Card";
                    float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                    card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                    CreateCard(card);
                    StackDeck();
                    StackCompetitorDeck();
                    DeckCardCount++;
                }

                GameObject Herocard = Instantiate(Resources.Load<GameObject>("CompetitorHeoCard"), GameObject.Find("CompetitorHeroPivot").transform);
            }
            else // DIĞER TURLAR
            {
                if (GameObject.Find("CompetitorDeck").transform.childCount < 10)
                {
                    GameObject CardCurrent = Instantiate(Resources.Load<GameObject>("CompetitorCard"), GameObject.Find("CompetitorDeck").transform);
                    CardCurrent.tag = "Card";
                    float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                    CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                    CreateCard(CardCurrent);
                    DeckCardCount++;
                }
                    
                StackDeck();
                StackCompetitorDeck();
            }

        }

        
        Mana = _GameManager.ManaCount;
        ManaCountText.text = Mana.ToString() + "/10";
        StartCoroutine(Refreshcard());

    }

    [PunRPC]
    public void RPC_SetSpellsExtraDamage(int damage)
    {
        SpellsExtraDamage = damage;
    }

    void CreateZeusSpell(GameObject CardCurrent)
    {
        ZeusCard zeusCard = new ZeusCard();


        int spellIndex = UnityEngine.Random.Range(0, zeusCard.spells.Count);
        string targetCardName = zeusCard.spells[spellIndex].name;

        CardCurrent.GetComponent<CardInformation>().CardName = zeusCard.spells[spellIndex].name;
        CardCurrent.GetComponent<CardInformation>().CardDes = zeusCard.spells[spellIndex].name + " POWWERRRRR!!!";
        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
        CardCurrent.GetComponent<CardInformation>().CardMana = zeusCard.spells[spellIndex].mana;
        CardCurrent.GetComponent<CardInformation>().SetInformation();

    }

    void CreateCard(GameObject CardCurrent)
    {
        
        switch (OwnMainCard)
        {
            case "Zeus":
                ZeusCard zeusCard = new ZeusCard();


                int CardIndex = UnityEngine.Random.Range(1, OwnDeck.Length); // 1 DEN BAŞLIYOR ÇÜNKĞ İNDEX 0 HEROMUZ
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

            case "Genghis":
                GenghisCard genghisCard = new GenghisCard();

                int GenghisCardIndex = UnityEngine.Random.Range(1, OwnDeck.Length); // 1 DEN BAŞLIYOR ÇÜNKĞ İNDEX 0 HEROMUZ
                string GenghistargetCardName = OwnDeck[GenghisCardIndex]; // Deste içinden gelen kart isminin miniyon mu buyu mu olduğunu belirle daha sonra özelliklerini getir.

                int GenghistargetIndex = -1;

                for (int i = 0; i < genghisCard.minions.Count; i++)
                  {
                      if (genghisCard.minions[i].name == GenghistargetCardName)
                      {
                          GenghistargetIndex = i;

                          CardCurrent.GetComponent<CardInformation>().CardName = genghisCard.minions[GenghistargetIndex].name;
                          CardCurrent.GetComponent<CardInformation>().CardDes = genghisCard.minions[GenghistargetIndex].name + " POWWERRRRR!!!";
                          CardCurrent.GetComponent<CardInformation>().CardHealth = genghisCard.minions[GenghistargetIndex].health.ToString();
                          CardCurrent.GetComponent<CardInformation>().CardDamage = genghisCard.minions[GenghistargetIndex].attack;
                          CardCurrent.GetComponent<CardInformation>().CardMana = genghisCard.minions[GenghistargetIndex].mana;
                          CardCurrent.GetComponent<CardInformation>().SetInformation();
                          break;
                      }
                  }

                  for (int i = 0; i < genghisCard.spells.Count; i++)
                  {
                      if (genghisCard.spells[i].name == GenghistargetCardName)
                      {
                          GenghistargetIndex = i;
                          CardCurrent.GetComponent<CardInformation>().CardName = genghisCard.spells[GenghistargetIndex].name;
                          CardCurrent.GetComponent<CardInformation>().CardDes = genghisCard.spells[GenghistargetIndex].name + " POWWERRRRR!!!";
                          CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                          CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                          CardCurrent.GetComponent<CardInformation>().CardMana = genghisCard.spells[GenghistargetIndex].mana;
                          CardCurrent.GetComponent<CardInformation>().SetInformation();
                          break;
                      }
                  }
                break;
        }
    }
  

    void StackDeck()
    {
     
        for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++)
        {
            float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz

            GameObject.Find("Deck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
        }

        GameObject.Find("Deck").transform.position = new Vector3(0.6f - GameObject.Find("Deck").transform.childCount * 0.2f, 2.7f, -3.81f);

    }

    void StackCompetitorDeck()
    {

        for (int i = 0; i < GameObject.Find("CompetitorDeck").transform.childCount; i++)
        {
            float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz

            GameObject.Find("CompetitorDeck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
        }

        GameObject.Find("CompetitorDeck").transform.position = new Vector3(0.6f - GameObject.Find("CompetitorDeck").transform.childCount * 0.2f, 1.95f, -0.86f);

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
                    OwnHeroAttackDamage = _GameManager.MasterAttackDamage;

                    OwnNameText.text = OwnName;
                    OwnMainCardText.text = OwnMainCard;
                    OwnHeroAttackDamageText.text = OwnHeroAttackDamage.ToString();

                    for (int i = 0; i < OwnDeck.Length; i++)
                    {
                        OwnDeckText.text += ", " + OwnDeck[i];
                    }
                   
                   



                    CompetitorName = _GameManager.OtherPlayerName;
                    CompetitorDeck = _GameManager.OtherDeck;
                    CompetitorMainCard = _GameManager.OtherrMainCard;
                    CompetitorHeroAttackDamage = _GameManager.OtherAttackDamage;



                    CompetitorNameText.text = CompetitorName;
                    CompetitorMainCardText.text = CompetitorMainCard;
                    CompetitorHeroAttackDamageText.text = CompetitorHeroAttackDamage.ToString();

                    for (int i = 0; i < CompetitorDeck.Length; i++)
                    {
                        CompetitorDeckText.text += ", " + CompetitorDeck[i];
                    }


                    Debug.LogError("Im MasterClient");
                }
                else
                {
                    OwnName = _GameManager.OtherPlayerName;
                    OwnDeck = _GameManager.OtherDeck;
                    OwnMainCard = _GameManager.OtherrMainCard;
                    OwnHeroAttackDamage = _GameManager.OtherAttackDamage;

                    OwnNameText.text = OwnName;
                    OwnMainCardText.text = OwnMainCard;
                    OwnHeroAttackDamageText.text = OwnHeroAttackDamage.ToString();

                    for (int i = 0; i < OwnDeck.Length; i++)
                    {
                        OwnDeckText.text += ", " +  OwnDeck[i];
                    }




                    CompetitorName = _GameManager.MasterPlayerName;
                    CompetitorDeck = _GameManager.MasterDeck;
                    CompetitorMainCard = _GameManager.MasterMainCard;
                    CompetitorHeroAttackDamage = _GameManager.MasterAttackDamage;
                    
                    CompetitorNameText.text = CompetitorName;
                    CompetitorMainCardText.text = CompetitorMainCard;
                    CompetitorHeroAttackDamageText.text = CompetitorHeroAttackDamage.ToString();

                    for (int i = 0; i < CompetitorDeck.Length; i++)
                    {
                        CompetitorDeckText.text += ", " + CompetitorDeck[i];
                    }

                    Debug.LogError("Im OtherClient");
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
                      card.tag = "Card";
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
                        GameObject card = Instantiate(Resources.Load<GameObject>("CompetitorCard"), GameObject.Find("CompetitorDeck").transform);
                        card.tag = "Card";
                        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                        card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                        CreateCard(card);
                        StackDeck();
                        StackCompetitorDeck();
                        DeckCardCount++;
                        

                    }

                    GameObject Herocard = Instantiate(Resources.Load<GameObject>("CompetitorHeoCard"), GameObject.Find("CompetitorHeroPivot").transform);
                }
                
            }
        }
    }
}
