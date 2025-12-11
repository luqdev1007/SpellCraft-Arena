using UnityEngine;
using DG.Tweening;

public class FloatingPulsarOrb : MonoBehaviour
{
    [SerializeField] private Transform _view;

    private void Start()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMoveY(transform.position.y + 0.25f, 1.5f).SetEase(Ease.InOutSine))
           .Append(transform.DOMoveY(transform.position.y, 1.5f).SetEase(Ease.InOutSine));

        seq.Join(transform.DOScale(transform.localScale * 1.05f, 0.75f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo));

        seq.SetLoops(-1, LoopType.Restart);
    }
}
