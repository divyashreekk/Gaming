using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class GeneratorPlatform : MonoBehaviour
{
    public Camera mainCamera;
    public Transform startPoint; //Point from where ground tiles will start
                                 public Platform tilePrefab;
                                 //public <Platform> tilePrefab1 = new List<Platform>();
   // public GameObject[] tilePrefab;
    public float movingSpeed = 12;
    public int tilesToPreSpawn = 15; //How many tiles should be pre-spawned
    public int tilesWithoutObstacles = 3; //How many tiles at the beginning should not have obstacles, good for warm-up

    List<Platform> spawnedTiles = new List<Platform>();
    int nextTileToActivate = -1;
    [HideInInspector]
    public bool gameOver = false;
    public bool gameStarted = true;
    public static float score = 0;

    public Text starttext;
	
	public Text Displayscoretext;
    public Text scoretext;

    public static float highscore;
    public Text highscoretextdisplay;
    public static GeneratorPlatform instance;

    public Text coinscoretext;
    public Text coindisplaytext;
    public int coinscore;


    public Obstacles obstacles_ex;

    private Coins coins_ex;

    public bool cointouched;

    private data data_ex;

    DatabaseReference Reference;
    public InputField Username;
    public InputField Email;

    private int high_score1;
    //private int old_score1;

    public Text highscoredisplay;


    public GameObject other;

    private ChangeTex changeTex;
    // Start is called before the first frame update
   public  void Start()
    {
       
       // PlayerPrefs.DeleteAll();
        // data_ex = GetComponent<data>();
        data_ex = FindObjectOfType<data>();

      //  Username.text = data_ex.Username.text;
      //  Debug.Log(Username.text);
        Reference = FirebaseDatabase.DefaultInstance.RootReference;

        obstacles_ex = GetComponent<Obstacles>();
       coins_ex = GetComponent<Coins>();
     //   coinscoretext = GetComponent<Text>();
        score = 0;
       // Debug.Log(highscore);
        Debug.Log(score);
        if (PlayerPrefs.HasKey("HighScore"))
        {
            //  Debug.Log("key");
            // if (score > highscore)
            //{
            //  highscore = PlayerPrefs.GetFloat("HighScore");
            //   Debug.Log(highscore);

            //}
            highscore = PlayerPrefs.GetFloat("HighScore");
            Debug.Log(highscore);


        }
        instance = this;

        highscoredisplay.text = highscore.ToString();
        high_score1 = (int)highscore;


//Debug.Log(startPoint.position);
//Displayscoretext= GetComponent<Text>();

Vector3 spawnPosition = startPoint.position;
        int tilesWithNoObstaclesTmp = tilesWithoutObstacles;
        for (int i = 0; i < tilesToPreSpawn; i++)
        {
            spawnPosition -= tilePrefab.startPoint.localPosition;
            Platform spawnedTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity) as Platform;
           // obstacles_ex.SpawnObstacles();

            if(tilesWithNoObstaclesTmp > 0)
            {
                spawnedTile.DeactivateAllObstacles();
                tilesWithNoObstaclesTmp--;
            }
            else
            {
                spawnedTile.ActivateRandomObstacle();
            }
            
            spawnPosition = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            spawnedTiles.Add(spawnedTile);
        }
    }
    public void savedata()
    {
        User user = new User();
        //   user.username = data_ex.Username.text;
        //  user.email = data_ex.Email.text;
        user.Email = Authentication.authentication_ex.email_signin.text;
        user.Username = Authentication.authentication_ex.user.DisplayName;
     //   Debug.Log(Authentication.authentication_ex.user.DisplayName);

        //    user.Email = Email.text;
  
        //  Debug.Log(score);
        user.Score = score.ToString();
      //  if(highscore>high_score1)
      //  {
            user.Old_score = high_score1.ToString();

     //   }
        user.Highscore = highscore.ToString();
        user.Coins = coinscore.ToString();
        string json = JsonUtility.ToJson(user);

        Reference.Child("User").Child(user.Username).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("success");
            }
        });
        //   Application.LoadLevel("GameScene 1");

    }

    // Update is called once per frame
    void Update()
    {
        if(cointouched==true)
        {
            coinscore++;
            Debug.Log(coinscore);
        }

       // highscore = score;
        // Move the object upward in world space x unit/second.
        //Increase speed the higher score we get
        if (gameStarted==true)
        {
          

           // starttext.text = "";
           
            transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * (movingSpeed + (score/500)), Space.World);
            score += Time.deltaTime * movingSpeed;

            // if(coins_ex.cointouched==true)

            //  {
            
             //   Debug.Log((int)score);
                Displayscoretext.text=("Score: "
                +(int)score).ToString();

            coindisplaytext.text = "Coins:" + coinscore.ToString();

        }


        if (mainCamera.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {
            //Move the tile to the front if it's behind the Camera
            Platform tileTmp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTmp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            tileTmp.ActivateRandomObstacle();
            spawnedTiles.Add(tileTmp);
        }

        if (gameStarted==false)
        {
          // savedata();


            // Time.timeScale = 0f;

            // highscoretextdisplay.text = ("High Score:" +(int) highscore).ToString();
            scoretext.text = Displayscoretext.text;
            coinscoretext.text = "Coins:"+coinscore.ToString();
            highscoretextdisplay.text = ("High Score: " + (int)highscore).ToString();
            //Debug.Log(scoretext);

            /*    if (score > highscore)
                {
                    PlayerPrefs.SetFloat("HighScore", score);
                  //  float d = PlayerPrefs.GetFloat("HighScore");

                 //   Debug.Log(d);
                }
                else if(highscore>score)
                {
                    PlayerPrefs.SetFloat("HighScore", score);

                }
                else
                {
                    PlayerPrefs.SetFloat("HighScore", highscore);

                }
            */

           


           // high_score1 =(int) score;
            Debug.Log(highscore);
            //   highscore = PlayerPrefs.GetFloat("HighScore");
            PlayerPrefs.Save();
            //Debug.Log(highscore);
            //Debug.Log(score);
            if (highscore > high_score1)
            {
                 highscore = (int)PlayerPrefs.GetFloat("HighScore",highscore);

                Debug.Log(high_score1);
                // PlayerPrefs.SetFloat("HighScore", score);

            }

            if (score > highscore)
            {
                PlayerPrefs.SetFloat("HighScore", score);

            }
            highscore = PlayerPrefs.GetFloat("HighScore");


            savedata();
         //   other.GetComponent<data>().savedata();

            // if(score>highscore)
            //{
            // Debug.Log(score);
            //   Debug.Log(highscore);
            //PlayerPrefs.SetFloat("HighScore", score);
            // PlayerPrefs.Save();
            //float scr = PlayerPrefs.GetFloat("HighScore");
            //Debug.Log(scr);
            //highscore = scr;


            // Debug.Log(highscore);

            //}
            // else
            //{
            //  PlayerPrefs.SetFloat("HighScore", highscore);

            //}

            // if (Input.GetKeyDown(KeyCode.Space))
            //   {
            if (gameOver)
                {
               // Time.timeScale = 0f;
                   
                    //Restart current scene
                 //   Scene scene = SceneManager.GetActiveScene();
                 //   SceneManager.LoadScene(scene.name);
                }
                else
                {
          //      Time.timeScale = 1f;

                //Start the game
           //     gameStarted = true;
                }
         //   }
        }
    }
    public void increasescore()
    {
        coinscore++;
        coinscoretext.text = "Coins:" + coinscore.ToString();
     //   Debug.Log(coinscore);
    }

   /* void OnGUI()
    {
        if (gameOver)
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "Game Over\nYour score is: " + ((int)score) + "\nPress 'Space' to restart");
        }
        else
        {
            if (!gameStarted)
            {
                GUI.color = Color.red;
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "Press 'Space' to start");
            }
        }


        GUI.color = Color.green;
        GUI.Label(new Rect(5, 5, 200, 25), "Score: " + ((int)score));
    }
   */

   
}