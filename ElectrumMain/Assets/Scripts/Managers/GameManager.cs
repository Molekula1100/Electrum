using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GameManager : MonoBehaviour
{
    private const float ACTIVE_DISTANCE_ROOM = 25f;

    public float timer;    
    public bool allEnemiesKilled;

    public static bool readyToSpawn;

    [SerializeField] private GameObject portalPref;
    [SerializeField] private GameObject startRoom;
    private GameObject player;
    private AstarPath astarPath;
    private RoomGenerator roomGenerator;

    public List<GameObject> rooms = new List<GameObject>();

    public static List<GameObject> enemiesOnScene = new List<GameObject>();

    private void Awake()
    {
        roomGenerator = GameObject.Find("StartRoom").GetComponent<RoomGenerator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Invoke("Scan", 0.1f);

        Invoke("AddObjects", 0.2f);
    }

    private void Scan()
    {
        astarPath = GameObject.Find("PathfindingController").GetComponent<AstarPath>();
        astarPath.Scan();
    }


    private void Update()
    {
        timer += Time.deltaTime;
        CheckEnemies();

        if(readyToSpawn)
        {
            DestroyRooms();
            Invoke("Scan", 0.1f);
            Invoke("Addbjects", 0.2f);
        }
    }

    private void CheckEnemies()
    {
        if(timer > 1.5f)
        {
            for(int i = 0; i < enemiesOnScene.Count - 1; i++)
            {
                allEnemiesKilled = false;
                if(enemiesOnScene[i] != null) return;
            }

            allEnemiesKilled = true;
            timer = 0f;
        }
    }

    private void DestroyRooms()
    {
        for(int i = 0; i < RoomGenerator.SpawnedObj.Count - 1; i++)
        {
            if(RoomGenerator.SpawnedObj[i] != null)
            {
                if(RoomGenerator.SpawnedObj[i]  != GameObject.Find("StartRoom"))
                {
                    Destroy(RoomGenerator.SpawnedObj[i]);
                } 
            }
        }

        foreach (GameObject hall in RoomGenerator.SpawnedHall) 
        { 
            Destroy(hall); 
        }

        foreach (GameObject aditional in RoomGenerator.SpawnedAditionalRoom) 
        {
            Destroy(aditional);
        }

        foreach (GameObject decoration in GameObject.FindGameObjectsWithTag("decoration")) 
        { 
            Destroy(decoration); 
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) 
        { 
            Destroy(enemy); 
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyClose"))
        {
            Destroy(enemy);
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyWorm")) 
        { 
            Destroy(enemy); 
        }

        Destroy(GameObject.FindGameObjectWithTag("Portal").transform.parent.transform.gameObject);

        Destroy(GameObject.Find("StartRoom"));
        GameObject start = Instantiate(startRoom, Vector2.zero, Quaternion.identity);
        start.name = "StartRoom";
        readyToSpawn = false;
        allEnemiesKilled = false;
        rooms.Clear();
        RoomGenerator.SpawnedObj.Clear();
        RoomGenerator.SpawnedHall.Clear();
    }
}
