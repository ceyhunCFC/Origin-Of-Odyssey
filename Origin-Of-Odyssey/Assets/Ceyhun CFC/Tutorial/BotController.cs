using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BotController : MonoBehaviour
{
    TutorialPlayerController _TutorialPlayerController;
    TutorialCardProgress _TutorialCardProgress;
    private bool specialAttackUsed = false;
   
   
    void Start()
    {
        _TutorialPlayerController = GetComponent<TutorialPlayerController>();

        for (int i = 0; i < 3; i++)
        {
            _TutorialPlayerController.CreateAnCompetitorCard();
        }

        Instantiate(Resources.Load<GameObject>("TutorialCompetitorHeroCard 1"), GameObject.Find("CompetitorHeroPivot").transform);

    }

    public void BotAttack()
    {
       

        if (!specialAttackUsed && UnityEngine.Random.Range(0, 5) == 0) // 1/5 þansla özel saldýrý artýrýlabilir, artmalý ilk raundlarda saldýrýyor
        {
            SpecialAttack();
            specialAttackUsed = true;
        }
        else
        {
            
            NormalAttack();
        }

        _TutorialPlayerController.CreateAnCompetitorCard();
        _TutorialPlayerController.WhoseTurnText.text = "Enemy Turn";

        StartCoroutine(Waiter());
    }

    private void SpecialAttack()
    {
        Debug.Log("Bot özel saldýrý yapýyor!");
        GameObject[] usedCards = GameObject.FindGameObjectsWithTag("UsedCard");

        if (usedCards.Length > 0 && UnityEngine.Random.Range(0, 2) == 0)
        {
           

            GameObject selectedUsedCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];
            CardInformation cardInfo = selectedUsedCard.GetComponent<CardInformation>();
            
            cardInfo.CardHealth = (int.Parse(cardInfo.CardHealth) - _TutorialPlayerController.CompetitorHeroAttackDamage).ToString();
            cardInfo.SetInformation();

            Debug.Log("Bot Özel Saldýrýsýný Seçilen Karta Yaptý");
            if (int.Parse(cardInfo.CardHealth) <= 0)
            {
                Destroy(selectedUsedCard);
            }
        }
        
        else
        {
            Debug.Log("Bot Özel Saldýrýsýný Oyuncuya Yaptý.");
            _TutorialPlayerController.OwnHealth -= _TutorialPlayerController.CompetitorHeroAttackDamage;
            _TutorialPlayerController.RefreshUI();
        }
        
    }

    private void NormalAttack()
    {
        GameObject[] usedCards = GameObject.FindGameObjectsWithTag("UsedCard");
        GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");

        if (usedCards.Length > 0 && competitorCards.Length > 0)
        {

            GameObject selectedCompetitorCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
            CardInformation competitorCardInfo = selectedCompetitorCard.GetComponent<CardInformation>();

            
            // 8-14 arasý kart kontrolü
            bool isCardInRange = CheckCardInRange(usedCards, 8, 14);

            GameObject selectedUsedCard = null;
            if (isCardInRange)
            {

                selectedUsedCard = GetRandomCardInRange(usedCards, 8, 14);
            }
            else
            {

                selectedUsedCard = GetRandomCardInRange(usedCards, 1, 7);
            }

            if (selectedUsedCard != null)
            {
                CardInformation usedCardInfo = selectedUsedCard.GetComponent<CardInformation>();


                if (competitorCardInfo.CardName == "Heracles")
                {
                    Debug.Log("Heracles " + usedCardInfo.CardName + " kartýna saldýrý yaptý");
                    int deadMonsterCount = _TutorialPlayerController.DeadMonsterCound;

                    competitorCardInfo.CardHealth = (int.Parse(competitorCardInfo.CardHealth) + (2 * deadMonsterCount)).ToString();
                    competitorCardInfo.CardDamage += (2 * deadMonsterCount);
                    competitorCardInfo.SetInformation();

                    usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                    usedCardInfo.SetInformation();

                    if (int.Parse(usedCardInfo.CardHealth) <= 0)
                    {
                        Destroy(selectedUsedCard);
                    }
                }


                else if (competitorCardInfo.CardName == "Stormcaller")
                {
                    Debug.Log("Stormcaller " + usedCardInfo.CardName + " kartýna saldýrý yaptý");
                    int spellsExtraDamage = 0;

                    // Rakibin elindeki kartlarý kontrol et
                    GameObject[] competitorDeck = GameObject.FindGameObjectsWithTag("CompetitorDeckCard");
                    foreach (GameObject card in competitorDeck)
                    {
                        CardInformation cardInfo = card.GetComponent<CardInformation>();
                        if (string.IsNullOrEmpty(cardInfo.CardHealth))  // Büyü kartý kontrolü
                        {
                            spellsExtraDamage = 1;
                        }
                    }
                    competitorCardInfo.CardDamage += spellsExtraDamage;
                    competitorCardInfo.SetInformation();

                    usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                    usedCardInfo.SetInformation();

                    if (int.Parse(usedCardInfo.CardHealth) <= 0)
                    {
                        Destroy(selectedUsedCard);
                    }
                }


                else if (competitorCardInfo.CardName == "Odyssean Navigator") // botun elindeki kartlara sadece gösteriþ olarak ekleniyor.
                {

                    bool isMinion = UnityEngine.Random.Range(0f, 1f) < 0.5f;
                    if (isMinion)
                    {
                        _TutorialPlayerController.CreateAnCompetitorCard();
                        Debug.LogError("ODYYYYYSEAAANN MÝNNYOONNNN YARATTTI ");
                        if (competitorCardInfo.HasAttacked)
                        {
                            Debug.LogError("Odyssean Navigator already attacked, performing normal attack.");

                            usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                            usedCardInfo.SetInformation();

                            if (int.Parse(usedCardInfo.CardHealth) <= 0)
                            {
                                Destroy(selectedUsedCard);
                            }
                        }
                        else
                        {

                            Debug.LogError("Odyssean Navigator has not attacked yet. Skipping attack.");
                            selectedCompetitorCard = null;
                            selectedUsedCard = null;
                            return;
                        }
                    }
                    else
                    {
                        _TutorialPlayerController.CreateAnCompetitorCard();
                        Debug.LogError("ODYYYYYSEAAANN SPEEEELLL YARATTTI ");
                        if (competitorCardInfo.HasAttacked)
                        {

                            Debug.LogError("Odyssean Navigator already attacked, performing normal attack.");

                            usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                            usedCardInfo.SetInformation();

                            if (int.Parse(usedCardInfo.CardHealth) <= 0)
                            {
                                Destroy(selectedUsedCard);
                            }
                        }
                        else
                        {

                            Debug.LogError("Odyssean Navigator has not attacked yet. Skipping attack.");
                            selectedCompetitorCard = null;
                            selectedUsedCard = null;
                            return;
                        }
                    }

                }


                else if (competitorCardInfo.CardName == "Oracle's Emissary")
                {
                    bool isSpell = UnityEngine.Random.Range(0f, 1f) < 0.5f;// destede büyü olma olasýlýðý
                    if (isSpell)
                    {
                        _TutorialPlayerController.CreateAnCompetitorCard();
                        int randomValue = UnityEngine.Random.Range(1, 11); // 1 ve 10 mana deðeri tahmin etme
                        Debug.Log("Oluþturulan Spell Kartýn mana deðeri :" + randomValue);
                        if (randomValue >= 4)
                        {
                            _TutorialPlayerController.CompetitorHealth += 4; // OwnHealth deðerine +4 can ekle
                            _TutorialPlayerController.RefreshUI();
                            Debug.Log("Oracle's Emissary in Ürettði Spell KArýn Mana Deðeri 4 ten büyük olduðý için OwnHealt Deðeri Artýrýldý.");
                        }
                    }
                }



                else if (competitorCardInfo.CardName == "Lightning Forger")
                {
                    Debug.Log("Lightning Forger masaya eklendi, OwnHeroAttackDamage artýrýldý.");
                    _TutorialPlayerController.CompetitorHeroAttackDamage += 3;
                    competitorCardInfo.SetInformation();
                }


                else if (competitorCardInfo.CardName == "Lightning Bolt")
                {

                    int damage = 1;

                    usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - damage).ToString();
                    usedCardInfo.SetInformation();

                    if (int.Parse(usedCardInfo.CardHealth) <= 0)
                    {
                        Destroy(selectedUsedCard);
                    }

                    Debug.Log("Lightning Bolt has attacked " + usedCardInfo.CardName);
                }


                else if (competitorCardInfo.CardName == "Gorgon")
                {
                    GameObject[] allTargets = GameObject.FindGameObjectsWithTag("UsedCard");

                    foreach (var card in allTargets)
                    {
                        if (card.GetComponent<CardInformation>().CardHealth != "")
                        {
                            int cardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, card.transform.parent.gameObject);
                            card.GetComponent<CardInformation>().CardFreeze = true;
                            Debug.Log("Gorgon Kartlarýný Dondurdu");
                        }
                    }
                }


                else if (competitorCardInfo.CardName == "Chimera")
                {
                    GameObject[] allTargets = GameObject.FindGameObjectsWithTag("UsedCard");

                    foreach (var card in allTargets)
                    {
                        CardInformation cardInfo = card.GetComponent<CardInformation>();

                        if (!string.IsNullOrEmpty(cardInfo.CardHealth))
                        {
                            cardInfo.CardHealth = (int.Parse(cardInfo.CardHealth) - 2).ToString();
                            cardInfo.SetInformation();
                            if (int.Parse(cardInfo.CardHealth) <= 0)
                            {
                                Destroy(card);
                                Debug.Log("Chimera " + cardInfo.CardName + " kartýný yok etti");
                            }
                        }
                    }

                    Debug.Log("Chimera tüm rakip kartlara 2 hasar verdi.");

                    // Chimera tekrar kullanmamak için yok edildi.
                    Destroy(selectedCompetitorCard);
                    Debug.Log("Chimera kartý yok edildi.");
                }


                else if (competitorCardInfo.CardName == "Athena")
                {
                    for (int i = 7; i < 14; i++)
                    {

                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            BotCreateHoplitesCard(i);
                        }
                        Debug.Log("Athena, Hoplite Kartlarý ön sýraya yerleþtirdi. ");
                    }
                }


                else if (competitorCardInfo.CardName == "Centaur Archer")
                {
                    if (usedCards.Length > 0)
                    {
                        GameObject SelectedUsedCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];
                        CardInformation UsedCardInfo = SelectedUsedCard.GetComponent<CardInformation>();

                        int damage = 3;
                        UsedCardInfo.CardHealth = (int.Parse(UsedCardInfo.CardHealth) - damage).ToString();
                        UsedCardInfo.SetInformation();

                        if (int.Parse(UsedCardInfo.CardHealth) <= 0)
                        {
                            Destroy(SelectedUsedCard);
                        }

                        Debug.Log("Centaur Archer " + UsedCardInfo.CardName + " kartýna 3 hasar verdi");
                    }
                }


                else if (competitorCardInfo.CardName == "Minotaur Warrior")
                {
                    if (usedCards.Length > 0)
                    {
                        GameObject SelectedUsedCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];
                        CardInformation UsedCardInfo = SelectedUsedCard.GetComponent<CardInformation>();

                        int damage = 5;
                        UsedCardInfo.CardHealth = (int.Parse(UsedCardInfo.CardHealth) - damage).ToString();
                        UsedCardInfo.SetInformation();

                        if (int.Parse(UsedCardInfo.CardHealth) <= 0)
                        {
                            Destroy(SelectedUsedCard);
                        }

                        Debug.Log("Minotaur Warrior " + UsedCardInfo.CardName + " kartýna 5 hasar verdi");
                    }
                }


                else if (competitorCardInfo.CardName == "Siren")
                {
                    List<GameObject> eligibleMinions = new List<GameObject>();
                    foreach (var minion in usedCards)
                    {
                        CardInformation minionInfo = minion.GetComponent<CardInformation>();
                        if (minionInfo.CardDamage < 3)
                        {
                            eligibleMinions.Add(minion);
                        }
                    }

                    if (eligibleMinions.Count > 0)
                    {
                        // Rastgele bir eligible minion seç
                        GameObject selectedMinion = eligibleMinions[UnityEngine.Random.Range(0, eligibleMinions.Count)];
                        CardInformation selectedMinionInfo = selectedMinion.GetComponent<CardInformation>();

                        // Minyona 4 hasar ver
                        selectedMinionInfo.CardHealth = (int.Parse(selectedMinionInfo.CardHealth) - 4).ToString();
                        selectedMinionInfo.SetInformation();

                        if (int.Parse(selectedMinionInfo.CardHealth) <= 0)
                        {
                            Destroy(selectedMinion);
                        }

                        // Rakip oyuncuya hasar ver
                        _TutorialPlayerController.CompetitorHealth -= 4;
                        _TutorialPlayerController.RefreshUI();

                        Debug.Log("Siren, " + selectedMinionInfo.CardName + " kartýna 4 hasar verdi ve rakibe 4 hasar verdi.");
                    }

                    // Siren kartý bir kere oynandýktan sonra yok edilir
                    Destroy(selectedCompetitorCard);
                    Debug.Log("Siren kartý oynandýktan sonra yok edildi.");
                }


                else if (competitorCardInfo.CardName == "Nemean Lion")
                {
                    if (usedCards.Length > 0)
                    {
                        GameObject SelectedUsedCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];
                        CardInformation UsedCardInfo = SelectedUsedCard.GetComponent<CardInformation>();

                        UsedCardInfo.CardHealth = (int.Parse(UsedCardInfo.CardHealth) - 1).ToString();
                        UsedCardInfo.SetInformation();

                        if (int.Parse(UsedCardInfo.CardHealth) <= 0)
                        {
                            Destroy(SelectedUsedCard);
                        }

                        Debug.Log("Nemean Lion " + UsedCardInfo.CardName + " kartýna saldýrdý ve maksimum 1 hasar verdi");

                        UsedCardInfo.CardDamage = Mathf.Min(UsedCardInfo.CardDamage, 1);
                        UsedCardInfo.SetInformation();
                    }
                }


                else if (competitorCardInfo.CardName == "Hydra")
                {
                    if (usedCards.Length > 0)
                    {
                        // Rastgele bir 'UsedCard' kartý seç
                        GameObject SelectedUsedCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];
                        CardInformation UsedCardInfo = SelectedUsedCard.GetComponent<CardInformation>();

                        // Vurulan kartýn indexini bul
                        int selectedIndex = Array.IndexOf(usedCards, SelectedUsedCard);
                        Debug.Log(UsedCardInfo.CardName + "Kartýný seçti");
                        // Saðdaki karta hasar ver
                        if (selectedIndex + 1 < usedCards.Length)
                        {
                            GameObject rightCard = usedCards[selectedIndex + 1];
                            CardInformation rightCardInfo = rightCard.GetComponent<CardInformation>();
                            int damage = competitorCardInfo.CardDamage;

                            rightCardInfo.CardHealth = (int.Parse(rightCardInfo.CardHealth) - damage).ToString();
                            rightCardInfo.SetInformation();

                            if (int.Parse(rightCardInfo.CardHealth) <= 0)
                            {
                                Destroy(rightCard);
                            }

                            Debug.Log("Hydra " + rightCardInfo.CardName + " kartýna " + damage + " hasar verdi (saðdaki kart)");
                        }

                        // Soldaki karta hasar ver
                        if (selectedIndex - 1 >= 0)
                        {
                            GameObject leftCard = usedCards[selectedIndex - 1];
                            CardInformation leftCardInfo = leftCard.GetComponent<CardInformation>();
                            int damage = competitorCardInfo.CardDamage;

                            leftCardInfo.CardHealth = (int.Parse(leftCardInfo.CardHealth) - damage).ToString();
                            leftCardInfo.SetInformation();

                            if (int.Parse(leftCardInfo.CardHealth) <= 0)
                            {
                                Destroy(leftCard);
                            }

                            Debug.Log("Hydra " + leftCardInfo.CardName + " kartýna " + damage + " hasar verdi (soldaki kart)");
                        }
                    }
                }


                else if (competitorCardInfo.CardName == "Pegasus Rider")
                {
                    // Rakip kartýna saldýrý
                    usedCardInfo = selectedUsedCard.GetComponent<CardInformation>();

                    if (usedCardInfo.FirstDamageTaken)
                    {
                        // Ýlk hasarý görmezden gel
                        usedCardInfo.FirstDamageTaken = false;
                        Debug.Log("Pegasus Rider ilk hasarý görmezden geldi.");
                    }
                    else
                    {
                        // Normal hasar iþlemi
                        usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                        usedCardInfo.SetInformation();

                        if (int.Parse(usedCardInfo.CardHealth) <= 0)
                        {
                            Destroy(selectedUsedCard);
                        }

                        Debug.Log("Pegasus Rider " + usedCardInfo.CardName + " kartýna " + competitorCardInfo.CardDamage + " hasar verdi.");
                    }
                }


                else if (competitorCardInfo.CardName == "Greek Hoplite")
                {
                    // Rakip kartýna saldýrý
                    usedCardInfo = selectedUsedCard.GetComponent<CardInformation>();

                    // Greek Hoplite 3 hasar verir
                    int damage = 3;
                    usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - damage).ToString();
                    usedCardInfo.SetInformation();

                    if (int.Parse(usedCardInfo.CardHealth) <= 0)
                    {
                        Destroy(selectedUsedCard);
                    }

                    Debug.Log("Greek Hoplite " + usedCardInfo.CardName + " kartýna " + damage + " hasar verdi.");
                }


                // Chengis Khan //

                else if (competitorCardInfo.CardName == "Mongol Messenger")
                {
                    _TutorialPlayerController.CreateAnCompetitorCard();
                }

                else if (competitorCardInfo.CardName == "Khan’s Envoy")
                {
                    bool acceptAttack = UnityEngine.Random.Range(0f, 1f) < 0.5f;
                    if (acceptAttack)
                    {
                        // Saldýrýyý kabul eder
                        usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                        usedCardInfo.SetInformation();

                        if (int.Parse(usedCardInfo.CardHealth) <= 0)
                        {
                            Destroy(selectedUsedCard);
                        }

                        Debug.Log("Khan’s Envoy saldýrýyý kabul etti ve " + usedCardInfo.CardName + " kartýna " + competitorCardInfo.CardDamage + " hasar verdi.");
                    }
                    else
                    {
                        // Baþka bir CompetitorCard seç ve saldýrýyý yönlendir
                        GameObject newCompetitorCard;
                        do
                        {
                            newCompetitorCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                        } while (newCompetitorCard == selectedCompetitorCard);

                        CardInformation newCompetitorCardInfo = newCompetitorCard.GetComponent<CardInformation>();

                        newCompetitorCardInfo.CardHealth = (int.Parse(newCompetitorCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                        newCompetitorCardInfo.SetInformation();

                        if (int.Parse(newCompetitorCardInfo.CardHealth) <= 0)
                        {
                            Destroy(newCompetitorCard);
                        }

                        Debug.Log("Khan’s Envoy saldýrýyý kabul etmedi ve " + newCompetitorCardInfo.CardName + " kartýna yönlendirdi.");
                    }
                }


                else if (competitorCardInfo.CardName == "Mongol Archer")
                {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, selectedCompetitorCard.transform.parent.gameObject);
                    int[] validIndices = { 0, 6, 7, 13 };

                    if (Array.Exists(validIndices, element => element == index))
                    {
                        Debug.Log("Selected Mongol Archer card is at a valid index: " + index);
                        selectedCompetitorCard.GetComponent<CardInformation>().CardDamage += 2;
                    }
                    else
                    {
                        Debug.Log("Selected Mongol Archer card is not at a valid index.");
                    }

                    selectedCompetitorCard.GetComponent<CardInformation>().SetInformation();
                }


                else if (competitorCardInfo.CardName == "Mongol Shaman")
                {
                    GameObject[] CompetitorCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
                    if (CompetitorCard.Length > 0)
                    {
                     
                        GameObject selectedCard = CompetitorCard[UnityEngine.Random.Range(0, CompetitorCard.Length)];
                        CardInformation CardInfo = selectedCard.GetComponent<CardInformation>();

                       
                        string maxHealth = CardInfo.MaxHealth; // Eski saðlýk deðerini al
                        CardInfo.CardHealth = maxHealth.ToString();
                        CardInfo.SetInformation();

                        Debug.Log("Mongol Shaman " + CardInfo.CardName + " kartýnýn saðlýðý eski deðerine geri yüklendi.");
                    }
                    else
                    {
                        Debug.LogWarning("Mongol Shaman uygun kart bulamadý");
                    }

                }


                else if (competitorCardInfo.CardName == "Eagle Hunter")
                {
                    List<int> emptyCells = new List<int>();

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            emptyCells.Add(i);
                        }
                    }
                    if (emptyCells.Count > 0)
                    {
                        int randomIndex = emptyCells[UnityEngine.Random.Range(0, emptyCells.Count)];

                       _TutorialPlayerController.CreateSpecialCard("Eagle", "1", 2, 0, randomIndex, true);
                    }
                    else
                    {
                        Debug.LogWarning("No empty cells available to place the Eagle card.");
                    }
                }


                else if (competitorCardInfo.CardName == "Yurt Builder")
                {
                   _TutorialPlayerController.Mana += 1;

                    _TutorialPlayerController.CompetitorManaCountText.text = _TutorialPlayerController.Mana.ToString() + "/10";
                    _TutorialPlayerController.CompetitorManaBar.fillAmount = _TutorialPlayerController.Mana / 10f;
                    _TutorialPlayerController.RefreshUI();


                    Debug.Log("Yurt Builder Mana deðerini bir arttýrdý");
                }


                else if (competitorCardInfo.CardName == "Marco Polo")
                {
                    _TutorialPlayerController.CreateAnCompetitorCard();
                    Debug.Log("destesine 1 kart eklendi");
                }


                else if (competitorCardInfo.CardName == "Steppe Warlord")
                {
                    GameObject[] mycards = GameObject.FindGameObjectsWithTag("CompetitorCard");
                    foreach (GameObject card in mycards)
                    {
                        if (card.GetComponent<CardInformation>().CardName == "Mongol Messenger" || card.GetComponent<CardInformation>().CardName == "Mongol Archer" || card.GetComponent<CardInformation>().CardName == "General Subutai")
                        {
                            Debug.Log("KartBulundu can arttýrýldý");
                            card.GetComponent<CardInformation>().CardDamage += 2;
                            card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                            card.GetComponent<CardInformation>().SetInformation();

                        }
                    }
                }


                else if (competitorCardInfo.CardName == "General Subutai")
                {
                    usedCardInfo = selectedUsedCard.GetComponent<CardInformation>();
                    usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                    usedCardInfo.SetInformation();
                    Debug.Log("General Subutai " + usedCardInfo.CardName + " kartýna " + competitorCardInfo.CardDamage + " hasar verdi.");
                }

                
                competitorCardInfo.HasAttacked = true;

            }



        }

    }
    void ApplySpellEffect(CardInformation cardInfo)
    {
        if (cardInfo.CardName == "Lightning Storm")
        {
            GameObject[] allTargets = GameObject.FindGameObjectsWithTag("UsedCard");

            foreach (var card in allTargets)
            {
                CardInformation targetCardInfo = card.GetComponent<CardInformation>();

                if (!string.IsNullOrEmpty(targetCardInfo.CardHealth))
                {
                    int damage = UnityEngine.Random.Range(2, 4);
                    targetCardInfo.CardHealth = (int.Parse(targetCardInfo.CardHealth) - damage).ToString();
                    targetCardInfo.SetInformation();

                    if (int.Parse(targetCardInfo.CardHealth) <= 0)
                    {
                        Destroy(card);
                        Debug.Log("Lightning Storm " + targetCardInfo.CardName + " kartýný yok etti");
                    }
                    else
                    {
                        Debug.Log("Lightning Storm " + targetCardInfo.CardName + " kartýna " + damage + " hasar verdi");
                    }
                }
                else
                {
                    Debug.Log("Lightning Storm saldýrý yapýcak kart bulamadý.");
                }
            }
            
        }

        else if (cardInfo.CardName == "Olympian Favor")
        {
            GameObject[] allUsedCards = GameObject.FindGameObjectsWithTag("UsedCard");
            GameObject[] allCompetitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");

            List<GameObject> allCards = new List<GameObject>();
            allCards.AddRange(allUsedCards);
            allCards.AddRange(allCompetitorCards);

            if (allCards.Count > 0)
            {
                GameObject selectedCard = allCards[UnityEngine.Random.Range(0, allCards.Count)];
                CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

                if (selectedCard.CompareTag("UsedCard"))
                {
                    Debug.Log("Olympian Favor " + selectedCardInfo.CardName + " kartýna 2 hasar verdi");
                    selectedCardInfo.CardHealth = (int.Parse(selectedCardInfo.CardHealth) - 2).ToString();
                    selectedCardInfo.SetInformation();

                    if (int.Parse(selectedCardInfo.CardHealth) <= 0)
                    {
                        Destroy(selectedCard);
                        Debug.Log("Olympian Favor " + selectedCardInfo.CardName + " kartýný yok etti");
                    }
                }
                else if (selectedCard.CompareTag("CompetitorCard") && selectedCardInfo.CardName != "Olympian Favor")
                {
                    Debug.Log("Olympian Favor " + selectedCardInfo.CardName + " kartýna 2 can verdi");
                    selectedCardInfo.CardHealth = (int.Parse(selectedCardInfo.CardHealth) + 2).ToString();
                    selectedCardInfo.SetInformation();
                }
            }
            else
            {
                Debug.Log("Olympian Favor etkinleþtirildi ancak tahtada seçilebilecek bir kart bulunamadý.");
            }

            Debug.Log("Olympian Favor kullanýldý.");
        }



        else if (cardInfo.CardName == "Golden Fleece")
        {
            GameObject[] CompetitorCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
            if (CompetitorCard.Length > 0)
            {
               
                GameObject selectedCard = CompetitorCard[UnityEngine.Random.Range(0, CompetitorCard.Length)];
                CardInformation CardInfo = selectedCard.GetComponent<CardInformation>();

                string maxHealth = CardInfo.MaxHealth; // Eski saðlýk deðerini al
                CardInfo.CardHealth = maxHealth.ToString();
                CardInfo.SetInformation();

                Debug.Log("Golden Fleece kullanýldý: " + CardInfo.CardName + " kartýnýn saðlýðý eski deðerine geri yüklendi.");
            }
            else
            {
                Debug.LogWarning("Golden Fleece etkinleþtirmek için uygun kart bulunamadý.");
            }
        }


        else if (cardInfo.CardName == "Labyrinth Maze")
        {
            GameObject[] usedCards = GameObject.FindGameObjectsWithTag("UsedCard");
            // Seçilen aralýkta kartlarý bul
            List<GameObject> cardsInRange = new List<GameObject>();
            for (int i = 8; i <= 14; i++)
            {
                GameObject card = GetRandomCardInRange(usedCards, i, i);
                if (card != null)
                {
                    cardsInRange.Add(card);
                }
            }
            GameObject deck = GameObject.Find("Deck");
            foreach (GameObject card in cardsInRange)
            {
                if (deck.transform.childCount < 10)
                {
                    LabyrinthMaze();
                    Debug.Log("Labyrinth Maze " + card.name + " kartýný desteye ekledi.");
                }
                else
                {
                    Destroy(card);
                    Debug.Log("Labyrinth Maze " + card.name + " kartýný yok etti.");
                }
            }

        }


        else if (cardInfo.CardName == "Divine Ascention")
        {
            GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");

            if (competitorCards.Length > 0)
            {
                GameObject selectedCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];

                CardInformation CardInfo = selectedCard.GetComponent<CardInformation>();

                if (CardInfo != null && !string.IsNullOrEmpty(CardInfo.CardHealth) && int.Parse(CardInfo.CardHealth) > 0)
                {
                    // Saðlýk bilgisini ikiye katlayacak Coroutine'i baþlat
                    StartCoroutine(DoubleHealthNextTurn(CardInfo));
                }
            }

        }

        else if (cardInfo.CardName == "Horseback Archery")
        {

            GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");

            foreach (GameObject card in competitorCards)
            {
                CardInformation CardInfo = card.GetComponent<CardInformation>();

                if (CardInfo.CardName == "Mongol Messenger" || CardInfo.CardName == "Mongol Archer" || CardInfo.CardName == "General Subutai")
                {
                    CardInfo.CardHealth = (int.Parse(CardInfo.CardHealth) + 2).ToString();
                    CardInfo.SetInformation();

                    Debug.Log("Horseback Archery " + CardInfo.CardName + " kartýna +2 can ekledi.");
                }
            }
        }

        else if (cardInfo.CardName == "Ger Defense")
        {
            GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");

            if (competitorCards.Length > 0)
            {
                GameObject selectedCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                List<GameObject> surroundingCards = GetSurroundingCards(selectedCard);

                foreach (GameObject card in surroundingCards)
                {
                    CardInformation CardInfo = card.GetComponent<CardInformation>();
                    CardInfo.CardHealth = (int.Parse(CardInfo.CardHealth) + 2).ToString();
                    CardInfo.SetInformation();
                    Debug.Log("Çevresine 2 can verdi");
                }

                // Tur bittiðinde eklenen canlarý geri almak için Coroutine baþlat
                StartCoroutine(RemoveTemporaryHealth(surroundingCards));
            }
        }

        else if (cardInfo.CardName == "Mongol Fury")
        {
            GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
            List<CardInformation> cardInfos = new List<CardInformation>();

            foreach (GameObject card in competitorCards)
            {
                CardInformation CardInfo = card.GetComponent<CardInformation>();
                if (CardInfo != null)
                {
                    cardInfos.Add(cardInfo);
                    CardInfo.CardDamage = CardInfo.CardDamage + 2;
                    CardInfo.SetInformation();
                }
            }

            // 1 tur bekleyip hasarý geri al
            StartCoroutine(RemoveTemporaryDamage(cardInfos));
        }

        else if (cardInfo.CardName == "Eternal Steppe’s Whisper")
        {
            GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");

            if (competitorCards.Length > 0)
            {
                // Rastgele bir rakip kartý seç
                GameObject selectedCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

                if (selectedCardInfo != null)
                {
                    // Kartý 1 tur boyunca saldýrýlardan koru
                    selectedCardInfo.IsImmuneToAttacks = true;
                    Debug.Log("Eternal Steppe’s Whisper etkinleþtirildi: " + selectedCardInfo.CardName + " kartý 1 tur boyunca saldýrýlardan korundu.");

                    // Kartýn saldýrýdan korunma durumunu geri almak için Coroutine baþlat
                    StartCoroutine(RemoveImmunityAfterOneTurn(selectedCardInfo));
                }
                
            }
            else
            {
                Debug.LogWarning("Eternal Steppe’s Whisper etkinleþtirmek için uygun kart bulunamadý.");
            }
        }


        else if (cardInfo.CardName == "Eternal Steppe’s Whisper")
        {
           _TutorialPlayerController.SteppeAmbush = true;
        }

    }
    private IEnumerator RemoveImmunityAfterOneTurn(CardInformation cardInfo)
    {
        yield return new WaitForSeconds(1f); // 1 tur beklemek için süreyi ayarlayýn

        if (cardInfo != null)
        {
            cardInfo.IsImmuneToAttacks = false;
            Debug.Log(cardInfo.CardName + " kartý artýk saldýrýlardan etkilenebilir.");
        }
    }
    IEnumerator RemoveTemporaryDamage(List<CardInformation> cardInfos)
    {
        yield return new WaitForSeconds(1); // 1 tur bekle

        foreach (CardInformation CardInfo in cardInfos)
        {
            CardInfo.CardDamage = CardInfo.CardDamage - 2;
            CardInfo.SetInformation();
        }

        Debug.Log("Mongol Fury etkisi sona erdi ve +2 hasar geri alýndý.");
    }
    List<GameObject> GetSurroundingCards(GameObject card)
    {
        
        List<GameObject> surroundingCards = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(card.transform.position, 1.0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("CompetitorCard"))
            {
                surroundingCards.Add(hitCollider.gameObject);
            }
        }
        return surroundingCards;
    }

    IEnumerator RemoveTemporaryHealth(List<GameObject> cards)
    {
        yield return new WaitForSeconds(1);

        foreach (GameObject card in cards)
        {
            CardInformation cardInfo = card.GetComponent<CardInformation>();
            cardInfo.CardHealth = (int.Parse(cardInfo.CardHealth) - 2).ToString();
            cardInfo.SetInformation();
        }

        Debug.Log("Ger Defense büyüsünün etkisi sona erdi ve +2 can geri alýndý.");
    }
    IEnumerator DoubleHealthNextTurn(CardInformation CardInfo)
    {
        
        // Bir sonraki tura kadar bekle
        yield return new WaitForSeconds(1); // Turun bittiðini varsayan bir senaryo
        Debug.Log("Bir sonraki tur bekleniyor");

      
        if (!string.IsNullOrEmpty(CardInfo.CardHealth) && int.Parse(CardInfo.CardHealth) > 0)
        {
            
            int currentHealth = int.Parse(CardInfo.CardHealth);
            CardInfo.CardHealth = (currentHealth * 2).ToString();
            CardInfo.SetInformation();

            Debug.Log("Divine Ascension " + CardInfo.CardName + " kartýnýn saðlýðýný ikiye katladý");
        }
    }

    
    void LabyrinthMaze()
    {
        CardsAreaCreator _cardsAreaCreator;
        _cardsAreaCreator = GameObject.Find("Area").GetComponent<CardsAreaCreator>();
        for (int i = 7; i < 14; i++)
        {
            GameObject areaCollision = _cardsAreaCreator.FrontAreaCollisions[i];
            int childCount = areaCollision.transform.childCount;
            if (childCount > 0)
            {

                GameObject deckObject = GameObject.Find("CompetitorDeck");
                if (deckObject.transform.childCount < 10)
                {

                    GameObject CardCurrent = Instantiate(Resources.Load<GameObject>("TutorialCompetitorCard"), GameObject.Find("CompetitorDeck").transform);

                    float xPos = _TutorialPlayerController.DeckCardCount * 0.8f - 0.8f; // Kartýn X konumunu belirliyoruz
                    CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartýn pozisyonunu ayarlýyoruz

                    _TutorialPlayerController.CreateAnCard();
                    _TutorialPlayerController.DeckCardCount++;
                    Destroy(areaCollision.transform.GetChild(0).gameObject);
                }

            }
        }
    }



    public void BotCreateHoplitesCard(int CreateCardIndex)
    {


        GameObject CardCurrent = Instantiate(Resources.Load<GameObject>("HoplitesCard_Prefab"), GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[CreateCardIndex].transform);

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

    // Kartlarýn belirli bir aralýkta olup olmadýðýný kontrol eder
    private bool CheckCardInRange(GameObject[] cards, int minIndex, int maxIndex)
    {
        foreach (GameObject card in cards)
        {
            int index = GetCardIndex(card);
            if (index >= minIndex && index <= maxIndex)
            {
                return true;
            }
        }
        return false;
    }

    // Kartýn bulunduðu alanýn indeksini döndürür
    private int GetCardIndex(GameObject card)
    {
        
        string parentName = card.transform.parent.name;
        int index;
        if (int.TryParse(parentName, out index))
        {
            return index;
        }
        return -1;
    }

    // Belirli bir aralýkta rastgele bir kart seçer
    private GameObject GetRandomCardInRange(GameObject[] cards, int minIndex, int maxIndex)
    {
        List<GameObject> cardsInRange = new List<GameObject>();

        foreach (GameObject card in cards)
        {
            int index = GetCardIndex(card);
            if (index >= minIndex && index <= maxIndex)
            {
                cardsInRange.Add(card);
            }
        }

        if (cardsInRange.Count > 0)
        {
            return cardsInRange[UnityEngine.Random.Range(0, cardsInRange.Count)];
        }
        return null;
    }
    

    int FindEmptyAreaBox()

   {
    
      

      List<int> EmptyIndexes = new List<int>();

      for (int i = 0; i < GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions.Length; i++)
      {
        if(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].transform.childCount==0)
        {
           EmptyIndexes.Add(i);
           print("Eklendi");
        }
        
      }
     
      return  EmptyIndexes[UnityEngine.Random.Range(0, EmptyIndexes.Count)];
        
   }


   public void CreateCardFromBot()
   {

    _TutorialPlayerController.RemoveAnCompetitorCard();
     GameObject CardCurrent = Instantiate(_TutorialPlayerController.CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[FindEmptyAreaBox()].transform);
     CardCurrent.tag = "CompetitorCard";

   
     CardCurrent.transform.localScale = new Vector3(1,1,0.04f);
     CardCurrent.transform.localPosition = Vector3.zero;
     CardCurrent.transform.localEulerAngles = new Vector3(45,0,180);

      _TutorialPlayerController.CreateInfoCard(CardCurrent);


        CardInformation cardInfo = CardCurrent.GetComponent<CardInformation>();

        // Kartýn bir büyü kartý olup olmadýðýný kontrol et ve büyü etkisini uygula
        if (string.IsNullOrEmpty(cardInfo.CardHealth))
        {
           
            ApplySpellEffect(cardInfo);
            Debug.LogError("USSEEDD A SPEEELLL");
            Destroy(CardCurrent); // Büyü kartlarý sahnede kalmaz, uygulandýktan sonra yok edilir
            
        }



    }

   IEnumerator Waiter()
   {
      yield return new WaitForSeconds(1);
      Debug.Log("SIRA SENDE");

      if(UnityEngine.Random.Range(0, 10)<6)
      {
         CreateCardFromBot();
      }
      
      _TutorialPlayerController.BeginerFunction();
      
   }
}
