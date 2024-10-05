using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = System.Action;

public class HydraHead : MonoBehaviour
{
    public static Action<HydraHead> OnDeath;
    public Action OnChompHit;
    public Action OnChompEnd;

    [Header("Components")]
    [SerializeField] private Animator _animator;
    [SerializeField] private BehaviorGraphAgent _behaviorGraphAgent;
    [SerializeField] private Collider2D _hitbox;
    [SerializeField] private MeshFilter _neckMesh;

    public Animator Animator => _animator;
    public Collider2D Hitbox => _hitbox;
    private HydraHeadData _hydraHeadData;

    private readonly List<Player> _targetsInRange = new();
    public IReadOnlyList<Player> TargetsInRange => _targetsInRange;

    private void Awake()
    {
        _hydraHeadData = ScriptableObject.CreateInstance<HydraHeadData>();
        _targetsInRange.Clear();
    }

    private void OnEnable()
    {
        if (Game.Instance != null && Game.Instance.GameData.Started)
        {
            if (_behaviorGraphAgent)
                _behaviorGraphAgent.Start();
        }
        else
        {
            Game.OnGameStarted += CB_OnGameStarted;
        }

        if (_neckMesh)
        {
            var mesh = Instantiate(_neckMesh.sharedMesh);
            _neckMesh.mesh = mesh;
        }
    }

    private void CB_OnGameStarted()
    {
        Game.OnGameStarted -= CB_OnGameStarted;

        if (_behaviorGraphAgent)
            _behaviorGraphAgent.Start();
    }

    private void OnDisable()
    {
        if (_behaviorGraphAgent)
            _behaviorGraphAgent.End();
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

    public void TakeDamage(int damage)
    {
        _hydraHeadData.Health -= damage;

        if (_hydraHeadData.Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject); // TODO: Plug animation and SFX
    }

    public void ANIM_ChompHit()
        => OnChompHit?.Invoke();

    public void ANIM_ChompEnd()
        => OnChompEnd?.Invoke();

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_behaviorGraphAgent == null)
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }
#endif
}
