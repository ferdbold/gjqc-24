using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoiner : MonoBehaviour
{
    [SerializeField] private List<Material> _playerMaterials = new();
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private PlayerManager _playerManager; // Reference to PlayerManager

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += CB_OnPlayerJoined;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= CB_OnPlayerJoined;
    }

    private void CB_OnPlayerJoined(PlayerInput playerInput)
    {
        var player = playerInput.GetComponent<Player>();
        player.SetPlayerMaterial(_playerMaterials[playerInput.playerIndex]);

        // Register the player and their PlayerData in the PlayerManager
        _playerManager.RegisterPlayer(player, playerInput.playerIndex);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_playerInputManager == null)
            _playerInputManager = GetComponent<PlayerInputManager>();

        if (_playerManager == null)
            _playerManager = FindFirstObjectByType<PlayerManager>();
    }
#endif
}
