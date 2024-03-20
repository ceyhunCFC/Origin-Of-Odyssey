using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCard : MonoBehaviour
{
    public GameObject prefab; 

    public void OnButtonPress()
    {
       
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        if (clickedButton != null)
        {
            Text buttonText = clickedButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                GameObject panel = GameObject.FindWithTag("CardContanierTag"); 
                if (panel != null)
                {
                   
                    GameObject newButton = Instantiate(prefab, panel.transform);

                    
                    newButton.transform.localPosition = new Vector3(0, 0, 0);

                    Text newButtonText = newButton.GetComponentInChildren<Text>();
                    if (newButtonText != null)
                    {
                        newButtonText.text = buttonText.text;
                    }
                }
            }
        }
    }
}
