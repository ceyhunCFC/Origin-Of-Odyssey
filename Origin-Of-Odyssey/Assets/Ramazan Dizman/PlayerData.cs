using System;

[Serializable]
public class PlayerData 
{
    public string userName;
    public string firstName;
    public string lastName;

    public string[] playerDeck;

    public PlayerData()
    {
        userName = AuthManager.userName;
        firstName = AuthManager.firstName;
        lastName = AuthManager.lastName;
        playerDeck = AuthManager.playerDeckArray;
    }
}
