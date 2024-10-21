using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Amount : MonoBehaviour
{
    
    public Text userNameText; 
    public PlayerData playerData; 

    void Start()
    {
        playerData = new PlayerData();
        UpdateUserNameOnUI();
    }

    void UpdateUserNameOnUI()
    {
        
        string userName = playerData.UserName;
        userNameText.text =userName;
        
    }
}


