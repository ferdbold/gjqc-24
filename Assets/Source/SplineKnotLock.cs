using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteAlways]
public class SplintKnotLock : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private Transform _lock;
    [SerializeField] private int _knotIndex = -1;

    private void OnEnable()
    {
        if (_splineContainer == null || _lock == null)
        {
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (_knotIndex < 0 || _knotIndex >= _splineContainer.Spline.Knots.Count())
            return;

        _splineContainer.Spline.SetKnot(_knotIndex, new BezierKnot(new float3(_lock.localPosition.x, -1f, _lock.localPosition.z)));
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_splineContainer == null)
            _splineContainer = GetComponent<SplineContainer>();
    }
#endif
}
