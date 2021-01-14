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
    public Button startBut;
    public Button optionsBut;
    public Button exitBut;

    [Header("GamePanel")]
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI timerText;


    [Header("EndPanel")]
    public Button startButEnd;
    public Button optionsButEnd;
    public Button exitButEnd;
    public TextMeshProUGUI matchEndStatus;

    private void Start()
    {
        exitBut.onClick.AddListener(() => ExitButPress());
        startBut.onClick.AddListener(() => StartButPress());
        optionsBut.onClick.AddListener(() => OptionsButPress());
        exitButEnd.onClick.AddListener(() => ExitButPressEnd());
        startButEnd.onClick.AddListener(() => StartButPressEnd());
        optionsButEnd.onClick.AddListener(() => OptionsButPressEnd());



        RefHolder.instance.audioController.Play(RefHolder.instance.audioController.GameMusic, true);
    }




    private void StartButPress()
    {
        animCon.MainMenuOut();
        animCon.GamePanelIn();
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.2f);
        RefHolder.instance.gamePlay.StartGame();
    }
    private void ExitButPress()
    {
        Application.Quit();
    }
    private void OptionsButPress()
    {

    }

    private void StartButPressEnd()
    {
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
        Application.Quit();
    }
    private void OptionsButPressEnd()
    {

    }


    public void SetPlayerText(string txt)
    {
        playerText.text = txt;
    }


    

    public void SetMatchEndStatusText(string txt)
    {
        matchEndStatus.text = txt;
    }

}
