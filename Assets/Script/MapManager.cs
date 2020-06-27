using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("prefabs")]
    public GameObject[] outWalls;// 外墙
    public GameObject[] floorWalls;// 地板
    public GameObject[] walls;// 障碍物
    public GameObject[] foods;
    public GameObject[] Enemys;
    public GameObject exitPrefab;
    
    [Header("settings")]
    public int rows = 10;// 列
    public int cols = 10;// 行
    public int minWallCount = 2;
    public int maxWallCount = 8;

    private List<Vector2> postions;// items等layer可以放置的位置

    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 初始化地图
    /// </summary>
    public void InitMap()
    {
        postions = new List<Vector2>();
        gameManager = this.GetComponent<GameManager>();
        
        // 生成围墙和地板
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (x == 0 || y == 0 || x == cols - 1 || y == rows - 1)
                {
                    GameObject prefab = RandomPrefab(outWalls);
                    GameObject.Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
                }
                else
                {
                    GameObject prefab = RandomPrefab(floorWalls);
                    GameObject.Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
                }
                
            }
        }

        postions.Clear();
        for (int x = 2; x < cols-2; x++)
        {
            for (int y = 2; y < rows-2; y++)
            {
                postions.Add(new Vector2(x, y));
            }
        }

        // 生成障碍物
        int currentWallCount = Random.Range(minWallCount, maxWallCount + 1);
        for (int i = 0; i < currentWallCount; i++)
        {
            // 获取障碍物位置
            Vector2 postion = RandomPostion();
            
            GameObject prefab = RandomPrefab(walls);
            GameObject.Instantiate(prefab, postion, Quaternion.identity);
        }
        
        // 生成食物 2-Level
        int currentFoodCount = Random.Range(2, gameManager.Level + 1);
        for (int i = 0; i < currentFoodCount; i++)
        {
            // 获取障碍物位置
            Vector2 postion = RandomPostion();
            
            GameObject prefab = RandomPrefab(foods);
            GameObject.Instantiate(prefab, postion, Quaternion.identity);
        }
        
        // 生成敌人 0-Level/2
        int currentEnemyCount = Random.Range(0, gameManager.Level/2 + 1);
        for (int i = 0; i < currentEnemyCount; i++)
        {
            // 获取障碍物位置
            Vector2 postion = RandomPostion();
            
            GameObject prefab = RandomPrefab(Enemys);
            GameObject.Instantiate(prefab, postion, Quaternion.identity);
        }
        
        // 生成出口
        Instantiate(exitPrefab, new Vector3(cols - 2, rows - 2), Quaternion.identity);
    }

    /// <summary>
    /// 随机一个prefab
    /// </summary>
    /// <param name="prefabs">预制体</param>
    /// <returns></returns>
    public GameObject RandomPrefab(GameObject[] prefabs)
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

    /// <summary>
    /// 随机获取一个可生成的位置
    /// </summary>
    /// <returns>位置</returns>
    public Vector2 RandomPostion()
    {
        // 获取Items位置
        int positionIndex = Random.Range(0, postions.Count);
        Vector2 postion = postions[positionIndex];
        postions.RemoveAt(positionIndex);
        return postion;
    }
    
    
}
