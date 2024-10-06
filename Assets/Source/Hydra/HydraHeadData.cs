using UnityEngine;

[CreateAssetMenu(menuName = "Jam/Hydra Head")]
public class HydraHeadData : ScriptableObject
{
    [Range(0, 100)] public float Health = 100f;

    private void OnEnable()
    {
        Health = 100f;
    }
}
