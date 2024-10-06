using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent _onHitCheck;
    [SerializeField] private UnityEvent _onAttackEnd;

    public void ANIM_HitCheck()
    {
        _onHitCheck?.Invoke();
    }

    public void ANIM_AttackEnd()
    {
        _onAttackEnd?.Invoke();
    }
}
