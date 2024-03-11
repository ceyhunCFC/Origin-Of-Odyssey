using System;

[Serializable]
public class SignResponse 
{
    public string localId;
    public string idToken;

    public SignResponse()
    {
        localId = AuthManager.localId;
        idToken = AuthManager.idToken;
    }
}
