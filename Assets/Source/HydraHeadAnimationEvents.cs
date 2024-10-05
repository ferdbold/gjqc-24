using UnityEngine;
using UnityEngine.Events;

public class HydraHeadAnimationEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent _onChompHit;
    [SerializeField] private UnityEvent _onChompEnd;

    public void ANIM_ChompHit()
    {
        _onChompHit?.Invoke();
    }

    public void ANIM_ChompEnd()
    {
        _onChompEnd?.Invoke();
    }
}
