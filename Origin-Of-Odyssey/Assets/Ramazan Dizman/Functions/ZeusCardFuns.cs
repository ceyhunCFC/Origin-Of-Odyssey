using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ZeusCardFuns 
{
  
  public static void HeraclesFun(GameObject selectedCard, PlayerController PC )
  { 
    selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + (2 * PC.DeadMonsterCound)).ToString();
    selectedCard.GetComponent<CardInformation>().CardDamage += (2 * PC.DeadMonsterCound);
    selectedCard.GetComponent<CardInformation>().SetInformation();

  }

   public static void LightningBoltFun(GameObject selectedCard, PlayerController PC )
  {

        PC.ManaCountText.text = PC.Mana.ToString() + "/10";
        PC.OwnManaBar.fillAmount =  PC.Mana / 10f;
        PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
        PC.CompetitorManaCountText.text = ( PC._GameManager.ManaCount) + "/10".ToString();
        PC.DeckCardCount--;

        PC.StackDeck();

        if ( PC.PV.IsMine)
        {
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
            PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
            PC. PV.RPC("RefreshPlayersInformation", RpcTarget.All);
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
    
                   // GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                   // TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";

  

    }

    public static void StormcallerFun(PlayerController PC)
    {
        PC.SpellsExtraDamage += 1;
    }

    public static void OdysseanNavigatorFun( PlayerController PC)
    {
        Debug.LogError("ODYYYYYSEAAANN");

        //  CreateRandomCard();

        GameObject spawnedObject = PC.SpawnAndReturnGameObject();

        if (spawnedObject.GetComponent<CardInformation>().CardHealth == "")
        {
            Debug.LogError("ODYYYYYSEAAANN SPEEEELLL YARATTTI ");
            PC.TalkCloud("Odyssean Navigator created a spell..");

            spawnedObject.GetComponent<CardInformation>().CardMana--;

        }
        else
        {
            Debug.LogError("ODYYYYYSEAAANN MÝNNYOONNNN YARATTTI ");
            PC.TalkCloud("Odyssean Navigator created a minion..");
        }
    }

    public static void OraclesEmissaryFun(PlayerController PC)
    {
        List<GameObject> OwnSpellCards = new List<GameObject>();

        for (int i = 0; i < GameObject.Find("Deck").transform.childCount; i++) // KENDÝ DESTEMÝZDEKÝ KARTLARI TEK TEK ÇAÐIR
        {
            if (GameObject.Find("Deck").transform.GetChild(i).GetComponent<CardInformation>().CardHealth == "")  // ÇAÐIRILAN KARTIN BÜYÜ MÜ OLDUÐU KONTROL ET
            {
                OwnSpellCards.Add(GameObject.Find("Deck").transform.GetChild(i).gameObject);
            }
        }


        if (OwnSpellCards.Count > 0)
        {

            GameObject spawnedObject = PC.CreateRandomCard();

            //CallRotateAndRevert(spawnedObject);   burda nul hatasý alýyor bazen bazen almýyor bakýlacak


        }
    }

    public static void LightningForgerFun( PlayerController PC)
    {
        if (PC.PV.IsMine)
        {
            if (PC.PV.Owner.IsMasterClient)
            {


                PC._GameManager.MasterAddAttackDamage(3);
                Debug.LogError(PC._GameManager.MasterAttackDamage);

            }
            else
            {


                PC._GameManager.OtherAddAttackDamage(3);
                Debug.LogError(PC._GameManager.OtherAttackDamage);

            }
        }

    }

    public static void GorgonFun( PlayerController PC)
    {
        PC._CardProgress.FreezeAllEnemyMinions("Gorgon");

    }

    public static void ChimeraFun(PlayerController PC)
    {
        PC._CardProgress.DamageToAlLOtherMinions(2 ,"Chimera");

    }

    public static void AthenaFun(PlayerController PC)
    {
        PC._CardProgress.FillWithHoplites();

    }

    public static void LightningStormFun(GameObject selectedCard, PlayerController PC)
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
        PC._CardProgress.AttackerCard = selectedCard;
        PC._CardProgress.LightningStorm();

        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        Debug.LogError("USSEEDD A SPEEELLL");
    }


    public static void OlympianFavorFun(GameObject selectedCard, PlayerController PC)
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

    public static void AegisShieldFun(GameObject selectedCard, PlayerController PC)
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

        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

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

        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

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


}
