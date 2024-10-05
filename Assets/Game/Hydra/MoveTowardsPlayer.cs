using System;
using PrimeTween;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveTowardsPlayer", story: "Move [_source] towards [_target] at [_speed] speed", category: "Action", id: "3346563065928da016456e1d29f9f164")]
public partial class MoveTowardsPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Source;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed;

    private bool _tweenComplete = false;

    protected override Status OnStart()
    {
        var source = Source.Value;
        var targetPos = Target.Value.transform.position;
        var distance = Vector2.Distance(new Vector2(source.transform.position.x, source.transform.position.z), new Vector2(targetPos.x, targetPos.z));
        var duration = Speed.Value * distance;

        Tween.Position(source.transform, targetPos, duration, Ease.InOutCirc).OnComplete(CB_OnTweenComplete);

        return Status.Running;
    }

    private void CB_OnTweenComplete()
    {
        _tweenComplete = true;
    }

    protected override Status OnUpdate()
    {
        return _tweenComplete ? Status.Success : Status.Running;
    }
}
