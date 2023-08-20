using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float start, end;
    public GameObject player;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // lay vi tri truc x cua player
        var playerX = player.transform.position.x;
        // lay vi tri truc x cua camera

        var camX = transform.position.x;
        if(playerX > start && playerX < end)
        {
            camX = playerX;
        }
        else
        {
            if(playerX < start)
            {
                camX = start;
            }
            else
            {
                camX = end;
            }
        }
        transform.position = new Vector3(camX, 0, -20);
    }
}
