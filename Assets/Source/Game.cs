using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    public static Action OnGameStarted;

    [SerializeField] private GameData _gameData;
    [SerializeField] private PlayerInputManager _inputManager;

    private void OnEnable()
    {
        if (_inputManager != null)
            _inputManager.onPlayerJoined += CB_OnPlayerJoined;
    }

    private void OnDisable()
    {
        if (_inputManager != null)
            _inputManager.onPlayerJoined -= CB_OnPlayerJoined;
    }

    private void CB_OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.actions.FindAction("Start").performed += CB_GameStartRequested;
    }

    private void CB_GameStartRequested(InputAction.CallbackContext ctx)
    {
        if (_gameData.Started)
            return;

        // TODO: Disable joining

        _gameData.Started = true;
        OnGameStarted?.Invoke();
    }
}
