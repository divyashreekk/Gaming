using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using UnityEngine.UI;
using Firebase.Extensions;
public class Authentication : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    public InputField username_signup;
    public InputField email_id_signup;
    public InputField password_signup;
    public InputField confrmpassword_signup;
    public Text warningregistertext;


    public InputField email_signin;
    public InputField password_signin;
    public Text warninglogintext;
    public Text confrmlogintext;


    public GameObject registerpage;
    public GameObject loginpagee;

    public GameObject homepage;
    public GameObject gamepage;

    public GameObject canvas;

    public static Authentication authentication_ex;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        authentication_ex = this;
        auth = FirebaseAuth.DefaultInstance;

        /*   FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
           {
               dependencyStatus = task.Result;
               if (dependencyStatus == DependencyStatus.Available)
               {
                   InitializeFirebase();
                   Debug.Log("firbase");

               }
               else
               {
                   Debug.LogError("Could not resolve all firebase dependency:" + dependencyStatus);
               }
           });
        */
    }

    // Update is called once per frame
    void Update()
    {
     

    }
    void InitializeFirebase()
    {
        Debug.Log("Setting firebase");
        auth =FirebaseAuth.DefaultInstance;
      //  auth.StateChanged += AuthStateChanged;
       // AuthStateChanged(this, null);
    }

    public void Loginbtn()
    {
        StartCoroutine(Login(email_signin.text,password_signin.text));

    }
    public void Registerbtn()
    {
        StartCoroutine(Register(email_id_signup.text,password_signup.text,username_signup.text));
    }
    IEnumerator Login(string email, string password)
    {
        var tasklogin = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(predicate: () => tasklogin.IsCompleted);
        
        if(tasklogin.Exception!=null)
        {
            FirebaseException firebaseex = tasklogin.Exception.GetBaseException() as FirebaseException;
            AuthError errorcode = (AuthError)firebaseex.ErrorCode;

            string message = "Login Failed";
            switch(errorcode)
            {
                case AuthError.MissingEmail:
                    message = "Missing email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing password";
                    break;
                case AuthError.WrongPassword:
                    message = "wrong password ";
                    break;
                case AuthError.InvalidEmail:
                    message = "invalid email";
                    break;
                case AuthError.UserNotFound:
                    message = "user not found";
                    break;

            }
            warninglogintext.text = message;
        }
        else
        {
            user = tasklogin.Result;
            Debug.LogFormat("User succuessfully: {0}({1})", user.DisplayName, user.Email);
            warninglogintext.text = "";
            confrmlogintext.text = "Loggin";
            yield return new WaitForSeconds(2f);

            Application.LoadLevel("Options");
            canvas.SetActive(false);
          //  Debug.Log(user.DisplayName);
        }
        /*   auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
               if (task.IsCanceled)
               {
                   Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                   return;
               }
               if (task.IsFaulted)
               {
                   Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                   return;
               }

               Firebase.Auth.FirebaseUser newUser = task.Result;
               Debug.LogFormat("User signed in successfully: {0} ({1})",
                   newUser.DisplayName, newUser.UserId);
           });
        */
      
        //Application.LoadLevel("Game Scene 1");
    }
    IEnumerator Register(string email, string password, string username)
    {
        if (username == "")
        {
            warningregistertext.text = "Missing username";
        }
        else if (password_signup.text != confrmpassword_signup.text)
        {
            warningregistertext.text = "Password does not match";

        }
        else
        {
         //   var registerTask = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password);

            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to reg task with{registerTask.Exception}");
                FirebaseException firebase_ex = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorcode = (AuthError)firebase_ex.ErrorCode;

                string message = "Reg Failed";
                switch (errorcode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing password";
                        break;
                    case AuthError.WeakPassword:
                        message = "your password is weak, use at least 6 character to make strong ";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "EmailAlreadyInUse";
                        break;

                }
                warningregistertext.text = message;
            }
            else
            {
             user = registerTask.Result;

                if (user != null)
             {
                     UserProfile userprofile = new UserProfile {DisplayName = username };

                    var profile_link = user.UpdateUserProfileAsync(userprofile);
                    yield return new WaitUntil(predicate: () => profile_link.IsCompleted);

                    if (profile_link.Exception != null)
                    {
                        Debug.LogFormat("failed to reg task with {profilelink.Exception}");
                     FirebaseException firebase_ex1 = profile_link.Exception.GetBaseException() as FirebaseException;
                        AuthError errorcode = (AuthError)firebase_ex1.ErrorCode;
                        warningregistertext.text = "username set failed";

                    }
                    else
                    {
                        warningregistertext.text = "User Created Please login";
                        yield return new WaitForSeconds(2f);
                        registerpage.SetActive(false);
                        loginpagee.SetActive(true);
                        email_signin.text = "";
                        password_signin.text = "";
                        warninglogintext.text = "";
                        confrmlogintext.text = "";
                        
                      // warningregistertext.text = "User Created Please login";

                    }
        
            }
            }
        }
    }
    public void forgotpassword()
    {
      //  confrmlogintext.text = "Send a reset password link to your register email";

        auth.SendPasswordResetEmailAsync(email_id_signup.text).ContinueWith(task =>
        {

            if (task.IsCompleted)
            {
                Debug.Log("send email");
                confrmlogintext.text = "Send a reset password link to your register email";

            }
        }) ;

    }
    public void log()
    {
        Application.LoadLevel("GameScene 1");
        canvas.SetActive(false);

    }
    public void signup()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email_id_signup.text, password_signup.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("cancel");
                return;
            }
            if(task.IsFaulted)
            {
                Debug.LogError("fault");
                return;
            }
            FirebaseUser newfirebase = task.Result;
            Debug.LogFormat("create {0} ({1})", newfirebase.DisplayName, newfirebase.UserId);
        });
    }
    public void createacc()
    {
        registerpage.SetActive(true);
        loginpagee.SetActive(false);
    }

}
