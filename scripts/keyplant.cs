using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyplant : MonoBehaviour
{
    public GameObject obstacles;

    private void Start()
    {
        obstacles = transform.GetChild(1).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "bullet")
        {
            obstacles.GetComponent<Animator>().SetTrigger("activate");
            GetComponent<Animator>().SetTrigger("activate");
        }

       

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && this.tag == "keyPlant02")
        {

            //Debug.Log("sorunvarr");

            if (other.gameObject.GetComponent<playerController>().flashUnlocked == true && other.gameObject.GetComponent<playerController>().lightAmount > 20 && Input.GetKeyDown("space"))
            {
                obstacles.GetComponent<Animator>().SetTrigger("activate");
                //GetComponent<Animator>().SetTrigger("activate");
            }

        }
    }
}
