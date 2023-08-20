using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBoxScript : MonoBehaviour
{

    public Vector2 originalPosition;
    public float speed; //tốc độ nẩy
    public float bounce; //khoảng cách nẩy
    public float ItemSpeedUp; //Tốc độ nẩy của item
    GameObject Player;

    public GameObject pre_coin;
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.contacts[0].normal.y > 0){
            ItemGoUp();
            Destroy(gameObject);
        }
    }
    //nẩy lên

    IEnumerator BounceUp()
    {
        //nẩy lên
        while (true)
        {

            Debug.Log(">>>>>>>>(Itembox Bounce)");

            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + speed * Time.deltaTime
                );
            if (transform.position.y > originalPosition.y + bounce)
                break;
            yield return null;

        }
    }

    private void Coin()
    {
        GameObject coin = (GameObject)Instantiate(pre_coin);
        coin.transform.SetParent(this.transform.parent);
        coin.transform.localPosition = new Vector2(originalPosition.x, originalPosition.y + 1f);
        StartCoroutine(CoinBounce(coin));
    }
    private void ItemGoUp()
    {
        StartCoroutine(BounceUp());
       
       

            Coin();
            Debug.Log(">>>>>>>>(Coin)");

        }
   

        IEnumerator CoinBounce(GameObject Coin)
    {
        while (true)
        {

            Debug.Log(">>>>>>>>(CoinBounce)");

            Coin.transform.position = new Vector3(
                Coin.transform.position.x,
                Coin.transform.position.y + ItemSpeedUp * Time.deltaTime
                );
            if (Coin.transform.position.y > originalPosition.y + 2f)
                break;
            yield return null;

        }

    }
  
  
}

