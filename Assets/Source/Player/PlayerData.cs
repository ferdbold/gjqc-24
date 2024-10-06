using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Player", menuName = "Jam/Player")]
public class PlayerData : ScriptableObject
{
    public int PlayerIndex = -1;
    [Range(0, 100)] public float Health;
    public int Score;
    public int StunProgress = -1;
    public Color PlayerColor;
    public bool Attacking = false;

    private void OnEnable()
    {
        PlayerIndex = -1;
        Health = 100f;
        Score = 0;
        StunProgress = -1;
        Attacking = false;
    }

    [CreateProperty] public int PlayerNumber => PlayerIndex + 1;
    [CreateProperty] public bool Stunned => StunProgress >= 0;

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    public static void RegisterConverters()
    {
        var floatToStyleLength = new ConverterGroup("Float to StyleLength Percent");
        floatToStyleLength.AddConverter((ref float val) => new StyleLength(new Length(val, LengthUnit.Percent)));
        ConverterGroups.RegisterConverterGroup(floatToStyleLength);
    }
}
