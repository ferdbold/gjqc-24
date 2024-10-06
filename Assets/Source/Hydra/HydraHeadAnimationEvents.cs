using UnityEngine;
using UnityEngine.Events;

public class HydraHeadAnimationEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent _onChompHit;
    [SerializeField] private UnityEvent _onChompEnd;
    [SerializeField] private UnityEvent _onDeathChop;
    [SerializeField] private UnityEvent _onDeathEnd;

    public void ANIM_ChompHit() => _onChompHit?.Invoke();
    public void ANIM_ChompEnd() => _onChompEnd?.Invoke();
    public void ANIM_DeathChop() => _onDeathChop?.Invoke();
    public void ANIM_DeathEnd() => _onDeathEnd?.Invoke();
}
