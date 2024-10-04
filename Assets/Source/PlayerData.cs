using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Jam/Player")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private string _playerName;
}
