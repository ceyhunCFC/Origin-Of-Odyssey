using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UI;


public class CardProgress : MonoBehaviourPunCallbacks
{

    
    public GameObject AttackerCard,TargetCard;
    public int TargetCardIndex;

    public bool SecoundTargetCard = false;
    CardInformation AttackerInfo = null;
    CardInformation TargetInfo = null;

    public bool ForMyCard = false;
    public bool ForHeal = false;
    public bool WindFury = true;

    private void Update()
    {
        if (AttackerCard == null)
            return;

        Debug.LogError("KART SECİMİ BEKLENİYORRR");

        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "There's a card in front of it.";


        if (Input.GetMouseButtonDown(0) && SecoundTargetCard == false) // KENDİ SALDIRI KARTIMIZI SEÇTİKTEN SONRA AKTİF OLUR 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("CompetitorCard"))
                {
                    TargetCard = hit.collider.gameObject; // SEÇİLEN RAKİP KART BUDUR


                    TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, hit.collider.gameObject.transform.parent.gameObject);
                    if (TargetCardIndex < 7 && TargetCardIndex >= 0)
                    {
                        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard");

                        foreach (GameObject target in allTargets)
                        {
                            if (target != TargetCard)
                            {
                                float distance = Vector3.Distance(TargetCard.transform.position, target.transform.position);
                                if (distance <= 0.80f)
                                {
                                    Vector3 directionToTarget = (target.transform.position - TargetCard.transform.position).normalized;

                                    float dotProductForward = Vector3.Dot(TargetCard.transform.up, directionToTarget);

                                    if (dotProductForward > 0.5f)
                                    {
                                        if (!AttackerCard.GetComponent<CardInformation>().CanAttackBehind)
                                        {
                                            Debug.Log("Önünde kart var");

                                            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "There's a card in front of it.";

                                            AttackerCard = null;
                                            TargetCard = null;
                                            TargetCardIndex = -1;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    FirstTargetCompotitorCard();


                }
                else if (hit.collider.gameObject.CompareTag("CompetitorHeroCard"))
                {
                    bool hasBackAreaCard = false;
                    for (int i = 0; i < 14; i++)
                    {
                        Transform area = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].transform;

                        if (area.childCount > 0)
                        {
                            hasBackAreaCard = true;
                        }
                    }

                    if (!hasBackAreaCard)
                    {
                        TargetCard = hit.collider.gameObject; // SEÇİLEN RAKİP KART BUDUR
                        StandartDamage(AttackerCard, TargetCard); // BİZİM KARTIMIZ VE SEÇİLEN RAKİP HERO GÖNDERİLİR
                    }
                    else
                    {
                        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "There are other cards ahead";
                        Debug.Log("Arka alanda kart bulundu, işlem iptal edildi.");
                        AttackerCard = null;
                        CloseEnemyAllCard();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && SecoundTargetCard == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("CompetitorCard"))
                {
                    TargetCard = hit.collider.gameObject;
                    TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, hit.collider.gameObject.transform.parent.gameObject);

                    Debug.LogError("İKİNCİ KART SEÇİLDİ");

                    Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Second Card Selected.";
                    SecondTargetCompotitorCard();




                }
                else if (hit.collider.gameObject.CompareTag("UsedCard"))
                {
                    TargetCard = hit.collider.gameObject;
                    TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, hit.collider.gameObject.transform.parent.gameObject);
                    Debug.LogError("İKİNCİ KART SEÇİLDİ");

                    Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Second Card Selected.";
                    SecondTargetUsedCard();

                }
            }
        }

    }

    void FirstTargetCompotitorCard()                    //standart rakip seçip saldırmakj için
    {
        AttackerCard.GetComponent<CardInformation>().isAttacked = true;
        Transform childTransform = AttackerCard.transform;
        Transform green = childTransform.Find("Green");
        if (green != null)
        {
            green.gameObject.SetActive(false);
        }
        Transform blue = childTransform.Find("Blue");
        if (blue != null)
        {
            blue.gameObject.SetActive(false);
        }
        CloseEnemyAllCard();
        if (TargetCard.GetComponent<CardInformation>().HaveShield == true && TargetCard.GetComponent<CardInformation>().FirstTakeDamage == true)
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.FirstTakeDamage = false;
            GetComponent<PlayerController>().RefreshCompotitorCard(TargetCardIndex, TargetInfo.FirstTakeDamage, TargetInfo.CardFreeze);
            Debug.LogError("Have Shield ");                 // aegis shild varsa saldıramaz

            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Have Shield.";


            AttackerCard = null;
            TargetCard = null;
            TargetCardIndex = -1;
            return;

        }
        if (TargetCard.GetComponent<CardInformation>().FirstDamageTaken == true && TargetCard.GetComponent<CardInformation>().CardName == "Pegasus Rider")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.FirstDamageTaken = false;
            Debug.LogError("Pegasus first damage shield ");                 //pegasus ilk saldırısında hasar almaz

            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Pegasus first damage shield.";


            AttackerCard = null;
            TargetCard = null;
            TargetCardIndex = -1;
            return;
        }
        if (TargetCard.GetComponent<CardInformation>().FirstDamageTaken == true && TargetCard.GetComponent<CardInformation>().CardName == "Odyssean Navigator")
        {
            Debug.LogError("Odyssean Navigator not attack yet ");                 //Odyssean Navigator saldırmadan saldırılamaz

            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Odyssean Navigator not attack yet.";

            AttackerCard = null;
            TargetCard = null;
            TargetCardIndex = -1;
            return;
        }
        if (TargetCard.GetComponent<CardInformation>().EternalShield == true)
        {
            Debug.Log("HaveEternalshield ");
            AttackerCard = null;
            TargetCard = null;
            TargetCardIndex = -1;
            return;
        }

        StandartDamage(AttackerCard, TargetCard); // BİZİM KARTIMIZ VE SEÇİLEN RAKİP KART GÖNDERİLİR 

    }

    void SecondTargetUsedCard()                     //kendi kartlarımı seçebilmek için
    {
        if (ForHeal)
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            if(AttackerInfo.CardName == "Mongol Shaman")
            {
                TargetInfo.CardHealth = TargetInfo.MaxHealth;
            }
            else if(AttackerInfo.CardName == "Necropolis Acolyte")
            {
                TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 2).ToString();
            }
            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.magenta);
            SecoundTargetCard = false;
            AttackerCard = null;
            ForHeal = false;
            CloseMyCardSign();
            return;
        }
        if (AttackerCard.GetComponent<CardInformation>().CardName == "Golden Fleece")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();

            GoldenFleece(TargetInfo);
            Debug.LogError("Golden Fleecee");

            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Golden Fleecee.";

        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Aegis Shield")
        {
            AttackerInfo = AttackerCard.GetComponent<CardInformation>();
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.HaveShield = true;

            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.yellow);
            

            
            Debug.LogError("Aegis Shieldddd");

            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Aegis Shieldddd.";

        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Olympian Favor")
        {
            AttackerInfo = AttackerCard.GetComponent<CardInformation>();
            TargetInfo = TargetCard.GetComponent<CardInformation>();

            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 2).ToString();
            TargetInfo.CardDamage += 2;
            TargetInfo.SetInformation();

            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            GetComponent<PlayerController>().RefreshLog(2, true, AttackerInfo.CardName, TargetInfo.CardName, Color.green);

            

            Debug.LogError("Olympiann");

            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Olympiann.";

        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Divine Ascention")
        {
            AttackerInfo = AttackerCard.GetComponent<CardInformation>();
            TargetInfo = TargetCard.GetComponent<CardInformation>();

            TargetInfo.DivineSelected = true;

            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.gray);

            

            Debug.LogError("Divine Ascention");

            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Divine Ascention.";

        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Ger Defense")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 2).ToString();
            TargetInfo.GerDefense = true;
            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);

            GerDefense();


            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Ger Defense.";

        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Eternal Steppe’s Whisper")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.EternalShield = true;
            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.clear);
            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Eternal Steppe’s Whisper";
            
        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Sleipnir’s Gallop")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.CardDamage += 3;
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 3).ToString();
            TargetInfo.Gallop = true;
            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Sun Disk Radiance")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.CardDamage += 3;
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 3).ToString();
            TargetInfo.SunDiskRadiance = true;
            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Sun Disk Radiance";
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Pyramid's Might")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.CardDamage += 4;
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 4).ToString();
            PyramidsMight();
            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Pyramid's Might";
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Canopic Preserver")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            AttackerInfo = AttackerCard.GetComponent<CardInformation>();
            AttackerInfo.CardDamage += TargetInfo.CardDamage;
            AttackerInfo.CardHealth = (int.Parse(AttackerInfo.CardHealth) + int.Parse(TargetInfo.CardHealth)).ToString();
            GetComponent<PlayerController>().DeleteMyCard(TargetCardIndex);
            RefreshMyCardDatas(AttackerInfo.HaveShield, AttackerInfo.CardDamage, AttackerInfo.DivineSelected, AttackerInfo.FirstTakeDamage, AttackerInfo.FirstDamageTaken, AttackerInfo.EternalShield);
            GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Properties Retrieved";
            CloseMyCardSign();
            ForMyCard = false;
            SecoundTargetCard = false;
            AttackerCard = null;
            TargetCard = null;
            TargetCardIndex = -1;
            return;
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Artistic Inspiration")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.ArtisticInspiration = true;
            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Artistic Inspiration";
        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Leonardo Da Vinci")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Leonardo Da Vinci";


            LeonardoCard leonardoCard = new LeonardoCard();

            for (int i = 0; i < leonardoCard.minions.Count; i++)
            {
                string cardName = TargetInfo.CardName;
                if (leonardoCard.minions[i].name == cardName &&
                    cardName != "Codex Guardian" &&
                    cardName != "Anatomist of the Unknown" &&
                    cardName != "Piscean Diver" &&
                    cardName != "Da Vinci's Helix Engineer")
                {
                    if (GetComponent<PlayerController>().AugmentCount >= 4)
                    {
                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 4).ToString();
                        TargetInfo.CardDamage += 4;
                        if(TargetInfo.CardName == "Eques Automaton")
                        {
                            TargetInfo.MaxHealth += 4;
                        }
                    }
                    else
                    {
                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 2).ToString();
                        TargetInfo.CardDamage += 2;
                        if (TargetInfo.CardName == "Eques Automaton")
                        {
                            TargetInfo.MaxHealth += 2;
                        }
                    }
                    GetComponent<PlayerController>().AugmentCount++;
                }
            }

            if (GetComponent<PlayerController>().PV.IsMine)
            {
                GetComponent<PlayerController>().SetMana(AttackerCard);
            }
            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            GameObject[] AllCard = GameObject.FindGameObjectsWithTag("UsedCard");
            foreach (var card in AllCard)
            {
                if (card.GetComponent<CardInformation>().CardName == "Organ Gun")
                {
                    GameObject[] cards = GameObject.FindGameObjectsWithTag("CompetitorCard");
                    if (cards.Length > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, cards.Length);
                        GameObject randomCard = cards[randomIndex];
                        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, randomCard.transform.parent.gameObject);
                        randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) - 3 * GetComponent<PlayerController>().DoubleDamage).ToString();
                        GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, randomCard.GetComponent<CardInformation>().CardHealth, randomCard.GetComponent<CardInformation>().CardDamage);
                        GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 3 * GetComponent<PlayerController>().DoubleDamage, false);
                        if (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
                        {

                            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                            GetComponent<PlayerController>().RefreshLog(-3 * GetComponent<PlayerController>().DoubleDamage, true, "Organ Gun", randomCard.GetComponent<CardInformation>().CardName, Color.red);
                        }
                        else
                            GetComponent<PlayerController>().RefreshLog(-3 * GetComponent<PlayerController>().DoubleDamage, false, "Organ Gun", randomCard.GetComponent<CardInformation>().CardName, Color.red);
                    }
                }
            }
            CloseMyCardSign();
            AttackerCard = null;
            TargetCard = null;
            SecoundTargetCard = false;
            TargetCardIndex = -1;
            GetComponent<PlayerController>().DoubleDamage = 1;
            return;
        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Scrap Shield")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.MaxHealth = (int.Parse(TargetInfo.MaxHealth) + 3).ToString();
            TargetInfo.CardHealth = TargetInfo.MaxHealth;
            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Scrap Shield";
        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Mutated Blood Sample")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            AttackerInfo = AttackerCard.GetComponent<CardInformation>();
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) -1 ).ToString();

            GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 1, true);
            if (int.Parse(TargetInfo.CardHealth) <= 0) 
            {

                GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                GetComponent<PlayerController>().RefreshLog(-1, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
            }
            else
            {
                GetComponent<PlayerController>().RefreshLog(-1, false, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
                TargetInfo.CardDamage += 2;
                TargetInfo.MutatedBlood = true;
                RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
                Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Scrap Shield";
            }
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Mechanical Reinforcement")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            AttackerInfo = AttackerCard.GetComponent<CardInformation>();
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + int.Parse(AttackerInfo.CardHealth)).ToString();
            TargetInfo.CardDamage += AttackerInfo.CardDamage;
            TargetInfo.SetInformation();
            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Mechanical Reinforcement";
        }
        Destroy(AttackerCard);
        CloseMyCardSign();
        ForMyCard = false;
        SecoundTargetCard = false;
        AttackerCard = null;
        TargetCard = null;
        TargetCardIndex = -1;
    }

    void SecondTargetCompotitorCard()                    //rakip karta saldırmak için
    {

        if (AttackerCard.GetComponent<CardInformation>().CardName == "Siren")
        {
            Siren(AttackerCard, TargetCard);

        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Lightning Bolt")
        {
            AttackerInfo = AttackerCard.GetComponent<CardInformation>();
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            LightningBolt(AttackerInfo, TargetInfo);
            Debug.LogError("LİGHTİNG BOLLLLTT");

            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "LIGHTING BOLLLLTT!";

        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Frost Giant")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.CardFreeze = true;
            GetComponent<PlayerController>().RefreshCompotitorCard(TargetCardIndex, TargetInfo.FirstTakeDamage, TargetInfo.CardFreeze);
            GetComponent<PlayerController>().RefreshLog(0, true, "Frost Giant", TargetInfo.CardName, Color.blue);
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Winter's Chill")
        {
            int damage = 3 + GetComponent<PlayerController>().SpellsExtraDamage;
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - damage).ToString();
            GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, damage, false);
            FreezeAround();
            if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
            {

                GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                GetComponent<PlayerController>().RefreshLog(-damage, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
            }
            else
                GetComponent<PlayerController>().RefreshLog(-damage, false, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Scales of Anubis")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
                GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Destroyed Card";
            }
            else
            {
                GetComponent<PlayerController>().ScalesOfAnubis(TargetCardIndex);
                GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
                Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Destroy and ReturnCard";
            }
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Falcon-Eyed Hunter")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 3).ToString();
            GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 3, false);
            if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
            {

                GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                GetComponent<PlayerController>().RefreshLog(-3, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
            }
            else
            {
                GetComponent<PlayerController>().RefreshLog(-3, false, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
                GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage);
            }
            AttackerCard.GetComponent<CardInformation>().isItFirstRaound = false;
            AttackerCard.GetComponent<CardInformation>().isAttacked = true;
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Wasteland Sniper")
        {
            AttackerInfo = AttackerCard.GetComponent<CardInformation>();
            TargetInfo = TargetCard.GetComponent<CardInformation>();

            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 2).ToString();
            GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 2, false);
            if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
            {

                GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                GetComponent<PlayerController>().RefreshLog(-2, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
            }
            else
            {
                GetComponent<PlayerController>().RefreshLog(-2, false, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
                GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage);
            }


            AttackerCard.GetComponent<CardInformation>().isItFirstRaound = false;
            AttackerCard.GetComponent<CardInformation>().isAttacked = true;
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Tome of Confusion")
        {
            AttackerInfo = AttackerCard.GetComponent<CardInformation>();
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            if (UnityEngine.Random.value <= 0.5f)
            {
                int randomChoice = UnityEngine.Random.Range(0, 2);

                if (randomChoice == 0)
                {
                    GameObject[] AllCompetitorCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
                    if (AllCompetitorCard.Length > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, AllCompetitorCard.Length);
                        GameObject selectedCard = AllCompetitorCard[randomIndex];

                        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, selectedCard.transform.parent.gameObject);

                        selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(AttackerInfo.CardHealth) - TargetInfo.CardDamage).ToString();
                        GetComponent<PlayerController>().RefreshUsedCard(index, selectedCard.GetComponent<CardInformation>().CardHealth, selectedCard.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                        GetComponent<PlayerController>().CreateTextAtTargetIndex(index, TargetInfo.CardDamage, false);
                    }
                }
                else
                {
                    GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
                    if (AllMyCard.Length > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, AllMyCard.Length);
                        GameObject selectedCard = AllMyCard[randomIndex];

                        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                        selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(AttackerInfo.CardHealth) - TargetInfo.CardDamage).ToString();
                        GetComponent<PlayerController>().RefreshUsedCard(index, selectedCard.GetComponent<CardInformation>().CardHealth, selectedCard.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                        GetComponent<PlayerController>().CreateTextAtTargetIndex(index, TargetInfo.CardDamage, true);
                    }
                }
            }
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Plague Carrier")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.CardDamage -= 2;
            TargetInfo.SetInformation();
            TargetInfo.PlagueCarrier = true;
            GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage);

        }
        else if (AttackerCard.GetComponent<CardInformation>().CardName == "Mystic Archer")
        {
            TargetInfo = TargetCard.GetComponent<CardInformation>();
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 2).ToString();
            GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 2, false);
            if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
            {

                GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                GetComponent<PlayerController>().RefreshLog(-2, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
            }
            else
            {
                GetComponent<PlayerController>().RefreshLog(-2, false, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
                GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage);
            }
            AttackerCard.GetComponent<CardInformation>().isItFirstRaound = false;
            AttackerCard.GetComponent<CardInformation>().isAttacked = true;
        }
        Destroy(AttackerCard);
        ForMyCard = false;
        SecoundTargetCard = false;
        AttackerCard = null;
        TargetCard = null;
        TargetCardIndex = -1;
        CloseEnemyAllCard();

    }


    public void StandartDamage(GameObject Attacker, GameObject Target) // HANGİ HASAR ŞEKLİ UYGULANACAĞI SEÇİLMELİDİR
    {
        if(GetComponent<PlayerController>().CompetitorMainCard == "Leonardo Da Vinci")
        {
            GameObject[] AllCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
            foreach (var card in AllCard)
            {
                if(card.GetComponent<CardInformation>().CardName == "Mirror Shield Automaton")
                {
                    AttackerInfo = Attacker.GetComponent<CardInformation>();
                    AttackerInfo.CardHealth = (int.Parse(AttackerInfo.CardHealth) - 1).ToString();
                    int attackerindex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Attacker.transform.parent.gameObject);
                    GetComponent<PlayerController>().RefreshUsedCard(attackerindex, AttackerInfo.CardHealth, AttackerInfo.CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                    GetComponent<PlayerController>().CreateTextAtTargetIndex(attackerindex, 1, false);

                    if (int.Parse(AttackerInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
                    {

                        GetComponent<PlayerController>().DeleteAreaCard(attackerindex);
                        GetComponent<PlayerController>().RefreshLog(-1, true, "Mirror Shield Automaton", AttackerInfo.CardName, Color.red);
                        AttackerCard = null;
                        TargetCard = null;
                        SecoundTargetCard = false;
                        TargetCardIndex = -1;
                        return;
                    }
                    else
                        GetComponent<PlayerController>().RefreshLog(-1, false, "Mirror Shield Automaton", AttackerInfo.CardName, Color.red);
                    
                }
            }
        }
        AttackerInfo = Attacker.GetComponent<CardInformation>();
        TargetInfo = Target.GetComponent<CardInformation>();
        if (GetComponent<PlayerController>().PV.IsMine)
        {
            GetComponent<PlayerController>().CheckNomadicTactics();
        }

        AttackerInfo.isAttacked = true;
        if (WindFury == true && AttackerInfo.CardName == "General Subutai" || WindFury == true && AttackerInfo.CardName == "Einherjar Champion" || WindFury == true && AttackerInfo.CardName == "Saxon Bowman")
        {
            AttackerCard.GetComponent<CardInformation>().isAttacked = false;
            WindFury = false;
        }

        if(TargetInfo.CardName == "Wasteland Giant")
        {
            GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
            if (AllMyCard.Length > 0)
            {
                foreach (var MyCard in AllMyCard)
                {
                    MyCard.GetComponent<CardInformation>().CardHealth = (int.Parse(MyCard.GetComponent<CardInformation>().CardHealth) - 3).ToString();
                    MyCard.GetComponent<CardInformation>().SetInformation();
                    int Index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, MyCard.transform.parent.gameObject);
                    GetComponent<PlayerController>().CreateTextAtTargetIndex(Index, 3, true);

                    if (int.Parse(MyCard.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
                    {

                        GetComponent<PlayerController>().DeleteMyCard(Index);
                        GetComponent<PlayerController>().RefreshLog(-3, true, TargetInfo.CardName,MyCard.GetComponent<CardInformation>().CardName , Color.red);
                    }
                    else
                        GetComponent<PlayerController>().RefreshLog(-3, false, TargetInfo.CardName, MyCard.GetComponent<CardInformation>().CardName, Color.red);
                    GetComponent<PlayerController>().RefreshMyCard(Index,
                                MyCard.GetComponent<CardInformation>().CardHealth,
                                MyCard.GetComponent<CardInformation>().HaveShield,
                                MyCard.GetComponent<CardInformation>().CardDamage,
                                MyCard.GetComponent<CardInformation>().DivineSelected,
                                MyCard.GetComponent<CardInformation>().FirstTakeDamage,
                                MyCard.GetComponent<CardInformation>().FirstDamageTaken,
                                MyCard.GetComponent<CardInformation>().EternalShield);
                    GetComponent<PlayerController>().DoubleDamage = 1;
                    AttackerCard = null;
                    TargetCard = null;
                    SecoundTargetCard = false;
                    TargetCardIndex = -1;
                    return;
                }
                
            }
        }
        
        if(TargetInfo.CardName == "Minotaur Labyrinth Keeper")
        {
            TargetInfo.CardDamage++;
            TargetInfo.SetInformation();
            GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage);
        }

        if (TargetInfo.CardName == "Nemean Lion")
        {
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 1).ToString();
            CheckMyCard();
            GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
            GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex,1, false);

            if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
            {

                GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                GetComponent<PlayerController>().RefreshLog(-1, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
            }
            else
                GetComponent<PlayerController>().RefreshLog(-1, false, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
            AttackerCard = null;
            TargetCard = null;
            SecoundTargetCard = false;
            TargetCardIndex = -1;
            return;
        }
        if(TargetInfo.CardName == "Tank of the Renaissance")
        {
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 1).ToString();
            CheckMyCard();
            GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
            GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 1, false);

            if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
            {

                GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                GetComponent<PlayerController>().RefreshLog(-1, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
            }
            else
                GetComponent<PlayerController>().RefreshLog(-1, false, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
            AttackerCard = null;
            TargetCard = null;
            SecoundTargetCard = false;
            TargetCardIndex = -1;
            return;
        }
        else if (TargetInfo.CardName == "Khan’s Envoy")
        {
            GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard");

            System.Random random = new System.Random();
            bool chooseCard = random.NextDouble() >= 0.5;

            if (allTargets.Length == 1)
            {
                TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
            }
            else
            {
                if (chooseCard)
                {
                    int randomIndex = random.Next(allTargets.Length);
                    GameObject selectedCard = allTargets[randomIndex];
                    Debug.Log("Randomly selected competitor card: " + selectedCard.GetComponent<CardInformation>().CardName);
                    TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, selectedCard.transform.parent.gameObject);
                    TargetInfo = selectedCard.GetComponent<CardInformation>();
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                }
                else
                {
                    Debug.Log("No card selected due to 50% chance.");
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                }
                CheckMyCard();
                RefreshCardDatas();
                AttackerCard = null;
                TargetCard = null;
                SecoundTargetCard = false;
                TargetCardIndex = -1;
                return;
            }

            
        }

        if (TargetCard.name == "CompetitorHeoCard(Clone)")
        {
            AttackerCard.GetComponent<CardController>().UsedCard(AttackerInfo.CardDamage, GetComponent<PlayerController>().PV.Owner.IsMasterClient); // HERO YA DAMAGE VURMA
            Debug.LogError("HEROYA DAMAGEEEEE");

            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Damage to the enemy!";


            if (GetComponent<PlayerController>().PV.IsMine)
            {
                GetComponent<PlayerController>().CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                GetComponent<PlayerController>().SetMana(AttackerCard);
            }
            GetComponent<PlayerController>().CreateTextHero(AttackerInfo.CardDamage);
            GetComponent<PlayerController>().RefreshLog(-AttackerInfo.CardDamage, false, AttackerInfo.CardName, GetComponent<PlayerController>().OwnMainCardText.text, Color.red);
            Transform transform = AttackerCard.transform;
            Transform Green = transform.Find("Green");
            Transform Blue = transform.Find("Blue");
            Blue.gameObject.SetActive(false);
            Green.gameObject.SetActive(false);
            AttackerCard = null;
            TargetCard = null;
            TargetCardIndex = -1;
            SecoundTargetCard = false;
            return;
        }
        else
        {
            switch (AttackerCard.GetComponent<CardInformation>().CardName)
            {

                case "Siren":
                    if (TargetInfo.CardDamage < 3)
                    {
                        GetComponent<CardProgress>().SecoundTargetCard = true;
                        Transform transform = AttackerCard.transform;
                        Transform Green = transform.Find("Green");
                        Transform Blue = transform.Find("Blue");
                        Blue.gameObject.SetActive(false);
                        Green.gameObject.SetActive(false);
                        AttackerCard = Target;
                        Debug.LogError("İLK KART SEÇİLDİ");

                        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "First card selected.";
                        return;

                    }
                    break;

                case "Nemean Lion":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    AttackerInfo.CardHealth = (int.Parse(AttackerInfo.CardHealth) - 1).ToString();
                    if (AttackerInfo.isAttacked || AttackerInfo.CardFreeze || AttackerInfo.isItFirstRaound)
                    {
                        Transform childTransform = AttackerCard.transform;
                        Transform green = childTransform.Find("Green");
                        Transform blue = childTransform.Find("Blue");
                        blue.gameObject.SetActive(false);
                        green.gameObject.SetActive(false);
                    }
                    CloseEnemyAllCard();

                    int mycardindex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject);
                    GetComponent<PlayerController>().CreateTextAtTargetIndex(mycardindex, 1, true);
                    GetComponent<PlayerController>().RefreshMyCard(mycardindex, AttackerInfo.CardHealth, AttackerInfo.HaveShield, AttackerInfo.CardDamage, AttackerInfo.DivineSelected, AttackerInfo.FirstTakeDamage, AttackerInfo.FirstDamageTaken, AttackerInfo.EternalShield);
                    if (int.Parse(AttackerInfo.CardHealth) <= 0)
                    {
                        GetComponent<PlayerController>().DeleteMyCard(mycardindex);
                        GetComponent<PlayerController>().RefreshLog(-1, true, TargetInfo.CardName, AttackerInfo.CardName, Color.red);
                    }
                    else
                        GetComponent<PlayerController>().RefreshLog(-1, false, TargetInfo.CardName, AttackerInfo.CardName, Color.red);
                    RefreshCardDatas();
                    AttackerCard = null;
                    TargetCard = null;
                    SecoundTargetCard = false;
                    TargetCardIndex = -1;
                    GetComponent<PlayerController>().DoubleDamage = 1;
                    return;

                case "Hydra":
                    //DamageCardsAround(1);
                    DamageAround(1);
                    break;


                case "Odyssean Navigator":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    if (GetComponent<PlayerController>().PV.IsMine)
                    {
                        int AttackerCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject);
                        AttackerCard.GetComponent<CardInformation>().FirstDamageTaken = false;
                        GetComponent<PlayerController>().RefreshMyCard(AttackerCardIndex,
                            AttackerCard.GetComponent<CardInformation>().CardHealth,
                            AttackerCard.GetComponent<CardInformation>().HaveShield,
                            AttackerCard.GetComponent<CardInformation>().CardDamage,
                            AttackerCard.GetComponent<CardInformation>().DivineSelected,
                            AttackerCard.GetComponent<CardInformation>().FirstTakeDamage,
                            AttackerCard.GetComponent<CardInformation>().FirstDamageTaken,
                            AttackerCard.GetComponent<CardInformation>().EternalShield);
                    }
                    break;

                case "Zeus":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    if (GetComponent<PlayerController>().PV.IsMine)
                    {
                        GetComponent<PlayerController>().SetMana(AttackerCard);
                    }
                    RefreshCardDatas();
                    AttackerCard = null;
                    TargetCard = null;
                    SecoundTargetCard = false;
                    TargetCardIndex = -1;
                    GetComponent<PlayerController>().DoubleDamage = 1;
                    return;
                case "Mongol Messenger":                                                                                  //genghis cards
                    if (GetComponent<PlayerController>().PV.IsMine)
                    {
                        GetComponent<PlayerController>().NomadsLand += 1;
                    }
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Mongol Archer":
                    if (GetComponent<PlayerController>().PV.IsMine)
                    {
                        GetComponent<PlayerController>().NomadsLand += 1;
                    }
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "General Subutai":
                    if (GetComponent<PlayerController>().PV.IsMine)
                    {
                        GetComponent<PlayerController>().NomadsLand += 1;
                    }
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Desert Bowman":
                    if(AttackerInfo.isItFirstRaound)
                    {
                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 1 * GetComponent<PlayerController>().DoubleDamage).ToString();
                        GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                        GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 1, false);

                        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
                        {

                            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                            GetComponent<PlayerController>().RefreshLog(-1, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
                        }
                        else
                            GetComponent<PlayerController>().RefreshLog(-1, false, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
                        AttackerCard = null;
                        TargetCard = null;
                        SecoundTargetCard = false;
                        TargetCardIndex = -1;
                        GetComponent<PlayerController>().DoubleDamage = 1;
                        return;
                    }
                    else
                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Sun Charioteer":
                    DamageAround(AttackerInfo.CardDamage);
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Chaos Scarab":
                    GetComponent<PlayerController>().SetActiveCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject));
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    RefreshCardDatas();
                    AttackerCard = null;
                    TargetCard = null;
                    SecoundTargetCard = false;
                    TargetCardIndex = -1;
                    return;
                case "Crypt Warden":
                    GetComponent<PlayerController>().SetActiveCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject));
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Automaton Duelist":
                    AttackerInfo.CardDamage += 1;
                    RefreshMyCardDatas(AttackerInfo.HaveShield, AttackerInfo.CardDamage, AttackerInfo.DivineSelected, AttackerInfo.FirstTakeDamage, AttackerInfo.FirstDamageTaken, AttackerInfo.EternalShield);
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Frost Wyrm Fafnir":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    TargetInfo.CardFreeze = true;
                    GetComponent<PlayerController>().RefreshCompotitorCard(TargetCardIndex,TargetInfo.FirstTakeDamage,TargetInfo.CardFreeze);
                    GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName,TargetInfo.CardName, Color.blue);
                    break;
                case "Piscean Diver":
                    GetComponent<PlayerController>().SetActiveCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject));
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Rebel Outcast":
                    GetComponent<PlayerController>().SetActiveCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject));
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Urban Ranger":
                    GetComponent<PlayerController>().SetActiveCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject));
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Shadow Assassin":
                    GetComponent<PlayerController>().SetActiveCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject));
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Elven Tracker":
                    GetComponent<PlayerController>().SetActiveCard(Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject));
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;
                case "Minor Glacial Elemental":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    TargetInfo.CardFreeze = true;
                    GetComponent<PlayerController>().RefreshCompotitorCard(TargetCardIndex, TargetInfo.FirstTakeDamage, TargetInfo.CardFreeze);
                    GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.blue);
                    break;
                default:
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage * GetComponent<PlayerController>().DoubleDamage).ToString();
                    break;

            }
        }
        if (TargetInfo.CardName == "Catacomb Guardian")
        {
            if (int.Parse(TargetInfo.CardHealth) >= 0)
            {
                TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 1).ToString();
                TargetInfo.SetInformation();
                GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
            }

        }

        if (GetComponent<PlayerController>().PV.IsMine)
        {
            GetComponent<PlayerController>().SetMana(AttackerCard);
        }
        GetComponent<PlayerController>().DoubleDamage = 1;
        CheckMyCard();
        RefreshCardDatas();
        AttackerCard = null;
        TargetCard = null;
        SecoundTargetCard = false;
        TargetCardIndex = -1;
    }

   
    public void SetAttackerCard(int AttackerCardIndex)
    {
        AttackerCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[AttackerCardIndex].transform.GetChild(0).gameObject;
        AttackerInfo=AttackerCard.GetComponent<CardInformation>();

        //if(AttackerCard.GetComponent<CardInformation>().isItFirstRaound)
        //{
        //    return;
        //}
        switch (AttackerCard.GetComponent<CardInformation>().CardName)
        {
            case "Lightning Bolt":
                EnemyAllCard();
                break;
            case "Olympian Favor":
                OpenMyCardSign();
                break;
            case "Aegis Shield":
                OpenMyCardSign();
                break;
            case "Golden Fleece":
                OpenMyCardSign();
                break;
            case "Divine Ascention":
                OpenMyCardSign();
                break;
            case "Mongol Shaman":
                if (AttackerCard.GetComponent<CardInformation>().isItFirstRaound)
                {
                    OpenMyCardSign();
                }
                else
                    EnemyAllCard();
                break;
            case "Ger Defense":
                OpenMyCardSign();
                break;
            case "Eternal Steppe’s Whisper":
                OpenMyCardSign();
                break;
            case "Sleipnir’s Gallop":
                OpenMyCardSign();
                break;
            case "Necropolis Acolyte":
                if (AttackerCard.GetComponent<CardInformation>().isItFirstRaound)
                {
                    OpenMyCardSign();
                }
                else
                    EnemyAllCard();
                break;
            case "Sun Disk Radiance":
                OpenMyCardSign();
                break;
            case "Pyramid's Might":
                OpenMyCardSign();
                break;
            case "Falcon-Eyed Hunter":
                if (AttackerCard.GetComponent<CardInformation>().isItFirstRaound)
                {
                    EnemyBackCard();
                }
                else
                    EnemyAllCard();
                break;
            case "Canopic Preserver":
                if (AttackerCard.GetComponent<CardInformation>().isItFirstRaound)
                {
                    OpenMyCardSign();
                }
                else
                    EnemyAllCard();
                break;
            case "Scrap Shield":
                OpenMyCardSign();
                break;
            case "Mutated Blood Sample":
                OpenMyCardSign();
                break;
            case "Mechanical Reinforcement":
                OpenMyCardSign();
                break;
            default:
                EnemyAllCard();
                break;
        }
    }

    public void SetMainAttackerCard(GameObject attackercard)
    {
        AttackerCard = attackercard;
        if(AttackerCard.GetComponent<CardInformation>().CardName=="Zeus")
        {
            EnemyAllCard();
        }
        else if(AttackerCard.GetComponent<CardInformation>().CardName == "Leonardo Da Vinci")
        {
            OpenMyCardSign();
        }
    }

    public void BattleableCard()
    {
        for (int i = 0; i < 14; i++)
        {
            Transform area = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].transform;

            if (area.childCount > 0)
            {
                CardInformation CurrentCardInfo = area.GetChild(0).GetComponent<CardInformation>();
                if (CurrentCardInfo != null && !CurrentCardInfo.isAttacked && !CurrentCardInfo.CardFreeze && !CurrentCardInfo.isItFirstRaound)
                {
                    if (CurrentCardInfo.CardName != "Chimera")
                    {
                        Transform green = area.GetChild(0).Find("Green");
                        if (green != null)
                        {
                            green.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void EnemyAllCard()
    {
        for (int i = 0; i < 14; i++)
        {
            Transform area = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].transform;

            if (area.childCount > 0)
            {
                Transform red = area.GetChild(0).Find("Red");
                if (red != null)
                {
                    red.gameObject.SetActive(true);
                }
            }
        }
    }

    public void EnemyBackCard()
    {
        for (int i = 0; i < 7; i++)
        {
            Transform area = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].transform;

            if (area.childCount > 0)
            {
                Transform red = area.GetChild(0).Find("Red");
                if (red != null)
                {
                    red.gameObject.SetActive(true);
                }
            }
        }
    }

    public void CloseEnemyAllCard()
    {
        for (int i = 0; i < 14; i++)
        {
            Transform area = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].transform;

            if (area.childCount > 0)
            {
                Transform red = area.GetChild(0).Find("Red");
                if (red != null)
                {
                    red.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ResetAllSign()
    {
        for (int i = 0; i < 14; i++)
        {
            Transform area = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].transform;

            if (area.childCount > 0)
            {
                Transform green = area.GetChild(0).Find("Green");
                if (green != null)
                {
                    green.gameObject.SetActive(false);
                }
                Transform yellow = area.GetChild(0).Find("Yellow");
                if (yellow != null)
                {
                    yellow.gameObject.SetActive(false);
                }
                Transform blue = area.GetChild(0).Find("Blue");
                if (blue != null)
                {
                    blue.gameObject.SetActive(false);
                }
            }
        }
        for (int i = 0; i < 14; i++)
        {
            Transform area = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].transform;

            if (area.childCount > 0)
            {
                Transform red = area.GetChild(0).Find("Red");
                if (red != null)
                {
                    red.gameObject.SetActive(false);
                }
            }
        }
    }

    public void CloseMyCardSign()
    {
        for (int i = 0; i < 14; i++)
        {
            Transform area = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].transform;
            if (area.childCount > 0)
            {
                Transform yellow = area.GetChild(0).Find("Yellow");
                if (yellow != null)
                {
                    yellow.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OpenMyCardSign()
    {
        for (int i = 0; i < 14; i++)
        {
            Transform area = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].transform;
            if (area.childCount > 0)
            {
                Transform yellow = area.GetChild(0).Find("Yellow");
                if (yellow != null)
                {
                    yellow.gameObject.SetActive(true);
                }
            }
        }
    }

    public void CloseBlueSign()
    {
        for (int i = 0; i < 14; i++)
        {
            Transform area = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].transform;
            if (area.childCount > 0)
            {
                Transform Blue = area.GetChild(0).Find("Blue");
                if (Blue != null)
                {
                    Blue.gameObject.SetActive(false);
                }
            }
        }
    }



    public void DamageAround(int damage)
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard");

        foreach (GameObject target in allTargets)
        {
            if (target != TargetCard)
            {
                float distance = Vector3.Distance(TargetCard.transform.position, target.transform.position);
                if (distance <= 1f)
                {
                    Vector3 directionToTarget = (target.transform.position - TargetCard.transform.position).normalized;

                    float dotProductRight = Vector3.Dot(TargetCard.transform.right, directionToTarget);
                    float dotProductLeft = Vector3.Dot(-TargetCard.transform.right, directionToTarget);

                    if (dotProductRight > 0.5f || dotProductLeft > 0.5f )
                    {
                        Debug.Log("Nearobject" + " " +target.GetComponent<CardInformation>().CardName);
                        target.GetComponent<CardInformation>().CardHealth = (int.Parse(target.GetComponent<CardInformation>().CardHealth) - damage).ToString(); 
                        int NearbyCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, target.transform.parent.gameObject);

                        GetComponent<PlayerController>().CreateTextAtTargetIndex(NearbyCardIndex, damage, false);
                        

                        GetComponent<PlayerController>().RefreshUsedCard(NearbyCardIndex, target.GetComponent<CardInformation>().CardHealth, target.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

                        if (int.Parse(target.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
                        {

                            GetComponent<PlayerController>().DeleteAreaCard(NearbyCardIndex);
                            GetComponent<PlayerController>().RefreshLog(-damage, true, "Hydra", target.GetComponent<CardInformation>().CardName, Color.red);

                        }
                        else
                            GetComponent<PlayerController>().RefreshLog(-damage, false, "Hydra", target.GetComponent<CardInformation>().CardName, Color.red);
                    }
                }
            }
        }

    }

    void FreezeAround()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard");

        foreach (GameObject target in allTargets)
        {
            if (target != TargetCard)
            {
                float distance = Vector3.Distance(TargetCard.transform.position, target.transform.position);
                if (distance <= 1f)
                {
                    Vector3 directionToTarget = (target.transform.position - TargetCard.transform.position).normalized;

                    float dotProductRight = Vector3.Dot(TargetCard.transform.right, directionToTarget);
                    float dotProductLeft = Vector3.Dot(-TargetCard.transform.right, directionToTarget);

                    if (dotProductRight > 0.5f || dotProductLeft > 0.5f)
                    {
                        Debug.Log("Nearobject" + " " + target.GetComponent<CardInformation>().CardName);
                        target.GetComponent<CardInformation>().CardHealth = (int.Parse(target.GetComponent<CardInformation>().CardHealth) - 1).ToString();
                        int NearbyCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, target.transform.parent.gameObject);

                        GetComponent<PlayerController>().CreateTextAtTargetIndex(NearbyCardIndex, 1, false);
                        target.GetComponent<CardInformation>().CardFreeze = true;
                        GetComponent<PlayerController>().RefreshCompotitorCard(NearbyCardIndex, target.GetComponent<CardInformation>().FirstTakeDamage, target.GetComponent<CardInformation>().CardFreeze);

                        GetComponent<PlayerController>().RefreshUsedCard(NearbyCardIndex, target.GetComponent<CardInformation>().CardHealth, target.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

                        if (int.Parse(target.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
                        {

                            GetComponent<PlayerController>().DeleteAreaCard(NearbyCardIndex);
                            GetComponent<PlayerController>().RefreshLog(-1, true, "Hydra", target.GetComponent<CardInformation>().CardName, Color.red);

                        }
                        else
                            GetComponent<PlayerController>().RefreshLog(-1, false, "Hydra", target.GetComponent<CardInformation>().CardName, Color.red);
                    }
                }
            }
        }
        
    }

    public void FreezeAllEnemyMinions(string name)
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKİBİN BÜTÜN KARTLARINI AL

       
        foreach (var Card in allTargets)
        {
          
            if (Card.GetComponent<CardInformation>().CardHealth!="") // MİNİYON OLUP OLMADIĞINI ÖĞREN - ŞARTI SAĞLIYORSA MİNYONDUR.
            {
                int CardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);
                Card.GetComponent<CardInformation>().CardFreeze = true;
                GetComponent<PlayerController>().RefreshCompotitorCard(CardIndex, Card.GetComponent<CardInformation>().FirstTakeDamage, Card.GetComponent<CardInformation>().CardFreeze);
                GetComponent<PlayerController>().RefreshLog(0, true, name, Card.GetComponent<CardInformation>().CardName,Color.blue);
            }
        }
    }

    public void DamageToAlLOtherMinions(int damage,string name)
    {

        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKİBİN BÜTÜN KARTLARINI AL

        foreach (var Card in allTargets)
        {

            if (Card.GetComponent<CardInformation>().CardHealth != "") // MİNİYON OLUP OLMADIĞINI ÖĞREN - ŞARTI SAĞLIYORSA MİNYONDUR.
            {
                int CurrentCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);

                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) - damage).ToString(); //  İKİ DAMAGE VURUYOR
                GetComponent<PlayerController>().RefreshUsedCard(CurrentCardIndex, Card.GetComponent<CardInformation>().CardHealth, Card.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                GetComponent<PlayerController>().CreateTextAtTargetIndex(CurrentCardIndex, damage, false);

                if (int.Parse(Card.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
                {

                    GetComponent<PlayerController>().DeleteAreaCard(CurrentCardIndex);
                    GetComponent<PlayerController>().RefreshLog(-damage, true, name, Card.GetComponent<CardInformation>().CardName, Color.red);
                }
                else
                    GetComponent<PlayerController>().RefreshLog(-damage, false, name, Card.GetComponent<CardInformation>().CardName, Color.red);

                if (TargetCardIndex != CurrentCardIndex) // ŞUANKİ KART İLK SEÇİLEN RAKİP MİNYON KARTI İLE AYNI DEĞİLSE ÇALIŞTIR
                {
                   

                }

            }
        }
    }

    public void FillWithHoplites()
    {

        for (int i = 7; i < 14; i++)
        {

            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount==0)
            {
                GetComponent<PlayerController>().CreateHoplitesCard(i);
            }
           
        }

    }

   

    void Siren(GameObject Attacker, GameObject Target)
    {
        Debug.LogError("SİREN HASAR VERDİ!");

        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "The siren did damage.";


        CardInformation AttackerInfo = Attacker.GetComponent<CardInformation>();
        CardInformation TargetInfo = Target.GetComponent<CardInformation>();

        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();

        GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth,TargetInfo.CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
        {

            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);

        }

    }

    void LightningBolt(CardInformation AttackerInfo, CardInformation TargetInfo)
    {
        int damage = 1;
        if (TargetInfo.CardHealth != "") // BU BİR MİNYON
        {
            if (GetComponent<PlayerController>().PV.IsMine)
            {
                damage += GetComponent<PlayerController>().SpellsExtraDamage;
                AttackerInfo.CardDamage = damage;
            }
            AttackerInfo.CardDamage = damage;
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - damage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR

            RefreshCardDatas();
        }
        else if (TargetCard.name == "CompetitorHeoCard(Clone)") // BU RAKİP HERO
        {
            if (GetComponent<PlayerController>().PV.IsMine)
            {
                damage += GetComponent<PlayerController>().SpellsExtraDamage;
                AttackerInfo.CardDamage = damage;
            }
            AttackerInfo.CardDamage = damage;
            AttackerCard.GetComponent<CardController>().UsedCard(damage, GetComponent<PlayerController>().PV.Owner.IsMasterClient); // HERO YA DAMAGE VURMA

            if (GetComponent<PlayerController>().PV.IsMine)
            {
                GetComponent<PlayerController>().CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);

            }
        }
        CloseEnemyAllCard();
        AttackerCard = null;

    }

    public void LightningStorm()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKİBİN BÜTÜN KARTLARINI AL

        foreach (var Card in allTargets)
        {

            if (Card.GetComponent<CardInformation>().CardHealth != "") // MİNİYON OLUP OLMADIĞINI ÖĞREN - ŞARTI SAĞLIYORSA MİNYONDUR.
            {
                int CurrentCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);
                int RandomDamage = UnityEngine.Random.Range(2,3);
                if(GetComponent<PlayerController>().PV.IsMine)
                {
                    RandomDamage += GetComponent<PlayerController>().SpellsExtraDamage;
                }
                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) - RandomDamage).ToString(); //  İKİ DAMAGE VURUYOR
                GetComponent<PlayerController>().RefreshUsedCard(CurrentCardIndex, Card.GetComponent<CardInformation>().CardHealth, Card.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                GetComponent<PlayerController>().CreateTextAtTargetIndex(CurrentCardIndex, RandomDamage, false);

                if (int.Parse(Card.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
                {

                    GetComponent<PlayerController>().DeleteAreaCard(CurrentCardIndex);
                    GetComponent<PlayerController>().RefreshLog(-RandomDamage, true, AttackerInfo.CardName, Card.GetComponent<CardInformation>().CardName, Color.red);
                }
                else
                {
                    GetComponent<PlayerController>().RefreshLog(-RandomDamage, false, AttackerInfo.CardName, Card.GetComponent<CardInformation>().CardName, Color.red);
                }

                if (TargetCardIndex != CurrentCardIndex) // ŞUANKİ KART İLK SEÇİLEN RAKİP MİNYON KARTI İLE AYNI DEĞİLSE ÇALIŞTIR
                {


                }

            }
        }
        AttackerCard = null;
    }


    void GoldenFleece(CardInformation Target)
    {
        Debug.LogError("Golden Fleece seçildi!");

       Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Golden Fleece selected.";


        if (Target.CardHealth != "") // BU BİR MİNYON
        {
            Target.CardHealth = (int.Parse(Target.CardHealth) + 5).ToString(); //5 can basıyor

            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected,TargetInfo.FirstTakeDamage,TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.magenta);
        }

        CloseMyCardSign();
        ForMyCard = false;
        SecoundTargetCard = false;
        AttackerCard = null;
        TargetCard = null;
        TargetCardIndex = -1;
    }

    public void FlamingCamel()
    {
        for (int i = 7; i < 14; i++)
        {
            var cell = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject;

            if (cell.transform.childCount != 0)
            {
                GameObject child = cell.transform.GetChild(0).gameObject;
                TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, child.transform.parent.gameObject);
                TargetInfo = child.GetComponent<CardInformation>();
                if (TargetInfo != null)
                {
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 2).ToString();
                    TargetInfo.SetInformation();
                    GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 2, true);


                    RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
                    if(int.Parse(TargetInfo.CardHealth) <= 0)
                    {
                        GetComponent<PlayerController>().DeleteMyCard(TargetCardIndex);
                        GetComponent<PlayerController>().RefreshLog(-2, true, "Flaming Camel", TargetInfo.CardName, Color.red);
                    }
                    else
                        GetComponent<PlayerController>().RefreshLog(-2, false, "Flaming Camel", TargetInfo.CardName, Color.red);
                }
            
            }

        }
    }

    public void CharceBrokandSindri()
    {
        for (int i = 7; i < 14; i++)
        {
            var cell = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject;

            if (cell.transform.childCount != 0)
            {
                GameObject child = cell.transform.GetChild(0).gameObject;
                TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, child.transform.parent.gameObject);
                TargetInfo = child.GetComponent<CardInformation>();
                if (TargetInfo != null)
                {
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 2).ToString();
                    TargetInfo.SetInformation();
                    GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                    GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 2, false);

                    if (int.Parse(TargetInfo.CardHealth) <= 0)
                    {
                        GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                        GetComponent<PlayerController>().RefreshLog(-2, true, "Brokk and Sindri", TargetInfo.CardName, Color.red);
                    }
                    else
                        GetComponent<PlayerController>().RefreshLog(-2, false, "Brokk and Sindri", TargetInfo.CardName, Color.red);
                }

            }

        }

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
            GetComponent<PlayerController>().CreateSpecialCard("Thor", "8", 8, 8, index, true);
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
                GetComponent<PlayerController>().CreateSpecialCard("Thor", "8", 8, 8, index, true);
                emptyBackCells.RemoveAt(randomIndex);
            }
            else
            {
                Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
            }
        }
    }

    public void KublaiKhan()
    {
        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (var card in AllOwnCards)
        {
            if(card.GetComponent<CardInformation>().CardName== "Mongol Messenger" || card.GetComponent<CardInformation>().CardName == "Mongol Archer" || card.GetComponent<CardInformation>().CardName == "General Subutai" )
            {
                TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                TargetInfo = card.GetComponent<CardInformation>();
                TargetInfo.CardDamage += 2;
                TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 2).ToString();
                TargetInfo.SetInformation();

                RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            }
        }
    }


    public void HorsebackArchery()
    {
        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("UsedCard");
        List<GameObject> SkirmisherCard = new List<GameObject>();
        foreach (var card in AllOwnCards)
        {
            if (card.GetComponent<CardInformation>().CardName == "Mongol Messenger" || card.GetComponent<CardInformation>().CardName == "Mongol Archer" || card.GetComponent<CardInformation>().CardName == "General Subutai")
            {
                SkirmisherCard.Add(card);
            }
        }

        if (SkirmisherCard.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, SkirmisherCard.Count);
            GameObject selectedCard = SkirmisherCard[randomIndex];

            TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
            TargetInfo = selectedCard.GetComponent<CardInformation>();

            Debug.Log(TargetInfo.CardName + "'in hasarı arttılırmıştır");
            TargetInfo.CardDamage += 2;
            TargetInfo.SetInformation();

            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.cyan);
        }
        else
        {
            Debug.Log("No Skirmisher cards found.");
        }
    }

    public void GerDefense()
    {
        GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");

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
                        TargetInfo = target.GetComponent<CardInformation>();

                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 2).ToString();
                        TargetInfo.SetInformation();
                        TargetInfo.GerDefense = true;
                        TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, target.transform.parent.gameObject);
                        RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
                        GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.black);
                        SecoundTargetCard = false;
                        ForMyCard = false;
                        AttackerCard = null;

                        Debug.Log("Updated health of " + TargetInfo.CardName + " to " + TargetInfo.CardHealth);
                        
                    }
                }
            }
        }
        CloseMyCardSign();
    }

    public void PyramidsMight()
    {
        GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");

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
                        TargetInfo = target.GetComponent<CardInformation>();

                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 1).ToString();
                        TargetInfo.CardDamage += 1;
                        TargetInfo.SetInformation();
                        TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, target.transform.parent.gameObject);
                        RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
                        GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.black);
                        SecoundTargetCard = false;
                        ForMyCard = false;
                        AttackerCard = null;

                    }
                }
            }
        }
        CloseMyCardSign();
    }

    public void MongolFury()
    {
        GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");

        foreach (GameObject card in mycards)
        {
            TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions,card.transform.parent.gameObject);
            TargetInfo=card.GetComponent<CardInformation>();

            TargetInfo.CardDamage +=2;
            TargetInfo.MongolFury = true;
            TargetInfo.SetInformation();
            GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.blue);
            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken,TargetInfo.EternalShield);
        }
        AttackerCard = null;
    }

    public void AroundtheGreatWall()
    {
        GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (GameObject card in mycards)
        {
            card.GetComponent<CardInformation>().CanAttackBehind = true;
            GetComponent<PlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.gray);
        }
        Destroy(AttackerCard);
        AttackerCard = null;
    }

    public void SteppeWarlord()
    {
        GameObject[] Competitorcard = GameObject.FindGameObjectsWithTag("CompetitorCard");

        foreach (GameObject card in Competitorcard)
        {
            TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, card.transform.parent.gameObject);
            TargetInfo = card.GetComponent<CardInformation>();

            if(TargetInfo.CardName == "Mongol Messenger" || TargetInfo.CardName == "Mongol Archer" || TargetInfo.CardName == "General Subutai")
            {
                TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 1).ToString();
                TargetInfo.CardDamage -= 1;

                RefreshCardDatas();

            }
            TargetInfo.SetInformation();
        }
    }



    void RefreshCardDatas()
    {
        CloseEnemyAllCard();
        GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage ); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
        GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, AttackerInfo.CardDamage,false);

        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
        {

            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
            GetComponent<PlayerController>().RefreshLog(-AttackerInfo.CardDamage, true,AttackerInfo.CardName,TargetInfo.CardName,Color.red);
        }
        else
            GetComponent<PlayerController>().RefreshLog(-AttackerInfo.CardDamage, false, AttackerInfo.CardName, TargetInfo.CardName,Color.red);

    }
    void RefreshMyCardDatas(bool haveshield,int damage,bool divineselected,bool firstdamage,bool firstdamagetaken,bool eternalshield)
    {
        TargetInfo.SetInformation();
        GetComponent<PlayerController>().RefreshMyCard(TargetCardIndex, TargetInfo.CardHealth,haveshield, damage, divineselected,firstdamage,firstdamagetaken,eternalshield); // Can alan kartı güncelle       
    }

    public void CheckMyCard()
    {
        AttackerInfo.CardHealth = (int.Parse(AttackerInfo.CardHealth) - TargetInfo.CardDamage).ToString();
        if (AttackerInfo.isAttacked || AttackerInfo.CardFreeze || AttackerInfo.isItFirstRaound)
        {
            Transform childTransform = AttackerCard.transform;
            Transform green = childTransform.Find("Green");
            Transform blue = childTransform.Find("Blue");
            blue.gameObject.SetActive(false);
            green.gameObject.SetActive(false);
        }
        CloseEnemyAllCard();
        int mycardindex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject);
        if (AttackerInfo.Invulnerable)
        {
            GetComponent<PlayerController>().CreateTextAtTargetIndex(mycardindex, 0, true);
            GetComponent<PlayerController>().RefreshMyCard(mycardindex, AttackerInfo.CardHealth, AttackerInfo.HaveShield, AttackerInfo.CardDamage, AttackerInfo.DivineSelected, AttackerInfo.FirstTakeDamage, AttackerInfo.FirstDamageTaken, AttackerInfo.EternalShield);
            GetComponent<PlayerController>().RefreshLog(0, true, TargetInfo.CardName, AttackerInfo.CardName, Color.red);
            AttackerInfo.Invulnerable = false;
        }
        else
        {
            GetComponent<PlayerController>().CreateTextAtTargetIndex(mycardindex, TargetInfo.CardDamage, true);
            GetComponent<PlayerController>().RefreshMyCard(mycardindex, AttackerInfo.CardHealth, AttackerInfo.HaveShield, AttackerInfo.CardDamage, AttackerInfo.DivineSelected, AttackerInfo.FirstTakeDamage, AttackerInfo.FirstDamageTaken, AttackerInfo.EternalShield);
            if (int.Parse(AttackerInfo.CardHealth) <= 0)
            {
                GetComponent<PlayerController>().DeleteMyCard(mycardindex);
                GetComponent<PlayerController>().RefreshLog(-TargetInfo.CardDamage, true, TargetInfo.CardName, AttackerInfo.CardName, Color.red);
            }
            else
                GetComponent<PlayerController>().RefreshLog(-TargetInfo.CardDamage, false, TargetInfo.CardName, AttackerInfo.CardName, Color.red);
        }
        
        
    }
}
