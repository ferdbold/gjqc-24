using System.Collections.Generic;
using UnityEngine;

public class TargetAcquirer : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private LayerMask _acquireMask;
    [SerializeField] private List<GameObject> _exclude = new();

    private readonly List<GameObject> _targets = new();

    public IReadOnlyList<GameObject> Targets
    {
        get
        {
            for (var i = _targets.Count - 1; i >= 0; i--)
            {
                if (_targets[i] == null)
                    _targets.RemoveAt(i);
            }

            return _targets;
        }
    }

    private void OnEnable()
    {
        _targets.Clear();

        HydraHead.OnDeath += CB_OnHeadDeath;
    }

    private void OnDisable()
    {
        _targets.Clear();

        HydraHead.OnDeath -= CB_OnHeadDeath;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_exclude.Contains(other.gameObject))
            return;

        if (_acquireMask == (_acquireMask | (1 << other.gameObject.layer)))
        {
            if (other.TryGetComponent(out GameObjectProxy gameObjectProxy))
            {
                Debug.Log($"{name} acquired {gameObjectProxy.Target.name}");
                _targets.Add(gameObjectProxy.Target);
            }
            else
            {
                Debug.Log($"{name} acquired {other.gameObject.name}");
                _targets.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out GameObjectProxy gameObjectProxy))
        {
            if (_targets.Remove(other.gameObject))
                Debug.Log($"{name} lost {gameObjectProxy.Target.name}");
        }
        else
        {
            if (_targets.Remove(other.gameObject))
                Debug.Log($"{name} lost {other.gameObject.name}");
        }
    }

    private void CB_OnHeadDeath(HydraHead head)
    {
        _targets.Remove(head.gameObject);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_collider == null)
            _collider = GetComponent<Collider2D>();
    }
#endif
}
