[System.Serializable]
public abstract class CardStats {
    public string cardName;
    public float hpValue;
    public float dodgeValue;
    public float attackValue;
    public float defenceValue;
    public float criticalValue;
    public int duelsWon;
    public string question;
    public float winRatio;
    public int duelsFought;
    public int hasBeenBought;
    public string description;
    public float manaValue;
    public float time;
    
    public string GetName()
    {
        return cardName;
    }
}


[System.Serializable]
public class ZeusCard : CardStats
{
    public ZeusCard()
    {
        cardName = "Zeus";
        hpValue = 15f;
        dodgeValue = 4f;
        attackValue = 6f;
        defenceValue = 6f;
        criticalValue = 5f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;
    }
}

[System.Serializable]
public class GenghisCard : CardStats
{
    public GenghisCard()
    {
        cardName = "Genghis";
        hpValue = 11f;
        dodgeValue = 4f;
        attackValue = 6f;
        defenceValue = 5f;
        criticalValue = 7f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;
    }
}

[System.Serializable]
public class LeonardoCard : CardStats
{
    public LeonardoCard()
    {
        cardName = "Leonardo Da Vinci";
        hpValue = 12f;
        dodgeValue = 4f;
        attackValue = 5f;
        defenceValue = 6f;
        criticalValue = 6f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;
    }
}
[System.Serializable]
public class OdinCard : CardStats
{
    public OdinCard()
    {
        cardName = "Odin";
        hpValue = 14f;
        dodgeValue = 4f;
        attackValue = 6f;
        defenceValue = 7f;
        criticalValue = 5f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;
    }
}
[System.Serializable]
public class DustinCard : CardStats
{
    public DustinCard()
    {
        cardName = "Dustin";
        hpValue = 11f;
        dodgeValue = 5f;
        attackValue = 5f;
        defenceValue = 7f;
        criticalValue = 6f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;
    }
}
[System.Serializable]
public class AnubisCard : CardStats
{
    public AnubisCard()
    {
        cardName = "Anubis";
        hpValue = 10f;
        dodgeValue = 3f;
        attackValue = 7f;
        defenceValue = 6f;
        criticalValue = 6f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;
    }
}






