using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int mana;
    public Room[] connectedRooms;

    public List<GameObject> cards = new List<GameObject>();
    public List<GameObject> instantiated = new List<GameObject>();
    [SerializeField]public GameObject cardHolder;
    public enum roomType
    {
        Normal,
        Treasure
    }

    public roomType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FightManager.instance.hasCombatEnded = false;
            EnterRoom();
        }
    }

    public void EnterRoom()
    {
        FightManager.instance.allies = cards;
        FightManager.instance.StartCombat();
    }

    public void AddToRoom(CardData entity)
    {
        cards.Add(entity.generatedTroup);
        mana -= entity.price;
    }

    public void RemoveFromRoom(CardData entity)
    {
        cards.Remove(entity.generatedTroup);
        mana += entity.price;
    }

    public void ExitRoom()
    {
        if(connectedRooms.Length > 1)
        {
            //choisir la prochaine salle
        }
        else
        {
            Debug.Log("change la salle");
            RoomManager.instance.ChangeCurrentRoom(connectedRooms[0]);
            connectedRooms[0].gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    public void CreateTroups()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject temp = Instantiate(cards[i], transform);
            
            instantiated.Add(temp);
        }
    }
}
