using System;
using Unity.Behavior;
using UnityEngine;
using Action = System.Action;

public class HydraHead : MonoBehaviour, ITakesDamage
{
    public static Action<HydraHead> OnDeath;
    public Action OnChompHit;
    public Action OnChompEnd;

    [Header("Components")]
    [SerializeField] private Animator _animator;
    [SerializeField] private BehaviorGraphAgent _behaviorGraphAgent;
    [SerializeField] private TargetAcquirer _targetAcquirer;
    [SerializeField] private MeshFilter _neckMesh;

    public Animator Animator => _animator;
    private HydraHeadData _hydraHeadData;

    public TargetAcquirer TargetAcquirer => _targetAcquirer;

    private void Awake()
    {
        _hydraHeadData = ScriptableObject.CreateInstance<HydraHeadData>();
    }

    private void OnEnable()
    {
        if (Game.Instance != null && Game.Instance.GameData.Started)
        {
            _behaviorGraphAgent.enabled = true;
        }
        else
        {
            _behaviorGraphAgent.enabled = false;
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
        _behaviorGraphAgent.enabled = true;
    }

    private void OnDisable()
    {
        if (_behaviorGraphAgent)
            _behaviorGraphAgent.End();
    }

    public void TakeDamage(float damage)
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
