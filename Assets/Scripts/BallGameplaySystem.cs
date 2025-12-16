using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class BallGameplaySystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingleton<SpawnerConfig>(out var config)) return;

        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(World.Unmanaged);
        

        foreach (var (request, reqEntity) in SystemAPI.Query<RefRO<SpawnBallRequest>>().WithEntityAccess())
        {
            Entity newBall = EntityManager.Instantiate(config.BallEntityPrefab);
            
            if (EntityManager.HasComponent<Parent>(newBall))
            {
                EntityManager.RemoveComponent<Parent>(newBall);
            }

       
            EntityManager.SetComponentData(newBall, LocalTransform.FromPosition(request.ValueRO.Position));
            
            EntityManager.SetComponentData(newBall, new PhysicsVelocity
            {
                Linear = request.ValueRO.Velocity,
                Angular = float3.zero
            });
            
            ecb.DestroyEntity(reqEntity);
        }
 
    }
}