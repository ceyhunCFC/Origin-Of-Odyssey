using UnityEngine;

namespace Ender.Scripts
{
    public class HoverInfo : MonoBehaviour
    {
        public HoverInfoType hoverInfoType;
        public string infoText;
    
        public void OnPointerEnter()
        {
            CardDisplay cardDisplay=null;
            CardInformation cardInformation=null;
            if (hoverInfoType==HoverInfoType.CardUI)
            {
                cardDisplay=gameObject.GetComponent<CardDisplay>();
            }
            else if (hoverInfoType==HoverInfoType.CardInGame)
            {
                cardInformation=gameObject.GetComponent<CardInformation>();
            }
            HoverInfoManager.Instance.OpenInfo(hoverInfoType, infoText,cardDisplay,cardInformation);
        }

        public void OnPointerExit()
        {
            HoverInfoManager.Instance.CloseInfo();
        }
    }
}
