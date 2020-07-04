using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Script.Command;

public class GameManager : MonoBehaviour
{
    private int level = 1;
    public int food = 100;
    public List<Enemy> _Enemies = new List<Enemy>();
    
    private static GameManager _gameManager;
    private bool sleepStep;
    private Text _text;
    private Text _failedText;
    private Player _player;

    // 命令管理器
    public CommandManager _commandManager;
    public static GameManager Instance
    {
        get => _gameManager;
    }
    
    public int Level
    {
        get => level;
        set => level = value;
    }

    private void Awake()
    {
        _gameManager = this;
        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    private void Start()
    {
        // 绑定玩家移动时
        
        SceneManager.sceneLoaded += OnSceneLoaded;


    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ++Level;

        InitGame();
    }

    void InitGame()
    {
        _commandManager = new CommandManager();
        
        _Enemies.Clear();
        GetComponent<MapManager>().InitMap();

        DealText();
        
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.enabled = false;
        
        StartCoroutine(HideImage());
    }

    void DealText()
    {
        _text = GameObject.Find("Text").GetComponent<Text>();
        _failedText = GameObject.Find("FailedText").GetComponent<Text>();

        UpdateFoodText(0);
        _failedText.enabled = false;
        GameObject.Find("DayText").GetComponent<Text>().text = "Day " + level;
    }

    void UpdateFoodText(int Change)
    {
        if (Change == 0)
            _text.text = "Food:" + food;
        else
        {
            string str = Change > 0 ? "+" : "";
            _text.text = str + Change + "   Food:" + food;
        }
    }

    public void AddFood(uint count)
    {
        food += (int)count;
        UpdateFoodText((int)count);
    }

    public void ReduceFood(uint count)
    {
        food -= (int) count;
        UpdateFoodText(-(int)count);
        if (food <= 0)
            _failedText.enabled = true;
    }


    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
    
    
    IEnumerator HideImage()
    {
        yield return new WaitForSeconds(1);
        GameObject.Find("DayImage").gameObject.SetActive(false);
        _player.enabled = true;
    }
}
