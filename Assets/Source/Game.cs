using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    public static Action OnGameStarted;

    [Header("References")]
    [SerializeField] private GameData _gameData;
    [SerializeField] private List<PlayerData> _playerDataAssets = new();
    [SerializeField] private PlayerInputManager _inputManager;

    [Header("Values")]
    public int ScorePerHit = 1;
    public int ScorePerKill = 10;
    public int ScorePerGoldenKill = 25;
    public int ScorePerRevive = 10;

    private readonly List<PlayerInput> _playerInputs = new();

    public static Game Instance { get; private set; }
    public GameData GameData => _gameData;

    private void OnEnable()
    {
        Instance = this;

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

        var playerIndex = _gameData.Players.Count;
        player.PlayerData = _playerDataAssets[playerIndex];
        player.PlayerData.PlayerIndex = playerIndex;

        _gameData.Players.Add(player.PlayerData);
        _playerInputs.Add(playerInput);

        player.gameObject.name = $"Player {player.PlayerData.PlayerIndex}";

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
