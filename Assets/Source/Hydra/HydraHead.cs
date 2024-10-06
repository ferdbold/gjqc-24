using System;
using PrimeTween;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Splines;
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
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private SplineAnimate _splineHeadDeathAnimation;
    [SerializeField] private BehaviorGraphAgent _behaviorGraphAgent;
    [SerializeField] private TargetAcquirer _targetAcquirer;
    [SerializeField] private MeshFilter _neckMesh;

    [Header("Audio")]
    [SerializeField] private AudioSource _sfxHiss;
    [SerializeField] private AudioSource _sfxChomp;
    [SerializeField] private AudioSource _sfxHurt;
    [SerializeField] private AudioSource _sfxDeath;

    [Header("Values")]
    [SerializeField] private float _hitRecoilStrength = 5f;
    [SerializeField] private float _hitRecoilDuration = 0.5f;

    private static readonly int APARAM_CHOMP = Animator.StringToHash("Chomp");
    private static readonly int APARAM_FIRE = Animator.StringToHash("Fire");
    private static readonly int APARAM_HURT = Animator.StringToHash("Hurt");
    private static readonly int APARAM_DEAD = Animator.StringToHash("Dead");

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

    public bool TakeDamage(float damage, out bool killConfirmed)
    {
        killConfirmed = false;

        if (_hydraHeadData.Dead)
            return false;

        _hydraHeadData.Health -= damage;

        if (_hydraHeadData.Health <= 0)
        {
            Die();
            killConfirmed = true;
        }
        else
        {
            HitRecoil();
        }

        return true;
    }

    public void PlayHissSFX()
    {
        if (_sfxHiss != null)
            _sfxHiss.Play();
    }

    private void HitRecoil()
    {
        _animator.SetTrigger(APARAM_HURT);
        if (_sfxHurt != null)
            _sfxHurt.Play();

        var randomAngle = Random.value * 360f;
        var normalizedVect = new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));
        var impulse = Mathf.Lerp(_hitRecoilStrength / 2, _hitRecoilStrength, Random.value) * normalizedVect;
        var dest = new Vector3(_head.position.x + impulse.x, _head.position.y, _head.position.z + impulse.y);
        Tween.Position(_head, dest, _hitRecoilDuration);
    }

    public void Die()
    {
        _behaviorGraphAgent.enabled = false;

        _animator.SetBool(APARAM_DEAD, true);
        if (_sfxDeath != null)
            _sfxDeath.Play();
    }

    public void PlayChompAnimation()
        => _animator.SetTrigger(APARAM_CHOMP);

    public void ANIM_ChompHit()
    {
        if (_sfxChomp != null)
            _sfxChomp.Play();

        OnChompHit?.Invoke();
    }

    public void ANIM_ChompEnd()
        => OnChompEnd?.Invoke();

    public void ANIM_DeathChop()
    {
        // TODO: Animate head backwards along spline
    }

    public void ANIM_DeathEnd()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_behaviorGraphAgent == null)
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }
#endif
}
