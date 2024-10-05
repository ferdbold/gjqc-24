using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CharacterController2D _characterController;
    [SerializeField] private MeshRenderer _playerVisual;

    private float _velocity;
    private bool _shouldJump;

    public void SetPlayerMaterial(Material mat)
    {
        if (_playerVisual == null)
            return;

        _playerVisual.material = mat;
    }

    public void INPUT_Move(InputAction.CallbackContext ctx)
    {
        _velocity = ctx.ReadValue<Vector2>().x;
    }

    public void INPUT_Jump(InputAction.CallbackContext _)
    {
        _shouldJump = true;
    }

    private void FixedUpdate()
    {
        _characterController.Move(_velocity, false, _shouldJump);
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
