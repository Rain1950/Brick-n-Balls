using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial class BrickCollisionSystem : SystemBase
{
    private Dictionary<Entity, double> _lastHitTimes = new Dictionary<Entity, double>(); //Store lastHitTimes to prevent multiple collisionEvents in one frame
    protected override void OnUpdate()
    {
        Dependency.Complete();
        var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        var ecbSingleton = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(World.Unmanaged);
        
        var healthLookup = SystemAPI.GetComponentLookup<BrickAuthoring.BrickHealth>(false);
        double currentTime = SystemAPI.Time.ElapsedTime;
        double cooldown = 0.1f;
        if (_lastHitTimes.Count > 1000) _lastHitTimes.Clear();
        
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
                    ProcessCollision(entityB, ecb, ref healthLookup,currentTime, cooldown);
            }
            else if (isBBrick && isABall)
            {
                ProcessCollision(entityB, ecb, ref healthLookup,currentTime, cooldown);
            }
        }
    }

    private void ProcessCollision(Entity brickEntity, EntityCommandBuffer ecb, ref ComponentLookup<BrickAuthoring.BrickHealth> healthLookup, double time, double cooldown)
    {
        if (_lastHitTimes.TryGetValue(brickEntity, out double lastTime))
        {
            if (time - lastTime < cooldown) return; 
        }

   
        if (healthLookup.HasComponent(brickEntity))
        {
            _lastHitTimes[brickEntity] = time;
            var health = healthLookup[brickEntity]; 
            ScoreManager.TriggerOnBrickCollided();
            health.Value--; 
            
            healthLookup[brickEntity] = health;
            
            
            if (health.Value <= 0)
            {
                _lastHitTimes.Remove(brickEntity);
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