using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayerController : MonoBehaviour
{
   string OwnName = "";
    public string[] OwnDeck;
    string OwnMainCard = "";
    float OwnHealth = 0;
    float OwnHeroAttackDamage = 0;


    string CompetitorName = "";
    string[] CompetitorDeck;
    string CompetitorMainCard = "";
    public float CompetitorHealth = 0;
    float CompetitorHeroAttackDamage = 0;

    public Text OwnNameText;
    public Text OwnHealthText;
    public Text ManaCountText;
    public Text WhoseTurnText;
    public Text Timer;

    public Text CompetitorNameText;
    public Text CompetitorDeckText;
    public Text CompetitorMainCardText;
    public Text CompetitorHealthText;
    public Text CompetitorManaCountText;


    public GameObject DamageText;
    public GameObject LogsPrefab;
    public Button finishButton;
    GameObject LogsContainerContent;

    public Image OwnHealthBar;
    public Image OwnManaBar;

    public Image CompetitorHealthBar;
    public Image CompetitorManaBar;

    public int DeadMonsterCound = 0;

    int DeckCardCount = 0;

    int CompetitorDeckCardCount = 0;
    public float Mana = 3;
    float elapsedTime = 60;

    GameObject selectedCard;
    GameObject lastHoveredCard = null;

    private Vector3 initialCardPosition;

    public Text OwnHeroAttackDamageText;
    public Text CompetitorHeroAttackDamageText;

    public int OlympiaKillCount = 0;
    public int SpellsExtraDamage = 0;
    public bool GodsBaneUsed = false;
    public bool SteppeAmbush = false;
    public bool NomadicTactics = false;
    public int NomadsLand = 0;

    int FirstHealthCount=0;

    public GameObject CardPrefabSolo,CardPrefabInGame; 

    float TurnCount = 1;

    TutorialCardProgress _TutorialCardProgress;

    void Awake()
    {
        _TutorialCardProgress = GetComponent<TutorialCardProgress>();
    }

    void Start()
    {
        SetUIData();

        for (int i = 0; i < 3; i++)
        {
           CreateAnCard();
        }


    }

    void Update()
    {
        if(finishButton.interactable==true)
        {
            elapsedTime -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);

            Timer.text = string.Format("{0:00}:{1:00}", 00, seconds);

            if (elapsedTime <= 0f)
            {
                elapsedTime = 60;
                Timer.text = "00:60";
                Debug.Log("Countdown finished!");
                FinishButton();
            }

        }
        else
            return;


        if (Input.GetMouseButtonDown(0) && Mana >0)
        {
            SelectCardFromDeck();

        }

        if (Input.GetMouseButton(0))
        {
            if(selectedCard!=null)
            {
                DragCardAfterSelect();
            }
            

        }


        if (Input.GetMouseButtonUp(0))
        {
            if (selectedCard != null)
            {
                ReleaseCard();
            }

        }
    }

     public void CreateHoplitesCard(int CreateCardIndex)
    {
        
         
            GameObject CardCurrent = Instantiate(Resources.Load<GameObject>("HoplitesCard_Prefab"), GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[CreateCardIndex].transform);

            CardCurrent.transform.localScale = new Vector3(1, 1, 0.04f);
            CardCurrent.transform.eulerAngles = new Vector3(45, 0, 180);
            CardCurrent.transform.localPosition = Vector3.zero;

            CardCurrent.GetComponent<CardInformation>().CardName = "Hoplite";
            CardCurrent.GetComponent<CardInformation>().CardDes = "Hoplitesssss";
            CardCurrent.GetComponent<CardInformation>().CardHealth = 1.ToString();
            CardCurrent.GetComponent<CardInformation>().CardDamage = 1;
            CardCurrent.GetComponent<CardInformation>().CardMana = 1;
            CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
            CardCurrent.GetComponent<CardInformation>().SetInformation();


           
        
    }

    public void DeleteAreaCard(int TargetCardIndex)
    {
        

            GameObject DeadCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[TargetCardIndex].transform.GetChild(0).transform.gameObject;
            Destroy(DeadCard);

            string DeadCardName = DeadCard.GetComponent<CardInformation>().CardName;

            if (DeadCardName == "Centaur Archer" || DeadCardName == "Minotaur Warrior" || DeadCardName == "Siren" || DeadCardName == "Gorgon" || DeadCardName == "Nemean Lion" || DeadCardName == "Chimera") // ÖLEN KART MONSTER MI?
            {
                DeadMonsterCound++;
                Debug.LogError(DeadMonsterCound + " TANE MONSTER CARD ÖLDÜ");

                GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "a total of " + DeadMonsterCound + "monsters are dead.";
            }
            else if(DeadCardName== "Keshik Cavalry")
            {
                
                Debug.LogError("keshikdead");

                CreateSpecialCard("Keshik on Foot", "2", 2, 0, TargetCardIndex,false);

                GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "Keshik Cavalry Dead!";
            }
            else if(DeadCardName== "Flaming Camel")
            {
                _TutorialCardProgress.FlamingCamel();
            }
            else if(DeadCardName== "Steppe Warlord")
            {
                _TutorialCardProgress.SteppeWarlord();
            }
            if(_TutorialCardProgress.AttackerCard != null)
            {
                if (_TutorialCardProgress.AttackerCard.GetComponent<CardInformation>().CardName == "Zeus")
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
                          
                        }

                    }
                }
                else if(_TutorialCardProgress.AttackerCard.GetComponent<CardInformation>().CardName == "Mongol Lancer")
                {
                    if (TargetCardIndex > 6 && TargetCardIndex <14)
                    {
                        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard");

                        foreach (GameObject target in allTargets)
                        {
                            if (target != DeadCard)
                            {
                                float distance = Vector3.Distance(DeadCard.transform.position, target.transform.position);

                                if (distance <= 0.55f)
                                {
                                    Vector3 directionToTarget = (target.transform.position - DeadCard.transform.position).normalized;

                                    float dotProductBackward = Vector3.Dot(-DeadCard.transform.up, directionToTarget);

                                    if (dotProductBackward > 0.5f)
                                    {
                                        Debug.Log("Arkasında kart var");
                                        target.GetComponent<CardInformation>().CardHealth =(int.Parse(target.GetComponent<CardInformation>().CardHealth) - _TutorialCardProgress.AttackerCard.GetComponent<CardInformation>().CardDamage).ToString();
                                        int index= Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions,target.transform.parent.gameObject);
                                      //  RefreshUsedCard(index, target.GetComponent<CardInformation>().CardHealth, target.GetComponent<CardInformation>().CardDamage);
                                        CreateTextAtTargetIndex(index, _TutorialCardProgress.AttackerCard.GetComponent<CardInformation>().CardDamage, false);
                                        RefreshLog(-_TutorialCardProgress.AttackerCard.GetComponent<CardInformation>().CardDamage, true, _TutorialCardProgress.AttackerCard.GetComponent<CardInformation>().CardName, target.GetComponent<CardInformation>().CardName, Color.red);
                                        if (int.Parse(target.GetComponent<CardInformation>().CardHealth) <= 0)
                                        {
                                            DeleteAreaCard(index);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            
              

           
        
    }

    public void DeleteMyCard(int TargetCardIndex)
    {
       
        GameObject DeadCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[TargetCardIndex].transform.GetChild(0).transform.gameObject;
        Destroy(DeadCard);

        
    }

     public void CreateTextAtTargetIndex(int targetIndex, int damage,bool mycard)
    {
        Transform targetTransform;
        if (mycard)
        {
            targetTransform = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[targetIndex].transform;
        }
        else
        {
            targetTransform = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[targetIndex].transform;
        }
        

        GameObject damageTextObject = Instantiate(DamageText);
        damageTextObject.transform.parent = targetTransform;
        damageTextObject.transform.localPosition = new Vector3(0, 3, 0);
        damageTextObject.transform.localRotation = Quaternion.Euler(5, 0, 0);
        damageTextObject.transform.localScale = new Vector3(0.01f, 0.05f, 0.01f);
        Text textComponent = damageTextObject.GetComponentInChildren<Text>();
        textComponent.text = "-" + damage.ToString();
        Destroy(damageTextObject, 3f);

    }

    public void SetMana(GameObject attackercard)
    {
        if(attackercard.GetComponent<CardInformation>().CardName=="Zeus" || attackercard.GetComponent<CardInformation>().CardName == "Genghis")
        {
            Mana -= attackercard.GetComponent<CardInformation>().CardMana;

            ManaCountText.text = Mana.ToString() + "/10";
            OwnManaBar.fillAmount = Mana / 10f;
            CompetitorManaBar.fillAmount = TurnCount / 10;
            CompetitorManaCountText.text = TurnCount + "/10".ToString();
        }
    }

    public void CreateTextHero(int damage)
    {
        GameObject competitorHeroPivot = GameObject.Find("CompetitorHeroPivot");

        // CompetitorHeroPivot'un ilk çocuğunu bul
        if (competitorHeroPivot != null && competitorHeroPivot.transform.childCount > 0)
        {
            Transform firstChild = competitorHeroPivot.transform.GetChild(0);

            // DamageText nesnesini oluştur
            GameObject damageTextObject = Instantiate(DamageText);

            // DamageText nesnesini ilk çocuğun içine yerleştir
            damageTextObject.transform.SetParent(firstChild);

            // DamageText nesnesinin yerel pozisyonunu ve dönüşünü ayarla (gerekirse)
            damageTextObject.transform.localPosition = new Vector3(-1,0,0);
            damageTextObject.transform.localRotation = Quaternion.Euler(-20, 180, 180);
            damageTextObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            // X ekseninde -1 çevir
            Vector3 localScale = damageTextObject.transform.localScale;
            localScale.x = -1 * localScale.x;
            damageTextObject.transform.localScale = localScale;

            Debug.Log("DamageText successfully created in the first child of CompetitorHeroPivot.");

            Text textComponent = damageTextObject.GetComponentInChildren<Text>();
            textComponent.text = "-" + damage.ToString();
            Destroy(damageTextObject, 3f);
        }
        else
        {
            Debug.LogWarning("CompetitorHeroPivot not found or it has no children.");
        }
    }


    public void RefreshLog(int damage, bool isdead, string attackercardname, string targetcardname, Color color)
    {
        GameObject logsObject = Instantiate(LogsPrefab, LogsContainerContent.transform);
     
        // Damage text objesini bul ve değerini güncelle
        Text logsDamageText = logsObject.transform.Find("Damage").GetComponent<Text>();
        if(damage!=0)
        {
            logsDamageText.text = damage.ToString();
        }
        else
        {
            logsDamageText.text = null;
        }
        

        // Dead objesini bul ve gerekli işlemi yap
        Transform deadObject = logsObject.transform.Find("Dead");
        if (deadObject != null && isdead)
        {
            deadObject.gameObject.SetActive(true);
            Image deadImage = deadObject.GetComponent<Image>();
            deadImage.color = color;
        }
        else if (deadObject == null)
        {
            Debug.LogWarning("Dead object not found in LogsPrefab.");
        }

        // Attacker ve Target objelerini bul ve resimlerini yükle
        Transform attackerObject = logsObject.transform.Find("Attacker");
        Transform targetObject = logsObject.transform.Find("Target");

        if (attackerObject != null)
        {
            Sprite foundSprite = Resources.Load<Sprite>("CardImages/" + attackercardname);

            if (foundSprite == null)
            {
                Debug.LogWarning("Sprite not found: " + attackercardname);
                foundSprite = Resources.Load<Sprite>("CardImages/" + "Centaur Archer");
            }

            Image attackerImage = attackerObject.GetComponent<Image>();
            if (attackerImage != null)
            {
                attackerImage.sprite = foundSprite;
            }
        }
        else
        {
            Debug.LogWarning("Attacker object not found in LogsPrefab.");
        }

        if (targetObject != null)
        {
            Sprite foundSprite = Resources.Load<Sprite>("CardImages/" + targetcardname);

            if (foundSprite == null)
            {
                Debug.LogWarning("Sprite not found: " + targetcardname);
                foundSprite = Resources.Load<Sprite>("CardImages/" + "Centaur Archer");
            }

            Image targetImage = targetObject.GetComponent<Image>();
            if (targetImage != null)
            {
                targetImage.sprite = foundSprite;
            }
        }
        else
        {
            Debug.LogWarning("Target object not found in LogsPrefab.");
        }

        // Rakip oyuncular için RPC çağrısı yap
    }


    
    public GameObject SpawnAndReturnGameObject()
    {

       if (GameObject.Find("Deck").transform.childCount < 10)
            {
                GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);

                float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                CreateInfoCard(CardCurrent);
                StackOwnDeck();
                DeckCardCount++;


                return CardCurrent;
            }
            else
            {
                return null;
            }

    }

    public GameObject CreateAreaCard(int boxindex, string name, string des, string heatlh, int damage, int mana)
    {
        GameObject CardCurrent = Instantiate(CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform);
        CardCurrent.tag = "UsedCard";

        CardCurrent.GetComponent<CardInformation>().CardName = name;
        CardCurrent.GetComponent<CardInformation>().CardDes = des;
        CardCurrent.GetComponent<CardInformation>().CardHealth = heatlh;
        CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
        CardCurrent.GetComponent<CardInformation>().CardMana = mana;
        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
        CardCurrent.GetComponent<CardInformation>().SetInformation();

        return CardCurrent;
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
        StackOwnDeck();
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


    public GameObject CreateRandomCard() /// BOŞTA
    {
       
        if (GameObject.Find("Deck").transform.childCount < 10)
            {
                GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);

                float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                CreateZeusSpell(CardCurrent);
                StackOwnDeck();
                DeckCardCount++;

                return CardCurrent;
            }
            else
            {
                return null;
            }
    }


     void CreateSpecialCard(string name, string health, int damage,int mana,int index,bool front)
    {
        
            GameObject CardCurrent;
            if(front)
            {
                CardCurrent = Instantiate(CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[index].transform);
                CardCurrent.tag = "UsedCard";
            }
            else
            {
                CardCurrent = Instantiate(CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[index].transform);
                CardCurrent.tag = "CompetitorCard";
            }


            CardCurrent.transform.localPosition = Vector3.zero;
            CardCurrent.transform.localEulerAngles = new Vector3(45, 0, 180);
            CardCurrent.transform.localScale = new Vector3(1, 1, 0.04f);
            CardCurrent.GetComponent<CardInformation>().CardName = name;
            CardCurrent.GetComponent<CardInformation>().CardHealth = health;
            CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
            CardCurrent.GetComponent<CardInformation>().CardMana = mana;
            CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
            CardCurrent.GetComponent<CardInformation>().SetInformation();

        
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
                
                    //DeleteAreaCard(i);

                    

                    GameObject deckObject = GameObject.Find("CompetitorDeck");
                    if(deckObject.transform.childCount < 10)
                    {
               
                        GameObject CardCurrent = Instantiate(Resources.Load<GameObject>("TutorialCompetitorCard"), GameObject.Find("CompetitorDeck").transform);

                        float xPos = CompetitorDeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz


                      
                      
                        StackCompetitorDeck();
                        CompetitorDeckCardCount++;

                    }
                
            }
        }
    }


     public void SelectedCard(GameObject Card)
    {
        GameObject CardCurrent= Instantiate(Card, GameObject.Find("Deck").transform);

        CardCurrent.name = Card.name;
        CardCurrent.tag = "Card";
        CardCurrent.transform.localEulerAngles = new Vector3(45, 0, 180);
        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
        StackOwnDeck();
        StackCompetitorDeck();
        DeckCardCount++;

        GameObject[] SelectCard = GameObject.FindGameObjectsWithTag("SelectCard");
        foreach (GameObject cardObj in SelectCard)
        {
            Destroy(cardObj);
        }

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
                GameObject TemporaryObject = selectedCard;
                selectedCard = CreateAreaCard(Boxindex, selectedCard.GetComponent<CardInformation>().CardName, selectedCard.GetComponent<CardInformation>().CardDes, selectedCard.GetComponent<CardInformation>().CardHealth, selectedCard.GetComponent<CardInformation>().CardDamage, selectedCard.GetComponent<CardInformation>().CardMana);
                Destroy(TemporaryObject);
                StartCoroutine(MoveAndRotateCard(selectedCard, transformBox.position, 0.3f));
                selectedCard.transform.SetParent(transformBox);
                selectedCard.transform.localPosition = Vector3.zero;
                selectedCard.transform.localScale = new Vector3(1, 1, 0.04f);
                selectedCard.transform.localEulerAngles = new Vector3(45f, 0f, 180);

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

                        GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                        TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "Odyssean Navigator created a spell..";

                        spawnedObject.GetComponent<CardInformation>().CardMana--;

                    }
                    else
                    {
                        Debug.LogError("ODYYYYYSEAAANN MİNNYOONNNN YARATTTI ");

                        GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                        TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "Odyssean Navigator created a minion..";
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

                    OwnHeroAttackDamage+=3;
                  
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Lightning Bolt")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);


                    selectedCard.SetActive(false);

                   
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");

                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";

                    return;

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Gorgon")
                {

                    _TutorialCardProgress.FreezeAllEnemyMinions();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Chimera")
                {

                    _TutorialCardProgress.DamageToAlLOtherMinions();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Athena")
                {

                    _TutorialCardProgress.FillWithHoplites();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Lightning Storm")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.LightningStorm();

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                      GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Olympian Favor") 
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    

                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.ForMyCard = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                      GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Aegis Shield") 
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    

                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.ForMyCard = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                      GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Golden Fleece") 
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    

                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.ForMyCard = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                      GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Labyrinth Maze") 
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    

                    LabyrinthMaze();

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                      GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Divine Ascention") 
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    

                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.ForMyCard = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                      GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Centaur Archer" 
                    || selectedCard.GetComponent<CardInformation>().CardName == "Minotaur Warrior" 
                    || selectedCard.GetComponent<CardInformation>().CardName == "Pegasus Rider" 
                    || selectedCard.GetComponent<CardInformation>().CardName == "Greek Hoplite" 
                    || selectedCard.GetComponent<CardInformation>().CardName =="Siren"                           //oynandığı tur saldırabilenler
                    || selectedCard.GetComponent<CardInformation>().CardName== "Mongol Lancer"
                    || selectedCard.GetComponent<CardInformation>().CardName == "Keshik Cavalry")
                {
                    selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
                    Transform childTransform = selectedCard.transform;
                    Transform green = childTransform.Find("Green");
                    green.gameObject.SetActive(true);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName== "Mongol Messenger")       //genghis here
                {
                    GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
                    foreach (GameObject card in mycards)
                    {
                        if (card.GetComponent<CardInformation>().CardName == "Steppe Warlord")
                        {
                            Debug.Log("SteppeWorld var can arttırıldı");
                            selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                            selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                            selectedCard.GetComponent<CardInformation>().SetInformation();
                           
                        }
                    }
                    SpawnAndReturnGameObject();
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName== "Mongol Archer")
                {
                    GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
                    foreach (GameObject card in mycards)
                    {
                        if (card.GetComponent<CardInformation>().CardName == "Steppe Warlord")
                        {
                            Debug.Log("SteppeWorld var can arttırıldı");
                            selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                            selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                            selectedCard.GetComponent<CardInformation>().SetInformation();
                          
                        }
                    }
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    int[] validIndices = { 0, 6, 7, 13 };

                    if (Array.Exists(validIndices, element => element == index))
                    {
                        Debug.Log("Selected Mongol Archer card is at a valid index: " + index);
                        selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                    }
                    else
                    {
                        Debug.Log("Selected Mongol Archer card is not at a valid index.");
                    }


                    selectedCard.GetComponent<CardInformation>().SetInformation();
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName== "Mongol Shaman")
                {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.ForHealMongolShaman = true;
                    _TutorialCardProgress.OpenMyCardSign();
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName=="Eagle Hunter")
                {
                    List<int> emptyCells = new List<int>();

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyCells.Add(i);
                        }
                    }
                    if (emptyCells.Count > 0)
                    {
                        int randomIndex = emptyCells[UnityEngine.Random.Range(0, emptyCells.Count)];

                        CreateSpecialCard("Eagle", "1", 2, 0, randomIndex, true);
                    }
                    else
                    {
                        Debug.LogWarning("No empty cells available to place the Eagle card.");
                    }
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName== "Yurt Builder")
                {
                     Mana += 1;

                        ManaCountText.text = Mana.ToString() + "/10";
                        OwnManaBar.fillAmount = Mana / 10f;
                        CompetitorManaBar.fillAmount = TurnCount / 10;
                        CompetitorManaCountText.text = (TurnCount) + "/10".ToString();

                        Debug.Log("Mana bir arttırıldı");
                   
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Marco Polo")
                {
                    if (GameObject.Find("Deck").transform.childCount < 10)
                    {
                        Vector3[] positions = new Vector3[3];
                        float yPosition = 2.7f;
                        float zPosition = -2.7f;

                        Vector3 screenPos1 = new Vector3(1f, yPosition, zPosition);
                        Vector3 screenPos2 = new Vector3(0f, yPosition, zPosition);
                        Vector3 screenPos3 = new Vector3(-1f, yPosition, zPosition);

                        positions[0] = screenPos1;
                        positions[1] = screenPos2;
                        positions[2] = screenPos3;

                        for (int i = 0; i < positions.Length; i++)
                        {
                            Quaternion rotation = Quaternion.Euler(80f, 0f, 180f);
                            GameObject CardCurrent = Instantiate(CardPrefabSolo, positions[i], rotation);
                            CardCurrent.tag = "SelectCard";
                            CardCurrent.AddComponent<Button>();
                            CreateInfoCard(CardCurrent);
                            CardCurrent.GetComponent<Button>().onClick.AddListener(() => SelectedCard(CardCurrent));

                        }
                    }
                    else
                        Debug.Log("Desten Dolu");

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Nomadic Scout")
                {
                  /*  PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
                    foreach (PlayerController obj in objects)
                    {
                        if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV != this.PV)
                        {
                            if(obj.GetComponent<PlayerController>().NomadicTactics==true)
                            {
                                Debug.Log("Nomadic tactik sırrı ortaya çıktı");
                                obj.GetComponent<PlayerController>().NomadicTactics = false;
                            }
                            if (obj.GetComponent<PlayerController>().SteppeAmbush == true)
                            {
                                Debug.Log("Steppeambush sırrı ortaya çıktı");
                                obj.GetComponent<PlayerController>().SteppeAmbush = false;
                            }
                        }
                    }*/
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Steppe Warlord")
                {
                    GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
                    foreach (GameObject card in mycards)
                    {
                        if (card.GetComponent<CardInformation>().CardName == "Mongol Messenger" || card.GetComponent<CardInformation>().CardName== "Mongol Archer" || card.GetComponent<CardInformation>().CardName== "General Subutai")
                        {
                            Debug.Log("KartBulundu can arttırıldı");
                            card.GetComponent<CardInformation>().CardDamage += 2;
                            card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                            card.GetComponent<CardInformation>().SetInformation();
                          
                        }
                    }
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "General Subutai")
                {
                    selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
                    Transform childTransform = selectedCard.transform;
                    Transform green = childTransform.Find("Green");
                    green.gameObject.SetActive(true);
                    GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
                    foreach (GameObject card in mycards)
                    {
                        if (card.GetComponent<CardInformation>().CardName == "Steppe Warlord")
                        {
                            Debug.Log("SteppeWorld var can arttırıldı");
                            selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                            selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                            selectedCard.GetComponent<CardInformation>().SetInformation();
                          
                        }
                    }
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Horseback Archery")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();                                                                                                            //genghis spells

                    
                    _TutorialCardProgress.HorsebackArchery();

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Ger Defense")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();                                                                                                            

                    
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.ForMyCard = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Mongol Fury")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.MongolFury();

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Around the Great Wall")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.AroundtheGreatWall();

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Eternal Steppe’s Whisper")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.ForMyCard = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "God’s Bane")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    
                    GodsBaneUsed = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Steppe Ambush")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    
                    SteppeAmbush = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Nomadic Tactics")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    
                    NomadicTactics = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }

                ManaCountText.text = Mana.ToString() + "/10";
                OwnManaBar.fillAmount = Mana / 10f;
                CompetitorManaBar.fillAmount = TurnCount / 10;
                CompetitorManaCountText.text = (TurnCount) + "/10".ToString();

//                selectedCard.GetComponent<CardController>().UsedCard(selectedCard.GetComponent<CardInformation>().CardDamage, PV.Owner.IsMasterClient); // HERO YA DAMAGE VURMA

                selectedCard.tag = "UsedCard";
                DeckCardCount--;

                StackOwnDeck();
                                
               
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

    void DragCardAfterSelect()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0.5f;       //kartın ekrana uzaklığı
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float followSpeed = 5f; // Kartın takip hızı
        selectedCard.transform.position = Vector3.Lerp(selectedCard.transform.position, targetPosition, Time.deltaTime * followSpeed);
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
            else if (hit.collider.gameObject.tag == "UsedCard" && _TutorialCardProgress.ForMyCard==false)
            {
                // _CardFunction.SelectFirstCard(hit.collider.gameObject);
                Debug.LogError(hit.collider.gameObject.transform.parent);
                if(hit.collider.gameObject.GetComponent<CardInformation>().CardFreeze == false && hit.collider.gameObject.GetComponent<CardInformation>().isItFirstRaound == false && hit.collider.gameObject.GetComponent<CardInformation>().isAttacked==false)
                {
                    _TutorialCardProgress.SetAttackerCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, hit.collider.gameObject.transform.parent.gameObject));
                }
                else
                {
                    Debug.Log("CardFreeze or firstraund or isattacked");
                   GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                   TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "CardFreeze or firstraund or isattacked.";
                }
            }

        }
    }

    public void BeginerFunction() // Tur bize geçtiğinde çalışan fonksion
    {
        GameObject[] AllEnemyCards = GameObject.FindGameObjectsWithTag("UsedCard");

        if (true)
        {
           
           
            if(true) // DIĞER TURLAR
            {
      
               
                TurnCount++;
                CreateAnCard();
                
                foreach (var card in AllEnemyCards)
                {
                    if (card.GetComponent<CardInformation>().DivineSelected == true)
                    {
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) * 2).ToString();
                        card.GetComponent<CardInformation>().SetInformation();
                        card.GetComponent<CardInformation>().DivineSelected = false;
                       
                    }

                    if(card.GetComponent<CardInformation>().FirstTakeDamage==false)
                    {
                        card.GetComponent<CardInformation>().FirstTakeDamage =true;
                      
                    }

                    if (card.GetComponent<CardInformation>().GerDefense == true)
                    {
                        card.GetComponent<CardInformation>().GerDefense = false;
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) - 2).ToString();
                        card.GetComponent<CardInformation>().SetInformation();

                        if (int.Parse(card.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
                        {
                            Destroy(card); // OLDUKTEN SONRA CONROL YAP
                        }
                    }

                    if (card.GetComponent<CardInformation>().EternalShield == true)
                    {
                        card.GetComponent<CardInformation>().EternalShield = false;
                      
                    }

                }

                if (OlympiaKillCount >= 4)
                {
                    foreach (var Cards in AllEnemyCards)
                    {
                        if (Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Centaur Archer" || Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Minotaur Warrior" || Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Siren" || Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Gorgon" || Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Nemean Lion" || Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardName == "Chimera")
                        {
                            Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardHealth = (int.Parse(Cards.GetComponent<CardInformation>().GetComponent<CardInformation>().CardHealth) + 2).ToString();
                        
                        }
                    }
                }
              

               
                
               
                _TutorialCardProgress.BattleableCard();
                WhoseTurnText.text = "Finish Turn";
                finishButton.interactable = true;

            }

        }
        

        
        Mana = TurnCount;
        RefreshUI();

    }

    public void FinishButton()
    {
        // Find all GameObjects with the specified name
      
        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("UsedCard");
        WhoseTurnText.text = "Enemy Turn";
        Timer.text = "00:60";
        elapsedTime = 60;
        finishButton.interactable = false;

        
        foreach (var card in AllOwnCards)
        {
            card.GetComponent<CardInformation>().isItFirstRaound = false;
            card.GetComponent<CardInformation>().CardFreeze = false;
            card.GetComponent<CardInformation>().isAttacked = false;
            card.GetComponent<CardInformation>().CanAttackBehind = false;
            if (card.GetComponent<CardInformation>().CardName == "Kublai Khan")
            {
                _TutorialCardProgress.KublaiKhan();
            }

            if(card.GetComponent<CardInformation>().MongolFury==true)
            {
                card.GetComponent<CardInformation>().CardDamage -= 2;
                card.GetComponent<CardInformation>().MongolFury = false;
                card.GetComponent<CardInformation>().SetInformation();
           
            }
        }
        _TutorialCardProgress.WindFury = true;
        _TutorialCardProgress.ResetAllSign();

       // ADD MANA
       // BOTU CALISTIR
       
      
       GetComponent<BotController>().BotAttack();

        _TutorialCardProgress.AttackerCard = null;
        _TutorialCardProgress.TargetCard = null;
        _TutorialCardProgress.TargetCardIndex = -1;
        _TutorialCardProgress.SecoundTargetCard = false;
        _TutorialCardProgress.ForMyCard = false;


    }

    void CreateAnCard()
    {

        GameObject card = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
        card.tag = "Card";
        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
        card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
        
        CreateInfoCard(card);
        StackOwnDeck();
       
        DeckCardCount++;

    }

    public void CreateAnCompetitorCard()
    {

        GameObject card = Instantiate(Resources.Load<GameObject>("TutorialCompetitorCard"), GameObject.Find("CompetitorDeck").transform);
        card.tag = "CompetitorDeckCard";
        float xPos = CompetitorDeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
        card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
        
        StackCompetitorDeck();
       
        CompetitorDeckCardCount++;

    }

    public void RemoveAnCompetitorCard()
    {

       // GameObject card = Instantiate(Resources.Load<GameObject>("TutorialCompetitorCard"), GameObject.Find("CompetitorDeck").transform);
        Destroy(GameObject.Find("CompetitorDeck").transform.GetChild(0).gameObject);
       
        StackCompetitorDeck();

         CompetitorDeckCardCount--;
       
        

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

    public Animator HeroAnimator;

     public void MainCardSpecial()
    {
      
       
     
        
            
            if ( finishButton.interactable==true && Mana >= 0)
            {
                GameObject existingObject = GameObject.Find("OwnMainCardGameObject");
                if (existingObject == null)
                {
                    GameObject OwnMainCardGameObject = new GameObject("OwnMainCardGameObject");
                    OwnMainCardGameObject.AddComponent<CardInformation>();
                   // OwnMainCardGameObject.AddComponent<CardController>();
                    existingObject = OwnMainCardGameObject;
                }

              

                switch (OwnMainCard)
                {

                    case "Zeus":
                        if (Mana >= 2)
                        {
                            existingObject.GetComponent<CardInformation>().CardName = "Zeus";
                            existingObject.GetComponent<CardInformation>().CardDamage =(int) OwnHeroAttackDamage;
                            existingObject.GetComponent<CardInformation>().CardMana = 2;
                            HeroAnimator.SetTrigger("Ulti");
                            _TutorialCardProgress.SetMainAttackerCard(existingObject);
                        }
                        break;
                    case "Genghis":
                        if (Mana >= 2)
                        {
                            existingObject.GetComponent<CardInformation>().CardName = "Genghis";
                            existingObject.GetComponent<CardInformation>().CardDamage = (int)OwnHeroAttackDamage;
                            existingObject.GetComponent<CardInformation>().CardMana = 2;
                            HeroAnimator.SetTrigger("Ulti");
                            
                            if(NomadsLand >= 5)
                            {
                                if(OwnHeroAttackDamage <= 2 )
                                {
                                    Debug.Log("Nomadiclands Aktif");
                                   OwnHeroAttackDamage++;
                                }
                            }

                            //existingObject.GetComponent<CardController>().UsedCard(existingObject.GetComponent<CardInformation>().CardDamage, PV.Owner.IsMasterClient);

                            SetMana(existingObject);
                            GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
                            foreach (GameObject card in mycards)
                            {
                                if(card.GetComponent<CardInformation>().CardName== "Horse Breeder")
                                {
                                    List<int> emptyCells = new List<int>();

                                    for (int i = 0; i < 14; i++)
                                    {
                                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                                        {
                                            emptyCells.Add(i);
                                        }
                                    }
                                    if (emptyCells.Count > 0)
                                    {
                                        int randomIndex = emptyCells[UnityEngine.Random.Range(0, emptyCells.Count)];

                                        CreateSpecialCard("Steppe Horse", "2", 2, 0, randomIndex, false);
                                    }
                                    else
                                    {
                                        Debug.LogWarning("No empty cells available to place the Eagle card.");
                                    }
                                }
                            }

                        }
                        break;
                }

             

            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.LogError("IT IS NOT YOUR TURN!");

                GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "It is not my Turn!";
            }
         

        

      
    }

    public void CreateUsedCard(int boxindex, string name, string des, string heatlh, int damage, int mana)
    {

        
            GameObject CardCurrent = Instantiate(CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[boxindex].transform);
            CardCurrent.tag = "CompetitorCard";

          //  CardCurrent.GetComponent<PhotonView>().ViewID = OwnDeck.Length;
            CardCurrent.transform.localScale = new Vector3(1,1,0.04f);
            CardCurrent.transform.localPosition = Vector3.zero;
            CardCurrent.transform.localEulerAngles = new Vector3(45,0,180);

            CardCurrent.GetComponent<CardInformation>().CardName = name;
            CardCurrent.GetComponent<CardInformation>().CardDes = des;
            CardCurrent.GetComponent<CardInformation>().CardHealth = heatlh;
            CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
            CardCurrent.GetComponent<CardInformation>().CardMana = mana;
            CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
            CardCurrent.GetComponent<CardInformation>().SetInformation();

        
    }

    void StackOwnDeck()
    {

        if (GameObject.Find("Deck").transform.childCount < 6)
        {

            for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++)
            {
                float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz

                GameObject.Find("Deck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
            }

            GameObject.Find("Deck").transform.position = new Vector3(0.6f - GameObject.Find("Deck").transform.childCount * 0.2f, 2.7f, -3.81f);

        }
        else if (GameObject.Find("Deck").transform.childCount < 10)
        {

            for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++)
            {
                float xPos = i * 0.4f - 0.4f; // Kartın X konumunu belirliyoruz

                GameObject.Find("Deck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                GameObject.Find("Deck").transform.GetChild(i).transform.eulerAngles = new Vector3(74.8931351f, 351.836639f, 174.237427f);



            }

            GameObject.Find("Deck").transform.position = new Vector3(0.3f - GameObject.Find("Deck").transform.childCount * 0.1f, 2.7f, -3.81f);

        }
        else
        {

            for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++)
            {
                float xPos = i * 0.3f - 0.3f; // Kartın X konumunu belirliyoruz

                GameObject.Find("Deck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                GameObject.Find("Deck").transform.GetChild(i).transform.eulerAngles = new Vector3(74.8471832f, 350.247925f, 173.120972f);



            }

            GameObject.Find("Deck").transform.position = new Vector3(0.05f - GameObject.Find("Deck").transform.childCount * 0.05f, 2.7f, -3.81f);

        }
    }

    public void CreateInfoCard(GameObject CardCurrent)
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
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
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
                          CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
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

    void SetUIData()
    {
        OwnName = AuthManager.userName;
        OwnDeck = AuthManager.playerDeckArray;
        OwnMainCard = OwnDeck[0];
        OwnHeroAttackDamage = FindHeroAttackDamage(OwnDeck[0]);
        OwnHealth = FindHeroHealth(OwnDeck[0]);

        if(FirstHealthCount==0)
        {
            FirstHealthCount = FindHeroHealth(OwnDeck[0]);

        }

       

        CompetitorName = "Dummy";
        CompetitorDeck = AuthManager.playerDeckArray;
        CompetitorMainCard = OwnDeck[0];
        CompetitorHeroAttackDamage = FindHeroAttackDamage(OwnDeck[0]);
        CompetitorHealth = FindHeroHealth(OwnDeck[0]);

        OwnNameText.text = OwnName;
        OwnHeroAttackDamageText.text = OwnHeroAttackDamage.ToString();
        OwnHealthText.text = OwnHealth.ToString() + "/" + FirstHealthCount ;

        CompetitorNameText.text = CompetitorName;
        CompetitorMainCardText.text = CompetitorMainCard;
        CompetitorHeroAttackDamageText.text = CompetitorHeroAttackDamage.ToString();
        CompetitorHealthText.text = CompetitorHealth.ToString() + "/" + FirstHealthCount ;
        
    }

    public void RefreshUI()
    {

     
        ManaCountText.text = Mana.ToString() + "/10";  
        OwnManaBar.fillAmount = Mana / 10;   

        OwnHealthText.text = OwnHealth.ToString() + "/" + FirstHealthCount.ToString();
        OwnHealthBar.fillAmount = OwnHealth / FirstHealthCount;


        CompetitorHealthText.text = CompetitorHealth.ToString() + "/" + FirstHealthCount.ToString();
        CompetitorHealthBar.fillAmount = CompetitorHealth / FirstHealthCount;

        CompetitorManaCountText.text = TurnCount.ToString() + "/10";  
        CompetitorManaBar.fillAmount = TurnCount / 10;   



    }

    public int FindHeroHealth(string HeroName)
    {
        switch (HeroName)
        {
            case "Zeus":
                ZeusCard zeusCard = new ZeusCard();
                return (int) zeusCard.hpValue;

            case "Genghis":
                GenghisCard genghisCard = new GenghisCard();
                return (int) genghisCard.hpValue;


        }

        return 10;

    }

     int FindHeroAttackDamage(string HeroName)
    {
        switch (HeroName)
        {
            case "Zeus":
                ZeusCard zeusCard = new ZeusCard();
                return (int)zeusCard.attackValue;

            case "Genghis":
                GenghisCard genghisCard = new GenghisCard();
                return (int)genghisCard.attackValue;


        }

        return 60;

    }


}
