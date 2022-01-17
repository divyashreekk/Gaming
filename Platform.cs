using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject[] obstacles; //Objects that contains different obstacle types which will be randomly activated
    public GameObject[] coins;
    public GameObject coinprefab;

public void Start()

{
     
        // System.Random random = new System.Random();

        //startPoint = new Vector3()
    }
    public void ActivateRandomObstacle()
    {
        DeactivateAllObstacles();

        System.Random random = new System.Random();
        int randomNumber = random.Next(0, obstacles.Length);
      //  System.Random random = new System.Random();

        //int randomcoins = random.Next(0, coins.Length);
        //coins[randomcoins].SetActive(true);
        //obstacles[randomNumber].SetActive(true);

      // var position = new Vector3(Random.Range(2,2), Random.Range(2,4), Random.Range(-5, 5));
       //Instantiate(coinprefab, position, Quaternion.identity);
    }

        public void DeactivateAllObstacles()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].SetActive(false);
        }
    }
}