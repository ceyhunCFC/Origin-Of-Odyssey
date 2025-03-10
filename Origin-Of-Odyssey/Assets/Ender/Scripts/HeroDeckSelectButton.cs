using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ender.Scripts
{
    public class HeroDeckSelectButton : MonoBehaviour
    {
        public Sprite selected,hovered,normal;
        public bool isSelected;
    
        public void OpenHovered()
        {
            if (isSelected)
            {
                return;
            }
            GetComponent<Image>().sprite = hovered;
        }
    
        public void OpenNormal()
        {
            if (isSelected)
            {
                return;
            }
            isSelected = false;
            GetComponent<Image>().sprite = normal;
        }
    
        public void OpenSelected()
        {
            isSelected = true;
            GetComponent<Image>().sprite = selected;
        }
    
    }
}
