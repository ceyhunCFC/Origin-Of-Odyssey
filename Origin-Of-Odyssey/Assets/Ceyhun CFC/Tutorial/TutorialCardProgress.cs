using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UI;


public class TutorialCardProgress : MonoBehaviourPunCallbacks
{


    public GameObject AttackerCard, TargetCard;
    public int TargetCardIndex;

    public bool SecoundTargetCard = false;
    CardInformation AttackerInfo = null;
    CardInformation TargetInfo = null;

    public bool ForMyCard = false;
    public bool ForHealMongolShaman = false;
    public bool WindFury = true;
    public bool ForHeal = false;
    private void Update()
    {
        if (AttackerCard == null)
            return;

        Debug.LogError("KART SECİMİ BEKLENİYORRR");



        if (Input.GetMouseButtonDown(0) && SecoundTargetCard == false) // KENDİ SALDIRI KARTIMIZI SEÇTİKTEN SONRA AKTİF OLUR 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("CompetitorCard") && GetComponent<TutorialPlayerController>().Mana > 0)
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

                                if (distance <= 1.09f)
                                {
                                    Vector3 directionToTarget = (target.transform.position - TargetCard.transform.position).normalized;

                                    float dotProductForward = Vector3.Dot(TargetCard.transform.up, directionToTarget);

                                    if (dotProductForward == 0f)
                                    {
                                        if (!AttackerCard.GetComponent<CardInformation>().CanAttackBehind)
                                        {
                                            Debug.Log("Önünde kart var");

                                            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "There's a card in front of it.";

                                            AttackerCard = null;
                                            TargetCard = null;
                                            TargetCardIndex = -1;
                                            ResetAllSign();
                                            BattleableCard();
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    AttackerCard.GetComponent<CardInformation>().isAttacked = true;
                    if (TargetCard.GetComponent<CardInformation>().HaveShield == true && TargetCard.GetComponent<CardInformation>().FirstTakeDamage == true)
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.FirstTakeDamage = false;
                        // GetComponent<TutorialPlayerController>().RefreshCompotitorCard(TargetCardIndex, TargetInfo.FirstTakeDamage,TargetInfo.CardFreeze);
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
        // SALDIRI KARTLARI
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


                    if (AttackerCard.GetComponent<CardInformation>().CardName == "Siren")
                    {
                        Siren(AttackerCard, TargetCard);
                        ForMyCard = false;
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;

                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Lightning Bolt")
                    {
                        AttackerInfo = AttackerCard.GetComponent<CardInformation>();
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        LightningBolt(AttackerInfo, TargetInfo);
                        ForMyCard = false;
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;
                        Debug.LogError("LİGHTİNG BOLLLLTT");

                        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "LIGHTING BOLLLLTT!";

                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Frost Giant")
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.CardFreeze = true;
                        GetComponent<PlayerController>().RefreshCompotitorCard(TargetCardIndex, TargetInfo.FirstTakeDamage, TargetInfo.CardFreeze);
                        GetComponent<PlayerController>().RefreshLog(0, true, "Frost Giant", TargetInfo.CardName, Color.blue);
                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Winter's Chill")
                    {
                        int damage = 3 + GetComponent<PlayerController>().SpellsExtraDamage;
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - damage).ToString();
                        GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, damage, false, AttackerCard.GetComponent<CardInformation>().CardName);
                        FreezeAround();
                        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
                        {

                            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
                            GetComponent<PlayerController>().RefreshLog(-damage, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
                        }
                        else
                            GetComponent<PlayerController>().RefreshLog(-damage, false, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
                    }

                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Scales of Anubis")
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
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Falcon-Eyed Hunter")
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 3).ToString();
                        GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 3, false, AttackerCard.GetComponent<CardInformation>().CardName);
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
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Wasteland Sniper")
                    {
                        AttackerInfo = AttackerCard.GetComponent<CardInformation>();
                        TargetInfo = TargetCard.GetComponent<CardInformation>();

                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 2).ToString();
                        GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 2, false, AttackerCard.GetComponent<CardInformation>().CardName);
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
                   
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Plague Carrier")
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
                        GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 2, false, AttackerCard.GetComponent<CardInformation>().CardName);
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
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Ancient Librarian")
                    {
                        GetComponent<PlayerController>().RefreshEnemyAllBuff(TargetCardIndex);

                    }
                    Destroy(AttackerCard);
                    ForMyCard = false;
                    SecoundTargetCard = false;
                    AttackerCard = null;
                    TargetCard = null;
                    TargetCardIndex = -1;
                    CloseEnemyAllCard();
                }

                // BÜYÜ KARTLARI

                else if (hit.collider.gameObject.CompareTag("UsedCard"))
                {
                    TargetCard = hit.collider.gameObject;
                    TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, hit.collider.gameObject.transform.parent.gameObject);
                    Debug.LogError("İKİNCİ KART SEÇİLDİ");

                    Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Second Card Selected.";

                    if (ForHealMongolShaman)
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.CardHealth = TargetInfo.MaxHealth;
                        RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
                        GetComponent<TutorialPlayerController>().RefreshLog(0, true, "Mongol Shaman", TargetInfo.CardName, Color.magenta);
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        ForHealMongolShaman = false;
                        CloseMyCardSign();
                        ResetAllSign();
                        return;
                    }
                    if (ForHeal)
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        if (AttackerInfo.CardName == "Necropolis Acolyte")
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
                        GetComponent<TutorialPlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.yellow);
                        ForMyCard = false;
                        Destroy(AttackerCard);
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;

                        CloseMyCardSign();

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
                        GetComponent<TutorialPlayerController>().RefreshLog(2, true, AttackerInfo.CardName, TargetInfo.CardName, Color.green);

                        ForMyCard = false;
                        Destroy(AttackerCard);
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;

                        CloseMyCardSign();

                        Debug.LogError("Olympiann");

                        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Olympiann.";

                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Divine Ascention")
                    {
                        AttackerInfo = AttackerCard.GetComponent<CardInformation>();
                        TargetInfo = TargetCard.GetComponent<CardInformation>();

                        TargetInfo.DivineSelected = true;

                        RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
                        GetComponent<TutorialPlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.gray);

                        ForMyCard = false;
                        Destroy(AttackerCard);
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;

                        CloseMyCardSign();

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
                        GetComponent<TutorialPlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.clear);
                        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Eternal Steppe’s Whisper";
                        CloseMyCardSign();
                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Sleipnir’s Gallop")
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.CardDamage += 3;
                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 3).ToString();
                        TargetInfo.Gallop = true;
                        RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Sun Disk Radiance")
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.CardDamage += 3;
                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 3).ToString();
                        TargetInfo.SunDiskRadiance = true;
                        RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
                        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Sun Disk Radiance";
                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Pyramid's Might")
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.CardDamage += 4;
                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 4).ToString();
                        PyramidsMight();
                        RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
                        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Pyramid's Might";
                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Canopic Preserver")
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
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Artistic Inspiration")
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
                                    if (TargetInfo.CardName == "Eques Automaton")
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

                        GetComponent<PlayerController>().SetMana(AttackerCard);
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
                                    GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 3 * GetComponent<PlayerController>().DoubleDamage, false, AttackerCard.GetComponent<CardInformation>().CardName);
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
                        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 1).ToString();

                        GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 1, true, AttackerCard.GetComponent<CardInformation>().CardName);
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
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Mechanical Reinforcement")
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
                /* else if (hit.collider.gameObject.CompareTag("CompetitorHeroCard"))
                 {
                     TargetCard = hit.collider.gameObject;

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
                     }
                 }*/
            }
        }

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

    public void StandartDamage(GameObject Attacker, GameObject Target) // HANGİ HASAR ŞEKLİ UYGULANACAĞI SEÇİLMELİDİR
    {
        Debug.Log("eefwewf");
        AttackerInfo = Attacker.GetComponent<CardInformation>();
        TargetInfo = Target.GetComponent<CardInformation>();



        AttackerInfo.isAttacked = true;
        if (WindFury == true && AttackerInfo.CardName == "General Subutai")
        {
            Debug.Log("General Subutai");
            AttackerCard.GetComponent<CardInformation>().isAttacked = false;
            WindFury = false;
        }

        if (TargetInfo.CardName == "Nemean Lion")
        {
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 1).ToString();
            CheckMyCard();
            RefreshCardDatas();
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
        print("here");
        if (TargetCard.name == "TutorialCompetitorHeroCard 1(Clone)")
        {

            GetComponent<TutorialPlayerController>().CompetitorHealth -= AttackerInfo.CardDamage;
            GetComponent<TutorialPlayerController>().SetMana(AttackerCard);


            Debug.LogError("HEROYA DAMAGEEEEE");

            Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Damage to the enemy!";

            GetComponent<TutorialPlayerController>().CreateTextHero(AttackerInfo.CardDamage); //burasi

            if (AttackerInfo.isAttacked || AttackerInfo.CardFreeze || AttackerInfo.isItFirstRaound)
            {
                Transform childTransform = AttackerCard.transform;
                Transform green = childTransform.Find("Green");
                green.gameObject.SetActive(false);
            }
            CloseEnemyAllCard();

            GetComponent<TutorialPlayerController>().RefreshUI();
            // GetComponent<TutorialPlayerController>().RefreshLog(-AttackerInfo.CardDamage, false, AttackerInfo.CardName, GetComponent<TutorialPlayerController>().OwnDeck[0], Color.red);
            // AttackerCard.g
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
                case "Centaur Archer":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Minotaur Warrior":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Greek Hoplite":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Siren":
                    if (TargetInfo.CardDamage < 3)
                    {
                        GetComponent<CardProgress>().SecoundTargetCard = true;
                        AttackerCard = Target;
                        Debug.LogError("İLK KART SEÇİLDİ");

                        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "First card selected.";
                        return;

                    }
                    break;

                case "Nemean Lion":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Hydra":
                    //DamageCardsAround(1);
                    DamageAround(1);
                    break;

                case "Pegasus Rider":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Gorgon":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Chimera":
                    //BU KART BİR DAHA ÇALIŞMAYACAK
                    break;

                case "Athena":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Heracles":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Stormcaller":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Odyssean Navigator":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    AttackerCard.GetComponent<CardInformation>().FirstDamageTaken = false;
                    break;

                case "Oracle's Emissary":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Lightning Forger":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;
                case "Zeus":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Mongol Messenger":                                                                                  //genghis cards
                    GetComponent<TutorialPlayerController>().NomadsLand += 1;

                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Khan’s Envoy":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Mongol Archer":
                    GetComponent<TutorialPlayerController>().NomadsLand += 1;

                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Steppe Warlord":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Nomadic Scout":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Keshik Cavalry":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Mongol Shaman":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Eagle Hunter":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Yurt Builder":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Mongol Lancer":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Horse Breeder":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Flaming Camel":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Kublai Khan":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "General Subutai":
                    GetComponent<TutorialPlayerController>().NomadsLand += 1;
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Marco Polo":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Genghis":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                default:
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
            }
        }


        //   TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR -- Centaur Archer - Minotaur Warrior - Greek Hoplite


        /*if (TargetInfo.CardDamage < 3)
       {
           SecoundTargetCard = true;
           AttackerCard = Target;              -- RAKİBİN 3 DAMAGEDEN KÜÇÜK OLAN BİR MİNYONU SEÇİLİR VE BAŞKA BİR RAKİP MİNYONA HASAR VERİLİR. - Sirens
           Debug.LogError("İLK KART SEÇİLDİ");
       }*/


        /* if (TargetInfo.CardName == "Nemean Lion")
        {
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 1).ToString(); // SADECE BİR DAMAGE VURUYOR -- Nemean Lion 

        }*/


        //DamageCardsAround(1); // VURULAN KARTIN ETRAFINDAKİ KARTLARA DA HASAR VERİYOR. -- Hydra

        /*if (TargetInfo.CardName == "Pegasus Rider" && TargetInfo.FirstTakeDamage==false)
      {
          TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // İLK ALDIĞI HASARI KARTA HASAR VERMEZ -- Pegasus Rider
      }
      else
      {
          TargetInfo.FirstTakeDamage = false;
      }*/


        //FreezeAllEnemyMinions(); // RAKİBİN BÜTÜN MİNYONLARINI DONDURUR -- Gorgon

        //DamageToAlLOtherMinions(); //  BÜTÜN MİNYONLARA İKİ HASAR VERİR. -- Chimera

        //FillWithHoplites(); // ON SIRAYI DOLDURUR -- Athena

        /*  LightningBolt(AttackerInfo,TargetInfo);  // Bir minyon ya da hero seçecek, ona hasar verecek
          return;*/

        // LightningStorm(); //  rakip minyonlara 2 yada 3 hasar verecek - Lightning Storm

        GetComponent<TutorialPlayerController>().SetMana(AttackerCard);
        CheckMyCard();
        RefreshCardDatas();
        AttackerCard = null;
        TargetCard = null;
        SecoundTargetCard = false;
        TargetCardIndex = -1;
        /*if(AttackerCard.GetComponent<CardInformation>().CardName=="Siren")
        {
            if (TargetInfo.CardDamage < 3)
            {
                GetComponent<CardProgress>().SecoundTargetCard = true;
                AttackerCard = Target;
                Debug.LogError("İLK KART SEÇİLDİ");
            }
        } */
    }





    public void SetAttackerCard(int AttackerCardIndex)
    {
        Debug.LogError(AttackerCardIndex);
        AttackerCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[AttackerCardIndex].transform.GetChild(0).gameObject;
        AttackerInfo = AttackerCard.GetComponent<CardInformation>();
        Debug.LogError(AttackerCard);

        if (AttackerCard.GetComponent<CardInformation>().isItFirstRaound)
        {
            return;
        }

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
                    EnemyFrontlineAndHero();
                break;
            case "Ger Defense":
                OpenMyCardSign();
                break;
            case "Eternal Steppe’s Whisper":
                OpenMyCardSign();
                break;
            default:
                if (AttackerCard.GetComponent<CardInformation>().CanAttackBehind)
                {
                    EnemyAllCard();
                }
                else
                {
                    EnemyFrontlineAndHero();
                }
                break;
        }
    }

    public void SetMainAttackerCard(GameObject attackercard)
    {
        AttackerCard = attackercard;

        if (AttackerCard.GetComponent<CardInformation>().CardName == "Zeus")
        {
            EnemyAllCard();
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
                    Transform green = area.GetChild(0).Find("Green");
                    if (green != null)
                    {
                        green.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void EnemyFrontlineAndHero()
    {
        for (int i = 7; i < 14; i++)
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


    void DamageCardsAround(int DamageCount)
    {
        // Bütün nesneleri etikete göre bul
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard");
        List<GameObject> nearbyObjects = new List<GameObject>();

        foreach (GameObject target in allTargets)
        {
            // Mesafeyi hesapla
            float distance = Vector3.Distance(transform.position, target.transform.position);

            // Eğer mesafe belirlenen yarıçapın içindeyse, listeye ekle
            if (distance <= 10f)
            {
                nearbyObjects.Add(target);
            }
        }

        // Bulunan nesneleri işleme
        foreach (GameObject obj in nearbyObjects)
        {
            Debug.LogError("Nearby object found: " + obj.name);

            obj.GetComponent<CardInformation>().CardHealth = (int.Parse(obj.GetComponent<CardInformation>().CardHealth) - DamageCount).ToString(); // SADECE BİR DAMAGE VURUYOR -- Nemean Lion
            int NearbyCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, obj.transform.parent.gameObject);

            // GetComponent<TutorialPlayerController>().RefreshUsedCard(NearbyCardIndex, obj.GetComponent<CardInformation>().CardHealth, obj.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

            if (int.Parse(obj.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
            {

                GetComponent<TutorialPlayerController>().DeleteAreaCard(NearbyCardIndex);

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
                if (distance <= 0.4f)
                {
                    Vector3 directionToTarget = (target.transform.position - TargetCard.transform.position).normalized;

                    float dotProductRight = Vector3.Dot(TargetCard.transform.right, directionToTarget);
                    float dotProductLeft = Vector3.Dot(-TargetCard.transform.right, directionToTarget);

                    if (dotProductRight > 0.5f || dotProductLeft > 0.5f)
                    {
                        Debug.Log("Nearobject" + " " + target.GetComponent<CardInformation>().CardName);
                        target.GetComponent<CardInformation>().CardHealth = (int.Parse(target.GetComponent<CardInformation>().CardHealth) - damage).ToString();
                        int NearbyCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, target.transform.parent.gameObject);

                        GetComponent<TutorialPlayerController>().CreateTextAtTargetIndex(NearbyCardIndex, 1, false, "Hydra");


                        // GetComponent<TutorialPlayerController>().RefreshUsedCard(NearbyCardIndex, target.GetComponent<CardInformation>().CardHealth, target.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

                        if (int.Parse(target.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
                        {

                            GetComponent<TutorialPlayerController>().DeleteAreaCard(NearbyCardIndex);
                            GetComponent<TutorialPlayerController>().RefreshLog(-1, true, "Hydra", target.GetComponent<CardInformation>().CardName, Color.red);

                        }
                        else
                            GetComponent<TutorialPlayerController>().RefreshLog(-1, false, "Hydra", target.GetComponent<CardInformation>().CardName, Color.red);
                    }
                }
            }
        }

    }

    public void FreezeAllEnemyMinions()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKİBİN BÜTÜN KARTLARINI AL


        foreach (var Card in allTargets)
        {

            if (Card.GetComponent<CardInformation>().CardHealth != "") // MİNİYON OLUP OLMADIĞINI ÖĞREN - ŞARTI SAĞLIYORSA MİNYONDUR.
            {
                int CardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);
                Card.GetComponent<CardInformation>().CardFreeze = true;
                //  GetComponent<TutorialPlayerController>().RefreshCompotitorCard(CardIndex, Card.GetComponent<CardInformation>().FirstTakeDamage, Card.GetComponent<CardInformation>().CardFreeze);
                GetComponent<TutorialPlayerController>().RefreshLog(0, true, "Gorgon", Card.GetComponent<CardInformation>().CardName, Color.blue);
            }
        }
    }

    public void DamageToAlLOtherMinions()
    {

        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKİBİN BÜTÜN KARTLARINI AL


        foreach (var Card in allTargets)
        {

            if (Card.GetComponent<CardInformation>().CardHealth != "") // MİNİYON OLUP OLMADIĞINI ÖĞREN - ŞARTI SAĞLIYORSA MİNYONDUR.
            {
                int CurrentCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);

                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) - 2).ToString(); //  İKİ DAMAGE VURUYOR
                                                                                                                                               // GetComponent<TutorialPlayerController>().RefreshUsedCard(CurrentCardIndex, Card.GetComponent<CardInformation>().CardHealth, Card.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                GetComponent<TutorialPlayerController>().CreateTextAtTargetIndex(CurrentCardIndex, 2, false, name);

                if (int.Parse(Card.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
                {

                    GetComponent<TutorialPlayerController>().DeleteAreaCard(CurrentCardIndex);
                    GetComponent<TutorialPlayerController>().RefreshLog(-2, true, "Chimera ", Card.GetComponent<CardInformation>().CardName, Color.red);
                }
                else
                    GetComponent<TutorialPlayerController>().RefreshLog(-2, false, "Chimera ", Card.GetComponent<CardInformation>().CardName, Color.red);

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

            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
            {
                GetComponent<TutorialPlayerController>().CreateHoplitesCard(i);
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

        // GetComponent<TutorialPlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth,TargetInfo.CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
        {

            GetComponent<TutorialPlayerController>().DeleteAreaCard(TargetCardIndex);

        }

    }

    void LightningBolt(CardInformation AttackerInfo, CardInformation TargetInfo)
    {
        int damage = 1;
        if (TargetInfo.CardHealth != "") // BU BİR MİNYON
        {
            damage += GetComponent<TutorialPlayerController>().SpellsExtraDamage;
            AttackerInfo.CardDamage = damage;
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - damage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR

            RefreshCardDatas();
        }
        else if (TargetCard.name == "TutorialCompetitorHeroCard 1(Clone)") // BU RAKİP HERO
        {
            damage += GetComponent<TutorialPlayerController>().SpellsExtraDamage;
            AttackerInfo.CardDamage = damage;
            GetComponent<TutorialPlayerController>().CompetitorHealth -= AttackerInfo.CardDamage;

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
                int RandomDamage = UnityEngine.Random.Range(2, 3);
                RandomDamage += GetComponent<TutorialPlayerController>().SpellsExtraDamage;
                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) - RandomDamage).ToString(); //  İKİ DAMAGE VURUYOR
                                                                                                                                                          // GetComponent<TutorialPlayerController>().RefreshUsedCard(CurrentCardIndex, Card.GetComponent<CardInformation>().CardHealth, Card.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                GetComponent<TutorialPlayerController>().CreateTextAtTargetIndex(CurrentCardIndex, RandomDamage, false, AttackerInfo.CardName);

                if (int.Parse(Card.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
                {

                    GetComponent<TutorialPlayerController>().DeleteAreaCard(CurrentCardIndex);
                    GetComponent<TutorialPlayerController>().RefreshLog(-RandomDamage, true, AttackerInfo.CardName, Card.GetComponent<CardInformation>().CardName, Color.red);
                }
                else
                {
                    GetComponent<TutorialPlayerController>().RefreshLog(-RandomDamage, false, AttackerInfo.CardName, Card.GetComponent<CardInformation>().CardName, Color.red);
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

            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
            GetComponent<TutorialPlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.magenta);
        }

        CloseMyCardSign();

        ForMyCard = false;
        Destroy(AttackerCard);
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
                    GetComponent<TutorialPlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 2, true, "Flaming Camel");


                    RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
                    if (int.Parse(TargetInfo.CardHealth) <= 0)
                    {

                        GetComponent<TutorialPlayerController>().DeleteMyCard(TargetCardIndex);
                        GetComponent<TutorialPlayerController>().RefreshLog(-2, true, "Flaming Camel", TargetInfo.CardName, Color.red);
                    }
                    else
                        GetComponent<TutorialPlayerController>().RefreshLog(-2, false, "Flaming Camel", TargetInfo.CardName, Color.red);
                }

            }

        }
    }

    public void KublaiKhan()
    {
        GameObject[] AllOwnCards = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (var card in AllOwnCards)
        {
            if (card.GetComponent<CardInformation>().CardName == "Mongol Messenger" || card.GetComponent<CardInformation>().CardName == "Mongol Archer" || card.GetComponent<CardInformation>().CardName == "General Subutai")
            {
                TargetInfo = card.GetComponent<CardInformation>();
                TargetInfo.CardDamage += 2;
                TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) + 2).ToString();
                TargetInfo.SetInformation();

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
            GetComponent<TutorialPlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.cyan);
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
                if (distance <= 0.55f)
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
                        GetComponent<TutorialPlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.black);
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

    public void MongolFury()
    {
        GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");

        foreach (GameObject card in mycards)
        {
            TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
            TargetInfo = card.GetComponent<CardInformation>();

            TargetInfo.CardDamage += 2;
            TargetInfo.MongolFury = true;
            TargetInfo.SetInformation();
            GetComponent<TutorialPlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.blue);
            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected, TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken, TargetInfo.EternalShield);
        }
        AttackerCard = null;
    }

    public void AroundtheGreatWall()
    {
        GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (GameObject card in mycards)
        {
            card.GetComponent<CardInformation>().CanAttackBehind = true;
            GetComponent<TutorialPlayerController>().RefreshLog(0, true, AttackerInfo.CardName, TargetInfo.CardName, Color.gray);
        }
        AttackerCard = null;
    }

    public void SteppeWarlord()
    {
        GameObject[] Competitorcard = GameObject.FindGameObjectsWithTag("CompetitorCard");

        foreach (GameObject card in Competitorcard)
        {
            TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, card.transform.parent.gameObject);
            TargetInfo = card.GetComponent<CardInformation>();

            if (TargetInfo.CardName == "Mongol Messenger" || TargetInfo.CardName == "Mongol Archer" || TargetInfo.CardName == "General Subutai")
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
        GetComponent<TutorialPlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth, TargetInfo.CardDamage ); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
        GetComponent<TutorialPlayerController>().CreateTextAtTargetIndex(TargetCardIndex, AttackerInfo.CardDamage, false, AttackerInfo.CardName);

        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
        {

            GetComponent<TutorialPlayerController>().DeleteAreaCard(TargetCardIndex);
            GetComponent<TutorialPlayerController>().RefreshLog(-AttackerInfo.CardDamage, true, AttackerInfo.CardName, TargetInfo.CardName, Color.red);
        }
        else
            GetComponent<TutorialPlayerController>().RefreshLog(-AttackerInfo.CardDamage, false, AttackerInfo.CardName, TargetInfo.CardName, Color.red);

    }

    void RefreshMyCardDatas(bool haveshield, int damage, bool divineselected, bool firstdamage, bool firstdamagetaken, bool eternalshield)
    {
         GetComponent<TutorialPlayerController>().RefreshMyCard(TargetCardIndex, TargetInfo.CardHealth,haveshield, damage, divineselected,firstdamage,firstdamagetaken,eternalshield); // Can alan kartı güncelle       
    }

    public void CheckMyCard()
    {
        AttackerInfo.CardHealth = (int.Parse(AttackerInfo.CardHealth) - TargetInfo.CardDamage).ToString();
        if (AttackerInfo.isAttacked || AttackerInfo.CardFreeze || AttackerInfo.isItFirstRaound)
        {
            Transform childTransform = AttackerCard.transform;
            Transform green = childTransform.Find("Green");
            green.gameObject.SetActive(false);
            //Transform blue = childTransform.Find("Blue");
            //blue.gameObject.SetActive(false);
        }
        CloseEnemyAllCard();

        int mycardindex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject);
        GetComponent<TutorialPlayerController>().CreateTextAtTargetIndex(mycardindex, TargetInfo.CardDamage, true, TargetInfo.CardName);
        GetComponent<TutorialPlayerController>().RefreshMyCard(mycardindex, AttackerInfo.CardHealth, AttackerInfo.HaveShield, AttackerInfo.CardDamage, AttackerInfo.DivineSelected, AttackerInfo.FirstTakeDamage, AttackerInfo.FirstDamageTaken, AttackerInfo.EternalShield);
        if (int.Parse(AttackerInfo.CardHealth) <= 0)
        {
            GetComponent<TutorialPlayerController>().DeleteMyCard(mycardindex);
            GetComponent<TutorialPlayerController>().RefreshLog(-TargetInfo.CardDamage, true, TargetInfo.CardName, AttackerInfo.CardName, Color.red);
        }
        else
            GetComponent<TutorialPlayerController>().RefreshLog(-TargetInfo.CardDamage, false, TargetInfo.CardName, AttackerInfo.CardName, Color.red);
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
                    GetComponent<PlayerController>().CreateTextAtTargetIndex(TargetCardIndex, 2, false, "Brokk and Sindri");

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

                        GetComponent<PlayerController>().CreateTextAtTargetIndex(NearbyCardIndex, 1, false, "hydra");
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
    public void DamageToAlLOtherMinions(int damage, string name)
    {

        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKİBİN BÜTÜN KARTLARINI AL

        foreach (var Card in allTargets)
        {

            if (Card.GetComponent<CardInformation>().CardHealth != "") // MİNİYON OLUP OLMADIĞINI ÖĞREN - ŞARTI SAĞLIYORSA MİNYONDUR.
            {
                int CurrentCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);

                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) - damage).ToString(); //  İKİ DAMAGE VURUYOR
                GetComponent<PlayerController>().RefreshUsedCard(CurrentCardIndex, Card.GetComponent<CardInformation>().CardHealth, Card.GetComponent<CardInformation>().CardDamage); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE
                GetComponent<PlayerController>().CreateTextAtTargetIndex(CurrentCardIndex, damage, false, name);

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
}
