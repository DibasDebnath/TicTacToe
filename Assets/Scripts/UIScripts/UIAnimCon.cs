using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimCon : MonoBehaviour
{


    public Animator mainMenu; 
    public Animator gamePanel; 

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
}
