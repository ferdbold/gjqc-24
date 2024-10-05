using System.Linq;
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CharacterController2D _characterController;
    [SerializeField] private Renderer _playerVisual;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _oneWayCollider;
    [SerializeField] private Collider2D _hitbox;

    [Header("Values")]
    [SerializeField] private int _stunInputsRequired = 8;

    [Header("AudioSources")]
    public AudioSource _sfxJump;
    public AudioSource _sfxHurt;
    public AudioSource _sfxAttack;

    private static readonly int APARAM_VELOCITY_X = Animator.StringToHash("VelocityX");
    private static readonly int APARAM_VELOCITY_Y = Animator.StringToHash("VelocityY");
    private static readonly int APARAM_ATTACK = Animator.StringToHash("Attack");
    private static readonly int APARAM_JUMP = Animator.StringToHash("Jump");
    private static readonly int APARAM_GROUNDED = Animator.StringToHash("Grounded");

    private PlayerData _playerData;
    public PlayerData PlayerData
    {
        get => _playerData;
        set => _playerData = value;
    }

    private bool _gameStarted = false;
    private float _velocity;
    private bool _shouldCrouch;
    private bool _shouldJump;
    private float _lastRecover = 0f;

    private void OnEnable()
    {
#if UNITY_EDITOR
        if (FindFirstObjectByType<Game>() == null)
            _gameStarted = true;
        else
#endif
        {
            Game.OnGameStarted += CB_OnGameStarted;
        }
    }

    private void OnDisable()
    {
        Game.OnGameStarted -= CB_OnGameStarted;
    }

    private void FixedUpdate()
    {
        _characterController.Move(_velocity, _shouldCrouch, _shouldJump);
        _shouldCrouch = false;
        _shouldJump = false;

        if (_animator)
        {
            _animator.SetFloat(APARAM_VELOCITY_X, Mathf.Abs(_velocity));
            _animator.SetFloat(APARAM_VELOCITY_Y, 0f); // TODO
            _animator.SetBool(APARAM_GROUNDED, _characterController.Grounded);
        }
    }

    public void SetPlayerMaterial(Material mat)
    {
        if (_playerVisual == null)
            return;

        _playerVisual.material = mat;
    }

    public void TakeDamage(float damage)
    {
        if (_playerData.Stunned)
            return;

        _playerData.Health -= damage;

        if (!_sfxHurt.isPlaying)
        {
            _sfxHurt.Play();
        }

        if (_playerData.Health <= 0)
        {
            Stun();
        }
    }

    public void Stun()
    {
        Debug.Log($"Player {_playerData.PlayerIndex} stunned");
        _playerData.Health = 0;
        _playerData.StunProgress = 0;
    }

    public void Recover()
    {
        Debug.Log($"Player {_playerData.PlayerIndex} recovered");
        _playerData.Health = 100f;
        _playerData.StunProgress = -1;
    }

    public void INPUT_Move(InputAction.CallbackContext ctx)
    {
        if (!_gameStarted)
            return;

        if (_playerData.Stunned)
            return;

        _velocity = ctx.ReadValue<float>();
    }

    public void INPUT_Jump(InputAction.CallbackContext _)
    {
        if (!_gameStarted)
            return;

        if (_playerData.Stunned)
            return;

        _shouldJump = true;

        if (_animator)
            _animator.SetTrigger(APARAM_JUMP);

        if (!_sfxJump.isPlaying)
        {
            _sfxJump.Play();
        }
    }

    public void INPUT_Attack(InputAction.CallbackContext ctx)
    {
        if (!_gameStarted)
            return;

        if (_playerData.Stunned)
            return;

        if (_animator)
            _animator.SetTrigger(APARAM_ATTACK);

        if (!_sfxAttack.isPlaying)
        {
            _sfxAttack.Play();
        }
    }

    public void INPUT_Dash(InputAction.CallbackContext ctx)
    {
        if (!_gameStarted)
            return;

        if (_playerData.Stunned)
            return;

        Debug.Log($"Player {_playerData.PlayerIndex} dashed");
    }

    public void INPUT_Crouch(InputAction.CallbackContext ctx)
    {
        if (!_gameStarted)
            return;

        if (_playerData.Stunned)
            return;

        _shouldCrouch = true;
        var value = ctx.ReadValue<float>() > 0f;
        _oneWayCollider.enabled = !value;
    }

    public void INPUT_Recover(InputAction.CallbackContext ctx)
    {
        if (!_gameStarted)
            return;

        if (!_playerData.Stunned)
            return;

        var value = ctx.ReadValue<float>();
        if (value is > -0.5f and < 0.5f)
            return;

        if (Mathf.Abs(value - _lastRecover) > 1f)
        {
            _playerData.StunProgress++;
            if (_playerData.StunProgress >= _stunInputsRequired)
            {
                Recover();
            }
        }
        _lastRecover = value;
    }

    private void CB_OnGameStarted()
        => _gameStarted = true;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    [Command("Stun")]
    private static void COMMAND_Stun(int index)
    {
        if (index < 0 || index >= Game.Instance.GameData.Players.Count)
            return;

        var data = Game.Instance.GameData.Players[index];
        var player = FindObjectsByType<Player>(FindObjectsSortMode.None).FirstOrDefault(pl => pl.PlayerData == data);
        player.Stun();
    }
#endif

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_playerInput == null)
            _playerInput = GetComponent<PlayerInput>();

        if (_characterController == null)
            _characterController = GetComponent<CharacterController2D>();
    }
#endif
}
