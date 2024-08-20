using System;
using System.Collections.Generic;
using System.Reflection;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

public class AnubisCardFuns 
{
  
  public static void NecropolisAcolyteFun(GameObject selectedCard, PlayerController PC )
  {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;
        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.ForHeal = true;
        PC._CardProgress.OpenMyCardSign();
    }

    public static void DesertBowmanFun(GameObject selectedCard, PlayerController PC )
    {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;
    }

    public static void OsirisBannermanFun(GameObject selectedCard, PlayerController PC)
    {
        GameObject[] AllCard = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (var card in AllCard)
        {
            if(card.GetComponent<CardInformation>().CardName == "Osiris")
            {
                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 2).ToString();
                selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                PC.RefreshMyCard(index,
                    card.GetComponent<CardInformation>().CardHealth,
                    card.GetComponent<CardInformation>().HaveShield,
                    card.GetComponent<CardInformation>().CardDamage,
                    card.GetComponent<CardInformation>().DivineSelected,
                    card.GetComponent<CardInformation>().FirstTakeDamage,
                    card.GetComponent<CardInformation>().FirstDamageTaken,
                    card.GetComponent<CardInformation>().EternalShield);
            }
        }
    }

    public static void RoyalMummyFun(string attackername,  PlayerController PC)
    {
        GameObject[] AllCard = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (var card in AllCard)
        {
            int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
            card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) - 2).ToString();
            PC.GetComponent<PlayerController>().CreateTextAtTargetIndex(index, 2, true);
            if (int.Parse(card.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
            {

                PC.GetComponent<PlayerController>().DeleteMyCard(index);
                PC.GetComponent<PlayerController>().RefreshLog(-2, true, attackername, card.GetComponent<CardInformation>().CardName, Color.red);
            }
            else
                PC.GetComponent<PlayerController>().RefreshLog(-2, false, attackername, card.GetComponent<CardInformation>().CardName, Color.red);
        }
    }

    public static void BookoftheDeadFun(GameObject selectedCard, PlayerController PC)
    {
        PC.ManaCountText.text = PC.Mana.ToString() + "/10";
        PC.OwnManaBar.fillAmount = PC.Mana / 10f;
        PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
        PC.CompetitorManaCountText.text = (PC._GameManager.ManaCount) + "/10".ToString();
        PC.DeckCardCount--;

        PC.StackDeck();

        if (PC.PV.IsMine)
        {
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
            PC.PV.RPC("RefreshPlayersInformation", RpcTarget.All);
        }

        if (PC.DeadMyCardName == null || PC.DeadMyCardName.Count == 0)
        {
            Debug.LogWarning("Card name list is empty or null.");
            return;
        }

        AnubisCard anubisCard = new AnubisCard();
        HashSet<string> revivedCards = new HashSet<string>();
        int reviveCount = 0;

        while (reviveCount < 5 && revivedCards.Count < PC.DeadMyCardName.Count)
        {
            string targetCardName = PC.DeadMyCardName[UnityEngine.Random.Range(0, PC.DeadMyCardName.Count)];

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
                    PC.CreateSpecialCard(targetCardName, cardHealth, cardDamage, 0, index, true);
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
                        PC.CreateSpecialCard(targetCardName, cardHealth, cardDamage, 0, index, true);
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

        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        Debug.LogError("USSEEDD A SPEEELLL");
        
    }

    public static void TombProtectorFun(GameObject selectedCard ,PlayerController PC)
    {
        selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + PC.CheckUndeadCards()).ToString();
        selectedCard.GetComponent<CardInformation>().SetInformation();
    }

    public static void FalconEyedHunterFun(GameObject selectedCard,PlayerController PC)
    {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;

    }

    public static void CanopicPreserverFun(GameObject selectedCard, PlayerController PC)
    {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;
        PC._CardProgress.ForMyCard = true;
    }

    public static void ScrollofDeathFun(GameObject selectedCard, PlayerController PC)
    {
        PC.ManaCountText.text = PC.Mana.ToString() + "/10";
        PC.OwnManaBar.fillAmount = PC.Mana / 10f;
        PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
        PC.CompetitorManaCountText.text = (PC._GameManager.ManaCount) + "/10".ToString();
        PC.DeckCardCount--;

        PC.StackDeck();

        if (PC.PV.IsMine)
        {
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
            PC.PV.RPC("RefreshPlayersInformation", RpcTarget.All);
        }

        int createdCardCount = 0;

        for (int i = 7; i < 14 && createdCardCount < 2; i++)
        {
            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
            {
                PC.CreateSpecialCard("Mummy", "1", 1, 0, i, true);
                createdCardCount++;
            }
        }


        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        Debug.LogError("USSEEDD A SPEEELLL");
    }
    public static void SandstoneScribeFun(GameObject CardCurrent, PlayerController PC)
    {
        CardCurrent.tag = "Card";
        float xPos = PC.DeckCardCount * 0.8f - 0.8f; // Kartýn X konumunu belirliyoruz
        CardCurrent.transform.localPosition = new Vector3(xPos, 0, 0); // Kartýn pozisyonunu ayarlýyoruz
        CardCurrent.GetComponent<CardInformation>().CardName = "Scroll of Death";
        CardCurrent.GetComponent<CardInformation>().CardDes = "Scroll of Death POWWERRRRR!!!";
        CardCurrent.GetComponent<CardInformation>().CardHealth = "0";
        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
        CardCurrent.GetComponent<CardInformation>().CardMana = 2;
        CardCurrent.GetComponent<CardInformation>().SetInformation();
        PC.DeckCardCount++;
        PC.StackDeck();
        PC.StackCompetitorDeck();
    }

    public static void SunDiskRadianceFun(GameObject selectedCard, PlayerController PC)
    {
        PC.ManaCountText.text = PC.Mana.ToString() + "/10";
        PC.OwnManaBar.fillAmount = PC.Mana / 10f;
        PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
        PC.CompetitorManaCountText.text = (PC._GameManager.ManaCount) + "/10".ToString();
        PC.DeckCardCount--;

        PC.StackDeck();

        if (PC.PV.IsMine)
        {
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
            PC.PV.RPC("RefreshPlayersInformation", RpcTarget.All);
        }
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;
        PC._CardProgress.ForMyCard = true;



        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        Debug.LogError("USSEEDD A SPEEELLL");
    }


    public static void PlagueofLocustsFun(GameObject selectedCard, PlayerController PC)
    {
        PC.ManaCountText.text = PC.Mana.ToString() + "/10";
        PC.OwnManaBar.fillAmount = PC.Mana / 10f;
        PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
        PC.CompetitorManaCountText.text = (PC._GameManager.ManaCount) + "/10".ToString();
        PC.DeckCardCount--;

        PC.StackDeck();

        if (PC.PV.IsMine)
        {
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
            PC.PV.RPC("RefreshPlayersInformation", RpcTarget.All);
        }
        DamageToAlLOtherMinionsForLocust(PC);

        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void RiversBlessingFun(GameObject selectedCard, PlayerController PC)
    {
        PC.ManaCountText.text = PC.Mana.ToString() + "/10";
        PC.OwnManaBar.fillAmount = PC.Mana / 10f;
        PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
        PC.CompetitorManaCountText.text = (PC._GameManager.ManaCount) + "/10".ToString();
        PC.DeckCardCount--;

        PC.StackDeck();

        if (PC.PV.IsMine)
        {
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
            PC.PV.RPC("RefreshPlayersInformation", RpcTarget.All);
        }

        selectedCard.tag = "Card";

        List<GameObject> usedCards = new List<GameObject>();
        foreach (GameObject card in GameObject.FindGameObjectsWithTag("UsedCard"))
        {
            usedCards.Add(card);
        }

        if (usedCards.Count == 0)
        {
            PC.UsedSpell(selectedCard);
            Debug.LogWarning("No used cards found.");
            selectedCard.SetActive(false);
            selectedCard = null;
            PC.lastHoveredCard = null;

            Debug.LogError("USSEEDD A SPEEELLL");
            return;
        }
        int totalHealthToDistribute = 10;
        System.Random random = new System.Random();

        while (totalHealthToDistribute > 0)
        {
            int cardIndex = random.Next(usedCards.Count);
            int healthToAdd = random.Next(1, totalHealthToDistribute + 1);

            // Caný seçilen karta ekle
            CardInformation cardInfo = usedCards[cardIndex].GetComponent<CardInformation>();
            if (cardInfo != null)
            {
                int currentHealth = int.Parse(cardInfo.CardHealth);
                currentHealth += healthToAdd;
                cardInfo.CardHealth = currentHealth.ToString();
                cardInfo.SetInformation();
                int CurrentCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, usedCards[cardIndex].transform.parent.gameObject);
                PC.GetComponent<PlayerController>().RefreshMyCard(CurrentCardIndex, cardInfo.CardHealth, cardInfo.HaveShield, cardInfo.CardDamage, cardInfo.DivineSelected, cardInfo.FirstTakeDamage, cardInfo.FirstDamageTaken, cardInfo.EternalShield);
            }

            totalHealthToDistribute -= healthToAdd;
        }


        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void PyramidsMightFun(GameObject selectedCard, PlayerController PC)
    {
        PC.ManaCountText.text = PC.Mana.ToString() + "/10";
        PC.OwnManaBar.fillAmount = PC.Mana / 10f;
        PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
        PC.CompetitorManaCountText.text = (PC._GameManager.ManaCount) + "/10".ToString();
        PC.DeckCardCount--;

        PC.StackDeck();

        if (PC.PV.IsMine)
        {
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
            PC.PV.RPC("RefreshPlayersInformation", RpcTarget.All);
        }
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;
        PC._CardProgress.ForMyCard = true;

        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void ScalesofAnubisFun(GameObject selectedCard, PlayerController PC)
    {
        PC.ManaCountText.text = PC.Mana.ToString() + "/10";
        PC.OwnManaBar.fillAmount = PC.Mana / 10f;
        PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
        PC.CompetitorManaCountText.text = (PC._GameManager.ManaCount) + "/10".ToString();
        PC.DeckCardCount--;

        PC.StackDeck();

        if (PC.PV.IsMine)
        {
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
            PC.PV.RPC("RefreshPlayersInformation", RpcTarget.All);
        }
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;

        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        Debug.LogError("USSEEDD A SPEEELLL");
    }
    public static void GatesofDuatFun(GameObject selectedCard, PlayerController PC)
    {
        PC.ManaCountText.text = PC.Mana.ToString() + "/10";
        PC.OwnManaBar.fillAmount = PC.Mana / 10f;
        PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
        PC.CompetitorManaCountText.text = (PC._GameManager.ManaCount) + "/10".ToString();
        PC.DeckCardCount--;

        PC.StackDeck();

        if (PC.PV.IsMine)
        {
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
            PC.PV.RPC("RefreshPlayersInformation", RpcTarget.All);
        }
        PC.GatesofDuat();

        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void CanFirstRauntAttack(GameObject selectedCard)
    {
        selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
        Transform childTransform = selectedCard.transform;
        Transform green = childTransform.Find("Green");
        green.gameObject.SetActive(true);
    }

    public static void DamageToAlLOtherMinionsForLocust(PlayerController PC)
    {

        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKÝBÝN BÜTÜN KARTLARINI AL

        foreach (var Card in allTargets)
        {

            if (Card.GetComponent<CardInformation>().CardHealth != "") // MÝNÝYON OLUP OLMADIÐINI ÖÐREN - ÞARTI SAÐLIYORSA MÝNYONDUR.
            {
                int CurrentCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);

                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) - 1).ToString(); //  ÝKÝ DAMAGE VURUYOR
                PC.GetComponent<PlayerController>().RefreshUsedCard(CurrentCardIndex, Card.GetComponent<CardInformation>().CardHealth, Card.GetComponent<CardInformation>().CardDamage); // DAMAGE YÝYEN KARTIN BÝLGÝLERÝNÝ GÜNCELLE
                PC.GetComponent<PlayerController>().CreateTextAtTargetIndex(CurrentCardIndex, 1, false);

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
                        PC.CreateSpecialCard("Plague of Locusts", "1", 1, 0, index, true);
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
                            PC.CreateSpecialCard("Plague of Locusts", "1", 1, 0, index, true);
                            emptyBackCells.RemoveAt(randomIndex);
                        }
                        else
                        {
                            Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
                        }
                    }
                    PC.GetComponent<PlayerController>().DeleteAreaCard(CurrentCardIndex);
                    PC.GetComponent<PlayerController>().RefreshLog(-1, true, "Plague of Locusts", Card.GetComponent<CardInformation>().CardName, Color.red);
                }
                else
                    PC.GetComponent<PlayerController>().RefreshLog(-1, false, "Plague of Locusts", Card.GetComponent<CardInformation>().CardName, Color.red);


            }
        }
    }

}
