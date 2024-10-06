using System.Collections;
using UnityEngine;

public class HitMarkers : MonoBehaviour
{
    [SerializeField] private GameObject _hitMarkerVFXPrefab;

    private static HitMarkers _instance;

    public static void PlayHitMarker(Vector3 position)
    {
        if (_instance == null)
            _instance = FindFirstObjectByType<HitMarkers>();

        if (_instance._hitMarkerVFXPrefab == null)
            return;

        _instance.StartCoroutine(_instance.PlayHitMarker_Impl(position));
    }

    private IEnumerator PlayHitMarker_Impl(Vector3 position)
    {
        if (_hitMarkerVFXPrefab == null)
            yield break;

        var vfx = Instantiate(_hitMarkerVFXPrefab, position + new Vector3(0, 0, -2f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        Destroy(vfx);
    }
}
