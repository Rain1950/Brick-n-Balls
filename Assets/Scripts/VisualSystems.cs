using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[UpdateInGroup(typeof(TransformSystemGroup))]
public partial class VisualSyncSystem : SystemBase
{

    protected override void OnUpdate()
    {


        foreach (var (physicsPos, entity) in SystemAPI.Query<RefRO<LocalTransform>>()
                     .WithAll<VisualData>()
                     .WithNone<TransformReference>()
                     .WithEntityAccess())
        {
            VisualData visualData = EntityManager.GetComponentData<VisualData>(entity);
            
            if (visualData.Value != null)
            {
                GameObject newGO = Object.Instantiate(visualData.Value);

               
                newGO.transform.position = physicsPos.ValueRO.Position;
                newGO.transform.rotation = physicsPos.ValueRO.Rotation;
                EntityManager.AddComponentObject(entity, new TransformReference 
                { 
                    transform = newGO.transform 
                });
                EntityManager.RemoveComponent<VisualData>(entity);
            }
        }

        foreach (var (physicsPos, entity) in SystemAPI.Query<RefRO<LocalTransform>>()
                     .WithEntityAccess()
                 .WithAll<TransformReference>()
                 )
        {
            TransformReference visualRef = EntityManager.GetComponentObject<TransformReference>(entity);
            if (visualRef.transform != null)
            {
                visualRef.transform.position = physicsPos.ValueRO.Position;
                visualRef.transform.rotation = physicsPos.ValueRO.Rotation;
            }
        }
    

        
    }
}