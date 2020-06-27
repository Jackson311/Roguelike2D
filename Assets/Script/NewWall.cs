using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWall : MonoBehaviour
{
    public Sprite damageSprite;

    private int hp = 2;
    
    public void OnDamage()
    {
        hp -= 1;
        if (hp == 1)
        {
            this.GetComponent<SpriteRenderer>().sprite = damageSprite;
        }
        else
        {
            Destroy(this.gameObject);
        }
    
    }


}
