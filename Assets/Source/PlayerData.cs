using Unity.Properties;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Jam/Player")]
public class PlayerData : ScriptableObject
{
    public int PlayerIndex = -1;
    [Range(0, 100)] public float Health;
    public int Score;
    public int StunProgress = -1;

    private void OnEnable()
    {
        Health = 100f;
        Score = 0;
        StunProgress = -1;
    }

    [CreateProperty] public bool Stunned => StunProgress >= 0;
}
