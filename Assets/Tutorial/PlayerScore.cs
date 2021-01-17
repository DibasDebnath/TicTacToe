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
        create.onClick.AddListener(() => startListener());
        signIn.onClick.AddListener(() => stopListener());
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

    

    public void startListener()
    {
        FirebaseDataCon.instance.StartValueChangeListener();
    }
    public void stopListener()
    {
        FirebaseDataCon.instance.StopValueChangeListener();
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










    public void pushData()
    {

        List<string> list = new List<string>();

        list.Add("demo@gmail.com");
        list.Add("demo2@gmail.com");

        playerdata p = new playerdata("dibasdeb@gmail.com",null,0, list);

       

        //string j = JsonUtility.ToJson(p);

        //FirebaseDataCon.instance.pushData(j,null);


        string j = JsonUtility.ToJson(p);

        FirebaseDataCon.instance.pushUserData(j,null);

    }


    public void getData()
    {
        FirebaseDataCon.instance.GetUserData("dibasdebnath@gmail.com");
        
    }


}
public class playerdata {
    public string email;
    public string displayName;
    public int match;   
    public List<string> friends = new List<string>();


    public playerdata(string email,string displayName, int match, List<string> friends)
    {
        this.email = email;
        this.match = match;
        this.displayName = displayName;
        this.friends = friends;
        
        
    }
}


