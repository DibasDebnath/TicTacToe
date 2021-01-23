using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerDataManager : MonoBehaviour
{

    public readonly string USER = "User";
    public readonly string NAME = "Name";
    public readonly string ROOMS = "Rooms";
    public readonly string ROOMID = "roomID";
    public readonly string MATCH = "match";
    public readonly string WIN = "win";




    public readonly string READY = "ready";
    public readonly string ISPRIVATE = "isPrivate";
    public readonly string CURRENTPLAYER = "CurrentPlayer";
    public readonly string USERONE = "userOne";
    public readonly string USERTWO = "userTwo";
    public readonly string UID = "uid";



    public int matchValue;
    public int winValue;
    public string roomIDValue;

    

    public bool roomCreated;
    public bool roomJoined;

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
        PlayerPrefs.SetInt(MATCH, value);
    }
    public int LoadMatchValue()
    {
        return PlayerPrefs.GetInt(MATCH, 0);
    }
    public void SaveWinValue(int value)
    {
        PlayerPrefs.SetInt(WIN, value);
    }
    public int LoadWinValue()
    {
        return PlayerPrefs.GetInt(WIN, 0);
    }
    public void SaveRoomIDValue(string value)
    {
        PlayerPrefs.SetString(ROOMID, value);
    }
    public string LoadRoomIDValue()
    {
        return PlayerPrefs.GetString(ROOMID, "");
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

        FirebaseController.instance.database.RootReference.Child(USER).Child(GetUID()).SetRawJsonValueAsync(JsonUtility.ToJson(u));

    }

    



    public void CreateRoom()
    {
        string roomID = CreateRandomString();
        //string roomID = "0sgf3a";

        SetRoomID(roomID);

        Room R = new Room(roomID, GetUID(), GetDisplayName(), Random.Range(0,2), true);

        FirebaseController.instance.database.RootReference.Child(ROOMS).OrderByKey().EqualTo(roomID).GetValueAsync().ContinueWith(task =>
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
            FirebaseController.instance.database.RootReference.Child(ROOMS).Child(roomID).SetRawJsonValueAsync(JsonUtility.ToJson(R));
            roomCreated = true;
            RefHolder.instance.gamePlay.onlinePlayer = 1;
            Debug.Log("Room Created");
        });
        //FirebaseController.instance.database.RootReference.Child(Rooms).Child(roomID).SetRawJsonValueAsync(JsonUtility.ToJson(R));
    }

    public void JoinRoom(string roomID)
    {

        Debug.LogError(roomID);
        SetRoomID(roomID);

        FirebaseController.instance.database.RootReference.Child(ROOMS).OrderByKey().EqualTo(roomID).GetValueAsync().ContinueWith(task =>
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
                        //SetRoomID(roomID);
                        roomJoined = true;
                        Debug.Log("Room Exists");
                        RefHolder.instance.gamePlay.onlinePlayer = 2;
                        FirebaseController.instance.database.RootReference.Child(ROOMS).Child(roomID).Child(USERTWO).Child(UID).SetValueAsync(GetUID());
                        //FirebaseController.instance.database.RootReference.Child(ROOMS).Child(GetRoomID()).Child(USERTWO).Child(NAME).SetValueAsync(GetDisplayName());
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
            FirebaseController.instance.database.RootReference.Child(ROOMS).Child(GetRoomID()).RemoveValueAsync();
        }
    }


    private Firebase.Database.Query tmpRoomRef;

    public Firebase.Database.DataSnapshot oldDataSnapshot;
    private Firebase.Database.DataSnapshot newDataSnapshot;
    public void StartRoomValueChangeListener()
    {
        oldDataSnapshot = null;
        tmpRoomRef = FirebaseController.instance.database.RootReference.Child(ROOMS).Child(GetRoomID());
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
            if (oldDataSnapshot.Child(USERTWO).Child(UID).Value.ToString() == "" && args.Snapshot.Child(USERTWO).Child(UID).Value.ToString() != "")
            {
                RefHolder.instance.uICon.readyBut.interactable = true;
                RefHolder.instance.uICon.matchMakingFriendsErrorTxt.text = "Player Joined Press Ready";
 
            }

            // Check if Other User Ready
            //Debug.Log(args.Snapshot.Child(oneReady).Value.ToString());
            if ((oldDataSnapshot.Child(USERONE).Child(READY).Value.ToString() == "False" && args.Snapshot.Child(USERONE).Child(READY).Value.ToString() == "True" && args.Snapshot.Child(USERTWO).Child(READY).Value.ToString() == "True")
                || (oldDataSnapshot.Child(USERTWO).Child(READY).Value.ToString() == "False" && args.Snapshot.Child(USERTWO).Child(READY).Value.ToString() == "True" && args.Snapshot.Child(USERONE).Child(READY).Value.ToString() == "True"))
            {
                // Start Game
                
                
                Debug.Log("start Game");
                RefHolder.instance.uICon.StartGameOnlineFriends();
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
            FirebaseController.instance.database.RootReference.Child(ROOMS).Child(GetRoomID()).Child(USERONE).Child(READY).SetValueAsync(true);
        }
        else
        {
            FirebaseController.instance.database.RootReference.Child(ROOMS).Child(GetRoomID()).Child(USERTWO).Child(READY).SetValueAsync(true);
        }
        RefHolder.instance.uICon.readyBut.interactable = false;
    }


    


    

    public void setTurnID(int currentPlayer)
    {

        FirebaseController.instance.database.RootReference.Child(ROOMS).Child(GetRoomID()).Child(CURRENTPLAYER).SetValueAsync(currentPlayer);
      
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
    public int currentPlayer;
    public bool isPrivate;

    
    public user userOne;
    public user userTwo;

    public Room(string roomID, string uidOne, string name,int currentPlayer,bool isPrivate)
    {
        this.roomID = roomID;
        this.status = 0;
        this.isPrivate = isPrivate;
        this.currentPlayer = currentPlayer;

        userOne = new user();
        userTwo = new user();

        userOne.uid = uidOne;
        userOne.name = name;
        userOne.input = "";
        userOne.ready = false;
        userOne.win = 0;

        userTwo.uid = "";
        userTwo.name = "";
        userTwo.input = "";
        userTwo.ready = false;
        userTwo.win = 0;

    }

    [System.Serializable]
    public class user
    {
        public string uid;
        public string name;
        public string input;
        public bool ready;
        public int win;
    }
}

