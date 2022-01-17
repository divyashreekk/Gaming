using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagment;

[RequireComponent(typeof(Rigidbody))]

public class Character : MonoBehaviour
{
    public float gravity = 20.0f;
    public float jumpHeight = 2.5f;

    Rigidbody r;
    bool grounded = false;
    Vector3 defaultScale;
    bool crouch = false;

public GameObject Gameover_panel;
	public Text scoretext;
	public GeneratorPlatform generatorplatform_ex;
    public GameObject[] prefab;

    public int highscore;

    public GameObject homepage;
    public GameObject gamepage;

    private bool homepageactive;


    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    public Animator player;
    private Transform startt;
    private float changecolor1, changecolor2, changecolor3;

  
    
        // Start is called before the first frame update
        void Start()
    {

     
        Time.timeScale = 1f;

        generatorplatform_ex = GetComponent <GeneratorPlatform>();

		
        r = GetComponent<Rigidbody>();
        r.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        r.freezeRotation = true;
        r.useGravity = false;
        defaultScale = transform.localScale;
	
        scoretext = GetComponent<Text>();
        //float.Parse(scoretext.text)= generatorplatform_ex.score;
        //Debug.Log(generatorplatform_ex.score);

       // highscore =Mathf.FloorToInt( generatorplatform_ex.score);
        if(PlayerPrefs.HasKey("High Score"))
        {
          //  PlayerPrefs.GetFloat("High Score", generatorplatform_ex.score);
        }
        else
        {
            PlayerPrefs.SetFloat("High Score", 0);
        }
     //   player = GetComponent<Animator>();

        // Debug.Log(generatorplatform_ex.score);

    }

    void Update()
    {
        if(transform.localPosition.y<-25)
        {

            GeneratorPlatform.instance.gameStarted = false;

            Gameover_panel.SetActive(true);
            Time.timeScale = 0f;
        }
      //  transform.Translate(Vector3.forward * Time.deltaTime * 2f, Space.World);

        //  transform.Translate(transform.forward * Time.deltaTime, Space.World);
        //startt.position = transform.localPosition;

        if(Input.GetMouseButton(0))
        {
           // Debug.Log("click");
            if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(defaultScale.x, defaultScale.y * 0.4f, defaultScale.z), Time.deltaTime);

              //  Debug.Log("clickq");

            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime);
                crouch = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                crouch = true;
                  Debug.Log(crouch);

                if (crouch)
                {
                    //   player.SetBool("down", true);

              //      transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(defaultScale.x, defaultScale.y * 0.4f, defaultScale.z), Time.deltaTime);
                }
                else
                {
               //     transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime);
               //     crouch = false;
                }
                Debug.Log("down swipe");
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();

            //swipe upwards
            if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                r.velocity = new Vector3(r.velocity.x, CalculateJumpVerticalSpeed(), r.velocity.z);

                Debug.Log("up swipe");

            }
            //swipe down
            if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
             //   crouch = true;
                if (crouch)
                {
                 //   player.SetBool("down", true);

           //         transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(defaultScale.x, defaultScale.y * 0.4f, defaultScale.z), Time.deltaTime);
                }
                else
                {
             //       transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime);
             //       crouch = false;
                }
                Debug.Log("down swipe");
            }
            //swipe left
            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (transform.position.x <= 0f)
                {
                 //   Debug.Log("Left");
                    transform.localPosition = new Vector3(-2f, -1.77f, 0);
                }
                else if (transform.position.x > 0f)
                {
                    transform.localPosition = new Vector3(0f, -1.77f, 0);
                }
                Debug.Log("left swipe");
            }
            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (transform.position.x >= 0f)

                {
                    transform.localPosition = new Vector3(2f, -1.77f,0);

                }
                else
                {

                    Debug.Log("Right");
                    transform.localPosition = new Vector3(0f, -1.77f,0 );

                }
                Debug.Log("right swipe");
            }
        }


        //Debug.Log(generatorplatform_ex.score);
        // Jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            r.velocity = new Vector3(r.velocity.x, CalculateJumpVerticalSpeed(), r.velocity.z);
        }

        if (Input.GetKeyDown(KeyCode.A) )
        {
		if(transform.position.x<=0f)
            {
            Debug.Log("Left");
            transform.localPosition= new Vector3(-2f, -1.77f, 0f);
            }
            else if(transform.position.x>0f)
            {
            transform.localPosition= new Vector3(0f, -1.77f, 0f);

            }

        }

                    if (Input.GetKeyDown(KeyCode.D))
                    {
                if(transform.position.x>=0f)

            {
            transform.localPosition= new Vector3(2f, -1.77f, 0f);

            }
            else{

            Debug.Log("Right");
            transform.localPosition= new Vector3(0f, -1.77f, 0f);

            }
                    }


        //Crouch
        crouch = Input.GetKey(KeyCode.S);
      //  Debug.Log(crouch);
        if (crouch)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(defaultScale.x, defaultScale.y * 0.4f, defaultScale.z), Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // We apply gravity manually for more tuning control
        r.AddForce(new Vector3(0, -gravity * r.mass, 0));

        grounded = false;
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

	public void retry()
	{
        //   if(homepageactive==true)
        //   {
      //  homepage.SetActive(false);

      //  gamepage.SetActive(true);
        Application.LoadLevel("GameScene 1");
       

    //    Gameover_panel.SetActive(false);
    //   Time.timeScale = 1f;
     //   GeneratorPlatform.instance.gameStarted = true;
    //    GeneratorPlatform.instance.Start();
        //   }

    }
    public void play()
    {
        //  Application.LoadLevel("");
        gamepage.SetActive(true);
        homepage.SetActive(false);
      //  homepageactive = true;
     //   Debug.Log(homepageactive);



    }
    public void Curvedpath()
	{
            Application.LoadLevel("GameScene 1");

    }
    public void straightpath()
    {
        Application.LoadLevel("GameScene 2");

    }

    public void gotp_options()
    {
        Application.LoadLevel("Options");
    }
    public void gotohome()
    {
        Application.LoadLevel("Login");
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Finish")
        {
            GeneratorPlatform.instance.gameStarted = false;

            Gameover_panel.SetActive(true);
            Time.timeScale = 0f;
            //     PlayerPrefs.SetFloat("High Score", generatorplatform_ex.score);
        //    Debug.Log(generatorplatform_ex.score);
            //scoretext.text = GeneratorPlatform.instance.score.ToString();



            //print("GameOver!");
            //GeneratorPlatform.instance.gameOver = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            GeneratorPlatform.instance.gameStarted = false;

            Gameover_panel.SetActive(true);
           // Time.timeScale = 0f;

            //     PlayerPrefs.SetFloat("High Score", generatorplatform_ex.score);
            //    Debug.Log(generatorplatform_ex.score);
         //   GeneratorPlatform.instance.gameStarted = false;
            //scoretext.text = GeneratorPlatform.instance.score.ToString();



            //print("GameOver!");
            //GeneratorPlatform.instance.gameOver = true;
        }
        if (collision.gameObject.tag == "Coins")
        {
            Debug.Log("destry coins");
           // GeneratorPlatform.instance.cointouched = false;

            // addscore();
            //  cointouched = true;
            //  Debug.Log(coinscore);
            // cointext.text = coinscore.ToString();
            // Destroy(other.gameobject);


        }

        /*   if(collision.gameObject.tag == "Coins")
           {
               Debug.Log("collect coins");
               for (int i = 0;i< prefab.Length; i++)
                   {
                   prefab[i].SetActive(false);
               }
           }*/
    }
}