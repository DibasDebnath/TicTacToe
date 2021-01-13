using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICon : MonoBehaviour
{

    public UIAnimCon animCon;



    public Button startBut;
    public Button optionsBut;
    public Button exitBut;

    public Text playerText;
    public Text timerText;

    private void Start()
    {
        exitBut.onClick.AddListener(() => ExitButPress());
        startBut.onClick.AddListener(() => StartButPress());
        optionsBut.onClick.AddListener(() => OptionsButPress());
    }




    private void StartButPress()
    {
        animCon.MainMenuOut();
        animCon.GamePanelIn();
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1f);
        RefHolder.instance.gamePlay.StartGame();
    }
    private void ExitButPress()
    {
        Application.Quit();
    }
    private void OptionsButPress()
    {

    }


    public void SetPlayerText(string txt)
    {
        playerText.text = txt;
    }


    public void StartCountDown(float time)
    {
        StartCoroutine(CoutDown(time));
    }

    IEnumerator CoutDown(float time)
    {

        while(time > 0)
        {
            yield return new WaitForSeconds(0.1f);
            time -= 0.1f;
            timerText.text = time.ToString("0.0");
        }
    }

}
