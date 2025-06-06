using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.IO;
using System;
using DG.Tweening;
using Ender.Scripts;
using TMPro;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PhotonView PV;
    public GameManager _GameManager;
    public GameObject CompetitorPV = null;
    CardsFunction _CardFunction;

    public GameObject CardPrefabSolo; // Tek kart
    public GameObject CardPrefabInGame;
    GameObject NextCard;
    
   
    string OwnName = "";
    string[] OwnDeck;
    [HideInInspector] public string OwnMainCard = "";
    float OwnHealth = 0;
    float OwnHeroAttackDamage = 0;


    string CompetitorName = "";
    [HideInInspector] public string[] CompetitorDeck;
    [HideInInspector] public string CompetitorMainCard = "";
    float CompetitorHealth = 0;
    float CompetitorHeroAttackDamage = 0;

    public Text OwnNameText;
    public Text OwnDeckText;
    public Text OwnMainCardText;
    public Text OwnHealthText;
    public Text ManaCountText;
    public Text WhoseTurnText;
    public Text Timer;

    public Text CompetitorNameText;
    public Text CompetitorDeckText;
    public Text CompetitorMainCardText;
    public Text CompetitorHealthText;
    public Text CompetitorManaCountText;

    public GameObject healthTextObject;
    public GameObject attackTextObject;
    public GameObject drawCardTextObject;
    public GameObject Freeze;
    public GameObject Mummies;
    public GameObject TakeGun;
    public GameObject Minion;
    public GameObject Spell;

    public GameObject DamageText;
    public GameObject LogsPrefab;
    public Button finishButton;
    public Button heroPowerButton;
    GameObject LogsContainerContent;

    [SerializeField] private Image countDownImage, whoseTurnImage;
    [SerializeField] private Sprite myTurnSprite, enemyTurnSprite;
    

    public Image OwnHealthBar;
    public Image OwnManaBar;

    public Image CompetitorHealthBar;
    public Image CompetitorManaBar;

    float elapsedTime = 60;

    GameObject selectedCard;
    public GameObject lastHoveredCard = null;
    public CardProgress _CardProgress;

    private Vector3 initialCardPosition;

    public Text OwnHeroAttackDamageText;
    public Text CompetitorHeroAttackDamageText;

    [HideInInspector] public int OlympiaKillCount = 0;
    [HideInInspector] public int SpellsExtraDamage = 0;
    [HideInInspector] public bool GodsBaneUsed = false;
    [HideInInspector] public bool SteppeAmbush = false;
    [HideInInspector] public bool NomadicTactics = false;
    [HideInInspector] public int NomadsLand = 0;
    [HideInInspector] public int AsgardQuestion = 0;
    [HideInInspector] public int DeadCardCount = 0;
    [HideInInspector] public int DeckCardCount = 0;

    [SerializeField] public Image CompetitorHeroIcon;
    [SerializeField] public List<Sprite> heroIcons=new List<Sprite>();
    [SerializeField] private TextMeshProUGUI damageTxt;
    [SerializeField] private CanvasGroup playerControlCanvasGroup;
    

    [HideInInspector]
    public float Mana
    {
        get { return _mana;}
        set
        {
            _mana = value;
            OnManaChange(_mana);
        }
    }

    private void OnManaChange(float mana)
    {
        if (mana>=ReturnHeroPowerManaValue() && WhoseTurnText.text == "Finish Turn" && CanAttackMainCard)
        {
            heroPowerButton.interactable = true;
        }
        else
        {
            heroPowerButton.interactable = false;
        }
    }

    private float ReturnHeroPowerManaValue()
    {
        switch (OwnMainCard)
        {
            case "Odin":
                if (AsgardQuestion >= 3)
                {
                    return 1;
                }
                return 2;
            case "Zeus":
                return 2;
            case "Anubis":
                return 2;
            case "Genghis":
                return 2;
            case "Dustin":
                return 2;
            case "DaVinci":
                return 2;
        }

        return 2;
    }

    private float _mana;
    [HideInInspector] public int DeadMonsterCound = 0;
    [HideInInspector] public int AugmentCount = 0;
    [HideInInspector] public bool CanAttackMainCard = true;
    [HideInInspector] public int DoubleDamage = 1;
    [HideInInspector] public bool NaiadProtector = false;     //true ise büyü kullandığında +1 sağlık kazanıyoruz
    [HideInInspector] public bool PlayedSpell = false;          //bu tur büyü oynandımı diye kontrol
    [HideInInspector] public bool PlagueCarrierBool = false;    //ikinci sefer çalışması için
    [HideInInspector] public int LessSpellCost = 0;             //büyülerin mana değerini 1 azaltıyor
    [HideInInspector] public bool MerfolkScoutBool = false;     //bir karta bakıyor daha sonra onu çekiyor
    [HideInInspector] public int osirisCount = 0;               //osiris parçaları toplanmasını sayıyor

    [HideInInspector] public List<string> DeadMyCardName = null;
    [HideInInspector] public List<GameObject> UsedSpellCard = null;

    public GameObject vfxMovePrefab;   
    public GameObject vfxLandingPrefab;
    public GameObject vfxAttackPrefab;

    public Material CompetitorCardMaterial;
    public Material CompetitorHeroCardMaterial;

    public int addedValue;
    public GameObject InventorySystem;

    GameObject[] Lavas;
    public Animator HeroAnimator;
    public HeroParticleController HeroParticleController;
    public List<HeroParticleController> HeroParticleControllers = new List<HeroParticleController>();
    private Tween timeTween;

    private TalkManager _talkManager;
    void Start()
    {
        //_CardFunction = GameObject.Find("GameManager").GetComponent<CardsFunction>();
        DeadMyCardName = new List<string>();
        UsedSpellCard = new List<GameObject>();
        PV = GetComponent<PhotonView>();
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _CardProgress = GetComponent<CardProgress>();
        LogsContainerContent = GameObject.Find("Container");
        if (!PV.IsMine)
        {
            GetComponent<PlayerController>().transform.GetChild(0).gameObject.SetActive(false);
        }
        Mana = 1;

        InventorySystem = GameObject.FindGameObjectWithTag("Inventory");
        Lavas = GameObject.FindGameObjectsWithTag("lava");
        
        /*if (PV.Owner.IsMasterClient)
        {
            OwnMainCard = _GameManager.MasterMainCard;
            CompetitorMainCard = _GameManager.OtherrMainCard;
        }
        else
        {
            OwnMainCard = _GameManager.OtherrMainCard;
            CompetitorMainCard = _GameManager.MasterMainCard;
        }
        print(OwnMainCard+"....");
        print(CompetitorMainCard+"....");*/
        GetDataForUI();
       

        addedValue = isRankedMap() ? GetHasBeenBoughtValue() : 0;
        AssignAnimatorController();
        
        if (AssignHeroParticleController())
        {
            _GameManager.PlayerController = this;
        }
        heroPowerButton.GetComponent<HoverInfo>().infoText = SetHoverHeroAttackInfo();
        _talkManager = GameObject.Find("TalkCloud").GetComponent<TalkManager>();
        StartCoroutine(StartFirstTurn());
    }

    private string SetHoverHeroAttackInfo()
    {
        switch (OwnMainCard)
        {
            case "Odin":
                return "";
            case "Zeus":
                return "Deal 1 damage to any target";
            case "Anubis":
                return "";
            case "Genghis":
                return "Deal 2 damage to enemy hero";
            case "Dustin":
                return "";
            case "DaVinci":
                return "";
        }
        return "Special Attack";
    }
    
    IEnumerator StartFirstTurn()
    {
        yield return new WaitForSeconds(3f);
       
        yield return new WaitForSeconds(.2f);
         playerControlCanvasGroup.alpha = 1f;
       // GetDataForUI();
       if (HeroParticleController!=null)
       {
           HeroParticleController.gameObject.SetActive(true);   
       }
        
        if (PV.IsMine)
        {
            _GameManager.Turn = true;
            WhoseTurnText.text = "Finish Turn";
            finishButton.interactable = true;
            heroPowerButton.interactable = false;
            whoseTurnImage.sprite = myTurnSprite;
            timeTween = countDownImage.DOFillAmount(0, 60).SetEase(Ease.Linear);
        }
        else
        {
            _GameManager.Turn = false;
            WhoseTurnText.text = "Opponent's Turn";
            finishButton.interactable = false;
            heroPowerButton.interactable = false;
            whoseTurnImage.sprite = enemyTurnSprite;
            timeTween = countDownImage.DOFillAmount(0, 60).SetEase(Ease.Linear);
        }
        
    }

    void HeroNftCard()
    {
        string competitorCardPath = $"NftCarddsİmage/{CompetitorMainCard}";

        Sprite competitorCardSprite = Resources.Load<Sprite>(competitorCardPath);

        if (CompetitorHeroCardMaterial != null && competitorCardSprite != null)
        {
            // Sprite'ın Texture'ını alıyoruz
            CompetitorHeroCardMaterial.SetTexture("_BaseMap", competitorCardSprite.texture);
        }
        else
        {
            Debug.LogWarning($"Material veya Texture atanmadı! Path: {competitorCardPath}");
        }
    }


    void LavaAnimations()
    {
        float totalDuration = 60f;  
        float startScale = 0.12f;
        float endScale = 8f;

        float scaleValue = Mathf.Lerp(startScale, endScale, 1 - (elapsedTime / totalDuration));

        foreach (var item in Lavas)
        {
            Vector3 newScale = item.transform.localScale;
            newScale.y = scaleValue;
            item.transform.localScale = newScale;
        }
    }

    void ResetLavaScale()
    {
        float resetScale = 0.12f;

        foreach (var item in Lavas)
        {
            Vector3 newScale = item.transform.localScale;
            newScale.y = resetScale;
            item.transform.localScale = newScale;
        }
    }


    private void Update()
    {
        if (!PV.IsMine)
            return;

        if(finishButton.interactable==true)
        {
            elapsedTime -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            LavaAnimations();
            Timer.text = string.Format("{0:00}:{1:00}", 00, seconds);

            if (elapsedTime <= 0f)
            {
                Debug.Log("Countdown finished!");
                FinishButton();
                elapsedTime = 60;
                Timer.text = "00:60";
            }
        }

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

            TalkCloud("It is not my Turn!");
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
            TalkCloud("It is not my Turn!");
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
            TalkCloud( "It is not my Turn!");
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
                        StartCoroutine(ChangeScale(lastHoveredCard.transform, new Vector3(0.7f, 1f, 0.01f), 0.2f));
                    }

                    lastHoveredCard = hit.collider.gameObject;
                    StartCoroutine(ChangeScale(lastHoveredCard.transform, new Vector3(0.9f, 1.2f, 0.06f), 0.2f));
                }
            }
            else 
            {
                if (lastHoveredCard != null)
                {
                    StopCoroutine("ChangeScale");
                    StartCoroutine(ChangeScale(lastHoveredCard.transform, new Vector3(0.7f, 1f, 0.01f), 0.2f));
                    lastHoveredCard = null; 
                }
            }
        }
        else 
        {
            if (lastHoveredCard != null)
            {
                StopCoroutine("ChangeScale");
                StartCoroutine(ChangeScale(lastHoveredCard.transform, new Vector3(0.7f, 1f, 0.01f), 0.2f));
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
                    GetComponent<AudioSource>().clip = SelectCardFX;
                    GetComponent<AudioSource>().Play();
                    selectedCard = hit.collider.gameObject;
                    selectedCard.GetComponent<Renderer>().material.color = Color.green;

                    initialCardPosition = selectedCard.transform.position;
                    initialCardRotation = selectedCard.transform.rotation.eulerAngles;
                    initialScale = selectedCard.transform.localScale;
                }
            }
            else if (hit.collider.gameObject.tag == "UsedCard" && _CardProgress.ForMyCard == false)
            {
                if (hit.collider.gameObject.GetComponent<CardInformation>().CardFreeze == false &&
                    hit.collider.gameObject.GetComponent<CardInformation>().isItFirstRaound == false &&
                    hit.collider.gameObject.GetComponent<CardInformation>().isAttacked == false)
                {
                    _CardProgress.CloseBlueSign();
                    _CardProgress.SetAttackerCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, hit.collider.gameObject.transform.parent.gameObject));
                    Transform childTransform = hit.collider.gameObject.transform;
                    Transform blue = childTransform.Find("Blue");
                    blue.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("CardFreeze or firstraund or isattacked");
                    TalkCloud("CardFreeze or firstraund or isattacked.");
                }
            }
            else if (hit.collider.gameObject.tag == "AreaBox" && _CardProgress.ForMyCard == false)
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
                            _CardProgress.CloseBlueSign();
                            _CardProgress.SetAttackerCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, hit.collider.gameObject));
                            Transform blue = firstChild.Find("Blue");
                            blue.gameObject.SetActive(true);
                        }
                        else
                        {
                            TalkCloud("CardFreeze or firstraund or isattacked.");
                        }
                    }
                }
            }
        }
    }


    private Vector3 initialCardRotation;
    private Vector3 initialScale;

    void DragCardAfterSelect()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0.5f;       //kartın ekrana uzaklığı
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float followSpeed = 5f; // Kartın takip hızı
        selectedCard.transform.position = Vector3.Lerp(selectedCard.transform.position, targetPosition, Time.deltaTime * followSpeed);
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("AreaBox"))
            {
                Transform transformBox = hit.collider.gameObject.transform;
                selectedCard.transform.position=transformBox.position;
                selectedCard.transform.localRotation = Quaternion.Euler(60, 0, 180);
                selectedCard.transform.localScale=new Vector3(6.3f,9,0.01f);
                
            }
            else
            {
                selectedCard.transform.localScale = new Vector3(0.7f, 1f, 0.01f);
            }
        }
        HoverInfoManager.Instance.CloseInfo();
    }
    public AudioClip DownCard,SelectCardFX;

    void ReleaseCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "AreaBox")
            {
                GetComponent<AudioSource>().clip = DownCard;
                GetComponent<AudioSource>().Play();
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
                            selectedCard.transform.rotation = Quaternion.Euler(initialCardRotation);
                            selectedCard.transform.localScale = initialScale;
                            print("hata");
                            selectedCard = null;
                            return;
                        }
                    }
                }

                selectedCard.GetComponent<Renderer>().material.color = Color.white;

                Transform transformBox = hit.collider.gameObject.transform;
                GameObject TemporaryObject = selectedCard;
                selectedCard = CreateAreaCard(Boxindex, selectedCard.GetComponent<CardInformation>().CardName, selectedCard.GetComponent<CardInformation>().CardDes, selectedCard.GetComponent<CardInformation>().CardHealth, selectedCard.GetComponent<CardInformation>().CardDamage, selectedCard.GetComponent<CardInformation>().CardMana,selectedCard.GetComponent<CardInformation>().rarity);
                Destroy(TemporaryObject);
                StartCoroutine(MoveAndRotateCard(selectedCard, transformBox.position, 0.3f));
                selectedCard.transform.SetParent(transformBox);
                selectedCard.transform.localPosition = Vector3.zero;

                Mana -= selectedCard.GetComponent<CardInformation>().CardMana;

                if(selectedCard.GetComponent<CardInformation>().CardHealth == "")     //spell demek
                {
                    Mana += LessSpellCost;
                    if(Mana > 10 )
                        Mana= 10;
                }

                //////////////////////////////////// DESTEDEN BİR KART MASYA EKLENDİĞİ ZAMAN ///////////////////////////////
                selectedCard.GetComponent<CardInformation>().AssignMaterialByRarity();
                
                if (selectedCard.GetComponent<CardInformation>().CardName == "Heracles") // MASAYA EKLENEN KART NEDİR
                {
                    ZeusCardFuns.HeraclesFun(selectedCard, this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Stormcaller")
                {
                    ZeusCardFuns.StormcallerFun(this);

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Odyssean Navigator")
                {
                    ZeusCardFuns.OdysseanNavigatorFun( this);

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Oracle's Emissary")
                {
                    ZeusCardFuns.OraclesEmissaryFun(this);


                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Lightning Forger")
                {
                    ZeusCardFuns.LightningForgerFun(this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Lightning Bolt")
                {

                    ZeusCardFuns.LightningBoltFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");

                    return;   

                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Gorgon")
                {

                    //_CardProgress.FreezeAllEnemyMinions();
                    ZeusCardFuns.GorgonFun(this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Chimera")
                {
                    //_CardProgress.DamageToAlLOtherMinions();
                    ZeusCardFuns.ChimeraFun(this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Athena")
                {
                    //_CardProgress.FillWithHoplites();
                    ZeusCardFuns.AthenaFun(this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Lightning Storm")
                {
                    ZeusCardFuns.LightningStormFun(selectedCard, this);
                    TalkCloud( "USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Olympian Favor") 
                {

                    ZeusCardFuns.OlympianFavorFun(selectedCard, this);
                    TalkCloud( "USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Aegis Shield") 
                {

                    ZeusCardFuns.AegisShieldFun(selectedCard, this);
                    TalkCloud( "USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Golden Fleece") 
                {

                    ZeusCardFuns.GoldenFleeceFun(selectedCard,this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Labyrinth Maze") 
                {

                    ZeusCardFuns.LabyrinthMazeFun(selectedCard,this);
                    TalkCloud( "USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Divine Ascention") 
                {

                    ZeusCardFuns.DivineAscentionFun(selectedCard,this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Centaur Archer" 
                    || selectedCard.GetComponent<CardInformation>().CardName == "Minotaur Warrior" 
                    || selectedCard.GetComponent<CardInformation>().CardName == "Pegasus Rider" 
                    || selectedCard.GetComponent<CardInformation>().CardName == "Greek Hoplite" 
                    || selectedCard.GetComponent<CardInformation>().CardName =="Siren" )
                {
                    ZeusCardFuns.CanFirstRauntAttack(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Mongol Lancer"
                    || selectedCard.GetComponent<CardInformation>().CardName == "Keshik Cavalry")
                {
                    GenghisCardFun.CanFirstRauntAttack(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName== "Mongol Messenger")       //genghis here
                {
                    GenghisCardFun.MongolMessengerFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName== "Mongol Archer")
                {
                    GenghisCardFun.MongolArcherFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName== "Mongol Shaman")
                {
                    GenghisCardFun.MongolShamanFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName=="Eagle Hunter")
                {
                    GenghisCardFun.EagleHunterFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName== "Yurt Builder")
                {
                    GenghisCardFun.YurtBuilderFun(this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Marco Polo")
                {

                    GenghisCardFun.MarcoPoloFun(this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Nomadic Scout")
                {

                    GenghisCardFun.NomadicScoutFun(this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Steppe Warlord")
                {
                    GenghisCardFun.SteppeWarlordFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "General Subutai")
                {

                    GenghisCardFun.GeneralSubutaiFun(selectedCard,this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Horseback Archery")
                {
                    GenghisCardFun.HorsebackArcheryFun(selectedCard, this);
                    TalkCloud( "USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Ger Defense")
                {

                    GenghisCardFun.GerDefenseFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Mongol Fury")
                {

                    GenghisCardFun.MongolFuryFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Around the Great Wall")
                {

                    GenghisCardFun.AroundtheGreatWallFun(selectedCard, this);
                    TalkCloud( "USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Eternal Steppe’s Whisper")
                {

                    GenghisCardFun.EternalSteppesWhisperFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "God’s Bane")
                {

                    GenghisCardFun.GodsBaneFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Steppe Ambush")
                {

                    GenghisCardFun.SteppeAmbushFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Nomadic Tactics")
                {

                    GenghisCardFun.NomadicTacticsFun(selectedCard, this);
                    TalkCloud( "USSEEDD A SPEEELLL!");
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Viking Raider")
                {
                    OdinCardFuns.CanFirstRauntAttack(selectedCard);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Runestone Mystic")
                {
                    OdinCardFuns.RunestoneMysticFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Shieldmaiden Defender")
                {
                    OdinCardFuns.ShieldmaidenDefenderFun(selectedCard);
                }       
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Fenrir's Spawn")
                {
                    OdinCardFuns.FenrirsSpawnFun(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Skald Bard")
                {
                    OdinCardFuns.SkaldBardDFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Mimir's Seer")
                {
                    OdinCardFuns.MimirsSeerFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Valkyrie's Chosen")
                {
                    OdinCardFuns.ValkyriesChosenFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Frost Giant")
                {
                    OdinCardFuns.FrostGiantFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Heimdallr")
                {
                    OdinCardFuns.HeimdallrFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Einherjar Caller")
                {
                    OdinCardFuns.EinherjarCallerFun(this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Norn Weaver" )
                {
                    if (GameObject.Find("Deck").transform.childCount < 10)
                    {
                        Vector3[] positions = new Vector3[3];
                        float yPosition = 1.4f;
                        float zPosition = 0f;

                        Vector3 screenPos1 = new Vector3(1.8f, yPosition, zPosition);
                        Vector3 screenPos2 = new Vector3(2.75f, yPosition, zPosition);
                        Vector3 screenPos3 = new Vector3(3.7f, yPosition, zPosition);

                        positions[0] = screenPos1;
                        positions[1] = screenPos2;
                        positions[2] = screenPos3;

                        for (int i = 0; i < positions.Length; i++)
                        {
                            Quaternion rotation = Quaternion.Euler(60f, 0f, 180f);
                            GameObject Card = Instantiate(Resources.Load<GameObject>("CompetitorCard"), positions[i], rotation);
                            Card.tag = "SelectCard";
                            Card.AddComponent<Button>();
                            CreateCard(Card);

                            Card.GetComponent<Button>().onClick.AddListener(() => {
                                foreach (GameObject card in GameObject.FindGameObjectsWithTag("SelectCard"))
                                {
                                    Destroy(card);
                                }
                                if (UnityEngine.Random.Range(0, 2) == 0)
                                {
                                    GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                                    OdinCardFuns.NornWeaverFun(CardCurrent, this);
                                }
                            });
                        }
                    }
                    else
                        print("DesrenDolu");
                    
                    
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Winter's Chill")
                {
                    OdinCardFuns.WintersChillFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Viking Raid")
                {
                    OdinCardFuns.VikingRaidFun(selectedCard, this);
                    TalkCloud( "USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Sleipnir’s Gallop")
                {
                    OdinCardFuns.SleipnirsGallopFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Gjallarhorn Call")
                {
                    OdinCardFuns.GjallarhornCallFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Rune Magic")
                {
                    OdinCardFuns.RuneMagicFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "The Allfather’s Decree")
                {
                    OdinCardFuns.TheAllfathersDecreeFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Mimir's Wisdom")
                {
                    int maxNewCards = 10 - GameObject.Find("Deck").transform.childCount+1;
                    int cardsToCreate = Mathf.Min(3, maxNewCards);
                    for (int i=0; i< cardsToCreate; i++)
                    {
                        GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                        OdinCardFuns.MimirsWisdomFun(selectedCard,CardCurrent, this);
                    }
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Necropolis Acolyte")                                         //anubis card
                {
                    AnubisCardFuns.NecropolisAcolyteFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Desert Bowman")
                {
                    AnubisCardFuns.DesertBowmanFun(selectedCard, this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Sphinx Riddler")
                {
                    if (GameObject.Find("Deck").transform.childCount < 11)
                    {
                        Vector3[] positions = new Vector3[3];
                        float yPosition = 1.4f;
                        float zPosition = 0f;

                        Vector3 screenPos1 = new Vector3(1.8f, yPosition, zPosition);
                        Vector3 screenPos2 = new Vector3(2.75f, yPosition, zPosition);
                        Vector3 screenPos3 = new Vector3(3.7f, yPosition, zPosition);

                        positions[0] = screenPos1;
                        positions[1] = screenPos2;
                        positions[2] = screenPos3;

                        int correctCardIndex = UnityEngine.Random.Range(0, positions.Length);

                        for (int i = 0; i < positions.Length; i++)
                        {
                            Quaternion rotation = Quaternion.Euler(60f, 0f, 180f);
                            GameObject Card = Instantiate(Resources.Load<GameObject>("CompetitorCard"), positions[i], rotation);
                            Card.tag = "SelectCard";
                            Card.AddComponent<Button>();
                            CreateCard(Card);

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
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Osiris’ Bannerman")
                {
                    AnubisCardFuns.OsirisBannermanFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Sun Charioteer")
                {
                    AnubisCardFuns.CanFirstRauntAttack(selectedCard);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Tomb Protector")
                {
                    AnubisCardFuns.TombProtectorFun(selectedCard,this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Falcon-Eyed Hunter")
                {
                    AnubisCardFuns.FalconEyedHunterFun(selectedCard, this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Canopic Preserver")
                {
                    AnubisCardFuns.CanopicPreserverFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Sandstone Scribe")
                {
                    if (GameObject.Find("Deck").transform.childCount < 10)
                    {
                        GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                        AnubisCardFuns.SandstoneScribeFun(CardCurrent, this);
                        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateRandomCard", RpcTarget.All);
                    }

                    
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Scroll of Death")
                {
                    AnubisCardFuns.ScrollofDeathFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Book of the Dead")
                {
                    AnubisCardFuns.BookoftheDeadFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Sun Disk Radiance")
                {
                    AnubisCardFuns.SunDiskRadianceFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Plague of Locusts")
                {
                    AnubisCardFuns.PlagueofLocustsFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "River's Blessing")
                {
                    AnubisCardFuns.RiversBlessingFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Pyramid's Might")
                {
                    AnubisCardFuns.PyramidsMightFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Scales of Anubis")
                {
                    AnubisCardFuns.ScalesofAnubisFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Gates of Duat")
                {
                    AnubisCardFuns.GatesofDuatFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Codex Guardian")                                                 //leonardocard
                {
                   LeonardoCardFuns.CodexGuardianFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Piscean Diver")
                {
                    LeonardoCardFuns.PisceanDiverFun(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Da Vinci's Helix Engineer")
                {
                    LeonardoCardFuns.DaVincisHelixEngineerFun(this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Vitruvian Firstborn")
                {
                    LeonardoCardFuns.VitruvianFirstbornFun();
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Da Vinci's Glider")
                {
                    LeonardoCardFuns.DaVincisGliderFun();
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Mechanical Lion")
                {
                    LeonardoCardFuns.MechanicalLionFun(selectedCard, this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Da Vinci’s Blueprint")
                {
                    LeonardoCardFuns.DaVincisBlueprintFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Tabula Aeterna")
                {
                    LeonardoCardFuns.TabulaAeternaFun(selectedCard, this);
                   TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Artistic Inspiration")
                {
                    LeonardoCardFuns.ArtisticInspirationFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Anatomical Insight")
                {
                    LeonardoCardFuns.AnatomicalInsightFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Wasteland Sniper")
                {
                    DustinCardFuns.WastelandSniperFun(selectedCard,this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Engineer of the Ruins")
                {
                    DustinCardFuns.EngineeroftheRuinsFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Rogue AI Drone")
                {
                    DustinCardFuns.RogueAIDroneFun(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Mutant Behemoth")
                {
                    DustinCardFuns.MutantBehemothFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Lone Cyborg")
                {
                    DustinCardFuns.LoneCyborgFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Scavenger Raider")
                {
                    DustinCardFuns.ScavengerRaiderFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Claire")
                {
                    DustinCardFuns.ClaireFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Scrap Shield")
                {
                    DustinCardFuns.ScrapShieldFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Shockwave/Impulse")
                {
                    DustinCardFuns.ShockwaveImpulseFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Garage Raid")
                {
                    DustinCardFuns.GarageRaidFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Radioactive Fallout")
                {
                    DustinCardFuns.RadioactiveFalloutFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    Destroy(selectedCard);
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Mutated Blood Sample")
                {
                    DustinCardFuns.MutatedBloodSampleFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Mechanical Reinforcement")
                {
                    LeonardoCardFuns.MechanicalReinforcementFun(selectedCard, this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Tome of Confusion")
                {
                    LeonardoCardFuns.TomeofConfusionFun(selectedCard,this);
                    TalkCloud("USSEEDD A SPEEELLL!");
                    return;
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Echo of Tomorrow")
                {
                    EchoOfTomorrow();
                }      
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Templar Knight")
                {
                    StandartCardFuns.TemplarKnightFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Cerberus Spawn")
                {
                    StandartCardFuns.CerberusSpawnFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Arcane Scholar")
                {
                    StandartCardFuns.ArcaneScholarFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Rebel Outcast")
                {
                    StandartCardFuns.RebelOutcastFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Naiad Protector")
                {
                    StandartCardFuns.NaiadProtectorFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Ruined City Scout")
                {
                    StandartCardFuns.RuinedCityScoutFun(this);  
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Gladiator Champion")
                {
                    StandartCardFuns.GladiatorChampionFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Urban Ranger")
                {
                    StandartCardFuns.UrbanRangerFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Byzantine Fire Slinger")
                {
                    StandartCardFuns.ByzantineFireSlingerFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Shadow Assassin")
                {
                    StandartCardFuns.ShadowAssassinFun(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Radiated Marauder")
                {
                    StandartCardFuns.CanFirstRauntAttack(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Rogue Mech-Pilot")
                {
                    StandartCardFuns.RogueMechPilotFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Rubble Raider")
                {
                    StandartCardFuns.CanFirstRauntAttack(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Desert Conjurer")
                {
                    StandartCardFuns.DesertConjurerFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Sandstorm")
                {
                    StandartCardFuns.SandstormFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Gaelic Warrior")
                {
                    StandartCardFuns.CanFirstRauntAttack(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Oasis Guardian")
                {
                    StandartCardFuns.OasisGuardianFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Battle Mage")
                {
                   StandartCardFuns.BattleMageFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Roving Merchant")
                {
                    StandartCardFuns.RovingMerchantFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Nomadic Hunter")
                {
                    StandartCardFuns.NomadicHunterFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Raiding Party")
                {
                    StandartCardFuns.RaidingPartyFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Tavern Brawler")
                {
                    StandartCardFuns.CanFirstRauntAttack(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Pyromaniac Wizard")
                {
                    StandartCardFuns.PyromaniacWizardFun(this);
                }
                else if (selectedCard.GetComponent<CardInformation>().CardName == "Frontline Militia")
                {
                    StandartCardFuns.FrontlineMilitiaFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Wandering Healer")
                {
                    StandartCardFuns.WanderingHealerFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Horse Archer")
                {
                    StandartCardFuns.CanFirstRauntAttack(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Viking Shield-Maiden")
                {
                     StandartCardFuns.VikingShieldMaidenFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Plague Carrier")
                {
                      StandartCardFuns.PlagueCarrierFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Dune Scout")
                {
                    StandartCardFuns.DuneScoutFun(selectedCard);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Scavenger's Daughter")
                {
                    StandartCardFuns.ScavengersDaughterFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Mystic Archer")
                {
                      StandartCardFuns.MysticArcherFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Toxic Rainmaker")
                {
                    StandartCardFuns.ToxicRainmakerFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Desert Warlock")
                {
                      StandartCardFuns.DesertWarlockFun(this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Merfolk Scout")
                {
                    MerfolkScout();
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Street Thug")
                {
                    StandartCardFuns.StreetThugFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Ancient Librarian")
                {
                    StandartCardFuns.AncientLibrarianFun(selectedCard, this);
                }
                else if(selectedCard.GetComponent<CardInformation>().CardName == "Apprentice Sorcerer")
                {
                    StandartCardFuns.ApprenticeSorcererFun(this);
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

                    selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + addedValue).ToString();
                    selectedCard.GetComponent<CardInformation>().CardDamage += addedValue;
                    selectedCard.GetComponent<CardInformation>().SetInformation();


                    Debug.LogError(Boxindex);
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);

                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("CreateUsedCard", RpcTarget.All, Boxindex,
                      selectedCard.GetComponent<CardInformation>().CardName,
                      selectedCard.GetComponent<CardInformation>().CardDes,
                      selectedCard.GetComponent<CardInformation>().CardHealth,
                      selectedCard.GetComponent<CardInformation>().CardDamage,
                      selectedCard.GetComponent<CardInformation>().CardMana,
                      selectedCard.GetComponent<CardInformation>().rarity);


                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_RefreshCompetitorMana", RpcTarget.Others,Mana);
                    PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                }
                DeactivateAllGreens();
                ActiveAllGreens();
                selectedCard = null;
                lastHoveredCard = null;
                return; 
            }
        }

        if (selectedCard != null)
        {
            selectedCard.GetComponent<Renderer>().material.color = Color.white;
            selectedCard.transform.position = initialCardPosition;
            selectedCard.transform.rotation = Quaternion.Euler(initialCardRotation);
            selectedCard.transform.localScale=new Vector3 (0.7f, 1f, 0.04f);
            selectedCard = null;
        }
        
        HoverInfoManager.Instance.CloseInfo();
    }

    public void TalkCloud(string text)
    {
       _talkManager.Talk(text);
    }

    bool isRankedMap()
    {
        return SceneManager.GetActiveScene().name == "RankedBattleMap"; 
    }

    public int GetHasBeenBoughtValue()
    {
        if (PV.IsMine)
        {
            Dictionary<string, int> cardIDMapping = new Dictionary<string, int>
            {
                { "Genghis", 0 },   
                { "Zeus", 1 },      
                { "Anubis", 10 },   
                { "Dustin", 9 },    
                { "Leonardo Da Vinci", 11 }, 
                { "Odin", 8 }       
            };

            if (cardIDMapping.ContainsKey(OwnMainCard))
            {
                int cardID = cardIDMapping[OwnMainCard];
                for(int i = 0; i < InventorySystem.GetComponent<InventorySystem>().playerCards.Count; i++) 
                {
                    if (InventorySystem.GetComponent<InventorySystem>().playerCards[i].id == cardID)
                    {
                        int hasBeenBought = InventorySystem.GetComponent<InventorySystem>().playerCards[i].hasBeenBought;

                        if (hasBeenBought < 100)
                        {
                            return 1; 
                        }
                        else if (hasBeenBought >= 100 && hasBeenBought < 250)
                        {
                            return 2; 
                        }
                        else if (hasBeenBought >= 250 && hasBeenBought < 450)
                        {
                            return 3; 
                        }
                        else if (hasBeenBought >= 450 && hasBeenBought < 700)
                        {
                            return 4; 
                        }
                        else if (hasBeenBought >= 700)
                        {
                            return 5; 
                        }
                    }
                }
            }

            return 0; 
        }
        return 0;    
    }


    public int CheckUndeadCards()
    {
        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("UsedCard");
        int count = 0;
        foreach (var card in AllOwnCards)
        {
            if(card.GetComponent<CardInformation>().CardName == "Royal Mummy" || card.GetComponent<CardInformation>().CardName == "Mummy")
            {
                count++;
            }
        }
        return count;
    }
    public void SelectedCard(GameObject Card)
    {
        GameObject CardCurrent= Instantiate(Card, GameObject.Find("Deck").transform);

        CardCurrent.name = Card.name;
        CardCurrent.tag = "Card";
        CardCurrent.transform.localEulerAngles = new Vector3(45, 0, 180);
        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
        StackDeck();
        StackCompetitorDeck();
        DeckCardCount++;

        GameObject[] SelectCard = GameObject.FindGameObjectsWithTag("SelectCard");
        foreach (GameObject cardObj in SelectCard)
        {
            Destroy(cardObj);
        }

        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateRandomCard", RpcTarget.All);
    }

    [PunRPC]
    void GodsBane(int mana)
    {
        for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++) // KENDİ DESTEMİZDEKİ KARTLARI TEK TEK ÇAĞIR
        {

            if (GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().CardHealth == "")  // ÇAĞIRILAN KARTIN BÜYÜ MÜ OLDUĞU KONTROL ET
            {
                GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().CardMana += mana;
                GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().SetInformation();
            }
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

    IEnumerator MoveAndRotateCard(GameObject card, Vector3 targetPosition, float duration)
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

        if (Application.platform == RuntimePlatform.WindowsPlayer ||
           Application.platform == RuntimePlatform.OSXPlayer ||
           Application.platform == RuntimePlatform.LinuxPlayer ||
           Application.platform == RuntimePlatform.WindowsEditor ||
           Application.platform == RuntimePlatform.OSXEditor ||
           Application.platform == RuntimePlatform.LinuxEditor)
        {
            // Movie VFX varsa onu instantiate et
            GameObject vfxMoveInstance = Instantiate(moveVFXToUse, card.transform.position, Quaternion.identity);
            vfxMoveInstance.transform.SetParent(card.transform);
            vfxMoveInstance.transform.localPosition = Vector3.zero;
            VisualEffect vfxMoveComponent = vfxMoveInstance.GetComponent<VisualEffect>();
            vfxMoveInstance.transform.localScale=new Vector3(1.2f,1,1);
            vfxMoveInstance.transform.localEulerAngles = new Vector3(0, 0, 0);

            if (vfxMoveComponent != null)
            {
                vfxMoveComponent.Play();
            }

            // İlk hareketten sonra biraz bekle
            yield return new WaitForSeconds(1.5f);

            // Movie VFX'i yok et
            Destroy(vfxMoveInstance);
        }
      

        // Kartı hedef pozisyona indir
        time = 0f;
        while (time < duration)
        {
            card.transform.position = Vector3.Lerp(targetPlus, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        card.transform.position = targetPosition;


        if (Application.platform == RuntimePlatform.WindowsPlayer ||
           Application.platform == RuntimePlatform.OSXPlayer ||
           Application.platform == RuntimePlatform.LinuxPlayer ||
           Application.platform == RuntimePlatform.WindowsEditor ||
           Application.platform == RuntimePlatform.OSXEditor ||
           Application.platform == RuntimePlatform.LinuxEditor)
        {
            // Last VFX prefab'ını yükle, eğer bulunamazsa default vfxLandingPrefab'ı kullan
            GameObject loadedLandingPrefab = Resources.Load<GameObject>(lastVFXPath);
            GameObject landingVFXToUse = loadedLandingPrefab != null ? loadedLandingPrefab : vfxLandingPrefab;

            // Last VFX varsa onu instantiate et
            GameObject vfxLandingInstance = Instantiate(landingVFXToUse, card.transform.position, Quaternion.identity);
            vfxLandingInstance.transform.SetParent(card.transform);
            vfxLandingInstance.transform.localPosition = Vector3.zero;
            VisualEffect vfxLandingComponent = vfxLandingInstance.GetComponent<VisualEffect>();
            vfxLandingInstance.transform.localScale=new Vector3(1.2f,1,1);;
            vfxLandingInstance.transform.localEulerAngles = new Vector3(0, 0, 0);
            if (vfxLandingComponent != null)
            {
                vfxLandingComponent.Play();
            }

            // Last VFX'i 5 saniye sonra yok et
            yield return new WaitForSeconds(5f);
            Destroy(vfxLandingInstance);
        }

       

        selectedCard = null;
        StackDeck();
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

    public GameObject CreateAreaCard(int boxindex, string name, string des, string heatlh, int damage, int mana, CardInformation.Rarity rarity)
    {
        GameObject CardCurrent = Instantiate(CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform);
        CardCurrent.tag = "UsedCard";

        CardCurrent.transform.localPosition = Vector3.zero;

        CardCurrent.GetComponent<CardInformation>().CardName = name;
        CardCurrent.GetComponent<CardInformation>().CardDes = des;
        CardCurrent.GetComponent<CardInformation>().CardHealth = heatlh;
        CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
        CardCurrent.GetComponent<CardInformation>().CardMana = mana;
        CardCurrent.GetComponent<CardInformation>().rarity = rarity;
        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
        CardCurrent.GetComponent<CardInformation>().SetInformation();

        return CardCurrent;
    }

    [PunRPC]
    public void CreateUsedCard(int boxindex, string name, string des, string heatlh, int damage, int mana, CardInformation.Rarity rarity)
    {

        if (PV.IsMine)
        {
            GameObject CardCurrent = Instantiate(CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[boxindex].transform);
            CardCurrent.tag = "CompetitorCard";

          //  CardCurrent.GetComponent<PhotonView>().ViewID = OwnDeck.Length;
            CardCurrent.transform.localPosition = Vector3.zero;
            if(name == "Crypt Warden" || name == "Chaos Scarab" ||name== "Gyrocopter" || name == "Piscean Diver" || name == "Rebel Outcast" || name == "Urban Ranger" || name == "Shadow Assassin" || name == "Elven Tracker")
            {
                CardCurrent.SetActive(false);
            }
            CardCurrent.GetComponent<CardInformation>().CardName = name;
            CardCurrent.GetComponent<CardInformation>().CardDes = des;
            CardCurrent.GetComponent<CardInformation>().CardHealth = heatlh;
            CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
            CardCurrent.GetComponent<CardInformation>().CardMana = mana;
            CardCurrent.GetComponent<CardInformation>().rarity = rarity;
            CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
            CardCurrent.GetComponent<CardInformation>().SetInformation();
            CardCurrent.GetComponent<CardInformation>().AssignMaterialByRarity();

            StartCoroutine(MoveAndRotateCard(CardCurrent, CardCurrent.transform.position, 0.3f));
        }
    } 
    public void SetActiveCard(int index)
    {
        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_SetActiveCard", RpcTarget.All, index);
    }

    [PunRPC]
    void RPC_SetActiveCard(int index)
    {
        if (PV.IsMine)
        {
            GameObject CardCurrent = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[index].transform.GetChild(0).transform.gameObject;;

            if (CardCurrent.GetComponent<CardInformation>().CardName == "Crypt Warden" ||
                CardCurrent.GetComponent<CardInformation>().CardName == "Chaos Scarab" ||
                CardCurrent.GetComponent<CardInformation>().CardName == "Piscean Diver"||
                CardCurrent.GetComponent<CardInformation>().CardName == "Rebel Outcast")
            {
                if (!CardCurrent.activeSelf)
                {
                    CardCurrent.SetActive(true);
                }
            }

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

    float firstCompetitorHealth=0;
    float firstOwnHealt=0;


    public void RefreshUI(float OwnHealth, float CompetitorHealth, float OwnAttackDamage, float CompetitiorAttackDamage)
    {

        if(firstCompetitorHealth==0 || firstOwnHealt==0)
        {   
            firstOwnHealt = OwnHealth;
            firstCompetitorHealth=CompetitorHealth;

        }

        OwnHealthText.text = OwnHealth.ToString() + "/" + firstOwnHealt.ToString();
        OwnHealthBar.fillAmount = OwnHealth / firstOwnHealt;

        OwnHeroAttackDamageText.text = OwnAttackDamage.ToString();



        CompetitorHealthText.text = CompetitorHealth.ToString() + "/" + firstCompetitorHealth.ToString();
        CompetitorHealthBar.fillAmount = CompetitorHealth / firstCompetitorHealth;

        CompetitorHeroAttackDamageText.text = CompetitiorAttackDamage.ToString();

        

        OwnManaBar.fillAmount = Mana / 10;

    }

    IEnumerator HeroText(int damage)
    {
        damageTxt.text = "-" + damage.ToString();
        yield return new WaitForSeconds(3f);
        damageTxt.text = "";
    }


    public void CreateTextHero(int damage)
    {
        /*GameObject competitorHeroPivot = GameObject.Find("CompetitorHeroPivot");

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
        }*/
        StartCoroutine(HeroText(damage));
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
        damageTextObject.transform.localScale = new Vector3(0.005f, 0.005f, 0.01f);

        Text textComponent = damageTextObject.GetComponentInChildren<Text>();
        textComponent.text = "-" + damage.ToString();
        Destroy(damageTextObject, 3f);

        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateTextAtTargetIndex", RpcTarget.Others, targetIndex, damage, mycard,cardname);
    }



    [PunRPC]
    void RPC_CreateTextAtTargetIndex(int targetIndex,int damage,bool mycard, string cardname)
    {
        Transform targetTransform;
        if (mycard)
        {
            targetTransform = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[targetIndex].transform;
        }
        else
        {
            targetTransform = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[targetIndex].transform;
        }
        GameObject targetChildObject = targetTransform.GetChild(0).gameObject;
        string cardName = targetChildObject.GetComponent<CardInformation>().CardName.Replace(" ", "");

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
        damageTextObject.transform.localPosition = new Vector3(0, 3, 0);
        damageTextObject.transform.localRotation = Quaternion.Euler(5, 0, 0);
        damageTextObject.transform.localScale = new Vector3(0.01f, 0.05f, 0.01f);
        Text textComponent = damageTextObject.GetComponentInChildren<Text>();
        textComponent.text = "-" + damage.ToString();
        Destroy(damageTextObject, 3f);
    }

    public void RefreshLog(int damage, bool isdead, string attackercardname, string targetcardname, Color color)
    {
        GameObject logsObject = Instantiate(LogsPrefab, LogsContainerContent.transform);
        string colorString = ColorToString(color);

        // Damage text objesini bul ve değerini güncelle
        Text logsDamageText = logsObject.transform.Find("DamageImage/Damage").GetComponent<Text>();
        if(damage!=0)
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

        // Rakip oyuncular için RPC çağrısı yap
        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_RefreshLog", RpcTarget.Others, damage, isdead, attackercardname, targetcardname, colorString);
    }


    [PunRPC]
    void RPC_RefreshLog(int damage,bool isdead,string attackercardname, string targetcardname,string colorString)
    {
        GameObject logsObject = Instantiate(LogsPrefab, LogsContainerContent.transform);
        Color color= ColorFromString(colorString);
        Text logsDamageText = logsObject.transform.Find("DamageImage/Damage").GetComponent<Text>();
        logsDamageText.text = damage.ToString();
        Transform deadObject = logsObject.transform.Find("TargetImage");
        if (damage != 0)
        {
            logsDamageText.text = damage.ToString();
        }
        else
        {
            logsDamageText.text = null;
        }
        if (deadObject != null && isdead)
        {
            Image deadImage = deadObject.GetComponent<Image>();
            deadImage.color=color;
        }
        else
        {
            Debug.LogWarning("Dead object not found in LogsPrefab.");
        }
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
    }
    public string ColorToString(Color color)
    {
        return color.r + "," + color.g + "," + color.b + "," + color.a;
    }

    public Color ColorFromString(string colorString)
    {
        string[] colorValues = colorString.Split(',');
        if (colorValues.Length == 4)
        {
            float r = float.Parse(colorValues[0]);
            float g = float.Parse(colorValues[1]);
            float b = float.Parse(colorValues[2]);
            float a = float.Parse(colorValues[3]);
            return new Color(r, g, b, a);
        }
        return Color.white; // Hata durumunda varsayılan renk
    }

    [PunRPC]
    void RPC_RefreshCompetitorMana(float mana)
    {
        CompetitorManaCountText.text = mana + "/10".ToString();
        CompetitorManaBar.fillAmount = mana / 10;
    }

    public void FinishButton()
    {
        ResetLavaScale();
        // Find all GameObjects with the specified name
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("UsedCard");
        CompetitorPV = null;
        WhoseTurnText.text = "Enemy Turn";
        Timer.text = "00:60";
        timeTween.Kill(true);
        countDownImage.fillAmount = 1;
        whoseTurnImage.sprite = enemyTurnSprite;
        timeTween=countDownImage.DOFillAmount(0, 60f);
        elapsedTime = 60;
        finishButton.interactable = false;
        heroPowerButton.interactable = false;
        CanAttackMainCard = true;
        DeactivateAllGreens();
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
        
        foreach (var card in AllOwnCards)
        {
            card.GetComponent<CardInformation>().isItFirstRaound = false;
            card.GetComponent<CardInformation>().CardFreeze = false;
            card.GetComponent<CardInformation>().isAttacked = false;
            card.GetComponent<CardInformation>().CanAttackBehind = false;
            if (card.GetComponent<CardInformation>().CardName == "Kublai Khan")
            {
                _CardProgress.KublaiKhan();
            }
            if(card.GetComponent<CardInformation>().MongolFury==true)
            {
                card.GetComponent<CardInformation>().CardDamage -= 2;
                card.GetComponent<CardInformation>().MongolFury = false;
                card.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                RefreshMyCard(index, 
                    card.GetComponent<CardInformation>().CardHealth, 
                    card.GetComponent<CardInformation>().HaveShield,
                    card.GetComponent<CardInformation>().CardDamage,
                    card.GetComponent<CardInformation>().DivineSelected,
                    card.GetComponent<CardInformation>().FirstTakeDamage, 
                    card.GetComponent<CardInformation>().FirstDamageTaken,
                    card.GetComponent<CardInformation>().EternalShield);
            }   
            if(card.GetComponent<CardInformation>().CardName == "Dwarven Blacksmith")
            {
                GameObject randomCard = AllOwnCards[UnityEngine.Random.Range(0, AllOwnCards.Length)];
                randomCard.GetComponent<CardInformation>().CardDamage += 2;
                randomCard.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, randomCard.transform.parent.gameObject);
                RefreshMyCard(index,
                    randomCard.GetComponent<CardInformation>().CardHealth,
                    randomCard.GetComponent<CardInformation>().HaveShield,
                    randomCard.GetComponent<CardInformation>().CardDamage,
                    randomCard.GetComponent<CardInformation>().DivineSelected,
                    randomCard.GetComponent<CardInformation>().FirstTakeDamage,
                    randomCard.GetComponent<CardInformation>().FirstDamageTaken,
                    randomCard.GetComponent<CardInformation>().EternalShield);
            }
            if(card.GetComponent<CardInformation>().CardName == "Thor")
            {
                _CardProgress.DamageToAlLOtherMinions(1,"Thor");
            }
            if (card.GetComponent<CardInformation>().Gallop == true)
            {
                card.GetComponent<CardInformation>().Gallop = false;
                DestroyAndCreateMyDeck(card);
            }
            if(card.GetComponent<CardInformation>().CardName == "Codex Guardian")
            {
                GetComponent<CardController>().AddHealCard(2, !PV.Owner.IsMasterClient);
            }
            if(card.GetComponent<CardInformation>().CardName == "Grand Cannon")
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
                        GetComponent<PlayerController>().RefreshLog(-2, false, "Grand Cannon",  randomEnemyCard.GetComponent<CardInformation>().CardName, Color.red);
                }
                else
                {
                    Debug.LogWarning("Hiç düşman kartı bulunamadı.");
                }
            }
            if(card.GetComponent<CardInformation>().Behemot)
            {
                card.GetComponent<CardInformation>().Behemot = false;
                card.GetComponent<CardInformation>().CardDamage = card.GetComponent<CardInformation>().MaxAttack;
                card.GetComponent<CardInformation>().SetInformation();
            }
            if(card.GetComponent<CardInformation>().CardName == "Eques Automaton")
            {
                card.GetComponent<CardInformation>().CardHealth = card.GetComponent<CardInformation>().MaxHealth;
                card.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                RefreshMyCard(index,
                    card.GetComponent<CardInformation>().CardHealth,
                    card.GetComponent<CardInformation>().HaveShield,
                    card.GetComponent<CardInformation>().CardDamage,
                    card.GetComponent<CardInformation>().DivineSelected,
                    card.GetComponent<CardInformation>().FirstTakeDamage,
                    card.GetComponent<CardInformation>().FirstDamageTaken,
                    card.GetComponent<CardInformation>().EternalShield);
            }
            if(card.GetComponent<CardInformation>().CardName == "Scrapyard Engineer")
            {
                GameObject randomCard = AllOwnCards[UnityEngine.Random.Range(0, AllOwnCards.Length)];
                randomCard.GetComponent<CardInformation>().CardDamage += 2;
                randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                randomCard.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, randomCard.transform.parent.gameObject);
                RefreshMyCard(index,
                    randomCard.GetComponent<CardInformation>().CardHealth,
                    randomCard.GetComponent<CardInformation>().HaveShield,
                    randomCard.GetComponent<CardInformation>().CardDamage,
                    randomCard.GetComponent<CardInformation>().DivineSelected,
                    randomCard.GetComponent<CardInformation>().FirstTakeDamage,
                    randomCard.GetComponent<CardInformation>().FirstDamageTaken,
                    randomCard.GetComponent<CardInformation>().EternalShield);
            }
            if(card.GetComponent<CardInformation>().CardName == "Forest Nymph")
            {
                GameObject[] MyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                foreach (GameObject cards in MyCard)
                {
                    if(cards.GetComponent<CardInformation>().CardName != "Forest Nymph")
                    {
                        cards.GetComponent<CardInformation>().CardHealth = (int.Parse(cards.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                        cards.GetComponent<CardInformation>().SetInformation();
                        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, cards.transform.parent.gameObject);
                        RefreshMyCard(index,
                            cards.GetComponent<CardInformation>().CardHealth,
                            cards.GetComponent<CardInformation>().HaveShield,
                            cards.GetComponent<CardInformation>().CardDamage,
                            cards.GetComponent<CardInformation>().DivineSelected,
                            cards.GetComponent<CardInformation>().FirstTakeDamage,
                            cards.GetComponent<CardInformation>().FirstDamageTaken,
                            cards.GetComponent<CardInformation>().EternalShield);
                    }
                }
            }
            if(card.GetComponent<CardInformation>().CardName == "Scrap Collector")
            {
                GameObject randomCard = AllOwnCards[UnityEngine.Random.Range(0, AllOwnCards.Length)];
                randomCard.GetComponent<CardInformation>().CardDamage += 1;
                randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                randomCard.GetComponent<CardInformation>().SetInformation();
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, randomCard.transform.parent.gameObject);
                RefreshMyCard(index,
                    randomCard.GetComponent<CardInformation>().CardHealth,
                    randomCard.GetComponent<CardInformation>().HaveShield,
                    randomCard.GetComponent<CardInformation>().CardDamage,
                    randomCard.GetComponent<CardInformation>().DivineSelected,
                    randomCard.GetComponent<CardInformation>().FirstTakeDamage,
                    randomCard.GetComponent<CardInformation>().FirstDamageTaken,
                    randomCard.GetComponent<CardInformation>().EternalShield);
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
                card.GetComponent<CardInformation>().SetInformation() ;
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
        if(AsgardQuestion >= 3)
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
        _CardProgress.WindFury = true;
        _CardProgress.ResetAllSign();

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

                      TalkCloud( "It is not my turn!");
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
                    TalkCloud("It is not my turn!");
                }

            }
        }

        _CardProgress.AttackerCard = null;
        _CardProgress.TargetCard = null;
        _CardProgress.TargetCardIndex = -1;
        _CardProgress.SecoundTargetCard = false;
        _CardProgress.ForMyCard = false;


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
                if(CardCurrent.GetComponent<CardInformation>().CardMana < 0)
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
            CreateCard(CardCurrent);
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
            float yPosition = 1.4f;
            float zPosition = 0f;

            Vector3 screenPos1 = new Vector3(1.8f, yPosition, zPosition);
            Vector3 screenPos2 = new Vector3(2.75f, yPosition, zPosition);
            Vector3 screenPos3 = new Vector3(3.7f, yPosition, zPosition);

            positions[0] = screenPos1;
            positions[1] = screenPos2;
            positions[2] = screenPos3;

            for (int i = 0; i < positions.Length; i++)
            {
                Quaternion rotation = Quaternion.Euler(60f, 0f, 180f);
                GameObject CardCurrent = Instantiate(CardPrefabSolo, positions[i], rotation); 
                CardCurrent.tag = "SelectCard";
                CardCurrent.AddComponent<Button>();
                CreateCard(CardCurrent);
                CardCurrent.GetComponent<Button>().onClick.AddListener(() => SelectedCard(CardCurrent));

            }
        }
        else
            Debug.Log("Desten Dolu");
    }

    public void SphinxoftheSands()
    {
        Vector3[] positions = new Vector3[3];
        float yPosition = 1.4f;
        float zPosition = 0f;

        Vector3 screenPos1 = new Vector3(1.8f, yPosition, zPosition);
        Vector3 screenPos2 = new Vector3(2.75f, yPosition, zPosition);
        Vector3 screenPos3 = new Vector3(3.7f, yPosition, zPosition);

        positions[0] = screenPos1;
        positions[1] = screenPos2;
        positions[2] = screenPos3;

        int correctCardIndex = UnityEngine.Random.Range(0, positions.Length);

        for (int i = 0; i < positions.Length; i++)
        {
            Quaternion rotation = Quaternion.Euler(60f, 0f, 180f);
            GameObject Card = Instantiate(Resources.Load<GameObject>("CompetitorCard"), positions[i], rotation);
            Card.tag = "SelectCard";
            Card.AddComponent<Button>();
            CreateCard(Card);

            Card.GetComponent<Button>().onClick.AddListener(() =>
            {
                foreach (GameObject card in GameObject.FindGameObjectsWithTag("SelectCard"))
                {
                    Destroy(card);
                }

                if (i == correctCardIndex)
                {
                    //hasar alınmaz olunacak
                }
            });
        }
    }

    void OsirisFuncion()
    {
        float probability = UnityEngine.Random.Range(0f, 1f);

        if (probability <= 0.4f) // 0.4, %40'lik ihtimale karşılık gelir
        {
            osirisCount++;
            if (osirisCount == 5)
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


                if (emptyFrontCells.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                    int index = emptyFrontCells[randomIndex];
                    CreateSpecialCard("Osiris", "20", 20, 0, index, true);
                }
                else if (emptyBackCells.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                    int index = emptyBackCells[randomIndex];
                    CreateSpecialCard("Osiris", "20", 20, 0, index, true);
                }
                else
                {
                    Debug.LogWarning("No empty cells available to place the Rat card.");
                }
            }
        }
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

    public GameObject CreateRandomCard() //büyü kartı çeker ownmaincarda göre
    {
        if (PV.IsMine)
        {
            if (GameObject.Find("Deck").transform.childCount < 10)
            {
                GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);

                float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                CreateSpell(CardCurrent);
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

    public void DeleteMyCard(int TargetCardIndex)
    {
        if(PV.IsMine)
        {
            GameObject DeadCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[TargetCardIndex].transform.GetChild(0).transform.gameObject;
            if(DeadCard.GetComponent<CardInformation>().CardName=="Stormcaller")
            {
                SpellsExtraDamage -= 1;
            }
            if(DeadCard.GetComponent<CardInformation>().ArtisticInspiration)
            {
                CanAttackMainCard = true;
            }
            if(DeadCard.GetComponent<CardInformation>().CardName == "Engineer of the Ruins")
            {
                DustinCardFuns.EngineeroftheRuinsFun(this);
            }
            if (DeadCard.GetComponent<CardInformation>().CardName == "Salvage Colossus")
            {
                CreateDeckCard("Salvaged Weapon", "5", 2, 0);
            }
            if (DeadCard.GetComponent<CardInformation>().CardName == "Claire")
            {
                DeadCard.GetComponent<CardController>().UsedCard(5, !GetComponent<PlayerController>().PV.Owner.IsMasterClient);
            }
            if(DeadCard.GetComponent<CardInformation>().CardName == "Arcane Scholar")
            {
                SpellsExtraDamage -= 2;
            }
            if (DeadCard.GetComponent<CardInformation>().CardName == "Naiad Protector")
            {
                NaiadProtector = false;
            }
            if (DeadCard.GetComponent<CardInformation>().CardName == "Labyrinth Guardian")
            {
                CreateSpecialCard("Labyrinth Guardian", "5", 6, 6, TargetCardIndex, true);
            }
            if (DeadCard.GetComponent<CardInformation>().CardName == "Radiated Marauder")
            {
                DeadCard.GetComponent<CardController>().UsedCard(2, !GetComponent<PlayerController>().PV.Owner.IsMasterClient);
            }
            if (DeadCard.GetComponent<CardInformation>().CardName == "Norse Axeman")
            {
                _CardProgress.DamageToAlLOtherMinions(1, "Norse Axeman");
            }
            if (DeadCard.GetComponent<CardInformation>().CardName == "Sewer Rat")
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


                if (emptyFrontCells.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                    int index = emptyFrontCells[randomIndex];
                    GameObject Card = CreateSpecialCard("Rat", "1", 1, 0, index, true);
                }
                else if (emptyBackCells.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                    int index = emptyBackCells[randomIndex];
                    GameObject Card = CreateSpecialCard("Rat", "1", 1, 0, index, true);
                }
                else
                {
                    Debug.LogWarning("No empty cells available to place the Rat card.");
                }
            }
            if (DeadCard.GetComponent<CardInformation>().CardName == "Battle Mage")
            {
                SpellsExtraDamage -= 1;
            }
            if(DeadCard.GetComponent<CardInformation>().CardName == "Dwarven Miner")
            {
                Mana++;
                ManaCountText.text = Mana + "/10".ToString();
            }
            if(DeadCard.GetComponent<CardInformation>().CardName == "Sand Elementals")
            {
                LessSpellCost -= 1;
                if (LessSpellCost < 0)
                    LessSpellCost = 0;
            }
            DeadMyCardName.Add(DeadCard.GetComponent<CardInformation>().CardName);
            DeadCardCount++;
            Destroy(DeadCard,2f);

            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_DeleteMyCard", RpcTarget.All, TargetCardIndex);
        }
    }

    [PunRPC]
    void RPC_DeleteMyCard(int TargetCardIndex)
    {
        if (!PV.IsMine)
            return;

        DeadCardCount++;
        Destroy(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[TargetCardIndex].transform.GetChild(0).transform.gameObject,2f);
    }


    public void DeleteAreaCard(int TargetCardIndex)
    {
        if (PV.IsMine)
        {
           

            GameObject DeadCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[TargetCardIndex].transform.GetChild(0).transform.gameObject;
            Destroy(DeadCard,2f);
            DeadCardCount++;
            string DeadCardName = DeadCard.GetComponent<CardInformation>().CardName;

            if (DeadCardName == "Centaur Archer" || DeadCardName == "Minotaur Warrior" || DeadCardName == "Siren" || DeadCardName == "Gorgon" || DeadCardName == "Nemean Lion" || DeadCardName == "Chimera") // ÖLEN KART MONSTER MI?
            {
                DeadMonsterCound++;
                Debug.LogError(DeadMonsterCound + " TANE MONSTER CARD ÖLDÜ");

                TalkCloud("a total of " + DeadMonsterCound + "monsters are dead.");
            }
            else if(DeadCardName== "Keshik Cavalry")
            {
                
                Debug.LogError("keshikdead");

                CreateSpecialCard("Keshik on Foot", "2", 2, 0, TargetCardIndex,false);

                TalkCloud("Keshik Cavalry Dead!");
            }
            else if(DeadCardName== "Flaming Camel")
            {
                _CardProgress.FlamingCamel();
            }
            else if(DeadCardName== "Steppe Warlord")
            {
                _CardProgress.SteppeWarlord();
            }
            else if(DeadCardName== "Stormcaller")
            {
                PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
                foreach (PlayerController obj in objects)
                {
                    if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV != this.PV)
                    {
                        // RPC çağrısı yap
                        obj.GetComponent<PlayerController>().PV.RPC("RPC_SetSpellsExtraDamage", RpcTarget.All, -1);
                    }
                }
            }
            else if(DeadCardName == "Royal Mummy")
            {
                if (_CardProgress.AttackerCard != null)
                {
                    AnubisCardFuns.RoyalMummyFun(_CardProgress.AttackerCard.GetComponent<CardInformation>().CardName, this);
                }
                    
            }
            else if(DeadCardName== "Crypt Warden")
            {
                for (int i = 7; i < 14; i++)
                {

                    if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                    {
                        CreateSpecialCard("Mummy", "1", 1, 0, i, false);
                    }

                }
            }
            else if (DeadCardName == "Arcane Scholar")
            {
                PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
                foreach (PlayerController obj in objects)
                {
                    if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV != this.PV)
                    {
                        // RPC çağrısı yap
                        obj.GetComponent<PlayerController>().PV.RPC("RPC_SetSpellsExtraDamage", RpcTarget.All, -2);
                    }
                }
            }
            if (_CardProgress.AttackerCard != null)
            {
                if (_CardProgress.AttackerCard.GetComponent<CardInformation>().CardName == "Zeus")
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
                                randomCard.GetComponent<CardInformation>().FirstDamageTaken,
                                randomCard.GetComponent<CardInformation>().EternalShield);
                        }

                    }
                }
                else if(_CardProgress.AttackerCard.GetComponent<CardInformation>().CardName == "Mongol Lancer")
                {
                    if (TargetCardIndex > 6 && TargetCardIndex <14)
                    {
                        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard");

                        foreach (GameObject target in allTargets)
                        {
                            if (target != DeadCard)
                            {
                                float distance = Vector3.Distance(DeadCard.transform.position, target.transform.position);

                                if (distance <= 0.80f)
                                {
                                    Vector3 directionToTarget = (target.transform.position - DeadCard.transform.position).normalized;

                                    float dotProductBackward = Vector3.Dot(-DeadCard.transform.up, directionToTarget);

                                    if (dotProductBackward > 0.5f)
                                    {
                                        Debug.Log("Arkasında kart var");
                                        target.GetComponent<CardInformation>().CardHealth =(int.Parse(target.GetComponent<CardInformation>().CardHealth) - _CardProgress.AttackerCard.GetComponent<CardInformation>().CardDamage).ToString();
                                        int index= Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions,target.transform.parent.gameObject);
                                        RefreshUsedCard(index, target.GetComponent<CardInformation>().CardHealth, target.GetComponent<CardInformation>().CardDamage);
                                        CreateTextAtTargetIndex(index, _CardProgress.AttackerCard.GetComponent<CardInformation>().CardDamage, false, _CardProgress.AttackerCard.GetComponent<CardInformation>().CardName);
                                        RefreshLog(-_CardProgress.AttackerCard.GetComponent<CardInformation>().CardDamage, true, _CardProgress.AttackerCard.GetComponent<CardInformation>().CardName, target.GetComponent<CardInformation>().CardName, Color.red);
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
                else if(_CardProgress.AttackerCard.GetComponent<CardInformation>().CardName == "Viking Raider")
                {
                    Mana += 1;
                    ManaCountText.text = Mana.ToString() + "/10";
                    OwnManaBar.fillAmount = Mana / 10f;
                    CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;
                    CompetitorManaCountText.text = (_GameManager.ManaCount) + "/10".ToString();
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_RefreshCompetitorMana", RpcTarget.Others, Mana);
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

        GameObject DeadCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[TargetCardIndex].transform.GetChild(0).transform.gameObject;
        DeadMyCardName.Add(DeadCard.GetComponent<CardInformation>().CardName);
        DeadCardCount++;
        if(DeadCard.GetComponent<CardInformation>().SunDiskRadiance == true)
        {
            CardInformation deadcard = DeadCard.GetComponent<CardInformation>();
            string name = deadcard.CardName;
            string health = deadcard.MaxHealth;
            int damage = deadcard.CardDamage;
            int mana = deadcard.CardMana;
            CreateDeckCard(name,health,damage,mana);
        }
        if(DeadCard.GetComponent<CardInformation>().CardName== "Engineer of the Ruins")
        {
            DustinCardFuns.EngineeroftheRuinsFun(this);
        }
        if(DeadCard.GetComponent<CardInformation>().CardName == "Rogue AI Drone")
        {
            DestroyAndCreateMyDeck(DeadCard);
        }
        if (DeadCard.GetComponent<CardInformation>().CardName == "Dune Raider")
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
                    CreateSpecialCard("Scavenger", "1", 1, 0, index, true);
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
                        CreateSpecialCard("Scavenger", "1", 1, 0, index, true);
                        emptyBackCells.RemoveAt(randomIndex);
                    }
                }
                else
                {
                    Debug.LogWarning("No empty cells available to place the Scavenger card.");
                }
            }
        }

        if (DeadCard.GetComponent<CardInformation>().CardName == "Warlord")
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

            if (emptyFrontCells.Count >= 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                    int index = emptyFrontCells[randomIndex];
                    CreateSpecialCard("Wasteland Warrior", "3", 3, 0, index, true);
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

                if (emptyBackCells.Count >= 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                        int index = emptyBackCells[randomIndex];
                        CreateSpecialCard("Wasteland Warrior", "3", 3, 0, index, true);
                        emptyBackCells.RemoveAt(randomIndex);
                    }
                }
                else
                {
                    Debug.LogWarning("No empty cells available to place the Wasteland Warrior card.");
                }
            }
        }
        if(DeadCard.GetComponent<CardInformation>().CardName == "Salvage Colossus")
        {
            CreateDeckCard("Salvaged Weapon", "5", 2, 0);
        }
        if (DeadCard.GetComponent<CardInformation>().CardName == "Claire")
        {
            DeadCard.GetComponent<CardController>().UsedCard(5, !GetComponent<PlayerController>().PV.Owner.IsMasterClient);
        }
        if (DeadCard.GetComponent<CardInformation>().CardName == "Dune Raider")
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
                    CreateSpecialCard("Frost Drakes", "4", 5, 0, index, true);
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
                        CreateSpecialCard("Frost Drakes", "4", 5, 0, index, true);
                        emptyBackCells.RemoveAt(randomIndex);
                    }
                }
                else
                {
                    Debug.LogWarning("No empty cells available to place the Scavenger card.");
                }
            }
        }
        if(DeadCard.GetComponent<CardInformation>().CardName == "Naiad Protector")
        {
            NaiadProtector = false;
        }
        if(DeadCard.GetComponent<CardInformation>().CardName == "Labyrinth Guardian")
        {
            CreateSpecialCard("Labyrinth Guardian", "5", 6, 6, TargetCardIndex, true);
        }
        if(DeadCard.GetComponent<CardInformation>().CardName == "Radiated Marauder")
        {
            DeadCard.GetComponent<CardController>().UsedCard(2, !GetComponent<PlayerController>().PV.Owner.IsMasterClient);
        }
        if(DeadCard.GetComponent<CardInformation>().CardName == "Norse Axeman")
        {
            _CardProgress.DamageToAlLOtherMinions(1, "Norse Axeman");
        }
        if(DeadCard.GetComponent<CardInformation>().CardName == "Sewer Rat")
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


            if (emptyFrontCells.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                int index = emptyFrontCells[randomIndex];
                GameObject Card = CreateSpecialCard("Rat", "1", 1, 0, index, true);
            }
            else if (emptyBackCells.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                int index = emptyBackCells[randomIndex];
                GameObject Card = CreateSpecialCard("Rat", "1", 1, 0, index, true);
            }
            else
            {
                Debug.LogWarning("No empty cells available to place the Rat card.");
            }
        }
        if(DeadCard.GetComponent<CardInformation>().CardName == "Battle Mage")
        {
            SpellsExtraDamage -= 1;
        }

        if (DeadCard.GetComponent<CardInformation>().CardName == "Dwarven Miner")
        {
            Mana++;
            ManaCountText.text = Mana + "/10".ToString();
        }
        if (DeadCard.GetComponent<CardInformation>().CardName == "Sand Elementals")
        {
            LessSpellCost -= 1;
            if (LessSpellCost < 0)
                LessSpellCost = 0;
        }
        Destroy(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[TargetCardIndex].transform.GetChild(0).transform.gameObject, 2f);

    }

    public void CreateDeckCard(string name,string health,int damage,int mana)
    {
        GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
        CardCurrent.GetComponent<CardInformation>().CardName = name;
        CardCurrent.GetComponent<CardInformation>().CardHealth = health;
        CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
        CardCurrent.GetComponent<CardInformation>().CardMana = mana;

        StackDeck();
        StackCompetitorDeck();
        DeckCardCount++;

        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateRandomCard", RpcTarget.All);
    }

    public void CreateHoplitesCard(int CreateCardIndex)
    {
        if (PV.IsMine)
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
        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
        CardCurrent.GetComponent<CardInformation>().SetInformation();
    }


    public void RefreshUsedCard(int boxindex, string heatlh,int damage)
    {
        if (PV.IsMine)
        {

            GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject.GetComponent<CardInformation>().SetInformation();

            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_RefreshUsedCard", RpcTarget.All, boxindex, heatlh, damage);
        }


    }

    [PunRPC]
    public void RPC_RefreshUsedCard(int boxindex, string heatlh,int damage)
    {

        if (PV.IsMine)
        {
            GameObject CardCurrent = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject;
         
            CardCurrent.GetComponent<CardInformation>().CardHealth = heatlh;
            CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
            CardCurrent.GetComponent<CardInformation>().SetInformation();

        }
    }

    [PunRPC]
    public void RPC_RefreshMaxDamage(int boxindex,bool behemot)
    {
        if (PV.IsMine)
        {
            GameObject CardCurrent = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject;

            CardCurrent.GetComponent<CardInformation>().Behemot = behemot;

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

    public void RefreshEnemyAllBuff(int boxindex)
    {
        CardInformation card = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject.GetComponent<CardInformation>();
        card.HaveShield = false;
        card.DivineSelected = false;
        card.GerDefense = false;
        card.MongolFury = false;
        card.EternalShield = false;
        card.Gallop = false;
        card.Invulnerable = false;
        card.SunDiskRadiance = false;
        card.ArtisticInspiration = false;
        card.Behemot = false;
        card.MutatedBlood = false;
        card.PlagueCarrier = false;
        card.ToxicRainmaker = false;
        card.ChargeBrokandSindri = 0;
    }

    [PunRPC]
    public void RPC_RefreshEnemyAllBuff(int boxindex)
    {
        CardInformation card = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject.GetComponent<CardInformation>();
        card.HaveShield = false;
        card.DivineSelected = false;
        card.GerDefense = false;
        card.MongolFury = false;
        card.EternalShield = false;
        card.Gallop = false;
        card.Invulnerable = false;
        card.SunDiskRadiance = false;
        card.ArtisticInspiration = false;
        card.Behemot = false;
        card.MutatedBlood = false;
        card.PlagueCarrier = false;
        card.ToxicRainmaker = false;
        card.ChargeBrokandSindri = 0;
    }

    public void RefreshMyCard(int boxindex, string heatlh,bool haveshield,int damage,bool divineselected,bool firstdamage,bool firstdamagetaken,bool eternalshield)
    {
        if (PV.IsMine)
        {

            GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform.GetChild(0).transform.gameObject.GetComponent<CardInformation>().SetInformation();       //kendi kart bilgimi güncelleme

            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_RefreshMyCard", RpcTarget.All, boxindex, heatlh,haveshield,damage,divineselected,firstdamage,firstdamagetaken,eternalshield);
        }
    }

    [PunRPC]
    public void RPC_RefreshMyCard(int boxindex, string heatlh,bool haveshield,int damage,bool divineselected,bool firstdamage,bool firstdamagetaken,bool eternalshield)
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
            CardCurrent.GetComponent<CardInformation>().EternalShield=eternalshield;
            CardCurrent.GetComponent<CardInformation>().SetInformation();

        }
    }

    [PunRPC]
    public void RPC_ShuffleCells(int[] shuffledIndexes)
    {
        List<Transform> allCells = new List<Transform>();
        List<Transform> cardTransforms = new List<Transform>();

        for (int i = 0; i < 14; i++)
        {
            var backAreaCollision = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].transform;
            allCells.Add(backAreaCollision);

            if (backAreaCollision.childCount != 0)
            {
                cardTransforms.Add(backAreaCollision.GetChild(0));
            }
        }

        int cardIndex = 0;
        for (int i = 0; i < shuffledIndexes.Length; i++)
        {
            Transform targetCell = allCells[shuffledIndexes[i]];

            if (targetCell.childCount == 0 && cardIndex < cardTransforms.Count)
            {
                cardTransforms[cardIndex].SetParent(targetCell);
                cardTransforms[cardIndex].localPosition = Vector3.zero;
                cardIndex++;
            }
        }
    }

    public void DestroyAndCreateMyDeck(GameObject CardCurrent)
    {
        
        GameObject deckObject = GameObject.Find("Deck");
        if (deckObject.transform.childCount < 10)
        {
            string cardName =CardCurrent.GetComponent<CardInformation>().CardName;
            string cardDes = cardName + " POWWERRRRR!!!";
            string cardHealth = CardCurrent.GetComponent<CardInformation>().CardHealth;
            int cardDamage =CardCurrent.GetComponent<CardInformation>().CardDamage;
            int cardMana =CardCurrent.GetComponent<CardInformation>().CardMana;


            GameObject Card = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);

            float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
            Card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz

            Card.GetComponent<CardInformation>().CardName = cardName;
            Card.GetComponent<CardInformation>().CardDes = cardDes;
            Card.GetComponent<CardInformation>().CardHealth = cardHealth;
            Card.GetComponent<CardInformation>().CardDamage = cardDamage;
            Card.GetComponent<CardInformation>().CardMana = cardMana;
            Card.GetComponent<CardInformation>().SetInformation();


            StackDeck();
            StackCompetitorDeck();
            DeckCardCount++;
            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateRandomCard", RpcTarget.All);
        }
        Destroy(CardCurrent);
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, CardCurrent.transform.parent.gameObject);
        CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_DeleteMyCard", RpcTarget.All, index);

    }

    public void ScalesOfAnubis(int index)
    {
        GameObject DeadCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[index].transform.GetChild(0).transform.gameObject;
        DeleteAreaCard(index);

        GameObject deckObject = GameObject.Find("CompetitorDeck");
        if (deckObject.transform.childCount < 10)
        {
            string cardName = DeadCard.GetComponent<CardInformation>().CardName;
            string cardDes = cardName + " POWWERRRRR!!!";
            string cardHealth = DeadCard.GetComponent<CardInformation>().CardHealth.ToString();
            int cardDamage = DeadCard.GetComponent<CardInformation>().CardDamage;
            int cardMana = DeadCard.GetComponent<CardInformation>().CardMana;


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



    private void AssignAnimatorController()
    {
        // Define the path relative to Resources folder (without "Assets/" and ".controller")
        string path = $"Controller/{OwnMainCard}";
    
        // Attempt to load the RuntimeAnimatorController from Resources
        RuntimeAnimatorController animatorController = Resources.Load<RuntimeAnimatorController>(path);
    
        // Check if controller was found
        if (animatorController != null)
        {
            HeroAnimator.runtimeAnimatorController = animatorController;
            Debug.Log($"{OwnMainCard} controller loaded and assigned successfully.");
        }
        else
        {
            Debug.LogWarning($"Animator controller for {OwnMainCard} not found at path: Resources/{path}");
        }
    }

    private bool AssignHeroParticleController()
    {
        Debug.Log(OwnMainCard + "_Aura");
        if (OwnMainCard == "") return false;
        HeroParticleController = GameObject.Find(OwnMainCard + "_Aura").transform.GetChild(0)
            .GetComponent<HeroParticleController>();
       
        return true;

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
                    if (Mana >= 2 && CanAttackMainCard)
                    {
                        existingObject.GetComponent<CardInformation>().CardName = "Zeus";
                        existingObject.GetComponent<CardInformation>().CardDamage = _GameManager.MasterAttackDamage;
                        existingObject.GetComponent<CardInformation>().CardMana = 2;
                        //HeroAnimator.SetTrigger("Ulti");
                        HeroParticleController.PlayHeroPower();
                        _CardProgress.SetMainAttackerCard(existingObject);
                    }
                    else
                    {
                        print("mana yetersiz");
                    }
                    break;
                case "Genghis":
                    if (Mana >= 2 && CanAttackMainCard)
                    {
                        existingObject.GetComponent<CardInformation>().CardName = "Genghis";
                        existingObject.GetComponent<CardInformation>().CardDamage = _GameManager.MasterAttackDamage;
                        existingObject.GetComponent<CardInformation>().CardMana = 2;
                        //HeroAnimator.SetTrigger("Ulti");
                        HeroParticleController.PlayHeroPower();
                        if (NomadsLand >= 5)
                        {
                            if (_GameManager.MasterAttackDamage <= 2)
                            {
                                Debug.Log("Nomadiclands Aktif");
                                _GameManager.MasterAddAttackDamage(1);
                            }
                        }

                        existingObject.GetComponent<CardController>().UsedCard(existingObject.GetComponent<CardInformation>().CardDamage, PV.Owner.IsMasterClient);

                        SetMana(existingObject);
                        GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
                        foreach (GameObject card in mycards)
                        {
                            if (card.GetComponent<CardInformation>().CardName == "Horse Breeder")
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
                                    Debug.LogWarning("No empty cells available to place the steppe ehorse card.");
                                }
                            }
                        }

                    }
                    else
                    {
                        print("mana yetersiz");
                    }
                    break;
                case "Odin":
                    if (AsgardQuestion >= 3 && Mana >= 1 && CanAttackMainCard)
                    {
                        existingObject.GetComponent<CardInformation>().CardMana = 1;
                        existingObject.GetComponent<CardInformation>().CardName = "Odin";
                        existingObject.GetComponent<CardInformation>().CardDamage = _GameManager.MasterAttackDamage;
                        SetMana(existingObject);
                        //HeroAnimator.SetTrigger("Ulti");
                        HeroParticleController.PlayHeroPower();
                        GameObject spawnedObject = SpawnAndReturnGameObject();

                        if (spawnedObject.GetComponent<CardInformation>().CardHealth == "")
                        {
                            Debug.LogError("Odin SPEEEELLL YARATTTI ");
                            TalkCloud("Odin created a spell..");

                            spawnedObject.GetComponent<CardInformation>().CardMana--;

                        }
                        else
                        {
                            Debug.LogError("Odin MİNNYOONNNN YARATTTI ");
                            TalkCloud("Odin created a minion..");
                        }
                    }
                    else if (AsgardQuestion < 3 && Mana >= 2 && CanAttackMainCard)
                    {
                        existingObject.GetComponent<CardInformation>().CardMana = 2;
                        existingObject.GetComponent<CardInformation>().CardName = "Odin";
                        existingObject.GetComponent<CardInformation>().CardDamage = _GameManager.MasterAttackDamage;
                        SetMana(existingObject);
                        //HeroAnimator.SetTrigger("Ulti");
                        HeroParticleController.PlayHeroPower();
                        GameObject spawnedObject = SpawnAndReturnGameObject();

                        if (spawnedObject.GetComponent<CardInformation>().CardHealth == "")
                        {
                            Debug.LogError("Odin SPEEEELLL YARATTTI ");
                            TalkCloud("Odin created a spell..");

                            spawnedObject.GetComponent<CardInformation>().CardMana--;

                        }
                        else
                        {
                            Debug.LogError("Odin MİNNYOONNNN YARATTTI ");
                            TalkCloud("Odin created a minion..");
                        }
                    }
                    else
                    {
                        print("mana yetersiz");
                    }
                    break;
                case "Anubis":
                    if (Mana >= 2 && CanAttackMainCard)
                    {
                        existingObject.GetComponent<CardInformation>().CardName = "Anubis";
                        existingObject.GetComponent<CardInformation>().CardDamage = _GameManager.MasterAttackDamage;
                        existingObject.GetComponent<CardInformation>().CardMana = 2;
                        //HeroAnimator.SetTrigger("Ulti");
                        HeroParticleController.PlayHeroPower();
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
                            if (DeadMyCardName.Count > 4)
                            {
                                string targetCardName = "Mummy";
                                string cardHealth = "2";
                                int cardDamage = 2;
                                CreateSpecialCard(targetCardName, cardHealth, cardDamage, 0, index, true);
                            }
                            else
                            {
                                string targetCardName = "Worker";
                                string cardHealth = "1";
                                int cardDamage = 1;
                                CreateSpecialCard(targetCardName, cardHealth, cardDamage, 0, index, true);
                            }
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
                                if (DeadMyCardName.Count > 4)
                                {
                                    string targetCardName = "Mummy";
                                    string cardHealth = "2";
                                    int cardDamage = 2;
                                    CreateSpecialCard(targetCardName, cardHealth, cardDamage, 0, index, true);
                                }
                                else
                                {
                                    string targetCardName = "Worker";
                                    string cardHealth = "1";
                                    int cardDamage = 1;
                                    CreateSpecialCard(targetCardName, cardHealth, cardDamage, 0, index, true);
                                }
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
                        print("mana yetersiz");
                    }

                    break;
                case "Leonardo Da Vinci":
                    if (Mana >= 2 && CanAttackMainCard)
                    {
                        existingObject.GetComponent<CardInformation>().CardName = "Leonardo Da Vinci";
                        existingObject.GetComponent<CardInformation>().CardDamage = _GameManager.MasterAttackDamage;
                        existingObject.GetComponent<CardInformation>().CardMana = 2;
                        //HeroAnimator.SetTrigger("Ulti");
                        HeroParticleController.PlayHeroPower();
                        _CardProgress.SetMainAttackerCard(existingObject);
                        _CardProgress.SecoundTargetCard = true;
                        _CardProgress.ForMyCard = true;
                    }
                    else
                    {
                        print("mana yetersiz");
                    }
                    break;
                case "Dustin":
                    if (Mana >= 2 && CanAttackMainCard)
                    {
                        existingObject.GetComponent<CardInformation>().CardName = "Dustin";
                        existingObject.GetComponent<CardInformation>().CardDamage = _GameManager.MasterAttackDamage;
                        existingObject.GetComponent<CardInformation>().CardMana = 2;
                        //HeroAnimator.SetTrigger("Ulti");
                        HeroParticleController.PlayHeroPower();
                        print("Fix gelecek");
                    }
                    else
                    {
                        print("mana yetersiz");
                    }
                    break;

            }
            
            if (PV.IsMine)
            {
                CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                PV.RPC("RefreshPlayersInformation", RpcTarget.All);
            }

        }

      
    }

    public void RuneMagic()
    {
        if(PV.IsMine)
        {
            healthTextObject.SetActive(true);
            attackTextObject.SetActive(true);
            drawCardTextObject.SetActive(true);

            healthTextObject.GetComponent<Button>().onClick.AddListener(() => OnTextClicked(healthTextObject));
            attackTextObject.GetComponent<Button>().onClick.AddListener(() => OnTextClicked(attackTextObject));
            drawCardTextObject.GetComponent<Button>().onClick.AddListener(() => OnTextClicked(drawCardTextObject));
        }
    }

    void OnTextClicked(GameObject clickedTextObject)
    {
        if (clickedTextObject == healthTextObject)
        {
            GetComponent<CardController>().AddHealCard(2, !PV.Owner.IsMasterClient);
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

    public void GatesofDuat()
    {
        if (PV.IsMine)
        {
            Freeze.SetActive(true);
            Mummies.SetActive(true);

            Freeze.GetComponent<Button>().onClick.AddListener(() => OnGameobjectClicked(Freeze));
            Mummies.GetComponent<Button>().onClick.AddListener(() => OnGameobjectClicked(Mummies));
        }
    }

    void OnGameobjectClicked(GameObject clickedTextObject)
    {
        if (clickedTextObject == Freeze)
        {
            _CardProgress.FreezeAllEnemyMinions("Gates of Duat");
        }
        else if (clickedTextObject == Mummies)
        {
            int createdCardCount = 0;

            for (int i = 7; i < 14 && createdCardCount < 6; i++)
            {
                if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                {
                    CreateSpecialCard("Mummy", "1", 1, 0, i, true);
                    createdCardCount++;
                }
            }

        }

        Freeze.SetActive(false);
        Mummies.SetActive(false);
    }

    public void ScavengerRaider()
    {
        if (PV.IsMine)
        {
            drawCardTextObject.SetActive(true);
            TakeGun.SetActive(true);

            drawCardTextObject.GetComponent<Button>().onClick.AddListener(() => OnGameobjectClicked2(drawCardTextObject));
            TakeGun.GetComponent<Button>().onClick.AddListener(() => OnGameobjectClicked2(TakeGun));
        }
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

    public void MinorOracleofDelphi()
    {
        if (PV.IsMine)
        {
            Minion.SetActive(true);
            Spell.SetActive(true);

            Minion.GetComponent<Button>().onClick.AddListener(() => OnGameobjectClicked3(Minion));
            Spell.GetComponent<Button>().onClick.AddListener(() => OnGameobjectClicked3(Spell));
        }
    }

    void OnGameobjectClicked3(GameObject clickedTextObject)
    {
        int randomValue = UnityEngine.Random.Range(0, 2); 

        if (randomValue == 0)
        {
            if (clickedTextObject == Minion)
            {
                if (GameObject.Find("Deck").transform.childCount < 10)
                {
                    GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                    CardCurrent.tag = "Card";
                    float xPos = DeckCardCount * 0.8f - 0.8f; 
                    CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); 
                    CreateCard(CardCurrent);
                    DeckCardCount++;
                }
            }
        }
        else if (randomValue == 1)
        {
            if (clickedTextObject == Spell)
            {
                CreateRandomCard();
            }
        }

        Minion.SetActive(false);
        Spell.SetActive(false);
    }


    public void SetMana(GameObject attackercard)
    {
        if(attackercard.GetComponent<CardInformation>().CardName=="Zeus" || attackercard.GetComponent<CardInformation>().CardName == "Genghis" || attackercard.GetComponent<CardInformation>().CardName == "Odin" || attackercard.GetComponent<CardInformation>().CardName == "Leonardo Da Vinci" || attackercard.GetComponent<CardInformation>().CardName == "Anubis")
        {
            Mana -= attackercard.GetComponent<CardInformation>().CardMana;
            CanAttackMainCard = false;
            ManaCountText.text = Mana.ToString() + "/10";
            OwnManaBar.fillAmount = Mana / 10f;
            DeactivateAllGreens();
            ActiveAllGreens();
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
                print("Beginner Function Turn Count if");
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
                WhoseTurnText.text = "Finish Turn";
                finishButton.interactable = true;
                OnManaChange(Mana);
                timeTween.Kill(true);
                countDownImage.fillAmount = 1;
                whoseTurnImage.sprite = myTurnSprite;
                timeTween=countDownImage.DOFillAmount(0, 60f);



            }
            else // DIĞER TURLAR
            {
                
                if(GameObject.Find("Deck").transform.childCount<10)
                {
                    if (MerfolkScoutBool && NextCard!=null)
                    {
                        GameObject CardCurrent = Instantiate(NextCard, GameObject.Find("Deck").transform);
                        CardCurrent.tag = "Card";
                        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                        DeckCardCount++;
                        NextCard = null;
                        MerfolkScoutBool = false;
                    }
                    else
                    {
                        GameObject CardCurrent = Instantiate(CardPrefabSolo, GameObject.Find("Deck").transform);
                        CardCurrent.tag = "Card";
                        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                        CreateCard(CardCurrent);
                        DeckCardCount++;
                    }
                    
                }
                
                StackDeck();
                StackCompetitorDeck();


                Transform deckTransform = GameObject.Find("Deck").transform;
                int childCount = deckTransform.childCount;

                for (int i = 0; i < childCount; i++)
                {
                    Transform child = deckTransform.GetChild(i);
                    CardInformation cardInfo = child.GetComponent<CardInformation>();
                    if (cardInfo != null && cardInfo.CardMana <= _GameManager.ManaCount)
                    {
                        Transform green = child.Find("Green");
                        if (green != null)
                        {
                            green.gameObject.SetActive(true);
                        }
                    }
                }
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
                            card.GetComponent<CardInformation>().FirstDamageTaken,
                            card.GetComponent<CardInformation>().EternalShield);
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
                            card.GetComponent<CardInformation>().FirstDamageTaken,
                            card.GetComponent<CardInformation>().EternalShield);
                    }
                    if (card.GetComponent<CardInformation>().GerDefense == true)
                    {
                        card.GetComponent<CardInformation>().GerDefense = false;
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) - 2).ToString();
                        card.GetComponent<CardInformation>().SetInformation();
                        if (int.Parse(card.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
                        {
                            int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                            DeleteAreaCard(index);
                        }
                    }
                    if (card.GetComponent<CardInformation>().EternalShield == true)
                    {
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);      //tur sonu eternalshiled false yap
                        card.GetComponent<CardInformation>().EternalShield = false;
                        RefreshMyCard(TargetCardIndex,
                            card.GetComponent<CardInformation>().CardHealth,
                            card.GetComponent<CardInformation>().HaveShield,
                            card.GetComponent<CardInformation>().CardDamage,
                            card.GetComponent<CardInformation>().DivineSelected,
                            card.GetComponent<CardInformation>().FirstTakeDamage,
                            card.GetComponent<CardInformation>().FirstDamageTaken,
                            card.GetComponent<CardInformation>().EternalShield);
                    }
                    if(card.GetComponent<CardInformation>().CardName == "Brokk and Sindri")
                    {
                        card.GetComponent<CardInformation>().ChargeBrokandSindri++;
                        if(card.GetComponent<CardInformation>().ChargeBrokandSindri == 3)
                        {
                            _CardProgress.CharceBrokandSindri();
                        }
                    }
                    if(card.GetComponent<CardInformation>().MutatedBlood == true)
                    {
                        GameObject[] RandomEnemyCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
                        if (RandomEnemyCards.Length > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, RandomEnemyCards.Length);
                            GameObject selectedEnemyCard = RandomEnemyCards[randomIndex];
                            _CardProgress.StandartDamage(card, selectedEnemyCard);
                        }
                    }
                    if(card.GetComponent<CardInformation>().CardName == "Siege Master Urban")
                    {
                        StandartCardFuns.SiegeMasterUrbanFun(this);
                    }
                    if(card.GetComponent<CardInformation>().CardName == "Gaelic Warrior")
                    {
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                        DeleteAreaCard(TargetCardIndex);
                    }
                    if(card.GetComponent<CardInformation>().CardName == "Wall Builder")
                    {
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                        card.GetComponent<CardInformation>().SetInformation();
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);           
                        RefreshMyCard(TargetCardIndex,
                            card.GetComponent<CardInformation>().CardHealth,
                            card.GetComponent<CardInformation>().HaveShield,
                            card.GetComponent<CardInformation>().CardDamage,
                            card.GetComponent<CardInformation>().DivineSelected,
                            card.GetComponent<CardInformation>().FirstTakeDamage,
                            card.GetComponent<CardInformation>().FirstDamageTaken,
                            card.GetComponent<CardInformation>().EternalShield);
                    }
                    if (card.GetComponent<CardInformation>().CardName == "Tavern Brawler")
                    {
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                        card.GetComponent<CardInformation>().SetInformation();
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                        RefreshMyCard(TargetCardIndex,
                            card.GetComponent<CardInformation>().CardHealth,
                            card.GetComponent<CardInformation>().HaveShield,
                            card.GetComponent<CardInformation>().CardDamage,
                            card.GetComponent<CardInformation>().DivineSelected,
                            card.GetComponent<CardInformation>().FirstTakeDamage,
                            card.GetComponent<CardInformation>().FirstDamageTaken,
                            card.GetComponent<CardInformation>().EternalShield);
                    }
                    if(card.GetComponent<CardInformation>().CardName == "Berserker Thrall")
                    {
                        GameObject[] RandomEnemyCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
                        if (RandomEnemyCards.Length > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, RandomEnemyCards.Length);
                            GameObject selectedEnemyCard = RandomEnemyCards[randomIndex];
                            _CardProgress.StandartDamage(card, selectedEnemyCard);
                        }
                    }

                }

                if (OwnMainCard == "Anubis")
                {
                    OsirisFuncion();
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
                                Cards.GetComponent<CardInformation>().CardHealth,
                                Cards.GetComponent<CardInformation>().HaveShield, 
                                Cards.GetComponent<CardInformation>().CardDamage,
                                Cards.GetComponent<CardInformation>().DivineSelected,
                                Cards.GetComponent<CardInformation>().FirstTakeDamage,
                                Cards.GetComponent<CardInformation>().FirstDamageTaken,
                                Cards.GetComponent<CardInformation>().EternalShield);
                        }
                    }
                }
                //bool stormcallerExists = false;

                //foreach (var card in AllEnemyCards)
                //{
                //    var cardInfo = card.GetComponent<CardInformation>();
                //    if (cardInfo != null)
                //    {
                //        if (cardInfo.CardName == "Stormcaller")
                //        {
                //            stormcallerExists = true;
                //            break;
                //        }
                //    }
                //}
                //PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
                //if (stormcallerExists)
                //{
                //    foreach (PlayerController obj in objects)
                //    {
                //        if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV == this.PV)
                //        {
                //            // RPC çağrısı yap
                //            obj.GetComponent<PlayerController>().PV.RPC("RPC_SetSpellsExtraDamage", RpcTarget.All, 1);
                //        }
                //    }
                //}
                //else
                //{
                //    foreach (PlayerController obj in objects)
                //    {
                //        if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV != this.PV)
                //        {
                //            // RPC çağrısı yap
                //            obj.GetComponent<PlayerController>().PV.RPC("RPC_SetSpellsExtraDamage", RpcTarget.All,-1);
                //        }
                //    }
                //}
                if(GodsBaneUsed==true)
                {
                    Debug.Log("godbande dropmana");
                    CompetitorPV.GetComponent<PlayerController>().PV.RPC("GodsBane", RpcTarget.All, -2);
                    GodsBaneUsed = false;
                }
                _CardProgress.BattleableCard();
                WhoseTurnText.text = "Finish Turn";
                timeTween.Kill(true);
                finishButton.interactable = true;
                OnManaChange(Mana);
                countDownImage.fillAmount = 1;
                whoseTurnImage.sprite = myTurnSprite;
                timeTween=countDownImage.DOFillAmount(0, 60f);

            }

        }
        else
        {
            if (_GameManager.TurnCount < 1) // ilk turun fon.
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject card = Instantiate(Resources.Load<GameObject>("CompetitorCard"), GameObject.Find("CompetitorDeck").transform);
                    card.tag = "CompetitorDeckCard";
                    float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                    card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                    //CreateCard(card);
                    StackDeck();
                    StackCompetitorDeck();
                    DeckCardCount++;
                }
                HeroNftCard();
                GameObject Herocard = Instantiate(Resources.Load<GameObject>("CompetitorHeoCard"), GameObject.Find("CompetitorHeroPivot").transform);
            }
            else // DIĞER TURLAR
            {
                if (GameObject.Find("CompetitorDeck").transform.childCount < 10)
                {
                    GameObject CardCurrent = Instantiate(Resources.Load<GameObject>("CompetitorCard"), GameObject.Find("CompetitorDeck").transform);
                    CardCurrent.tag = "CompetitorDeckCard";
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

        CompetitorManaCountText.text = _GameManager.ManaCount + "/10".ToString();
        CompetitorManaBar.fillAmount = _GameManager.ManaCount / 10;

    }

    [PunRPC]
    public void RPC_SetSpellsExtraDamage(int damage)
    {
        SpellsExtraDamage += damage;
    }

    [PunRPC]
    void RPC_SteppeAmbush(bool steppeambush)
    {
        PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
        foreach (PlayerController obj in objects)
        {
            if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV != this.PV)
            {
                obj.GetComponent<PlayerController>().SteppeAmbush = steppeambush;
            }
        }
    }
    public void UsedSpell(GameObject SpellCard)
    {
        if (!PV.IsMine)
            return;
        PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
        foreach (PlayerController obj in objects)
        {
            if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV != this.PV && obj.GetComponent<PlayerController>().SteppeAmbush ==true)
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
            if(Card.GetComponent<CardInformation>().CardName == "Anatomist of the Unknown")
            {
                if (myCards.Length > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, myCards.Length);
                    GameObject randomCard = myCards[randomIndex];
                    int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, randomCard.transform.parent.gameObject);
                    randomCard.GetComponent<CardInformation>().CardDamage += 2;
                    randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                    RefreshMyCard(TargetCardIndex,
                                randomCard.GetComponent<CardInformation>().CardHealth,
                                randomCard.GetComponent<CardInformation>().HaveShield,
                                randomCard.GetComponent<CardInformation>().CardDamage,
                                randomCard.GetComponent<CardInformation>().DivineSelected,
                                randomCard.GetComponent<CardInformation>().FirstTakeDamage,
                                randomCard.GetComponent<CardInformation>().FirstDamageTaken,
                                randomCard.GetComponent<CardInformation>().EternalShield);
                }
                else if(Card.GetComponent<CardInformation>().CardName == "Automaton Apprentice")
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

        if(NaiadProtector)
        {
            GetComponent<CardController>().AddHealCard(1, !PV.Owner.IsMasterClient);
        }

        UsedSpellCard.Add(SpellCard);

        PlayedSpell = true;
    }
    [PunRPC]
    void RPC_NomadicTactics(bool nomadictactics)
    {
        PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
        foreach (PlayerController obj in objects)
        {
            if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV != this.PV)
            {
                obj.GetComponent<PlayerController>().NomadicTactics = nomadictactics;
            }
        }
    }
    public void CheckNomadicTactics()
    {
        if (!PV.IsMine)
            return;


        PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
        foreach (PlayerController obj in objects)
        {

            if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV != this.PV && obj.GetComponent<PlayerController>().NomadicTactics == true)
            {

                GameObject TargetCard = _CardProgress.TargetCard;
                GameObject[] mycards = GameObject.FindGameObjectsWithTag("CompetitorCard");
                GameObject attackercard = _CardProgress.AttackerCard;

                foreach (GameObject target in mycards)
                {

                    if (target != TargetCard)
                    {
                        float distance = Vector3.Distance(TargetCard.transform.position, target.transform.position);

                        if (distance <= 1f)
                        {

                            Vector3 directionToTarget = (target.transform.position - TargetCard.transform.position).normalized;

                            float dotProductForward = Vector3.Dot(TargetCard.transform.up, directionToTarget);
                            float dotProductRight = Vector3.Dot(TargetCard.transform.right, directionToTarget);
                            float dotProductLeft = Vector3.Dot(-TargetCard.transform.right, directionToTarget);
                            float dotProductBackward = Vector3.Dot(-TargetCard.transform.up, directionToTarget);


                            if (dotProductForward > 0.5f || dotProductRight > 0.5f || dotProductLeft > 0.5f || dotProductBackward > 0.5f)
                            {

                                CardInformation TargetInfo = target.GetComponent<CardInformation>();

                                int cardindex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, attackercard.transform.parent.gameObject);

                                attackercard.GetComponent<CardInformation>().CardHealth = (int.Parse(attackercard.GetComponent<CardInformation>().CardHealth) - TargetInfo.CardDamage).ToString();

                                RefreshMyCard(cardindex,
                                attackercard.GetComponent<CardInformation>().CardHealth,
                                attackercard.GetComponent<CardInformation>().HaveShield,
                                attackercard.GetComponent<CardInformation>().CardDamage,
                                attackercard.GetComponent<CardInformation>().DivineSelected,
                                attackercard.GetComponent<CardInformation>().FirstTakeDamage,
                                attackercard.GetComponent<CardInformation>().FirstDamageTaken,
                                attackercard.GetComponent<CardInformation>().EternalShield);

                                if (int.Parse(attackercard.GetComponent<CardInformation>().CardHealth) <= 0)
                                {
                                    Debug.Log("Target card " + attackercard.GetComponent<CardInformation>().CardName + " is dead");
                                    DeleteMyCard(cardindex);
                                    break;
                                }

                                Debug.Log(TargetInfo.CardName + " " + attackercard.GetComponent<CardInformation>().CardHealth);
                            }
                        }
                    }
                }

                obj.GetComponent<PlayerController>().NomadicTactics = false;
            }
        }
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


    public GameObject CreateSpecialCard(string name, string health, int damage, int mana, int index, bool front)
    {
        if(PV.IsMine)
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
            CardCurrent.GetComponent<CardInformation>().CardName = name;
            CardCurrent.GetComponent<CardInformation>().CardHealth = (int.Parse(health) + addedValue).ToString(); ;
            CardCurrent.GetComponent<CardInformation>().CardDamage = damage + addedValue;
            CardCurrent.GetComponent<CardInformation>().CardMana = mana;
            CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
            CardCurrent.GetComponent<CardInformation>().SetInformation();

            StartCoroutine(MoveAndRotateCard(CardCurrent, CardCurrent.transform.position, 0.3f));

            CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateSpecialCard", RpcTarget.All, name, CardCurrent.GetComponent<CardInformation>().CardHealth, CardCurrent.GetComponent<CardInformation>().CardDamage, mana, index,front);
            return CardCurrent;
        }
        return null;
    }

    [PunRPC]
    void RPC_CreateSpecialCard(string name,  string health, int damage, int mana, int index,bool front)
    {
        if (!PV.IsMine)
            return;
        GameObject CardCurrent;
        if (front)
        {
            CardCurrent = Instantiate(CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[index].transform);
            CardCurrent.tag = "CompetitorCard";
        }
        else
        {
            CardCurrent = Instantiate(CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[index].transform);
            CardCurrent.tag = "UsedCard";
        }

         
        CardCurrent.transform.localPosition = Vector3.zero;
        CardCurrent.GetComponent<CardInformation>().CardName = name;
        CardCurrent.GetComponent<CardInformation>().CardHealth = health;
        CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
        CardCurrent.GetComponent<CardInformation>().CardMana = mana;
        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
        CardCurrent.GetComponent<CardInformation>().SetInformation();

        StartCoroutine(MoveAndRotateCard(CardCurrent, CardCurrent.transform.position, 0.3f));

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
                        CardCurrent.GetComponent<CardInformation>().CardDes = zeusCard.minions[targetIndex].description;
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
                        CardCurrent.GetComponent<CardInformation>().CardDes = zeusCard.spells[targetIndex].description;
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
                          CardCurrent.GetComponent<CardInformation>().CardDes = genghisCard.minions[GenghistargetIndex].description;
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
                          CardCurrent.GetComponent<CardInformation>().CardDes = genghisCard.spells[GenghistargetIndex].description;
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
                        CardCurrent.GetComponent<CardInformation>().CardDes = odinCard.minions[OdintargetIndex].description;
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
                        CardCurrent.GetComponent<CardInformation>().CardDes = odinCard.spells[OdintargetIndex].description;
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = odinCard.spells[OdintargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                if(CardCurrent.GetComponent<CardInformation>().CardName == "Naglfar")
                {
                    if(DeadCardCount >= 6)
                    {
                        CardCurrent.GetComponent<CardInformation>().CardMana -= 3;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                    }
                }
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
                        CardCurrent.GetComponent<CardInformation>().CardDes = anubisCard.minions[AnubistargetIndex].description;
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
                        CardCurrent.GetComponent<CardInformation>().CardDes = anubisCard.spells[AnubistargetIndex].description;
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
                        CardCurrent.GetComponent<CardInformation>().CardDes = leonardoCard.minions[LeonardotargetIndex].description;
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
                        CardCurrent.GetComponent<CardInformation>().CardDes = leonardoCard.spells[LeonardotargetIndex].description;
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
                        CardCurrent.GetComponent<CardInformation>().CardDes = dustinCard.minions[DustinTargetIndex].description;
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
                        CardCurrent.GetComponent<CardInformation>().CardDes = dustinCard.spells[DustinTargetIndex].description;
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

    void CreateStandartCards(GameObject CardCurrent ,string name)
    {
        StandartCards standartCard = new StandartCards();

        int StandartCardTargetIndex = -1;

        for (int i = 0; i < standartCard.standartcards.Count; i++)
        {
            if (standartCard.standartcards[i].name == name)
            {
                StandartCardTargetIndex = i;

                CardCurrent.GetComponent<CardInformation>().CardName = standartCard.standartcards[StandartCardTargetIndex].name;
                CardCurrent.GetComponent<CardInformation>().CardDes = standartCard.standartcards[StandartCardTargetIndex].description;
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

    public void DraugrWarrior()
    {
        if (OwnMainCard == "Odin")
        {
            GameObject deckObject = GameObject.Find("Deck");
            if (deckObject != null)
            {
                for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++)
                {
                    CardInformation cardInfo = deckObject.transform.GetChild(i).GetComponent<CardInformation>();
                    if (cardInfo != null && cardInfo.CardName == "Draugr Warrior")
                    {
                        cardInfo.CardMana = 5 - GameObject.Find("CompetitorDeck").transform.childCount;
                        if (cardInfo.CardMana < 0)
                        {
                            cardInfo.CardMana = 0;
                        }
                        cardInfo.SetInformation();
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                Debug.LogWarning("Deck object not found.");
            }
        }
        
    }

    public void GjallarhornCall()
    {
        if (OwnMainCard == "Odin")
        {
            GameObject deckObject = GameObject.Find("Deck");
            if (deckObject != null)
            {
                for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++)
                {
                    CardInformation cardInfo = deckObject.transform.GetChild(i).GetComponent<CardInformation>();
                    if (cardInfo != null && cardInfo.CardName == "Gjallarhorn Call")
                    {
                        Debug.Log("Gjallarhorn Call found in your Deck.");
                        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("UsedCard");
                        bool heimdallrfind = false;
                        foreach (var card in AllOwnCards)
                        {
                            if(card.GetComponent<CardInformation>().CardName == "Heimdallr")
                            {
                                heimdallrfind = true;
                            }
                        }
                        if(heimdallrfind)
                        {
                            cardInfo.CardMana = 0;
                        }
                        else
                            cardInfo.CardMana = 6;
                        cardInfo.SetInformation();
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                Debug.LogWarning("Deck object not found.");
            }
        }
    }



    public void StackDeck()
    {
        DraugrWarrior();
        GjallarhornCall();
        
        Transform deckTransform = GameObject.Find("Deck").transform;
        int cardCount = deckTransform.childCount;

        if (cardCount == 0)
        {
            return;
        }

        if (cardCount <= 1)
        {
            return;
        }

        float baseStartX = -0.5f; 
        float baseEndX = 0.5f;   
        float maxExpansion = 0.95f; 

        // Dinamik X sınırlarını hesapla
        float expansionFactor = Mathf.Clamp((float)cardCount / 10f, 0f, 1f);
        float startX = baseStartX - maxExpansion * expansionFactor; 
        float endX = baseEndX + maxExpansion * expansionFactor;    

        float startYZ = -0.1f;   
        float peakYZ = 0.10f;  

        if (cardCount % 2 == 1)
        {
            peakYZ *= 0.85f; 
        }

        float startZRotation = 200f;
        float endZRotation = 160f;  

        for (int i = 0; i < cardCount; i++)
        {
            float lerpFactor = (float)i / (cardCount - 1);
            float xPosition = Mathf.Lerp(startX, endX, lerpFactor);

            float yzPosition;
            float normalizedIndex = Mathf.Abs((float)(i - (cardCount - 1) / 2f)) / ((cardCount - 1) / 2f);
            yzPosition = Mathf.Lerp(peakYZ, startYZ, normalizedIndex);

            float incrementalY = i * 0.01f;  
            float incrementalZ = i * 0.01f;  

            float yPosition = yzPosition + incrementalY;
            float zPosition = yzPosition - incrementalZ;

            float zRotation = Mathf.Lerp(startZRotation, endZRotation, lerpFactor);

            Transform cardTransform = deckTransform.GetChild(i);
            cardTransform.localPosition = new Vector3(xPosition, yPosition, zPosition); 
            cardTransform.localRotation = Quaternion.Euler(45, 0, zRotation);
            deckTransform.GetChild(i).tag = "Card";
        }
    }


    public void StackCompetitorDeck()
    {

        for (int i = 0; i < GameObject.Find("CompetitorDeck").transform.childCount; i++)
        {
            float xPos = i * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz

            GameObject.Find("CompetitorDeck").transform.GetChild(i).transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
        }

        GameObject.Find("CompetitorDeck").transform.position = new Vector3(3.3f - GameObject.Find("CompetitorDeck").transform.childCount * 0.2f, 1.6f, 1.54f);

    }

    public void DeactivateAllGreens()
    {
        Transform deckTransform = GameObject.Find("Deck").transform;
        int childCount = deckTransform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform green = deckTransform.GetChild(i).Find("Green");
            if (green != null)
                green.gameObject.SetActive(false);
        }
    }

    public void ActiveAllGreens()
    {
        Transform deckTransform = GameObject.Find("Deck").transform;
        int childCount = deckTransform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = deckTransform.GetChild(i);
            CardInformation cardInfo = child.GetComponent<CardInformation>();
            if (cardInfo != null && cardInfo.CardMana <=Mana)
            {
                Transform green = child.Find("Green");
                if (green != null)
                {
                    green.gameObject.SetActive(true);
                }
            }
        }
    }


    void GetDataForUI()
    {
        if (OwnName == "" || OwnDeck == null || CompetitorName == "" || CompetitorDeck == null)
        {
            print("Data is null");
            if (PV.IsMine)
            {
                print("Im Mine.....");
                if (PV.Owner.IsMasterClient)
                {
                    print("MasterClient.....");
                    OwnName = _GameManager.MasterPlayerName;
                    OwnDeck = _GameManager.MasterDeck;
                    OwnMainCard = _GameManager.MasterMainCard;
                    OwnHeroAttackDamage = _GameManager.MasterAttackDamage;

                   

                    OwnNameText.text = OwnName;
                    OwnMainCardText.text = OwnMainCard;
                    OwnHeroAttackDamageText.text = OwnHeroAttackDamage.ToString();
                    
                    OwnHealth = _GameManager.MasterHealth; //new
                    OwnHealthText.text = OwnHealth.ToString(); // new 

                   

                    for (int i = 0; i < OwnDeck.Length; i++)
                    {
                        OwnDeckText.text += ", " + OwnDeck[i];
                    }
                   
                    CompetitorName = _GameManager.OtherPlayerName;
                    CompetitorDeck = _GameManager.OtherDeck;
                    CompetitorMainCard = _GameManager.OtherrMainCard;
                    CompetitorHeroAttackDamage = _GameManager.OtherAttackDamage;

                    CompetitorHealth = _GameManager.OtherHealth; // new 
                    CompetitorHealthText.text = CompetitorHealth.ToString(); //new


                    CompetitorNameText.text = CompetitorName;
                    CompetitorMainCardText.text = CompetitorMainCard;
                    CompetitorHeroAttackDamageText.text = CompetitorHeroAttackDamage.ToString();
                    if (CompetitorMainCard!=null)
                    {
                        CompetitorHeroIcon.sprite = CompetitorMainCard switch
                        {
                            "Zeus" => heroIcons[0],
                            "Odin" => heroIcons[1],
                            "Genghis" => heroIcons[2],
                            "Anubis" => heroIcons[3],
                            "Dustin" => heroIcons[4],
                            "Leonardo Da Vinci" => heroIcons[5],
                            _ => CompetitorHeroIcon.sprite
                        };
                    }
                    

                    for (int i = 0; i < CompetitorDeck.Length; i++)
                    {
                        CompetitorDeckText.text += ", " + CompetitorDeck[i];
                    }

                    

                    WhoseTurnText.text = "Finish Turn";
                    Debug.LogError("Im MasterClient");
                }
                else
                {
                    print("OtherClient.....");
                    OwnName = _GameManager.OtherPlayerName;
                    OwnDeck = _GameManager.OtherDeck;
                    OwnMainCard = _GameManager.OtherrMainCard;
                    OwnHeroAttackDamage = _GameManager.OtherAttackDamage;

                   

                    OwnNameText.text = OwnName;
                    OwnMainCardText.text = OwnMainCard;
                    OwnHeroAttackDamageText.text = OwnHeroAttackDamage.ToString();

                    OwnHealth = _GameManager.OtherHealth; //new
                    OwnHealthText.text = OwnHealth.ToString(); //new



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
                    
                    CompetitorHealth = _GameManager.MasterHealth; //new
                    CompetitorHealthText.text = CompetitorHealth.ToString(); //new
                    
                    if (CompetitorMainCard!=null)
                    {
                        CompetitorHeroIcon.sprite = CompetitorMainCard switch
                        {
                            "Zeus" => heroIcons[0],
                            "Odin" => heroIcons[1],
                            "Genghis" => heroIcons[2],
                            "Anubis" => heroIcons[3],
                            "Dustin" => heroIcons[4],
                            "Leonardo Da Vinci" => heroIcons[5],
                            _ => CompetitorHeroIcon.sprite
                        };
                    }

                    for (int i = 0; i < CompetitorDeck.Length; i++)
                    {
                        CompetitorDeckText.text += ", " + CompetitorDeck[i];
                    }

                    WhoseTurnText.text = "Enemy Turn";
                    finishButton.interactable = false;
                    OnManaChange(Mana);
                    timeTween.Kill(true);
                    countDownImage.fillAmount = 1;
                    whoseTurnImage.sprite = enemyTurnSprite;
                    timeTween=countDownImage.DOFillAmount(0, 60f);
                    Debug.LogError("Im OtherClient");
                }
            }

            Invoke("GetDataForUI", 3.5f);
        }
        else // GAME STARTER WİTH MASTER CLIENT
        {
            print("Data is not null");
            if (PV.IsMine)
            {
                print("Im Mine.....");
                if (PV.Owner.IsMasterClient)
                {
                    print("MasterClient.....");
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

                    OwnHealth = _GameManager.MasterHealth; //new
                    OwnHealthText.text = OwnHealth.ToString(); // new 

                    CompetitorHealth = _GameManager.OtherHealth; // new 
                    CompetitorHealthText.text = CompetitorHealth.ToString(); //new

                


                  

                }
                else
                {
                    print("OtherClient.....");
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject card = Instantiate(Resources.Load<GameObject>("CompetitorCard"), GameObject.Find("CompetitorDeck").transform);
                        card.tag = "CompetitorDeckCard";
                        float xPos = DeckCardCount * 0.8f - 0.8f; // Kartın X konumunu belirliyoruz
                        card.transform.localPosition = new Vector3(xPos, 0, 0); // Kartın pozisyonunu ayarlıyoruz
                        //CreateCard(card);
                        StackDeck();
                        StackCompetitorDeck();
                        DeckCardCount++;

                        MeshRenderer cardRenderer = card.GetComponent<MeshRenderer>();

                        if (cardRenderer != null && CompetitorCardMaterial != null)
                        {
                            // Directly assign the CompetitorCardMaterial to the card's MeshRenderer
                            cardRenderer.material = CompetitorCardMaterial;
                            Debug.Log("Materyal atandı: " + CompetitorCardMaterial.name);
                        }



                    }

                    OwnHealth = _GameManager.OtherHealth; //new
                    OwnHealthText.text = OwnHealth.ToString(); //new

                    CompetitorHealth = _GameManager.MasterHealth; //new
                    CompetitorHealthText.text = CompetitorHealth.ToString(); //new


                    HeroNftCard();
                    GameObject Herocard = Instantiate(Resources.Load<GameObject>("CompetitorHeoCard"), GameObject.Find("CompetitorHeroPivot").transform);
                }

            }
        }
    }
}
