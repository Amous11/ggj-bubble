using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIFade : UIDotweenAnimation<float>
{
    [SerializeField]
    private bool _isImage;

    [SerializeField, ShowIf("_isImage")]
    private Image _image;

    [SerializeField, HideIf("_isImage")]
    private CanvasGroup _canvasGroup;

    private UnityAction _action;
    private Tween _currentTween;

    public override void PlayAnimation(UnityAction onFinishedAction = null)
    {
        _currentTween?.Kill();
        _action = onFinishedAction;


        if (_startFromBeginning)
        {
            if (_isImage)
            {
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _startingValue);
            }
            else
            {
                _canvasGroup.alpha = _startingValue;
            }
        }

        if (_isImage)
        {
            _currentTween = _image.DOFade(_targetValue, _forwardAnimationSettings.duration)
                .SetEase(_forwardAnimationSettings.EaseFunction).OnComplete(OnFadeInComplete);
        }
        else
        {
            _currentTween = _canvasGroup.DOFade(_targetValue, _forwardAnimationSettings.duration)
                .SetEase(_forwardAnimationSettings.EaseFunction).OnComplete(OnFadeInComplete);
        }

        ToggleInteraction(true);
    }

    public override void PlayAnimationReversed(UnityAction onFinishedAction = null)
    {
        _currentTween?.Kill();
        _action = onFinishedAction;

        if (_startFromBeginning)
        {
            if (_isImage)
            {
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _targetValue);
            }
            else
            {
                _canvasGroup.alpha = _targetValue;
            }
        }

        if (_isImage)
        {
            _image.DOFade(_startingValue, _useForwardSettingsForReverse ? _forwardAnimationSettings.duration : _reverseAnimationSettings.duration)
                .SetEase(_useForwardSettingsForReverse ? _forwardAnimationSettings.EaseFunction : _reverseAnimationSettings.EaseFunction)
                .OnComplete(OnFadeInComplete);
        }
        else
        {
            _canvasGroup.DOFade(_startingValue, _useForwardSettingsForReverse ? _forwardAnimationSettings.duration : _reverseAnimationSettings.duration)
                .SetEase(_useForwardSettingsForReverse ? _forwardAnimationSettings.EaseFunction : _reverseAnimationSettings.EaseFunction)
                .OnComplete(OnFadeInComplete);
        }

        ToggleInteraction(false);
    }

    public override void ResetAnimation()
    {
        if (_isImage)
        {
            _image.DOFade(_startingValue, 0);
        }
        else
        {
            _canvasGroup.DOFade(_startingValue, 0);
        }
    }

    public override void InterruptAnimation()
    {
        _currentTween?.Kill();
    }

    private void ToggleInteraction(bool state)
    {

        if (_isImage)
        {
            _image.raycastTarget = state;
        }
        else
        {
            _canvasGroup.interactable = state;
            _canvasGroup.blocksRaycasts = state;
        }
    }

    private void OnFadeOutComplete()
    {
        _action?.Invoke();
    }

    private void OnFadeInComplete()
    {
        _action?.Invoke();
    }

    protected override void ExecuteGrabStartingValue()
    {
        _startingValue = _canvasGroup.alpha;
    }
}