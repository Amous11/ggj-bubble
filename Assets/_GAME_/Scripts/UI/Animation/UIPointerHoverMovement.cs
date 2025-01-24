using DG.Tweening;
using UnityEngine;

public class UIPointerHoverMovement : UIPointerOverHandler<UIPointerHoverMovement>
{
    [SerializeField]
    private Transform _target;
    Transform _currentTarget
    {
        get
        {
            if (_target == null)
            {
                _target = transform;
            }
            return _target;
        }
    }
    [SerializeField, Header("Animation")]
    private Vector3 _mouseOverLocalPosition;
    [SerializeField]
    private Vector3 _mouseOutLocalPosition = Vector3.zero;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private bool _forceKillOnHover;
    [SerializeField]
    private bool _forceKillOnExit;
    [SerializeField]
    private Ease _ease;

    public bool IsActivatedMode = false;
    [SerializeField] bool IgnorePointer = false;

    public void CallForcedEffect(bool activated)
    {
        if (activated)
        {
            DOTween.Kill(_currentTarget);
            if (_forceKillOnHover) ForceFinishTween(false);
            _currentTarget.DOLocalMove(_mouseOverLocalPosition, _duration).SetEase(_ease);
        }
        else
        {
            DOTween.Kill(_currentTarget);
            if (_forceKillOnExit) ForceFinishTween(true);
            _currentTarget.DOLocalMove(_mouseOutLocalPosition, _duration).SetEase(_ease);
        }
    }

    protected override void OnPointerEnter()
    {
        if (IgnorePointer) return;
        if (_forceKillOnHover) ForceFinishTween(false);
        _currentTarget.DOLocalMove(_mouseOverLocalPosition, _duration).SetEase(_ease);
    }

    protected override void OnPointerExit()
    {
        if (IgnorePointer) return;
        if (IsActivatedMode) return;
        if (_forceKillOnExit) ForceFinishTween(true);
        _currentTarget.DOLocalMove(_mouseOutLocalPosition, _duration).SetEase(_ease);
    }

    private void ForceFinishTween(bool mouseOut)
    {
        if (IsActivatedMode) return;
        /*if (DOTween.IsTweening(_currentTarget))
        {
            DOTween.Kill(_currentTarget);
            Vector3 position = mouseOut ? _mouseOverLocalPosition : _mouseOutLocalPosition;
            _currentTarget.DOLocalMove(position, 0);
        }*/
    }

    private void OnDisable()
    {
        ForceFinishTween(true);
    }
}