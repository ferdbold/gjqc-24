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
    [SerializeReference] public BlackboardVariable<Transform> Source;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed;

    private Tween _tween;
    private bool _completed = false;

    protected override Status OnStart()
    {
        var source = Source.Value;
        var targetPos = Target.Value.transform.position;
        //var distance = Vector2.Distance(new Vector2(source.position.x, source.position.z), new Vector2(targetPos.x, targetPos.z));
        //var duration = Speed.Value / distance;

        _tween = Tween.Position(source, targetPos, 1f, Ease.OutCubic).OnComplete(CB_OnTweenComplete);

        return Status.Running;
    }

    private void CB_OnTweenComplete()
        => _completed = true;

    protected override Status OnUpdate()
    {
        if (Source.Value == null)
            return Status.Failure;

        return _completed ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
        _tween.Complete();
        _completed = false;
    }
}
