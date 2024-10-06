using System;
using PrimeTween;
using Unity.Behavior;
using UnityEngine;
using Action = System.Action;
using Random = UnityEngine.Random;

public class HydraHead : MonoBehaviour, ITakesDamage
{
    public static Action<HydraHead> OnDeath;
    public Action OnChompHit;
    public Action OnChompEnd;

    [Header("Components")]
    [SerializeField] private Transform _head;
    [SerializeField] private Animator _animator;
    [SerializeField] private BehaviorGraphAgent _behaviorGraphAgent;
    [SerializeField] private TargetAcquirer _targetAcquirer;
    [SerializeField] private MeshFilter _neckMesh;

    [Header("Values")]
    [SerializeField] private float _hitRecoilStrength = 5f;
    [SerializeField] private float _hitRecoilDuration = 0.5f;

    public Animator Animator => _animator;
    private HydraHeadData _hydraHeadData;
    public HydraHeadData HydraHeadData => _hydraHeadData;

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
        else
        {
            HitRecoil();
        }
    }

    private void HitRecoil()
    {
        var randomAngle = Random.value * 360f;
        var normalizedVect = new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));
        var impulse = Mathf.Lerp(_hitRecoilStrength / 2, _hitRecoilStrength, Random.value) * normalizedVect;
        var dest = new Vector3(_head.position.x + impulse.x, _head.position.y, _head.position.z + impulse.y);
        Tween.Position(_head, dest, _hitRecoilDuration);
    }

    public void Die()
    {
        _behaviorGraphAgent.enabled = false;

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
