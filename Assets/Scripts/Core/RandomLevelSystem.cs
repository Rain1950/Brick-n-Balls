using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class RandomLevelSystem : SystemBase
{
    // Potrzebujemy generatora liczb losowych w ECS
    private Unity.Mathematics.Random _random;

    protected override void OnCreate()
    {
        // Inicjalizacja ziarna losowości (np. na podstawie czasu lub stała)
        _random = new Unity.Mathematics.Random(1234); 
    }

    protected override void OnUpdate()
    {
        // 1. Pobieramy konfigurację. Jeśli jej nie ma, kończymy.
        if (!SystemAPI.TryGetSingletonEntity<RandomLevelConfig>(out Entity configEntity)) return;
        
        RandomLevelConfig config = SystemAPI.GetComponent<RandomLevelConfig>(configEntity);

        // 2. Przygotowujemy dane wzorca wizualnego (jeśli jest potrzebny do skopiowania VisualData)
        // Zakładam, że prefab cegły ma VisualData.
        VisualData prototypeVisual = EntityManager.GetComponentData<VisualData>(config.BrickPrefab);

        // 3. Pętla generująca (Twoje "for i < numOfBricks")
        for (int i = 0; i < config.Count; i++)
        {
            // A. Instantiate Encji
            Entity newBrick = EntityManager.Instantiate(config.BrickPrefab);
            
            
            int randomHP = _random.NextInt(1, 4); // Losuje 1, 2 lub 3
            EntityManager.SetComponentData(newBrick, new BrickAuthoring.BrickHealth { Value = randomHP });
            
            // B. Losowa Pozycja (logika z Twojego GetRandomPointInsideCollider)
            float3 randomPos = _random.NextFloat3(config.BoundsMin, config.BoundsMax);

            // C. Losowa Rotacja Y (0-360 stopni)
            // W ECS używamy Quaternionów z biblioteki Mathematics
            float randomAngleY = _random.NextFloat(0, 360); // Stopnie
            quaternion rotation = quaternion.RotateY(math.radians(randomAngleY));

            // D. Ustawienie Transformu
            // Resetujemy rodzica (dla pewności)
            if (EntityManager.HasComponent<Parent>(newBrick))
                EntityManager.RemoveComponent<Parent>(newBrick);

            EntityManager.SetComponentData(newBrick, LocalTransform.FromPositionRotation(randomPos, rotation));

            // E. Ręczne kopiowanie wizualizacji (VisualData jest klasą, trzeba kopiować ręcznie w Unity 6)
            if (prototypeVisual != null)
            {
                EntityManager.AddComponentObject(newBrick, new VisualData 
                { 
                    Value = prototypeVisual.Value 
                });
            }
        }

        // 4. Niszczymy config, żeby nie generować cegieł w nieskończoność co klatkę
        EntityManager.DestroyEntity(configEntity);
    }
}