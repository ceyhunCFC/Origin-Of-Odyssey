using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class StandartCardFuns 
{
    public static void SiegeMasterUrbanFun(PlayerController PC)
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


        if (emptyFrontCells.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
            int index = emptyFrontCells[randomIndex];
            GameObject Card = PC.CreateSpecialCard("Siege Engine", "2", 2, 0, index, true);
            CanFirstRauntAttack(Card);
            emptyFrontCells.RemoveAt(randomIndex);
        }
        else if (emptyBackCells.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
            int index = emptyBackCells[randomIndex];
            GameObject Card = PC.CreateSpecialCard("Siege Engine", "2", 2, 0, index, true);
            CanFirstRauntAttack(Card);
            emptyBackCells.RemoveAt(randomIndex);
        }
        else
        {
            Debug.LogWarning("No empty cells available to place the Siege Engine card.");
        }

    }



    public static void TemplarKnightFun(GameObject selectedCard,PlayerController PC)
    {
        selectedCard.GetComponent<CardInformation>().DivineSelected = true;
        int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
        PC.RefreshMyCard(TargetCardIndex,
                    selectedCard.GetComponent<CardInformation>().CardHealth,
                    selectedCard.GetComponent<CardInformation>().HaveShield,
                    selectedCard.GetComponent<CardInformation>().CardDamage,
                    selectedCard.GetComponent<CardInformation>().DivineSelected,
                    selectedCard.GetComponent<CardInformation>().FirstTakeDamage,
                    selectedCard.GetComponent<CardInformation>().FirstDamageTaken,
                    selectedCard.GetComponent<CardInformation>().EternalShield);
    }

    public static void CerberusSpawnFun(PlayerController PC)
    {
        GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
        GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");

        if (AllEnemyCard.Length > AllMyCard.Length)
        {
            Debug.Log("Enemy has more cards than you. CerberusSpawnFun is activated.");
            List<int> emptyFrontCells = new List<int>();
            List<int> emptyBackCells = new List<int>();

            for (int i = 7; i < 14; i++)
            {
                if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount == 0)
                {
                    emptyFrontCells.Add(i);
                }
            }

            if (emptyFrontCells.Count >= 2)
            {
                for (int j = 0; j < 2; j++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                    int index = emptyFrontCells[randomIndex];
                    GameObject currentcard = PC.CreateSpecialCard("Hellhound", "1", 1, 0, index, true);
                    CanFirstRauntAttack(currentcard);
                    emptyFrontCells.RemoveAt(randomIndex);
                }
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

                if (emptyBackCells.Count >= 2)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                        int index = emptyBackCells[randomIndex];
                        GameObject currentcard = PC.CreateSpecialCard("Hellhound", "1", 1, 0, index, true);
                        CanFirstRauntAttack(currentcard);
                        emptyBackCells.RemoveAt(randomIndex);
                    }
                }
                else
                {
                    Debug.LogWarning("No empty cells available to place the Scavenger card.");
                }
            }
        }
        else
        {
            Debug.Log("You have equal or more cards than the enemy. CerberusSpawnFun is not activated.");
        }
    }


    public static void ArcaneScholarFun(PlayerController PC)
    {
        PC.SpellsExtraDamage += 2;
        PC.CreateRandomCard();
    }

    public static void RebelOutcastFun(PlayerController PC)
    {
        GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
        GameObject randomCard = AllEnemyCard[UnityEngine.Random.Range(0, AllEnemyCard.Length)];
        randomCard.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard.GetComponent<CardInformation>().CardHealth) - 4).ToString();
        randomCard.GetComponent<CardInformation>().SetInformation();
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, randomCard.transform.parent.gameObject);
        PC.CreateTextAtTargetIndex(index, 2, false, "Rebel Outcast");
        PC.RefreshUsedCard(index, randomCard.GetComponent<CardInformation>().CardHealth, randomCard.GetComponent<CardInformation>().CardDamage);

    }

    public static void NaiadProtectorFun(PlayerController PC)
    {
        PC.NaiadProtector = true;

    }

    public static void RuinedCityScoutFun(PlayerController PC)
    {
        PC.MarcoPolo();

    }

    public static void GladiatorChampionFun(GameObject selectedCard,PlayerController PC)
    {
        GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
        int count = AllEnemyCard.Length;

        if(count > 0)
        {
            selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + count).ToString();
            selectedCard.GetComponent<CardInformation>().CardDamage += count;
            selectedCard.GetComponent<CardInformation>().SetInformation();
            int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
            PC.RefreshMyCard(TargetCardIndex,
                        selectedCard.GetComponent<CardInformation>().CardHealth,
                        selectedCard.GetComponent<CardInformation>().HaveShield,
                        selectedCard.GetComponent<CardInformation>().CardDamage,
                        selectedCard.GetComponent<CardInformation>().DivineSelected,
                        selectedCard.GetComponent<CardInformation>().FirstTakeDamage,
                        selectedCard.GetComponent<CardInformation>().FirstDamageTaken,
                        selectedCard.GetComponent<CardInformation>().EternalShield);
        }

    }

    public static void UrbanRangerFun(PlayerController PC)
    {
        GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");

        if (AllEnemyCard.Length < 2)
        {
            Debug.LogWarning("Not enough enemy cards to target two distinct cards.");
            return;
        }

        GameObject randomCard1 = AllEnemyCard[UnityEngine.Random.Range(0, AllEnemyCard.Length)];
        randomCard1.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard1.GetComponent<CardInformation>().CardHealth) - 2).ToString();
        randomCard1.GetComponent<CardInformation>().SetInformation();
        int index1 = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, randomCard1.transform.parent.gameObject);
        PC.CreateTextAtTargetIndex(index1, 2, false, "Urban Ranger");
        PC.RefreshUsedCard(index1, randomCard1.GetComponent<CardInformation>().CardHealth, randomCard1.GetComponent<CardInformation>().CardDamage);                                                          //iki kart seçiyor ilkinin aynýsý olmasýn ve 2 hasar veriyor ikisine de

        GameObject randomCard2;
        do
        {
            randomCard2 = AllEnemyCard[UnityEngine.Random.Range(0, AllEnemyCard.Length)];
        } while (randomCard2 == randomCard1);

        randomCard2.GetComponent<CardInformation>().CardHealth = (int.Parse(randomCard2.GetComponent<CardInformation>().CardHealth) - 2).ToString();
        randomCard2.GetComponent<CardInformation>().SetInformation();
        int index2 = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, randomCard2.transform.parent.gameObject);
        PC.CreateTextAtTargetIndex(index2, 2, false, "Urban Ranger");
        PC.RefreshUsedCard(index2, randomCard2.GetComponent<CardInformation>().CardHealth, randomCard2.GetComponent<CardInformation>().CardDamage);
    }

    public static void ByzantineFireSlingerFun(PlayerController PC)
    {
        PC._CardProgress.DamageToAlLOtherMinions(1, "Byzantine Fire Slinger");
    }

    public static void ShadowAssassinFun(GameObject selectedCard)
    {
        selectedCard.GetComponent<CardInformation>().SunDiskRadiance = true;
    }

    public static void RogueMechPilotFun(PlayerController PC)
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

        if (emptyFrontCells.Count >= 2)
        {
            for (int j = 0; j < 2; j++)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                int index = emptyFrontCells[randomIndex];
                GameObject currentcard = PC.CreateSpecialCard("Defective Drone", "1", 1, 0, index, true);
                CanFirstRauntAttack(currentcard);
                emptyFrontCells.RemoveAt(randomIndex);
            }
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

            if (emptyBackCells.Count >= 2)
            {
                for (int j = 0; j < 2; j++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                    int index = emptyBackCells[randomIndex];
                    GameObject currentcard = PC.CreateSpecialCard("Defective Drone", "1", 1, 0, index, true);
                    CanFirstRauntAttack(currentcard);
                    emptyBackCells.RemoveAt(randomIndex);
                }
            }
            else
            {
                Debug.LogWarning("No empty cells available to place the Defective Drone card.");
            }
        }
    }

    public static void DesertConjurerFun(PlayerController PC)
    {
        PC.CreateDeckCard("Sandstorm", "0", 0, 0);
    }

    public static void SandstormFun(PlayerController PC)
    {
        PC._CardProgress.DamageToAlLOtherMinions(1, "Sandstorm");
    }

    public static void OasisGuardianFun(PlayerController PC)
    {
        PC.GetComponent<CardController>().AddHealCard(2, !PC.PV.Owner.IsMasterClient);
    }

    public static void BattleMageFun(PlayerController PC)
    {
        PC.SpellsExtraDamage += 1;
    }

    public static void RovingMerchantFun(PlayerController PC)
    {
        GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
        if(AllMyCard.Length >= 3)
        {
            PC.SpawnAndReturnGameObject();
        }
    }

    public static void NomadicHunterFun(GameObject selectedCard,PlayerController PC)
    {
        if(PC.PlayedSpell)
        {
            CanFirstRauntAttack(selectedCard);
        }
    }

    public static void RaidingPartyFun(PlayerController PC)
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

        if (emptyFrontCells.Count >= 2)
        {
            for (int j = 0; j < 2; j++)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                int index = emptyFrontCells[randomIndex];
                GameObject currentcard = PC.CreateSpecialCard("Pirate", "1", 1, 0, index, true);
                CanFirstRauntAttack(currentcard);
                emptyFrontCells.RemoveAt(randomIndex);
            }
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

            if (emptyBackCells.Count >= 2)
            {
                for (int j = 0; j < 2; j++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                    int index = emptyBackCells[randomIndex];
                    GameObject currentcard = PC.CreateSpecialCard("Pirate", "1", 1, 0, index, true);
                    CanFirstRauntAttack(currentcard);
                    emptyBackCells.RemoveAt(randomIndex);
                }
            }
            else
            {
                Debug.LogWarning("No empty cells available to place the Pirate card.");
            }
        }
    }

    public static void PyromaniacWizardFun(PlayerController PC)
    {
        PC._CardProgress.DamageToAlLOtherMinions(1, "Pyromaniac Wizard");
    }

    public static void FrontlineMilitiaFun(GameObject selectedCard,PlayerController PC)
    {
        GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
        if(AllMyCard.Length > 0)
        {
            selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + AllMyCard.Length).ToString();
            selectedCard.GetComponent<CardInformation>().SetInformation();
            int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
            PC.RefreshMyCard(TargetCardIndex,
                        selectedCard.GetComponent<CardInformation>().CardHealth,
                        selectedCard.GetComponent<CardInformation>().HaveShield,
                        selectedCard.GetComponent<CardInformation>().CardDamage,
                        selectedCard.GetComponent<CardInformation>().DivineSelected,
                        selectedCard.GetComponent<CardInformation>().FirstTakeDamage,
                        selectedCard.GetComponent<CardInformation>().FirstDamageTaken,
                        selectedCard.GetComponent<CardInformation>().EternalShield);
        }
    }

    public static void WanderingHealerFun(GameObject selectedCard,PlayerController PC)
    {
        GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach(GameObject Card in AllMyCard)
        {
            if(Card != selectedCard)
            {
                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) + 3).ToString();
                Card.GetComponent<CardInformation>().SetInformation();
                int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, Card.transform.parent.gameObject);
                PC.RefreshMyCard(TargetCardIndex,
                            Card.GetComponent<CardInformation>().CardHealth,
                            Card.GetComponent<CardInformation>().HaveShield,
                            Card.GetComponent<CardInformation>().CardDamage,
                            Card.GetComponent<CardInformation>().DivineSelected,
                            Card.GetComponent<CardInformation>().FirstTakeDamage,
                            Card.GetComponent<CardInformation>().FirstDamageTaken,
                            Card.GetComponent<CardInformation>().EternalShield);
            }
        }
    }

    public static void VikingShieldMaidenFun(GameObject selectedCard,PlayerController PC)
    {
        GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (GameObject Card in AllMyCard)
        {
            if(Card.GetComponent<CardInformation>().CardName == "Viking Raider" || Card.GetComponent<CardInformation>().CardName == "Viking" || Card.GetComponent<CardInformation>().CardName == "Viking Spirit")
            {
                selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                selectedCard.GetComponent<CardInformation>().CardDamage++;
                selectedCard.GetComponent<CardInformation>().SetInformation();
                int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
                PC.RefreshMyCard(TargetCardIndex,
                            selectedCard.GetComponent<CardInformation>().CardHealth,
                            selectedCard.GetComponent<CardInformation>().HaveShield,
                            selectedCard.GetComponent<CardInformation>().CardDamage,
                            selectedCard.GetComponent<CardInformation>().DivineSelected,
                            selectedCard.GetComponent<CardInformation>().FirstTakeDamage,
                            selectedCard.GetComponent<CardInformation>().FirstDamageTaken,
                            selectedCard.GetComponent<CardInformation>().EternalShield);
                return;
            }
        }


    }

    public static void PlagueCarrierFun(GameObject selectedCard, PlayerController PC)
    {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;
    }

    public static void DuneScoutFun(GameObject selectedCard)
    {
        GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
        if(AllEnemyCard.Length == 0)
        {
            CanFirstRauntAttack(selectedCard);
        }
    }

    public static void ScavengersDaughterFun(GameObject selectedCard,PlayerController PC)
    {
        GameObject[] AllMyCard = GameObject.FindGameObjectsWithTag("UsedCard");
        foreach (GameObject Card in AllMyCard)
        {
            if(Card!=selectedCard)
            {
                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) + 1).ToString();
                Card.GetComponent<CardInformation>().SetInformation();
                int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, Card.transform.parent.gameObject);
                PC.RefreshMyCard(TargetCardIndex,
                            Card.GetComponent<CardInformation>().CardHealth,
                            Card.GetComponent<CardInformation>().HaveShield,
                            Card.GetComponent<CardInformation>().CardDamage,
                            Card.GetComponent<CardInformation>().DivineSelected,
                            Card.GetComponent<CardInformation>().FirstTakeDamage,
                            Card.GetComponent<CardInformation>().FirstDamageTaken,
                            Card.GetComponent<CardInformation>().EternalShield);
            }
            
        }
    }

    public static void MysticArcherFun(GameObject selectedCard,PlayerController PC)
    {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;
    }

    public static void ToxicRainmakerFun(PlayerController PC)
    {
        GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
        foreach (GameObject card in AllEnemyCard)
        {
            card.GetComponent<CardInformation>().CardDamage -= 1;
            card.GetComponent<CardInformation>().SetInformation();
            card.GetComponent<CardInformation>().ToxicRainmaker = true;
            int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, card.transform.parent.gameObject);
            PC.RefreshUsedCard(index, card.GetComponent<CardInformation>().CardHealth, card.GetComponent<CardInformation>().CardDamage);
        }
    }

    public static void DesertWarlockFun(PlayerController PC)
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

        if (emptyFrontCells.Count >= 2)
        {
            for (int j = 0; j < 2; j++)
            {
                int randomIndex = UnityEngine.Random.Range(0, emptyFrontCells.Count);
                int index = emptyFrontCells[randomIndex];
                GameObject currentcard = PC.CreateSpecialCard("Sand Elementals", "2", 2, 0, index, true);
                CanFirstRauntAttack(currentcard);
                emptyFrontCells.RemoveAt(randomIndex);
            }
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

            if (emptyBackCells.Count >= 2)
            {
                for (int j = 0; j < 2; j++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, emptyBackCells.Count);
                    int index = emptyBackCells[randomIndex];
                    GameObject currentcard = PC.CreateSpecialCard("Sand Elementals", "2", 2, 0, index, true);
                    CanFirstRauntAttack(currentcard);
                    emptyBackCells.RemoveAt(randomIndex);
                }
            }
            else
            {
                Debug.LogWarning("No empty cells available to place the Sand Elementals card.");
            }
        }
        PC.LessSpellCost += 1;
    }

    public static void StreetThugFun(GameObject selectedCard,PlayerController PC)
    {
        GameObject[] AllEnemyCard = GameObject.FindGameObjectsWithTag("CompetitorCard");
        if(AllEnemyCard.Length == 1)
        {
            selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + 2).ToString();
            selectedCard.GetComponent<CardInformation>().CardDamage += 2;
            selectedCard.GetComponent<CardInformation>().SetInformation();
            int TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);
            PC.RefreshMyCard(TargetCardIndex,
                        selectedCard.GetComponent<CardInformation>().CardHealth,
                        selectedCard.GetComponent<CardInformation>().HaveShield,
                        selectedCard.GetComponent<CardInformation>().CardDamage,
                        selectedCard.GetComponent<CardInformation>().DivineSelected,
                        selectedCard.GetComponent<CardInformation>().FirstTakeDamage,
                        selectedCard.GetComponent<CardInformation>().FirstDamageTaken,
                        selectedCard.GetComponent<CardInformation>().EternalShield);
        }
    }

    public static void AncientLibrarianFun(GameObject selectedCard,PlayerController PC)
    {
        int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

        PC._CardProgress.SecoundTargetCard = true;
        PC._CardProgress.SetAttackerCard(index);
        PC._CardProgress.AttackerCard = selectedCard;
    }

    public static void ApprenticeSorcererFun(PlayerController PC)
    {
        GameObject CurrentCard;

        do
        {
            CurrentCard = PC.CreateRandomCard();
            if (CurrentCard.GetComponent<CardInformation>().CardMana > 3)
            {
                PC.CreateSpell(CurrentCard);
            }
        }
        while (CurrentCard.GetComponent<CardInformation>().CardMana > 3);
    }

    public static void CanFirstRauntAttack(GameObject selectedCard)
    {
        selectedCard.GetComponent<CardInformation>().isItFirstRaound = false;
        Transform childTransform = selectedCard.transform;
        Transform green = childTransform.Find("Green");
        green.gameObject.SetActive(true);
    }
}
