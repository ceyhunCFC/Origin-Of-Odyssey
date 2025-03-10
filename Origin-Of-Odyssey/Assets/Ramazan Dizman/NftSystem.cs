using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

public class NftSystem : MonoBehaviour
{
    JSonToken token;
    public LoadStuffResult receivedData;
    public InventorySystem inventorySystem;
    string zeus_dataToSend, genghis_dataToSend, odin_dataToSend, dustin_dataToSend, anubis_dataToSend, leonardo_dataToSend;
    public bool firstLogin = false;
    public MainMenuCanvas mainMenuCanvas;
    public string transactionId, transactionprice;

    private void Start()
    {
        token = TokenBridge.tokenTransfer;
        LoadInv();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadInv();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            SaveInv();
        }
    }

    public void LoadInv()
    {
        StartCoroutine("Routine_ReceiveData");
    }
    IEnumerator Routine_ReceiveData()
    {
        string getDataUri = "https://api.infinitex.space/game";

        using (UnityWebRequest webData = UnityWebRequest.Get(getDataUri))
        {
            webData.SetRequestHeader("Authorization", "Bearer " + token.access_token);
            yield return webData.SendWebRequest();
            if (webData.result != UnityWebRequest.Result.Success)
            {
                print(webData.error);
            }
            else
            {
                string data2nd = webData.downloadHandler.text;
                data2nd = "{\"Items\":" + data2nd + "}";
                LoadStuffResult data = JsonUtility.FromJson<LoadStuffResult>(data2nd);
                receivedData = data;
                bool secondAegisFound = false;
                for (int i = 0; i < receivedData.Items.Length; i++)
                {
                    var item = receivedData.Items[i];
                    if (item.product.productId == "2")
                    {
                        if (!secondAegisFound)
                        {
                            secondAegisFound = true;
                        }
                        else
                        {
                            List<LoadStuff> tempList = new List<LoadStuff>(receivedData.Items);
                            tempList.RemoveAt(i);
                            receivedData.Items = tempList.ToArray();
                            break;
                        }
                    }
                }
                Debug.Log(data2nd);
                TransferCardsToInventorySystem();
                StartCoroutine(Campaign());
            }
        }
    }

    IEnumerator Campaign()
    {
        string url = "https://api.infinitex.space/game/campaign";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + token.access_token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Kampanya durumu al?n?rken bir hata olu?tu: " + webRequest.error);
            }
            else
            {
                string response = webRequest.downloadHandler.text;
                JObject campaignData = JObject.Parse(response);
                firstLogin = (bool)campaignData["campaign"];
            }
        }
        print(firstLogin);
        /*if(firstLogin==false)
        {
            mainMenuCanvas.OpenEventCanvas();
        }*/
        //Remember: If firstLogin is false, open the event canvas.
    }

    private void TransferCardsToInventorySystem()
    {
        List<PlayerCards> cardList = new List<PlayerCards>();

        for (int i = 0; i < receivedData.Items.Length; i++)
        {
            var item = receivedData.Items[i];
            int productId = int.Parse(item.productId);

            if (productId == 0 || productId == 1 || productId == 8 || productId == 9 || productId == 10 || productId == 11)
            {
                PlayerCards updatedCard = new PlayerCards(
                    productId,
                    item.productData.data.hasBeenBought,
                    item.productData.data.hp,
                    item.productData.data.attack,
                    item.productData.data.defence,
                    item.productData.data.dodge,
                    item.productData.data.critical,
                    item.productData.data.duelsFought,
                    item.productData.data.duelsWon,
                    item.productData.data.winRatio,
                    item.productData.data.question,
                    item.productData.data.powerLevel
                );

                InventorySystem.instance.UpdateCard(updatedCard);
            }
        }
    }

    public void campaignset(bool status)
    {
        StartCoroutine(SendCampaignStatus(status));
    }

    IEnumerator SendCampaignStatus(bool status)
    {
        string url = "https://api.infinitex.space/game/campaign";
        string json = "{\"status\": " + status.ToString().ToLower() + "}";


        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            webRequest.SetRequestHeader("Authorization", "Bearer " + token.access_token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error updating campaign status: " + webRequest.error);
            }
            else
            {
                Debug.Log("Campaign status updated successfully.");
                mainMenuCanvas.CloseEventCavnas();
                SaveInv();
            }
        }
    }


    public void SaveInv()
    {
        Zeus_Save();
        Genghis_Save();
        Odin_Save();
        Leonardo_Save();
        Anubis_Save();
        Dustin_Save();
    }

    public void Zeus_Save()
    {
        string dataSave = JsonUtility.ToJson(inventorySystem.playerCards[1], true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "playerCardData.json", dataSave);
        var jsonDataToSend = JsonConvert.SerializeObject(inventorySystem.playerCards[1]);
        zeus_dataToSend = jsonDataToSend;
        print(zeus_dataToSend);
        StartCoroutine("ZEUS_Routine_SendDataToServer");
    }
    public void Genghis_Save()
    {
        string dataSave = JsonUtility.ToJson(inventorySystem.playerCards[0], true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "playerCardData.json", dataSave);
        var jsonDataToSend = JsonConvert.SerializeObject(inventorySystem.playerCards[0]);
        genghis_dataToSend = jsonDataToSend;
        print(genghis_dataToSend);
        StartCoroutine("GENGHIS_Routine_SendDataToServer");
    }
    public void Odin_Save()
    {
        string dataSave = JsonUtility.ToJson(inventorySystem.playerCards[2], true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "playerCardData.json", dataSave);
        var jsonDataToSend = JsonConvert.SerializeObject(inventorySystem.playerCards[2]);
        odin_dataToSend = jsonDataToSend;
        print(odin_dataToSend);
        StartCoroutine("ODIN_Routine_SendDataToServer");

    }
    public void Leonardo_Save()
    {
        string dataSave = JsonUtility.ToJson(inventorySystem.playerCards[5], true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "playerCardData.json", dataSave);
        var jsonDataToSend = JsonConvert.SerializeObject(inventorySystem.playerCards[5]);
        leonardo_dataToSend = jsonDataToSend;
        print(leonardo_dataToSend);
        StartCoroutine("LEONARDO_Routine_SendDataToServer");
    }
    public void Dustin_Save()
    {
        string dataSave = JsonUtility.ToJson(inventorySystem.playerCards[3], true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "playerCardData.json", dataSave);
        var jsonDataToSend = JsonConvert.SerializeObject(inventorySystem.playerCards[3]);
        dustin_dataToSend = jsonDataToSend;
        print(dustin_dataToSend);
        StartCoroutine("DUSTIN_Routine_SendDataToServer");
    }
    public void Anubis_Save()
    {
        string dataSave = JsonUtility.ToJson(inventorySystem.playerCards[4], true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "playerCardData.json", dataSave);
        var jsonDataToSend = JsonConvert.SerializeObject(inventorySystem.playerCards[4]);
        anubis_dataToSend = jsonDataToSend;
        print(anubis_dataToSend);
        StartCoroutine("ANUBIS_Routine_SendDataToServer");
    }

    private IEnumerator GENGHIS_Routine_SendDataToServer()
    {
        //string uriToSendDataTo = "https://api.infinitex.space/game/cardid";  //base URI
        string uriToSendDataTo = "https://api.infinitex.space/game/92116287-7989-45b5-820d-9321c70bb24a";

        UnityWebRequest webRequest = new UnityWebRequest(uriToSendDataTo, "PUT");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + token.access_token); //put access token here!!!
        byte[] rawCardsData = Encoding.UTF8.GetBytes(genghis_dataToSend);
        webRequest.uploadHandler = new UploadHandlerRaw(rawCardsData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            print(genghis_dataToSend);
        }
    }



    private IEnumerator ZEUS_Routine_SendDataToServer()
    {

        //string uriToSendDataTo = "https://api.infinitex.space/game/cardid";  //base URI
        string uriToSendDataTo = "https://api.infinitex.space/game/2e528cd2-1011-4229-978f-ada7aab939cb";


        using UnityWebRequest webRequest = new UnityWebRequest(uriToSendDataTo, "PUT");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + token.access_token); //put access token here!!!
        byte[] rawCardsData = Encoding.UTF8.GetBytes(zeus_dataToSend);
        webRequest.uploadHandler = new UploadHandlerRaw(rawCardsData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            print(zeus_dataToSend);
        }
    }
    private IEnumerator ODIN_Routine_SendDataToServer()
    {
        string uriToSendDataTo = "https://api.infinitex.space/game/6d9788e1-b1f2-409b-b92a-afa198ae64a1";

        UnityWebRequest webRequest = new UnityWebRequest(uriToSendDataTo, "PUT");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + token.access_token); //put access token here!!!
        byte[] rawCardsData = Encoding.UTF8.GetBytes(odin_dataToSend);
        webRequest.uploadHandler = new UploadHandlerRaw(rawCardsData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            print(odin_dataToSend);
        }
    }

    private IEnumerator LEONARDO_Routine_SendDataToServer()
    {
        string uriToSendDataTo = "https://api.infinitex.space/game/4ca611c4-a7f8-4622-a516-7572a55aaf78";

        UnityWebRequest webRequest = new UnityWebRequest(uriToSendDataTo, "PUT");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + token.access_token); //put access token here!!!
        byte[] rawCardsData = Encoding.UTF8.GetBytes(leonardo_dataToSend);
        webRequest.uploadHandler = new UploadHandlerRaw(rawCardsData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            print(leonardo_dataToSend);
        }
    }
    private IEnumerator DUSTIN_Routine_SendDataToServer()
    {
        string uriToSendDataTo = "https://api.infinitex.space/game/3f952db0-e64d-4733-a893-71048d4c39d8";

        UnityWebRequest webRequest = new UnityWebRequest(uriToSendDataTo, "PUT");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + token.access_token); //put access token here!!!
        byte[] rawCardsData = Encoding.UTF8.GetBytes(dustin_dataToSend);
        webRequest.uploadHandler = new UploadHandlerRaw(rawCardsData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            print(dustin_dataToSend);
        }
    }
    private IEnumerator ANUBIS_Routine_SendDataToServer()
    {
        string uriToSendDataTo = "https://api.infinitex.space/game/6ed752f1-b84f-416f-ab28-1638a8c5878b";

        UnityWebRequest webRequest = new UnityWebRequest(uriToSendDataTo, "PUT");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + token.access_token); //put access token here!!!
        byte[] rawCardsData = Encoding.UTF8.GetBytes(anubis_dataToSend);
        webRequest.uploadHandler = new UploadHandlerRaw(rawCardsData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            print(anubis_dataToSend);
        }
    }
}

#region SaveStuff
[System.Serializable]
public class PlayerCards
{
    public int id;
    public int hasBeenBought;
    public float hp;
    public float attack;
    public float defence;
    public float dodge;
    public float critical;
    public int duelsFought;
    public int duelsWon;
    public float winRatio;
    public string question;
    public int powerLevel;
    public PlayerCards(int id, int hasBeenBought, float hp, float attack, float defence, float dodge, float critical, int duelsFought, int duelsWon, float winRatio, string question, int powerLevel)
    {
        this.id = id;
        this.hasBeenBought = hasBeenBought;
        this.hp = hp;
        this.attack = attack;
        this.defence = defence;
        this.dodge = dodge;
        this.critical = critical;
        this.duelsFought = duelsFought;
        this.duelsWon = duelsWon;
        this.winRatio = winRatio;
        this.question = question;
        this.powerLevel = powerLevel;
    }
}
#endregion
[System.Serializable]
public class LoadStuff
{
    public Product product;
    public ProductData productData;
    public string id; //send id is this one!
    public string ownerId;
    public string productId;
}
[System.Serializable]
public class LoadStuffResult
{
    public LoadStuff[] Items;
}
[System.Serializable]
public class Product
{
    public string id;
    public string productId;
    public string title;
}
[System.Serializable]
public class ProductData
{
    public string id;
    public PlayerCards data;
    public bool locked; //if false client can use it, meaning it's in the game
    //if locked don't show in the game
}

[System.Serializable]
public class Transaction
{
    public string id { get; set; }
    public string hash { get; set; }
    public bool proceed { get; set; }
    public string price { get; set; }
}
