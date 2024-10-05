using System;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : MonoBehaviour
{
    public static Action OnHydraDefeated;

    [Header("Components")]
    [SerializeField] private Transform _neckAnchor;

    [Header("References")]
    [SerializeField] private HydraHead _headPrefab;

    private List<HydraHead> _heads = new();

    public void OnEnable()
    {
        _heads.Clear();
    }

    public void InstantiateHydraHead()
    {
        var newHead = Instantiate(_headPrefab, _neckAnchor);

    }
}
