using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private List<Player> players = new List<Player>(); // List of player components
    private Dictionary<int, PlayerData> playerDataDict = new Dictionary<int, PlayerData>(); // Map PlayerIndex to PlayerData
    public GameObject crownPrefab;  // Prefab for the crown

    private Transform crownInstance;

    void Start()
    {
        if (crownPrefab != null)
        {
            crownInstance = Instantiate(crownPrefab).transform;
            crownInstance.gameObject.SetActive(false); // Ensure crown is hidden initially
        }
    }

    void Update()
    {
        Player highestScoringPlayer = GetHighestScoringPlayer();

        if (highestScoringPlayer != null)
        {
            FollowPlayer(highestScoringPlayer.transform);
        }
        else if (crownInstance != null)
        {
            // No player has a score greater than 0, hide the crown
            crownInstance.gameObject.SetActive(false);
        }

        // Update rescue sprites for all players
        foreach (var player in players)
        {
            if (player.PlayerData.Stunned)
            {
                player.ShowRescueSprite();
            }
            else
            {
                player.HideRescueSprite();
            }
        }
    }

    // Method to register player in the manager when they join
    public void RegisterPlayer(Player player, int playerIndex)
    {
        players.Add(player);
        playerDataDict[playerIndex] = player.PlayerData; // Assuming Player has a PlayerData field
    }

    // Method to find the player with the highest score
    Player GetHighestScoringPlayer()
    {
        Player highestScorer = null;
        int highestScore = 0; // Start with 0

        foreach (var player in players)
        {
            if (player.PlayerData.Score > highestScore)
            {
                highestScore = player.PlayerData.Score;
                highestScorer = player;
            }
        }

        // Return null if no player has a score greater than 0
        return highestScore > 0 ? highestScorer : null;
    }

    // Method to make the crown follow the highest scoring player
    void FollowPlayer(Transform playerTransform)
    {
        if (crownInstance != null)
        {
            // Position the crown above the player's head
            crownInstance.gameObject.SetActive(true); // Ensure the crown is visible
            var crownPosition = playerTransform.position + Vector3.up * 2f + new Vector3(0f, 0f, -2f);
            crownInstance.position = crownPosition;
        }
    }
}
