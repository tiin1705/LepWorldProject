using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float start, end;
    private bool isRight; // true => di chuyen phai, false => di chuyen trai
    public float Speed = 0f;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Left")
        {
            isRight = isRight ? false : true;
        }
    }
    private void Move() {
        // di chuyen
        var EnemyPosittion = transform.position.x;
        // di theo player
        if(player != null)
        {
            var PlayerPosittion = player.transform.position.x;
            if (PlayerPosittion > start && PlayerPosittion < end)
            {
                if (PlayerPosittion < EnemyPosittion) isRight = false;
                if (PlayerPosittion > EnemyPosittion) isRight = true;
            }
        }
        


        if (EnemyPosittion < start)
        {
            isRight = true;
        }
        if (EnemyPosittion > end)
        {
            isRight = false;
        }
        Vector2 scale = transform.localScale;
        if (isRight)
        {
            scale.x = -1;
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }
        else
        {
            scale.x = 1;
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }
        transform.localScale = scale;
    }

    public void SetStart(float start)
    {
        this.start = start;
    }

    public void SetEnd(float end)
    {
        this.end = end;
    }
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
