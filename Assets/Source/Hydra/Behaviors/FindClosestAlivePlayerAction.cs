using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindClosestAlivePlayer", story: "Find alive [_target] closest to [_agent] with [_tag]", category: "Action", id: "88763221f67ba1796933b584e9c74810")]
public partial class FindClosestAlivePlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<string> Tag;

    protected override Node.Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent provided.");
            return Status.Failure;
        }

        Vector3 agentPosition = Agent.Value.transform.position;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(Tag.Value);
        float closestDistanceSq = Mathf.Infinity;
        GameObject closestGameObject = null;
        foreach (GameObject gameObject in gameObjects)
        {
            var player = gameObject.GetComponent<Player>();
            if (player.PlayerData.Stunned)
                continue;

            float distanceSq = Vector3.SqrMagnitude(agentPosition - gameObject.transform.position);
            if (closestGameObject == null || distanceSq < closestDistanceSq)
            {
                closestDistanceSq = distanceSq;
                closestGameObject = gameObject;
            }
        }

        Target.Value = closestGameObject;
        return Target.Value == null ? Status.Failure : Status.Success;
    }
}

