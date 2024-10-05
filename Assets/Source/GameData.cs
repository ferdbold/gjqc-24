using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Jam/Game")]
public class GameData : ScriptableObject
{
    public bool Started = false;
    public List<PlayerData> Players = new();

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
        var boolToDisplayStyle = new ConverterGroup("Bool to Inverse Display Style");
        boolToDisplayStyle.AddConverter((ref bool val) => new StyleEnum<DisplayStyle>(val ? DisplayStyle.None : DisplayStyle.Flex));
        ConverterGroups.RegisterConverterGroup(boolToDisplayStyle);
    }
}
