using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class UIScale : UIDotweenAnimation<Vector3>
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private bool _isPunchAnimation = false;

    private UnityAction _callback;
    private Tween _currentTween;

    public void PlayToggle(bool value)
    {
        if (value)
            PlayAnimation();
        else
            PlayAnimationReversed();
    }

    public override void PlayAnimation(UnityAction onFinishedAction = null)
    {
        _currentTween?.Kill();
        if (_startFromBeginning)
        {
            _target.localScale = _startingValue;
        }

        _callback = onFinishedAction;

        _currentTween = _target.DOScale(_targetValue,
            _forwardAnimationSettings.duration)
            .SetEase(_forwardAnimationSettings.EaseFunction)
            .OnComplete(Finished);
    }

    public override void PlayAnimationReversed(UnityAction onFinishedAction = null)
    {
        _currentTween?.Kill();
        if (_isPunchAnimation)
        {
            PlayAnimationPunch(onFinishedAction);
        }
        else
        {
            if (_startFromBeginning)
            {
                _target.localScale = _targetValue;
            }

            _callback = onFinishedAction;

            _currentTween = _target.DOScale(_startingValue,
                _useForwardSettingsForReverse ? _forwardAnimationSettings.duration : _reverseAnimationSettings.duration)
                .SetEase(_useForwardSettingsForReverse ? _forwardAnimationSettings.EaseFunction : _reverseAnimationSettings.EaseFunction)
                .OnComplete(Finished);
        }
    }

    public void PlayAnimationPunch(UnityAction onFinishedAction = null)
    {
        _currentTween?.Kill();
        _callback = onFinishedAction;

        _target.localScale = _startingValue;

        _currentTween = _target.DOPunchScale(_targetValue,
            _forwardAnimationSettings.duration)
            .SetEase(_forwardAnimationSettings.EaseFunction)
            .OnComplete(Finished);
    }

    public override void ResetAnimation()
    {
        _target.localScale = _startingValue;
    }

    public override void InterruptAnimation()
    {
        _currentTween?.Kill();
    }

    private void Finished()
    {
        _callback?.Invoke();
    }

    protected override void ExecuteGrabStartingValue()
    {
        _startingValue = _target.localScale;
    }
}