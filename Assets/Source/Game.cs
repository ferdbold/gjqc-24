using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    public static Action OnGameStarted;

    [SerializeField] private GameData _gameData;
    [SerializeField] private PlayerInputManager _inputManager;

    private readonly List<PlayerInput> _playerInputs = new();

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
        var player = playerInput.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player could not be found");
            return;
        }

        _gameData.Players.Add(player.PlayerData);
        _playerInputs.Add(playerInput);
        playerInput.actions.FindAction("Start").performed += CB_GameStartRequested;
    }

    private void CB_GameStartRequested(InputAction.CallbackContext ctx)
    {
        if (_gameData.Started)
            return;

        StartGame();
    }

    private void StartGame()
    {
        _inputManager.DisableJoining();
        _gameData.Started = true;

        foreach (var playerInput in _playerInputs)
            playerInput.actions.FindAction("Start").performed -= CB_GameStartRequested;
        _playerInputs.Clear();

        OnGameStarted?.Invoke();
    }
}
