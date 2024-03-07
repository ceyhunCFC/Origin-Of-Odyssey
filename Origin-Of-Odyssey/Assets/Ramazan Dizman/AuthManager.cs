using Proyecto26;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
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
                info.text = "Giri� ba�ar�l�!";
                localId = response.localId;
                RestClient.Get<PlayerData>(databaseURL + "/" + localId + ".json?auth=" + response.idToken).Then(userResponse =>
                {
                    userName = userResponse.userName; 
                }).Catch(error =>
                {
                    Debug.LogError("Kullan�c� ad� al�n�rken hata olu�tu: " + error.Message);
                });


            }).Catch(error =>
            {
                Debug.LogError("Giri� s�ras�nda hata olu�tu: " + error.Message);
                info.text = "Giri� s�ras�nda hata olu�tu: " + error.Message;
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
                info.text = "Kay�t Ba�ar�l�";
                localId = response.localId;
                userName = username;
                PostToDatabase( response.idToken);

            }).Catch(error =>
            {
                Debug.LogError("Kay�t s�ras�nda hata olu�tu: " + error.Message);
                info.text = "Kay�t s�ras�nda hata olu�tu: " + error.Message;
            });
    }

    private void PostToDatabase( string idTokenTemp = "")
    {
        PlayerData user = new PlayerData();


        RestClient.Put(databaseURL + "/" + localId + ".json?auth=" + idTokenTemp, user);
    }

}
