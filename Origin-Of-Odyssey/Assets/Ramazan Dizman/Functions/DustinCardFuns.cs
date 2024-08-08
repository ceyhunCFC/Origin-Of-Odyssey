using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DustinCardFuns
{
    public static void WastelandSniperFun(GameObject selectedCard, PlayerController PC)
    {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;

    }

    public static void ShockwaveImpulseFun(GameObject selectedCard, PlayerController PC)
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

        for (int i = 7; i < 14; i++)
        {
            var cardsAreaCreator = GameObject.Find("Area").GetComponent<CardsAreaCreator>();
            var backAreaCollision = cardsAreaCreator.BackAreaCollisions[i];

            if (backAreaCollision.gameObject.transform.childCount != 0)
            {
                var cardInfo = backAreaCollision.gameObject.GetComponent<CardInformation>();

                int cardIndex = Array.IndexOf(cardsAreaCreator.BackAreaCollisions, backAreaCollision.gameObject.transform.parent.gameObject);
                cardInfo.CardFreeze = true;

                var playerController = PC.GetComponent<PlayerController>();
                playerController.RefreshCompotitorCard(cardIndex, cardInfo.FirstTakeDamage, cardInfo.CardFreeze);
                playerController.RefreshLog(0, true, selectedCard.GetComponent<CardInformation>().CardName, cardInfo.CardName, Color.blue);
            }
        }



        selectedCard.SetActive(false);


        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");

        // GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
        // TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";



    }

    public static void EngineeroftheRuinsFun(PlayerController PC)
    {
        GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach(var card in AllMyCard)
        {
            card.GetComponent<CardInformation>().CardHealth = (int.Parse(card.GetComponent<CardInformation>().CardHealth) + 2).ToString();
            card.GetComponent<CardInformation>().SetInformation();
            int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, card.transform.parent.gameObject);
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

    public static void RogueAIDroneFun(GameObject selectedCard)
    {
          selectedCard.GetComponent<CardInformation>().Invulnerable = true;
    }

    public static void ScrapShieldFun(GameObject selectedCard, PlayerController PC)
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

    public static void MutantBehemothFun(GameObject selectedCard,PlayerController PC)
    {
        for (int i = 7; i < 14; i++)
        {

            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount != 0)
            {
                GameObject CurrentCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.GetChild(0).gameObject;

                int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, CurrentCard.transform.parent.gameObject);
                CurrentCard.GetComponent<CardInformation>().CardDamage -= 2;
                CurrentCard.GetComponent<CardInformation>().Behemot = true;

                PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_RefreshMaxDamage", RpcTarget.All, index, CurrentCard.GetComponent<CardInformation>().Behemot);
                PC.RefreshLog(0, true, selectedCard.GetComponent<CardInformation>().CardName, CurrentCard.GetComponent<CardInformation>().CardName, Color.magenta);
            }
        }

    }

    public static void LoneCyborgFun(GameObject selectedCard,PlayerController PC)
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
        if(cardscount == 1)
        {
            selectedCard.GetComponent<CardInformation>().CardDamage += 3;
            selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 3).ToString();
            selectedCard.GetComponent<CardInformation>().DivineSelected = true;
            selectedCard.GetComponent<CardInformation>().SetInformation();
            int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
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

    public static void ScavengerRaiderFun(PlayerController PC)
    {
        PC.ScavengerRaider();

    }

    public static void ClaireFun(GameObject selectedCard,PlayerController PC)
    {
        PC._CardProgress.DamageToAlLOtherMinions(selectedCard.GetComponent<CardInformation>().CardDamage, selectedCard.GetComponent<CardInformation>().CardName);
        selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) - 2).ToString();
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
        PC.CreateTextAtTargetIndex(index,2, true);
        PC.RefreshMyCard(index,
                    selectedCard.GetComponent<CardInformation>().CardHealth,
                    selectedCard.GetComponent<CardInformation>().HaveShield,
                    selectedCard.GetComponent<CardInformation>().CardDamage,
                    selectedCard.GetComponent<CardInformation>().DivineSelected,
                    selectedCard.GetComponent<CardInformation>().FirstTakeDamage,
                    selectedCard.GetComponent<CardInformation>().FirstDamageTaken,
                    selectedCard.GetComponent<CardInformation>().EternalShield);

    }

    public static void GarageRaidFun(GameObject selectedCard, PlayerController PC)
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
        int randomChoice = UnityEngine.Random.Range(0, 2); 

        if (randomChoice == 0)
        {
            PC.CreateDeckCard("Warlord", "6", 6, 7);
        }
        else
        {
            PC.CreateDeckCard("Dune Raider", "5", 4, 5);
        }

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }


    public static void RadioactiveFalloutFun(GameObject selectedCard, PlayerController PC)
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
        PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_ShuffleCells", RpcTarget.All, shuffledIndexes.ToArray());

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;
        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void MutatedBloodSampleFun(GameObject selectedCard, PlayerController PC)
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

    public static void GoldenFleeceFun(GameObject selectedCard, PlayerController PC)
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

    public static void LabyrinthMazeFun(GameObject selectedCard, PlayerController PC)
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
        PC.LabyrinthMaze();

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
        Debug.LogError("USSEEDD A SPEEELLL");
    }
    public static void DivineAscentionFun(GameObject selectedCard, PlayerController PC)
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

    public static void CanFirstRauntAttack(GameObject selectedCard)
    {
        selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
        Transform childTransform = selectedCard.transform;
        Transform green = childTransform.Find("Green");
        green.gameObject.SetActive(true);
    }

}
