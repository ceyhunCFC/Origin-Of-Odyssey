using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GenghisCardFun 
{
    public static void CanFirstRauntAttack(GameObject selectedCard)
    {
        selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
        Transform childTransform = selectedCard.transform;
        Transform green = childTransform.Find("Green");
        green.gameObject.SetActive(true);
    }

    public static void MongolMessengerFun(GameObject selectedCard, PlayerController PC)
    {
        GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (GameObject card in mycards)
        {
            if (card.GetComponent<CardInformation>().CardName == "Steppe Warlord")
            {
                Debug.Log("SteppeWorld var can arttýrýldý");
                selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                selectedCard.GetComponent<CardInformation>().SetInformation();
                int cardindex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                PC.RefreshMyCard(cardindex,
                    selectedCard.GetComponent<CardInformation>().CardHealth,
                    selectedCard.GetComponent<CardInformation>().HaveShield,
                    selectedCard.GetComponent<CardInformation>().CardDamage,
                    selectedCard.GetComponent<CardInformation>().DivineSelected,
                    selectedCard.GetComponent<CardInformation>().FirstTakeDamage,
                    selectedCard.GetComponent<CardInformation>().FirstDamageTaken,
                    selectedCard.GetComponent<CardInformation>().EternalShield);
            }
        }
        PC.SpawnAndReturnGameObject();
    }

    public static void MongolArcherFun(GameObject selectedCard, PlayerController PC)
    {
        GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (GameObject card in mycards)
        {
            if (card.GetComponent<CardInformation>().CardName == "Steppe Warlord")
            {
                Debug.Log("SteppeWorld var can arttýrýldý");
                selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                selectedCard.GetComponent<CardInformation>().SetInformation();
                int cardindex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                PC.RefreshMyCard(cardindex,
                    selectedCard.GetComponent<CardInformation>().CardHealth,
                    selectedCard.GetComponent<CardInformation>().HaveShield,
                    selectedCard.GetComponent<CardInformation>().CardDamage,
                    selectedCard.GetComponent<CardInformation>().DivineSelected,
                    selectedCard.GetComponent<CardInformation>().FirstTakeDamage,
                    selectedCard.GetComponent<CardInformation>().FirstDamageTaken,
                    selectedCard.GetComponent<CardInformation>().EternalShield);
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
    public static void MongolShamanFun(GameObject selectedCard, PlayerController PC)
    {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.ForHealMongolShaman = true;
        PC._CardProgress.OpenMyCardSign();
    }
    public static void EagleHunterFun( PlayerController PC)
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

            PC.CreateSpecialCard("Eagle", "1", 2, 0, randomIndex, true);
        }
        else
        {
            Debug.LogWarning("No empty cells available to place the Eagle card.");
        }
    }

    public static void YurtBuilderFun( PlayerController PC)
    {
        if (PC.PV.IsMine)
        {
            PC.Mana += 1;

            PC.ManaCountText.text = PC.Mana.ToString() + "/10";
            PC.OwnManaBar.fillAmount = PC.Mana / 10f;
            PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
            PC.CompetitorManaCountText.text = (PC._GameManager.ManaCount) + "/10".ToString();

            Debug.Log("Mana bir arttýrýldý");
        }
    }

    public static void MarcoPoloFun( PlayerController PC)
    {
        PC.MarcoPolo();
    }

    public static void NomadicScoutFun( PlayerController PC)
    {
        PlayerController[] objects = PlayerController.FindObjectsOfType<PlayerController>();
        foreach (PlayerController obj in objects)
        {
            if (obj.name == "PlayerController(Clone)" && obj.GetComponent<PlayerController>().PV != PC.PV)
            {
                if (obj.GetComponent<PlayerController>().NomadicTactics == true)
                {
                    Debug.Log("Nomadic tactik sýrrý ortaya çýktý");
                    obj.GetComponent<PlayerController>().NomadicTactics = false;
                }
                if (obj.GetComponent<PlayerController>().SteppeAmbush == true)
                {
                    Debug.Log("Steppeambush sýrrý ortaya çýktý");
                    obj.GetComponent<PlayerController>().SteppeAmbush = false;
                }
            }
        }
    }

    public static void SteppeWarlordFun( PlayerController PC)
    {
        GameObject[] mycards = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (GameObject card in mycards)
        {
            if (card.GetComponent<CardInformation>().CardName == "Mongol Messenger" || card.GetComponent<CardInformation>().CardName == "Mongol Archer" || card.GetComponent<CardInformation>().CardName == "General Subutai")
            {
                Debug.Log("KartBulundu can arttýrýldý");
                card.GetComponent<CardInformation>().CardDamage += 2;
                card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                card.GetComponent<CardInformation>().SetInformation();
                int cardindex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
                PC.RefreshMyCard(cardindex,
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

    public static void GeneralSubutaiFun(GameObject selectedCard,PlayerController PC)
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
                Debug.Log("SteppeWorld var can arttýrýldý");
                selectedCard.GetComponent<CardInformation>().CardDamage += 2;
                selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                selectedCard.GetComponent<CardInformation>().SetInformation();
                int cardindex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                PC.RefreshMyCard(cardindex,
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

    public static void HorsebackArcheryFun(GameObject selectedCard, PlayerController PC)
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
        PC._CardProgress.HorsebackArchery();

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void GerDefenseFun(GameObject selectedCard, PlayerController PC)
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
        PC._CardProgress.ForMyCard = true;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void MongolFuryFun(GameObject selectedCard, PlayerController PC)
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
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.MongolFury();

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void AroundtheGreatWallFun(GameObject selectedCard, PlayerController PC)
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

        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AroundtheGreatWall();

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void EternalSteppesWhisperFun(GameObject selectedCard, PlayerController PC)
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
        PC._CardProgress.ForMyCard = true;

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void GodsBaneFun(GameObject selectedCard, PlayerController PC)
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
        PC.GodsBaneUsed = true;
        PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("GodsBane", RpcTarget.All, 2);          //rakibin kartýnýn manasýný 2 arttýrýr

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void SteppeAmbushFun(GameObject selectedCard, PlayerController PC)
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
        PC.SteppeAmbush = true;
        PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_SteppeAmbush", RpcTarget.Others, PC.SteppeAmbush);

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void NomadicTacticsFun(GameObject selectedCard, PlayerController PC)
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
        PC.NomadicTactics = true;
        PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_NomadicTactics", RpcTarget.Others, PC.NomadicTactics);

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

}
