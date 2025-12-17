using Unity.Entities;
using UnityEngine;

public class VisualData : IComponentData
{
    public GameObject Value; // visual prefab
}

public class TransformReference : IComponentData
{
    public Transform transform;
}