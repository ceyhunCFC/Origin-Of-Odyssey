using UnityEngine;
using Proyecto26;

public class StoragePP : MonoBehaviour
{
    private string url = "https://firebasestorage.googleapis.com/v1beta/projects/origin-of-odyssey-eee04/buckets/origin-of-odyssey-eee04.appspot.com";

    private void Start()
    {
        string userData = "{\"email\":\"" + "rambodizma972n@gmail.com" + "\",\"password\":\"" + "asdasd" + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=AIzaSyCBurDB1K_PUu_KaD5HqVZqu3gRY1WytPE", userData).Then(
            response =>
            {
                GetFileNameFromUrl(response.idToken);
            }).Catch(error =>
            {
                Debug.LogError("errorrr " + error.Message);
            });
    }

    void GetFileNameFromUrl(string idtoken)
    {
        string requestUrl = url + "?access_token=" + idtoken;

        RestClient.Get(requestUrl)
            .Then(response =>
            {
                Debug.Log("Response: " + response.Text);
            })
            .Catch(error =>
            {
                Debug.LogError("Error: " + error.Message);
            });
    }
}


