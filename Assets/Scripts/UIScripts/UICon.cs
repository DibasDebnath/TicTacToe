using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public InputField nameInput;


    [Header("Play Friends Panel")]
    public Button joinRoomBut;
    public Button createRoomBut;
    public Button playFriendsbackBut;
    public TextMeshProUGUI playFriendsErrorTxt;
    public InputField roomCodeInput;

    [Header("Match Making Friends Panel")]
    public Button readyBut;
    public Button matchMakingFriendsbackBut;
    public TextMeshProUGUI matchMakingFriendsErrorTxt;
    public TextMeshProUGUI matchMakingFriendsRoomCodeTxt;

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
        joinRoomBut.onClick.AddListener(() => JoinRoomButPress());
        createRoomBut.onClick.AddListener(() => CreateRoomButPress());
        playFriendsbackBut.onClick.AddListener(() => PlayFriendsBackBut());
        readyBut.onClick.AddListener(() => MatchMakingFriendsReadyButPress());
        matchMakingFriendsbackBut.onClick.AddListener(() => MatchMakingFriendsBackButPress());



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
        if (FirebaseController.instance.isSignedIn == false)
        {
            animCon.PlayOptionsOut();
            if(RefHolder.instance.dataManager.GetDisplayName() != "")
            {
                nameInput.text = RefHolder.instance.dataManager.GetDisplayName();
            }
            
            animCon.SignInIn();
        }
        else
        {
            animCon.PlayFriendsIn();
            animCon.PlayOptionsOut();
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
        //takeInput = false;
        StartCoroutine(lateSignInCheck());
    }

    IEnumerator lateSignInCheck()
    {
        yield return new WaitForSeconds(2f);
        if(FirebaseController.instance.isSignedIn == false)
        {
            takeInput = true;
            errorTxt.text = "Connection Error... Try Again";
        }
        else
        {
            
            takeInput = true;
            if (RefHolder.instance.dataManager.GetDisplayName() != nameInput.text)
            {
                FirebaseController.instance.updateDesplayName(nameInput.text);
            }
            RefHolder.instance.dataManager.UpdateUserData();
            //Debug.LogError("display name '"+ FirebaseController.instance.user.DisplayName + "'");
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



    #region PlayFriendsPanel




    public void CreateRoomButPress()
    {
        if (!takeInput)
        {
            return;
        }
        takeInput = false;
        playFriendsErrorTxt.text = "Connecting...";
        RefHolder.instance.dataManager.CreateRoom();
        StartCoroutine(LateCreateRoomCheck());
    }

    IEnumerator LateCreateRoomCheck()
    {
        yield return new WaitForSeconds(1f);
        if (RefHolder.instance.dataManager.roomCreated)
        {
            matchMakingFriendsRoomCodeTxt.text = "Created Room ID = '" + RefHolder.instance.dataManager.GetRoomID() + "'";
            matchMakingFriendsErrorTxt.text = "Waiting For player to Join";
            RefHolder.instance.dataManager.StartRoomValueChangeListener();
            animCon.MatchMakingFriendsPanelIn();
            animCon.PlayFriendsOut();
            takeInput = true;
        }
        else
        {
            playFriendsErrorTxt.text = "Connection Error Try Again";
            takeInput = true;
        }
        
    }

    public void JoinRoomButPress()
    {
        if (!takeInput)
        {
            return;
        }
        takeInput = false;
        playFriendsErrorTxt.text = "Connecting...";
        RefHolder.instance.dataManager.JoinRoom(roomCodeInput.text.ToLower());
        StartCoroutine(LateJoinRoomCheck());
    }



    IEnumerator LateJoinRoomCheck()
    {
        yield return new WaitForSeconds(1f);
        if (RefHolder.instance.dataManager.roomJoined)
        {
            matchMakingFriendsRoomCodeTxt.text = "Joined Room ID = '" + RefHolder.instance.dataManager.GetRoomID() + "'";
            matchMakingFriendsErrorTxt.text = "Press Ready";
            readyBut.interactable = true;
            RefHolder.instance.dataManager.StartRoomValueChangeListener();
            animCon.MatchMakingFriendsPanelIn();
            animCon.PlayFriendsOut();
            takeInput = true;
        }
        else
        {
            playFriendsErrorTxt.text = "Error Try Again";
            takeInput = true;
        }
    }






    public void PlayFriendsBackBut()
    {
        animCon.PlayFriendsOut();
        animCon.PlayOptionsIn();
    }

    #endregion



    #region MatchMaking Friends Panel

    public void MatchMakingFriendsReadyButPress()
    {
        matchMakingFriendsErrorTxt.text = "Waiting Players to be ready";
        RefHolder.instance.dataManager.setUserReady();
    }



    public void StartGameOnlineFriends()
    {
        RefHolder.instance.gamePlay.onlineMode = true;
        takeInput = false;
        animCon.MatchMakingFriendsPanelOut();
        animCon.GamePanelIn();
    }






    public void MatchMakingFriendsBackButPress()
    {
        RefHolder.instance.dataManager.DeletePreviousRoomIfExists();
        SceneManager.LoadScene("Game");
    }

    #endregion

}
