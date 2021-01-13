using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GamePlay : MonoBehaviour
{

    [SerializeField]
    private List<Transform> buttonTransforms;

    private List<Transform> winningButtonTransforms = new List<Transform>();
    private bool takeInput = false;
    public bool isWon = false;
    private bool isCountdownOn;
    private float coutdownTimer;

    private Button[,] buttonArray = new Button[3,3];
    private GameObject[,] OArray = new GameObject[3,3];
    private GameObject[,] XArray = new GameObject[3,3];

    private int[,] board = new int[3,3]; // 0 - Empty , 1 - O , 2 - X

    public int currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
        if (buttonTransforms[0] == null)
        {
            Debug.LogError("Set Buttons");
        }
        


    } 


    public void StartGame()
    {
        InitializeGame();
        AddListeners();
        ResetAll();
        takeInput = true;
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
                currentPlayer = Random.Range(1, 3);
                RefHolder.instance.uICon.playerText.text = "Player " + currentPlayer;
            }
        }

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

    private void ButtonClick(int j, int k)
    {
        Debug.Log("ButtonCLicked " + j + " " + k);
        //RefHolder.instance.playerInput.buttonClick(j, k);
        if (takeInput)
        {
            if (board[j, k] == 0)
            {
                board[j, k] = currentPlayer;
                buttonArray[j, k].interactable = false;
                switch (currentPlayer)
                {
                    case 1:
                        OArray[j, k].SetActive(true);
                        WinCheck();
                        DrawCheck();
                        SwithPlayer();
                        break;
                    case 2:
                        XArray[j, k].SetActive(true);
                        WinCheck();
                        DrawCheck();
                        SwithPlayer();
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
        if (!isWon)
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
        
    }

    private void SwithPlayer()
    {
        if(currentPlayer == 1)
        {
            currentPlayer = 2;
            Debug.Log("CurrentPlayer " + currentPlayer);
            RefHolder.instance.uICon.playerText.text = "Player " + currentPlayer;
            StartCountDown();
        }
        else
        {
            currentPlayer = 1;
            Debug.Log("CurrentPlayer " + currentPlayer);
            RefHolder.instance.uICon.playerText.text = "Player " + currentPlayer;
            StartCountDown();
        }
    }

    private void EndMatch(bool endStatus)
    {
        //win
        if (endStatus)
        {
            isWon = true;
            RefHolder.instance.uICon.SetMatchEndStatusText("Player "+currentPlayer+" Won");
            StartCoroutine(WinAnimation());
        }
        else // draw
        {
            RefHolder.instance.uICon.SetMatchEndStatusText("Match Draw");
            RefHolder.instance.uICon.animCon.GamePanelOut();
            RefHolder.instance.uICon.animCon.EndPanelIn();
        }
        isCountdownOn = false;
        StopCoroutine(CoutDown());
    }

    private void EndMatchCountdown()
    {
        isWon = true;
        SwithPlayer();
        RefHolder.instance.uICon.SetMatchEndStatusText("Player " + currentPlayer + " Won");
        RefHolder.instance.uICon.animCon.GamePanelOut();
        RefHolder.instance.uICon.animCon.EndPanelIn();
    }

    IEnumerator WinAnimation()
    {
        takeInput = false;
        for(int i = 0;i < winningButtonTransforms.Count; i++)
        {
            winningButtonTransforms[i].GetComponent<Animator>().SetTrigger("Flash");
        }
        yield return new WaitForSeconds(1f);
        RefHolder.instance.uICon.animCon.GamePanelOut();
        RefHolder.instance.uICon.animCon.EndPanelIn();
        //takeInput = false;
    }



    public void StartCountDown()
    {
        if(!isCountdownOn)
        {
            coutdownTimer = 5f;
            isCountdownOn = true;
            StartCoroutine(CoutDown());
        }
        else
        {
            coutdownTimer = 5f;
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
        isCountdownOn = false;
        takeInput = false;
        EndMatchCountdown();

    }


    #region AI


    #endregion

}
