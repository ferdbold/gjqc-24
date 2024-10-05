using System;
using System.Collections.Generic;
using UnityEngine;

public class HydraHead : MonoBehaviour
{
    public Action OnCrunchHit;
    public Action OnCrunchEnd;

    [Header("Components")]
    [SerializeField] private Animator _animator;
    public Animator Animator => _animator;

    [SerializeField] private Collider2D _hitbox;
    public Collider2D Hitbox => _hitbox;

    private HydraHeadData _hydraHeadData;

    private readonly List<Player> _targetsInRange = new();
    public IReadOnlyList<Player> TargetsInRange => _targetsInRange;

    private void Awake()
    {
        _hydraHeadData = ScriptableObject.CreateInstance<HydraHeadData>();
        _targetsInRange.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
            _targetsInRange.Add(player);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
            _targetsInRange.Remove(player);
    }

    public void ANIM_Crunch()
        => OnCrunchHit?.Invoke();

    public void ANIM_CrunchEnd()
        => OnCrunchEnd?.Invoke();
}
