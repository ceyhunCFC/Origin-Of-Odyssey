using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BuyCard : MonoBehaviour
{
    public string url = "https://request.infinitex.space/";
    public JSonToken token;
    public InputField HowMuchText;
    public Button purchaseButton;

    void Start()
    {
        token = TokenBridge.tokenTransfer;
    }


    public void BoughtCard()
    {
        purchaseButton.interactable = false;
        StartCoroutine(BuyTheCardIE());
    }
    private IEnumerator BuyTheCardIE()
    {
        string uName = PlayerPrefs.GetString("Email");
        string stringHowMany = HowMuchText.text;

        string jsonData = $"{{\"email\":\"{uName}\",\"price\":\"{stringHowMany}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest("https://request.infinitex.space/", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                OpenWebLink(responseText);
            }
        }

    }

    public void OpenWebLink(string webLink)
    {
        Application.OpenURL(webLink);
        purchaseButton.interactable = true;
    }
}
