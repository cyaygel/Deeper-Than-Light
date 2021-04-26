using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{
    public GameObject player;
    public float detectionRange = 5;
    Vector3 playerDistance;

    private void Start()
    {
        player = GameObject.Find("player");
    }

    void Update()
    {
        playerDistance = player.transform.position - transform.position;

        if(playerDistance.magnitude < detectionRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.1f);
        }
    }
}
