using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float lifetime = 2;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player" && other.tag != "enemyBullet")
        {

            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;

        if(lifetime<= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
