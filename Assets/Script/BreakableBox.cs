using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableBox : MonoBehaviour
{
    [SerializeField] private UnityEvent _hit;
    private void Awake()
    {
     
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<PlayerMovement>();
        if((player && collision.contacts[0].normal.y > 0)){
            _hit.Invoke();
        }
    }
}
