using JetBrains.Annotations;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

public class BotController : MonoBehaviour
{
    TutorialPlayerController _TutorialPlayerController;
    TutorialCardProgress _TutorialCardProgress;
    private bool specialAttackUsed = false;
    [HideInInspector] public List<string> DeadMyCardName = null;
    public GameObject AttackerCard, TargetCard;
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


        if (!specialAttackUsed && UnityEngine.Random.Range(0, 5) == 0) // 1/5 �ansla �zel sald�r� art�r�labilir,
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
        Debug.Log("Bot �zel sald�r� yap�yor!");
        GameObject[] usedCards = GameObject.FindGameObjectsWithTag("UsedCard");

        if (usedCards.Length > 0 && UnityEngine.Random.Range(0, 2) == 0)
        {


            GameObject selectedUsedCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];
            CardInformation cardInfo = selectedUsedCard.GetComponent<CardInformation>();

            cardInfo.CardHealth = (int.Parse(cardInfo.CardHealth) - _TutorialPlayerController.CompetitorHeroAttackDamage).ToString();
            cardInfo.SetInformation();

            Debug.Log("Bot �zel Sald�r�s�n� Se�ilen Karta Yapt�");
            if (int.Parse(cardInfo.CardHealth) <= 0)
            {
                Destroy(selectedUsedCard);
            }
            GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
            foreach (GameObject card in competitorCards)
            {

                if (card.GetComponent<CardInformation>().CardName == "Organ Gun")
                {
                    GameObject randomCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];

                    randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) - 3).ToString();
                    randomCard.GetComponent<CardInformation>().SetInformation();
                    Debug.Log(randomCard.GetComponent<CardInformation>().CardName + "-3 hasar ald�");

                }
            }
        }

        else
        {
            Debug.Log("Bot �zel Sald�r�s�n� Oyuncuya Yapt�.");
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
            

            // 8-14 aras� kart kontrol�
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
                    Debug.Log("Heracles " + usedCardInfo.CardName + " kart�na sald�r� yapt�");
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
                    Debug.Log("Stormcaller " + usedCardInfo.CardName + " kart�na sald�r� yapt�");
                    int spellsExtraDamage = 0;

                    // Rakibin elindeki kartlar� kontrol et
                    GameObject[] competitorDeck = GameObject.FindGameObjectsWithTag("CompetitorDeckCard");
                    foreach (GameObject card in competitorDeck)
                    {
                        CardInformation cardInfo = card.GetComponent<CardInformation>();
                        if (string.IsNullOrEmpty(cardInfo.CardHealth))  // B�y� kart� kontrol�
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


                else if (competitorCardInfo.CardName == "Odyssean Navigator") // botun elindeki kartlara sadece g�steri� olarak ekleniyor.
                {

                    bool isMinion = UnityEngine.Random.Range(0f, 1f) < 0.5f;
                    if (isMinion)
                    {
                        _TutorialPlayerController.CreateAnCompetitorCard();
                        Debug.LogError("ODYYYYYSEAAANN M�NNYOONNNN YARATTTI ");
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
                    bool isSpell = UnityEngine.Random.Range(0f, 1f) < 0.5f;// destede b�y� olma olas�l���
                    if (isSpell)
                    {
                        _TutorialPlayerController.CreateAnCompetitorCard();
                        int randomValue = UnityEngine.Random.Range(1, 11); // 1 ve 10 mana de�eri tahmin etme
                        Debug.Log("Olu�turulan Spell Kart�n mana de�eri :" + randomValue);
                        if (randomValue >= 4)
                        {
                            _TutorialPlayerController.CompetitorHealth += 4; // OwnHealth de�erine +4 can ekle
                            _TutorialPlayerController.RefreshUI();
                            Debug.Log("Oracle's Emissary in �rett�i Spell KAr�n Mana De�eri 4 ten b�y�k oldu�� i�in OwnHealt De�eri Art�r�ld�.");
                        }
                    }
                }



                else if (competitorCardInfo.CardName == "Lightning Forger")
                {
                    Debug.Log("Lightning Forger masaya eklendi, OwnHeroAttackDamage art�r�ld�.");
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
                            int cardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                            card.GetComponent<CardInformation>().CardFreeze = true;
                            Debug.Log("Gorgon Kartlar�n� Dondurdu");
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
                                Debug.Log("Chimera " + cardInfo.CardName + " kart�n� yok etti");
                            }
                        }
                    }

                    Debug.Log("Chimera t�m rakip kartlara 2 hasar verdi.");

                    // Chimera tekrar kullanmamak i�in yok edildi.
                    Destroy(selectedCompetitorCard);
                    Debug.Log("Chimera kart� yok edildi.");
                }


                else if (competitorCardInfo.CardName == "Athena")
                {
                    for (int i = 7; i < 14; i++)
                    {

                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount == 0)
                        {
                            BotCreateHoplitesCard(i);
                        }
                        Debug.Log("Athena, Hoplite Kartlar� �n s�raya yerle�tirdi. ");
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

                        Debug.Log("Centaur Archer " + UsedCardInfo.CardName + " kart�na 3 hasar verdi");
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

                        Debug.Log("Minotaur Warrior " + UsedCardInfo.CardName + " kart�na 5 hasar verdi");
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
                        // Rastgele bir eligible minion se�
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

                        Debug.Log("Siren, " + selectedMinionInfo.CardName + " kart�na 4 hasar verdi ve rakibe 4 hasar verdi.");
                    }

                    // Siren kart� bir kere oynand�ktan sonra yok edilir
                    Destroy(selectedCompetitorCard);
                    Debug.Log("Siren kart� oynand�ktan sonra yok edildi.");
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

                        Debug.Log("Nemean Lion " + UsedCardInfo.CardName + " kart�na sald�rd� ve maksimum 1 hasar verdi");

                        UsedCardInfo.CardDamage = Mathf.Min(UsedCardInfo.CardDamage, 1);
                        UsedCardInfo.SetInformation();
                    }
                }


                else if (competitorCardInfo.CardName == "Hydra")
                {
                    if (usedCards.Length > 0)
                    {
                        // Rastgele bir 'UsedCard' kart� se�
                        GameObject SelectedUsedCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];
                        CardInformation UsedCardInfo = SelectedUsedCard.GetComponent<CardInformation>();

                        // Vurulan kart�n indexini bul
                        int selectedIndex = Array.IndexOf(usedCards, SelectedUsedCard);
                        Debug.Log(UsedCardInfo.CardName + "Kart�n� se�ti");
                        // Sa�daki karta hasar ver
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

                            Debug.Log("Hydra " + rightCardInfo.CardName + " kart�na " + damage + " hasar verdi (sa�daki kart)");
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

                            Debug.Log("Hydra " + leftCardInfo.CardName + " kart�na " + damage + " hasar verdi (soldaki kart)");
                        }
                    }
                }


                else if (competitorCardInfo.CardName == "Pegasus Rider")
                {
                    // Rakip kart�na sald�r�
                    usedCardInfo = selectedUsedCard.GetComponent<CardInformation>();

                    if (usedCardInfo.FirstDamageTaken)
                    {
                        // �lk hasar� g�rmezden gel
                        usedCardInfo.FirstDamageTaken = false;
                        Debug.Log("Pegasus Rider ilk hasar� g�rmezden geldi.");
                    }
                    else
                    {
                        // Normal hasar i�lemi
                        usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                        usedCardInfo.SetInformation();

                        if (int.Parse(usedCardInfo.CardHealth) <= 0)
                        {
                            Destroy(selectedUsedCard);
                        }

                        Debug.Log("Pegasus Rider " + usedCardInfo.CardName + " kart�na " + competitorCardInfo.CardDamage + " hasar verdi.");
                    }
                }


                else if (competitorCardInfo.CardName == "Greek Hoplite")
                {
                    // Rakip kart�na sald�r�
                    usedCardInfo = selectedUsedCard.GetComponent<CardInformation>();

                    // Greek Hoplite 3 hasar verir
                    int damage = 3;
                    usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - damage).ToString();
                    usedCardInfo.SetInformation();

                    if (int.Parse(usedCardInfo.CardHealth) <= 0)
                    {
                        Destroy(selectedUsedCard);
                    }

                    Debug.Log("Greek Hoplite " + usedCardInfo.CardName + " kart�na " + damage + " hasar verdi.");
                }


                // Chengis Khan //

                else if (competitorCardInfo.CardName == "Mongol Messenger")
                {
                    _TutorialPlayerController.CreateAnCompetitorCard();
                }

                else if (competitorCardInfo.CardName == "Khan�s Envoy")
                {
                    bool acceptAttack = UnityEngine.Random.Range(0f, 1f) < 0.5f;
                    if (acceptAttack)
                    {
                        // Sald�r�y� kabul eder
                        usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                        usedCardInfo.SetInformation();

                        if (int.Parse(usedCardInfo.CardHealth) <= 0)
                        {
                            Destroy(selectedUsedCard);
                        }

                        Debug.Log("Khan�s Envoy sald�r�y� kabul etti ve " + usedCardInfo.CardName + " kart�na " + competitorCardInfo.CardDamage + " hasar verdi.");
                    }
                    else
                    {
                        // Ba�ka bir CompetitorCard se� ve sald�r�y� y�nlendir
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

                        Debug.Log("Khan�s Envoy sald�r�y� kabul etmedi ve " + newCompetitorCardInfo.CardName + " kart�na y�nlendirdi.");
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


                        string maxHealth = CardInfo.MaxHealth; // Eski sa�l�k de�erini al
                        CardInfo.CardHealth = maxHealth.ToString();
                        CardInfo.SetInformation();

                        Debug.Log("Mongol Shaman " + CardInfo.CardName + " kart�n�n sa�l��� eski de�erine geri y�klendi.");
                    }
                    else
                    {
                        Debug.LogWarning("Mongol Shaman uygun kart bulamad�");
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


                    Debug.Log("Yurt Builder Mana de�erini bir artt�rd�");
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
                            Debug.Log("KartBulundu can artt�r�ld�");
                            card.GetComponent<CardInformation>().CardDamage += 2;
                            card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                            card.GetComponent<CardInformation>().SetInformation();

                        }
                    }
                }
                //leonardo

                else if (competitorCardInfo.CardName == "General Subutai")
                {
                    usedCardInfo = selectedUsedCard.GetComponent<CardInformation>();
                    usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - competitorCardInfo.CardDamage).ToString();
                    usedCardInfo.SetInformation();
                    Debug.Log("General Subutai " + usedCardInfo.CardName + " kart�na " + competitorCardInfo.CardDamage + " hasar verdi.");
                }
                else if (competitorCardInfo.CardName == "Codex Guardian")
                {
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, competitorCardInfo.transform.parent.gameObject);
                    competitorCardInfo.GetComponent<CardInformation>().DivineSelected = true;
                    Debug.Log("codex guardian kullan�ld� ");
                }


                else if (competitorCardInfo.CardName == "Automaton Apprentice")
                {
                    competitorCardInfo.GetComponent<CardInformation>().CardHealth = (int.Parse(competitorCardInfo.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                    competitorCardInfo.GetComponent<CardInformation>().SetInformation();
                }

                else if (competitorCardInfo.CardName == "Automaton Duelist")
                {

                    competitorCardInfo.GetComponent<CardInformation>().CardDamage += 1; ;
                    competitorCardInfo.GetComponent<CardInformation>().SetInformation();

                }
               
                else if (competitorCardInfo.CardName == "Gyrocopter")
                {

                    GameObject[] AllCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
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
                                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, competitorCardInfo.transform.parent.gameObject);
                                competitorCardInfo.GetComponent<CardInformation>().CardHealth = (int.Parse(competitorCardInfo.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                                competitorCardInfo.GetComponent<CardInformation>().CardDamage += 2;

                            }
                        }
                    }
                }
                
                else if (competitorCardInfo.CardName == "Piscean Diver")
                {
                    competitorCardInfo.GetComponent<CardInformation>().CanAttackBehind = true;
                }
                else if (competitorCardInfo.CardName == "Da Vinci's Helix Engineer")
                {
                    _TutorialPlayerController.CreateAnCompetitorCard();
                    bool isMinion = UnityEngine.Random.Range(0f, 1f) < 0.5f;
                    if (isMinion)
                    {

                        Debug.LogError("Helix SPEEEELLL YARATTTI ");
                    }
                    else
                    {
                        Debug.LogError("Helix M�NNYOONNNN YARATTTI ");

                    }
                }
                
                //odincards
                else if (competitorCardInfo.CardName == "Viking Raider")
                {
                    competitorCardInfo.GetComponent<CardInformation>().isItFirstRaound = false;
                }
                else if (competitorCardInfo.CardName == "Runestone Mystic")
                {
                    _TutorialPlayerController.CreateAnCompetitorCard();
                    int spellsExtraDamage = 0;
                    competitorCardInfo.CardDamage += spellsExtraDamage;
                    competitorCardInfo.SetInformation();
                }
                else if (competitorCardInfo.CardName == "Fenrir's Spawn")
                {
                    if (usedCards.Length > competitorCards.Length)
                    {
                        competitorCardInfo.GetComponent<CardInformation>().CardHealth = (int.Parse(competitorCardInfo.GetComponent<CardInformation>().CardHealth) + 3).ToString();
                        competitorCardInfo.GetComponent<CardInformation>().CardDamage += 2;
                    }

                }
                else if (competitorCardInfo.CardName == "Shieldmaiden Defender")
                {
                    int frontRowCardCount = 0;

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount > 0)
                        {
                            frontRowCardCount++;
                        }
                    }
                    competitorCardInfo.GetComponent<CardInformation>().CardHealth = (int.Parse(competitorCardInfo.GetComponent<CardInformation>().CardHealth) + frontRowCardCount).ToString();
                    competitorCardInfo.GetComponent<CardInformation>().SetInformation();
                }
                else if (competitorCardInfo.CardName == "Draugr Warrior")
                {
                    DraugrWarrior();
                }
                else if (competitorCardInfo.CardName == "Norn Weaver")
                {
                    
                    bool isMinion = UnityEngine.Random.Range(0f, 1f) < 0.5f;
                    if (isMinion)
                    {

                        _TutorialPlayerController.CreateAnCompetitorCard();
                    }
                    else
                    {
                        Debug.LogError("yanl�� tahmin ");

                    }

                }
                else if (competitorCardInfo.CardName == "Skald Bard")
                {

                    _TutorialPlayerController.CreateAnCompetitorCard();

                }
                else if (competitorCardInfo.CardName == "Mimir's Seer")
                {

                    _TutorialPlayerController.CreateAnCompetitorCard();

                }
                else if (competitorCardInfo.CardName == "Frost Giant")
                {

                    GameObject randomCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];

                    int cardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, randomCard.transform.parent.gameObject);
                    randomCard.GetComponent<CardInformation>().CardFreeze = true;
                    Debug.Log("Frost Giant random bir kart Dondurdu");
                       
                }

                // DustinCards

                else if (competitorCardInfo.CardName == "Mutant Behemoth")
                {
                    for (int i = 7; i < 14; i++)
                    {

                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount != 0)
                        {
                            GameObject CurrentCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.GetChild(0).gameObject;

                            int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, CurrentCard.transform.parent.gameObject);
                            CurrentCard.GetComponent<CardInformation>().CardDamage -= 2;
                            CurrentCard.GetComponent<CardInformation>().Behemot = true;

                            
                        }
                    }

                }
                else if (competitorCardInfo.CardName == "Scavenger Raider")
                {
                    _TutorialPlayerController.CreateAnCompetitorCard();
                }
              
                else if (competitorCardInfo.CardName == "Wasteland Sniper")
                {
                    GameObject randomCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];

                    int cardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, randomCard.transform.parent.gameObject);
                    randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(usedCardInfo.GetComponent<CardInformation>().CardHealth) -2).ToString();
                    Debug.Log("Frost Giant random bir kart Dondurdu");


                }
                else if (competitorCardInfo.CardName == "Engineer of the Ruins")
                {

                    GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("Competit�rCard");
                    foreach (var card in AllMyCard)
                    {
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                        card.GetComponent<CardInformation>().SetInformation();

                    }

                }
                else if (competitorCardInfo.CardName == "Lone Cyborg")
                {

                    int cardscount = 0;
                    for (int i = 7; i < 14; i++)
                    {
                        var cardsAreaCreator = GameObject.Find("Area").GetComponent<CardsAreaCreator>();
                        var areaCollision = cardsAreaCreator.BackAreaCollisions[i];

                        if (areaCollision.gameObject.transform.childCount != 0)
                        {
                            cardscount++;
                        }
                    }
                    if (cardscount == 1)
                    {
                        competitorCardInfo.GetComponent<CardInformation>().CardDamage += 3;
                        competitorCardInfo.GetComponent<CardInformation>().CardHealth = (int.Parse(competitorCardInfo.GetComponent<CardInformation>().CardHealth) + 3).ToString();
                        competitorCardInfo.GetComponent<CardInformation>().DivineSelected = true;
                        competitorCardInfo.GetComponent<CardInformation>().SetInformation();

                    }
                    else if (competitorCardInfo.CardName == "Rogue AI Drone")
                    {
                        competitorCardInfo.GetComponent<CardInformation>().Invulnerable = true;

                    }
                    else if (competitorCardInfo.CardName == "Claire")
                    {
                        _TutorialCardProgress.DamageToAlLOtherMinions(competitorCardInfo.GetComponent<CardInformation>().CardDamage, competitorCardInfo.GetComponent<CardInformation>().CardName);
                        competitorCardInfo.GetComponent<CardInformation>().CardHealth = (int.Parse(competitorCardInfo.GetComponent<CardInformation>().CardHealth) - 2).ToString();
                        

                    }

                    //AnubisCards
                    else if (competitorCardInfo.CardName == "Sandstone Scribe")
                    {
                        _TutorialPlayerController.CreateAnCompetitorCard();

                    }
                    else if (competitorCardInfo.CardName == "Tomb Protector")
                    {
                        competitorCardInfo.GetComponent<CardInformation>().CardHealth = (int.Parse(competitorCardInfo.GetComponent<CardInformation>().CardHealth) + CheckUndeadCards()).ToString();
                        competitorCardInfo.GetComponent<CardInformation>().SetInformation();

                    }
                    else if (competitorCardInfo.CardName == "Necropolis Acolyte")
                    {

                        GameObject randomCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];

                        randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) +2).ToString();
                        randomCard.GetComponent<CardInformation>().SetInformation();
                    }
                    else if (competitorCardInfo.CardName == "Desert Bowman")
                    {
                        GameObject randomCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];

                        randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) -1).ToString();
                        randomCard.GetComponent<CardInformation>().SetInformation();
                    }
                    else if (competitorCardInfo.CardName == "Sun Charioteer")
                    {
                        if (usedCards.Length > 0)
                        {
                            // 'Sun Charioteer' kart�n�n bulundu�u kart� bul
                            GameObject SelectedUsedCard = usedCards.FirstOrDefault(card => card.GetComponent<CardInformation>().CardName == "Sun Charioteer");

                            if (SelectedUsedCard != null)
                            {
                                // Vurulan kart�n indexini bul
                                int selectedIndex = Array.IndexOf(usedCards, SelectedUsedCard);
                                Debug.Log("Sun Charioteer " + competitorCardInfo.CardName + " kart�n� se�ti");
                                int damage = competitorCardInfo.CardDamage;

                                // Sa�daki karta hasar ver
                                if (selectedIndex + 1 < usedCards.Length)
                                {
                                    GameObject rightCard = usedCards[selectedIndex + 1];
                                    CardInformation rightCardInfo = rightCard.GetComponent<CardInformation>();

                                    rightCardInfo.CardHealth = (int.Parse(rightCardInfo.CardHealth) - damage).ToString();
                                    rightCardInfo.SetInformation();

                                    if (int.Parse(rightCardInfo.CardHealth) <= 0)
                                    {
                                        Destroy(rightCard);
                                    }

                                    Debug.Log("Sun Charioteer " + rightCardInfo.CardName + " kart�na " + damage + " hasar verdi (sa�daki kart)");
                                }

                                // Soldaki karta hasar ver
                                if (selectedIndex - 1 >= 0)
                                {
                                    GameObject leftCard = usedCards[selectedIndex - 1];
                                    CardInformation leftCardInfo = leftCard.GetComponent<CardInformation>();

                                    leftCardInfo.CardHealth = (int.Parse(leftCardInfo.CardHealth) - damage).ToString();
                                    leftCardInfo.SetInformation();

                                    if (int.Parse(leftCardInfo.CardHealth) <= 0)
                                    {
                                        Destroy(leftCard);
                                    }

                                    Debug.Log("Sun Charioteer " + leftCardInfo.CardName + " kart�na " + damage + " hasar verdi (soldaki kart)");
                                }
                            }
                            else
                            {
                                Debug.Log("Sun Charioteer kart� bulunamad�.");
                            }
                        }
                    }
                    else if (competitorCardInfo.CardName == "Royal Mummy*")
                    {
                        GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                        foreach (var card in AllMyCard)
                        {
                            card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) - 3).ToString();
                            card.GetComponent<CardInformation>().SetInformation();

                        }
                    }

                    
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
                            Debug.Log("Lightning Storm " + targetCardInfo.CardName + " kart�n� yok etti");
                        }
                        else
                        {
                            Debug.Log("Lightning Storm " + targetCardInfo.CardName + " kart�na " + damage + " hasar verdi");
                        }
                    }
                    else
                    {
                        Debug.Log("Lightning Storm sald�r� yap�cak kart bulamad�.");
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
                        Debug.Log("Olympian Favor " + selectedCardInfo.CardName + " kart�na 2 hasar verdi");
                        selectedCardInfo.CardHealth = (int.Parse(selectedCardInfo.CardHealth) - 2).ToString();
                        selectedCardInfo.SetInformation();

                        if (int.Parse(selectedCardInfo.CardHealth) <= 0)
                        {
                            Destroy(selectedCard);
                            Debug.Log("Olympian Favor " + selectedCardInfo.CardName + " kart�n� yok etti");
                        }
                    }
                    else if (selectedCard.CompareTag("CompetitorCard") && selectedCardInfo.CardName != "Olympian Favor")
                    {
                        Debug.Log("Olympian Favor " + selectedCardInfo.CardName + " kart�na 2 can verdi");
                        selectedCardInfo.CardHealth = (int.Parse(selectedCardInfo.CardHealth) + 2).ToString();
                        selectedCardInfo.SetInformation();
                    }
                }
                else
                {
                    Debug.Log("Olympian Favor etkinle�tirildi ancak tahtada se�ilebilecek bir kart bulunamad�.");
                }

                Debug.Log("Olympian Favor kullan�ld�.");
            }



            else if (cardInfo.CardName == "Golden Fleece")
            {
                GameObject[] CompetitorCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
                if (CompetitorCard.Length > 0)
                {

                    GameObject selectedCard = CompetitorCard[UnityEngine.Random.Range(0, CompetitorCard.Length)];
                    CardInformation CardInfo = selectedCard.GetComponent<CardInformation>();

                    string maxHealth = CardInfo.MaxHealth; // Eski sa�l�k de�erini al
                    CardInfo.CardHealth = maxHealth.ToString();
                    CardInfo.SetInformation();

                    Debug.Log("Golden Fleece kullan�ld�: " + CardInfo.CardName + " kart�n�n sa�l��� eski de�erine geri y�klendi.");
                }
                else
                {
                    Debug.LogWarning("Golden Fleece etkinle�tirmek i�in uygun kart bulunamad�.");
                }
            }


            else if (cardInfo.CardName == "Labyrinth Maze")
            {
                GameObject[] usedCards = GameObject.FindGameObjectsWithTag("UsedCard");
                // Se�ilen aral�kta kartlar� bul
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
                        Debug.Log("Labyrinth Maze " + card.name + " kart�n� desteye ekledi.");
                    }
                    else
                    {
                        Destroy(card);
                        Debug.Log("Labyrinth Maze " + card.name + " kart�n� yok etti.");
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
                        // Sa�l�k bilgisini ikiye katlayacak Coroutine'i ba�lat
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

                        Debug.Log("Horseback Archery " + CardInfo.CardName + " kart�na +2 can ekledi.");
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
                        Debug.Log("�evresine 2 can verdi");
                    }

                    // Tur bitti�inde eklenen canlar� geri almak i�in Coroutine ba�lat
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

                // 1 tur bekleyip hasar� geri al
                StartCoroutine(RemoveTemporaryDamage(cardInfos));
            }

            else if (cardInfo.CardName == "Eternal Steppe�s Whisper")
            {
                GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
            _TutorialPlayerController.SteppeAmbush = true;
            if (competitorCards.Length > 0)
                {
                    // Rastgele bir rakip kart� se�
                    GameObject selectedCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                    CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

                    if (selectedCardInfo != null)
                    {
                        // Kart� 1 tur boyunca sald�r�lardan koru
                        selectedCardInfo.IsImmuneToAttacks = true;
                        Debug.Log("Eternal Steppe�s Whisper etkinle�tirildi: " + selectedCardInfo.CardName + " kart� 1 tur boyunca sald�r�lardan korundu.");

                        // Kart�n sald�r�dan korunma durumunu geri almak i�in Coroutine ba�lat
                        StartCoroutine(RemoveImmunityAfterOneTurn(selectedCardInfo));
                    }

                }
                else
                {
                    Debug.LogWarning("Eternal Steppe�s Whisper etkinle�tirmek i�in uygun kart bulunamad�.");
                }
            }


        else if (cardInfo.CardName == "Da Vinci�s Blueprint")
        {
            _TutorialPlayerController.CreateAnCompetitorCard();

            
            if (UnityEngine.Random.Range(0, 5) == 0)
            {
                GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");

                if (competitorCards.Length > 0)
                {
                   
                    GameObject selectedCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                    CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

                    
                    selectedCardInfo.CardHealth = (int.Parse(selectedCardInfo.CardHealth) + 2).ToString();
                    selectedCardInfo.CardDamage += 3;
                    selectedCardInfo.SetInformation();

                    Debug.Log("Da Vinci�s Blueprint etkisi: " + selectedCardInfo.CardName + " kart�na +2 can ve +3 sald�r� ekledi.");
                }
                else
                {
                    Debug.LogWarning("Da Vinci�s Blueprint etkisini uygulamak i�in uygun kart bulunamad�.");
                }
            }
        }
        else if (cardInfo.CardName == "Tabula Aeterna")
        {
            
            GameObject[] usedCards = GameObject.FindGameObjectsWithTag("UsedCard");

            if (usedCards.Length > 0)
            {
              
                GameObject selectedCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];
                CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

                
                Destroy(selectedCard);
                Debug.Log("Tabula Aeterna, " + selectedCardInfo.CardName + " kart�n� yok etti.");

               
               _TutorialPlayerController.CreateAnCard();
                Debug.Log("Tabula Aeterna, yok edilen " + selectedCardInfo.CardName + " kart�n� desteye tekrar ekledi.");
            }
            else
            {
                Debug.LogWarning("Tabula Aeterna etkinle�tirildi ancak tahtada uygun bir kart bulunamad�.");
            }
        }
        else if (cardInfo.CardName == "Rune Magic")
        {
            int randomEffect = UnityEngine.Random.Range(0, 3); // 0, 1 veya 2 aras�nda rastgele se�im yap

            if (randomEffect == 0)
            {
                // Rastgele bir UsedCard kart�na 2 hasar ver
                GameObject[] usedCards = GameObject.FindGameObjectsWithTag("UsedCard");
                if (usedCards.Length > 0)
                {
                    GameObject selectedCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];
                    CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

                    selectedCardInfo.CardHealth = (int.Parse(selectedCardInfo.CardHealth) - 2).ToString();
                    selectedCardInfo.SetInformation();
                    Debug.Log("Rune Magic, " + selectedCardInfo.CardName + " kart�na 2 hasar verdi.");

                    if (int.Parse(selectedCardInfo.CardHealth) <= 0)
                    {
                        Destroy(selectedCard);
                        Debug.Log("Rune Magic, " + selectedCardInfo.CardName + " kart�n� yok etti.");
                    }
                }
                else
                {
                    Debug.Log("Rune Magic etkisi: Tahtada UsedCard bulunamad�.");
                }
            }
            else if (randomEffect == 1)
            {
                // Rastgele bir CompetitorCard kart�na 2 can ver
                GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
                if (competitorCards.Length > 0)
                {
                    GameObject selectedCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                    CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

                    selectedCardInfo.CardHealth = (int.Parse(selectedCardInfo.CardHealth) + 2).ToString();
                    selectedCardInfo.SetInformation();
                    Debug.Log("Rune Magic, " + selectedCardInfo.CardName + " kart�na 2 can ekledi.");
                }
                else
                {
                    Debug.Log("Rune Magic etkisi: Tahtada CompetitorCard bulunamad�.");
                }
            }
            else if (randomEffect == 2)
            {
                _TutorialPlayerController.CreateAnCompetitorCard();
                Debug.Log("Rune Magic, desteye yeni bir kart ekledi.");
            }
        }
        else if (cardInfo.CardName == "Winter's Chill")
        {
            GameObject[] usedCards = GameObject.FindGameObjectsWithTag("UsedCard");
            if (usedCards.Length > 0)
            {
                // Rastgele bir UsedCard kart�n� se�
                GameObject selectedCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];
                CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

                // Se�ilen karta 3 hasar ver
                selectedCardInfo.CardHealth = (int.Parse(selectedCardInfo.CardHealth) - 3).ToString();
                selectedCardInfo.SetInformation();
                Debug.Log("Winter's Chill, " + selectedCardInfo.CardName + " kart�na 3 hasar verdi.");

                if (int.Parse(selectedCardInfo.CardHealth) <= 0)
                {
                    Destroy(selectedCard);
                    Debug.Log("Winter's Chill, " + selectedCardInfo.CardName + " kart�n� yok etti.");
                }

                // Yan�ndaki kartlara 1 hasar ver
                for (int i = 0; i < usedCards.Length; i++)
                {
                    if (usedCards[i] == selectedCard)
                    {
                        if (i > 0) // Soldaki kart
                        {
                            GameObject leftNeighbor = usedCards[i - 1];
                            CardInformation leftNeighborInfo = leftNeighbor.GetComponent<CardInformation>();
                            leftNeighborInfo.CardHealth = (int.Parse(leftNeighborInfo.CardHealth) - 1).ToString();
                            leftNeighborInfo.SetInformation();
                            Debug.Log("Winter's Chill, " + leftNeighborInfo.CardName + " kart�na 1 hasar verdi.");

                            if (int.Parse(leftNeighborInfo.CardHealth) <= 0)
                            {
                                Destroy(leftNeighbor);
                                Debug.Log("Winter's Chill, " + leftNeighborInfo.CardName + " kart�n� yok etti.");
                            }
                        }

                        if (i < usedCards.Length - 1) // Sa�daki kart
                        {
                            GameObject rightNeighbor = usedCards[i + 1];
                            CardInformation rightNeighborInfo = rightNeighbor.GetComponent<CardInformation>();
                            rightNeighborInfo.CardHealth = (int.Parse(rightNeighborInfo.CardHealth) - 1).ToString();
                            rightNeighborInfo.SetInformation();
                            Debug.Log("Winter's Chill, " + rightNeighborInfo.CardName + " kart�na 1 hasar verdi.");

                            if (int.Parse(rightNeighborInfo.CardHealth) <= 0)
                            {
                                Destroy(rightNeighbor);
                                Debug.Log("Winter's Chill, " + rightNeighborInfo.CardName + " kart�n� yok etti.");
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("Winter's Chill etkisi: Tahtada UsedCard bulunamad�.");
            }
        }
        else if (cardInfo.CardName == "Sleipnir's Gallop")
        {
            GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
            cardInfo.GetComponent<CardInformation>().Gallop = true;
            if (competitorCards.Length > 0)
            {
                // Rastgele bir CompetitorCard kart� se�
                GameObject selectedCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

                // Can puan�na 3 ekle
                int currentHealth = int.Parse(selectedCardInfo.CardHealth);
                currentHealth += 3;
                selectedCardInfo.CardHealth = currentHealth.ToString();

                // Sald�r� g�c�ne 3 ekle
                selectedCardInfo.CardDamage += 3;

                // Kart bilgilerini g�ncelle
                selectedCardInfo.SetInformation();

                Debug.Log("Sleipnir's Gallop, " + selectedCardInfo.CardName + " kart�na +3 can ve +3 sald�r� g�c� verdi.");
            }
            else
            {
                Debug.Log("Sleipnir's Gallop etkisi: Tahtada CompetitorCard bulunamad�.");
            }
        }
        else if (cardInfo.CardName == "Radioactive Fallout")
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
        }
        else if (cardInfo.CardName == "Scrap Shield")
        {
            GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
            
            if (competitorCards.Length > 0)
            {
                // Rastgele bir CompetitorCard kart� se�
                GameObject selectedCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

                // Can puan�na 3 ekle
                int currentHealth = int.Parse(selectedCardInfo.CardHealth);
                currentHealth += 3;
                selectedCardInfo.CardHealth = currentHealth.ToString();

            }
            
        }
       
        else if (cardInfo.CardName == "Survival Instincts")
        {
           cardInfo.isAttacked = false;
        }
        else if (cardInfo.CardName == "Bir dost minyona +2/+2 ")
        {
            GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
            if (competitorCards.Length > 0)
            {
                // Rastgele bir CompetitorCard kart� se�
                GameObject selectedCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                CardInformation selectedCardInfo = selectedCard.GetComponent<CardInformation>();

              
                int currentHealth = int.Parse(selectedCardInfo.CardHealth);
                currentHealth += 2;
                selectedCardInfo.CardHealth = currentHealth.ToString();

                selectedCardInfo.CardDamage += 2;

                selectedCardInfo.SetInformation();

            }
        }
        else if (cardInfo.CardName == "Plague of Locusts")
        {
            GameObject[] allUsedCards = GameObject.FindGameObjectsWithTag("UsedCard");

            foreach (var card in allUsedCards)
            {
                CardInformation targetCardInfo = card.GetComponent<CardInformation>();

                if (!string.IsNullOrEmpty(targetCardInfo.CardHealth))
                {
                    // Kart�n can puan�n� 1 azalt
                    int currentHealth = int.Parse(targetCardInfo.CardHealth);
                    currentHealth -= 1;
                    targetCardInfo.CardHealth = currentHealth.ToString();

                    // Kart bilgilerini g�ncelle
                    targetCardInfo.SetInformation();

                    if (currentHealth <= 0)
                    {
                        Destroy(card);
                        Debug.Log("Plague of Locusts, " + targetCardInfo.CardName + " kart�n� yok etti.");
                       
                    }
                    else
                    {
                        Debug.Log("Plague of Locusts, " + targetCardInfo.CardName + " kart�na 1 hasar verdi.");
                    }
                }
            }
        }
        else if (cardInfo.CardName == "Pyramid's Might")
        {
            GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");

            // Rastgele bir rakip kart se�
            if (competitorCards.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, competitorCards.Length);
                CardInformation selectedCardInfo = competitorCards[randomIndex].GetComponent<CardInformation>();

                selectedCardInfo.CardHealth = (int.Parse(selectedCardInfo.CardHealth) + 4).ToString();
                selectedCardInfo.CardDamage += 4; // Sald�r� hasar�n� art�r
                selectedCardInfo.SetInformation();

                Debug.Log("Pyramid's Might, " + selectedCardInfo.CardName + " kart�na 4 can ve 4 sald�r� eklendi.");

                // Yan�ndaki kartlara 1 can ve 1 sald�r� ekle
                foreach (GameObject competitorCard in competitorCards)
                {
                    if (competitorCard != selectedCardInfo.gameObject) // Se�ilen kart d���nda
                    {
                        CardInformation cardInfoNeighbor = competitorCard.GetComponent<CardInformation>();
                        if (cardInfoNeighbor != null)
                        {
                            cardInfoNeighbor.CardHealth = (int.Parse(cardInfoNeighbor.CardHealth) + 1).ToString();
                            cardInfoNeighbor.CardDamage += 1; // Yan�ndaki karta 1 sald�r� art�r
                            cardInfoNeighbor.SetInformation();
                            Debug.Log("Yan�ndaki karta 1 can ve 1 sald�r� eklendi: " + cardInfoNeighbor.CardName);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Pyramid's Might etkinle�tirildi, ancak rakip kart bulunamad�.");
            }
        }


    }
    public int CheckUndeadCards()
    {
        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
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
    private IEnumerator RemoveImmunityAfterOneTurn(CardInformation cardInfo)
        {
            yield return new WaitForSeconds(1f); // 1 tur beklemek i�in s�reyi ayarlay�n

            if (cardInfo != null)
            {
                cardInfo.IsImmuneToAttacks = false;
                Debug.Log(cardInfo.CardName + " kart� art�k sald�r�lardan etkilenebilir.");
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

            Debug.Log("Mongol Fury etkisi sona erdi ve +2 hasar geri al�nd�.");
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

            Debug.Log("Ger Defense b�y�s�n�n etkisi sona erdi ve +2 can geri al�nd�.");
        }
        IEnumerator DoubleHealthNextTurn(CardInformation CardInfo)
        {

            // Bir sonraki tura kadar bekle
            yield return new WaitForSeconds(1); // Turun bitti�ini varsayan bir senaryo
            Debug.Log("Bir sonraki tur bekleniyor");


            if (!string.IsNullOrEmpty(CardInfo.CardHealth) && int.Parse(CardInfo.CardHealth) > 0)
            {

                int currentHealth = int.Parse(CardInfo.CardHealth);
                CardInfo.CardHealth = (currentHealth * 2).ToString();
                CardInfo.SetInformation();

                Debug.Log("Divine Ascension " + CardInfo.CardName + " kart�n�n sa�l���n� ikiye katlad�");
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

                        float xPos = _TutorialPlayerController.DeckCardCount * 0.8f - 0.8f; // Kart�n X konumunu belirliyoruz
                        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kart�n pozisyonunu ayarl�yoruz

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

        // Kartlar�n belirli bir aral�kta olup olmad���n� kontrol eder
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

        // Kart�n bulundu�u alan�n indeksini d�nd�r�r
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

        // Belirli bir aral�kta rastgele bir kart se�er
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
                if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].transform.childCount == 0)
                {
                    EmptyIndexes.Add(i);
                    print("Eklendi");
                }

            }

            return EmptyIndexes[UnityEngine.Random.Range(0, EmptyIndexes.Count)];

        }

    public void DraugrWarrior()
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
    public void CreateCardFromBot()
        {

            _TutorialPlayerController.RemoveAnCompetitorCard();
            GameObject CardCurrent = Instantiate(_TutorialPlayerController.CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[FindEmptyAreaBox()].transform);
            CardCurrent.tag = "CompetitorCard";


            //CardCurrent.transform.localScale = new Vector3(1,1,0.04f);
            //CardCurrent.transform.localPosition = Vector3.zero;
            //CardCurrent.transform.localEulerAngles = new Vector3(45,0,180);

            _TutorialPlayerController.CreateInfoCard(CardCurrent);


            CardInformation cardInfo = CardCurrent.GetComponent<CardInformation>();

            // Kart�n bir b�y� kart� olup olmad���n� kontrol et ve b�y� etkisini uygula
            if (string.IsNullOrEmpty(cardInfo.CardHealth))
            {

                ApplySpellEffect(cardInfo);
                Debug.LogError("USSEEDD A SPEEELLL");
                Destroy(CardCurrent); // B�y� kartlar� sahnede kalmaz, uyguland�ktan sonra yok edilir
                GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");

                foreach (GameObject card in competitorCards)
                {
                    if (card.GetComponent<CardInformation>().CardName == "Automaton Apprentice")
                    {
                        card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                        card.GetComponent<CardInformation>().SetInformation();
                        Debug.Log("Automaton Apprentice'in CardHealth de�eri 1 art�r�ld�.");

                    }
                    if (card.GetComponent<CardInformation>().CardName == "Anatomist of the Unknown")
                    {
                        GameObject randomCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];

                        randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                        randomCard.GetComponent<CardInformation>().SetInformation();
                        Debug.Log(randomCard.GetComponent<CardInformation>().CardName + "+2 can ald�");


                    }
                }
            }



        }
        int brokkAndSindriTurnCounter = 0;
        IEnumerator Waiter()
        {
            yield return new WaitForSeconds(1);
            Debug.Log("SIRA SENDE");

        GameObject[] competitorCards = GameObject.FindGameObjectsWithTag("CompetitorCard");
        GameObject[] usedCards = GameObject.FindGameObjectsWithTag("UsedCard");
        bool anatomicalInsightActive = false;
        foreach (GameObject card in competitorCards)
        {

            if (card.GetComponent<CardInformation>().CardName == "Scrapyard Engineer")
            {
                GameObject randomCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                randomCard.GetComponent<CardInformation>().CardDamage += 2;
                randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                randomCard.GetComponent<CardInformation>().SetInformation();

            }
            if (card.GetComponent<CardInformation>().CardName == "Scrap Collector")
            {
                GameObject randomCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                randomCard.GetComponent<CardInformation>().CardDamage += 1;
                randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                randomCard.GetComponent<CardInformation>().SetInformation();
                Debug.Log(randomCard.GetComponent<CardInformation>().CardName + "+1 can ve damage kazand�");

            }
            if (card.GetComponent<CardInformation>().CardName == "Grand Cannon")
            {
                GameObject randomCard = usedCards[UnityEngine.Random.Range(0, usedCards.Length)];

                randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) - 2).ToString();
                randomCard.GetComponent<CardInformation>().SetInformation();
                Debug.Log(randomCard.GetComponent<CardInformation>().CardName + "-2 hasar ald�");


            }
            if (card.GetComponent<CardInformation>().CardName == "Eques Automaton")
            {
                card.GetComponent<CardInformation>().SetMaxHealth();
            }

            if (card.GetComponent<CardInformation>().CardName == "Dwarven Blacksmith")
            {
                GameObject randomCard = competitorCards[UnityEngine.Random.Range(0, competitorCards.Length)];
                randomCard.GetComponent<CardInformation>().CardDamage += 2;
                randomCard.GetComponent<CardInformation>().SetInformation();
                Debug.Log(randomCard.GetComponent<CardInformation>().CardName + "+2 damage kazand�");
            }
            if (card.GetComponent<CardInformation>().CardName == "Brokk and Sindri")
            {
                brokkAndSindriTurnCounter++;
                if (brokkAndSindriTurnCounter == 3)
                {

                    for (int i = 7; i < 14; i++)
                    {
                        if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount > 0)
                        {

                            GameObject cardInArea = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.GetChild(0).gameObject;
                            CardInformation cardInfo = cardInArea.GetComponent<CardInformation>();

                            // Kart�n sa�l�k de�eri string olarak tutuldu�undan d�n��t�r�p g�ncelleme
                            cardInfo.CardHealth = (int.Parse(cardInfo.CardHealth) - 2).ToString();
                            cardInfo.SetInformation(); // G�ncellenmi� bilgiyi uygulama

                            Debug.Log(cardInArea.name + " kart�na 2 hasar verildi.");

                        }
                    }

                    Debug.Log("Bu kart 3. turda da burada!");
                }
            }
            if (card.GetComponent<CardInformation>().CardName == "Thor")
            {
                Debug.Log("Thor kart� bulundu, t�m UsedCard'lara 1 hasar veriliyor...");

                foreach (GameObject usedCard in usedCards)
                {
                    CardInformation usedCardInfo = usedCard.GetComponent<CardInformation>();


                    usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - 1).ToString();
                    usedCardInfo.SetInformation();

                }
            }
            if (card.GetComponent<CardInformation>().CardName == "Radiated Hulk")
            {
                Debug.Log("Radiated Hulk kart� bulundu, t�m UsedCard'lara 1 hasar veriliyor...");

                foreach (GameObject usedCard in usedCards)
                {
                    CardInformation usedCardInfo = usedCard.GetComponent<CardInformation>();


                    usedCardInfo.CardHealth = (int.Parse(usedCardInfo.CardHealth) - 1).ToString();
                    usedCardInfo.SetInformation();

                }
            }
            if (card.GetComponent<CardInformation>().CardName == "Anatomical Insight")
            {
                anatomicalInsightActive = true;
                Debug.Log("Anatomical Insight kart� aktif, UsedCard kartlar� bu tur 2 kat hasar alacak.");
            }
            if (card.GetComponent<CardInformation>().Gallop == true)
            {
                card.GetComponent<CardInformation>().Gallop = false;
              _TutorialPlayerController.DestroyAndCreateMyDeck(card);
            }
            if (card.GetComponent<CardInformation>().CardName == "Survival Instincts")
            {
                card.GetComponent<CardInformation>().isAttacked = true;
            }

            
        }
        if (anatomicalInsightActive)
        {
            foreach (GameObject usedCard in usedCards)
            {
                CardInformation usedCardInfo = usedCard.GetComponent<CardInformation>();
                int originalHealth = int.Parse(usedCardInfo.CardHealth);
                usedCardInfo.CardHealth = (originalHealth - 2 * 1).ToString(); // 2 kat hasar uygulan�yor
                usedCardInfo.SetInformation();
                Debug.Log(usedCard.name + " kart� Anatomical Insight nedeniyle 2 kat hasar ald�.");
            }
        }

        if (UnityEngine.Random.Range(0, 10) < 6)
            {
                CreateCardFromBot();

            }

            _TutorialPlayerController.BeginerFunction();

        }
    }

