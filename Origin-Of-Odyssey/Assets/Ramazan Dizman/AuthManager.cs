using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{

    public InputField LoginEmail,LoginPassword;
    public InputField RegisterEmail,RegisterPassword,RegisterUserName,RegisterFirstName,RegisterLastName;
    public InputField ResetPasswordEmail;
    public Text loginInfo,registerInfo;
    public Toggle robotToggle,robotToggle1,userAgreementToggle,RemindMeToggle;
    public InputField[] inputFields;


    private readonly string apiKey = "AIzaSyCBurDB1K_PUu_KaD5HqVZqu3gRY1WytPE";    //firabase api
    private readonly string RegisterURL = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=";
    private readonly string LoginURL = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=";
    private readonly string databaseURL = "https://origin-of-odyssey-eee04-default-rtdb.firebaseio.com/Users";

    public static string localId,idToken;
    public static string userName, firstName, lastName;
    public static string[] playerDeckArray;

    public bool RemindMe;

    
    private void Start()
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        {
            AccountLogin();
        }

     //   LoginAccount("test1@test.com", "test1@test.com");

    }

    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectNextInputField();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AccountLogin();
        }

    }
    public void SelectNextInputField()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i].isFocused)
            {
                int nextIndex = (i + 1) % inputFields.Length;
                inputFields[nextIndex].Select();
                return;
            }
        }
        if (inputFields.Length > 0)
        {
            inputFields[0].Select();
        }
    }
    public void AccountLogin()
    {
        /*if(PlayerPrefs.GetInt("IsLoggedIn") == 0)
        {
            if (!robotToggle.isOn)
            {
                loginInfo.text = "Please verify the CAPTCHA!";
                loginInfo.color = Color.red;
                return;
            }
        }*/
         
        loginInfo.text = "";
        string email = LoginEmail.text;
        string password = LoginPassword.text;

        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        {
            email = PlayerPrefs.GetString("Username");
            password = PlayerPrefs.GetString("Password");
        }    
        LoginAccount(email, password);
    }

    private void LoginAccount(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>(LoginURL + apiKey, userData).Then(
            response =>
            {
                //Get IdInfo
                localId = response.localId;
                idToken = response.idToken;

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

                /* string emailVerification = "{\"idToken\":\"" + response.idToken + "\"}";
                 RestClient.Post(
                     "https://www.googleapis.com/identitytoolkit/v3/relyingparty/getAccountInfo?key=" + apiKey,
                     emailVerification).Then(
                     emailResponse =>
                     {
                         EmailConfirmationInfo emailConfirmationInfo = JsonUtility.FromJson<EmailConfirmationInfo>(emailResponse.Text);

                         if (emailConfirmationInfo.users[0].emailVerified)
                         {
                             loginInfo.text = "Login successful!";

                             //Get IdInfo
                             localId = response.localId;
                             idToken = response.idToken;

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
                             if (RemindMe == true)
                             {
                                 PlayerPrefs.SetString("Username", email);
                                 PlayerPrefs.SetString("Password", password);
                                 PlayerPrefs.SetInt("IsLoggedIn", 1);
                             }
                             //Get PlayerDeck 
                             RestClient.Get(databaseURL + "/" + localId + "/PlayerDeck" + ".json?auth=" + response.idToken).Then(PlayerDeck =>
                             {
                                 playerDeckArray = ParseJsonArray(PlayerDeck.Text);
                             }).Catch(error =>
                             {
                                 Debug.LogError("Error retrieving playerdeck: " + error.Message);
                             });
                             StartCoroutine(LoadMainMenu());
                         }
                         else
                         {
                             loginInfo.text = "email not verified";
                         }
                     }).Catch(error =>
                     {
                         Debug.Log(error.Message);
                     }); */



            }).Catch(error =>
            {
                PlayerPrefs.SetString("Username", null);
                PlayerPrefs.SetString("Password", null);
                PlayerPrefs.SetInt("IsLoggedIn", 0);
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
        if (!robotToggle1.isOn)
        {
            registerInfo.text = "Please verify the CAPTCHA!";
            registerInfo.color = Color.red;
            return;
        }
        if (!userAgreementToggle.isOn)
        {
            registerInfo.text = "Please accept the user agreement!";
            registerInfo.color = Color.red;
            return;
        }
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
                string emailVerification = "{\"requestType\":\"VERIFY_EMAIL\",\"idToken\":\"" + response.idToken + "\"}";
                RestClient.Post(
                    "https://www.googleapis.com/identitytoolkit/v3/relyingparty/getOobConfirmationCode?key=" + apiKey,
                    emailVerification);
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
        RestClient.Put(databaseURL + "/" + localId + "/UserInfo" + ".json?auth=" + idTokenTemp, user)
            .Then(userinfo =>
            {

            }).Catch(error =>
            {
                Debug.LogError("An error saved to userinfo");
            });

        List<string> cardNames = new List<string>();
        ZeusCard zeusCard = new ZeusCard();
        StandartCards standartCards = new StandartCards();

        cardNames.Add(zeusCard.cardName);            //zeusname add
        foreach (Minion minion in zeusCard.minions)
        {
            cardNames.Add(minion.name);             //zeus minions add
        }
        foreach (Spell spell in zeusCard.spells)
        {
            cardNames.Add(spell.name);             //zeus spells add
        }
        /*int remainingCardsCount = 40 - cardNames.Count;

        foreach (StandartCard standartCard in standartCards.standartcards)
        {
            if (remainingCardsCount <= 0)
            {
                break;                                                                              //zeuskart d���nda kartlar� 40 a tamamlamak i�in demo i�in kald�r�ld�
            }
            if (!cardNames.Contains(standartCard.name))
            {
                cardNames.Add(standartCard.name);    //40-zeuscards and add standartcards
                remainingCardsCount--;
            }
        }   */
        string jsonData = "[" + string.Join(",", cardNames.ConvertAll(name => "\"" + name + "\"").ToArray()) + "]";

        RestClient.Put(databaseURL + "/" + localId + "/PlayerDeck" + ".json?auth=" + idToken, jsonData)
           .Then(response =>
           {

           })
           .Catch(error =>
           {
               Debug.LogError("An error saved to cards data: " + error.Message);
           });
    }

    public void ToggleRemindMe(bool toggle)
    {
        RemindMe = toggle;
    }

    public void ResetPasswordButton()
    {
        SendPasswordResetEmail(ResetPasswordEmail.text);
    }

    private void SendPasswordResetEmail(string email)
    {
        string resetPasswordData = "{\"requestType\":\"PASSWORD_RESET\",\"email\":\"" + email + "\"}";
        RestClient.Post(
            "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + apiKey,
            resetPasswordData).Then(
            response =>
            {
            }).Catch(error =>
            {
                Debug.LogError("Reset Password error " + error.Message);
            });
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

    [Serializable]
    private class EmailConfirmationInfo
    {
        public UserInfo[] users;
    }

    [Serializable]
    public class UserInfo
    {
        public bool emailVerified;
    }
}
