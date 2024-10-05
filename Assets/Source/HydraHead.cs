using UnityEngine;

public class HydraHead : MonoBehaviour
{
    private HydraHeadData _hydraHeadData;

    private void Awake()
    {
        _hydraHeadData = ScriptableObject.CreateInstance<HydraHeadData>();
    }
}
