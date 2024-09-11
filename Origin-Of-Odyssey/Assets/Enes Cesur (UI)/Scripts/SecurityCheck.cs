using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SecurityCheck : MonoBehaviour
{
    public Transform[] images;
    public float[] angles = { 0f, 90f, 180f, 270f };
    public Toggle robotToggle;
    public GameObject darkPanel,loginPage;
    public Text alignmentText;

    private int[] currentImageIndices;
    private int referenceImageIndex = 0;

    void Start()
    {
        currentImageIndices = new int[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            RotateImage(images[i], i);
        }
    }

    void RotateImage(Transform image, int index)
    {
        float randomAngle = angles[Random.Range(0, angles.Length)];
        image.rotation = Quaternion.Euler(0f, 0f, randomAngle);
        currentImageIndices[index] = randomAngleIndex(randomAngle);
    }

    public void RotateLeft(int imageIndex)
    {
        int currentAngleIndex = currentImageIndices[imageIndex];
        int newAngleIndex = (currentAngleIndex + 1) % angles.Length;
        float newAngle = angles[newAngleIndex];
        images[imageIndex].rotation = Quaternion.Euler(0f, 0f, newAngle);
        currentImageIndices[imageIndex] = newAngleIndex;
    }

    public void RotateRight(int imageIndex)
    {
        int currentAngleIndex = currentImageIndices[imageIndex];
        int newAngleIndex = (currentAngleIndex - 1 + angles.Length) % angles.Length;
        float newAngle = angles[newAngleIndex];
        images[imageIndex].rotation = Quaternion.Euler(0f, 0f, newAngle);
        currentImageIndices[imageIndex] = newAngleIndex;
    }

    private int randomAngleIndex(float angle)
    {
        for (int i = 0; i < angles.Length; i++)
        {
            if (angles[i] == angle)
            {
                return i;
            }
        }
        return -1;
    }

    public void CheckAlignment()
    {
        bool aligned = true;
        Quaternion referenceRotation = images[referenceImageIndex].rotation;

        foreach (Transform image in images)
        {
            if (image.rotation != referenceRotation && image != images[referenceImageIndex])
            {
                aligned = false;
                break;
            }
        }

        if (aligned)
        {   
            
            robotToggle.isOn = true;
            darkPanel.SetActive(false);
            loginPage.SetActive(true);
        }
        else
        {
            robotToggle.isOn = false;
            alignmentText.text = "The arrow is not in the right direction!";
            alignmentText.color = Color.red;

        }
    }


}
