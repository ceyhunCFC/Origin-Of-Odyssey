using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ender.Scripts
{
    public class WinLosePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameTxt, heroTxt;
        [SerializeField] private Image heroImg;
        
        public void Initialize(string nameString, string heroNameString, Sprite heroImgSprite)
        {
            nameTxt.text = nameString;
            heroTxt.text = heroNameString;
            heroImg.sprite = heroImgSprite;
        }
    }
}