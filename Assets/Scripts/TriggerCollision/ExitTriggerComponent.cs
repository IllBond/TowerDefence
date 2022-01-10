using System;
using UnityEngine;
using UnityEngine.Events;

public class ExitTriggerComponent : MonoBehaviour
{
    [SerializeField] LayerMask _layer = ~0;
    [SerializeField] EnterEvent _action;

    public void OnTriggerExit(Collider collider)
    {
        if (!collider.gameObject.IsInLayer(_layer)) return;
        _action?.Invoke(collider.gameObject);
    }

}

[Serializable]
public class ExiteEvent : UnityEvent<GameObject>
{

}

