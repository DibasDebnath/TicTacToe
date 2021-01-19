using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICon : MonoBehaviour
{

    public UIAnimCon animCon;

    public bool takeInput;
    

    [Header("MainPanel")]
    public Button playBut;
    public Button optionsBut;
    public Button exitBut;

    [Header("GamePanel")]
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI timerText;


    [Header("EndPanel")]
    public Button playButEnd;
    public Button optionsButEnd;
    public Button exitButEnd;
    public TextMeshProUGUI matchEndStatus;


    [Header("Play Options")]
    public Button playLocalBut;
    public Button playOnlineBut;
    public Button playFriendsBut;
    public Button playAIBut;
    public Button playOptionsBack;


    [Header("Sign In Panel")]
    public Button signInGoogleBut;
    public Button signInAnonBut;
    public Button signbackBut;
    public TextMeshProUGUI errorTxt;


    private void Start()
    {
        exitBut.onClick.AddListener(() => ExitButPress());
        playBut.onClick.AddListener(() => PlayButPress());
        optionsBut.onClick.AddListener(() => OptionsButPress());
        exitButEnd.onClick.AddListener(() => ExitButPressEnd());
        playButEnd.onClick.AddListener(() => StartButPressEnd());
        optionsButEnd.onClick.AddListener(() => OptionsButPressEnd());
        playLocalBut.onClick.AddListener(() => PlayLocal());
        playOnlineBut.onClick.AddListener(() => OptionsButPressEnd());
        playFriendsBut.onClick.AddListener(() => PlayFriends());
        playAIBut.onClick.AddListener(() => PlayAI());
        playOptionsBack.onClick.AddListener(() => BackPlayOptions());
        signInGoogleBut.onClick.AddListener(() => SignInGoogleButPress());
        signInAnonBut.onClick.AddListener(() => SignInAnonButPress());
        signbackBut.onClick.AddListener(() => SignInBackButPress());



        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.GameMusic, true);
    }


    #region MainMenu
    private void PlayButPress()
    {
        if (!takeInput)
        {
            return;
        }
            
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
        animCon.MainMenuOut();
        animCon.PlayOptionsIn();


    }

    private void ExitButPress()
    {
        if (!takeInput)
        {
            return;
        }
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
        Application.Quit();
    }
    private void OptionsButPress()
    {
        if (!takeInput)
        {
            return;
        }
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
    }

    #endregion







    #region PlayOptionsPanel


    public void PlayLocal()
    {
        if (!takeInput)
        {
            return;
        }
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
        RefHolder.instance.gamePlay.AIMode = false;
        animCon.PlayOptionsOut();
        animCon.GamePanelIn();
        StartCoroutine(LateStartLocal());
    }

    public void PlayAI()
    {
        if (!takeInput)
        {
            return;
        }
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
        RefHolder.instance.gamePlay.AIMode = true;
        animCon.PlayOptionsOut();
        animCon.GamePanelIn();
        StartCoroutine(LateStartLocal());
    }


    IEnumerator LateStartLocal()
    {
        
        yield return new WaitForSeconds(0.2f);
        RefHolder.instance.gamePlay.StartGame();
    }



    public void PlayFriends()
    {
        if (!takeInput)
        {
            return;
        }
        if (FirebaseController.instance.user == null)
        {
            animCon.PlayOptionsOut();
            animCon.SignInIn();
        }
        else
        {

        }
    }


    public void BackPlayOptions()
    {
        if (!takeInput)
        {
            return;
        }
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);

        animCon.MainMenuIn();
        animCon.PlayOptionsOut();
    }

    #endregion





    #region EndScreen

    private void StartButPressEnd()
    {
        if (!takeInput)
        {
            return;
        }
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
        animCon.EndPanelOut();
        animCon.GamePanelIn();
        StartCoroutine(LateStartEnd());
    }
    IEnumerator LateStartEnd()
    {
        
        yield return new WaitForSeconds(0.2f);
        RefHolder.instance.gamePlay.StartGame();
    }
    private void ExitButPressEnd()
    {
        if (!takeInput)
        {
            return;
        }
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
        Application.Quit();
    }
    private void OptionsButPressEnd()
    {
        if (!takeInput)
        {
            return;
        }
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
    }

    public void SetMatchEndStatusText(string txt)
    {
        matchEndStatus.text = txt;
    }

    #endregion

    #region GamePanel

    public void SetPlayerText(string txt)
    {
        playerText.text = txt;
    }

    #endregion



    #region Sign In

    public void SignInGoogleButPress()
    {
        if (!takeInput)
        {
            return;
        }
        errorTxt.text = "connecting...";
        GoogleSignInDemo.instance.SignInWithGoogle();
        takeInput = false;
        StartCoroutine(lateSignInCheck());
    }

    IEnumerator lateSignInCheck()
    {
        yield return new WaitForSeconds(3f);
        if(FirebaseController.instance.user == null)
        {
            errorTxt.text = "Connection Error... Try Again";
        }
        else
        {
            RefHolder.instance.dataManager.SetUID(FirebaseController.instance.auth.CurrentUser.UserId);
            takeInput = true;
            animCon.PlayOptionsIn();
            animCon.SignInOut();
        }
    }

    public void SignInAnonButPress()
    {
        if (!takeInput)
        {
            return;
        }
        errorTxt.text = "connecting...";
        FirebaseController.instance.AnonSignIn();
        takeInput = false;
        StartCoroutine(lateSignInCheck());

    }

    


    public void SignInBackButPress()
    {
        if (!takeInput)
        {
            return;
        }
        animCon.PlayOptionsIn();
        animCon.SignInOut();
    }

    #endregion




}
