using UnityEngine;
using Unity.Entities;


public struct SpawnerConfig : IComponentData
{
    public Entity BallEntityPrefab;
}


public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject ballPrefab; 

    class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new SpawnerConfig
            {
                BallEntityPrefab = GetEntity(authoring.ballPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}