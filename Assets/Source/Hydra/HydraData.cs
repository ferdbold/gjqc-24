using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jam/Hydra")]
public class HydraData : ScriptableObject
{
    public List<HydraHeadData> Heads = new();
    public int HeadsDefeated = 0;

    private void OnEnable()
    {
        Heads.Clear();
        HeadsDefeated = 0;
    }
}
