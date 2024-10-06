using UnityEngine;
using UnityEngine.Events;

public class HydraAnimationEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent _onHandStomp;

    public void ANIM_HandStomp() => _onHandStomp?.Invoke();
}
