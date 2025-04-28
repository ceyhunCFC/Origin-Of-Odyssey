using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI talkText;
    [SerializeField] private GameObject cloud;
    public void Talk(string text)
    {
        talkText.text = text;
        cloud.SetActive(true);
        StartCoroutine(Remover());
    }

    IEnumerator Remover()
    {
        yield return new WaitForSeconds(2);

       cloud.SetActive(false);
    }
    
}
