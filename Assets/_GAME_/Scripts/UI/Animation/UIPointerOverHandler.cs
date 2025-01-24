using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class UIPointerOverHandler<T> : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public readonly UnityEvent OnPointerEnterEvent = new UnityEvent();
    public readonly UnityEvent OnPointerExitEvent = new UnityEvent();

    [SerializeField, Tooltip("Calls OnPointerEnter/Exit for each children with UIPointerOverHandler component")]
    protected bool _handleChildren;
    private bool _locked;

    public bool IsPointerOver { get; private set; }
    public bool Locked
    {
        get
        {
            return _locked;
        }
        set
        {
            _locked = value;
            if (_handleChildren)
            {
                UIPointerOverHandler<T>[] childrens = GetComponentsInChildren<UIPointerOverHandler<T>>();
                for (int i = 0; i < childrens.Length; i++)
                {
                    if (childrens[i] != this)
                    {
                        childrens[i].Locked = value;
                    }
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsPointerOver = true;
        if (!Locked)
        {
            OnPointerEnter();
            if (_handleChildren)
            {
                UIPointerOverHandler<T>[] childrens = GetComponentsInChildren<UIPointerOverHandler<T>>();
                for (int i = 0; i < childrens.Length; i++)
                {
                    if (childrens[i] != this)
                    {
                        childrens[i].OnPointerEnter(eventData);
                    }
                }
            }
        }
        OnPointerEnterEvent.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsPointerOver = false;
        if (!Locked)
        {
            OnPointerExit();
            if (_handleChildren)
            {
                UIPointerOverHandler<T>[] childrens = GetComponentsInChildren<UIPointerOverHandler<T>>();
                for (int i = 0; i < childrens.Length; i++)
                {
                    if (childrens[i] != this)
                    {
                        childrens[i].OnPointerExit(eventData);
                    }
                }
            }
        }
        OnPointerExitEvent.Invoke();
    }

    protected abstract void OnPointerEnter();

    protected abstract void OnPointerExit();

}