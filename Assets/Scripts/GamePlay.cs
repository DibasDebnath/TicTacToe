using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GamePlay : MonoBehaviour
{

    [SerializeField]
    private List<Transform> buttonTransforms;

    public bool AIMode;
    public bool onlineMode;

    private List<Transform> winningButtonTransforms = new List<Transform>();
    
    public bool isMatchEnd = false;
    private Coroutine CountDownCor;
    private float coutdownTimer;
    public float TimerValue;

    private Button[,] buttonArray = new Button[3,3];
    private GameObject[,] OArray = new GameObject[3,3];
    private GameObject[,] XArray = new GameObject[3,3];

    public int[,] board = new int[3,3]; // 0 - Empty , 1 - O , 2 - X

    public int currentPlayer;
    public int onlinePlayer;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("random string = " + CreateRandomString());
        if (buttonTransforms[0] == null)
        {
            Debug.LogError("Set Buttons");
        }

        InitializeGame();
        AddListeners();

    } 


    public void StartGame()
    {
       
        ResetAll();
        if (AIMode && currentPlayer == 2 && onlineMode == false)
        {
            AIInput();
        }
        else if (AIMode && currentPlayer == 1 && onlineMode == false)
        {
            RefHolder.instance.uICon.takeInput = true;
        }
        else if (AIMode == false  && onlineMode == true)
        {
            if(onlinePlayer == currentPlayer)
            {
                RefHolder.instance.uICon.takeInput = true;
            }
            else
            {
                // Wait for opponents Turn
            }
        }
        StartCountDown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void ResetAll()
    {
        //Setting Empty Board
        
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                board[j, k] = 0;
                buttonArray[j, k].interactable = true;
                OArray[j, k].SetActive(false);
                XArray[j, k].SetActive(false);
                
            }
        }
        if (onlineMode)
        {
            string tmp = RefHolder.instance.dataManager.oldDataSnapshot.Child(RefHolder.instance.dataManager.CURRENTPLAYER).Value.ToString();
            currentPlayer = int.Parse(tmp);
        }
        else
        {
            currentPlayer = Random.Range(1, 3);
        }
        
        if (AIMode == true && currentPlayer == 2 && onlineMode == false)
        {
            RefHolder.instance.uICon.playerText.text = "AI Turn";
        }
        else if (AIMode == true && currentPlayer == 1 && onlineMode == false)
        {
            RefHolder.instance.uICon.playerText.text = "Your Turn";
        }
        else if(AIMode == true && onlineMode == false)
        {
            RefHolder.instance.uICon.playerText.text = "Player " + currentPlayer;
        }
        else if (AIMode == false && onlineMode == true)
        {
            if(currentPlayer == onlinePlayer)
            {
                RefHolder.instance.uICon.playerText.text = "Your Turn";
            }
            else
            {
                RefHolder.instance.uICon.playerText.text = "Opponents Turn";
            }
            
        }
        
        isMatchEnd = false;
        coutdownTimer = 0;
    }



    private void InitializeGame()
    {
        //Setting Buttons at start
        int i = 0;
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                buttonArray[j, k] = buttonTransforms[i++].GetComponent<Button>();
            }
        }

        //Setting O at start
        i = 0;
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                OArray[j, k] = buttonTransforms[i++].GetChild(0).gameObject;
            }
        }
        //Setting X at start
        i = 0;
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                XArray[j, k] = buttonTransforms[i++].GetChild(1).gameObject;
            }
        }
        
    }

    private void AddListeners()
    {
        
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                int j_copy = j;
                int k_copy = k;
                buttonArray[j_copy, k_copy].onClick.AddListener(() => ButtonClick(j_copy, k_copy));
            }
        }
        
    }

    public void ButtonClick(int j, int k)
    {
        Debug.Log("ButtonCLicked " + j + " " + k);
        //RefHolder.instance.playerInput.buttonClick(j, k);
        if (RefHolder.instance.uICon.takeInput)
        {
            RefHolder.instance.uICon.takeInput = false;
            if (board[j, k] == 0)
            {
                RefHolder.instance.audioController.Play(RefHolder.instance.audioController.Tap, false);
                board[j, k] = currentPlayer;               
                buttonArray[j, k].interactable = false;
                switch (currentPlayer)
                {
                    case 1:
                        OArray[j, k].SetActive(true);
                        WinCheck();
                        if (!isMatchEnd)
                        {
                            DrawCheck();
                        }
                        if (!isMatchEnd)
                        {
                            SwithPlayer(j,k);
                        }
                        
                        break;
                    case 2:
                        XArray[j, k].SetActive(true);
                        WinCheck();
                        if (!isMatchEnd)
                        {
                            DrawCheck();
                        }
                        if (!isMatchEnd)
                        {
                            SwithPlayer(j,k);
                        }
                        break;
                    default:
                        break;

                }

            }
        }
        
        
        

    }



    private void WinCheck()
    {
        int tmp = 0;
        winningButtonTransforms.Clear();
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                if(board[j,k] == currentPlayer)
                {
                    tmp++;
                    winningButtonTransforms.Add(buttonArray[j, k].transform);
                    if (tmp == 3)
                    {
                        Debug.Log("Player " + currentPlayer + " Wins");
                        EndMatch(true);
                    }
                }
                else
                {
                    winningButtonTransforms.Clear();
                }
            }
            tmp = 0;
        }
        for (int k = 0; k < 3; k++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[j, k] == currentPlayer)
                {
                    tmp++;
                    winningButtonTransforms.Add(buttonArray[j, k].transform);
                    if (tmp == 3)
                    {
                        Debug.Log("Player " + currentPlayer + " Wins");
                        EndMatch(true);
                    }
                }
                else
                {
                    winningButtonTransforms.Clear();
                }
            }
            tmp = 0;
        }
        for (int i = 0; i < 3; i++)
        {
            if (board[i, i] == currentPlayer)
            {
                tmp++;
                winningButtonTransforms.Add(buttonArray[i,i].transform);
                if (tmp == 3)
                {
                    Debug.Log("Player " + currentPlayer + " Wins");
                    EndMatch(true);
                }
            }
            else
            {
                winningButtonTransforms.Clear();
            }
            
        }
        tmp = 0;

        for (int k = 0; k < 3; k++)
        {
            if (board[2-k, k] == currentPlayer)
            {
                tmp++;
                winningButtonTransforms.Add(buttonArray[2 - k, k].transform);
                if (tmp == 3)
                {
                    Debug.Log("Player " + currentPlayer + " Wins");
                    EndMatch(true);
                }
            }
            else
            {
                winningButtonTransforms.Clear();
            }
        }


    }


    private void DrawCheck()
    {

        bool check = true;
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                if (board[j, k] == 0)
                {
                    check = false;
                }
            }
        }
        if (check == true)
        {

            Debug.Log("Match Draw");
            EndMatch(false);
        }


    }

    private void SwithPlayer(int j,int k)
    {
        
        if (currentPlayer == 1)
        {
            currentPlayer = 2;
            if (!onlineMode)
            {
                if (AIMode)
                {
                    RefHolder.instance.uICon.playerText.text = "AI Turn";
                }
                else 
                {
                    RefHolder.instance.uICon.playerText.text = "Player " + currentPlayer;
                }
               
            }
            else
            {
                if (currentPlayer != onlinePlayer)
                {
                    RefHolder.instance.uICon.playerText.text = "Your Turn";
                    RefHolder.instance.uICon.takeInput = true;
                }
                else
                {
                    RefHolder.instance.uICon.playerText.text = "Opponents Turn";
                    RefHolder.instance.uICon.takeInput = false;
                }
            }
            
            //Debug.Log("CurrentPlayer " + currentPlayer);

            StartCountDown();


            if (AIMode)
            {
                // call ai here;
                AIInput();
            }
            if (onlineMode)
            {
                RefHolder.instance.dataManager.onlineInput(j, k);
            }

        }
        else
        {
            currentPlayer = 1;
            if (!onlineMode)
            {
                if (AIMode)
                {
                    RefHolder.instance.uICon.playerText.text = "Your Turn";
                }
                else if (!AIMode)
                {
                    RefHolder.instance.uICon.playerText.text = "Player " + currentPlayer;
                }
            }
            else
            {
                if (currentPlayer != onlinePlayer)
                {
                    RefHolder.instance.uICon.playerText.text = "Your Turn";
                    RefHolder.instance.uICon.takeInput = true;
                }
                else
                {
                    RefHolder.instance.uICon.playerText.text = "Opponents Turn";
                    RefHolder.instance.uICon.takeInput = false;
                }
            }

           
            //Debug.Log("CurrentPlayer " + currentPlayer);

            StartCountDown();
            if (onlineMode)
            {
                RefHolder.instance.dataManager.onlineInput(j, k);
            }

        }
    }

    private void EndMatch(bool endStatus)
    {
        isMatchEnd = true;
        coutdownTimer = 0f;
        //Debug.LogError("asdad");
            StopCoroutine(CountDownCor);
            CountDownCor = null;
        RefHolder.instance.dataManager.ResetBothReady();
        //win
        if (endStatus)
        {
            if (!onlineMode)
            {
                if (AIMode && currentPlayer == 2)
                {
                    RefHolder.instance.uICon.SetMatchEndStatusText("AI Won");
                }
                else if (AIMode && currentPlayer == 1)
                {
                    RefHolder.instance.uICon.SetMatchEndStatusText("You Won");
                }
                else
                {
                    RefHolder.instance.uICon.SetMatchEndStatusText("Player " + currentPlayer + " Won");
                }
            }
            else
            {
                if (onlinePlayer == currentPlayer)
                {
                    RefHolder.instance.uICon.EndPanelTextSetUp("You Win");
                }
                else
                {
                    RefHolder.instance.uICon.EndPanelTextSetUp("Opposition Won");
                }
            }
           
            StartCoroutine(WinAnimation());
        }
        else // draw
        {
            if (!onlineMode)
            {
                RefHolder.instance.uICon.SetMatchEndStatusText("Match Draw");
                RefHolder.instance.uICon.animCon.GamePanelOut();
                RefHolder.instance.uICon.animCon.EndPanelIn();
            }
            else
            {
                RefHolder.instance.uICon.EndPanelTextSetUp("Match Draw");
                RefHolder.instance.uICon.animCon.GamePanelOut();
                RefHolder.instance.uICon.animCon.EndPanelOnlineIn();
            }
            
            
        }
        
        
        
    }

    private void EndMatchCountdown()
    {
        
        StopCoroutine(CountDownCor);
        CountDownCor = null;
        isMatchEnd = true;
        if (currentPlayer == 1)
        {
            currentPlayer = 2;            
        }
        else
        {
            currentPlayer = 1;          
        }

        if (!onlineMode)
        {
            if (AIMode && currentPlayer == 2)
            {
                RefHolder.instance.uICon.SetMatchEndStatusText("AI Won");
            }
            else
            {
                RefHolder.instance.uICon.SetMatchEndStatusText("Player " + currentPlayer + " Won");
            }
            RefHolder.instance.uICon.animCon.GamePanelOut();
            RefHolder.instance.uICon.animCon.EndPanelIn();
        }
        else
        {
            if (onlinePlayer == currentPlayer)
            {
                RefHolder.instance.uICon.EndPanelTextSetUp("You Win");
            }
            else
            {
                RefHolder.instance.uICon.EndPanelTextSetUp("Opposition Won");
            }
            RefHolder.instance.uICon.animCon.GamePanelOut();
            RefHolder.instance.uICon.animCon.EndPanelOnlineIn();
        }
        
    }

    IEnumerator WinAnimation()
    {
        RefHolder.instance.uICon.takeInput = false;
        for(int i = 0;i < winningButtonTransforms.Count; i++)
        {
            winningButtonTransforms[i].GetComponent<Animator>().SetTrigger("Flash");
        }
        yield return new WaitForSeconds(1f);
        if (!onlineMode)
        {
            RefHolder.instance.uICon.animCon.GamePanelOut();
            RefHolder.instance.uICon.animCon.EndPanelIn();
        }
        else
        {
            RefHolder.instance.uICon.animCon.GamePanelOut();
            RefHolder.instance.uICon.animCon.EndPanelOnlineIn();
        }
       
        //takeInput = false;
    }



    public void StartCountDown()
    {
        if (CountDownCor == null)
        {
            coutdownTimer = TimerValue;
            CountDownCor = StartCoroutine(CoutDown());
        }
        else
        {
            coutdownTimer = TimerValue;
        }
    }

    IEnumerator CoutDown()
    {

        while (coutdownTimer > 0)
        {
            yield return new WaitForSeconds(0.1f);
            coutdownTimer -= 0.1f;
            RefHolder.instance.uICon.timerText.text = coutdownTimer.ToString("0.0");
        }
        coutdownTimer = 0f;
        //Debug.LogError("as "+coutdownTimer);
        RefHolder.instance.uICon.takeInput = false;
        EndMatchCountdown();

    }


    #region AI


    private void AIInput()
    {
        RefHolder.instance.uICon.takeInput = false;


        StartCoroutine(lateAIInput());

    }

    IEnumerator lateAIInput()
    {
        int[] input = GetInputAI();
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        RefHolder.instance.uICon.takeInput = true;
        ButtonClick(input[0], input[1]);
        
    }


    private int[] GetInputAI()
    {

        int j;
        int k;

        int[] tmp = new int[2];

        bool gotInput = false;

        for(int i = 0; i<50; i++)
        {
            j = Random.Range(0, 3);
            k = Random.Range(0, 3);

            if (board[j,k] == 0)
            {
                tmp[0] = j;
                tmp[1] = k;
                gotInput = true;
                break;

            }
        }
        if (!gotInput)
        {
            for (j = 0; j < 3; j++)
            {
                for (k = 0; k < 3; k++)
                {
                    if (board[j, k] == 0)
                    {
                        tmp[0] = j;
                        tmp[1] = k;
                        gotInput = true;
                    }
                }
            }
        }
        
        return tmp;

    }

    #endregion





    #region Online Fuctions





    








    



    #endregion

}
