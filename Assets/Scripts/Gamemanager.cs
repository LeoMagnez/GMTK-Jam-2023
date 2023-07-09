using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public GameObject paladin;
    enum Phase
    {
        Draw, Map, Fight

    }
    [SerializeField] Phase currentPhase = Phase.Fight;
    bool inEditRoom;

    public void NextStep() 
    {
        int n = (int)currentPhase;
        n++;
        if (n > 2) n = 0;
        currentPhase = (Phase)n;

        switch(currentPhase) { 
            case Phase.Draw:
                Debug.Log("PIOCHE");
                NextStep();
                break; 
            case Phase.Map: 
                //activer le Map manager
                MapManager.instance.gameObject.SetActive(true);


                break;  
            case Phase.Fight:
                //FightManager
                //activer le paladin
                FightManager.instance.gameObject.SetActive(true);
                MapManager.instance.gameObject.SetActive(false);

                break;
        
        }

    }


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
        NextStep();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
