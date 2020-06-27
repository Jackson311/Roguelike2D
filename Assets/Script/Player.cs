using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Vector2 tagerPostion = new Vector2(1,1);
    private BoxCollider2D _boxCollider2D;
    private Animator _animator;
    
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
        _rigidbody2D.MovePosition(Vector2.Lerp(transform.position, tagerPostion, smoothing * Time.deltaTime));
        
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
            restTimeHandle = 0;
            GameManager.Instance.OnPlayerMove();
            AudioManager.Instance.RandomClip(_footAudio);

            
            _boxCollider2D.enabled = false;
            RaycastHit2D hit =  Physics2D.Linecast(tagerPostion, tagerPostion + new Vector2(x, y));
            _boxCollider2D.enabled = true;

            if (!hit.transform)
            {
                tagerPostion += new Vector2(x, y);
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
                        tagerPostion += new Vector2(x, y);
                        break;
                    case "soda":
                        GameManager.Instance.AddFood(20);
                        AudioManager.Instance.RandomClip(_sodaAudio);
                        Destroy(hit.transform.gameObject);
                        tagerPostion += new Vector2(x, y);
                        break;
                    case "end":
                        GameManager.Instance.EndGame();
                        break;
                }
            }
            
        }
        
    }

    public void OnDamage(uint reduceFoodCount)
    {
        GameManager.Instance.Reduce(reduceFoodCount);
        _animator.SetTrigger("damage");
    }
}
