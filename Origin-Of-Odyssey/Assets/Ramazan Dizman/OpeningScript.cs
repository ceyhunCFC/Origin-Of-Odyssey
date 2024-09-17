using System.Collections;
using UnityEngine;

public class OpeningScript : MonoBehaviour
{
    public GameObject Versus;
    public GameObject SettingCanvas;
    public GameObject board;

    void Start()
    {
        StartCoroutine(CloseVersusAfterDelay(3f));
    }

    IEnumerator CloseVersusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Versus.SetActive(false);
        board.SetActive(true);
    }

    public void OpenSettingsCanvas()
    {
        SettingCanvas.SetActive(true);
    }

    public void CloseSettingsCanvas()
    {
        SettingCanvas.SetActive(false);
    }
}
