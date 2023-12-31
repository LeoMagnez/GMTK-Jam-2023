using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    public Room currentRoom;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        else
        {
            instance = this;
        }
        gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCurrentRoom(Room newRoom, bool start = false)
    {
        Debug.Log(newRoom);
        newRoom.gameObject.SetActive(true);
        currentRoom = newRoom;
        if (!start)
        {
            currentRoom.EnterRoom();
        }


    }
}
