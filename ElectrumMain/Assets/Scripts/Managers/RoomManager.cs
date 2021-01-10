using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private const int ENEMY_TYPE_COUNT = 3, DECOR_TYPE_COUNT = 6;
    private const float FLOOR_SCALE = 16F;
    private const string SPAWN_METHOD = "SpawnAll", DECOR_TAG = "decoration";

    private float startX;
    private float startY;

    [Header("The chance counts in percents")]
    [Range(0, 100)] public float enemySpawnChance; 
    [Range(0, 100)] public float decorSpawnChance;

    private List<GameObject> spawnPointsInRoom = new List<GameObject>();

    [SerializeField] private GameObject spawnPointPref;
    [SerializeField] private GameObject portal;

    [SerializeField] private GameObject[] decorations = new GameObject[DECOR_TYPE_COUNT];
    [SerializeField] private GameObject[] enemies = new GameObject[ENEMY_TYPE_COUNT];

    private void Start()
    {   
        Invoke(SPAWN_METHOD, 1.2f);
    }

    private void SpawnAll() 
    {
        foreach(GameObject room in RoomGenerator.SpawnedObj)
        {
            if(room != RoomGenerator.SpawnedObj[0])
            {
                GenerationSpawnPoints(spawnPointsInRoom, spawnPointPref, room);
                for(int i = 0; i < spawnPointsInRoom.Count; i++)
                {
                    if(spawnPointsInRoom[i] == null)
                    {
                       spawnPointsInRoom.Remove(spawnPointsInRoom[i]);
                    }
                }

		        SpawnObjects(decorations, room);
                SpawnObjects(enemies, room); 

                spawnPointsInRoom.Clear();
            }
        }
	} 

    private void GenerationSpawnPoints(List<GameObject> spawnPointsInRoom, GameObject spawnPointPref, GameObject room)
    {
        startX = room.transform.position.x - (FLOOR_SCALE / 2);
        startY = room.transform.position.y - (FLOOR_SCALE / 2);
        float maxX = room.transform.position.x + FLOOR_SCALE; 
        float maxY = room.transform.position.y + FLOOR_SCALE; 

        for (int x = (int)startX; x < (int)maxX; x++)
        {
            for(int y = (int)startY; y < (int)maxY; y++)
            {
                GameObject generatedSpawnPoint = Instantiate(spawnPointPref, new Vector2((float)x, (float)y), Quaternion.identity);
                generatedSpawnPoint.transform.SetParent(gameObject.transform);
                generatedSpawnPoint.AddComponent<SpawnPointBehavior>();
                spawnPointsInRoom.Add(generatedSpawnPoint);
            }
        }
    }

    private void SpawnObjects(GameObject[] objectsToSpawn, GameObject room)
    {
        int spawnChance = 0;
        float posZ = 0f;
        switch (objectsToSpawn.Length)
        {
            case ENEMY_TYPE_COUNT:
                spawnChance = ((int)(100f / decorSpawnChance)); 
                posZ = 0f;
                break;
            case DECOR_TYPE_COUNT:
                spawnChance = ((int)(100f / enemySpawnChance));
                posZ = 20f;
                break;
        }

        for (int i = 0; i < spawnPointsInRoom.Count; i++)
        {
            int spawnChanceNum = Random.Range(1, spawnChance);

            if(spawnChanceNum == 1 && spawnPointsInRoom[i] != null)
            {
                int randomSpawnObj = Random.Range(0, objectsToSpawn.Length);
                GameObject instantiated = Instantiate(objectsToSpawn[randomSpawnObj], new Vector3(spawnPointsInRoom[i].transform.position.x, 
                spawnPointsInRoom[i].transform.position.y, posZ), Quaternion.identity);

                if(instantiated.tag == DECOR_TAG)
                {
                    instantiated.transform.SetParent(room.transform);
                }
            }
        }
    }
}
