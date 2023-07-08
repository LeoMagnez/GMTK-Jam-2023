using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;

    [SerializeField]
    private int troopCounter;

    [SerializeField]
    private List<float> troopCooldown = new List<float>();

    //[SerializeField]
    //public List<Troops> troops = new List<Troops>();

    [SerializeField]
    public bool attacking = false;

    public List<GameObject> allies = new List<GameObject>();
    public GameObject paladin;

    public bool hasCombatEnded = false;

    private void Awake()
    {
        

        if(instance != null)
        {
            Destroy(this);
        }

        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        paladin = Gamemanager.instance.paladin;
    }
    // Start is called before the first frame update
    public void StartCombat()
    {

        //Debug.Log("Start Combat");
        InitCooldown();
        for(int i = 0; i < allies.Count; i++)
        {
            //Debug.Log("Created Coroutine for : " + troops[i].name);
            StartCoroutine(TroopLogic(allies[i].GetComponent<Troops>()));
        }
        StartCoroutine(paladin.GetComponent<Paladin>().PaladinLogic());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitCooldown()
    {
        allies = RoomManager.instance.currentRoom.cards;

        //for (int i = 0; i < allies.Count; i++)
        //{
        //    troops.Add(allies[i].GetComponent<Troops>());
        //}
        
        //troops.Add(paladin.GetComponent<Troops>());
       
    }

    public IEnumerator TroopLogic(Troops entity)
    {
        float cooldown = entity.cooldown;
        while(entity.health > 0)
        {
            
            while (cooldown > 0)
            {
                if (!attacking)
                {
                    cooldown -= Time.deltaTime;
                }

                yield return null;
            }

            if(entity.health > 0 && entity.GetComponent<Troops>().attackIndex <= allies.Count)
            {
                Debug.Log(entity.GetComponent<Troops>().attackIndex);
                attacking = true;
                
                entity.Attack();
                
                cooldown = entity.cooldown;
            }

            yield return null;
        }
    }  

    public void CheckForEndOfCombat()
    {
        hasCombatEnded = true;
        RoomManager.instance.currentRoom.ExitRoom();
        //Debug.Log("End of Combat");

        //foreach(Troops troop in troops)
        //{
        //    if(troop.name != "Paladin")
        //    {
        //        Destroy(troop.gameObject);
        //    }
            
        //}


        allies.Clear();
        StopAllCoroutines();
        paladin.GetComponent<Paladin>().attackIndex = 0;
        attacking = false;
        if(paladin.GetComponent<Paladin>().breakArmor > 0)
        {
            paladin.GetComponent<Paladin>().breakArmor--;
        }
        if (paladin.GetComponent<Paladin>().slowEffect > 0)
        {
            paladin.GetComponent<Paladin>().slowEffect--;
        }
        //StopCoroutine(TroopLogic(troops[i]));

    }

}
