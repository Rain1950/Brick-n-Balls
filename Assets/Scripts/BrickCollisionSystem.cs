using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial class BrickCollisionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Dependency.Complete();
        var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        var ecbSingleton = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(World.Unmanaged);
        
        var healthLookup = SystemAPI.GetComponentLookup<BrickAuthoring.BrickHealth>(false);

        foreach (var collisionEvent in simulation.AsSimulation().CollisionEvents)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;
            
            bool isABrick = SystemAPI.HasComponent<BrickAuthoring.BrickTag>(entityA);
            bool isBBrick = SystemAPI.HasComponent<BrickAuthoring.BrickTag>(entityB);
            bool isABall = SystemAPI.HasComponent<BallTag>(entityA);
            bool isBBall = SystemAPI.HasComponent<BallTag>(entityB);
            
            if (isABrick && isBBall)
            {
                ProcessCollision(entityA, ecb, ref healthLookup);
            }
            else if (isBBrick && isABall)
            {
                ProcessCollision(entityB, ecb, ref healthLookup);
            }
        }
    }

    private void ProcessCollision(Entity brickEntity, EntityCommandBuffer ecb, ref ComponentLookup<BrickAuthoring.BrickHealth> healthLookup)
    {
        ScoreManager.TriggerOnBrickCollided();

   
        if (healthLookup.HasComponent(brickEntity))
        {
            var health = healthLookup[brickEntity];
            health.Value--; 
            
            healthLookup[brickEntity] = health;
            
            
            if (health.Value <= 0)
            {
                DestroyBrick(brickEntity, ecb);
            }
        }
    }

    private void DestroyBrick(Entity brickEntity, EntityCommandBuffer ecb)
    {
        if (EntityManager.HasComponent<TransformReference>(brickEntity))
        {
            var transformRef = EntityManager.GetComponentObject<TransformReference>(brickEntity);
            if (transformRef.transform != null)
                Object.Destroy(transformRef.transform.gameObject);
        }
        
        ecb.DestroyEntity(brickEntity);
    }
}