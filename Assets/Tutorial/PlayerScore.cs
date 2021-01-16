using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
public class PlayerScore : MonoBehaviour
{

    //public TextMeshProUGUI ScoreText;


    public int score;
    public string playerName;
    public string email;
    public string password;
    public string ConPassword;


    public Button create;
    public Button signIn;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI emailTxt;
    public TextMeshProUGUI passwordTxt;
    public TextMeshProUGUI ConPasswordTxt;
    public TextMeshProUGUI ErrorTxt;

    private void Awake()
    {
        create.onClick.AddListener(() => SubmitPress());
        signIn.onClick.AddListener(() => SignInAnon());
    }

    // Start is called before the first frame update
    void Start()
    {
        //score = Random.Range(0, 100);
        //ScoreText.text = score.ToString("0");
    }


    public void SubmitPress()
    {
        playerName = nameTxt.text;
        email = emailTxt.text;
        password = passwordTxt.text;
        ConPassword = ConPasswordTxt.text;

        if(password == ConPassword)
        {
            //CreateNewUserWithEmail();
        }

        //SignInAnon();

        FirebaseController.instance.updateDesplayName(playerName);

    }

    public void SignInWithGoogle()
    {
        GoogleSignInDemo.instance.SignInWithGoogle();
    }

    public void Varify()
    {
        FirebaseController.instance.VarifyEmail();
    }
    // Update is called once per frame
    void Update()
    {
        
    }



    public void SignInWithEmail()
    {
        
    }
    public void SignInAnon()
    {
        FirebaseController.instance.AnonSignIn();
    }

    public void CreateNewUserWithEmail()
    {
        FirebaseController.instance.CreateUserWithEmail(email,password);
    }
}