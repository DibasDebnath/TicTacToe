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

    // Start is called before the first frame update
    void Start()
    {

        if (buttonTransforms[0] == null)
        {
            Debug.LogError("Set Buttons");
        }
        //Setting Buttons at start
        int i = 0;
        for(int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                buttonArray[j,k] = buttonTransforms[i++].GetComponent<Button>();
            }
        }

        //Setting O at start
        i = 0;
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                OArray[j,k] = buttonTransforms[i++].GetChild(0).gameObject;
            }
        }
        //Setting X at start
        i = 0;
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                XArray[j,k] = buttonTransforms[i++].GetChild(1).gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void startGame()
    {

    }
}
