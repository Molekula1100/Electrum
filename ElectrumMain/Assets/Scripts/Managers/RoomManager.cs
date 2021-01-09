using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private const int ENEMY_TYPE_COUNT = 3, DECOR_TYPE_COUNT = 5;
    private const float FLOOR_SCALE = 8f;
    private const string INVOKE_METHOD_NAME = "SpawnAll", DECOR_TAG = "decoration", GAME_CONTROLLER = "GameController";

    private float startX;
    private float startY;

    private GameManager gameManager;

    [SerializeField] private GameObject spawnPointPref;
    [SerializeField] private GameObject portal;

    [SerializeField] private GameObject[] decorations = new GameObject[DECOR_TYPE_COUNT];
    [SerializeField] private GameObject[] enemies = new GameObject[ENEMY_TYPE_COUNT];
    [SerializeField] private List<GameObject> spawnPointsOnScene = new List<GameObject>();

    private void Start()
    {   
        gameManager = GameObject.Find(GAME_CONTROLLER).GetComponent<GameManager>();

        GenerationSpawnPoints(spawnPointsOnScene, spawnPointPref);
        Invoke(INVOKE_METHOD_NAME, 1.2f);
    }

    private void SpawnAll() 
    {
        for(int i = 0; i < spawnPointsOnScene.Count; i++)
        {
            if(spawnPointsOnScene[i] == null)
            {
               spawnPointsOnScene.Remove(spawnPointsOnScene[i]);
            }
        }

		SpawnObjects(decorations);

        SpawnObjects(enemies); 
	} 

    private void GenerationSpawnPoints(List<GameObject> spawnPointsOnScene, GameObject spawnPointPref)
    {
        startX = transform.position.x - FLOOR_SCALE;
        startY = transform.position.y - FLOOR_SCALE;
        float maxX = transform.position.x + (2 * FLOOR_SCALE); 
        float maxY = transform.position.y + (2 * FLOOR_SCALE); 

        for (int x = (int)startX; x < (int)maxX; x++)
        {
            for(int y = (int)startY; y < (int)maxY; y++)
            {
                GameObject generatedSpawnPoint = Instantiate(spawnPointPref, new Vector2((float)x, (float)y), Quaternion.identity);
                generatedSpawnPoint.transform.SetParent(gameObject.transform);
                generatedSpawnPoint.AddComponent<SpawnPointBehavior>();
                spawnPointsOnScene.Add(generatedSpawnPoint);
            }
        }
    }

    private void SpawnObjects(GameObject[] spawnObjects)
    {
        int spawnChance = 0;
        float posZ = 0f;
        switch (spawnObjects.Length)
        {
            case ENEMY_TYPE_COUNT:
                spawnChance = ((int)(100f / gameManager.decorSpawnChance)); 
                posZ = 0f;
                break;
            case DECOR_TYPE_COUNT:
                spawnChance = ((int)(100f / gameManager.enemySpawnChance));
                posZ = 20f;
                break;
        }

        for (int i = 0; i < spawnPointsOnScene.Count; i++)
        {
            int spawnChanceNum = Random.Range(1, spawnChance);

            if(spawnChanceNum == 1 && spawnPointsOnScene[i] != null)
            {
                int randomSpawnObj = Random.Range(0, spawnObjects.Length);
                GameObject instantiated = Instantiate(spawnObjects[randomSpawnObj], new Vector3(spawnPointsOnScene[i].transform.position.x, 
                spawnPointsOnScene[i].transform.position.y, posZ), Quaternion.identity);

                if(instantiated.tag == DECOR_TAG)
                {
                    instantiated.transform.SetParent(this.gameObject.transform);
                }
            }
        }
    }
}
