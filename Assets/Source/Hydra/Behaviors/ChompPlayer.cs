using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChompPlayer", story: "Chomp with [_head] for [_damage] damage", category: "Action", id: "b478a95716611a184e81139f568683f6")]
public partial class AttackPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<HydraHead> Head;
    [SerializeReference] public BlackboardVariable<float> Damage;

    private bool _completed;

    protected override Status OnStart()
    {
        var head = Head.Value;
        head.OnChompHit += CB_OnChompHit;
        head.OnChompEnd += CB_OnChompEnd;

        head.PlayChompAnimation();

        return Status.Running;
    }

    private void CB_OnChompHit()
    {
        var head = Head.Value;

        foreach (var target in head.TargetAcquirer.Targets)
        {
            Debug.Log($"{Head.Value.name} hit target {target.name}");

            var takesDamage = target.GetComponent<ITakesDamage>();
            if (takesDamage != null)
                takesDamage.TakeDamage(Damage.Value, out _);
        }
    }

    private void CB_OnChompEnd()
        => _completed = true;

    protected override Status OnUpdate()
        => _completed ? Status.Success : Status.Running;

    protected override void OnEnd()
    {
        var head = Head.Value;
        head.OnChompHit -= CB_OnChompHit;
        head.OnChompEnd -= CB_OnChompEnd;

        _completed = false;
    }
}
