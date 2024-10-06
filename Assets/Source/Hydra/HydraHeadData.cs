using UnityEngine;

[CreateAssetMenu(menuName = "Jam/Hydra Head")]
public class HydraHeadData : ScriptableObject
{
    [Range(0, 200)] public float Health = 200f;

    private void OnEnable()
    {
        Health = 200f;
    }
}
