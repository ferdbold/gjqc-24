using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Find Closest With Tag, Non Stunned",
    description: "Finds the closest GameObject with the given tag.",
    story: "Find [Target] closest to [Agent] with tag: [Tag]",
    category: "Action/Find",
    id: "66251d9e7971a8112ab0517c0279d08f")]
internal partial class FindClosestWithTagNonStunnedAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<string> Tag;

    protected override Node.Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            LogFailure("No agent or target provided.");
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
