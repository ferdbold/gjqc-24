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

        if (_player.PlayerData.Stunned)
            return;

        Debug.Log($"Player {_player.PlayerData.PlayerIndex} attacked");

        if (_animator)
            _animator.SetTrigger(APARAM_ATTACK);

        if (!_sfxAttack.isPlaying)
        {
            _sfxAttack.Play();
        }
    }

    private void CB_HitCheck()
    {
        if (_targetAcquirer.Targets.Count > 0)
        {
            Debug.Log($"Hit from {_player.PlayerData.PlayerIndex} connected");

            if (_sfxHit)
                _sfxHit.Play();

            // TODO: Play SFX
        }

        foreach (var target in _targetAcquirer.Targets)
        {
            var takesDamage = target.GetComponent<ITakesDamage>();
            if (takesDamage != null)
                takesDamage.TakeDamage(_attackDamage);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_player == null)
            _player = GetComponent<Player>();
    }
#endif
}
