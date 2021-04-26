using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    public float playerSpeed = 0;
    public float flashSpeed = 0.8f;
    public float dashSpeed = 10;
    public float flashLimit;
    public float flashCooldown;
    public float minRange = 1;
    public float maxRange = 16;
    public float bulletSpeed = 10;
    public float keyCount;

    [Range(0, 100)]
    public int lightAmount;

    public bool canFlash = true;
    public bool shotUnlocked = false;
    public bool flashUnlocked = false;
    public bool dashUnlocked = false;
    public bool isDashing = false;
    public bool onDashPlant = false;

    private float flashTimer = 0;
    private float dashTimer = 0;
    private float startLightRange;

    public GameObject seeingLightObject;
    public GameObject flashLightObject;
    public GameObject bulletPrefab;
    public GameObject dashEffect;
    public GameObject shotLocked;
    public GameObject flashLocked;
    public GameObject dashlocked;
    public GameObject gameoverPanel;
    public GameObject youwinPanel;
    public GameObject pausePanel;

    public Text keyText;
    public Text lightAmountText;
    public Slider lightAmountSlider;

    Light seeingLight;

    Vector3 movement;
    Rigidbody rb;


    private void Start()
    {
        lightAmount = 50;
        seeingLightObject = GameObject.Find("seeing Light");
        flashLightObject = GameObject.Find("flashLight");
        dashEffect = GameObject.Find("dashEffect");
        seeingLight = seeingLightObject.GetComponent<Light>();
        seeingLight.range = 1;
        startLightRange = seeingLight.range;
        rb = GetComponent<Rigidbody>();
        dashEffect.GetComponent<ParticleSystem>().enableEmission = false;
    }
    private void Update()
    {

        if(lightAmount > 100)
        {
            lightAmount = 100;
        }

        if(lightAmount <= 0)
        {
            GameOver();
        }

        if(keyCount == 3)
        {
            youwinPanel.SetActive(true);
        }

        if (Input.GetKeyDown("escape"))
        {
            if (pausePanel.activeInHierarchy)
            {
                pausePanel.SetActive(false);
            }
            else
            {
                pausePanel.SetActive(true);
            }
            
        }

        updateTexts();
        seeingLight.range = minRange + lightAmount * ((maxRange -minRange)/100);

        flashTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        flashCooldown -= Time.deltaTime;

        if (Input.GetButtonDown("Fire2"))
        {
            if (dashUnlocked)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.parent == null)
                    {
                        dashTimer = 0;
                        dashEffect.GetComponent<ParticleSystem>().enableEmission = true;
                        isDashing = true;
                    }
                    else
                    {
                        Debug.Log("cant dash there");
                    }
                }
            }
            else
            {
                Debug.Log("this skill is not unlocked");
            }
           

           
        }

        if (dashTimer > 0.2f)
        {
            isDashing = false;
            dashEffect.GetComponent<ParticleSystem>().enableEmission = false;
        }

        if (isDashing)
        {
            molekulertrasnportasyon();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetKeyDown("space"))
        { 
            flash();
        }
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        movement.z = 0;

        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * playerSpeed;
    }

    void updateTexts()
    {
        keyText.text = keyCount.ToString() + "/3";
        lightAmountText.text = lightAmount.ToString() + "/100";
        lightAmountSlider.value = lightAmount;
    }

    void molekulertrasnportasyon()
    {
        //Debug.Log("sal beni ýþýnlanacam");

        Vector3 mousePos;
        Vector3 direction;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        direction = transform.position - mousePos;
        direction.z = 0;
        mousePos.z = 0;
        
        transform.position = Vector3.Lerp(transform.position, mousePos, dashSpeed * Time.deltaTime );
        
    }

    void flash()
    {
        if (flashUnlocked)
        {
            if (lightAmount > 20)
            {
                lightAmount -= 20;
                flashLightObject.GetComponent<Animator>().SetTrigger("flash");
                Camera.main.GetComponent<Animator>().SetTrigger("flash");
            }
            else
            {
                Debug.Log("not enough light");
            }
        }
        else
        {
            Debug.Log("this skill is not unlocked");
        }
        
    }

    void Shoot()
    {
        if (shotUnlocked)
        {
            GameObject b;

            Vector3 mousePos;
            Vector3 direction;

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            direction = transform.position - mousePos;

            if (GameObject.Find("shot(Clone)") == null)
            {
                if(lightAmount > 5)
                {
                    b = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    b.GetComponent<Rigidbody>().AddForce(-direction * bulletSpeed);
                    lightAmount -= 5;
                }
                else
                {
                    Debug.Log("dont have enough light");
                }
               
            }
            else
            {
                Debug.Log("you cant fire that much");
            }
           

        }
        else
        {
            Debug.Log("this skill is not unlocked");
        }

    }

    public void GameOver()
    {
        gameoverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void mainMenu()
    {
        Application.Quit();
    }

    public void TakeDamage (int damageAmount)
    {
        lightAmount -= damageAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Light")
        {
            lightAmount += 10;
            Destroy(other.gameObject);
        }

        if(other.tag == "shotUnlock")
        {
            shotUnlocked = true;
            Destroy(other.gameObject);
            shotLocked.SetActive(false);
        }

        if(other.tag == "flashUnlock")
        {
            flashUnlocked = true;
            Destroy(other.gameObject);
            flashLocked.SetActive(false);
        }

        if (other.tag == "dashUnlock")
        {
            dashUnlocked = true;
            Destroy(other.gameObject);
            dashlocked.SetActive(false);
        }

        if(other.tag == "Enemy" || other.tag == "enemyBullet")
        {
            TakeDamage(10);
        }

        if(other.tag == "key")
        {
            keyCount += 1;
            Destroy(other.gameObject);
        }

       
       
    }


}
