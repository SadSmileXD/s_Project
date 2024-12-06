using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    
 

   
    private void Awake()
    {
       
    }
    void Update()
    {
         float h = Input.GetAxis("Horizontal");
         float v = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(h, 0, v);
        transform.Translate(moveVector * 10f * Time.deltaTime);
    }
}
