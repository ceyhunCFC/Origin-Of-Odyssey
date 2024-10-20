using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class OdinCardFuns 
{
  
    public static void ShieldmaidenDefenderFun(GameObject selectedCard )
    {
        int frontRowCardCount = 0;

        for (int i = 7; i < 14; i++)
        {
            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount > 0)
            {
                frontRowCardCount++;
            }
        }
        selectedCard.GetComponent<CardInformation>().CardHealth=(int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 5).ToString();
        selectedCard.GetComponent<CardInformation>().SetInformation();

    }

    public static void RunestoneMysticFun(PlayerController PC)
    {
        PC.SpellsExtraDamage += 1;
        PC.CreateRandomCard();
    }

    public static void FenrirsSpawnFun(GameObject selectedCard)                                                                               //rakibin ve benim kartlar�m�n say�s�na bak�yor rakip fazla ise kart sald�r� ve damage kazan�yor
    {
        int mycard = 0;
        int enemycard = 0;

        for (int i = 0; i < 14; i++)
        {
            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].gameObject.transform.childCount > 0)
            {
                enemycard++;
            }
            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount > 0)
            {
                mycard++;
            }
        }
        if(enemycard > mycard)
        {
            Debug.Log("D��man fazla");
            selectedCard.GetComponent<CardInformation>().CardDamage += 2;
            selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 3).ToString();
            selectedCard.GetComponent<CardInformation>().SetInformation();
        }
    }

    public static void SkaldBardDFun(PlayerController PC)
    {
        PC.SpawnAndReturnGameObject();
    }

    public static void MimirsSeerFun(PlayerController PC)
    {
        for(int i = 0;i<2;i++)
        {
            GameObject spawnedObject = PC.SpawnAndReturnGameObject();

            if (spawnedObject.GetComponent<CardInformation>().CardHealth == "")
            {
                Debug.LogError("Mimirsseer SPEEEELLL YARATTTI ");
                PC.TalkCloud("Mimirs Seer created a spell..");

                spawnedObject.GetComponent<CardInformation>().CardMana--;

            }
            else
            {
                Debug.LogError("Mimirsseer M�NNYOONNNN YARATTTI ");
                PC.TalkCloud("Mimirs Seer created a minion..");
            }
        }

    }

    public static void EinherjarCallerFun(PlayerController PC)
    {
        if (PC.DeadMyCardName == null || PC.DeadMyCardName.Count == 0)
        {
            Debug.LogWarning("Card name list is empty or null.");
            return;
        }
        string targetCardName = PC.DeadMyCardName[UnityEngine.Random.Range(0, PC.DeadMyCardName.Count)];
        int targetIndex = -1;
        string cardHealth = string.Empty;
        int cardDamage = 0;
        bool cardFound = false;

        OdinCard odinCard = new OdinCard();

        for (int i = 0; i < odinCard.minions.Count; i++)
        {
            if (odinCard.minions[i].name == targetCardName)
            {
                targetIndex = i;
                cardHealth = odinCard.minions[targetIndex].health.ToString();
                cardDamage = odinCard.minions[targetIndex].attack;
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
        }
        else
        {
            Debug.LogWarning("Card not found in OdinCard minions.");
        }
    }


    public static void ValkyriesChosenFun(PlayerController PC)
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
        for (int i = 0; i < 7; i++)
        {
            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
            {
                emptyBackCells.Add(i);
            }
        }

        int cardsCreated = 0;
        while (cardsCreated < 2)
        {
            if (emptyFrontCells.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                int index = emptyFrontCells[randomIndex];
                PC.CreateSpecialCard("Viking Spirit", "1", 1, 0, index, true);
                emptyFrontCells.RemoveAt(randomIndex);
                cardsCreated++;
            }
            else if (emptyBackCells.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                int index = emptyBackCells[randomIndex];
                PC.CreateSpecialCard("Viking Spirit", "1", 1, 0, index, true);
                emptyBackCells.RemoveAt(randomIndex);
                cardsCreated++;
            }
            else
            {
                Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
                break;
            }
        }
    }

    public static void FrostGiantFun(GameObject selectedCard, PlayerController PC)
    {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;
        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.EnemyAllCard();
    }
    public static void HeimdallrFun( PlayerController PC)
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
        for (int i = 0; i < 7; i++)
        {
            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
            {
                emptyBackCells.Add(i);
            }
        }

        int cardsCreated = 0;
        while (cardsCreated < 3)
        {
            if (emptyFrontCells.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                int index = emptyFrontCells[randomIndex];
                PC.CreateSpecialCard("Viking", "1", 1, 0, index, true);
                emptyFrontCells.RemoveAt(randomIndex);
                cardsCreated++;
            }
            else if (emptyBackCells.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                int index = emptyBackCells[randomIndex];
                PC.CreateSpecialCard("Viking", "1", 1, 0, index, true);
                emptyBackCells.RemoveAt(randomIndex);
                cardsCreated++;
            }
            else
            {
                Debug.LogWarning("No empty cells available to place the Viking card.");
                break;
            }
        }
    }

    public static void WintersChillFun(GameObject selectedCard, PlayerController PC)
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
        PC._CardProgress.EnemyAllCard();

        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void VikingRaidFun(GameObject selectedCard, PlayerController PC)
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

        List<int> emptyFrontCells = new List<int>();
        List<int> emptyBackCells = new List<int>();

        for (int i = 7; i < 14; i++)
        {
            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
            {
                emptyFrontCells.Add(i);
            }
        }
        for (int i = 0; i < 7; i++)
        {
            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
            {
                emptyBackCells.Add(i);
            }
        }

        int cardsCreated = 0;
        while (cardsCreated < 3)
        {
            if (emptyFrontCells.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                int index = emptyFrontCells[randomIndex];
                GameObject Card = PC.CreateSpecialCard("Viking", "2", 2, 0, index, true);
                CanFirstRauntAttack(Card);
                emptyFrontCells.RemoveAt(randomIndex);
                cardsCreated++;
            }
            else if (emptyBackCells.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                int index = emptyBackCells[randomIndex];
                GameObject Card = PC.CreateSpecialCard("Viking", "2", 2, 0, index, true);
                CanFirstRauntAttack(Card);
                emptyBackCells.RemoveAt(randomIndex);
                cardsCreated++;
            }
            else
            {
                Debug.LogWarning("No empty cells available to place the Viking card.");
                break;
            }
        }
        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void SleipnirsGallopFun(GameObject selectedCard, PlayerController PC)
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
    public static void GjallarhornCallFun(GameObject selectedCard, PlayerController PC)
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
        List<string> einherjarNames = new List<string> { "Einherjar Champion", "Einherjar Berserker", "Einherjar Duelist" };
        int randomIndex = UnityEngine.Random.Range(0, einherjarNames.Count);
        string selectedName = einherjarNames[randomIndex];
        int index = -1;
        
        
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
            int indexrandom = UnityEngine.Random.Range(0, emptyFrontCells.Count);
            index = emptyFrontCells[indexrandom];
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
                int indexrandom = UnityEngine.Random.Range(0, emptyBackCells.Count);
                index = emptyBackCells[indexrandom];
            }
            else
            {
                Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
            }
        }
        if (index == -1)
            return;
        
        if (selectedName == "Einherjar Champion")
        {
            GameObject CurrentCard = PC.CreateSpecialCard("Einherjar Champion", "7", 3, 0, index, true);
            CanFirstRauntAttack(CurrentCard);
        }
        else if (selectedName == "Einherjar Berserker")
        {
            PC.CreateSpecialCard("Einherjar Champion", "5", 3, 0, index, true);
        }
        else if (selectedName == "Einherjar Duelist")
        {
            PC.CreateSpecialCard("Einherjar Champion", "5", 5, 0, index, true);
        }
        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void NornWeaverFun(GameObject selectedCard, PlayerController PC)
    {
        float xPos = PC.DeckCardCount * 0.8f - 0.8f; 
        selectedCard.transform.localPosition = new Vector3(xPos, 0, 0);
        selectedCard.tag = "Card";
        CreateCard(selectedCard,PC);
        PC.StackDeck();
        PC.StackCompetitorDeck();
        PC.DeckCardCount++;

        PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateRandomCard", RpcTarget.All);

    }

    public static void RuneMagicFun(GameObject selectedCard, PlayerController PC)
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
        PC.RuneMagic();
        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        
        Debug.LogError("USSEEDD A SPEEELLL");
        
    }

    public static void TheAllfathersDecreeFun(GameObject selectedCard, PlayerController PC)
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

        List<int> emptyFrontCells = new List<int>();
        List<int> emptyBackCells = new List<int>();

        GameObject CreatedCard;

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
            CreatedCard = PC.CreateSpecialCard("Gungnir", "4", 2, 0, index, true);
            CreatedCard.GetComponent<CardInformation>().Invulnerable = true;
            CanFirstRauntAttack(CreatedCard);
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
                CreatedCard = PC.CreateSpecialCard("Gungnir", "4", 2, 0, index, true);
                CreatedCard.GetComponent<CardInformation>().Invulnerable = true;
                CanFirstRauntAttack(CreatedCard);
            }
            else
            {
                Debug.LogWarning("No empty cells available to place the Viking Spirit card.");
            }
        }

        
        PC.UsedSpell(selectedCard);
        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        
        Debug.LogError("USSEEDD A SPEEELLL");
    }

    public static void MimirsWisdomFun(GameObject selectedCard,GameObject Card, PlayerController PC)
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
        float xPos = PC.DeckCardCount * 0.8f - 0.8f;
        Card.transform.localPosition = new Vector3(xPos, 0, 0);
        Card.tag = "Card";
        CreateCard(Card, PC);
        PC.StackDeck();
        PC.StackCompetitorDeck();
        PC.DeckCardCount++;

        PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RPC_CreateRandomCard", RpcTarget.All);

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

    static void CreateCard(GameObject CardCurrent,PlayerController PC)
    {

        switch (PC.CompetitorMainCard)
        {
            case "Zeus":
                ZeusCard zeusCard = new ZeusCard();


                int CardIndex = UnityEngine.Random.Range(1, PC.CompetitorDeck.Length); // 1 DEN BA�LIYOR ��NK� �NDEX 0 HEROMUZ
                string targetCardName = PC.CompetitorDeck[CardIndex]; // Deste i�inden gelen kart isminin miniyon mu buyu mu oldu�unu belirle daha sonra �zelliklerini getir.

                int targetIndex = -1;



                for (int i = 0; i < zeusCard.minions.Count; i++)
                {
                    if (zeusCard.minions[i].name == targetCardName)
                    {
                        targetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = zeusCard.minions[targetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = zeusCard.minions[targetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = zeusCard.minions[targetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = zeusCard.minions[targetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = zeusCard.minions[targetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < zeusCard.spells.Count; i++)
                {
                    if (zeusCard.spells[i].name == targetCardName)
                    {
                        targetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = zeusCard.spells[targetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = zeusCard.spells[targetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = zeusCard.spells[targetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                break;

            case "Genghis":
                GenghisCard genghisCard = new GenghisCard();

                int GenghisCardIndex = UnityEngine.Random.Range(1, PC.CompetitorDeck.Length); // 1 DEN BA�LIYOR ��NK� �NDEX 0 HEROMUZ
                string GenghistargetCardName = PC.CompetitorDeck[GenghisCardIndex]; // Deste i�inden gelen kart isminin miniyon mu buyu mu oldu�unu belirle daha sonra �zelliklerini getir.

                int GenghistargetIndex = -1;

                for (int i = 0; i < genghisCard.minions.Count; i++)
                {
                    if (genghisCard.minions[i].name == GenghistargetCardName)
                    {
                        GenghistargetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = genghisCard.minions[GenghistargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = genghisCard.minions[GenghistargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = genghisCard.minions[GenghistargetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = genghisCard.minions[GenghistargetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = genghisCard.minions[GenghistargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < genghisCard.spells.Count; i++)
                {
                    if (genghisCard.spells[i].name == GenghistargetCardName)
                    {
                        GenghistargetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = genghisCard.spells[GenghistargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = genghisCard.spells[GenghistargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = genghisCard.spells[GenghistargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                break;
            case "Odin":
                OdinCard odinCard = new OdinCard();

                int OdinCardIndex = UnityEngine.Random.Range(1, PC.CompetitorDeck.Length); // 1 DEN BA�LIYOR ��NK� �NDEX 0 HEROMUZ
                string OdintargetCardName = PC.CompetitorDeck[OdinCardIndex]; // Deste i�inden gelen kart isminin miniyon mu buyu mu oldu�unu belirle daha sonra �zelliklerini getir.

                int OdintargetIndex = -1;

                for (int i = 0; i < odinCard.minions.Count; i++)
                {
                    if (odinCard.minions[i].name == OdintargetCardName)
                    {
                        OdintargetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = odinCard.minions[OdintargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = odinCard.minions[OdintargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = odinCard.minions[OdintargetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = odinCard.minions[OdintargetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = odinCard.minions[OdintargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < odinCard.spells.Count; i++)
                {
                    if (odinCard.spells[i].name == OdintargetCardName)
                    {
                        OdintargetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = odinCard.spells[OdintargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = odinCard.spells[OdintargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = odinCard.spells[OdintargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                if (CardCurrent.GetComponent<CardInformation>().CardName == "Naglfar")
                {
                    if (PC.DeadCardCount >= 6)
                    {
                        CardCurrent.GetComponent<CardInformation>().CardMana -= 3;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                    }
                }
                break;
            case "Anubis":
                AnubisCard anubisCard = new AnubisCard();

                int AnubisCardIndex = UnityEngine.Random.Range(1, PC.CompetitorDeck.Length); // 1 DEN BA�LIYOR ��NK� �NDEX 0 HEROMUZ
                string AnubistargetCardName = PC.CompetitorDeck[AnubisCardIndex]; // Deste i�inden gelen kart isminin miniyon mu buyu mu oldu�unu belirle daha sonra �zelliklerini getir.

                int AnubistargetIndex = -1;

                for (int i = 0; i < anubisCard.minions.Count; i++)
                {
                    if (anubisCard.minions[i].name == AnubistargetCardName)
                    {
                        AnubistargetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = anubisCard.minions[AnubistargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = anubisCard.minions[AnubistargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = anubisCard.minions[AnubistargetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = anubisCard.minions[AnubistargetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = anubisCard.minions[AnubistargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < anubisCard.spells.Count; i++)
                {
                    if (anubisCard.spells[i].name == AnubistargetCardName)
                    {
                        AnubistargetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = anubisCard.spells[AnubistargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = anubisCard.spells[AnubistargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = anubisCard.spells[AnubistargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                break;
            case "Leonardo Da Vinci":
                LeonardoCard leonardoCard = new LeonardoCard();

                int LeonardoCardIndex = UnityEngine.Random.Range(1, PC.CompetitorDeck.Length); // 1 DEN BA�LIYOR ��NK� �NDEX 0 HEROMUZ
                string LeonardoTargetCardName = PC.CompetitorDeck[LeonardoCardIndex]; // Deste i�inden gelen kart isminin miniyon mu buyu mu oldu�unu belirle daha sonra �zelliklerini getir.

                int LeonardotargetIndex = -1;

                for (int i = 0; i < leonardoCard.minions.Count; i++)
                {
                    if (leonardoCard.minions[i].name == LeonardoTargetCardName)
                    {
                        LeonardotargetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = leonardoCard.minions[LeonardotargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = leonardoCard.minions[LeonardotargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = leonardoCard.minions[LeonardotargetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = leonardoCard.minions[LeonardotargetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = leonardoCard.minions[LeonardotargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < leonardoCard.spells.Count; i++)
                {
                    if (leonardoCard.spells[i].name == LeonardoTargetCardName)
                    {
                        LeonardotargetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = leonardoCard.spells[LeonardotargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = leonardoCard.spells[LeonardotargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = leonardoCard.spells[LeonardotargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                break;
            case "Dustin":
                DustinCard dustinCard = new DustinCard();

                int DustinCardIndex = UnityEngine.Random.Range(1, PC.CompetitorDeck.Length); // 1 DEN BA�LIYOR ��NK� �NDEX 0 HEROMUZ
                string DustinTargetCardName = PC.CompetitorDeck[DustinCardIndex]; // Deste i�inden gelen kart isminin miniyon mu buyu mu oldu�unu belirle daha sonra �zelliklerini getir.

                int DustinTargetIndex = -1;

                for (int i = 0; i < dustinCard.minions.Count; i++)
                {
                    if (dustinCard.minions[i].name == DustinTargetCardName)
                    {
                        DustinTargetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = dustinCard.minions[DustinTargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = dustinCard.minions[DustinTargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = dustinCard.minions[DustinTargetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = dustinCard.minions[DustinTargetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = dustinCard.minions[DustinTargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < dustinCard.spells.Count; i++)
                {
                    if (dustinCard.spells[i].name == DustinTargetCardName)
                    {
                        DustinTargetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = dustinCard.spells[DustinTargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = dustinCard.spells[DustinTargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = dustinCard.spells[DustinTargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }
                break;
        }
    }

}
