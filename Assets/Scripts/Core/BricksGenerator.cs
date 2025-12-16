using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BricksGenerator : MonoBehaviour
{
    public BoxCollider bricksSpawnBounds;
    public GameObject brickPrefab;
    public int numOfBricks;


    private void Awake()
    {
       GenerateBricks();
    }

    private void GenerateBricks()
    {
        for (int i = 0; i < numOfBricks; i++)
        {
            Vector3 pos = bricksSpawnBounds.GetRandomPointInsideCollider();
            Instantiate(brickPrefab, pos, Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)) , this.transform);
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



public static class BoxColliderExtensions
{
    public static Vector3 GetRandomPointInsideCollider( this BoxCollider boxCollider )
    {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range( -extents.x, extents.x ),
            Random.Range( -extents.y, extents.y ),
            Random.Range( -extents.z, extents.z )
        );

        return boxCollider.transform.TransformPoint( point );
    }

}
