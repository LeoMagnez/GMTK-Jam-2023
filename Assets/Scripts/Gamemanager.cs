using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public GameObject paladin;
    public CardData[] deck;
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
                
                while (HandManager.instance.GetNumberOfCard() < 7) {
                    HandManager.instance.DrawCard(deck[Random.Range(0,deck.Length)]);
                }
                NextStep();
                break; 
            case Phase.Map: 
                //activer le Map manager
                MapManager.instance.gameObject.SetActive(true);


                break;  
            case Phase.Fight:
                //FightManager
                //activer le paladin
                paladin.SetActive(true);
                FightManager.instance.gameObject.SetActive(true);

                RoomManager.instance.currentRoom.EnterRoom();
                MapManager.instance.currentTemplate.SetActive(false);
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
        
    }
    // Start is called before the first frame update
    void Start()
    {
        NextStep();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
