using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerDataManager : MonoBehaviour
{

    public readonly string USER = "User";
    public readonly string NAME = "name";
    public readonly string ROOMS = "Rooms";
    public readonly string ROOMID = "roomID";
    public readonly string MATCH = "match";
    public readonly string WIN = "win";




    public readonly string READY = "ready";
    public readonly string ISPRIVATE = "isPrivate";
    public readonly string CURRENTPLAYER = "currentPlayer";
    public readonly string USERONE = "userOne";
    public readonly string USERTWO = "userTwo";
    public readonly string INPUT = "input";
    public readonly string UID = "uid";



    public int matchValue;
    public int winValue;
    public string roomIDValue;

    private string player1Name;
    private string player2Name;

    public bool roomCreated;
    public bool roomJoined;

    private void Start()
    {
        GetAllPlayerPrefsValue();
        //PlayerPrefs.DeleteAll();
        //Debug.LogError(GetRoomID());
        StartCoroutine(DelayFirebaseAction());
    }

    IEnumerator DelayFirebaseAction()
    {
        yield return new WaitForSeconds(0.5f);
        if(FirebaseController.instance.user != null)
        {
            GetUserData();
            yield return new WaitForSeconds(1f);
            SetMatchValue(matchValue);
            SetWinValue(winValue);
        }
        
        
    }
    public void GetAllPlayerPrefsValue()
    {
        matchValue = LoadMatchValue();
        winValue = LoadWinValue();
        roomIDValue = LoadRoomIDValue();
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
        if(FirebaseController.instance.user != null)
        {
            return FirebaseController.instance.user.DisplayName;
        }
        else
        {
            return "player";
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


    public void GetUserData()
    {
        
        
        FirebaseController.instance.database.RootReference.Child(USER).OrderByKey().EqualTo(GetUID()).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Problem Connecting");
            }
            else if (task.IsCompleted)
            {
                foreach (var childSnapshot in task.Result.Children)
                {
                    if (childSnapshot.Child(GetUID()) == null)
                    {
                        Debug.Log("Found Null");
                        UpdateUserData();
                        return;
                    }
                    else
                    {
                        int m = int.Parse(childSnapshot.Child(MATCH).Value.ToString());
                        int w = int.Parse(childSnapshot.Child(WIN).Value.ToString());
                        Debug.Log("User Exists");
                        
          
                        if(GetMatchValue() > m)
                        {
                            UpdateUserData();
                        }
                        else
                        {
                            matchValue = m;
                            winValue = w;
                        }
                        return;

                    }
                }
                UpdateUserData();
                return;
            }
            
        });
        //FirebaseController.instance.database.RootReference.Child(Rooms).Child(roomID).SetRawJsonValueAsync(JsonUtility.ToJson(R));
    }



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

        Room R = new Room(roomID, GetUID(), GetDisplayName(), Random.Range(1,3), true);

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

        //Debug.LogError(roomID);
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
                        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
                        childUpdates[UID] = GetUID();
                        childUpdates[NAME] = GetDisplayName();

                        FirebaseController.instance.database.RootReference.Child(ROOMS).Child(roomID).Child(USERTWO).UpdateChildrenAsync(childUpdates);
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
            newDataSnapshot = args.Snapshot;
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



                RefHolder.instance.uICon.SetGamePanelTextAtStart(newDataSnapshot.Child(USERONE).Child(NAME).Value.ToString(),
                    newDataSnapshot.Child(USERTWO).Child(NAME).Value.ToString(),
                    newDataSnapshot.Child(USERONE).Child(WIN).Value.ToString(),
                    newDataSnapshot.Child(USERTWO).Child(WIN).Value.ToString());

                Debug.Log("start Game");
                RefHolder.instance.uICon.StartGameOnlineFriends();
            }


            //Getting Input
            if (oldDataSnapshot.Child(CURRENTPLAYER).Value.ToString() != args.Snapshot.Child(CURRENTPLAYER).Value.ToString())
            {
                Debug.LogError("1");
                if(args.Snapshot.Child(CURRENTPLAYER).Value.ToString() == RefHolder.instance.gamePlay.onlinePlayer.ToString())
                {
                    Debug.LogError("2");
                    RefHolder.instance.uICon.takeInput = true;
                    if (args.Snapshot.Child(CURRENTPLAYER).Value.ToString() == "1")
                    {
                        Debug.LogError("3");

                        string str = args.Snapshot.Child(USERTWO).Child(INPUT).Value.ToString();

                        char[] arr = str.ToCharArray();

                        int j = int.Parse(arr[0].ToString());
                        int k = int.Parse(arr[1].ToString());

                        //Debug.Log("Button CLick " + j + k);
                        RefHolder.instance.gamePlay.ButtonClick(j,k);
                    }
                    else
                    {
                        Debug.LogError("4");

                        string str = args.Snapshot.Child(USERONE).Child(INPUT).Value.ToString();

                        char[] arr = str.ToCharArray();

                        int j = int.Parse(arr[0].ToString());
                        int k = int.Parse(arr[1].ToString());

                        Debug.Log("Button CLick " + j + k);
                        RefHolder.instance.gamePlay.ButtonClick(j, k);
                    }
                }
            }


            // if Match Ended Unconditionally
            if(oldDataSnapshot.Child(USERONE).Child(READY).Value.ToString() == "True" &&
                oldDataSnapshot.Child(USERTWO).Child(READY).Value.ToString() == "True"&&
                args.Snapshot.Child(USERONE).Child(READY).Value.ToString() == "False" &&
                args.Snapshot.Child(USERTWO).Child(READY).Value.ToString() == "False" )
            {
                //RefHolder.instance.gamePlay.EndMatch(false);
            }



            // Set ready false
            if(RefHolder.instance.gamePlay.onlinePlayer == 1)
            {
                if (oldDataSnapshot.Child(USERONE).Child(READY).Value.ToString() == "True" &&
                args.Snapshot.Child(USERONE).Child(READY).Value.ToString() == "False")
                {
                    RefHolder.instance.uICon.matchMakingFriendsErrorTxt.text = "Press Ready";
                    RefHolder.instance.uICon.readyBut.interactable = true;

                    RefHolder.instance.uICon.readyButEnd.interactable = true;
                    RefHolder.instance.uICon.gamePanelErorrText.text = "";
                    RefHolder.instance.uICon.matchOnlineEndError.text = "Press Ready";
                    SetEndPanelOnlineEnd();
                }
            }
            else
            {
                if (oldDataSnapshot.Child(USERTWO).Child(READY).Value.ToString() == "True" &&
                args.Snapshot.Child(USERTWO).Child(READY).Value.ToString() == "False")
                {
                    RefHolder.instance.uICon.matchMakingFriendsErrorTxt.text = "Press Ready";
                    RefHolder.instance.uICon.readyBut.interactable = true;

                    RefHolder.instance.uICon.readyButEnd.interactable = true;
                    RefHolder.instance.uICon.gamePanelErorrText.text = "";
                    RefHolder.instance.uICon.matchOnlineEndError.text = "Press Ready";
                    SetEndPanelOnlineEnd();
                }
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


    public void ResetBothReady()
    {
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[USERONE + "/" + READY] = false;
        childUpdates[USERTWO + "/" + READY] = false;


        FirebaseController.instance.database.RootReference.Child(ROOMS).Child(GetRoomID()).UpdateChildrenAsync(childUpdates);
    }


    

   public void EndMatchOnline(bool win)
    {
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[USERONE + "/" + READY] = false;
        childUpdates[USERTWO + "/" + READY] = false;
        if (win)
        {
            int tmp;
            if (RefHolder.instance.gamePlay.onlinePlayer == 1)
            {
                //Debug.LogError("parse " + oldDataSnapshot.Child(USERONE).Child(WIN).ToString());
                tmp = int.Parse(newDataSnapshot.Child(USERONE).Child(WIN).Value.ToString()) + 1;
                childUpdates[USERONE + "/" + WIN] = tmp;
            }
            else
            {
                //Debug.LogError("parse " + oldDataSnapshot.Child(USERTWO).Child(WIN).ToString());

                tmp = int.Parse(newDataSnapshot.Child(USERTWO).Child(WIN).Value.ToString()) + 1;
                childUpdates[USERTWO + "/" + WIN] = tmp;
            }
            
        }

        

        FirebaseController.instance.database.RootReference.Child(ROOMS).Child(GetRoomID()).UpdateChildrenAsync(childUpdates);


        

    }




    public void onlineInput(int j, int k)
    {
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        if (RefHolder.instance.gamePlay.currentPlayer == 1)
        {          
            childUpdates[CURRENTPLAYER] = 2;
            childUpdates[USERONE + "/" + INPUT] = j.ToString() + k.ToString();
        }
        else
        {
            childUpdates[CURRENTPLAYER] = 1;
            childUpdates[USERTWO + "/" + INPUT] = j.ToString() + k.ToString();
        }
        


        FirebaseController.instance.database.RootReference.Child(ROOMS).Child(GetRoomID()).UpdateChildrenAsync(childUpdates);
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


    public void SetEndPanelOnlineEnd()
    {
        RefHolder.instance.uICon.SetEndPanelTextAtEndOnline(newDataSnapshot.Child(USERONE).Child(NAME).Value.ToString(),
                    newDataSnapshot.Child(USERTWO).Child(NAME).Value.ToString(),
                    newDataSnapshot.Child(USERONE).Child(WIN).Value.ToString(),
                    newDataSnapshot.Child(USERTWO).Child(WIN).Value.ToString());
    }

    public void IncreaseMatchStat(bool win)
    {
        SetMatchValue(GetMatchValue() + 1);

        if (RefHolder.instance.gamePlay.onlinePlayer == RefHolder.instance.gamePlay.currentPlayer)
        {
            if (win)
            {
                SetWinValue(GetWinValue() + 1);
            }
        }
    }

    #endregion

};
public class Userdata
{
    public int match;
    public int win;

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

