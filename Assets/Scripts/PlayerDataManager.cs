using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerDataManager : MonoBehaviour
{

    public readonly string User = "User";
    public readonly string Rooms = "Rooms";
    public readonly string RoomID = "roomID";
    public readonly string Match = "match";
    public readonly string Win = "win";



    public readonly string roomID = "roomID";
    public readonly string status = "status";
    public readonly string board = "board";
    public readonly string uidOne = "uidOne";
    public readonly string uidTwo = "uidTwo";
    public readonly string turnUid = "turnUid";
    public readonly string isPrivate = "isPrivate";
    public readonly string oneReady = "oneReady";
    public readonly string twoReady = "twoReady";



    public int matchValue;
    public int winValue;
    public string roomIDValue;

    public bool roomCreated;
    public bool roomJoined;
    public bool playerEntered;

    private void Start()
    {
        GetAllPlayerPrefsValue();
        //PlayerPrefs.DeleteAll();
        //Debug.LogError(GetRoomID());
    }
    public void GetAllPlayerPrefsValue()
    {
        matchValue = GetMatchValue();
        winValue = GetWinValue();
        roomIDValue = GetRoomID();
    }
    public void SaveMatchValue(int value)
    {
        PlayerPrefs.SetInt(Match, value);
    }
    public int LoadMatchValue()
    {
        return PlayerPrefs.GetInt(Match, 0);
    }
    public void SaveWinValue(int value)
    {
        PlayerPrefs.SetInt(Win, value);
    }
    public int LoadWinValue()
    {
        return PlayerPrefs.GetInt(Win, 0);
    }
    public void SaveRoomIDValue(string value)
    {
        PlayerPrefs.SetString(RoomID, value);
    }
    public string LoadRoomIDValue()
    {
        return PlayerPrefs.GetString(RoomID, "");
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
        matchValue = value;
        SaveMatchValue(value);
    }
    public int GetMatchValue()
    {
        return matchValue;
    }
    public void SetWinValue(int value)
    {
        winValue = value;
        SaveWinValue(value);
    }
    public int GetWinValue()
    {
        return winValue;
    }
    public void SetRoomID(string value)
    {
        roomIDValue = value;
        SaveRoomIDValue(value);
    }
    public string GetRoomID()
    {
        return roomIDValue;
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


    #region Firebase


    

    public void UpdateUserData()
    {
        Userdata u = new Userdata(GetMatchValue(), GetWinValue());

        FirebaseController.instance.database.RootReference.Child(User).Child(GetUID()).SetRawJsonValueAsync(JsonUtility.ToJson(u));

    }

    



    public void CreateRoom()
    {
        string roomID = CreateRandomString();
        //string roomID = "0sgf3a";

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
                        CreateRoom();
                        Debug.Log("Same Room Exist");
                        //DeletePreviousRoomIfExists();
                        
                        return;
                        
                    }
                }

            }
            FirebaseController.instance.database.RootReference.Child(Rooms).Child(roomID).SetRawJsonValueAsync(JsonUtility.ToJson(R));
            roomCreated = true;
            Debug.Log("Room Created");
        });
        //FirebaseController.instance.database.RootReference.Child(Rooms).Child(roomID).SetRawJsonValueAsync(JsonUtility.ToJson(R));
    }

    public void JoinRoom(string roomID)
    {
        
        

        FirebaseController.instance.database.RootReference.Child(Rooms).OrderByKey().EqualTo(roomID).GetValueAsync().ContinueWith(task =>
        {
            roomJoined = false;
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
                        SetRoomID(roomID);
                        roomJoined = true;
                        Debug.Log("Room Exists");
                        FirebaseController.instance.database.RootReference.Child(Rooms).Child(GetRoomID()).Child(uidTwo).SetValueAsync(GetUID());
                        //DeletePreviousRoomIfExists();
                        return;

                    }
                }

            }
            
        });
    }


    public void DeletePreviousRoomIfExists()
    {
        //Debug.Log("Same Room Exist 1 "+ GetRoomID());
        if (GetRoomID() != "")
        {
            //Debug.Log("Same Room Exist 2");
            FirebaseController.instance.database.RootReference.Child(Rooms).Child(GetRoomID()).RemoveValueAsync();
        }
    }


    private Firebase.Database.Query tmpRoomRef;

    private Firebase.Database.DataSnapshot oldDataSnapshot;
    private Firebase.Database.DataSnapshot newDataSnapshot;
    public void StartRoomValueChangeListener()
    {
        oldDataSnapshot = null;
        tmpRoomRef = FirebaseController.instance.database.RootReference.Child(Rooms).Child(GetRoomID());
        tmpRoomRef.ValueChanged += HandleRoomValueChange;
    }


    public void StopRoomValueChangeListener()
    {
        if (tmpRoomRef != null)
        {
            tmpRoomRef.ValueChanged -= HandleRoomValueChange;
        }
    }


    private void HandleRoomValueChange(object sender, Firebase.Database.ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log("Received values.");

        if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
        {
            if (oldDataSnapshot == null)
            {
                //Save old Data at first 
                oldDataSnapshot = args.Snapshot;
                return;
            }
            // Check if User opposite User Entered
            if (oldDataSnapshot.Child(uidTwo).Value.ToString() == "" && args.Snapshot.Child(uidTwo).Value.ToString() != "")
            {
                RefHolder.instance.uICon.readyBut.interactable = true;
                RefHolder.instance.uICon.matchMakingFriendsErrorTxt.text = "Player Joined Press Ready";
            }

            // Check if Other User Ready
            //Debug.Log(args.Snapshot.Child(oneReady).Value.ToString());
            if ((oldDataSnapshot.Child(oneReady).Value.ToString() == "False" && args.Snapshot.Child(oneReady).Value.ToString() == "True" && args.Snapshot.Child(twoReady).Value.ToString() == "True")
                || (oldDataSnapshot.Child(twoReady).Value.ToString() == "False" && args.Snapshot.Child(twoReady).Value.ToString() == "True" && args.Snapshot.Child(oneReady).Value.ToString() == "True"))
            {
                // Start Game
                Debug.Log("start Game");
            }
            
            



            //replace old at the end
            oldDataSnapshot = args.Snapshot;
        }
        else
        {
            Debug.LogError("Recieved null data");
            SetRoomID("");
            SceneManager.LoadScene("Game");
        }
    }






    public void setUserReady()
    {
        if (roomCreated)
        {
            FirebaseController.instance.database.RootReference.Child(Rooms).Child(GetRoomID()).Child(oneReady).SetValueAsync(true);
        }
        else
        {
            FirebaseController.instance.database.RootReference.Child(Rooms).Child(GetRoomID()).Child(twoReady).SetValueAsync(true);
        }
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
        this.twoReady = twoReady;
    }

}
