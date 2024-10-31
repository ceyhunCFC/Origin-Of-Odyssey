using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OpeningScript : MonoBehaviour
{
    public GameObject Versus;
    public GameObject SettingCanvas;
    public Image leftimage, rightimage;
    public string OwnMainCard, CompetitorMainCard;

    void Start()
    {
        Versus.SetActive(false);  
        StartCoroutine(WaitForDataAndShowVersus(3f));
    }

    IEnumerator WaitForDataAndShowVersus(float delay)
    {
        while (string.IsNullOrEmpty(OwnMainCard) || string.IsNullOrEmpty(CompetitorMainCard))
        {
            GetData();
            yield return new WaitForSeconds(0.1f); 
        }

        Versus.SetActive(true);

        SetCardImages();

        yield return new WaitForSeconds(delay);
        Versus.SetActive(false);
    }

    public void OpenSettingsCanvas()
    {
        SettingCanvas.SetActive(true);
    }

    public void CloseSettingsCanvas()
    {
        SettingCanvas.SetActive(false);
    }

    void GetData()
    {
        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();

        foreach (PlayerController pc in playerControllers)
        {
            if (pc.PV.IsMine) 
            {
                OwnMainCard = pc.OwnMainCard;
                CompetitorMainCard = pc.CompetitorMainCard;
                break;
            }
        }
    }

    void SetCardImages()
    {
        string ownCardPath = $"NftCardds›mage/{OwnMainCard}"; 
        string competitorCardPath = $"NftCardds›mage/{CompetitorMainCard}";

        Sprite ownCardSprite = Resources.Load<Sprite>(ownCardPath);
        Sprite competitorCardSprite = Resources.Load<Sprite>(competitorCardPath);

        if (ownCardSprite != null)
        {
            leftimage.sprite = ownCardSprite;
        }
        else
        {
            Debug.LogWarning($"Image not found for OwnMainCard: {ownCardPath}");
        }

        if (competitorCardSprite != null)
        {
            rightimage.sprite = competitorCardSprite;
        }
        else
        {
            Debug.LogWarning($"Image not found for CompetitorMainCard: {competitorCardPath}");
        }
    }
}
