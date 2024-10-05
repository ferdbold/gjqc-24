using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CharacterController2D _characterController;
    [SerializeField] private MeshRenderer _playerVisual;
    [SerializeField] private Collider2D _oneWayCollider;

    private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;

    private bool _gameStarted = false;
    private float _velocity;
    private bool _shouldCrouch;
    private bool _shouldJump;

    private void OnEnable()
    {
        _playerData = ScriptableObject.CreateInstance<PlayerData>();

        Game.OnGameStarted += CB_OnGameStarted;
    }

    private void OnDisable()
    {
        Game.OnGameStarted -= CB_OnGameStarted;
    }

    public void SetPlayerMaterial(Material mat)
    {
        if (_playerVisual == null)
            return;

        _playerVisual.material = mat;
    }

    private void FixedUpdate()
    {
        if (!_gameStarted)
            return;

        _characterController.Move(_velocity, _shouldCrouch, _shouldJump);
        _shouldCrouch = false;
        _shouldJump = false;
    }

    public void INPUT_Move(InputAction.CallbackContext ctx)
    {
        _velocity = ctx.ReadValue<float>();
    }

    public void INPUT_Jump(InputAction.CallbackContext _)
    {
        _shouldJump = true;
    }

    public void INPUT_Attack(InputAction.CallbackContext ctx)
    {
        Debug.Log("Attack");
    }

    public void INPUT_Dash(InputAction.CallbackContext ctx)
    {
        Debug.Log("Dash");
    }

    public void INPUT_Crouch(InputAction.CallbackContext ctx)
    {
        _shouldCrouch = true;
        var value = ctx.ReadValue<float>() > 0f;
        _oneWayCollider.enabled = !value;
    }

    private void CB_OnGameStarted()
        => _gameStarted = true;

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
