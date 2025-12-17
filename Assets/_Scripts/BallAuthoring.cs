using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;




public struct SpawnBallRequest : IComponentData
{
    public float3 Position;
    public float3 Velocity;
}
public struct BallTag : IComponentData { }

public class BallAuthoring : MonoBehaviour
{
    public GameObject visualPrefab; 

    public class BallBaker : Baker<BallAuthoring>
    {
        public override void Bake(BallAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponentObject(entity, new VisualData { Value = authoring.visualPrefab });
            
            AddComponent<BallTag>(entity);
        }
    }
}