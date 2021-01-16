using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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




    public void pushData(string json, string link)
    {


        //var push = FirebaseController.instance.reference.Child("Users").Push();

        //push.SetValueAsync("dibasdebnath@gmail.com");

        FirebaseController.instance.reference.Child("Users").Child("456f").SetValueAsync("123");
    }



    

}
