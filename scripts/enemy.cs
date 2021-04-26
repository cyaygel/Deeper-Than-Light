using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float patrolSpeed;
    public float patrolWideness;
    public float detectionRange = 50;
    public float bulletSpeed = 20;
    public float health = 20;
    public float attackCooldown;

   
    public bool onLeft = false;
    public bool playerSeen = false;

    public GameObject bulletPrefab;
    public GameObject lightDrop;

    GameObject player;
    Vector3 startPos;
    Vector3 patrolBorder01;
    Vector3 patrolBorder02;

    Rigidbody rb;
    private void Start()
    {
        player = GameObject.Find("player");
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        patrolBorder01 = startPos - transform.right * patrolWideness;
        patrolBorder02 = startPos + transform.right * patrolWideness;
    }

    void Update()
    {
        attackCooldown += Time.deltaTime;

        Debug.DrawLine(patrolBorder01, patrolBorder02);

        if (Vector3.Distance(player.transform.position, transform.position) < detectionRange)
        {
            //Debug.Log("player seen");

            RaycastHit hit;
            Vector3 rayDirection;
            rayDirection = transform.position - player.transform.position;

            if (Physics.Raycast(transform.position, -rayDirection, out hit))
            {
                Debug.DrawRay(transform.position, -rayDirection, Color.red, 1);
                if(hit.collider.tag != "Player")
                {

                    Debug.Log(hit.collider.tag);
                    Debug.Log("there is an obstacle in thye way");

                    playerSeen = false;
                }
                else
                {
                    //player seen
                    playerSeen = true;
                }
            }
        }

        if (playerSeen)
        {
            Shoot();
        }

        else
        {
            playerSeen = false;
        }

        if(Mathf.FloorToInt(Vector3.Distance(transform.position, patrolBorder01)) == 0)
        {
            onLeft = true;
        }

        if(Mathf.FloorToInt(Vector3.Distance(transform.position, patrolBorder02)) == 0)
        {
            onLeft = false;
        }

        if (onLeft)
        {
            walkRight();
        }
        else
        {
            walkLeft();
        }

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Drop();
        Drop();
        Drop();
        Destroy(this.gameObject);
    }


    void Drop()
    {
        GameObject l;

        l = Instantiate(lightDrop, transform.position, transform.rotation);

        l.GetComponent<Rigidbody>().AddForce (new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0) * 5);
    }
    void Shoot()
    {
        GameObject b;

        Vector3 direction;
        direction = transform.position - player.transform.position;

        if(attackCooldown > 1.4f)
        {
            b = Instantiate(bulletPrefab, transform.position, transform.rotation);
            b.GetComponent<Rigidbody>().AddForce(-direction * bulletSpeed);
            attackCooldown = 0;
        }
       
        
    }
    
    void walkRight()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolBorder02, patrolSpeed);

    }

    void walkLeft()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolBorder01, patrolSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "bullet")
        {
            health -= 10;
        }
    }
}
