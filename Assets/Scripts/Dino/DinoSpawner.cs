using UnityEngine;

public class DinoSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f,1f)]
        public float spawnChance;
    };

    public SpawnableObject[] objects;

    public float minSpawnRate = 1.5f;
    public float maxSpawnRate = 2.5f;

    private void OnEnable()
    {
        int id = Random.Range(0, objects.Length);
        DinoGameController.Instance.idCollect = id;
        DinoGameController.Instance.collectImg.sprite = objects[id].prefab.GetComponent<SpriteRenderer>().sprite;
        DinoGameController.Instance.idCollectNameText.text = objects[id].prefab.name;

        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        float spawnChance = Random.value;

        for (int i = 0; i < objects.Length; ++i)
        {
            var obj = objects[i];

            if (spawnChance < obj.spawnChance)
            {
                GameObject obstacle = Instantiate(obj.prefab);
                obstacle.transform.position += transform.position;
                obstacle.GetComponent<DinoObstacle>().id = i;
                break;
            }
            spawnChance -= obj.spawnChance;
        }

        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }
}
