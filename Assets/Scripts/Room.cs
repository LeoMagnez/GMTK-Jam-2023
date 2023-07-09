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

    public bool havetoPlay;

    public List<GameObject> melees = new List<GameObject>();
    public List<GameObject> ranges = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameObject.name);
        if (Input.GetKeyDown(KeyCode.Space ) || havetoPlay)
        {
            havetoPlay = false;
            //
            EnterRoom();

        }
    }

    public void EnterRoom()
    {
        FightManager.instance.hasCombatEnded = false;
        
        for (int i = 0; i < cards.Count; i++)
        {
            Debug.Log("ajoute carte :" + cards[i].name);
            Debug.Log("avec port�e :" + cards[i].GetComponent<Troops>().melee);
            if (cards[i].GetComponent<Troops>().melee)
            {
                Debug.Log("ici");
                melees.Add(cards[i]);
            }
            else
            {
                ranges.Add(cards[i]);
            }
        }

        FightManager.instance.allies.Clear();

        for (int i = 0; i < melees.Count; i++)
        {
            Debug.Log("ajoute � allies, m�l�e : " + melees[i].name);
            FightManager.instance.allies.Add(melees[i]);
        }
            

        for (int i = 0; i < ranges.Count; i++)
        {
            Debug.Log("ajoute � allies, range : " + ranges[i].name);
            FightManager.instance.allies.Add(ranges[i]);
        }
            

        //FightManager.instance.allies = cards;
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
            GameObject temp = Instantiate(cards[i], Gamemanager.instance.paladin.transform);
            
            instantiated.Add(temp);
        }
    }
}
