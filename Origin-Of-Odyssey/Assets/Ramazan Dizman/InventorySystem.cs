using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    public List<PlayerCards> playerCards = new List<PlayerCards>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            InitializeDefaultCards();
        }
    }

    private void InitializeDefaultCards()
    {
        int[] defaultIds = { 0, 1, 8, 9, 10, 11 };

        foreach (int id in defaultIds)
        {
            PlayerCards defaultCard = new PlayerCards(
                id,
                0,
                0, 0, 0, 0, 0, 0, 0, 0, "", 0 
            );

            playerCards.Add(defaultCard);
        }
    }

    public void UpdateCard(PlayerCards updatedCard)
    {
        var existingCard = playerCards.Find(card => card.id == updatedCard.id);

        if (existingCard != null)
        {
            existingCard.hasBeenBought = updatedCard.hasBeenBought;
            existingCard.hp = updatedCard.hp;
            existingCard.attack = updatedCard.attack;
            existingCard.defence = updatedCard.defence;
            existingCard.dodge = updatedCard.dodge;
            existingCard.critical = updatedCard.critical;
            existingCard.duelsFought = updatedCard.duelsFought;
            existingCard.duelsWon = updatedCard.duelsWon;
            existingCard.winRatio = updatedCard.winRatio;
            existingCard.question = updatedCard.question;
            existingCard.powerLevel = updatedCard.powerLevel;
        }
    }
}
