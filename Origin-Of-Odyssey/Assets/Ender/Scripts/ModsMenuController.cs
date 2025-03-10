using System;
using System.Collections.Generic;
using DG.Tweening;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ender.Scripts
{
    public class ModsMenuController : MonoBehaviour
    {
        public List<ModButton> modButtons=new List<ModButton>();
        private readonly List<Vector3> _modButtons=new List<Vector3>();
        [SerializeField] private Image bgImage;
        

        private void Awake()
        {
            foreach (var modButton in modButtons)
            {
                _modButtons.Add(modButton.gameObject.transform.localPosition);
            }
        }

        public void NextMode()
        {
            foreach (var button in modButtons)
            {
                var index = (button.index + 1) % _modButtons.Count;
                button.gameObject.transform.DOScale(index == 1 ? new Vector3(1.3f, 1.3f, 1.3f) : Vector3.one, .3f);
                if (index==1)
                {
                    button.gameObject.transform.SetAsLastSibling();
                    bgImage.DOColor(Color.black, .15f).OnComplete(() =>
                    {
                        bgImage.sprite = button.bgSprite;
                        bgImage.DOColor(Color.white, .15f);
                    });
                }
                button.gameObject.transform.DOLocalMove(_modButtons[index], .3f);
                button.index = index;
            }
        }
        
        public void PreviousMode()
        {
            foreach (var button in modButtons)
            {
                var index = (button.index - 1 + _modButtons.Count) % _modButtons.Count;
                button.gameObject.transform.DOScale(index == 1 ? new Vector3(1.3f, 1.3f, 1.3f) : Vector3.one, .3f);
                if (index==1)
                {
                    button.gameObject.transform.SetAsLastSibling();
                    bgImage.DOColor(Color.black, .15f).OnComplete(() =>
                    {
                        bgImage.sprite = button.bgSprite;
                        bgImage.DOColor(Color.white, .15f);
                    });
                }
                button.gameObject.transform.DOLocalMove(_modButtons[index], .3f);
                button.index = index;
            }
        }
    }

    [Serializable]
    public class ModButton
    {
        public GameObject gameObject;
        public int index;
        public Sprite bgSprite;
    }
}