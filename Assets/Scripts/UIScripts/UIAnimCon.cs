using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimCon : MonoBehaviour
{


    public Animator mainMenu; 
    public Animator gamePanel; 
    public Animator endPanel; 
    public Animator endPanelOnline; 
    public Animator playOptionsPanel; 
    public Animator signInPanel; 
    public Animator PlayFriendsPanel; 
    public Animator MatchMakingFriendsPanel; 
    public Animator UserPanel; 

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
    public void EndPanelOnlineIn()
    {
        endPanelOnline.SetTrigger("PanelIn");
    }
    public void EndPanelOnlineOut()
    {
        endPanelOnline.SetTrigger("PanelOut");
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
    public void PlayFriendsIn()
    {
        PlayFriendsPanel.SetTrigger("PanelIn");
    }
    public void PlayFriendsOut()
    {
        PlayFriendsPanel.SetTrigger("PanelOut");
    }
    public void MatchMakingFriendsPanelIn()
    {
        MatchMakingFriendsPanel.SetTrigger("PanelIn");
    }
    public void MatchMakingFriendsPanelOut()
    {
        MatchMakingFriendsPanel.SetTrigger("PanelOut");
    }
    public void UserPanelIn()
    {
        UserPanel.SetTrigger("PanelIn");
    }
    public void UserPanelOut()
    {
        UserPanel.SetTrigger("PanelOut");
    }
}
