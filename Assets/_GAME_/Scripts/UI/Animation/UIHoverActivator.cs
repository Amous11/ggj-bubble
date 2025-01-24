using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIHoverActivator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private UIAnimation _animationOnHover;

    [SerializeField]
    private float _delayOnEnter = 0f;

    [SerializeField]
    private float _delayOnExit = 0f;

    private Coroutine _currentCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_currentCoroutine != null)
        {
            _animationOnHover.InterruptAnimation();
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = StartCoroutine(Play(_delayOnEnter, () => _animationOnHover.PlayAnimation()));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_currentCoroutine != null)
        {
            _animationOnHover.InterruptAnimation();
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = StartCoroutine(Play(_delayOnExit, () => _animationOnHover.PlayAnimationReversed()));

    }

    private IEnumerator Play(float delay, UnityAction action)
    {
        yield return delay;
        action?.Invoke();
    }

    private void OnDisable()
    {
        _animationOnHover.ResetAnimation();
    }
}