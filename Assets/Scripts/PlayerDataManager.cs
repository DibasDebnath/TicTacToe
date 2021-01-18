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
        if(PlayerPrefs.GetString(UID) != uid)
        {
            PlayerPrefs.SetString(uid, uid);
        }
    }
    public string GetUID()
    {
        return PlayerPrefs.GetString(UID, " ");
    }
}
