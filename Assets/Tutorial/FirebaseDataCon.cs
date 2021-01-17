using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class FirebaseDataCon : MonoBehaviour
{
    public static FirebaseDataCon instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    public void pushUserData(string json, string link)
    {


        var push = FirebaseController.instance.reference.Child("Users").Push();

        push.SetRawJsonValueAsync(json);

        
    }

    public void GetUserData(string email)
    {


        FirebaseController.instance.database.GetReference("Users").OrderByChild("email").EqualTo(email).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Problem Connecting");
            }
            else if (task.IsCompleted)
            {
                foreach (var childSnapshot in task.Result.Children)
                {
                    if (childSnapshot.Child("email") == null)
                    {
                        Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                        break;
                    }
                    else
                    {
                        Debug.Log("Got value " + childSnapshot.Child("email").Value.ToString());
                        Debug.Log("Got value " + childSnapshot.Child("displayName").Value.ToString());
                        Debug.Log("Got value " + childSnapshot.Child("match").Value.ToString());

                        List<string> list = new List<string>();


                        //Demo of adding data in uncertain place
                        //var push = childSnapshot.Child("friends").Reference.Push();

                        //push.SetValueAsync("demo3@gmail.com");

                        childSnapshot.Child("friends").Reference.GetValueAsync().ContinueWith(task =>
                        {
                            foreach (var childSnapshoted in task.Result.Children)
                            {
                                list.Add(childSnapshoted.Value.ToString());
                                Debug.Log(childSnapshoted.Value.ToString());
                            }
                        });



                    }
                }
                
            }
            

    });


    }
    Query tmpref;

    public void StartValueChangeListener()
    {
        tmpref = FirebaseController.instance.database.GetReference("Users").OrderByChild("email").EqualTo("dibasdebnath@gmail.com");
        tmpref.ValueChanged += handleValueChange;
    }

    public void StopValueChangeListener()
    {
        if(tmpref != null)
        {
            tmpref.ValueChanged -= handleValueChange;
        }
        

        
        
    }


    public void handleValueChange(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log("Received values.");

        if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
        {
            foreach (var childSnapshot in args.Snapshot.Children)
            {
                if (childSnapshot.Child("email") == null)
                {
                    Debug.LogError("Bad data in sample.");
                    break;
                }
                else
                {
                    Debug.Log("Got value " + childSnapshot.Child("email").Value.ToString());
                    Debug.Log("Got value " + childSnapshot.Child("displayName").Value.ToString());
                    Debug.Log("Got value " + childSnapshot.Child("match").Value.ToString());
                    childSnapshot.Child("friends").Reference.GetValueAsync().ContinueWith(task =>
                    {
                        foreach (var childSnapshoted in task.Result.Children)
                        {
                            //list.Add(childSnapshoted.Value.ToString());
                            Debug.Log(childSnapshoted.Value.ToString());
                        }
                    });
                }
            }
        }
        else
        {
            Debug.LogError("Recieved null data");
        }
    }

}

