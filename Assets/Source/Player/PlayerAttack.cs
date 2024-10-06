using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Player _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _hitCollider;
    [SerializeField] private TargetAcquirer _targetAcquirer;
    [SerializeField] private AudioSource _sfxAttack;
    [SerializeField] private AudioSource _sfxHit;

    [Header("Values")]
    [SerializeField] private float _attackDamage = 25f;

    private static readonly int APARAM_ATTACK = Animator.StringToHash("Attack");

    public void INPUT_Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.phase != InputActionPhase.Performed)
            return;

        if (!_player.GameStarted)
            return;

        if (_player.PlayerData.Stunned || _player.PlayerData.Attacking)
            return;

        _player.PlayerData.Attacking = true;

        if (_animator)
            _animator.SetTrigger(APARAM_ATTACK);

        if (!_sfxAttack.isPlaying)
        {
            _sfxAttack.Play();
        }
    }

    public void ANIM_HitCheck()
    {
        if (_targetAcquirer.Targets.Count > 0)
        {
            Debug.Log($"Hit from {_player.PlayerData.PlayerIndex} connected");

            if (_sfxHit)
                _sfxHit.Play();

            // TODO: Play SFX
        }

        for (var i = _targetAcquirer.Targets.Count - 1; i >= 0; i--)
        {
            var target = _targetAcquirer.Targets[i];
            Debug.Log($"{gameObject.name} hit {target}");

            var takesDamage = target.GetComponent<ITakesDamage>();
            if (takesDamage != null)
                takesDamage.TakeDamage(_attackDamage);
        }
    }

    public void ANIM_AttackEnd()
    {
        _player.PlayerData.Attacking = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_player == null)
            _player = GetComponent<Player>();
    }
#endif
}
