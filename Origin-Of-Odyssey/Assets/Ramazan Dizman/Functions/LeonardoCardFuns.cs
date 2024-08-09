using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeonardoCardFuns 
{
    public static void CodexGuardianFun(GameObject selectedCard, PlayerController PC)
    {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
        selectedCard.GetComponent<CardInformation>().DivineSelected = true;
        PC.RefreshMyCard(index,
                    selectedCard.GetComponent<CardInformation>().CardHealth,
                    selectedCard.GetComponent<CardInformation>().HaveShield,
                    selectedCard.GetComponent<CardInformation>().CardDamage,
                    selectedCard.GetComponent<CardInformation>().DivineSelected,
                    selectedCard.GetComponent<CardInformation>().FirstTakeDamage,
                    selectedCard.GetComponent<CardInformation>().FirstDamageTaken,
                    selectedCard.GetComponent<CardInformation>().EternalShield);
    }

    public static void PisceanDiverFun(GameObject selectedCard)
    {
        selectedCard.GetComponent<CardInformation>().CanAttackBehind = true;
    }

    public static void OsirisBannermanFun(GameObject selectedCard, PlayerController PC)
    {
        GameObject[] AllCard = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (var card in AllCard)
        {
            if (card.GetComponent<CardInformation>().CardName == "Osiris")
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

    public static void DaVincisHelixEngineerFun( PlayerController PC)
    {
        GameObject spawnedObject = PC.SpawnAndReturnGameObject();

        if (spawnedObject.GetComponent<CardInformation>().CardHealth == "")
        {
            Debug.LogError("Helix SPEEEELLL YARATTTI ");
            PC.TalkCloud("Helix created a spell..");

            spawnedObject.GetComponent<CardInformation>().CardMana -=2;
            spawnedObject.GetComponent<CardInformation>().SetInformation();

        }
        else
        {
            Debug.LogError("Helix MÝNNYOONNNN YARATTTI ");
            PC.TalkCloud("Helix created a minion..");
        }
    }
    

    public static void VitruvianFirstbornFun()
    {
        for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++) // KENDÝ DESTEMÝZDEKÝ KARTLARI TEK TEK ÇAÐIR
        {

            if (GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().CardHealth == "")  // ÇAÐIRILAN KARTIN BÜYÜ MÜ OLDUÐU KONTROL ET
            {
                GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().CardMana--;
                GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().SetInformation();
            }
        }

    }

    public static void DaVincisGliderFun()
    {
        for (int i = 0; i < 14; i++)
        {

            var cell = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject;

            if (cell.transform.childCount != 0)
            {
                GameObject child = cell.transform.GetChild(0).gameObject;
                int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, child.transform.parent.gameObject);
                
                if(!child.activeSelf)
                {
                    child.SetActive(true);
                    Debug.Log("Gizli kart bulundu");
                }
                child.GetComponent<CardInformation>().SetInformation();

            }

        }
    }

    public static void MechanicalLionFun(GameObject selectedCard, PlayerController PC)
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
                    PC.RefreshMyCard(index,
                        selectedCard.GetComponent<CardInformation>().CardHealth, 
                        selectedCard.GetComponent<CardInformation>().HaveShield,
                        selectedCard.GetComponent<CardInformation>().CardDamage,
                        selectedCard.GetComponent<CardInformation>().DivineSelected,
                        selectedCard.GetComponent<CardInformation>().FirstTakeDamage,
                        selectedCard.GetComponent<CardInformation>().FirstDamageTaken,
                        selectedCard.GetComponent<CardInformation>().EternalShield);
                }
            }
        }
    }


    public static void DaVincisBlueprintFun(GameObject selectedCard, PlayerController PC)
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

        GameObject spawnedObject = PC.SpawnAndReturnGameObject();
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
                spawnedObject.GetComponent<CardInformation>().CardDamage += 2;;
                spawnedObject.GetComponent<CardInformation>().SetInformation();
            }
        }


        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void TabulaAeternaFun(GameObject selectedCard, PlayerController PC)
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

        GameObject[] cards = GameObject.FindGameObjectsWithTag("CompetitorCard");
        if (cards.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, cards.Length);
            GameObject randomCard = cards[randomIndex];
            int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, randomCard.transform.parent.gameObject);
            randomCard.GetComponent<CardInformation>().CardMana--;
            PC.DeleteAreaCard(TargetCardIndex);
            PC.DestroyAndCreateMyDeck(randomCard);
        }


        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }
    public static void ArtisticInspirationFun(GameObject selectedCard, PlayerController PC)
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


        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void AnatomicalInsightFun(GameObject selectedCard, PlayerController PC)
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
        PC.DoubleDamage = 2;



        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void MechanicalReinforcementFun(GameObject selectedCard, PlayerController PC)
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


        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void TomeofConfusionFun(GameObject selectedCard, PlayerController PC)
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


        selectedCard.SetActive(false);


        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

}
