using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class TutorialPlayerController : MonoBehaviour
{
   string OwnName = "";
    public string[] OwnDeck;
    string OwnMainCard = "";
    public float OwnHealth = 0;
    float OwnHeroAttackDamage = 0;


    string CompetitorName = "";
    string[] CompetitorDeck;
    string CompetitorMainCard = "";
    public float CompetitorHealth = 0;
    public float CompetitorHeroAttackDamage = 0;

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

    int CompetitorDeckCardCount = 0;
    float elapsedTime = 60;

    GameObject selectedCard;
    GameObject lastHoveredCard = null;

    private Vector3 initialCardPosition;

    public Text OwnHeroAttackDamageText;
    public Text CompetitorHeroAttackDamageText;
    int FirstHealthCount=0;
    public GameObject CardPrefabSolo,CardPrefabInGame; 

    float TurnCount = 1;

    TutorialCardProgress _TutorialCardProgress;

    [HideInInspector] public int OlympiaKillCount = 0;
    [HideInInspector] public int SpellsExtraDamage = 0;
    [HideInInspector] public bool GodsBaneUsed = false;
    [HideInInspector] public bool SteppeAmbush = false;
    [HideInInspector] public bool NomadicTactics = false;
    [HideInInspector] public int NomadsLand = 0;
    [HideInInspector] public int AsgardQuestion = 0;
    [HideInInspector] public int DeadCardCount = 0;
    [HideInInspector] public int DeckCardCount = 0;
    [HideInInspector] public float Mana = 3;
    [HideInInspector] public int DeadMonsterCound = 0;
    [HideInInspector] public int AugmentCount = 0;
    [HideInInspector] public bool CanAttackMainCard = true;
    [HideInInspector] public int DoubleDamage = 1;
    [HideInInspector] public bool NaiadProtector = false;     //true ise büyü kullandığında +1 sağlık kazanıyoruz
    [HideInInspector] public bool PlayedSpell = false;          //bu tur büyü oynandımı diye kontrol
    [HideInInspector] public bool PlagueCarrierBool = false;    //ikinci sefer çalışması için
    [HideInInspector] public int LessSpellCost = 0;             //büyülerin mana değerini 1 azaltıyor
    [HideInInspector] public bool MerfolkScoutBool = false;     //bir karta bakıyor daha sonra onu çekiyor

    [HideInInspector] public List<string> DeadMyCardName = null;
    [HideInInspector] public List<GameObject> UsedSpellCard = null;

    public GameObject CompetitorPV = null;
    public GameObject healthTextObject;
    public GameObject attackTextObject;
    public GameObject drawCardTextObject;
    public GameObject Freeze;
    public GameObject Mummies;
    public GameObject TakeGun;
    public GameObject Minion;
    public GameObject Spell;
    GameObject NextCard;

    public GameObject vfxMovePrefab;
    public GameObject vfxLandingPrefab;
    public GameObject vfxAttackPrefab;
    void Awake()
    {
        _TutorialCardProgress = GetComponent<TutorialCardProgress>();
    }

    void Start()
    {
        Mana = 1;
        SetUIData();

        for (int i = 0; i < 3; i++)
        {
           CreateAnCard();
        }

        LogsContainerContent = GameObject.Find("Container");
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
                                        CreateTextAtTargetIndex(index, _TutorialCardProgress.AttackerCard.GetComponent<CardInformation>().CardDamage, false, _TutorialCardProgress.AttackerCard.GetComponent<CardInformation>().CardName);
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

    public void CreateTextAtTargetIndex(int targetIndex, int damage, bool mycard, string cardname)
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

        string cardName = cardname.Replace(" ", "");

        // Kart adına göre VFX yolunu oluştur
        string attackVFXPath = $"Vfx/VFX_{cardName}_Attack";
        GameObject loadedVFXPrefab = Resources.Load<GameObject>(attackVFXPath);

        // Eğer kart VFX prefab'ı bulunamazsa, default prefab'ı kullan
        if (loadedVFXPrefab != null)
        {
            GameObject attackInstance = Instantiate(loadedVFXPrefab, targetTransform);
            Destroy(attackInstance, 5f);
        }
        else
        {
            Debug.LogError("Attack VFX prefab'ı bulunamadı: " + attackVFXPath + ". Default VFX çalıştırılıyor.");
            GameObject attackInstance = Instantiate(vfxAttackPrefab, targetTransform); // vfxAttackPrefab zaten default olarak tanımlı
            Destroy(attackInstance, 5f);
        }

        GameObject damageTextObject = Instantiate(DamageText);
        damageTextObject.transform.parent = targetTransform;
        damageTextObject.transform.localPosition = new Vector3(0, 0.5f, 0);
        damageTextObject.transform.localRotation = Quaternion.Euler(50, 0, 0);
        damageTextObject.transform.localScale = new Vector3(0.005f, 0.01f, 0.01f);

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


    public string ColorToString(Color color)
    {
        return color.r + "," + color.g + "," + color.b + "," + color.a;
    }
    public void RefreshLog(int damage, bool isdead, string attackercardname, string targetcardname, Color color)
    {
        GameObject logsObject = Instantiate(LogsPrefab, LogsContainerContent.transform);
        string colorString = ColorToString(color);

        // Damage text objesini bul ve değerini güncelle
        Text logsDamageText = logsObject.transform.Find("DamageImage/Damage").GetComponent<Text>();
        if (damage != 0)
        {
            logsDamageText.text = damage.ToString();
        }
        else
        {
            logsDamageText.text = null;
        }


        // Dead objesini bul ve gerekli işlemi yap
        Transform deadObject = logsObject.transform.Find("TargetImage");
        if (deadObject != null && isdead)
        {
            Image deadImage = deadObject.GetComponent<Image>();
            deadImage.color = color;
        }
        else if (deadObject == null)
        {
            Debug.LogWarning("Dead object not found in LogsPrefab.");
        }

        // Attacker ve Target objelerini bul ve resimlerini yükle
        Transform attackerObject = logsObject.transform.Find("AttackerImage/Attacker");
        Transform targetObject = logsObject.transform.Find("TargetImage/Target");

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

        // Rakip oyuncular için RPC çağrısı yap
    }

    private void UpdateDamageText(GameObject logsObject, int damage)
    {
        Text logsDamageText = logsObject.transform.Find("Damage").GetComponent<Text>();
        logsDamageText.text = damage != 0 ? damage.ToString() : null;
    }

    private void UpdateDeadObject(GameObject logsObject, bool isDead, Color color)
    {
        Transform deadObject = logsObject.transform.Find("Dead");
        if (deadObject != null)
        {
            deadObject.gameObject.SetActive(isDead);
            if (isDead)
            {
                Image deadImage = deadObject.GetComponent<Image>();
                deadImage.color = color;
            }
        }
        else
        {
            Debug.LogWarning("Dead object not found in LogsPrefab.");
        }
    }

    private void UpdateCardImage(GameObject logsObject, string objectName, string cardName)
    {
        Transform cardObject = logsObject.transform.Find(objectName);
        if (cardObject != null)
        {
            Sprite foundSprite = LoadCardSprite(cardName);
            Image cardImage = cardObject.GetComponent<Image>();
            if (cardImage != null)
            {
                cardImage.sprite = foundSprite;
            }
        }
        else
        {
            Debug.LogWarning($"{objectName} object not found in LogsPrefab.");
        }
    }

    private Sprite LoadCardSprite(string cardName)
    {
        Sprite foundSprite = Resources.Load<Sprite>("CardImages/" + cardName);
        if (foundSprite == null)
        {
            Debug.LogWarning("Sprite not found: " + cardName);
            foundSprite = Resources.Load<Sprite>("CardImages/" + "Centaur Archer");
        }
        return foundSprite;
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
        // Kart ismini al ve boşlukları kaldır
        string cardName = card.GetComponent<CardInformation>().CardName.Replace(" ", "");

        // Kart ismine göre VFX prefab yollarını oluştur
        string movieVFXPath = $"Vfx/VFX_{cardName}_Movie";
        string lastVFXPath = $"Vfx/VFX_{cardName}_Last";

        // Movie VFX prefab'ını yükle, eğer bulunamazsa default vfxMovePrefab'ı kullan
        GameObject loadedMovePrefab = Resources.Load<GameObject>(movieVFXPath);
        GameObject moveVFXToUse = loadedMovePrefab != null ? loadedMovePrefab : vfxMovePrefab;

        Vector3 startPosition = card.transform.position;
        float time = 0f;



        // Kartı yukarı ve ileri hareket ettir
        Vector3 targetPlus = targetPosition + new Vector3(0f, 0.4f, 0.8f);
        while (time < duration)
        {
            card.transform.position = Vector3.Lerp(startPosition, targetPlus, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Movie VFX varsa onu instantiate et
        GameObject vfxMoveInstance = Instantiate(moveVFXToUse, card.transform.position, Quaternion.identity);
        vfxMoveInstance.transform.SetParent(card.transform);
        vfxMoveInstance.transform.localPosition = Vector3.zero;
        VisualEffect vfxMoveComponent = vfxMoveInstance.GetComponent<VisualEffect>();
        if (vfxMoveComponent != null)
        {
            vfxMoveComponent.Play();
        }

        // İlk hareketten sonra biraz bekle
        yield return new WaitForSeconds(1.5f);

        // Movie VFX'i yok et
        Destroy(vfxMoveInstance);

        // Kartı hedef pozisyona indir
        time = 0f;
        while (time < duration)
        {
            card.transform.position = Vector3.Lerp(targetPlus, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        card.transform.position = targetPosition;

        // Last VFX prefab'ını yükle, eğer bulunamazsa default vfxLandingPrefab'ı kullan
        GameObject loadedLandingPrefab = Resources.Load<GameObject>(lastVFXPath);
        GameObject landingVFXToUse = loadedLandingPrefab != null ? loadedLandingPrefab : vfxLandingPrefab;

        // Last VFX varsa onu instantiate et
        GameObject vfxLandingInstance = Instantiate(landingVFXToUse, card.transform.position, Quaternion.identity);
        vfxLandingInstance.transform.SetParent(card.transform);
        vfxLandingInstance.transform.localPosition = Vector3.zero;
        VisualEffect vfxLandingComponent = vfxLandingInstance.GetComponent<VisualEffect>();
        if (vfxLandingComponent != null)
        {
            vfxLandingComponent.Play();
        }

        // Last VFX'i 5 saniye sonra yok et
        yield return new WaitForSeconds(5f);
        Destroy(vfxLandingInstance);

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


     
    public GameObject CreateSpecialCard(string name, string health, int damage, int mana, int index, bool front)
    {
        if (true)
        {
            GameObject CardCurrent;
            if (front)
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

            
            return CardCurrent;
        }
        return null;
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
               // selectedCard.transform.localScale = new Vector3(1, 1, 0.04f);
               // selectedCard.transform.localEulerAngles = new Vector3(45f, 0f, 180);

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
                    UsedSpell(selectedCard);

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
                    UsedSpell(selectedCard);

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
                    UsedSpell(selectedCard);


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
                    UsedSpell(selectedCard);


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
                    UsedSpell(selectedCard);


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
                    UsedSpell(selectedCard);


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
                    UsedSpell(selectedCard);


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
                    UsedSpell(selectedCard);

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
                    UsedSpell(selectedCard);

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
                    UsedSpell(selectedCard);

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
                    UsedSpell(selectedCard);

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
                    UsedSpell(selectedCard);

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
                    UsedSpell(selectedCard);

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
                    UsedSpell(selectedCard);

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
                    UsedSpell(selectedCard);

                    NomadicTactics = true;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                   
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                // ODIN HERE

                else if (selectedCard.GetComponent<CardInformation>().CardName == "Viking Raider")
                {
                    selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
                    Transform childTransform = selectedCard.transform;
                    Transform green = childTransform.Find("Green");
                    green.gameObject.SetActive(true);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Runestone Mystic")
                {
                    SpellsExtraDamage += 1;
                    CreateRandomCard();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Shieldmaiden Defender")
                {
                    int frontRowCardCount = 0;

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount > 0)
                        {
                            frontRowCardCount++;
                        }
                    }
                    selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 5).ToString();
                    selectedCard.GetComponent<CardInformation>().SetInformation();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Fenrir's Spawn")
                {
                    int mycard = 0;
                    int enemycard = 0;

                    for (int i = 0; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount > 0)
                        {
                            enemycard++;
                        }
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount > 0)
                        {
                            mycard++;
                        }
                    }
                    if (enemycard > mycard)
                    {
                        Debug.Log("Düşman fazla");
                        selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                        selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 3).ToString();
                        selectedCard.GetComponent<CardInformation>().SetInformation();
                    }
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Skald Bard")
                {
                    SpawnAndReturnGameObject();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Mimir's Seer")
                {
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject spawnedObject = SpawnAndReturnGameObject();

                        if (spawnedObject.GetComponent<CardInformation>().CardHealth == "")
                        {
                            Debug.LogError("Mimirsseer SPEEEELLL YARATTTI ");
                            GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                            TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "Mimirs Seer created a spell..";
                            
                            spawnedObject.GetComponent<CardInformation>().CardMana--;

                        }
                        else
                        {
                            Debug.LogError("Mimirsseer MİNNYOONNNN YARATTTI ");
                            GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                            TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "Mimirs Seer created a minion..";
                            
                        }
                    }
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Valkyrie's Chosen")
                {

                    List<int> emptyFrontCells = new List<int>();
                    List<int> emptyBackCells = new List<int>();

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyFrontCells.Add(i);
                        }
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyBackCells.Add(i);
                        }
                    }

                    int cardsCreated = 0;
                    while (cardsCreated < 2)
                    {
                        if (emptyFrontCells.Count > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                            int index = emptyFrontCells[randomIndex];
                            CreateSpecialCard("Viking Spirit", "1", 1, 0, index, true);
                            emptyFrontCells.RemoveAt(randomIndex);
                            cardsCreated++;
                        }
                        else if (emptyBackCells.Count > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                            int index = emptyBackCells[randomIndex];
                            CreateSpecialCard("Viking Spirit", "1", 1, 0, index, true);
                            emptyBackCells.RemoveAt(randomIndex);
                            cardsCreated++;
                        }
                        else
                        {
                            Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
                            break;
                        }
                    }
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Frost Giant")
                {   
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.AttackerCard = selectedCard;
                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.EnemyAllCard();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Heimdallr")
                {
                    List<int> emptyFrontCells = new List<int>();
                    List<int> emptyBackCells = new List<int>();

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyFrontCells.Add(i);
                        }
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyBackCells.Add(i);
                        }
                    }

                    int cardsCreated = 0;
                    while (cardsCreated < 3)
                    {
                        if (emptyFrontCells.Count > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                            int index = emptyFrontCells[randomIndex];
                            CreateSpecialCard("Viking", "1", 1, 0, index, true);
                            emptyFrontCells.RemoveAt(randomIndex);
                            cardsCreated++;
                        }
                        else if (emptyBackCells.Count > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                            int index = emptyBackCells[randomIndex];
                            CreateSpecialCard("Viking", "1", 1, 0, index, true);
                            emptyBackCells.RemoveAt(randomIndex);
                            cardsCreated++;
                        }
                        else
                        {
                            Debug.LogWarning("No empty cells available to place the Viking card.");
                            break;
                        }
                    }
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Einherjar Caller")
                {
                    if (DeadMyCardName == null || DeadMyCardName.Count == 0)
                    {
                        Debug.LogWarning("Card name list is empty or null.");
                        return;
                    }
                    string targetCardName = DeadMyCardName[UnityEngine.Random.Range(0, DeadMyCardName.Count)];
                    int targetIndex = -1;
                    string cardHealth = string.Empty;
                    int cardDamage = 0;
                    bool cardFound = false;

                    OdinCard odinCard = new OdinCard();

                    for (int i = 0; i < odinCard.minions.Count; i++)
                    {
                        if (odinCard.minions[i].name == targetCardName)
                        {
                            targetIndex = i;
                            cardHealth = odinCard.minions[targetIndex].health.ToString();
                            cardDamage = odinCard.minions[targetIndex].attack;
                            cardFound = true;
                            break;
                        }
                    }

                    if (cardFound)
                    {
                        List<int> emptyFrontCells = new List<int>();
                        List<int> emptyBackCells = new List<int>();

                        for (int i = 7; i < 14; i++)
                        {
                            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                            {
                                emptyFrontCells.Add(i);
                            }
                        }

                        if (emptyFrontCells.Count > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                            int index = emptyFrontCells[randomIndex];
                            CreateSpecialCard(targetCardName, cardHealth, cardDamage, 0, index, true);
                            emptyFrontCells.RemoveAt(randomIndex);
                        }
                        else
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                                {
                                    emptyBackCells.Add(i);
                                }
                            }

                            if (emptyBackCells.Count > 0)
                            {
                                int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                                int index = emptyBackCells[randomIndex];
                                CreateSpecialCard(targetCardName, cardHealth, cardDamage, 0, index, true);
                                emptyBackCells.RemoveAt(randomIndex);
                            }
                            else
                            {
                                Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Card not found in OdinCard minions.");
                    }
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Norn Weaver")
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
                            GameObject Card = Instantiate(Resources.Load<GameObject>("CompetitorCard"), positions[i], rotation);
                            Card.tag = "SelectCard";
                            Card.AddComponent<Button>();
                            CreateInfoCard(Card);

                            Card.GetComponent<Button>().onClick.AddListener(() => {
                                foreach (GameObject card in GameObject.FindGameObjectsWithTag("SelectCard"))
                                {
                                    Destroy(card);
                                }
                                if (UnityEngine.Random.Range(0, 2) == 0)
                                {
                                    GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                                    float xPos = DeckCardCount * 0.8f - 0.8f;
                                    selectedCard.transform.localPosition = new Vector3(xPos, 0, 0);
                                    selectedCard.tag = "Card";
                                    CreateInfoCard(selectedCard);
                                    StackOwnDeck(); 
                                    StackCompetitorDeck();
                                    DeckCardCount++;

                                }
                            });
                        }
                    }
                    else
                        print("DesrenDolu");


                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Winter's Chill")
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
                    _TutorialCardProgress.AttackerCard = selectedCard;
                    _TutorialCardProgress.EnemyAllCard();

                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Viking Raid")
                {

                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    List<int> emptyFrontCells = new List<int>();
                    List<int> emptyBackCells = new List<int>();

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyFrontCells.Add(i);
                        }
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyBackCells.Add(i);
                        }
                    }

                    int cardsCreated = 0;
                    while (cardsCreated < 3)
                    {
                        if (emptyFrontCells.Count > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                            int index = emptyFrontCells[randomIndex];
                            GameObject Card = CreateSpecialCard("Viking", "2", 2, 0, index, true);
                            CanFirstRauntAttack(Card);
                            emptyFrontCells.RemoveAt(randomIndex);
                            cardsCreated++;
                        }
                        else if (emptyBackCells.Count > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                            int index = emptyBackCells[randomIndex];
                            GameObject Card = CreateSpecialCard("Viking", "2", 2, 0, index, true);
                            CanFirstRauntAttack(Card);
                            emptyBackCells.RemoveAt(randomIndex);
                            cardsCreated++;
                        }
                        else
                        {
                            Debug.LogWarning("No empty cells available to place the Viking card.");
                            break;
                        }
                    }
                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Sleipnir’s Gallop")
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
                    _TutorialCardProgress.AttackerCard = selectedCard;
                    _TutorialCardProgress.ForMyCard = true;
                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;


                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Gjallarhorn Call")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    List<string> einherjarNames = new List<string> { "Einherjar Champion", "Einherjar Berserker", "Einherjar Duelist" };
                    int randomIndex = UnityEngine.Random.Range(0, einherjarNames.Count);
                    string selectedName = einherjarNames[randomIndex];
                    int index = -1;


                    List<int> emptyFrontCells = new List<int>();
                    List<int> emptyBackCells = new List<int>();

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyFrontCells.Add(i);
                        }
                    }

                    if (emptyFrontCells.Count > 0)
                    {
                        int indexrandom = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                        index = emptyFrontCells[indexrandom];
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                            {
                                emptyBackCells.Add(i);
                            }
                        }

                        if (emptyBackCells.Count > 0)
                        {
                            int indexrandom = UnityEngine.Random.Range(0, emptyBackCells.Count);
                            index = emptyBackCells[indexrandom];
                        }
                        else
                        {
                            Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
                        }
                    }
                    if (index == -1)
                        return;

                    if (selectedName == "Einherjar Champion")
                    {
                        GameObject CurrentCard = CreateSpecialCard("Einherjar Champion", "7", 3, 0, index, true);
                        CanFirstRauntAttack(CurrentCard);
                    }
                    else if (selectedName == "Einherjar Berserker")
                    {
                        CreateSpecialCard("Einherjar Champion", "5", 3, 0, index, true);
                    }
                    else if (selectedName == "Einherjar Duelist")
                    {
                        CreateSpecialCard("Einherjar Champion", "5", 5, 0, index, true);
                    }
                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;


                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Rune Magic")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    RuneMagic();
                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "The Allfather’s Decree")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    List<int> emptyFrontCells = new List<int>();
                    List<int> emptyBackCells = new List<int>();

                    GameObject CreatedCard;

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyFrontCells.Add(i);
                        }
                    }

                    if (emptyFrontCells.Count > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                        int index = emptyFrontCells[randomIndex];
                        CreatedCard = CreateSpecialCard("Gungnir", "4", 2, 0, index, true);
                        CreatedCard.GetComponent<CardInformation>().Invulnerable = true;
                        CanFirstRauntAttack(CreatedCard);
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                            {
                                emptyBackCells.Add(i);
                            }
                        }

                        if (emptyBackCells.Count > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                            int index = emptyBackCells[randomIndex];
                            CreatedCard = CreateSpecialCard("Gungnir", "4", 2, 0, index, true);
                            CreatedCard.GetComponent<CardInformation>().Invulnerable = true;
                            CanFirstRauntAttack(CreatedCard);
                        }
                        else
                        {
                            Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
                        }
                    }


                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;


                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Mimir's Wisdom")
                {
                    int maxNewCards = 10 - GameObject.Find("Deck").transform.childCount + 1;
                    int cardsToCreate = Mathf.Min(3, maxNewCards);
                    for (int i = 0; i < cardsToCreate; i++)
                    {
                        GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                        ManaCountText.text = Mana.ToString() + "/10";
                        OwnManaBar.fillAmount = Mana / 10f;
                        CompetitorManaBar.fillAmount = TurnCount / 10;
                        CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                        DeckCardCount--;

                        StackOwnDeck();

                        float xPos = DeckCardCount * 0.8f - 0.8f;
                        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0);  // Card yerine CardCurrent kullanılıyor
                        CardCurrent.tag = "Card";
                        CreateInfoCard(CardCurrent);
                        StackCompetitorDeck();
                        DeckCardCount++;

                        UsedSpell(selectedCard);
                        selectedCard.SetActive(false);
                        selectedCard = null;
                        lastHoveredCard = null;

                        Debug.LogError("USSEEDD A SPEEELLL");
                    }
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }


                // ANUBİS HERE

                else if (selectedCard.GetComponent<CardInformation>().CardName == "Necropolis Acolyte")
                {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.AttackerCard = selectedCard;
                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.ForHeal = true;
                    _TutorialCardProgress.OpenMyCardSign();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Desert Bowman")
                {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.AttackerCard = selectedCard;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Sphinx Riddler")
                {
                    if (GameObject.Find("Deck").transform.childCount < 11)
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

                        int correctCardIndex = UnityEngine.Random.Range(0, positions.Length);

                        for (int i = 0; i < positions.Length; i++)
                        {
                            Quaternion rotation = Quaternion.Euler(80f, 0f, 180f);
                            GameObject Card = Instantiate(Resources.Load<GameObject>("CompetitorCard"), positions[i], rotation);
                            Card.tag = "SelectCard";
                            Card.AddComponent<Button>();
                            CreateInfoCard(Card);

                            Card.GetComponent<Button>().onClick.AddListener(() => {
                                foreach (GameObject card in GameObject.FindGameObjectsWithTag("SelectCard"))
                                {
                                    Destroy(card);
                                }

                                if (i == correctCardIndex)
                                {
                                    SpawnAndReturnGameObject();
                                }
                            });
                        }
                    }
                    else
                    {
                        print("Deck is Full");
                    }
                }
                
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Osiris’ Bannerman")
                {
                    GameObject[] AllCard = GameObject.FindGameObjectsWithTag("UsedCard");
                    foreach (var card in AllCard)
                    {
                        if (card.GetComponent<CardInformation>().CardName == "Osiris")
                        {
                            selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                            selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                        }
                    }
                }


                else if (selectedCard.GetComponent<CardInformation>().CardName == "Sun Charioteer")
                {
                    selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
                    Transform childTransform = selectedCard.transform;
                    Transform green = childTransform.Find("Green");
                    green.gameObject.SetActive(true);
                }

                else if (selectedCard.GetComponent<CardInformation>().CardName == "Tomb Protector")
                {
                    selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + CheckUndeadCards()).ToString();
                    selectedCard.GetComponent<CardInformation>().SetInformation();
                }

                else if (selectedCard.GetComponent<CardInformation>().CardName == "Falcon-Eyed Hunter")
                {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.AttackerCard = selectedCard;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Canopic Preserver")
                {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.AttackerCard = selectedCard;
                    _TutorialCardProgress.ForMyCard = true;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Sandstone Scribe")
                {
                    if (GameObject.Find("Deck").transform.childCount < 10)
                    {
                        GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                        CardCurrent.tag = "Card";
                        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                        CardCurrent.GetComponent<CardInformation>().CardName = "Scroll of Death";
                        CardCurrent.GetComponent<CardInformation>().CardDes = "Scroll of Death POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "0";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = 2;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        DeckCardCount++;
                        StackOwnDeck(); 
                        StackCompetitorDeck();

                    }
                }

                else if (selectedCard.GetComponent<CardInformation>().CardName == "Scroll of Death")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    UsedSpell(selectedCard);
                    int createdCardCount = 0;

                    for (int i = 7; i < 14 && createdCardCount < 2; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            CreateSpecialCard("Mummy", "1", 1, 0, i, true);
                            createdCardCount++;
                        }
                    }


                   
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Book of the Dead")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    if (DeadMyCardName == null || DeadMyCardName.Count == 0)
                    {
                        Debug.LogWarning("Card name list is empty or null.");
                        return;
                    }

                    AnubisCard anubisCard = new AnubisCard();
                    HashSet<string> revivedCards = new HashSet<string>();
                    int reviveCount = 0;

                    while (reviveCount < 5 && revivedCards.Count < DeadMyCardName.Count)
                    {
                        string targetCardName = DeadMyCardName[UnityEngine.Random.Range(0, DeadMyCardName.Count)];

                        if (revivedCards.Contains(targetCardName))
                        {
                            continue; // Bu kart zaten diriltildi, yeni bir kart seç
                        }

                        int targetIndex = -1;
                        string cardHealth = string.Empty;
                        int cardDamage = 0;
                        bool cardFound = false;

                        for (int i = 0; i < anubisCard.minions.Count; i++)
                        {
                            if (anubisCard.minions[i].name == targetCardName)
                            {
                                targetIndex = i;
                                cardHealth = anubisCard.minions[targetIndex].health.ToString();
                                cardDamage = anubisCard.minions[targetIndex].attack;
                                cardFound = true;
                                break;
                            }
                        }

                        if (cardFound)
                        {
                            List<int> emptyFrontCells = new List<int>();
                            List<int> emptyBackCells = new List<int>();

                            for (int i = 7; i < 14; i++)
                            {
                                if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                                {
                                    emptyFrontCells.Add(i);
                                }
                            }

                            if (emptyFrontCells.Count > 0)
                            {
                                int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                                int index = emptyFrontCells[randomIndex];
                                CreateSpecialCard(targetCardName, cardHealth, cardDamage, 0, index, true);
                                emptyFrontCells.RemoveAt(randomIndex);
                            }
                            else
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                                    {
                                        emptyBackCells.Add(i);
                                    }
                                }

                                if (emptyBackCells.Count > 0)
                                {
                                    int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                                    int index = emptyBackCells[randomIndex];
                                    CreateSpecialCard(targetCardName, cardHealth, cardDamage, 0, index, true);
                                    emptyBackCells.RemoveAt(randomIndex);
                                }
                                else
                                {
                                    Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
                                }
                            }

                            revivedCards.Add(targetCardName);
                            reviveCount++;
                        }
                        else
                        {
                            Debug.LogWarning("Card not found in OdinCard minions.");
                        }
                    }

                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }

                else if (selectedCard.GetComponent<CardInformation>().CardName == "Sun Disk Radiance")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    UsedSpell(selectedCard);

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Plague of Locusts")
                {

                }

                else if (selectedCard.GetComponent<CardInformation>().CardName == "River's Blessing")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    UsedSpell(selectedCard);


                    selectedCard.tag = "Card";

                    List<GameObject> usedCards = new List<GameObject>();
                    foreach (GameObject card in GameObject.FindGameObjectsWithTag("UsedCard"))
                    {
                        usedCards.Add(card);
                    }

                    if (usedCards.Count == 0)
                    {
                       
                        Debug.LogWarning("No used cards found.");
                        selectedCard.SetActive(false);
                        selectedCard = null;
                        lastHoveredCard = null;

                        Debug.LogError("USSEEDD A SPEEELLL");
                        return;
                    }
                    int totalHealthToDistribute = 10;
                    System.Random random = new System.Random();

                    while (totalHealthToDistribute > 0)
                    {
                        int cardIndex = random.Next(usedCards.Count);
                        int healthToAdd = random.Next(1, totalHealthToDistribute + 1);

                        // Canı seçilen karta ekle
                        CardInformation cardInfo = usedCards[cardIndex].GetComponent<CardInformation>();
                        if (cardInfo != null)
                        {
                            int currentHealth = int.Parse(cardInfo.CardHealth);
                            currentHealth += healthToAdd;
                            cardInfo.CardHealth = currentHealth.ToString();
                            cardInfo.SetInformation();
                            int CurrentCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, usedCards[cardIndex].transform.parent.gameObject);
                            GetComponent<PlayerController>().RefreshMyCard(CurrentCardIndex, cardInfo.CardHealth, cardInfo.HaveShield, cardInfo.CardDamage, cardInfo.DivineSelected, cardInfo.FirstTakeDamage, cardInfo.FirstDamageTaken, cardInfo.EternalShield);
                        }

                        totalHealthToDistribute -= healthToAdd;
                    }


                   
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Pyramid's Might")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    UsedSpell(selectedCard);
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.AttackerCard = selectedCard;
                    _TutorialCardProgress.ForMyCard = true;

                    
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Scales of Anubis")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    UsedSpell(selectedCard);
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.AttackerCard = selectedCard;
                   
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Gates of Duat")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    UsedSpell(selectedCard);
                    //GatesofDuat();

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Codex Guardian")
                {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    selectedCard.GetComponent<CardInformation>().DivineSelected = true;
                     
                }

                // LeonardoCard HERE

                else if (selectedCard.GetComponent<CardInformation>().CardName == "Codex Guardian")
                {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    selectedCard.GetComponent<CardInformation>().DivineSelected = true;

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Piscean Diver")
                {
                    selectedCard.GetComponent<CardInformation>().CanAttackBehind = true;

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Da Vinci's Helix Engineer")
                {
                    GameObject spawnedObject = SpawnAndReturnGameObject();

                    if (spawnedObject.GetComponent<CardInformation>().CardHealth == "")
                    {
                        Debug.LogError("Helix SPEEEELLL YARATTTI ");

                        GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                        TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "Helix created a spell..";
                        spawnedObject.GetComponent<CardInformation>().CardMana -= 2;
                        spawnedObject.GetComponent<CardInformation>().SetInformation();

                    }
                    else
                    {
                        Debug.LogError("Helix MİNNYOONNNN YARATTTI ");
                        GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                        TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "Helix created a minion..";
                    }

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Vitruvian Firstborn")
                {
                    for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++) // KENDİ DESTEMİZDEKİ KARTLARI TEK TEK ÇAĞIR
                    {

                        if (GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().CardHealth == "")  // ÇAĞIRILAN KARTIN BÜYÜ MÜ OLDUĞU KONTROL ET
                        {
                            GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().CardMana--;
                            GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().SetInformation();
                        }
                    }

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Da Vinci's Glider")
                {
                    for (int i = 0; i < 14; i++)
                    {

                        var cell = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject;

                        if (cell.transform.childCount != 0)
                        {
                            GameObject child = cell.transform.GetChild(0).gameObject;
                            int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, child.transform.parent.gameObject);

                            if (!child.activeSelf)
                            {
                                child.SetActive(true);
                                Debug.Log("Gizli kart bulundu");
                            }
                            child.GetComponent<CardInformation>().SetInformation();

                        }

                    }

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Mechanical Lion")
                {
                    GameObject[] AllCard = GameObject.FindGameObjectsWithTag("UsedCard");
                    LeonardoCard leonardoCard = new LeonardoCard();

                    foreach (var card in AllCard)
                    {
                        for (int i = 0; i < leonardoCard.minions.Count; i++)
                        {
                            string cardName = card.GetComponent<CardInformation>().CardName;
                            if (leonardoCard.minions[i].name == cardName &&
                                cardName != "Codex Guardian" &&
                                cardName != "Anatomist of the Unknown" &&
                                cardName != "Piscean Diver" &&
                                cardName != "Da Vinci's Helix Engineer")
                            {
                                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                                selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                                selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                                
                            }
                        }
                    }

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Da Vinci’s Blueprint")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    UsedSpell(selectedCard);


                    GameObject spawnedObject = SpawnAndReturnGameObject();
                    LeonardoCard leonardoCard = new LeonardoCard();
                    for (int i = 0; i < leonardoCard.minions.Count; i++)
                    {
                        string cardName = spawnedObject.GetComponent<CardInformation>().CardName;
                        if (leonardoCard.minions[i].name == cardName &&
                            cardName != "Codex Guardian" &&
                            cardName != "Anatomist of the Unknown" &&
                            cardName != "Piscean Diver" &&
                            cardName != "Da Vinci's Helix Engineer")
                        {
                            spawnedObject.GetComponent<CardInformation>().CardHealth = (int.Parse(spawnedObject.GetComponent<CardInformation>().CardHealth) + 3).ToString();
                            spawnedObject.GetComponent<CardInformation>().CardDamage += 2; ;
                            spawnedObject.GetComponent<CardInformation>().SetInformation();
                        }
                    }


                   
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Tabula Aeterna")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    UsedSpell(selectedCard);

                    GameObject[] cards = GameObject.FindGameObjectsWithTag("CompetitorCard");
                    if (cards.Length > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, cards.Length);
                        GameObject randomCard = cards[randomIndex];
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, randomCard.transform.parent.gameObject);
                        randomCard.GetComponent<CardInformation>().CardMana--;
                        DeleteAreaCard(TargetCardIndex);
                        DestroyAndCreateMyDeck(randomCard);
                    }

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }

                else if (selectedCard.GetComponent<CardInformation>().CardName == "Artistic Inspiration")
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
                    _TutorialCardProgress.AttackerCard = selectedCard;
                    _TutorialCardProgress.ForMyCard = true;


                    
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;
                    UsedSpell(selectedCard);
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }

                else if (selectedCard.GetComponent<CardInformation>().CardName == "Anatomical Insight")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    UsedSpell(selectedCard);
                    DoubleDamage = 2;

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                // DUSTİN HERE
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Wasteland Sniper")
                {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    _TutorialCardProgress.SecoundTargetCard = true;
                    _TutorialCardProgress.SetAttackerCard(index);
                    _TutorialCardProgress.AttackerCard = selectedCard;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Engineer of the Ruins")
                {
                    GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                    foreach (var card in AllMyCard)
                    {
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                        card.GetComponent<CardInformation>().SetInformation();
                        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                       
                    }
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Rogue AI Drone")
                {
                    selectedCard.GetComponent<CardInformation>().Invulnerable = true;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Mutant Behemoth")
                {
                    for (int i = 7; i < 14; i++)
                    {

                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount != 0)
                        {
                            GameObject CurrentCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.GetChild(0).gameObject;

                            int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, CurrentCard.transform.parent.gameObject);
                            CurrentCard.GetComponent<CardInformation>().CardDamage -= 2;
                            CurrentCard.GetComponent<CardInformation>().Behemot = true;

                            GetComponent<PlayerController>().PV.RPC("RPC_RefreshMaxDamage", RpcTarget.All, index, CurrentCard.GetComponent<CardInformation>().Behemot);
                            RefreshLog(0, true, selectedCard.GetComponent<CardInformation>().CardName, CurrentCard.GetComponent<CardInformation>().CardName, Color.magenta);
                        }
                    }
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Lone Cyborg")
                {
                    int cardscount = 0;
                    for (int i = 7; i < 14; i++)
                    {
                        var cardsAreaCreator = GameObject.Find("Area").GetComponent<CardsAreaCreator>();
                        var areaCollision = cardsAreaCreator.FrontAreaCollisions[i];

                        if (areaCollision.gameObject.transform.childCount != 0)
                        {
                            cardscount++;
                        }
                    }
                    if (cardscount == 1)
                    {
                        selectedCard.GetComponent<CardInformation>().CardDamage += 3;
                        selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 3).ToString();
                        selectedCard.GetComponent<CardInformation>().DivineSelected = true;
                        selectedCard.GetComponent<CardInformation>().SetInformation();
                        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                        
                    }
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Scavenger Raider")
                {
                    //ScavengerRaider();
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Claire")
                {
                    _TutorialCardProgress.DamageToAlLOtherMinions(selectedCard.GetComponent<CardInformation>().CardDamage, selectedCard.GetComponent<CardInformation>().CardName);
                    selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) - 2).ToString();
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    CreateTextAtTargetIndex(index, 2, true, "Claire");
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Scrap Shield")
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
                    _TutorialCardProgress.AttackerCard = selectedCard;
                    _TutorialCardProgress.ForMyCard = true;

                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Shockwave/Impulse")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    

                    for (int i = 7; i < 14; i++)
                    {
                        var cardsAreaCreator = GameObject.Find("Area").GetComponent<CardsAreaCreator>();
                        var backAreaCollision = cardsAreaCreator.BackAreaCollisions[i];

                        if (backAreaCollision.gameObject.transform.childCount != 0)
                        {
                            var cardInfo = backAreaCollision.gameObject.GetComponent<CardInformation>();

                            int cardIndex = Array.IndexOf(cardsAreaCreator.BackAreaCollisions, backAreaCollision.gameObject.transform.parent.gameObject);
                            cardInfo.CardFreeze = true;

                            var playerController = GetComponent<PlayerController>();
                            playerController.RefreshCompotitorCard(cardIndex, cardInfo.FirstTakeDamage, cardInfo.CardFreeze);
                            playerController.RefreshLog(0, true, selectedCard.GetComponent<CardInformation>().CardName, cardInfo.CardName, Color.blue);
                        }
                    }



                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");

                    // GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    // TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Garage Raid")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();
                    int randomChoice = UnityEngine.Random.Range(0, 2);

                    if (randomChoice == 0)
                    {
                        CreateDeckCard("Warlord", "6", 6, 7);
                    }
                    else
                    {
                        CreateDeckCard("Dune Raider", "5", 4, 5);
                    }

                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Radioactive Fallout")
                {
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = TurnCount / 10;
                    CompetitorManaCountText.text = (TurnCount) + "/10".ToString();
                    DeckCardCount--;

                    StackOwnDeck();

                    

                    List<Transform> allCells = new List<Transform>();
                    List<Transform> cardTransforms = new List<Transform>();

                    for (int i = 0; i < 14; i++)
                    {
                        var backAreaCollision = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].transform;
                        allCells.Add(backAreaCollision);

                        if (backAreaCollision.childCount != 0)
                        {
                            cardTransforms.Add(backAreaCollision.GetChild(0));
                        }
                    }

                    List<Transform> shuffledCells = new List<Transform>(allCells);
                    for (int i = 0; i < shuffledCells.Count; i++)
                    {
                        Transform temp = shuffledCells[i];
                        int randomIndex = UnityEngine.Random.Range(i, shuffledCells.Count);
                        shuffledCells[i] = shuffledCells[randomIndex];
                        shuffledCells[randomIndex] = temp;
                    }

                    List<int> shuffledIndexes = new List<int>();
                    int cardIndex = 0;
                    for (int i = 0; i < shuffledCells.Count; i++)
                    {
                        shuffledIndexes.Add(allCells.IndexOf(shuffledCells[i]));

                        if (shuffledCells[i].childCount == 0 && cardIndex < cardTransforms.Count)
                        {
                            cardTransforms[cardIndex].SetParent(shuffledCells[i]);
                            cardTransforms[cardIndex].localPosition = Vector3.zero;
                            cardIndex++;
                        }
                    }
                   GetComponent<PlayerController>().PV.RPC("RPC_ShuffleCells", RpcTarget.All, shuffledIndexes.ToArray());

                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;
                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Mutated Blood Sample")
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
                    _TutorialCardProgress.AttackerCard = selectedCard;
                    _TutorialCardProgress.ForMyCard = true;

                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Mechanical Reinforcement")
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
                    _TutorialCardProgress.AttackerCard = selectedCard;
                    _TutorialCardProgress.ForMyCard = true;


                    UsedSpell(selectedCard);
                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Tome of Confusion")
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
                    _TutorialCardProgress.AttackerCard = selectedCard;



                    UsedSpell(selectedCard);

                    selectedCard.SetActive(false);
                    selectedCard = null;
                    lastHoveredCard = null;

                    Debug.LogError("USSEEDD A SPEEELLL");
                
                    GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                    TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Echo of Tomorrow")
                {
                    EchoOfTomorrow();
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


            // Standart HERE

            else if (selectedCard.GetComponent<CardInformation>().CardName == "Templar Knight")
            {
                selectedCard.GetComponent<CardInformation>().DivineSelected = true;
                int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Cerberus Spawn")
            {
                GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");

                if (AllEnemyCard.Length > AllMyCard.Length)
                {
                    Debug.Log("Enemy has more cards than you. CerberusSpawnFun is activated.");
                    List<int> emptyFrontCells = new List<int>();
                    List<int> emptyBackCells = new List<int>();

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyFrontCells.Add(i);
                        }
                    }

                    if (emptyFrontCells.Count >= 2)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                            int index = emptyFrontCells[randomIndex];
                            //GameObject currentcard = CreateSpecialCard("Hellhound", "1", 1, 0, index, true);
                            //CanFirstRauntAttack(currentcard);
                            emptyFrontCells.RemoveAt(randomIndex);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                            {
                                emptyBackCells.Add(i);
                            }
                        }

                        if (emptyBackCells.Count >= 2)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                                int index = emptyBackCells[randomIndex];
                                GameObject currentcard = CreateSpecialCard("Hellhound", "1", 1, 0, index, true);
                                CanFirstRauntAttack(currentcard);
                                emptyBackCells.RemoveAt(randomIndex);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("No empty cells available to place the Scavenger card.");
                        }
                    }
                }
                else
                {
                    Debug.Log("You have equal or more cards than the enemy. CerberusSpawnFun is not activated.");
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Arcane Scholar")
            {
                SpellsExtraDamage += 2;
                CreateRandomCard();
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Rebel Outcast")
            {
                GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
                GameObject randomCard = AllEnemyCard[UnityEngine.Random.Range(0, AllEnemyCard.Length)];
                randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) - 4).ToString();
                randomCard.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, randomCard.transform.parent.gameObject);
                CreateTextAtTargetIndex(index, 2, false, "Rebel Outcast");
                
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Naiad Protector")
            {
                NaiadProtector = true;
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Ruined City Scout")
            {
                MarcoPolo();
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Gladiator Champion")
            {
                GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
                int count = AllEnemyCard.Length;

                if (count > 0)
                {
                    selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + count).ToString();
                    selectedCard.GetComponent<CardInformation>().CardDamage += count;
                    selectedCard.GetComponent<CardInformation>().SetInformation();
                    int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                   
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Urban Ranger")
            {
                GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");

                if (AllEnemyCard.Length < 2)
                {
                    Debug.LogWarning("Not enough enemy cards to target two distinct cards.");
                    return;
                }

                GameObject randomCard1 = AllEnemyCard[UnityEngine.Random.Range(0, AllEnemyCard.Length)];
                randomCard1.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard1.GetComponent<CardInformation>().CardHealth) - 2).ToString();
                randomCard1.GetComponent<CardInformation>().SetInformation();
                int index1 = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, randomCard1.transform.parent.gameObject);
                CreateTextAtTargetIndex(index1, 2, false, "Urban Ranger");                                                    //iki kart seçiyor ilkinin aynısı olmasın ve 2 hasar veriyor ikisine de

                GameObject randomCard2;
                do
                {
                    randomCard2 = AllEnemyCard[UnityEngine.Random.Range(0, AllEnemyCard.Length)];
                } while (randomCard2 == randomCard1);

                randomCard2.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard2.GetComponent<CardInformation>().CardHealth) - 2).ToString();
                randomCard2.GetComponent<CardInformation>().SetInformation();
                int index2 = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, randomCard2.transform.parent.gameObject);
                CreateTextAtTargetIndex(index2, 2, false, "Urban Ranger");
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Byzantine Fire Slinger")
            {
                _TutorialCardProgress.DamageToAlLOtherMinions(1, "Byzantine Fire Slinger");
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Shadow Assassin")
            {
                selectedCard.GetComponent<CardInformation>().SunDiskRadiance = true;
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Radiated Marauder")
            {
                selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
                Transform childTransform = selectedCard.transform;
                Transform green = childTransform.Find("Green");
                green.gameObject.SetActive(true);
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Rogue Mech-Pilot")
            {
                List<int> emptyFrontCells = new List<int>();
                List<int> emptyBackCells = new List<int>();

                for (int i = 7; i < 14; i++)
                {
                    if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                    {
                        emptyFrontCells.Add(i);
                    }
                }

                if (emptyFrontCells.Count >= 2)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                        int index = emptyFrontCells[randomIndex];
                        GameObject currentcard = CreateSpecialCard("Defective Drone", "1", 1, 0, index, true);
                        CanFirstRauntAttack(currentcard);
                        emptyFrontCells.RemoveAt(randomIndex);
                    }
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyBackCells.Add(i);
                        }
                    }

                    if (emptyBackCells.Count >= 2)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                            int index = emptyBackCells[randomIndex];
                            GameObject currentcard = CreateSpecialCard("Defective Drone", "1", 1, 0, index, true);
                            CanFirstRauntAttack(currentcard);
                            emptyBackCells.RemoveAt(randomIndex);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No empty cells available to place the Defective Drone card.");
                    }
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Rubble Raider")
            {
                selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
                Transform childTransform = selectedCard.transform;
                Transform green = childTransform.Find("Green");
                green.gameObject.SetActive(true);
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Desert Conjurer")
            {
                CreateDeckCard("Sandstorm", "0", 0, 0);
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Sandstorm")
            {
                _TutorialCardProgress.DamageToAlLOtherMinions(1, "Sandstorm");
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Gaelic Warrior")
            {
                StandartCardFuns.CanFirstRauntAttack(selectedCard);
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Oasis Guardian")
            {
                selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
                Transform childTransform = selectedCard.transform;
                Transform green = childTransform.Find("Green");
                green.gameObject.SetActive(true);
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Battle Mage")
            {
                SpellsExtraDamage += 1;
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Roving Merchant")
            {
                GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                if (AllMyCard.Length >= 3)
                {
                    SpawnAndReturnGameObject();
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Nomadic Hunter")
            {
                if (PlayedSpell)
                {
                    CanFirstRauntAttack(selectedCard);
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Raiding Party")
            {
                List<int> emptyFrontCells = new List<int>();
                List<int> emptyBackCells = new List<int>();

                for (int i = 7; i < 14; i++)
                {
                    if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                    {
                        emptyFrontCells.Add(i);
                    }
                }

                if (emptyFrontCells.Count >= 2)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                        int index = emptyFrontCells[randomIndex];
                        GameObject currentcard = CreateSpecialCard("Pirate", "1", 1, 0, index, true);
                        CanFirstRauntAttack(currentcard);
                        emptyFrontCells.RemoveAt(randomIndex);
                    }
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyBackCells.Add(i);
                        }
                    }

                    if (emptyBackCells.Count >= 2)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                            int index = emptyBackCells[randomIndex];
                            GameObject currentcard = CreateSpecialCard("Pirate", "1", 1, 0, index, true);
                            CanFirstRauntAttack(currentcard);
                            emptyBackCells.RemoveAt(randomIndex);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No empty cells available to place the Pirate card.");
                    }
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Tavern Brawler")
            {
                selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
                Transform childTransform = selectedCard.transform;
                Transform green = childTransform.Find("Green");
                green.gameObject.SetActive(true);
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Pyromaniac Wizard")
            {
                _TutorialCardProgress.DamageToAlLOtherMinions(1, "Pyromaniac Wizard");
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Frontline Militia")
            {
                GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                if (AllMyCard.Length > 0)
                {
                    selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + AllMyCard.Length).ToString();
                    selectedCard.GetComponent<CardInformation>().SetInformation();
                    int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Wandering Healer")
            {
                GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                foreach (GameObject Card in AllMyCard)
                {
                    if (Card != selectedCard)
                    {
                        Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) + 3).ToString();
                        Card.GetComponent<CardInformation>().SetInformation();
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, Card.transform.parent.gameObject);
                    }  
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Horse Archer")
            {
                    selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
                    Transform childTransform = selectedCard.transform;
                    Transform green = childTransform.Find("Green");
                    green.gameObject.SetActive(true);
                }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Viking Shield-Maiden")
            {
                    GameObject[] allMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                    foreach (GameObject Card in allMyCard)
                    {
                        if (Card.GetComponent<CardInformation>().CardName == "Viking Raider" || Card.GetComponent<CardInformation>().CardName == "Viking" || Card.GetComponent<CardInformation>().CardName == "Viking Spirit")
                        {
                            selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                            selectedCard.GetComponent<CardInformation>().CardDamage++;
                            selectedCard.GetComponent<CardInformation>().SetInformation();
                            int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                            
                            return;
                        }
                    }
                }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Plague Carrier")
            {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                _TutorialCardProgress.SecoundTargetCard = true;
                _TutorialCardProgress.SetAttackerCard(index);
                _TutorialCardProgress.AttackerCard = selectedCard;
                }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Dune Scout")
            {
                GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
                if (AllEnemyCard.Length == 0)
                {
                    CanFirstRauntAttack(selectedCard);
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Scavenger's Daughter")
            {
                GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                foreach (GameObject Card in AllMyCard)
                {
                    if (Card != selectedCard)
                    {
                        Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                        Card.GetComponent<CardInformation>().SetInformation();
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, Card.transform.parent.gameObject);
                      
                    }

                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Mystic Archer")
            {
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                _TutorialCardProgress.SecoundTargetCard = true;
                _TutorialCardProgress.SetAttackerCard(index);
                _TutorialCardProgress.AttackerCard = selectedCard;
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Toxic Rainmaker")
            {
                GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
                foreach (GameObject card in AllEnemyCard)
                {
                    card.GetComponent<CardInformation>().CardDamage -= 1;
                    card.GetComponent<CardInformation>().SetInformation();
                    card.GetComponent<CardInformation>().ToxicRainmaker = true;
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, card.transform.parent.gameObject);
                    
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Desert Warlock")
            {
                List<int> emptyFrontCells = new List<int>();
                List<int> emptyBackCells = new List<int>();

                for (int i = 7; i < 14; i++)
                {
                    if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                    {
                        emptyFrontCells.Add(i);
                    }
                }

                if (emptyFrontCells.Count >= 2)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                        int index = emptyFrontCells[randomIndex];
                        GameObject currentcard = CreateSpecialCard("Sand Elementals", "2", 2, 0, index, true);
                        CanFirstRauntAttack(currentcard);
                        emptyFrontCells.RemoveAt(randomIndex);
                    }
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyBackCells.Add(i);
                        }
                    }

                    if (emptyBackCells.Count >= 2)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                            int index = emptyBackCells[randomIndex];
                            GameObject currentcard = CreateSpecialCard("Sand Elementals", "2", 2, 0, index, true);
                            CanFirstRauntAttack(currentcard);
                            emptyBackCells.RemoveAt(randomIndex);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No empty cells available to place the Sand Elementals card.");
                    }
                }
                LessSpellCost += 1;
            }
            
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Merfolk Scout")
            {
                MerfolkScout();
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Street Thug")
            {
                GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
                if (AllEnemyCard.Length == 1)
                {
                    selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                    selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                    selectedCard.GetComponent<CardInformation>().SetInformation();
                    int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                    
                }
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Ancient Librarian")
            {
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                _TutorialCardProgress.SecoundTargetCard = true;
                _TutorialCardProgress.SetAttackerCard(index);
                _TutorialCardProgress.AttackerCard = selectedCard;
                
            }
            else if (selectedCard.GetComponent<CardInformation>().CardName == "Apprentice Sorcerer")
            {
                GameObject CurrentCard;

                do
                {
                    CurrentCard = CreateRandomCard();
                    if (CurrentCard.GetComponent<CardInformation>().CardMana > 3)
                    {
                        CreateSpell(CurrentCard);
                    }
                }
                while (CurrentCard.GetComponent<CardInformation>().CardMana > 3);
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
    public void RuneMagic()
    {
        
            healthTextObject.SetActive(true);
            attackTextObject.SetActive(true);
            drawCardTextObject.SetActive(true);

            healthTextObject.GetComponent<Button>().onClick.AddListener(() => OnTextClicked(healthTextObject));
            attackTextObject.GetComponent<Button>().onClick.AddListener(() => OnTextClicked(attackTextObject));
            drawCardTextObject.GetComponent<Button>().onClick.AddListener(() => OnTextClicked(drawCardTextObject));
       
    }
    void OnTextClicked(GameObject clickedTextObject)
    {
        if (clickedTextObject == healthTextObject)
        {
            GetComponent<CardController>().AddHealCard(2,true);
        }
        else if (clickedTextObject == attackTextObject)
        {
            GetComponent<CardController>().UsedCard(2, GetComponent<PlayerController>().PV.Owner.IsMasterClient);
        }
        else if (clickedTextObject == drawCardTextObject)
        {
            SpawnAndReturnGameObject();
        }

        healthTextObject.SetActive(false);
        attackTextObject.SetActive(false);
        drawCardTextObject.SetActive(false);
    }
    public void CreateSpell(GameObject CardCurrent)
    {
        List<Spell> spells = null;
        if (OwnMainCard == "Zeus")
        {
            ZeusCard zeusCard = new ZeusCard();
            spells = zeusCard.spells;
        }
        else if (OwnMainCard == "Odin")
        {
            OdinCard odinCard = new OdinCard();
            spells = odinCard.spells;
        }
        else if (OwnMainCard == "Genghis")
        {
            GenghisCard genghisCard = new GenghisCard();
            spells = genghisCard.spells;
        }
        else if (OwnMainCard == "Anubis")
        {
            AnubisCard anubisCard = new AnubisCard();
            spells = anubisCard.spells;
        }
        else if (OwnMainCard == "Dustin")
        {
            DustinCard dustinCard = new DustinCard();
            spells = dustinCard.spells;
        }
        else if (OwnMainCard == "Leonardo Da Vinci")
        {
            LeonardoCard leonardoCard = new LeonardoCard();
            spells = leonardoCard.spells;
        }

        if (spells != null && spells.Count > 0)
        {
            // Rastgele bir büyü seçiyoruz.
            int spellIndex = UnityEngine.Random.Range(0, spells.Count);
            Spell selectedSpell = spells[spellIndex];

            // Seçilen büyüyü CardCurrent nesnesine uyguluyoruz.
            CardCurrent.GetComponent<CardInformation>().CardName = selectedSpell.name;
            CardCurrent.GetComponent<CardInformation>().CardDes = selectedSpell.name + " POWWERRRRR!!!";
            CardCurrent.GetComponent<CardInformation>().CardHealth = "";
            CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
            CardCurrent.GetComponent<CardInformation>().CardMana = selectedSpell.mana;

            // Kart bilgilerinin güncellenmesini sağlıyoruz.
            CardCurrent.GetComponent<CardInformation>().SetInformation();
        }
        else
        {
            Debug.LogError("Spell listesi boş veya geçersiz bir kart seçildi.");
        }
    }
    public void MerfolkScout()
    {
        if (GameObject.Find("Deck").transform.childCount < 10)
        {
            Vector3[] positions = new Vector3[3];
            float yPosition = 2.7f;
            float zPosition = -2.7f;

            Vector3 screenPos1 = new Vector3(0f, yPosition, zPosition);

            positions[0] = screenPos1;

            Quaternion rotation = Quaternion.Euler(80f, 0f, 180f);
            GameObject CardCurrent = Instantiate(CardPrefabSolo, positions[0], rotation);
            CardCurrent.AddComponent<Button>();
            CreateInfoCard(CardCurrent);
            MerfolkScoutBool = true;
            NextCard = CardCurrent;
            CardCurrent.GetComponent<Button>().onClick.AddListener(() => Destroy(CardCurrent));
        }
        else
            Debug.Log("Desten Dolu");
    }
    public void MarcoPolo()
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
    public void ScavengerRaider()
    {
        
        
            drawCardTextObject.SetActive(true);
            TakeGun.SetActive(true);

            drawCardTextObject.GetComponent<Button>().onClick.AddListener(() => OnGameobjectClicked2(drawCardTextObject));
            TakeGun.GetComponent<Button>().onClick.AddListener(() => OnGameobjectClicked2(TakeGun));
        
    }
    void OnGameobjectClicked2(GameObject clickedTextObject)
    {
        if (clickedTextObject == drawCardTextObject)
        {
            SpawnAndReturnGameObject();
        }
        else if (clickedTextObject == TakeGun)
        {
            CreateDeckCard("Gun", "2", 2, 0);

        }

        drawCardTextObject.SetActive(false);
        TakeGun.SetActive(false);
    }
    public static void CanFirstRauntAttack(GameObject selectedCard)
    {
        selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
        Transform childTransform = selectedCard.transform;
        Transform green = childTransform.Find("Green");
        green.gameObject.SetActive(true);
    }
    public void EchoOfTomorrow()
    {
        if (UsedSpellCard.Count > 0)
        {
            if (GameObject.Find("Deck").transform.childCount < 10)
            {
                // Son büyü kartını seçiyoruz
                GameObject lastSpellCard = UsedSpellCard[UsedSpellCard.Count - 1];

                // Kartı oluşturuyoruz
                GameObject CardCurrent = Instantiate(lastSpellCard, GameObject.Find("Deck").transform);
                CardCurrent.tag = "Card";
                CardCurrent.GetComponent<CardInformation>().CardMana -= 3;
                if (CardCurrent.GetComponent<CardInformation>().CardMana < 0)
                {
                    CardCurrent.GetComponent<CardInformation>().CardMana = 0;
                }
                CardCurrent.GetComponent<CardInformation>().SetInformation();

                float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz

                DeckCardCount++;
            }
        }
    }
    public void CreateDeckCard(string name, string health, int damage, int mana)
    {
        GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
        CardCurrent.GetComponent<CardInformation>().CardName = name;
        CardCurrent.GetComponent<CardInformation>().CardHealth = health;
        CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
        CardCurrent.GetComponent<CardInformation>().CardMana = mana;

        StackOwnDeck();
        StackCompetitorDeck();
        DeckCardCount++;

       
    }
    public void UsedSpell(GameObject SpellCard)
    {
       
        PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
        foreach (PlayerController obj in objects)
        {
            if (obj.name == "PlayerController(Clone)"  && obj.GetComponent<PlayerController>().SteppeAmbush == true)
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

                    CreateSpecialCard("Horse Archer", "2", 3, 0, randomIndex, false);
                    obj.GetComponent<PlayerController>().SteppeAmbush = false;
                }
                else
                {
                    Debug.LogWarning("No empty cells available to place the horse archer card.");
                }
            }
        }
        GameObject[] myCards = GameObject.FindGameObjectsWithTag("UsedCard"); // RAKİBİN BÜTÜN KARTLARINI AL


        foreach (var Card in myCards)
        {
            if (Card.GetComponent<CardInformation>().CardName == "Anatomist of the Unknown")
            {
                if (myCards.Length > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, myCards.Length);
                    GameObject randomCard = myCards[randomIndex];
                    int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, randomCard.transform.parent.gameObject);
                    randomCard.GetComponent<CardInformation>().CardDamage += 2;
                    randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                    
                }
                else if (Card.GetComponent<CardInformation>().CardName == "Automaton Apprentice")
                {
                    Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                    Card.GetComponent<CardInformation>().SetInformation();
                }
                else
                {
                    Debug.LogWarning("Hiç kart bulunamadı.");
                }
            }
        }
        AsgardQuestion++;

        if (NaiadProtector)
        {
            GetComponent<CardController>().AddHealCard(1,true);
        }

        UsedSpellCard.Add(SpellCard);

        PlayedSpell = true;
    }
    public void RefreshUsedCard(int boxindex, string heatlh, int damage)
    {
        GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject.GetComponent<CardInformation>().SetInformation();

    }
    public void DamageToAlLOtherMinionsForLocust()
    {

        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKİBİN BÜTÜN KARTLARINI AL

        foreach (var Card in allTargets)
        {

            if (Card.GetComponent<CardInformation>().CardHealth != "") // MİNİYON OLUP OLMADIĞINI ÖĞREN - ŞARTI SAĞLIYORSA MİNYONDUR.
            {
                int CurrentCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);

                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) - 1).ToString(); //  İKİ DAMAGE VURUYOR
                RefreshUsedCard(CurrentCardIndex, Card.GetComponent<CardInformation>().CardHealth, Card.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                CreateTextAtTargetIndex(CurrentCardIndex, 1, false, name);

                if (int.Parse(Card.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
                {
                    List<int> emptyFrontCells = new List<int>();
                    List<int> emptyBackCells = new List<int>();

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyFrontCells.Add(i);
                        }
                    }

                    if (emptyFrontCells.Count > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                        int index = emptyFrontCells[randomIndex];
                        CreateSpecialCard("Plague of Locusts", "1", 1, 0, index, true);
                        emptyFrontCells.RemoveAt(randomIndex);
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                            {
                                emptyBackCells.Add(i);
                            }
                        }

                        if (emptyBackCells.Count > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                            int index = emptyBackCells[randomIndex];
                            CreateSpecialCard("Plague of Locusts", "1", 1, 0, index, true);
                            emptyBackCells.RemoveAt(randomIndex);
                        }
                        else
                        {
                            Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
                        }
                    }
                    GetComponent<PlayerController>().DeleteAreaCard(CurrentCardIndex);
                    GetComponent<PlayerController>().RefreshLog(-1, true, "Plague of Locusts", Card.GetComponent<CardInformation>().CardName, Color.red);
                }
                else
                    GetComponent<PlayerController>().RefreshLog(-1, false, "Plague of Locusts", Card.GetComponent<CardInformation>().CardName, Color.red);


            }
        }
    }
    public void DestroyAndCreateMyDeck(GameObject CardCurrent)
    {

        GameObject deckObject = GameObject.Find("Deck");
        if (deckObject.transform.childCount < 10)
        {
            string cardName = CardCurrent.GetComponent<CardInformation>().CardName;
            string cardDes = cardName + " POWWERRRRR!!!";
            string cardHealth = CardCurrent.GetComponent<CardInformation>().CardHealth;
            int cardDamage = CardCurrent.GetComponent<CardInformation>().CardDamage;
            int cardMana = CardCurrent.GetComponent<CardInformation>().CardMana;


            GameObject Card = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);

            float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
            Card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz

            Card.GetComponent<CardInformation>().CardName = cardName;
            Card.GetComponent<CardInformation>().CardDes = cardDes;
            Card.GetComponent<CardInformation>().CardHealth = cardHealth;
            Card.GetComponent<CardInformation>().CardDamage = cardDamage;
            Card.GetComponent<CardInformation>().CardMana = cardMana;
            Card.GetComponent<CardInformation>().SetInformation();


            StackOwnDeck();
            StackCompetitorDeck();
            DeckCardCount++;
            GetComponent<PlayerController>().PV.RPC("RPC_CreateRandomCard", RpcTarget.All);
        }
        Destroy(CardCurrent);
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, CardCurrent.transform.parent.gameObject);
        GetComponent<PlayerController>().PV.RPC("RPC_DeleteMyCard", RpcTarget.All, index);

    }
    public int CheckUndeadCards()
    {
        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("UsedCard");
        int count = 0;
        foreach (var card in AllOwnCards)
        {
            if (card.GetComponent<CardInformation>().CardName == "Royal Mummy" || card.GetComponent<CardInformation>().CardName == "Mummy")
            {
                count++;
            }
        }
        return count;
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
                //_CardFunction.SelectFirstCard(hit.collider.gameObject);
                Debug.LogError(hit.collider.gameObject.transform.parent);
                if (hit.collider.gameObject.GetComponent<CardInformation>().CardFreeze == false && hit.collider.gameObject.GetComponent<CardInformation>().isItFirstRaound == false && hit.collider.gameObject.GetComponent<CardInformation>().isAttacked == false)
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
            else if (hit.collider.gameObject.tag == "AreaBox" && _TutorialCardProgress.ForMyCard == false)
            {
                if (hit.collider.gameObject.transform.childCount > 0)
                {
                    Transform firstChild = hit.collider.gameObject.transform.GetChild(0);

                    if (firstChild.tag == "UsedCard")
                    {
                        if (firstChild.GetComponent<CardInformation>().CardFreeze == false &&
                            firstChild.GetComponent<CardInformation>().isItFirstRaound == false &&
                            firstChild.GetComponent<CardInformation>().isAttacked == false)
                        {
                            _TutorialCardProgress.CloseBlueSign();

                           
                        }
                        else
                        {
                            Debug.Log("nocard");
                        }
                    }
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
                   
                    if (card.GetComponent<CardInformation>().CardName == "Brokk and Sindri")
                    {
                        card.GetComponent<CardInformation>().ChargeBrokandSindri++;
                        if (card.GetComponent<CardInformation>().ChargeBrokandSindri == 3)
                        {
                            _TutorialCardProgress.CharceBrokandSindri();
                        }
                    }
                    if (card.GetComponent<CardInformation>().MutatedBlood == true)
                    {
                        GameObject[] RandomEnemyCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
                        if (RandomEnemyCards.Length > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, RandomEnemyCards.Length);
                            GameObject selectedEnemyCard = RandomEnemyCards[randomIndex];
                            _TutorialCardProgress.StandartDamage(card, selectedEnemyCard);
                        }
                    }
                    
                    if (card.GetComponent<CardInformation>().CardName == "Gaelic Warrior")
                    {
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                        DeleteAreaCard(TargetCardIndex);
                    }
                    if (card.GetComponent<CardInformation>().CardName == "Wall Builder")
                    {
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                        card.GetComponent<CardInformation>().SetInformation();
                        
                    }
                    if (card.GetComponent<CardInformation>().CardName == "Tavern Brawler")
                    {
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                        card.GetComponent<CardInformation>().SetInformation();
                        
                    }
                    if (card.GetComponent<CardInformation>().CardName == "Berserker Thrall")
                    {
                        GameObject[] RandomEnemyCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
                        if (RandomEnemyCards.Length > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, RandomEnemyCards.Length);
                            GameObject selectedEnemyCard = RandomEnemyCards[randomIndex];
                            _TutorialCardProgress.StandartDamage(card, selectedEnemyCard);
                        }
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
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("UsedCard");
        WhoseTurnText.text = "Enemy Turn";
        Timer.text = "00:60";
        elapsedTime = 60;
        finishButton.interactable = false;
        CanAttackMainCard = true;
        foreach (GameObject obj in objects)
        {
            if (obj.name == "PlayerController(Clone)")
            {

                if (true)
                {
                    CompetitorPV = obj;
                }
            }
        }

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
            if (card.GetComponent<CardInformation>().MongolFury == true)
            {
                card.GetComponent<CardInformation>().CardDamage -= 2;
                card.GetComponent<CardInformation>().MongolFury = false;
                card.GetComponent<CardInformation>().SetInformation();
                
            }
            if (card.GetComponent<CardInformation>().CardName == "Dwarven Blacksmith")
            {
                GameObject randomCard = AllOwnCards[UnityEngine.Random.Range(0, AllOwnCards.Length)];
                randomCard.GetComponent<CardInformation>().CardDamage += 2;
                randomCard.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, randomCard.transform.parent.gameObject);
               
            }
            if (card.GetComponent<CardInformation>().CardName == "Thor")
            {
                _TutorialCardProgress.DamageToAlLOtherMinions(1, "Thor");
            }
            if (card.GetComponent<CardInformation>().Gallop == true)
            {
                card.GetComponent<CardInformation>().Gallop = false;
                DestroyAndCreateMyDeck(card);
            }
            if (card.GetComponent<CardInformation>().CardName == "Codex Guardian")
            {
                GetComponent<CardController>().AddHealCard(2, true);
            }
            if (card.GetComponent<CardInformation>().CardName == "Grand Cannon")
            {
                GameObject[] AllEnemyCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
                if (AllEnemyCards.Length > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, AllEnemyCards.Length);
                    GameObject randomEnemyCard = AllEnemyCards[randomIndex];
                    int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, randomEnemyCard.transform.parent.gameObject);
                    randomEnemyCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomEnemyCard.GetComponent<CardInformation>().CardHealth) - 2).ToString();
                    RefreshUsedCard(TargetCardIndex, randomEnemyCard.GetComponent<CardInformation>().CardHealth, randomEnemyCard.GetComponent<CardInformation>().CardDamage);
                    CreateTextAtTargetIndex(TargetCardIndex, 2, false, "Grand Cannon");
                    if (int.Parse(randomEnemyCard.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
                    {

                        GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                        GetComponent<PlayerController>().RefreshLog(-2, true, "Grand Cannon", randomEnemyCard.GetComponent<CardInformation>().CardName, Color.red);
                    }
                    else
                        GetComponent<PlayerController>().RefreshLog(-2, false, "Grand Cannon", randomEnemyCard.GetComponent<CardInformation>().CardName, Color.red);
                }
                else
                {
                    Debug.LogWarning("Hiç düşman kartı bulunamadı.");
                }
            }
            if (card.GetComponent<CardInformation>().Behemot)
            {
                card.GetComponent<CardInformation>().Behemot = false;
                card.GetComponent<CardInformation>().CardDamage = card.GetComponent<CardInformation>().MaxAttack;
                card.GetComponent<CardInformation>().SetInformation();
            }
            if (card.GetComponent<CardInformation>().CardName == "Eques Automaton")
            {
                card.GetComponent<CardInformation>().CardHealth = card.GetComponent<CardInformation>().MaxHealth;
                card.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
               
            }
            if (card.GetComponent<CardInformation>().CardName == "Scrapyard Engineer")
            {
                GameObject randomCard = AllOwnCards[UnityEngine.Random.Range(0, AllOwnCards.Length)];
                randomCard.GetComponent<CardInformation>().CardDamage += 2;
                randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                randomCard.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, randomCard.transform.parent.gameObject);
                
            }
            if (card.GetComponent<CardInformation>().CardName == "Forest Nymph")
            {
                GameObject[] MyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                foreach (GameObject cards in MyCard)
                {
                    if (cards.GetComponent<CardInformation>().CardName != "Forest Nymph")
                    {
                        cards.GetComponent<CardInformation>().CardHealth = (int.Parse(cards.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                        cards.GetComponent<CardInformation>().SetInformation();
                        //int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, cards.transform.parent.gameObject);
                        //RefreshMyCard(index,
                        //    cards.GetComponent<CardInformation>().CardHealth,
                        //    cards.GetComponent<CardInformation>().HaveShield,
                        //    cards.GetComponent<CardInformation>().CardDamage,
                        //    cards.GetComponent<CardInformation>().DivineSelected,
                        //    cards.GetComponent<CardInformation>().FirstTakeDamage,
                        //    cards.GetComponent<CardInformation>().FirstDamageTaken,
                        //    cards.GetComponent<CardInformation>().EternalShield);
                    }
                }
            }
            if (card.GetComponent<CardInformation>().CardName == "Scrap Collector")
            {
                GameObject randomCard = AllOwnCards[UnityEngine.Random.Range(0, AllOwnCards.Length)];
                randomCard.GetComponent<CardInformation>().CardDamage += 1;
                randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                randomCard.GetComponent<CardInformation>().SetInformation();
               
            }
            if (card.GetComponent<CardInformation>().CardName == "Chronomancer Cleopatra")
            {
                if (UsedSpellCard.Count != 0)
                {
                    if (GameObject.Find("Deck").transform.childCount < 10)
                    {
                        System.Random random = new System.Random();
                        int randomIndex = random.Next(UsedSpellCard.Count);
                        GameObject selectedSpellCard = UsedSpellCard[randomIndex];

                        GameObject CardCurrent = Instantiate(selectedSpellCard, GameObject.Find("Deck").transform);
                        CardCurrent.tag = "Card";

                        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz

                        DeckCardCount++;
                    }
                }
            }

            if (card.GetComponent<CardInformation>().PlagueCarrier && PlagueCarrierBool)
            {
                card.GetComponent<CardInformation>().PlagueCarrier = false;
                PlagueCarrierBool = false;
                card.GetComponent<CardInformation>().CardDamage += 2;
                card.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, card.transform.parent.gameObject);
                RefreshUsedCard(index, card.GetComponent<CardInformation>().CardHealth, card.GetComponent<CardInformation>().CardDamage);
            }
            else
            {
                PlagueCarrierBool = true;
            }
            if (card.GetComponent<CardInformation>().ToxicRainmaker && PlagueCarrierBool)
            {
                card.GetComponent<CardInformation>().ToxicRainmaker = false;
                PlagueCarrierBool = false;
                card.GetComponent<CardInformation>().CardDamage += 1;
                card.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, card.transform.parent.gameObject);
                RefreshUsedCard(index, card.GetComponent<CardInformation>().CardHealth, card.GetComponent<CardInformation>().CardDamage);
            }
            else
            {
                PlagueCarrierBool = true;
            }

        }
        if (AsgardQuestion >= 3)
        {
            List<GameObject> selectedCards = new List<GameObject>();

            while (selectedCards.Count < 2)
            {
                int randomIndex = UnityEngine.Random.Range(0, AllOwnCards.Length);
                GameObject randomCard = AllOwnCards[randomIndex];

                if (!selectedCards.Contains(randomCard))
                {
                    selectedCards.Add(randomCard);
                }
            }

            foreach (GameObject card in selectedCards)
            {
                card.GetComponent<CardInformation>().DivineSelected = true;
            }
        }
        if (AugmentCount >= 4)
        {
            for (int i = 0; i < 2; i++)
            {
                if (GameObject.Find("Deck").transform.childCount < 10)
                {
                    int health = UnityEngine.Random.Range(1, 3);
                    int damage = UnityEngine.Random.Range(1, 3);

                    CreateDeckCard("Mechanical Reinforcement", health.ToString(), damage, 1);
                }
            }
        }
        PlayedSpell = false;
        _TutorialCardProgress.WindFury = true;
        _TutorialCardProgress.ResetAllSign();

    
        _TutorialCardProgress.WindFury = true;
        _TutorialCardProgress.ResetAllSign();

       
       
      
       GetComponent<BotController>().BotAttack();

        _TutorialCardProgress.AttackerCard = null;
        _TutorialCardProgress.TargetCard = null;
        _TutorialCardProgress.TargetCardIndex = -1;
        _TutorialCardProgress.SecoundTargetCard = false;
        _TutorialCardProgress.ForMyCard = false;


    }
    public void RefreshMyCard(int boxindex, string heatlh, bool haveshield, int damage, bool divineselected, bool firstdamage, bool firstdamagetaken, bool eternalshield)
    {
        GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject.GetComponent<CardInformation>().SetInformation();       //kendi kart bilgimi güncelleme
    }

    public void CreateAnCard()
    {
        GameObject deckObject = GameObject.Find("Deck");

        
        // Kart sayısını kontrol ediyoruz
        if (deckObject.transform.childCount < 10)
        {
            GameObject card = Instantiate(CardPrefabSolo, deckObject.transform);
            card.tag = "Card";

            float xPos = deckObject.transform.childCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
            card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz

            CreateInfoCard(card);
            StackOwnDeck();

            DeckCardCount++;
        }
        else
        {
            Debug.Log("Kart Sınırına Ulaştınız.");
        }
    }


    public void CreateAnCompetitorCard()
    {
        GameObject deckObject = GameObject.Find("CompetitorDeck");

       
        // Kart sayısını kontrol ediyoruz
        if (deckObject.transform.childCount < 10)
        {
            GameObject card = Instantiate(Resources.Load<GameObject>("TutorialCompetitorCard"), deckObject.transform);
            card.tag = "CompetitorDeckCard";

            float xPos = deckObject.transform.childCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
            card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz

            StackCompetitorDeck();

            CompetitorDeckCardCount++;
        }
        else
        {
            Debug.Log("Bot Kart Sınırına Ulaştı");
        }
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

        GameObject.Find("CompetitorDeck").transform.position = new Vector3(3.3f - GameObject.Find("CompetitorDeck").transform.childCount * 0.2f, 1.26f, 1.54f);

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
    

    public void CreateUsedCard(int boxindex, string name, string des, string heatlh, int damage, int mana, CardInformation.Rarity rarity)
    {

        
            GameObject CardCurrent = Instantiate(CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[boxindex].transform);
            CardCurrent.tag = "UsedCard";

            //  CardCurrent.GetComponent<PhotonView>().ViewID = OwnDeck.Length;
            if (name == "Crypt Warden" || name == "Chaos Scarab" || name == "Gyrocopter" || name == "Piscean Diver" || name == "Rebel Outcast" || name == "Urban Ranger" || name == "Shadow Assassin" || name == "Elven Tracker")
            {
                CardCurrent.SetActive(false);
            }
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
            CardCurrent.GetComponent<CardInformation>().AssignMaterialByRarity();

            StartCoroutine(MoveAndRotateCard(CardCurrent, CardCurrent.transform.position, 0.3f));


    }

    void StackOwnDeck()
    {

        float yOffset = 0f; // Başlangıç z pozisyonu

        if (GameObject.Find("Deck").transform.childCount < 6)
        {
            for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++)
            {
                float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz

                GameObject.Find("Deck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, yOffset, 0); // Kartın pozisyonunu ayarlıyoruz

                yOffset += 0.01f; // Z pozisyonunu her kart için 0.01 artırıyoruz
            }

            GameObject.Find("Deck").transform.position = new Vector3(3.35f - GameObject.Find("Deck").transform.childCount * 0.2f, 0.9f, -1.09f);
        }
        else if (GameObject.Find("Deck").transform.childCount < 10)
        {
            yOffset = 0f; // Z pozisyonunu sıfırlıyoruz

            for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++)
            {
                float xPos = i * 0.4f - 0.4f; // Kartın X konumunu belirliyoruz

                GameObject.Find("Deck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, yOffset, 0); // Kartın pozisyonunu ayarlıyoruz
                GameObject.Find("Deck").transform.GetChild(i).transform.eulerAngles = new Vector3(60.8931351f, 351.836639f, 174.237427f);

                yOffset += 0.01f; // Z pozisyonunu her kart için 0.01 artırıyoruz
            }

            GameObject.Find("Deck").transform.position = new Vector3(3.02f - GameObject.Find("Deck").transform.childCount * 0.1f, 0.9f, -1.09f);
        }
        else
        {
            yOffset = 0f; // Z pozisyonunu sıfırlıyoruz

            for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++)
            {
                float xPos = i * 0.3f - 0.3f; // Kartın X konumunu belirliyoruz

                GameObject.Find("Deck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, yOffset, 0); // Kartın pozisyonunu ayarlıyoruz
                GameObject.Find("Deck").transform.GetChild(i).transform.eulerAngles = new Vector3(60.8471832f, 350.247925f, 173.120972f);

                yOffset += 0.01f; // Z pozisyonunu her kart için 0.01 artırıyoruz
            }

            GameObject.Find("Deck").transform.position = new Vector3(2.80f - GameObject.Find("Deck").transform.childCount * 0.05f, 0.9f, -1.09f);
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
                        CardCurrent.GetComponent<CardInformation>().rarity = (CardInformation.Rarity)zeusCard.minions[targetIndex].rarity;
                        CardCurrent.GetComponent<CardInformation>().AssignMaterialByRarity();
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

                CreateStandartCards(CardCurrent, targetCardName);
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
                        CardCurrent.GetComponent<CardInformation>().rarity = (CardInformation.Rarity)genghisCard.minions[GenghistargetIndex].rarity;
                        CardCurrent.GetComponent<CardInformation>().AssignMaterialByRarity();
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
                CreateStandartCards(CardCurrent, GenghistargetCardName);
                break;
            case "Odin":
                OdinCard odinCard = new OdinCard();

                int OdinCardIndex = UnityEngine.Random.Range(1, OwnDeck.Length); // 1 DEN BAŞLIYOR ÇÜNKĞ İNDEX 0 HEROMUZ
                string OdintargetCardName = OwnDeck[OdinCardIndex]; // Deste içinden gelen kart isminin miniyon mu buyu mu olduğunu belirle daha sonra özelliklerini getir.
                int OdintargetIndex = -1;

                for (int i = 0; i < odinCard.minions.Count; i++)
                {
                    if (odinCard.minions[i].name == OdintargetCardName)
                    {
                        OdintargetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = odinCard.minions[OdintargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = odinCard.minions[OdintargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = odinCard.minions[OdintargetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = odinCard.minions[OdintargetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = odinCard.minions[OdintargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().rarity = (CardInformation.Rarity)odinCard.minions[OdintargetIndex].rarity;
                        CardCurrent.GetComponent<CardInformation>().AssignMaterialByRarity();
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < odinCard.spells.Count; i++)
                {
                    if (odinCard.spells[i].name == OdintargetCardName)
                    {
                        OdintargetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = odinCard.spells[OdintargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = odinCard.spells[OdintargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = odinCard.spells[OdintargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                //if (CardCurrent.GetComponent<CardInformation>().CardName == "Naglfar")
                //{
                //    if (DeadCardCount >= 6)
                //    {
                //        CardCurrent.GetComponent<CardInformation>().CardMana -= 3;
                //        CardCurrent.GetComponent<CardInformation>().SetInformation();
                //    }
                //}
                CreateStandartCards(CardCurrent, OdintargetCardName);
                break;
            case "Anubis":
                AnubisCard anubisCard = new AnubisCard();

                int AnubisCardIndex = UnityEngine.Random.Range(1, OwnDeck.Length); // 1 DEN BAŞLIYOR ÇÜNKĞ İNDEX 0 HEROMUZ
                string AnubistargetCardName = OwnDeck[AnubisCardIndex]; // Deste içinden gelen kart isminin miniyon mu buyu mu olduğunu belirle daha sonra özelliklerini getir.
                int AnubistargetIndex = -1;

                for (int i = 0; i < anubisCard.minions.Count; i++)
                {
                    if (anubisCard.minions[i].name == AnubistargetCardName)
                    {
                        AnubistargetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = anubisCard.minions[AnubistargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = anubisCard.minions[AnubistargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = anubisCard.minions[AnubistargetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = anubisCard.minions[AnubistargetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = anubisCard.minions[AnubistargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().rarity = (CardInformation.Rarity)anubisCard.minions[AnubistargetIndex].rarity;
                        CardCurrent.GetComponent<CardInformation>().AssignMaterialByRarity();
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < anubisCard.spells.Count; i++)
                {
                    if (anubisCard.spells[i].name == AnubistargetCardName)
                    {
                        AnubistargetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = anubisCard.spells[AnubistargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = anubisCard.spells[AnubistargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = anubisCard.spells[AnubistargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                CreateStandartCards(CardCurrent, AnubistargetCardName);
                break;
            case "Leonardo Da Vinci":
                LeonardoCard leonardoCard = new LeonardoCard();

                int LeonardoCardIndex = UnityEngine.Random.Range(1, OwnDeck.Length); // 1 DEN BAŞLIYOR ÇÜNKĞ İNDEX 0 HEROMUZ
                string LeonardoTargetCardName = OwnDeck[LeonardoCardIndex]; // Deste içinden gelen kart isminin miniyon mu buyu mu olduğunu belirle daha sonra özelliklerini getir.
                int LeonardotargetIndex = -1;

                for (int i = 0; i < leonardoCard.minions.Count; i++)
                {
                    if (leonardoCard.minions[i].name == LeonardoTargetCardName)
                    {
                        LeonardotargetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = leonardoCard.minions[LeonardotargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = leonardoCard.minions[LeonardotargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = leonardoCard.minions[LeonardotargetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = leonardoCard.minions[LeonardotargetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = leonardoCard.minions[LeonardotargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().rarity = (CardInformation.Rarity)leonardoCard.minions[LeonardotargetIndex].rarity;
                        CardCurrent.GetComponent<CardInformation>().AssignMaterialByRarity();
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < leonardoCard.spells.Count; i++)
                {
                    if (leonardoCard.spells[i].name == LeonardoTargetCardName)
                    {
                        LeonardotargetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = leonardoCard.spells[LeonardotargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = leonardoCard.spells[LeonardotargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = leonardoCard.spells[LeonardotargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                CreateStandartCards(CardCurrent, LeonardoTargetCardName);
                break;
            case "Dustin":
                DustinCard dustinCard = new DustinCard();

                int DustinCardIndex = UnityEngine.Random.Range(1, OwnDeck.Length); // 1 DEN BAŞLIYOR ÇÜNKĞ İNDEX 0 HEROMUZ
                string DustinTargetCardName = OwnDeck[DustinCardIndex]; // Deste içinden gelen kart isminin miniyon mu buyu mu olduğunu belirle daha sonra özelliklerini getir.
                int DustinTargetIndex = -1;

                for (int i = 0; i < dustinCard.minions.Count; i++)
                {
                    if (dustinCard.minions[i].name == DustinTargetCardName)
                    {
                        DustinTargetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = dustinCard.minions[DustinTargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = dustinCard.minions[DustinTargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = dustinCard.minions[DustinTargetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = dustinCard.minions[DustinTargetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = dustinCard.minions[DustinTargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().rarity = (CardInformation.Rarity)dustinCard.minions[DustinTargetIndex].rarity;
                        CardCurrent.GetComponent<CardInformation>().AssignMaterialByRarity();
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < dustinCard.spells.Count; i++)
                {
                    if (dustinCard.spells[i].name == DustinTargetCardName)
                    {
                        DustinTargetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = dustinCard.spells[DustinTargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = dustinCard.spells[DustinTargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = dustinCard.spells[DustinTargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                CreateStandartCards(CardCurrent, DustinTargetCardName);
                break;
        }
    }
    void CreateStandartCards(GameObject CardCurrent, string name)
    {
        StandartCards standartCard = new StandartCards();

        int StandartCardTargetIndex = -1;

        for (int i = 0; i < standartCard.standartcards.Count; i++)
        {
            if (standartCard.standartcards[i].name == name)
            {
                StandartCardTargetIndex = i;

                CardCurrent.GetComponent<CardInformation>().CardName = standartCard.standartcards[StandartCardTargetIndex].name;
                CardCurrent.GetComponent<CardInformation>().CardDes = standartCard.standartcards[StandartCardTargetIndex].name + " POWWERRRRR!!!";
                CardCurrent.GetComponent<CardInformation>().CardHealth = standartCard.standartcards[StandartCardTargetIndex].health.ToString();
                CardCurrent.GetComponent<CardInformation>().CardDamage = standartCard.standartcards[StandartCardTargetIndex].attack;
                CardCurrent.GetComponent<CardInformation>().CardMana = standartCard.standartcards[StandartCardTargetIndex].mana;
                CardCurrent.GetComponent<CardInformation>().rarity = (CardInformation.Rarity)standartCard.standartcards[StandartCardTargetIndex].rarity;
                CardCurrent.GetComponent<CardInformation>().AssignMaterialByRarity();
                CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                CardCurrent.GetComponent<CardInformation>().SetInformation();
                break;
            }
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

            case "LeonardoCard":
                LeonardoCard leonardoCard = new LeonardoCard();
                return (int)leonardoCard.hpValue;

            case "OdinCard":
                OdinCard odinCard = new OdinCard();
                return (int)odinCard.hpValue;

            case "DustinCard":
                DustinCard dustinCard = new DustinCard();
                return (int)dustinCard.hpValue;

            case "AnubisCard":
                AnubisCard anubisCard = new AnubisCard();
                return (int)anubisCard.hpValue;

            

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
            case "LeonardoCard":
                LeonardoCard leonardoCard = new LeonardoCard();
                return (int)leonardoCard.attackValue;

            case "OdinCard":
                OdinCard odinCard = new OdinCard();
                return (int)odinCard.attackValue;

            case "DustinCard":
                DustinCard dustinCard = new DustinCard();
                return (int)dustinCard.attackValue;

            case "AnubisCard":
                AnubisCard anubisCard = new AnubisCard();
                return (int)anubisCard.attackValue;

        }

        return 60;

    }
   

}
