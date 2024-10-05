using System.Collections.Generic;
using System.Linq;
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
    public bool GameWon = false;

    private void OnEnable()
    {
        Started = false;
        Players.Clear();
      
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
    }
}
