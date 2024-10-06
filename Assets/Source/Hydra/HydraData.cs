using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jam/Hydra")]
public class HydraData : ScriptableObject
{
    public List<HydraHeadData> Heads = new();

    private void OnEnable()
    {
        Heads.Clear();
    }
}
