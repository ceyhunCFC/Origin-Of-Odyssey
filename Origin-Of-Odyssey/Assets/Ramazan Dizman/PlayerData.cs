using System;

[Serializable]
public class PlayerData 
{
    public string userName;

    public PlayerData()
    {
        userName = AuthManager.userName;
    }
}
