using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    // Start is called before the first frame update

    public int coinscore;
  //  public Text cointext;
    public bool cointouched;

    public static Coins coinss;

    private float changecolor1, changecolor2, changecolor3;

    private Renderer playerrenderer;

    private Color newplayer;


    void Start()
    {
        //   cointext = GetComponent<Text>();
        playerrenderer =gameObject.GetComponent<Renderer>();

        changecolor1 = Random.Range(0f, 1f);
        changecolor2 = Random.Range(0f, 1f);
        changecolor3 = Random.Range(0f, 1f);
        Debug.Log(changecolor1);

        newplayer = new Color(changecolor1, changecolor2, changecolor3, 1f);
      //  playerrenderer.material.SetColor("_Color", newplayer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void addscore()
    {
        coinscore++;
        Debug.Log(coinscore);


    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<GeneratorPlatform>().increasescore();
            Debug.Log("destry coins");
          //  addscore();
            cointouched = true;
            //Debug.Log(coinscore);
            // cointext.text = coinscore.ToString();
            Destroy(gameObject);


        }

        if (collision.gameObject.tag == "Finish")
        {
            Destroy(gameObject);
           // Debug.Log("destry coin");
        }
    }
}
