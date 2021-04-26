using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
