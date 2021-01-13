using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GamePlay : MonoBehaviour
{

    [SerializeField]
    private List<Transform> buttonTransforms;


    private Button[,] buttonArray = new Button[3,3];
    private GameObject[,] OArray = new GameObject[3,3];
    private GameObject[,] XArray = new GameObject[3,3];

    private int[,] board = new int[3,3]; // 0 - Empty , 1 - O , 2 - X

    private int currentPlayer;

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
            }
        }

        
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

        if(board[j,k] == 0)
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



    private void WinCheck()
    {
        int tmp = 0;
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                if(board[j,k] == currentPlayer)
                {
                    tmp++;
                    if(tmp == 3)
                    {
                        Debug.Log("Player " + currentPlayer + " Wins");
                    }
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
                    if (tmp == 3)
                    {
                        Debug.Log("Player " + currentPlayer + " Wins");
                    }
                }
            }
            tmp = 0;
        }
        for (int i = 0; i < 3; i++)
        {
            if (board[i, i] == currentPlayer)
            {
                tmp++;
                if (tmp == 3)
                {
                    Debug.Log("Player " + currentPlayer + " Wins");
                }
            }
        }
        tmp = 0;

        for (int k = 0; k < 3; k++)
        {
            if (board[2-k, k] == currentPlayer)
            {
                tmp++;
                if (tmp == 3)
                {
                    Debug.Log("Player " + currentPlayer + " Wins");
                }
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
        if(check == true)
        {
            Debug.Log("Match Draw");
        }
    }

    private void SwithPlayer()
    {
        if(currentPlayer == 1)
        {
            currentPlayer = 2;
            Debug.Log("CurrentPlayer " + currentPlayer);
        }
        else
        {
            currentPlayer = 1;
            Debug.Log("CurrentPlayer " + currentPlayer);
        }
    }
                
}
