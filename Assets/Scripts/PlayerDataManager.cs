using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour
{

    private readonly string UID = "uid";


    private void Start()
    {
        
    }

    public void SetUID(string uid)
    {
        PlayerPrefs.SetString(UID, uid);
    }
    public string GetUID()
    {
        
        return PlayerPrefs.GetString(UID, " ");
    }
}
