using System.Collections.Generic;
using UnityEngine;

public class TargetAcquirer : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private LayerMask _acquireMask;

    private readonly List<GameObject> _targets = new();
    public IReadOnlyList<GameObject> Targets => _targets;

    private void OnEnable()
    {
        _targets.Clear();
    }

    private void OnDisable()
    {
        _targets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_acquireMask == (_acquireMask | (1 << other.gameObject.layer)))
        {
            _targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _targets.Remove(other.gameObject);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_collider == null)
            _collider = GetComponent<Collider2D>();
    }
#endif
}
