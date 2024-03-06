using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class Login : MonoBehaviour
{

    public InputField accountUserName;
    public InputField accountPassword;
    public Text info;

    public string apiKey = "AIzaSyCBurDB1K_PUu_KaD5HqVZqu3gRY1WytPE";     //firebaseden gelen apikey
    public string RegisterURL= "https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=AIzaSyCBurDB1K_PUu_KaD5HqVZqu3gRY1WytPE";
    public string LoginURL = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=AIzaSyCBurDB1K_PUu_KaD5HqVZqu3gRY1WytPE";


    public void AccountLogin()
    {
        info.text = "";
        string email = accountUserName.text;
        string password = accountPassword.text;
        StartCoroutine(LoginAccount(email, password));
    }

    IEnumerator LoginAccount(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        using (UnityWebRequest www = UnityWebRequest.Post(LoginURL, form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
                info.text += www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                info.text += www.result;
                Debug.Log(responseText);
            }
        }
    }

    /*
    public void LoginUser(string email, string password)
    {
        StartCoroutine(SendLoginRequest(email, password));
    }

    IEnumerator SendLoginRequest(string email, string password)
    {
        // Giriþ verilerini oluþtur
        string requestBody = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";

        // HTTP POST isteði oluþtur
        UnityWebRequest request = new UnityWebRequest(LoginURL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Ýstek gönder
        yield return request.SendWebRequest();

        // Ýstek sonucunu iþle
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Giriþ baþarýlý!");
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Giriþ sýrasýnda hata oluþtu: " + request.error);
        }
    }       */










    IEnumerator Patch()
    {
        Debug.Log("Patch");
        string jsonData = "{\"name\": \"Ramazan\"}";

        // Veri güncellenecek URL (Ramazan'ýn olduðu yere dikkat edin)
        string url = "https://origin-of-odyssey-eee04-default-rtdb.firebaseio.com/.json";

        // UnityWebRequest oluþtur
        UnityWebRequest www = UnityWebRequest.Put(url, jsonData);
        www.method = "PATCH"; // PATCH yöntemini kullanarak güncelleme yapýlýr

        // Ýsteði gönder ve yanýtý bekle
        yield return www.SendWebRequest();

        // Ýsteðin sonucunu kontrol et
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Veri güncelleme baþarýsýz: " + www.error);
        }
        else
        {
            Debug.Log("Veri güncelleme baþarýlý!");
        }

    }


    IEnumerator Postwww()
    {
        Debug.Log("Postwww");
        string jsonData = "{\"name\": \"Dizman\"}";
        string url = "https://origin-of-odyssey-eee04-default-rtdb.firebaseio.com/.json";
        string apikey = "BOoYh-C9I79XwoufvOF5iQLqX2mxsO2MbyiYL2SJIyp6Hu41iFCJ6y-w1N3VErqxj2PYNbtSyu_oE-P4EuigKgk";

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, jsonData))
        {
            // Anahtar ekleme
            www.SetRequestHeader("Authorization", "Bearer" + apikey);

            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            www.uploadHandler.contentType = "application/json";
            // Ýsteði gönder ve yanýtý bekle
            yield return www.SendWebRequest();

            // Ýsteðin sonucunu kontrol et
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("POST isteði baþarýsýz: " + www.error);
            }
            else
            {
                Debug.Log("POST isteði baþarýlý! Yanýt: " + www.downloadHandler.text);
            }
        }


    }

    IEnumerator Get()
    {
        Debug.Log("Get");
        string url = "https://origin-of-odyssey-eee04-default-rtdb.firebaseio.com/.json";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("get isteði baþarýsýz: " + www.error);
            }
            else
            {
                Debug.Log("get isteði baþarýlý! Yanýt: " + www.downloadHandler.text);
            }
        }
    }
}
