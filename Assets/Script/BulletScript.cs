using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 5f;
    private bool isRight;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //di chuyen dan
    public void Movement()
    {
        transform.Translate((isRight ? Vector3.right : Vector3.left) * Time.deltaTime * 20f);
    }

    public void setIsRight(bool isRight)
    {
        this.isRight = isRight;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Up_enemy" || collision.gameObject.tag == "Left_Enemy")
            {
                //kill enemy
                Destroy(gameObject);
                var name = collision.attachedRigidbody.name;
                Destroy(GameObject.Find(name));
            }
       
    }

}
