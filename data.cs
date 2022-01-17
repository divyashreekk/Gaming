using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;


public class data : MonoBehaviour
{
    DatabaseReference Reference;
    public InputField Username;
    public InputField Email;
    public InputField nametoread;



    // Start is called before the first frame update
    void Start()
    {
      //  DontDestroyOnLoad(this);

        Reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void savedata()
    {
        User user = new User();
        user.Username = Username.text;
        user.Email = Email.text;
        user.Score = GeneratorPlatform.instance.coinscore.ToString();
        string json = JsonUtility.ToJson(user);

        Reference.Child("User").Child(user.Username).SetRawJsonValueAsync(json).ContinueWith (task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("success");
            }
        });
     //   Application.LoadLevel("GameScene 1");

    }

    public void read_Data()
    {

        Reference.Child("User").Child(nametoread.text).GetValueAsync().ContinueWith(task=>
        {
            //  Debug.Log("not ");

            if (task.IsCompleted)
            {
                Debug.Log("success");
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Child("username").Value.ToString());
                Debug.Log(snapshot.Child("email").Value.ToString());
            }
            else
            {
                Debug.Log("not scu");
            }
        });
       

        
    }
    // Update is called once per frame

    void Update()
    {
    }
}
