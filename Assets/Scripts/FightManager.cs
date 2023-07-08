using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;

    [SerializeField]
    private float ticker;

    [SerializeField]
    private List<float> troopCooldown = new List<float>();

    [SerializeField]
    public List<Troops> troops = new List<Troops>();

    [SerializeField]
    public bool attacking = false;

    public GameObject[] allies;
    public GameObject paladin;

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

    // Start is called before the first frame update
    void Start()
    {
        InitCooldown();
        for(int i = 0; i < troops.Count; i++)
        {
            StartCoroutine(TroopLogic(troops[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitCooldown()
    {
        for (int i = 0; i < allies.Length; i++)
        {
            troops.Add(allies[i].GetComponent<Troops>());
        }
        
        troops.Add(paladin.GetComponent<Troops>());
       
    }

    public IEnumerator TroopLogic(Troops entity)
    {
        float cooldown = entity.cooldown;
        while(entity.health > 0)
        {
            while(cooldown > 0)
            {
                if (!attacking)
                {
                    cooldown -= Time.deltaTime;
                }

                yield return null;
            }

            if(entity.health > 0 && entity.GetComponent<Troops>().attackIndex <= allies.Length)
            {
                Debug.Log(entity.GetComponent<Troops>().attackIndex);
                attacking = true;
                entity.Attack();
                cooldown = entity.cooldown;
            }

            yield return null;
        }
    }  



}
