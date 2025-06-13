using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    private struct SpawnableObject
    {
        public GameObject prefab; // Doğacak nesnenin prefab'ı
        [Range(0f, 1f)] public float spawnChance;
    }

    [SerializeField] private SpawnableObject[] spawnableObjects;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 2f;

    private void OnEnable()
    {
        ScheduleNextSpawn();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void ScheduleNextSpawn()
    {
        float delay = Random.Range(minSpawnInterval, maxSpawnInterval); // Rastgele bir bekleme süresi seç
        Invoke(nameof(Spawn), delay); // Belirtilen süre sonunda Spawn metodunu çağır
    }

    private void Spawn()
    {
        SpawnRandomObject();
        ScheduleNextSpawn();
    }

    private void SpawnRandomObject()
    {
        float randomValue = Random.value;

        foreach (var obj in spawnableObjects)
        {
            if (randomValue < obj.spawnChance)
            {
                InstantiateObject(obj);
                break;
            }
            randomValue -= obj.spawnChance; // Olasılığı düşür, bir sonraki nesneye geç
        }
    }

    private void InstantiateObject(SpawnableObject obj)
    {
        Instantiate(obj.prefab, transform.position, Quaternion.identity);
    }
}