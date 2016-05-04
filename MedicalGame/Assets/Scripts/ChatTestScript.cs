using UnityEngine;
using System.Collections;
using Gamedonia.Backend;
using System.Collections.Generic;
using System;
public class ChatTestScript : MonoBehaviour {

    private static float PollingFrequency = 10.0f;
    private float LastPollingTime = 0.0f;
    private string errorMsg = "";
    private bool StartPolling = false;

    private ulong LastSyncedMsg = 0;
    // Use this for initialization
    void Start ()
    {
        //Join a Chat Room
        GamedoniaScripts.Run("joinchatroom", new Dictionary<string, object>() { }, OnJoinChat);
        //Handle push
        GDPushService service = new GDPushService();
        service.RegisterEvent += new RegisterEventHandler(OnNotification);
        GamedoniaPushNotifications.AddService(service);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (StartPolling)
        {
            LastPollingTime += Time.deltaTime;
            if (LastPollingTime >= PollingFrequency)
            {
                LastPollingTime = 0.0f;
                SearchNewMessages();
            }
        }

    }

    void OnNotification(Dictionary<string, object> notification)
    {
        Hashtable payload = notification["payload"] != null ? (Hashtable)notification["payload"] : new Hashtable();
        string type = payload.ContainsKey("type") ? payload["type"].ToString() : "";
        switch (type)
        {
            case "message":
                SearchNewMessages();
                break;
            default:
                // Do nothing
                break;
        }
    }
   

    // Create the callback function
    void OnJoinChat(bool success, object data)
    {
        if (success)
        {
            StartPolling = true;
            //TODO: Show info join chat room.
        }
        else {
            errorMsg = "Can't Join Chat";
                //Gamedonia.getLastError().ToString();
            Debug.Log(errorMsg);
        }
    }



    void SearchNewMessages()
    {
        GamedoniaData.Search("chatroom", "{$and: [{ty:\"message\"}, {t:{$gte:\"" + LastSyncedMsg + "\"}}]}", 0, "{t:1}", 0, OnSearchChat);
    }

    void OnSearchChat(bool success, IList chats)
    {
        if (success)
        {
            if (chats != null)
            {
                //Get last chat
                IDictionary chat = (IDictionary)chats[chats.Count - 1];

                //Set sync time
                var time = chat["t"] as string;
                if (Convert.ToUInt64(time) > LastSyncedMsg)
                    LastSyncedMsg = Convert.ToUInt64(time);
            }

            //TODO: Show chat messages here.
        }
        else {
            errorMsg = "";
                //Gamedonia.getLastError().ToString();
            Debug.Log(errorMsg);
        }
    }

    public void OnButtonClick()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>() { { "m", "YOUR_MESSAGE_HERE" }, { "ty", "message" } };

        GamedoniaData.Create("chatroom", parameters, delegate (bool success, IDictionary data) {
            if (success)
            {
                //TODO Your success processing 
            }
            else {
                //TODO Your fail processing
            }
        });
    }

}

