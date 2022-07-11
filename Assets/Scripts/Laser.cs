using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;
 
    void Update()
    {
 
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        if(transform.position.y > 8)
        {
            Destroy(this.gameObject);
        }
    }
}
