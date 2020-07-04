using System;
using System.Collections;
using System.Collections.Generic;
using Script.Command;
using UnityEngine;

// 玩家移动代理
public delegate void PlayerMoveDelegate();

public class Player : MonoBehaviour
{
    public float smoothing = 40;
    public float restTime = 0.5f;
    private float restTimeHandle = 0;
    [Header("Audio")]
    public AudioClip[] _attackAudio;
    public AudioClip[] _footAudio;
    public AudioClip[] _foodAudio;
    public AudioClip[] _sodaAudio;
    public AudioClip _deathAudio;
    
    private Rigidbody2D _rigidbody2D;
    private Vector3 targetPostion = new Vector3(1,1,0);
    private BoxCollider2D _boxCollider2D;
    private Animator _animator;
    private Vector3 tempPostion = new Vector3();
    
    // 当玩家移动时
    public event PlayerMoveDelegate onPlayerMove;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _boxCollider2D = this.GetComponent<BoxCollider2D>();
        _animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.MovePosition(Vector2.Lerp(transform.position, targetPostion, smoothing * Time.deltaTime));
        
        if(GameManager.Instance.food<=0)
            return;

        // 每过restTime才可以继续操作
        restTimeHandle += Time.deltaTime;
        if (restTimeHandle < restTime)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        // x 的优先级比 y 高
        if (x != 0)
            y = 0;
        
        if (x != 0 || y != 0)
        {
            tempPostion.x = x;
            tempPostion.y = y;
            CommandManager.Instance().ExecuteCommand(new PlayerMoveCommand(this,targetPostion + tempPostion));
        }
        
    }

    public void MoveTo(Vector3 newPostion)
    {

        if(onPlayerMove != null)
            onPlayerMove();
            
        restTimeHandle = 0;
            
        AudioManager.Instance.RandomClip(_footAudio);
            
        _boxCollider2D.enabled = false;
        RaycastHit2D hit =  Physics2D.Linecast(targetPostion,newPostion);
        _boxCollider2D.enabled = true;

        if (!hit.transform)
        {
            targetPostion = newPostion;
        }
        else
        {
            switch (hit.collider.tag)
            {
                case "wall":
                    _animator.SetTrigger("attack");
                    AudioManager.Instance.RandomClip(_attackAudio);
                    hit.collider.SendMessage("OnDamage",null,SendMessageOptions.DontRequireReceiver);
                    break;
                case "outWall":
                    break;
                case "food":
                    GameManager.Instance.AddFood(10);
                    AudioManager.Instance.RandomClip(_foodAudio);
                    Destroy(hit.transform.gameObject);
                    targetPostion = newPostion;
                    break;
                case "soda":
                    GameManager.Instance.AddFood(20);
                    AudioManager.Instance.RandomClip(_sodaAudio);
                    Destroy(hit.transform.gameObject);
                    targetPostion = newPostion;
                    break;
                case "end":
                    GameManager.Instance.EndGame();
                    break;
            }
        }
    }
        

    public void OnDamage(uint reduceFoodCount)
    {
        GameManager.Instance.ReduceFood(reduceFoodCount);
        _animator.SetTrigger("damage");
    }
}

public class PlayerMoveCommand : ICommand
{
    private Vector3 _oldPosition;
    private Vector3 _targetPosition;
    private Player _player;

    public PlayerMoveCommand(Player player, Vector3 newPosition)
    {
        _player = player;
        _oldPosition = player.transform.position;
        _targetPosition = newPosition;
    }
    
    public void DoCommand()
    {
        _player.MoveTo(_targetPosition);
        GameManager.Instance.ReduceFood(1);
    }

    public void UnDoCommand()
    {
        _player.MoveTo(_oldPosition);
        GameManager.Instance.AddFood(1);
    }
}