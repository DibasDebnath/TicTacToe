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
        playFriendsBut.onClick.AddListener(() => OptionsButPressEnd());
        playAIBut.onClick.AddListener(() => PlayAI());
        playOptionsBack.onClick.AddListener(() => BackPlayOptions());



        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.GameMusic, true);
    }


    #region MainMenu
    private void PlayButPress()
    {
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
        animCon.MainMenuOut();
        animCon.PlayOptionsIn();


    }

    private void ExitButPress()
    {
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
        Application.Quit();
    }
    private void OptionsButPress()
    {
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
    }

    #endregion







    #region PlayOptionsPanel


    public void PlayLocal()
    {
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
        RefHolder.instance.gamePlay.AIMode = false;
        animCon.PlayOptionsOut();
        animCon.GamePanelIn();
        StartCoroutine(LateStartLocal());
    }

    public void PlayAI()
    {
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

    public void BackPlayOptions()
    {
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);

        animCon.MainMenuIn();
        animCon.PlayOptionsOut();
    }

    #endregion












    #region EndScreen

    private void StartButPressEnd()
    {
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
        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
        Application.Quit();
    }
    private void OptionsButPressEnd()
    {
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


    





}
