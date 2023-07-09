using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSlot : MonoBehaviour
{
    public Room projectedRoom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MapManager.instance.currentRoomInEdit = projectedRoom;
            
            MapManager.instance.SwitchView();
            // Whatever you want it to do.
        }
    }
}
