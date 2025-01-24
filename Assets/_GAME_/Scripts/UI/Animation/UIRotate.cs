using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class UIRotate : UIDotweenAnimation<Vector3>
{
    [SerializeField]
    private Transform _target;

    private UnityAction _callback;
    private Tween _currentTween;

    public override void PlayAnimation(UnityAction onFinishedAction = null)
    {
        _currentTween?.Kill();
        _callback = onFinishedAction;

        if (_startFromBeginning)
        {
            _target.localEulerAngles = _startingValue;
        }

        _currentTween = _target.DOLocalRotate(_targetValue, _forwardAnimationSettings.duration)
            .SetEase(_forwardAnimationSettings.EaseFunction).OnComplete(Finished);
    }

    public override void PlayAnimationReversed(UnityAction onFinishedAction = null)
    {
        _currentTween?.Kill();
        _callback = onFinishedAction;

        if (_startFromBeginning)
        {
            _target.localEulerAngles = _targetValue;
        }

        _currentTween = _target.DOLocalRotate(_startingValue, _useForwardSettingsForReverse ? _forwardAnimationSettings.duration : _reverseAnimationSettings.duration)
            .SetEase(_useForwardSettingsForReverse ? _forwardAnimationSettings.EaseFunction : _reverseAnimationSettings.EaseFunction)
            .OnComplete(Finished);
    }

    public override void ResetAnimation()
    {
        _target.localEulerAngles = _startingValue;
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
        _startingValue = _target.localEulerAngles;
    }
}