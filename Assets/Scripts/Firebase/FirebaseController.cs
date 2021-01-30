using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseController : MonoBehaviour
{
    public static FirebaseController instance;

    public FirebaseAuth auth;
    private FirebaseApp app;
    public FirebaseDatabase database;
    public DatabaseReference reference;
    public FirebaseUser user;

    public bool isSignedIn;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);


        database = FirebaseDatabase.DefaultInstance;
        CheckFirebaseDependencies();


        //user = auth.CurrentUser;
        
        //auth.SignOut();

        //reference = database.RootReference;
        //database = FirebaseDatabase.DefaultInstance;
        //reference = database.RootReference;
        //Debug.Log(auth.CurrentUser.UserId);
    }
    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out from auth change " + user.UserId);
                isSignedIn = false;
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in from auth change " + user.UserId);
                isSignedIn = true;
            }
        }
    }

    void OnDestroy()
    {
        if(auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public string CreateUserWithEmail(string email, string password)
    {

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            user = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                user.DisplayName, user.UserId);


        });

        //Debug.Log(auth.CurrentUser);
        return null;
    }


    public void VarifyEmail()
    {
        if (user != null)
        {

            Debug.Log(user.Email);
            user.SendEmailVerificationAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendEmailVerificationAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Email sent successfully.");
            });
        }
    }

    public void SignInWithEnmail(string email, string password)
    {
        

    }


    public void SignInWithGoogle(string googleIdToken , string googleAccessToken)
    {
        //string uid = " ";

        Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                
                return;
            }

            user = task.Result;
            
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

            //auth.CurrentUser.
            
            
        });
        
        
    }

    public void updateDesplayName(string name)
    {
        user = auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = name,
                //PhotoUrl = new System.Uri("https://example.com/jane-q-user/profile.jpg"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }
                RefHolder.instance.uICon.errorUserPanel.text = "Update Complete";
                Debug.Log("User Display Name updated successfully. - ' "+ user.DisplayName+"'");
            });
        }
    }


    public void AnonSignIn()
    {
        
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                
                return;
            }
            
            user = task.Result;
            
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

            
        });

       
        //Debug.Log("as"+user.UserId);
        
    }





    private void CheckFirebaseDependencies()
    {
        
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            //database = FirebaseDatabase.DefaultInstance;
            
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
}
