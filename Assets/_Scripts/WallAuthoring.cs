using UnityEngine;
using Unity.Entities;

public class WallAuthoring : MonoBehaviour
{
    public GameObject visualPrefab; 
    
    
    public struct WallTag : IComponentData { }
    class Baker : Baker<WallAuthoring>
    {
        public override void Bake(WallAuthoring authoring)
        {
            
            Entity entity = GetEntity(TransformUsageFlags.Dynamic); 
            
            if (authoring.visualPrefab != null)
            {
                AddComponentObject(entity, new VisualData
                {
                    Value = authoring.visualPrefab
                });
            }
            
             AddComponent(entity, new WallTag());
        }
    }
}