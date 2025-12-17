using UnityEngine;
using Unity.Entities;

public class BrickAuthoring : MonoBehaviour
{
    public struct BrickTag : IComponentData { }

    public struct BrickHealth : IComponentData
    {
        public int Value;
    }
    
    public GameObject visualPrefab; 

    class Baker : Baker<BrickAuthoring>
    {
        public override void Bake(BrickAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new BrickTag());
            

            AddComponent(entity, new BrickHealth());
            
            if (authoring.visualPrefab != null)
            {
                AddComponentObject(entity, new VisualData
                {
                    Value = authoring.visualPrefab
                });
            }
        }
    }
}