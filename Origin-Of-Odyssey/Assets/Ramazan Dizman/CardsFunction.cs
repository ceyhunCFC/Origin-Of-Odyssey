using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Photon.Pun;

public class CardsFunction : MonoBehaviour
{

    private GameObject firstSelectedCard = null;
    private List<GameObject> enemyCards = new List<GameObject>();
    private List<GameObject> ownCards = new List<GameObject>();

    CardsAreaCreator _cardsAreaCreator;

    public GameObject CardObject;
    PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        _cardsAreaCreator = GameObject.Find("Area").GetComponent<CardsAreaCreator>();
    }

    private void Update()
    {
        if (firstSelectedCard == null)
            return;


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                print(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "CompetitorCard")
                {
                    Debug.Log("Run");
                    StandartAttack(firstSelectedCard, hit.collider.gameObject);
                    //SelectEnemyCard(hit.collider.gameObject,2);
                    //StandartHealth(hit.collider.gameObject, 3);
                    //SelectOwnCard(hit.collider.gameObject, 2);
                    //SelectAll("Enemy", "All");
                    //StandartFreeze(hit.collider.gameObject);
                    //CheckAround(hit.collider.gameObject);
                    //SelectAll("Own", "All");

                    //StandartDamage(hit.collider.gameObject, 3);         //3 damage ve kom?ulara 1 hasar ve dondur winterschill
                    //CheckAround(hit.collider.gameObject);

                    //DropMana(1);             //Vitruvian Firstborn  destedeki kaertlar?n maliyetini 1 azalt


                    //MultipleCreateCard("Hoplite", "2", 2,7);                 //athena ?n cepheyi doldur
                    //MultipleCreateCard("Spirits of Warriors ", "1", 1, 2);                 //Valkyrie's Chosen 2 tane spirits ?a??r

                    
                }
            }

        }
    }

    public void SelectFirstCard(GameObject card)
    {
        firstSelectedCard = card;

        Debug.Log("Select the second card...");
    }

    private void MultipleAttack(GameObject attacker)
    {
        foreach (GameObject enemyCard in enemyCards)
        {
            StandartAttack(attacker, enemyCard);
        }

        enemyCards.Clear();
    }
    private void StandartAttack(GameObject attacker, GameObject target)
    {
        CardInformation attackerInfo = attacker.GetComponent<CardInformation>();
        CardInformation targetInfo = target.GetComponent<CardInformation>();
        
        int damageDealt = attackerInfo.CardDamage;
        int targetHealth = int.Parse(targetInfo.CardHealth);

        if(targetInfo.CardName== "Nemean Lion")
        {
            damageDealt = 1;                                                                                       //sald?r?lan kart nemean lion ise 1 hasar ver
        }
        else if(targetInfo.CardName== "Mirror Shield Automaton")
        {
            int attackerHealth = int.Parse(attackerInfo.CardHealth);
            attackerHealth -= 1;                                                                                   // Mirror Shield Automaton'a sald?ran?n can?n? 1 azalt
            attackerInfo.CardHealth = attackerHealth.ToString();
        }
        else if(targetInfo.CardName== "Spartan Hoplite")
        {
            targetInfo.CardDamage += 1;                                                                            //Spartan Hoplitea sald?r?l?rsa spartan hoplite 1 sald?r? g?c? al?r
        }
        else if(targetInfo.CardName== "Wasteland Giant")
        {
            SelectAll("Own", "All");
        }
        else if(targetInfo.CardName== "Minotaur Labyrinth Keeper")
        {
            targetInfo.CardDamage += 1;                                                                             //Minotaur Labyrinth Keeper hasar al?rsa damagi 1 arts?n
        }
        
        targetHealth -= damageDealt;
        targetInfo.CardHealth = targetHealth.ToString();

        if (attackerInfo.CardName == "Automaton Duelist")
        {
            attacker.GetComponent<CardInformation>().CardDamage += 1;                                              //Automaton Duelist sald?rd?ktan sonra sald?r? de?eri 1 artar
        }
        else if (attackerInfo.CardName == "Knight Errant")
        {
            int attackerHealth = int.Parse(attackerInfo.CardHealth);                                              //Knight Errant sald?rd?ktan sonra can de?eri 1 artar
            attackerHealth += 1;
            attackerInfo.CardHealth= attackerHealth.ToString();
        }
        else if(attackerInfo.CardName== "Minor Glacial Elemental")
        {
            targetInfo.CardFreeze = true;                                                                        //Minor Glacial Elemental sald?rd??? d??man donar
        }

        if (targetHealth <= 0)
        {
            if(attackerInfo.CardName == "Viking Raider")
            {
                //viking raider adam ?ld?r?rse mana 1 art?r
            }
            Destroy(target);

         

        }

        firstSelectedCard = null;
    }

   

    private void StandartDamage(GameObject target, int damage)
    {
        CardInformation targetInfo = target.GetComponent<CardInformation>();

        int targetHealth = int.Parse(targetInfo.CardHealth);
        targetHealth -= damage;

        targetInfo.CardHealth = targetHealth.ToString();

        firstSelectedCard = null;

    }

    private void DropDamage(GameObject target, int damageloss) 
    {
        CardInformation targetInfo = target.GetComponent<CardInformation>();

        targetInfo.CardDamage-= damageloss;

        firstSelectedCard = null;
    }

    private void PlusDamage(GameObject target, int damageplus)
    {
        CardInformation targetInfo = target.GetComponent<CardInformation>();

        targetInfo.CardDamage += damageplus;

        firstSelectedCard = null;
    }

    private void DropMana(int mana)
    {
        GameObject deckObject = GameObject.Find("Deck");

        CardInformation[] deckCards = deckObject.GetComponentsInChildren<CardInformation>();

        foreach (CardInformation cardInfo in deckCards)
        {
            if(cardInfo.CardMana > 0)
            {
                cardInfo.CardMana -= mana;    
                Debug.Log("D??t?");
            }
            else
            {
                Debug.Log("0");
            }
        }

        firstSelectedCard = null;
    }


    private void MultipleHealth(int health)
    {
        foreach (GameObject ownCard in ownCards)
        {
            StandartHealth(ownCard,health);
        }

        ownCards.Clear();
    }
    private void StandartHealth(GameObject target,int health)
    {
        CardInformation targetInfo = target.GetComponent<CardInformation>();

        int targetHealth = int.Parse(targetInfo.CardHealth);

        targetHealth += health;
        targetInfo.CardHealth = targetHealth.ToString();

        if(targetInfo.CardName== "Jade Monk")
        {
            target.GetComponent<CardInformation>().CardDamage += 1;                                   //      Jade Monk her iyile?mede +1 sald?r? kazan?r
        }

        Debug.Log( " healed " + targetInfo.CardName + " for " + health + " value!");

        firstSelectedCard = null;
    }


    private void StandartFreeze(GameObject target)
    {
        CardInformation targetInfo = target.GetComponent<CardInformation>();

        targetInfo.CardFreeze = true;

        Debug.Log(" freezed " + targetInfo.CardName );

        firstSelectedCard = null;
    }

    private void SelectOwnCard(GameObject card,int owncount)
    {
        ownCards.Add(card);
        if (ownCards.Count == owncount)
        {
            MultipleHealth(3);
        }
    }
    private void SelectEnemyCard(GameObject card,int enemycount)
    {
        enemyCards.Add(card);
        if (enemyCards.Count==enemycount)
        {
            MultipleAttack(firstSelectedCard);
        }

    }

    private void CheckAround(GameObject target)
    {
        Collider[] childColliders = target.GetComponentsInChildren<Collider>();

        foreach (Collider childCollider in childColliders)
        {
            if (childCollider.gameObject.name != "Card_Prefab(Clone)")
            {
                Bounds bounds = childCollider.bounds;
                Vector3 center = bounds.center;
                Vector3 extents = bounds.extents;

                Collider[] overlappingColliders = new Collider[10];

                int numCollisions = Physics.OverlapBoxNonAlloc(center, extents, overlappingColliders);

                for (int i = 0; i < numCollisions; i++)
                {
                    if (overlappingColliders[i].gameObject.name == "Card_Prefab(Clone)")
                    {
                        Debug.Log(childCollider + " ile ?arp??an obje: " + overlappingColliders[i].gameObject.name);
                        //StandartAttack(target, overlappingColliders[i].gameObject);    

                        //StandartDamage(overlappingColliders[i].gameObject, 1);               //kom?ulara 1 hasar winterschill
                        //StandartFreeze(overlappingColliders[i].gameObject);

                        if (target.GetComponent<CardInformation>().CardName == "Pyramid's Might")
                        {
                            CardInformation targetInfo = target.GetComponent<CardInformation>();

                            int targetHealth = int.Parse(targetInfo.CardHealth);

                            targetHealth += 1;                                                                         //  Pyramid's Might etraf?ndakilerin can?na ve damagine +1 veriyor
                            targetInfo.CardHealth = targetHealth.ToString();

                            targetInfo.CardDamage += 1;
                        }
                    }
                }
            }
        }
        firstSelectedCard = null;
    }

    private void SelectRandom(List<GameObject> cards)
    {
        int randomIndex = Random.Range(0, cards.Count);

        // Se?ilen rastgele kart
        GameObject selectedCard = cards[randomIndex];

        Debug.Log("Rastgele se?ilen kart: " + selectedCard.GetComponent<CardInformation>().CardName);
        //PlusDamage(selectedCard, 1);

        //PlusDamage(selectedCard, 2);                                                 //her b?y?den sonra rastgele bir dosta 2 can 2 hasar ver  Anatomist of the Unknown 
        //StandartHealth(selectedCard, 2);

        if (firstSelectedCard.GetComponent<CardInformation>().CardName== "Falcon-Eyed Hunter")
        {
            //StandartDamage(selectedCard, 3);                         //Falcon-Eyed Hunter ise arkadaki rastgele bir birime 3 hasar ver
        }

        cards.Clear();
        firstSelectedCard = null;
    }

    private void MultipleCreateCard(string name, string health, int damage, int numberOfCards)
    {
        List<int> emptySlots = new List<int>();

        for (int i = 7; i < 14; i++)
        {
            Transform targetArea = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].transform;

            if (targetArea.childCount == 0 || (targetArea.childCount > 0 && !targetArea.GetChild(0).CompareTag("UsedCard")))
            {
                emptySlots.Add(i);
            }
        }

        int cardsToCreate = Mathf.Min(numberOfCards, emptySlots.Count);

        if (cardsToCreate > 0)
        {
            System.Random rand = new System.Random();
            emptySlots = emptySlots.OrderBy(x => rand.Next()).ToList();

            for (int j = 0; j < cardsToCreate; j++)
            {
                CreateUsedCard(emptySlots[j], name, "", health, damage, 0);
            }

            Debug.Log(cardsToCreate + " kart rastgele bo? yerlere olu?turuldu.");
        }
        else
        {
            Debug.Log("Hi? bo? yer yok.");
        }
    }



    public void CreateUsedCard(int boxindex, string name, string des, string heatlh, int damage, int mana)
    {

        GameObject CardCurrent = Instantiate(CardObject, GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[boxindex].transform);
        CardCurrent.transform.localScale = Vector3.one;
        CardCurrent.transform.eulerAngles = new Vector3(90, 0, 180);

        CardCurrent.tag = "UsedCard";

        CardCurrent.GetComponent<CardInformation>().CardName = name;
        CardCurrent.GetComponent<CardInformation>().CardDes = des;
        CardCurrent.GetComponent<CardInformation>().CardHealth = heatlh;
        CardCurrent.GetComponent<CardInformation>().CardDamage = damage;
        CardCurrent.GetComponent<CardInformation>().CardMana = mana;
        CardCurrent.GetComponent<CardInformation>().SetInformation();

        firstSelectedCard = null;
    }

    private void DistributeHealthRandomly(List<GameObject> cards, int totalHealth)
    {
        if (cards.Count == 0)
        {
            Debug.Log("Hi? kart yok.");
            return;
        }

        System.Random rand = new System.Random();
        int[] healthDistribution = new int[cards.Count];

        for (int i = 0; i < totalHealth; i++)
        {
            healthDistribution[rand.Next(cards.Count)]++;
        }

        for (int i = 0; i < cards.Count; i++)
        {
            StandartHealth(cards[i], healthDistribution[i]);
        }
        cards.Clear();
    }

    private void SelectAll(string whos, string line)
    {
        int howMuch = 0;
        switch (whos)
        {
            case "Own":
                if (line == "Backline")
                {
                    for (int i = 0; i < 7; i++)
                    {
                        GameObject areaCollision = _cardsAreaCreator.FrontAreaCollisions[i];
                        int childCount = areaCollision.transform.childCount;
                        if (childCount > 0)
                        {
                            howMuch++;
                        }
                        else
                        {
                            
                        }
                    }
                }
                else if (line == "Frontline")
                {
                    for (int i = 7; i < 14; i++)
                    {
                        GameObject areaCollision = _cardsAreaCreator.FrontAreaCollisions[i];
                        int childCount = areaCollision.transform.childCount;
                        if (childCount > 0)
                        {
                            howMuch++;
                        }
                        else
                        {
                            
                        }
                    }
                }
                else if (line == "All")
                {
                    List<GameObject> ownCards = new List<GameObject>();
                    foreach (GameObject areaCollision in _cardsAreaCreator.FrontAreaCollisions)
                    {
                        int childCount = areaCollision.transform.childCount;
                        if (childCount > 0)
                        {
                            howMuch++;
                            ownCards.Add(areaCollision.transform.GetChild(0).gameObject);
                            //PlusDamage(areaCollision.transform.GetChild(0).gameObject, 2);   //minyonlara 2 art? hasar verir Mongol Fury
                            //StandartHealth(areaCollision.transform.GetChild(0).gameObject, 3); //Wandering Healer t?m dostlara 3 can ver
                            //ownCards.Add(areaCollision.transform.GetChild(0).gameObject);
                            //StandartDamage(areaCollision.transform.GetChild(0).gameObject, 3);  //Wasteland Giant hasar ald???nda herkese 3 hasar ver
                            /*if (areaCollision.transform.GetChild(0).gameObject.GetComponent<CardInformation>().CardName== "Automaton Apprentice")
                            {
                                CardInformation targetInfo = areaCollision.transform.GetChild(0).gameObject.GetComponent<CardInformation>();

                                int targetHealth = int.Parse(targetInfo.CardHealth);                                                                         //b?y?den sonra     Automaton Apprentice karakteri varsa  +! can als?n

                                targetHealth += 1;
                                targetInfo.CardHealth = targetHealth.ToString();
                            }*/


                        }
                        else
                        {
                            
                        }
                    }
                    //SelectRandom(ownCards);
                    //DistributeHealthRandomly(ownCards, 10);                      //River's Blessing rastgele 10 can ver 
                }
                break;
            case "Enemy":
                if (line == "Backline")
                {
                    for (int i = 0; i < 7; i++)
                    {
                        GameObject areaCollision = _cardsAreaCreator.BackAreaCollisions[i];
                        int childCount = areaCollision.transform.childCount;
                        if (childCount > 0)
                        {
                            howMuch++;
                            //enemyCards.Add(areaCollision.transform.GetChild(0).gameObject);
                        }
                        else
                        {
                            
                        }
                    }
                    //SelectRandom(enemyCards);
                }
                else if (line == "Frontline")
                {
                    for (int i = 7; i < 14; i++)
                    {
                        GameObject areaCollision = _cardsAreaCreator.BackAreaCollisions[i];
                        int childCount = areaCollision.transform.childCount;
                        if (childCount > 0)
                        {
                            howMuch++;
                            //DropDamage(areaCollision.transform.GetChild(0).gameObject, 2);      //?n hat sald?r? 2 eksilt Mutant Behemoth 
                        }
                        else
                        {
                            
                        }
                    }
                    //how much kadar can ekle ana karaktere
                }
                else if (line == "All")
                {
                    List<GameObject> _enemyCards = new List<GameObject>();
                    foreach (GameObject areaCollision in _cardsAreaCreator.BackAreaCollisions)
                    {
                        int childCount = areaCollision.transform.childCount;
                        if (childCount > 0)
                        {
                            howMuch++;
                            _enemyCards.Add(areaCollision.transform.GetChild(0).gameObject);
                            StandartFreeze(areaCollision.transform.GetChild(0).gameObject);       // t?m d??manlar? dondur gorgon
                            //StandartDamage(areaCollision.transform.GetChild(0).gameObject,2);     //t?m d??manlara 2 hasar ver chimera  ve Lightning Storm ve Wasteland Sniper 
                            //StandartDamage(areaCollision.transform.GetChild(0).gameObject,1);     //Pyromaniac Wizard t?m d??manlara 1 hasar ver
                        }
                        else
                        {
                            
                        }
                    }
                    //DropMana(howMuch);
                    /*if (_enemyCards.Count > 1)
                    {
                        int randomIndex1 = Random.Range(0, _enemyCards.Count);
                        int randomIndex2 = Random.Range(0, _enemyCards.Count);
                    
                        while (randomIndex1 == randomIndex2)
                        {
                            randomIndex2 = Random.Range(0, _enemyCards.Count); // ?kinci indexi tekrar se?
                        }
                    
                        // ?ki farkl? kart? se?ip her birine hasar ver                                                                       //Urban Ranger rastgele 2 d??man 2 hasar ver
                        StandartDamage(_enemyCards[randomIndex1], 2);
                        StandartDamage(_enemyCards[randomIndex2], 2);
                    
                        Debug.Log("Rastgele 2 farkl? d??man kart?na 2'?er hasar verildi.");
                    }
                    else if (_enemyCards.Count == 1)
                    {
                        StandartDamage(_enemyCards[0], 2);
                        Debug.Log("Sadece bir d??man kart? oldu?u i?in ona 2 hasar verildi.");
                    }
                    else
                    {
                        Debug.Log("Yeterli d??man kart? yok.");
                    }*/
                }
                break;
            default:
                Debug.LogError("Invalid selection: " + whos);
                break;
        }
        firstSelectedCard = null;
    }



}
