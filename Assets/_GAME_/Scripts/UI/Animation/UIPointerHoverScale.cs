using DG.Tweening;
using UnityEngine;

public class UIPointerHoverScale : UIPointerOverHandler<UIPointerHoverScale>
{
    [SerializeField]
    private Transform _target;
    Transform currentTarget
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
    private Vector3 _mouseOverScale;
    [SerializeField]
    public Vector3 _mouseOutScale = Vector3.one;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private bool _forceKillOnHover;
    [SerializeField]
    private bool _forceKillOnExit;
    [SerializeField]
    private Ease _ease;
    public bool IsActivatedMode = false;
    public Vector3 MouseOverScale => _mouseOverScale;
    public Vector3 MouseOutScale => _mouseOutScale;

    public void CallForcedEffect(bool activated)
    {
        if (activated)
        {
            if (_forceKillOnHover) ForceFinishTween(false);
            currentTarget.DOScale(_mouseOverScale, _duration).SetEase(_ease);
        }
        else
        {
            if (_forceKillOnExit) ForceFinishTween(true);
            currentTarget.DOScale(_mouseOutScale, _duration).SetEase(_ease);
        }
    }

    protected override void OnPointerEnter()
    {
        if (IsActivatedMode) return;
        if (_forceKillOnHover) ForceFinishTween(false);
        currentTarget.DOScale(_mouseOverScale, _duration).SetEase(_ease);
    }

    protected override void OnPointerExit()
    {
        if (IsActivatedMode) return;
        if (_forceKillOnExit) ForceFinishTween(true);
        currentTarget.DOScale(_mouseOutScale, _duration).SetEase(_ease);
    }

    private void ForceFinishTween(bool mouseOut)
    {
        if (IsActivatedMode) return;
        if (DOTween.IsTweening(currentTarget))
        {
            DOTween.Kill(currentTarget);
            currentTarget.localScale = mouseOut ? _mouseOutScale : _mouseOverScale;
        }
    }

    private void OnDisable()
    {
        ForceFinishTween(true);
    }
}