using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestionMarkAnim : MonoBehaviour
{
    public float moveSpeed = 5f; // Kartýn hareket hýzý
    private Vector3 targetPosition = new Vector3(-0.1f, 1.55f, 0.7f); // Hedef pozisyon
    private Quaternion targetRotation = Quaternion.Euler(40, 0, 180); // Nihai hedef rotasyonu
    private Vector3 initialPosition; // Kartýn baþlangýçtaki pozisyonu
    private Quaternion initialRotation; // Kartýn baþlangýçtaki rotasyonu
    public GameObject CloseButton;
    public Button QuestionMarkButton;

    public void MoveCardToTargetPosition()
    {
        initialPosition = transform.position; 
        initialRotation = transform.rotation; 
        transform.localRotation = Quaternion.Euler(45, 0, 180); 
        QuestionMarkButton.interactable = false;

        gameObject.tag = "QuestionMark";
        StartCoroutine(MoveCardToPosition());
    }

    private IEnumerator MoveCardToPosition()
    {
        float duration = 0.5f; // Animasyon süresi
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, step);

            float rotationStep = 360f * Time.deltaTime / duration; 
            transform.Rotate(Vector3.forward, rotationStep, Space.Self); 

            timeElapsed += Time.deltaTime;

            yield return null; 
        }

        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;
        CloseButton.SetActive(true);
    }

    public void CloseButtonPressed()
    {
        QuestionMarkButton.interactable = true;
        StartCoroutine(MoveCardBackToInitialPosition());
    }

    private IEnumerator MoveCardBackToInitialPosition()
    {
        while (Vector3.Distance(transform.position, initialPosition) > 0.1f ||
               Quaternion.Angle(transform.rotation, initialRotation) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, moveSpeed * Time.deltaTime * 100f);

            yield return null; 
        }
        gameObject.tag = "Card";
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        CloseButton.SetActive(false); 
    }
}
