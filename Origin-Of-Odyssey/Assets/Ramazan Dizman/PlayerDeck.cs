using Proyecto26;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayerDeck : MonoBehaviour
{
    private readonly string databaseURL = "https://origin-of-odyssey-eee04-default-rtdb.firebaseio.com/Users";
    
    private List<string> cardNames = new List<string>();

    public string localId;
    public string idToken;

    public GameObject NotEnoughCard;

    private void Start()
    {
        localId = AuthManager.localId;
        idToken = AuthManager.idToken;
    }

    public void PostZeusCardButton()
    {
        AddCardName("Zeus");
    }

    public void PostOdinCardButton()
    {
        AddCardName("Odin"); 
    }

    public void PostGenghisCardButton()
    {
        AddCardName("Genghis"); 
    }

    public void PostDustinCardButton()
    {
        AddCardName("Dustin"); 
    }

    public void PostAegisCardButton()
    {
        AddCardName("Aegis"); 
    }

    private void AddCardName(string cardName)
    {
        if (!cardNames.Contains(cardName))
        {
            cardNames.Add(cardName);
        }
    }

    public void SaveButton()
    {
        string jsonData = "[" + string.Join(",", cardNames.ConvertAll(name => "\"" + name + "\"").ToArray()) + "]";
        
        if(cardNames.Count > 3)
        {
            RestClient.Put(databaseURL + "/" + localId + "/PlayerDeck" + ".json?auth=" + idToken, jsonData)
            .Then(response =>
            {
                cardNames.Clear();
            })
            .Catch(error =>
            {
                Debug.LogError("Error for save cards " + error.Message);
            });
            AuthManager.playerDeckArray = ParseJsonArray(jsonData);
            NotEnoughCard.SetActive(false);
        }
        else
        {
            NotEnoughCard.SetActive(true);
        }
        
        //SceneManager.LoadScene("Lobby");
    }
    string[] ParseJsonArray(string jsonArray)
    {
        int startIndex = jsonArray.IndexOf('[') + 1;
        int endIndex = jsonArray.LastIndexOf(']');
        string elements = jsonArray.Substring(startIndex, endIndex - startIndex);

        string[] parts = elements.Split(',');

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i] = parts[i].Trim('\"');
        }

        return parts;
    }
}
