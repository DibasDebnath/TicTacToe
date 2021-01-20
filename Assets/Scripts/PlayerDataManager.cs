using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour
{

    public readonly string User = "User";
    public readonly string Rooms = "Rooms";
    public readonly string RoomID = "RoomID";
    public readonly string Match = "match";
    public readonly string Win = "win";




    private void Start()
    {
        
    }

    
    public string GetUID()
    {
        return FirebaseController.instance.user.UserId;
    }
    public string GetDisplayName()
    {
        if (FirebaseController.instance.isSignedIn != false)
        {
            return FirebaseController.instance.user.DisplayName;
        }
        else
        {
            return "Player";
        }
    }

    public void SetMatchValue(int value)
    {
        PlayerPrefs.SetInt(Match, value);
    }
    public int GetMatchValue()
    {
        return PlayerPrefs.GetInt(Match , 0);
    }
    public void SetWinValue(int value)
    {
        PlayerPrefs.SetInt(Win, value);
    }
    public int GetWinValue()
    {
        return PlayerPrefs.GetInt(Win, 0);
    }
    public void MatchEndUpdateData(bool win)
    {
        SetMatchValue(GetMatchValue() + 1);
        if (win)
        {
            SetWinValue(GetWinValue() + 1);
        }
        UpdateUserData();
    }
    public void SetRoomID(string value)
    {
        PlayerPrefs.SetString(RoomID, value);
    }
    public string GetRoomID()
    {
        return PlayerPrefs.GetString(RoomID, "");
    }

    #region Firebase


    public void UpdateUserData()
    {
        Userdata u = new Userdata(GetMatchValue(), GetWinValue());

        FirebaseController.instance.database.RootReference.Child(User).Child(GetUID()).SetRawJsonValueAsync(JsonUtility.ToJson(u));

    }

    



    public void CreateRoom()
    {
        //string roomID = CreateRandomString();
        string roomID = "0sgf3a";

        SetRoomID(roomID);

        Room R = new Room(roomID, 0,"000000000",GetUID(),"","",true,false,false);

        FirebaseController.instance.database.RootReference.Child(Rooms).OrderByKey().EqualTo(roomID).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Problem Connecting");
            }
            else if (task.IsCompleted)
            {
                foreach (var childSnapshot in task.Result.Children)
                {
                    if (childSnapshot.Child(roomID) == null)
                    {
                        Debug.Log("Found Null");

                    }
                    else
                    {
                        Debug.Log("Same Room Exist");
                        return;
                        
                    }
                }

            }
            FirebaseController.instance.database.RootReference.Child(Rooms).Child(roomID).SetRawJsonValueAsync(JsonUtility.ToJson(R));
            Debug.Log("Room Created");
        });
        //FirebaseController.instance.database.RootReference.Child(Rooms).Child(roomID).SetRawJsonValueAsync(JsonUtility.ToJson(R));
    }





    #endregion




    #region Helper Methods

    public string CreateRandomString()
    {
        string pull = "abcdefghijklmnopqrstuvwxyz0123456789";

        string random = "000000";

        char[] array = random.ToCharArray();

        for (int i = 0; i < 6; i++)
        {
            array[i] = pull[Random.Range(0, 36)];
        }


        random = new string(array);

        return random;

    }




    #endregion

};
public class Userdata
{
    int match;
    int win;

    public Userdata(int m,int w)
    {
        match = m;
        win = w;
    }

};
public class Room
{
    //"gameID" : "kkkkkk",
    //        "Status" : 0,
    //        "Board" : "000000000",
    //        "uidOne" : "asdafsdawdasdawd",
    //        "uidTwo" : "asdawfasdawdasda",
    //        "turnUid" : "asdawfsdawdasddwa",
    //        "private" : 0


    public string roomID;
    public int status;
    public string board;
    public string uidOne;
    public string uidTwo;
    public string turnUid;
    public bool isPrivate;
    public bool oneReady;
    public bool twoReady;



    public Room(string roomID, int status,string board,string uidOne,string uidTwo,string turnUid,bool isPrivate,bool oneReady,bool twoReady)
    {
        this.roomID = roomID;
        this.status = status;
        this.board = board;
        this.uidOne = uidOne;
        this.uidTwo = uidTwo;
        this.turnUid = turnUid;
        this.isPrivate = isPrivate;
        this.oneReady = oneReady;
        this.oneReady = oneReady;
    }

}
