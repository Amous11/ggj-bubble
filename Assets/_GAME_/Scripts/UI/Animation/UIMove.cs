using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class UIMove : UIDotweenAnimation<Vector3>
{
    [SerializeField]
    private RectTransform _target;

    [SerializeField]
    private bool _useAnchoredPosition = false;

    private UnityAction _callback;
    private Tween _currentTween;

    private void Awake()
    {
        _target = GetComponent<RectTransform>();
    }

    public override void PlayAnimation(UnityAction onFinishedAction = null)
    {
        _callback = onFinishedAction;

        _currentTween?.Kill();

        if (_startFromBeginning)
        {
            if (_useAnchoredPosition)
            {
                _target.anchoredPosition = _startingValue;
            }
            else
            {
                _target.localPosition = _startingValue;
            }
        }

        _currentTween = _useAnchoredPosition
            ? _target.DOAnchorPos(_targetValue, _forwardAnimationSettings.duration)
                          .SetEase(_forwardAnimationSettings.EaseFunction)
                          .OnComplete(Finished)
            : _target.DOLocalMove(_targetValue, _forwardAnimationSettings.duration)
                          .SetEase(_forwardAnimationSettings.EaseFunction)
                          .OnComplete(Finished);
    }

    public override void PlayAnimationReversed(UnityAction onFinishedAction = null)
    {
        _callback = onFinishedAction;

        _currentTween?.Kill();

        if (_startFromBeginning)
        {
            if (_useAnchoredPosition)
            {
                _target.anchoredPosition = _targetValue;
            }
            else
            {
                _target.localPosition = _targetValue;
            }
        }

        _currentTween = _useAnchoredPosition
            ? _target.DOAnchorPos(_startingValue, _useForwardSettingsForReverse ? _forwardAnimationSettings.duration : _reverseAnimationSettings.duration)
                          .SetEase(_useForwardSettingsForReverse ? _forwardAnimationSettings.EaseFunction : _reverseAnimationSettings.EaseFunction)
                          .OnComplete(Finished)
            : _target.DOLocalMove(_startingValue, _useForwardSettingsForReverse ? _forwardAnimationSettings.duration : _reverseAnimationSettings.duration)
                          .SetEase(_useForwardSettingsForReverse ? _forwardAnimationSettings.EaseFunction : _reverseAnimationSettings.EaseFunction)
                          .OnComplete(Finished);
    }

    public override void ResetAnimation()
    {
        if (_useAnchoredPosition)
        {
            _target.anchoredPosition = _startingValue;
        }
        else
        {
            _target.localPosition = _startingValue;
        }
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
        if (_useAnchoredPosition)
        {
            _startingValue = _target.anchoredPosition;
        }
        else
        {
            _startingValue = _target.localPosition;
        }
    }
}
