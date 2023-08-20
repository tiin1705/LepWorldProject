using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] 
    private GameObject _object;



    public void Spawn()
    {
        Instantiate(_object, transform.position, Quaternion.identity);
    }
}
