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
    public TextMeshProUGUI gamePanelErorrText;
    public TextMeshProUGUI playerOneName;
    public TextMeshProUGUI playerTwoName;
    public TextMeshProUGUI playerOneWin;
    public TextMeshProUGUI playerTwoWin;


    [Header("EndPanel")]
    public Button playButEnd;
    public Button optionsButEnd;
    public Button exitButEnd;
    public TextMeshProUGUI matchEndStatus;
    public TextMeshProUGUI playerOneNameEnd;
    public TextMeshProUGUI playerTwoNameEnd;
    public TextMeshProUGUI playerOneWinEnd;
    public TextMeshProUGUI playerTwoWinEnd;

    [Header("EndPanelOnline")]
    public Button readyButEnd;
    public Button backButEnd;
    public TextMeshProUGUI matchOnlineEndStatus;
    public TextMeshProUGUI matchOnlineEndError;
    public TextMeshProUGUI playerOneNameEndOnline;
    public TextMeshProUGUI playerTwoNameEndOnline;
    public TextMeshProUGUI playerOneWinEndOnline;
    public TextMeshProUGUI playerTwoWinEndOnline;


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
        readyButEnd.onClick.AddListener(() => EndPanelOnlineReadyButPress());
        backButEnd.onClick.AddListener(() => EndPanelOnlineBackButPress());



        RefHolder.instance.audioController.SetAtStart();
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
        SetGamePanelTextAtStart("Player 1","Player 2","0","0");
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
        SetGamePanelTextAtStart("Player 1", "AI", "0", "0");
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
        playerOneWin.text = RefHolder.instance.gamePlay.player1Win.ToString();
        playerTwoWin.text = RefHolder.instance.gamePlay.player2Win.ToString();
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
        SceneManager.LoadScene("Game");
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


    public void SetEndPanelTextAtEnd(string player1, string player2, string player1W, string player2W)
    {
        playerOneNameEnd.text = player1;
        playerTwoNameEnd.text = player2;
        playerOneWinEnd.text = player1W;
        playerTwoWinEnd.text = player2W;
    }

    #endregion

    #region GamePanel

    public void SetPlayerText(string txt)
    {
        playerText.text = txt;
    }



    public void SetGamePanelTextAtStart(string player1,string player2,string player1W,string player2W)
    {
        playerOneName.text = player1;
        playerTwoName.text = player2;
        playerOneWin.text = player1W;
        playerTwoWin.text = player2W;
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
                if(nameInput.text == "")
                {
                    FirebaseController.instance.updateDesplayName("player");
                }
                else
                {
                    FirebaseController.instance.updateDesplayName(nameInput.text);
                }
                
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
        
        takeInput = false;
        if (RefHolder.instance.gamePlay.onlineMode)
        {
            animCon.EndPanelOnlineOut();
        }
        else
        {
            RefHolder.instance.gamePlay.onlineMode = true;
            animCon.MatchMakingFriendsPanelOut();
        }
        animCon.GamePanelIn();
        RefHolder.instance.gamePlay.StartGame();
    }






    public void MatchMakingFriendsBackButPress()
    {
        RefHolder.instance.dataManager.DeletePreviousRoomIfExists();
        SceneManager.LoadScene("Game");
    }

    #endregion



    #region EndPanelOnline



    public void EndPanelOnlineReadyButPress()
    {
        readyButEnd.interactable = false;
        gamePanelErorrText.text = "";
        matchOnlineEndError.text = "Waiting For Other Player to Be ready";
        RefHolder.instance.dataManager.setUserReady();
    }


    public void EndPanelOnlineBackButPress()
    {
        RefHolder.instance.dataManager.SetRoomID("");
        SceneManager.LoadScene("Game");
    }

    public void EndPanelTextSetUp(string txt)
    {
        matchOnlineEndStatus.text = txt;
        matchOnlineEndError.text = "Press Ready to Play Again";
    }



    public void SetEndPanelTextAtEndOnline(string player1, string player2, string player1W, string player2W)
    {
        playerOneNameEndOnline.text = player1;
        playerTwoNameEndOnline.text = player2;
        playerOneWinEndOnline.text = player1W;
        playerTwoWinEndOnline.text = player2W;
    }

    #endregion

}
