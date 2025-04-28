using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningScript : MonoBehaviour
{
    public GameObject Versus;
    public GameObject SettingCanvas;
    public Image leftimage, rightimage;
    public string OwnMainCard, CompetitorMainCard;
    public TextMeshProUGUI OwnName, CompetitorName, OwnHero, CompetitorHero;

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
        print("vs open....");
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
        /*PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();

        foreach (PlayerController pc in playerControllers)
        {
            if (pc.PV.IsMine) 
            {
                OwnMainCard = pc.OwnMainCard;
                CompetitorMainCard = pc.CompetitorMainCard;
                break;
            }
        }*/

        GameManager[] gameManager = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
        OwnMainCard = gameManager[0].MasterMainCard;
        CompetitorMainCard = gameManager[0].OtherrMainCard;
        OwnName.text = gameManager[0].MasterPlayerName;
        CompetitorName.text = gameManager[0].OtherPlayerName;
        OwnHero.text = OwnMainCard;
        CompetitorHero.text = CompetitorMainCard;
    }

    void SetCardImages()
    {
        

        string competitorCardPath = $"NftCarddsİmage/{CompetitorMainCard}";

        Sprite competitorCardSprite = Resources.Load<Sprite>("CardImages/" + CompetitorMainCard);
        string ownCardPath = $"NftCarddsİmage/{OwnMainCard}";

        Sprite ownCardSprite = Resources.Load<Sprite>("CardImages/" + OwnMainCard);

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
