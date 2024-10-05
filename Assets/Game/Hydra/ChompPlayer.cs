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
        head.OnCrunchHit += CB_OnCrunchHit;
        head.OnCrunchEnd += CB_OnCrunchEnd;
        head.Animator.Play("Chomp");

        return Status.Running;
    }

    private void CB_OnCrunchHit()
    {
        var head = Head.Value;

        foreach (var target in head.TargetsInRange)
            target.TakeDamage(Damage.Value);
    }

    private void CB_OnCrunchEnd()
        => _completed = true;

    protected override Status OnUpdate()
        => _completed ? Status.Success : Status.Running;

    protected override void OnEnd()
    {
        var head = Head.Value;
        head.OnCrunchHit -= CB_OnCrunchHit;
        head.OnCrunchEnd -= CB_OnCrunchEnd;
    }
}
