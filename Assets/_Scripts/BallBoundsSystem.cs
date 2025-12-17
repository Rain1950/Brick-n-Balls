using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;

public partial class BallBoundsSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(World.Unmanaged);

        float bottomLimit = -30.0f;

        foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>()
                     .WithAll<BallTag>()
                     .WithEntityAccess())
        {
            if (transform.ValueRO.Position.y < bottomLimit)
            {
                ScoreManager.TriggerBallDied();

                if (EntityManager.HasComponent<TransformReference>(entity))
                {
                    var transformRef = EntityManager.GetComponentObject<TransformReference>(entity);
                    if (transformRef != null && transformRef.transform != null)
                    {
                        UnityEngine.Object.Destroy(transformRef.transform.gameObject);
                    }
                }

                ecb.DestroyEntity(entity);
            }
        }
    }
}