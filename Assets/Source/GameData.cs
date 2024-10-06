using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Jam/Game")]
public class GameData : ScriptableObject
{
    public bool Started = false;
    public List<PlayerData> Players = new();
    public PlayerData WinningPlayer;
    public bool GameLost = false;
    public float TimeLeft = 30f;

    [CreateProperty] public bool GameWon => TimeLeft <= 0f;
    [CreateProperty] public int TimeLeftInt => (int)TimeLeft;

    [CreateProperty] public bool IsP1Present => Players.Count >= 1;
    [CreateProperty] public bool IsP2Present => Players.Count >= 2;
    [CreateProperty] public bool IsP3Present => Players.Count >= 3;
    [CreateProperty] public bool IsP4Present => Players.Count >= 4;

    private void OnEnable() => Reset();
    private void OnDisable() => Reset();

    public void Reset()
    {
        Started = false;
        Players.Clear();
        WinningPlayer = null;
        TimeLeft = 30f;
    }

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    public static void RegisterConverters()
    {
        var boolToDisplayStyle = new ConverterGroup("Bool to Display Style");
        boolToDisplayStyle.AddConverter((ref bool val) => new StyleEnum<DisplayStyle>(val ? DisplayStyle.Flex : DisplayStyle.None));
        ConverterGroups.RegisterConverterGroup(boolToDisplayStyle);

        var boolToInverseDisplayStyle = new ConverterGroup("Bool to Inverse Display Style");
        boolToInverseDisplayStyle.AddConverter((ref bool val) => new StyleEnum<DisplayStyle>(val ? DisplayStyle.None : DisplayStyle.Flex));
        ConverterGroups.RegisterConverterGroup(boolToInverseDisplayStyle);

        var anyPlayers = new ConverterGroup("Any Players to Display Style");
        anyPlayers.AddConverter((ref List<PlayerData> val) => new StyleEnum<DisplayStyle>(val.Any() ? DisplayStyle.Flex : DisplayStyle.None));
        ConverterGroups.RegisterConverterGroup(anyPlayers);

        var boolToScale = new ConverterGroup("Bool to Scale");
        boolToScale.AddConverter((ref bool val) => new StyleScale(val ? Vector2.one : Vector2.zero));
        ConverterGroups.RegisterConverterGroup(boolToScale);
    }
}
