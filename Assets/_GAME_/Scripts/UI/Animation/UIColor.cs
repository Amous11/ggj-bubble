using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIColor : UIAnimation
{
    [SerializeField]
    private Image _target;
    //
    [Space]
    [Header("Animation")]
    [SerializeField] private Color _originalColor;
    [SerializeField] private Color _ColorTarget;
    [SerializeField] private float _duration;
    [SerializeField] private bool _startFromZero;

    [Space, Header("Tweening")]
    [SerializeField] private bool _customEase = false;
    [SerializeField, HideIf("_customEase")] private Ease _ease;
    [SerializeField, ShowIf("_customEase")] private AnimationCurve _customEaseCurve;
    [SerializeField] private bool _isPunchAnimation = false;
    private UnityAction _callback;
    private Tween _currentTween;
    public override void InterruptAnimation()
    {
        _currentTween?.Kill();
    }

    public override void PlayAnimation(UnityAction onFinishedAction = null)
    {
        _currentTween?.Kill();
        if (_startFromZero)
        {
            _target.color = _originalColor;
        }
        _callback = onFinishedAction;
        if (_customEase)
        {
            _currentTween = _target.DOColor(_ColorTarget, _duration).SetEase(_customEaseCurve).OnComplete(Finished);
        }
        else
        {
            _currentTween = _target.DOColor(_ColorTarget, _duration).SetEase(_ease).OnComplete(Finished);
        }
    }

    public override void PlayAnimationReversed(UnityAction onFinishedAction = null)
    {
        _currentTween?.Kill();

        if (_startFromZero)
        {
            _target.color = _ColorTarget;
        }

        _callback = onFinishedAction;

        if (_customEase)
        {
            _currentTween = _target.DOColor(_originalColor, _duration).SetEase(_customEaseCurve).OnComplete(Finished);
        }
        else
        {
            _currentTween = _target.DOColor(_originalColor, _duration).SetEase(_ease).OnComplete(Finished);
        }
    }

    public override void ResetAnimation()
    {
        _target.color = _originalColor;
    }
    private void Finished()
    {
        _callback?.Invoke();
    }
}