using DG.Tweening;
using DG.Tweening.Core.Easing;
using NaughtyAttributes;
using System;
using UnityEngine;

public abstract class UIDotweenAnimation<T> : UIAnimation
{
    [Serializable]
    public class UIDOTweenAnimationSettings : UIAnimationSettings
    {
        [Header("Easing")]
        [SerializeField]
        public bool _useCustomEase;

        [SerializeField, HideIf("_useCustomEase")]
        public Ease _defaultEase;

        [SerializeField, ShowIf("_useCustomEase")]
        public AnimationCurve _customEase;

        public float EaseFunction(float time, float duration, float overshootOrAmplitude, float period)
        {
            if (_useCustomEase)
            {
                return _customEase.Evaluate(time / duration);
            }
            else
            {
                return EaseManager.Evaluate(_defaultEase, null, time, duration, overshootOrAmplitude, period);
            }
        }
    }

    [SerializeField]
    protected T _startingValue;

    [SerializeField]
    protected T _targetValue;

    public T StartingValue => _startingValue;
    public T TargetValue => _targetValue;

    [SerializeField, Tooltip("For forward it will start from Starting Value, and for reverse it will start from Target Value")]
    protected bool _startFromBeginning;

    [Header("Animation Properties"), SerializeField]
    protected UIDOTweenAnimationSettings _forwardAnimationSettings;

    [SerializeField]
    protected bool _useForwardSettingsForReverse = true;

    [SerializeField, HideIf("_useForwardSettingsForReverse")]
    protected UIDOTweenAnimationSettings _reverseAnimationSettings;

    protected abstract void ExecuteGrabStartingValue();

    [Button("Auto assign current starting value")]
    private void AutoGrabStartingValue()
    {
        try
        {
            ExecuteGrabStartingValue();
            Debug.Log("Grabbed Starting Value Successfully!");
        }
        catch
        {
            Debug.LogError("Error grabbing starting value, make sure references are assigned correctly.");
        }

    }
}