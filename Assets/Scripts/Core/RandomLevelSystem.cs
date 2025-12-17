using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class RandomLevelSystem : SystemBase
{
    private Unity.Mathematics.Random _random;

    protected override void OnCreate()
    {
        _random = new Unity.Mathematics.Random(1234); 
    }

    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingletonEntity<RandomLevelConfig>(out Entity configEntity)) return;
        
        RandomLevelConfig config = SystemAPI.GetComponent<RandomLevelConfig>(configEntity);

        VisualData prototypeVisual = EntityManager.GetComponentData<VisualData>(config.BrickPrefab);
        
        for (int i = 0; i < config.Count; i++)
        {
  
            Entity newBrick = EntityManager.Instantiate(config.BrickPrefab);
            
            
            int randomHP = _random.NextInt(1, 4); 
            EntityManager.SetComponentData(newBrick, new BrickAuthoring.BrickHealth { Value = randomHP });
            
            float3 randomPos = _random.NextFloat3(config.BoundsMin, config.BoundsMax);
            
            float randomAngleY = _random.NextFloat(0, 360); 
            quaternion rotation = quaternion.Euler(90, randomAngleY, 0);

 
            if (EntityManager.HasComponent<Parent>(newBrick))
                EntityManager.RemoveComponent<Parent>(newBrick);

            EntityManager.SetComponentData(newBrick, LocalTransform.FromPositionRotation(randomPos, rotation));


            if (prototypeVisual != null)
            {
                EntityManager.AddComponentObject(newBrick, new VisualData 
                { 
                    Value = prototypeVisual.Value 
                });
            }
        }
        EntityManager.DestroyEntity(configEntity);
    }
}