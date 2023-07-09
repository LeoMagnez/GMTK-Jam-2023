using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int mana;
    public Room[] connectedRooms;

    public List<GameObject> cards = new List<GameObject>();
    public List<GameObject> slots = new List<GameObject>();
    public List<GameObject> instantiated = new List<GameObject>();
    [SerializeField]public GameObject cardHolder;
    [SerializeField] GameObject environnement;
    [SerializeField] ParticleSystem dust;

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


        dust.Play();
        environnement.SetActive(true);

        StartCoroutine(WaitBeforeFight());

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
            environnement.SetActive(false);
        }
    }
    public void CreateTroups(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = Instantiate(list[i], slots[i].transform);
            
            instantiated.Add(temp);
        }
    }

    public IEnumerator WaitBeforeFight()
    {
        yield return new WaitForSeconds(3f);
        List<GameObject> sorted = new List<GameObject>();
        FightManager.instance.hasCombatEnded = false;
        for (int i = 0; i < cards.Count; i++)
        {
            Debug.Log("ajoute carte :" + cards[i].name);
            Debug.Log("avec portée :" + cards[i].GetComponent<Troops>().melee);
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
            Debug.Log("ajoute à allies, mélée : " + melees[i].name);
            sorted.Add(melees[i]);
        }


        for (int i = 0; i < ranges.Count; i++)
        {
            Debug.Log("ajoute à allies, range : " + ranges[i].name);
            sorted.Add(ranges[i]);
        }
        CreateTroups(sorted);

        FightManager.instance.allies = instantiated;
        FightManager.instance.StartCombat();
    }
}
