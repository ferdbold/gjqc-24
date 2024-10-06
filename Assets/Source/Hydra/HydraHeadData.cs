using Unity.Properties;
using UnityEngine;

[CreateAssetMenu(menuName = "Jam/Hydra Head")]
public class HydraHeadData : ScriptableObject
{
    [Range(0, 200)] public float Health = 250f;

    [CreateProperty] public bool Dead => Health <= 0;

    private void OnEnable() => Reset();
    private void OnDisable() => Reset();

    private void Reset()
    {
        Health = 250f;
    }
}
