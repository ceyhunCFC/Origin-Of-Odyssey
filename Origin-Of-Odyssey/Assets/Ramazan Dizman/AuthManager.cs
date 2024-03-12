using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{

    public InputField LoginEmail,LoginPassword;
    public InputField RegisterEmail,RegisterPassword,RegisterUserName,RegisterFirstName,RegisterLastName;
    public Text loginInfo,registerInfo;

    private readonly string apiKey = "AIzaSyCBurDB1K_PUu_KaD5HqVZqu3gRY1WytPE";    //firabase api
    private readonly string RegisterURL = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=";
    private readonly string LoginURL = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=";
    private readonly string databaseURL = "https://origin-of-odyssey-eee04-default-rtdb.firebaseio.com/Users";

    public static string localId,idToken;
    public static string userName, firstName, lastName;
    public static string[] playerDeckArray;

    public void AccountLogin()
    {
        loginInfo.text = "";
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
                loginInfo.text = "Login successful!";

                //Get IdInfo
                localId = response.localId;
                idToken=response.idToken;

                //Get UserInfo
                RestClient.Get<PlayerData>(databaseURL + "/" + localId + "/UserInfo" + ".json?auth=" + response.idToken).Then(userResponse =>
                {
                    userName = userResponse.userName;
                    firstName = userResponse.firstName;
                    lastName = userResponse.lastName;
                }).Catch(error =>
                {
                    Debug.LogError("Error retrieving username: " + error.Message);
                });

                //Get PlayerDeck 
                RestClient.Get(databaseURL + "/" + localId + "/PlayerDeck" + ".json?auth=" + response.idToken).Then(PlayerDeck =>
                {
                    playerDeckArray = ParseJsonArray(PlayerDeck.Text);
                }).Catch(error =>
                {
                    Debug.LogError("Error retrieving playerdeck: " + error.Message);
                });
                StartCoroutine(LoadMainMenu());
            }).Catch(error =>
            {
                Debug.LogError("An error occurred while logging in: " + error.Message);
                loginInfo.text = "An error occurred while logging in: " + error.Message;
            });
    }
    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("MainMenuScene");
    }

    public void AccountRegister()
    {
        registerInfo.text = "";
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
                registerInfo.text = "Register Succsesful!";
                localId = response.localId;
                userName = username;
                firstName = firstname;
                lastName = lastname;
                idToken=response.idToken;
                PostToDatabase( response.idToken);

            }).Catch(error =>
            {
                Debug.LogError("An error occurred while registering: " + error.Message);
                registerInfo.text = "An error occurred while registering: " + error.Message;
            });
    }

    private void PostToDatabase( string idTokenTemp = "")
    {
        PlayerData user = new PlayerData();
        RestClient.Put(databaseURL + "/" + localId  +"/UserInfo"+".json?auth=" + idTokenTemp, user);
    }

    //For json to array
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
