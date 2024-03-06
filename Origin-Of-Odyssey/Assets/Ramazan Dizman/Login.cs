using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class Login : MonoBehaviour
{

    public InputField accountEmail;
    public InputField accountPassword;
    public InputField accountUserName;
    public Text info;

    private string apiKey = "AIzaSyCBurDB1K_PUu_KaD5HqVZqu3gRY1WytPE";    //firabase api
    private string RegisterURL= "https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=";
    private string LoginURL = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=";
    private string databaseURL = "https://origin-of-odyssey-eee04-default-rtdb.firebaseio.com/Users";

    public static string localId;
    public static string userName;

    public void AccountLogin()
    {
        info.text = "";
        string email = accountEmail.text;
        string password = accountPassword.text;
        LoginAccount(email, password);
    }

    private void LoginAccount(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>(LoginURL + apiKey, userData).Then(
            response =>
            {
                Debug.Log("Giriþ baþarýlý!");
                info.text = "Giriþ baþarýlý!";
                localId = response.localId;
                Debug.Log(localId);
                RestClient.Get<User>(databaseURL + "/" + localId + ".json?auth=" + response.idToken).Then(userResponse =>
                {
                    userName = userResponse.userName; 
                    Debug.Log("Kullanýcý adý: " + userName);
                }).Catch(error =>
                {
                    Debug.LogError("Kullanýcý adý alýnýrken hata oluþtu: " + error.Message);
                });


            }).Catch(error =>
            {
                Debug.LogError("Giriþ sýrasýnda hata oluþtu: " + error.Message);
                info.text = "Giriþ sýrasýnda hata oluþtu: " + error.Message;
            });
    }

    public void AccountRegister()
    {
        info.text = "";
        string email = accountEmail.text;
        string password = accountPassword.text;
        string username=accountUserName.text;
        RegisterAccount(email, password,username);
    }

    private void RegisterAccount(string email, string password,string username)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>(RegisterURL + apiKey, userData).Then(
            response =>
            {
                Debug.Log("Kayýt Baþarýlý");
                info.text = "Kayýt Baþarýlý";
                localId = response.localId;
                userName = username;
                PostToDatabase( response.idToken);

            }).Catch(error =>
            {
                Debug.LogError("Kayýt sýrasýnda hata oluþtu: " + error.Message);
                info.text = "Kayýt sýrasýnda hata oluþtu: " + error.Message;
            });
    }

    private void PostToDatabase( string idTokenTemp = "")
    {
        User user = new User();


        RestClient.Put(databaseURL + "/" + localId + ".json?auth=" + idTokenTemp, user);
    }

}
