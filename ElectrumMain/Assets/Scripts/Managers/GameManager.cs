using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class GameManager : MonoBehaviour
{
    private const string SCAN_METHOD = "Scan", PATHFINDING_CONTROLLER = "PathfindingController";

    private AstarPath astarPath;
    private Player player;

    [SerializeField] private GameObject portalPref;

    public List<GameObject> rooms = new List<GameObject>();

    public static bool allEnemiesKilled; 

    public static List<GameObject> enemiesOnScene = new List<GameObject>();

    private void Start()
    {
        player = GameObject.Find(Player.uniqName).GetComponent<Player>();
        player.LoadNextLevel += LoadNextLevel;

        allEnemiesKilled = false;
        Invoke(SCAN_METHOD, 0.1f);
    }

    private void Scan()
    {
        astarPath = GameObject.Find(PATHFINDING_CONTROLLER).GetComponent<AstarPath>();
        astarPath.Scan();
    }

    private void Update()
    {
        CheckEnemies();
    }

    private void CheckEnemies()
    {
        for(int i = 0; i < enemiesOnScene.Count - 1; i++)
        {
            allEnemiesKilled = false;
            if(enemiesOnScene[i] != null) return;
        }
        allEnemiesKilled = true;

    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
