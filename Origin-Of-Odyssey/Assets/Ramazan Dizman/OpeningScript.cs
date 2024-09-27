using System.Collections;
using UnityEngine;

public class OpeningScript : MonoBehaviour
{
    public GameObject Versus;
    public GameObject SettingCanvas;

    void Start()
    {
        StartCoroutine(CloseVersusAfterDelay(3f));
    }

    IEnumerator CloseVersusAfterDelay(float delay)
    {
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
}
