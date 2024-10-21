using System;

[Serializable]
public class PlayerData 
{
    public string UserName;
    public string firstName;
    public string lastName;

    public string[] playerDeck;

    public PlayerData()
    {
        UserName = AuthManager.userName;
        firstName = AuthManager.firstName;
        lastName = AuthManager.lastName;
        playerDeck = AuthManager.playerDeckArray;
    }
}
