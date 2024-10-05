using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private MeshRenderer _playerVisual;

    public void SetPlayerMaterial(Material mat)
    {
        if (_playerVisual == null)
            return;

        _playerVisual.material = mat;
    }

    public void INPUT_Move(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<Vector2>();
        transform.position = new Vector3(transform.position.x + value.x, transform.position.y, transform.position.z);
    }

    public void INPUT_Jump(InputAction.CallbackContext _)
    {
        Debug.Log("Jump");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_playerInput == null)
            _playerInput = GetComponent<PlayerInput>();
    }
#endif
}
