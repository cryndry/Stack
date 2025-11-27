using System;
using UnityEngine;

class EventManager : LazySingleton<EventManager>
{
    public event Action<Collider2D> OnTapOnUI;
    public void InvokeTapOnUI(Collider2D collider)
    {
        OnTapOnUI?.Invoke(collider);
    }

    public event Action OnTapTouchLayer;
    public void InvokeTapTouchLayer()
    {
        OnTapTouchLayer?.Invoke();
    }
}