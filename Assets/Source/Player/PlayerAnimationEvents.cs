using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent _onHitCheck;

    public void ANIM_HitCheck()
    {
        _onHitCheck?.Invoke();
    }
}
