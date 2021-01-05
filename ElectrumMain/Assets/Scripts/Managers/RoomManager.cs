using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private const int ENEMY_TYPE_COUNT = 3, DECOR_TYPE_COUNT = 5;
    private const string INVOKE_METHOD_NAME = "SpawnObj", DECOR_TAG = "decoration";

    private float startX;
    private float startY;
    private float floorScale;

    [SerializeField] private GameObject spawnPref;
    [SerializeField] private GameObject portal;

    [SerializeField] private GameObject[] decorations = new GameObject[DECOR_TYPE_COUNT];
    [SerializeField] private GameObject[] enemies = new GameObject[ENEMY_TYPE_COUNT];
    [SerializeField] private List<GameObject> spawnPointsOnScene = new List<GameObject>();

    private void Start()
    {   
        Generation(spawnPointsOnScene, spawnPref);
        Invoke(INVOKE_METHOD_NAME, 1.2f);
    }

    private void SpawnObj() 
    {
        for(int i = 0; i < spawnPointsOnScene.Count; i++)
        {
            if(spawnPointsOnScene[i] == null)
            {
               spawnPointsOnScene.Remove(spawnPointsOnScene[i]);
            }
        }

		Spawn(decorations); // спавним декорации

        Spawn(enemies);  // спавним врагов
	}
    

    public static Vector2 RandomPoint(Bounds bounds)
    {
        return new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
    }    

    private void Generation(List<GameObject> spawnPointsOnScene, GameObject spawnPref)
    {
        floorScale = 8f;
        startX = transform.position.x - floorScale;
        startY = transform.position.y - floorScale;
        float maxX = transform.position.x + 16f; //в константу 16
        float maxY = transform.position.y + 16f; //в константу 16

        for (int x = (int)startX; x < (int)maxX; x++)
        {
            for(int y = (int)startY; y < (int)maxY; y++)
            {
                GameObject generateObject = Instantiate(spawnPref, new Vector2((float)x, (float)y), Quaternion.identity);
                generateObject.transform.SetParent(gameObject.transform);
                generateObject.AddComponent<SpawnPointBehavior>();
                spawnPointsOnScene.Add(generateObject);
            }
        }
    }

    private void Spawn(GameObject[] spawnObjects)
    {
        int maxFirst = 0;
        float posZ = 0f;
        switch (spawnObjects.Length)
        {
            case ENEMY_TYPE_COUNT:
                maxFirst = 28; // в константу
                posZ = 0f;
                break;
            case DECOR_TYPE_COUNT:
                maxFirst = 24;
                posZ = 20f;
                break;
        }
        for (int i = 0; i < spawnPointsOnScene.Count; i++)
        {
            int rnd = Random.Range(1, maxFirst);

            if(rnd == 1 && spawnPointsOnScene[i] != null)
            {
                int rndObj = Random.Range(0, spawnObjects.Length);
                GameObject instantiated = Instantiate(spawnObjects[rndObj], new Vector3(spawnPointsOnScene[i].transform.position.x, 
                spawnPointsOnScene[i].transform.position.y, posZ), Quaternion.identity);

                if(instantiated.tag == DECOR_TAG)
                {
                    instantiated.transform.SetParent(this.gameObject.transform);
                }
            }
        }
    }
}

