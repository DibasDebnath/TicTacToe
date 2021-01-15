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


    public Button submit;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI emailTxt;
    public TextMeshProUGUI passwordTxt;
    public TextMeshProUGUI ConPasswordTxt;
    public TextMeshProUGUI ErrorTxt;

    private void Awake()
    {
        submit.onClick.AddListener(() => SubmitPress());
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

        SignInAnon();



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
