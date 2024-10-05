using UnityEngine;

[ExecuteAlways]
public class HydraHeadLookConstraint : MonoBehaviour
{
    [SerializeField] private float _deadzone = 2f;
    [SerializeField] private Transform _target;

    private void OnEnable()
    {
        if (_target == null)
        {
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        var value = Mathf.Clamp(transform.localPosition.x / _deadzone, -1f, 1f);
        _target.localRotation = Quaternion.Euler(Mathf.Lerp(-90f, 90f, value), 90, 90);
    }
}
