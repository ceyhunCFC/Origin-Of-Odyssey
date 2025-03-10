using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ender.Scripts
{
    public class Animations : MonoBehaviour
    {
        public Image animationImageOn, animationImageOff, lineImage, bgImage;
        public Sprite[] sprites;
        public GameObject dot, arrow, mid;
        public Vector3 midPoint,dotPoint,arrowPoint;
        private Tween _tweenMove,_tweenScale,_bgTween,_lineTween;

        private void Start()
        {
            if (dot!=null && arrow!=null && mid!=null)
            {
                dotPoint = dot.transform.position;
                arrowPoint = arrow.transform.position;
                midPoint = mid.transform.position;
            }
        
        }

        public void ScaleUp(RectTransform rectTransform)
        {
            var scale = rectTransform.localScale;
            rectTransform.DOScale(scale.x > 0 ? Vector3.one : new Vector3(-1, 1, 1), .1f);
        }
    
        public void ScaleDown(RectTransform rectTransform)
        {
            var scale = rectTransform.localScale;
            rectTransform.DOScale(scale.x > 0 ? Vector3.one*.8f : new Vector3(-.8f, .8f, .8f), .1f);
        }

        public void BowEnter()
        {
            animationImageOn.gameObject.GetComponent<CanvasGroup>().DOFade(1, .3f);
            animationImageOff.DOColor(new Color(1,1,1,0), .1f);
        }
    
        public void BowExit()
        {
            animationImageOn.gameObject.GetComponent<CanvasGroup>().DOFade(0, .3f);
            animationImageOff.DOColor(new Color(1,1,1,1), .1f);
        }

        public void ButtonAnimationOn()
        {
            _tweenMove?.Complete(true);
            _tweenScale?.Complete(true);
            _bgTween?.Complete(true);
            _lineTween?.Complete(true);
            _tweenMove=dot.transform.DOMove(midPoint, .1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                arrow.transform.position = midPoint;
                arrow.transform.DOMove(arrowPoint, .1f).SetEase(Ease.InOutSine);
            });
        
            _bgTween=bgImage.DOFillAmount(1, .2f);
            _lineTween=lineImage.DOFillAmount(1, .2f);
        
            _tweenScale=dot.transform.DOScale(Vector3.zero, .1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                arrow.transform.DOScale(Vector3.one, .1f).SetEase(Ease.InOutSine);
            });
        }
    
        public void ButtonAnimationOff()
        {
            _tweenMove?.Complete(true);
            _tweenScale?.Complete(true);
            _bgTween?.Complete(true);
            _lineTween?.Complete(true);
            arrow.transform.DOMove(midPoint, .1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                dot.transform.position = midPoint;
                dot.transform.DOMove(dotPoint, .1f).SetEase(Ease.InOutSine);
            });
        
            _bgTween=bgImage.DOFillAmount(0, .2f);
            _lineTween=lineImage.DOFillAmount(0, .2f);
        
            arrow.transform.DOScale(Vector3.zero, .1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                dot.transform.DOScale(Vector3.one, .1f).SetEase(Ease.InOutSine);
            });
        }
    }
}
