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

    public static void FenrirsSpawnFun(GameObject selectedCard)                                                                               //rakibin ve benim kartlarýmýn sayýsýna bakýyor rakip fazla ise kart saldýrý ve damage kazanýyor
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
            Debug.Log("Düþman fazla");
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
                Debug.LogError("Mimirsseer MÝNNYOONNNN YARATTTI ");
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

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
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

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
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

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
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

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
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

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
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

        

        selectedCard.SetActive(false);
        selectedCard = null;
        PC.lastHoveredCard = null;

        PC.UsedSpell();
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

    static void CreateCard(GameObject CardCurrent,PlayerController PC)
    {

        switch (PC.CompetitorMainCard)
        {
            case "Zeus":
                ZeusCard zeusCard = new ZeusCard();


                int CardIndex = UnityEngine.Random.Range(1, PC.CompetitorDeck.Length); // 1 DEN BAÞLIYOR ÇÜNKÐ ÝNDEX 0 HEROMUZ
                string targetCardName = PC.CompetitorDeck[CardIndex]; // Deste içinden gelen kart isminin miniyon mu buyu mu olduðunu belirle daha sonra özelliklerini getir.

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

                int GenghisCardIndex = UnityEngine.Random.Range(1, PC.CompetitorDeck.Length); // 1 DEN BAÞLIYOR ÇÜNKÐ ÝNDEX 0 HEROMUZ
                string GenghistargetCardName = PC.CompetitorDeck[GenghisCardIndex]; // Deste içinden gelen kart isminin miniyon mu buyu mu olduðunu belirle daha sonra özelliklerini getir.

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

                int OdinCardIndex = UnityEngine.Random.Range(1, PC.CompetitorDeck.Length); // 1 DEN BAÞLIYOR ÇÜNKÐ ÝNDEX 0 HEROMUZ
                string OdintargetCardName = PC.CompetitorDeck[OdinCardIndex]; // Deste içinden gelen kart isminin miniyon mu buyu mu olduðunu belirle daha sonra özelliklerini getir.

                int OdintargetIndex = -1;

                for (int i = 0; i < odinCard.minions.Count; i++)
                {
                    if (odinCard.minions[i].name == OdintargetCardName)
                    {
                        GenghistargetIndex = i;

                        CardCurrent.GetComponent<CardInformation>().CardName = odinCard.minions[GenghistargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = odinCard.minions[GenghistargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = odinCard.minions[GenghistargetIndex].health.ToString();
                        CardCurrent.GetComponent<CardInformation>().CardDamage = odinCard.minions[GenghistargetIndex].attack;
                        CardCurrent.GetComponent<CardInformation>().CardMana = odinCard.minions[GenghistargetIndex].mana;
                        CardCurrent.GetComponent<CardInformation>().SetMaxHealth();
                        CardCurrent.GetComponent<CardInformation>().SetInformation();
                        break;
                    }
                }

                for (int i = 0; i < odinCard.spells.Count; i++)
                {
                    if (odinCard.spells[i].name == OdintargetCardName)
                    {
                        GenghistargetIndex = i;
                        CardCurrent.GetComponent<CardInformation>().CardName = odinCard.spells[GenghistargetIndex].name;
                        CardCurrent.GetComponent<CardInformation>().CardDes = odinCard.spells[GenghistargetIndex].name + " POWWERRRRR!!!";
                        CardCurrent.GetComponent<CardInformation>().CardHealth = "";
                        CardCurrent.GetComponent<CardInformation>().CardDamage = 0;
                        CardCurrent.GetComponent<CardInformation>().CardMana = odinCard.spells[GenghistargetIndex].mana;
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
        }
    }

}
