using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : MonoBehaviour
{
    private GameObject _player;
    private Vector2 targetPostion;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider;
    private Animator _animator;

    
    public uint lossFood = 10;
    public AudioClip[] _attackAudio;
    private bool sleepMove = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("player");
        targetPostion = transform.position;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();

        _player.GetComponent<Player>().onPlayerMove += Move;
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.MovePosition(Vector2.Lerp(transform.position,targetPostion,20*Time.deltaTime));

    }

    public void Move()
    {
        if (sleepMove)
        {
            sleepMove = false;
            return;
        }

        sleepMove = true;
        
        
        Vector2 offset = _player.transform.position - transform.position;
        if (offset.magnitude <= 1.1f)
        {
            // 攻击
            _animator.SetTrigger("attack");
            AudioManager.Instance.RandomClip(_attackAudio);
            _player.SendMessage("OnDamage",lossFood);
        }
        else
        {
            float x = 0, y =0;
            // 移动
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
            {
                if (offset.x > 0)
                    x = 1;
                else
                    x = -1;
            }
            else
            {
                if (offset.y > 0)
                    y = 1;
                else
                    y = -1;
            }
    
            _collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPostion+ new Vector2(x, y), targetPostion );
            _collider.enabled = true;
            
            if(!hit.transform)
                targetPostion += new Vector2(x,y);
            else
            {
                Debug.Log("物体：" + hit.transform);
                switch (hit.collider.tag)
                {
                    case "food":
                        targetPostion += new Vector2(x,y);
                        break;
                    case "soda":
                        targetPostion += new Vector2(x,y);
                        break;
                    case "Enemy":
                        targetPostion += new Vector2(x,y);
                        break;
                }
            }
        }

    }
    
    
}
