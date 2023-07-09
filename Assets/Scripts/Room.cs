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
        ManaDisplay.instance.UpdateDisplay(6- mana);
    }

    public void RemoveFromRoom(CardData entity)
    {
        cards.Remove(entity.generatedTroup);
        mana += entity.price;
        ManaDisplay.instance.UpdateDisplay(6-mana);
    }

    public void ExitRoom()
    {
        if (Gamemanager.instance.Paladead)
        {
            Gamemanager.instance.Paladead = false;

            

            Debug.Log("change la salle");
            RoomManager.instance.ChangeCurrentRoom(MapManager.instance.startRoom);
            MapManager.instance.startRoom.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            environnement.SetActive(false);
        }
        else if (connectedRooms[0] == null)
        {
            nettoyage();
        }
        else if(connectedRooms.Length > 1)
        {
            //choisir la prochaine salle



            float densite1 = 6f - connectedRooms[0].mana ;
            float densite2 = 6f - connectedRooms[1].mana ;

            int direction;
            //float ratioHealth = Gamemanager.instance.paladin.GetComponent<Paladin>().health / Gamemanager.instance.paladin.GetComponent<Paladin>().maxHealth;

            if(Gamemanager.instance.paladin.GetComponent<Paladin>().health > Gamemanager.instance.paladin.GetComponent<Paladin>().maxHealth/2f)
            {
                if(densite1 > densite2)
                {
                    direction = 0;
                    //RoomManager.instance.ChangeCurrentRoom(connectedRooms[0]);
                }
                else if(densite1 < densite2)
                {
                    direction = 1;
                    //RoomManager.instance.ChangeCurrentRoom(connectedRooms[1]);
                }
                else
                {
                    direction = Random.Range(0, 2);
                    //RoomManager.instance.ChangeCurrentRoom(connectedRooms[Random.Range(0,2)]);
                }
            }
            else
            {
                if (densite1 > densite2)
                {
                    direction = 1;
                    //RoomManager.instance.ChangeCurrentRoom(connectedRooms[1]);
                }
                else if (densite1 < densite2)
                {
                    direction = 0;
                    //RoomManager.instance.ChangeCurrentRoom(connectedRooms[0]);
                }
                else
                {
                    direction = Random.Range(0, 2);
                    //RoomManager.instance.ChangeCurrentRoom(connectedRooms[Random.Range(0, 2)]);
                }
            }

            RoomManager.instance.ChangeCurrentRoom(connectedRooms[direction]);
            connectedRooms[direction].gameObject.SetActive(true);



            //this.gameObject.SetActive(false);
            environnement.SetActive(false);


            
        }
        else
        {
            Debug.Log("change la salle");
            RoomManager.instance.ChangeCurrentRoom(connectedRooms[0]);
            connectedRooms[0].gameObject.SetActive(true);
            //this.gameObject.SetActive(false);
            environnement.SetActive(false);

            
        }
    }
    public void CreateTroups(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                GameObject temp = Instantiate(list[i], slots[i].transform);

                instantiated.Add(temp);
            }
            else
            {
                Destroy(slots[i].transform.GetChild(0).gameObject);
            }
            
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
        if(FightManager.instance.allies != null)
        {
            FightManager.instance.allies.Clear();
        }
        

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

    public void nettoyage()
    {
        //MapManager.instance.currentTemplate = null;

        MapManager.instance.ChooseMap();

        FightManager.instance.allies = null;

        //this.gameObject.SetActive(false);
        environnement.SetActive(false);

        Gamemanager.instance.NextStep();
    }
}
