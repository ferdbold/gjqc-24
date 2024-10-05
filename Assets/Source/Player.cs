using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CharacterController2D _characterController;
    [SerializeField] private MeshRenderer _playerVisual;

    private float _velocity;
    private bool _shouldCrouch;
    private bool _shouldJump;

    public void SetPlayerMaterial(Material mat)
    {
        if (_playerVisual == null)
            return;

        _playerVisual.material = mat;
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
    }

    private void FixedUpdate()
    {
        _characterController.Move(_velocity, _shouldCrouch, _shouldJump);
        _shouldCrouch = false;
        _shouldJump = false;
    }

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
