using DG.Tweening;
using UnityEngine;

public class UIPointerHoverRotation : UIPointerOverHandler<UIPointerHoverRotation>
{
    [SerializeField]
    private Transform _target;
    [SerializeField, Header("Animation")]
    private Vector3 _mouseOverRotation;
    [SerializeField]
    private Vector3 _mouseOutRotation = Vector3.zero;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private bool _forceKillOnHover;
    [SerializeField]
    private bool _forceKillOnExit;
    [SerializeField]
    private Ease _ease;

    private void Start()
    {
        if (_target == null)
        {
            _target = transform;
        }
    }

    protected override void OnPointerEnter()
    {
        _target.DOKill();
        if (_forceKillOnHover)
            ForceFinishTween(false);
        _target.DORotate(_mouseOverRotation, _duration).SetEase(_ease);
    }

    protected override void OnPointerExit()
    {
        _target.DOKill();
        if (_forceKillOnExit)
            ForceFinishTween(true);
        _target.DORotate(_mouseOutRotation, _duration).SetEase(_ease);
    }

    private void ForceFinishTween(bool exit)
    {
        _target.DOKill();
        Vector3 rotation = exit ? _mouseOutRotation : _mouseOverRotation;
        _target.rotation = Quaternion.Euler(rotation);
    }

    private void OnDisable()
    {
        ForceFinishTween(true);
    }
}