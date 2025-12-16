using System;
using Core;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class CameraShooter : MonoBehaviour
{
    public float shootForce = 20f; 

    public int ammoCount;

    private EntityManager _entityManager;

    public static  event Action<int> OnShoot; 

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        ammoCount = GameManager.Instance.ballCount;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) 
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (ammoCount > 0)
        {
            ammoCount--;
            OnShoot?.Invoke(ammoCount);
            float3 spawnPosition = transform.position;
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            float3 direction = 
                (Camera.main.ScreenToWorldPoint( new Vector3(mousePosition.x, mousePosition.y, 5)) - 
                 transform.position).normalized;
        
            
            Entity requestEntity = _entityManager.CreateEntity();
            
            _entityManager.AddComponentData(requestEntity, new SpawnBallRequest
            {
                Position = spawnPosition, 
                Velocity =  direction * shootForce 
            });
        }
       
    }
}