using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;


public struct RandomLevelConfig : IComponentData
{
    public Entity BrickPrefab;  
    public int Count;          
    public float3 BoundsMin;    
    public float3 BoundsMax;    
}

public class BricksGenerator : MonoBehaviour
{
 
    public BoxCollider bricksSpawnBounds;
    public GameObject brickPrefab; 
    public int numOfBricks = 50;

    
    class Baker : Baker<BricksGenerator>
    {
        public override void Bake(BricksGenerator authoring)
        {
            if (authoring.brickPrefab == null || authoring.bricksSpawnBounds == null) return;
            
            Entity entity = GetEntity(TransformUsageFlags.None);
            
            Bounds bounds = authoring.bricksSpawnBounds.bounds;
            
            AddComponent(entity, new RandomLevelConfig
            {
                BrickPrefab = GetEntity(authoring.brickPrefab, TransformUsageFlags.Dynamic),
                Count = authoring.numOfBricks,
                
                BoundsMin = (float3)bounds.min,
                BoundsMax = (float3)bounds.max
            });
        }
    }
    
    private void OnDrawGizmos()
    {
        if (bricksSpawnBounds != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f); 
            Gizmos.DrawCube(bricksSpawnBounds.bounds.center, bricksSpawnBounds.bounds.size);
        
            Gizmos.color = Color.green; 
            Gizmos.DrawWireCube(bricksSpawnBounds.bounds.center, bricksSpawnBounds.bounds.size);
        }
    }
}