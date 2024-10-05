using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Jam/Player")]
public class PlayerData : ScriptableObject
{
    [Range(0, 100)] public float Health;
    public int Score;

    private void OnEnable()
    {
        Health = 100f;
        Score = 0;
    }
}
