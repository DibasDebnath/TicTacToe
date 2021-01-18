using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimCon : MonoBehaviour
{


    public Animator mainMenu; 
    public Animator gamePanel; 
    public Animator endPanel; 
    public Animator playOptionsPanel; 
    public Animator signInPanel; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MainMenuIn()
    {
        mainMenu.SetTrigger("MenuIn");
    }
    public void MainMenuOut()
    {
        mainMenu.SetTrigger("MenuOut");
    }
    public void GamePanelIn()
    {
        gamePanel.SetTrigger("GamePanelIn");
    }
    public void GamePanelOut()
    {
        gamePanel.SetTrigger("GamePanelOut");
    }
    public void EndPanelIn()
    {
        endPanel.SetTrigger("PanelIn");
    }
    public void EndPanelOut()
    {
        endPanel.SetTrigger("PanelOut");
    }
    public void PlayOptionsIn()
    {
        playOptionsPanel.SetTrigger("PanelIn");
    }
    public void PlayOptionsOut()
    {
        playOptionsPanel.SetTrigger("PanelOut");
    }
    public void SignInIn()
    {
        signInPanel.SetTrigger("PanelIn");
    }
    public void SignInOut()
    {
        signInPanel.SetTrigger("PanelOut");
    }
}
