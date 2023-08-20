using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int count = 1;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // tu dong spawn bug
        if (count-- > 0)
        {
            float posittion = Random.Range(-5f, 6f);
            GameObject bug = (GameObject)Instantiate(Resources.Load("Prefabs/Bug"),
                        new Vector3(posittion, -3.5f, 0), Quaternion.identity);
            bug.GetComponent<EnemyScript>().SetStart(posittion - 5f);
            bug.GetComponent<EnemyScript>().SetEnd(posittion + 5f);
            bug.GetComponent<EnemyScript>().SetPlayer(player);
        }
    }
}
