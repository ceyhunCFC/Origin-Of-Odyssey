using Proyecto26;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{

    public InputField LoginEmail,LoginPassword;
    public InputField RegisterEmail,RegisterPassword,RegisterUserName,RegisterFirstName,RegisterLastName;
    public Text info;

    private string apiKey = "AIzaSyCBurDB1K_PUu_KaD5HqVZqu3gRY1WytPE";    //firabase api
    private string RegisterURL= "https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=";
    private string LoginURL = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=";
    private string databaseURL = "https://origin-of-odyssey-eee04-default-rtdb.firebaseio.com/Users";

    public static string localId;
    public static string userName,firstName,lastName;

    public void AccountLogin()
    {
        info.text = "";
        string email = LoginEmail.text;
        string password = LoginPassword.text;
        LoginAccount(email, password);
    }

    private void LoginAccount(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>(LoginURL + apiKey, userData).Then(
            response =>
            {
                info.text = "Login successful!";
                localId = response.localId;
                RestClient.Get<PlayerData>(databaseURL + "/" + localId + ".json?auth=" + response.idToken).Then(userResponse =>
                {
                    userName = userResponse.userName; 
                }).Catch(error =>
                {
                    Debug.LogError("Error retrieving username: " + error.Message);
                });


            }).Catch(error =>
            {
                Debug.LogError("An error occurred while logging in: " + error.Message);
                info.text = "An error occurred while logging in: " + error.Message;
            });
    }

    public void AccountRegister()
    {
        info.text = "";
        string email = RegisterEmail.text;
        string password = RegisterPassword.text;
        string username= RegisterUserName.text;
        string firstName = RegisterFirstName.text;
        string lastName = RegisterLastName.text;
        RegisterAccount(email, password,username,firstName,lastName);
    }

    private void RegisterAccount(string email, string password,string username,string firstname,string lastname)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>(RegisterURL + apiKey, userData).Then(
            response =>
            {
                info.text = "Register Succsesful!";
                localId = response.localId;
                userName = username;
                firstName = firstname;
                lastName = lastname;
                PostToDatabase( response.idToken);

            }).Catch(error =>
            {
                Debug.LogError("An error occurred while registering: " + error.Message);
                info.text = "An error occurred while registering: " + error.Message;
            });
    }

    private void PostToDatabase( string idTokenTemp = "")
    {
        PlayerData user = new PlayerData();
        RestClient.Put(databaseURL + "/" + localId  +"/UserInfo"+".json?auth=" + idTokenTemp, user);
    }

}
