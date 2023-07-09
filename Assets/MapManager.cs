using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    [SerializeField] bool ready = false;
    [SerializeField] bool inMap = true;
    [SerializeField] bool validSave = true;
    [SerializeField] GameObject[] templates;
    [SerializeField] GameObject showRoom;
    [SerializeField] public Room currentRoomInEdit;

    [SerializeField] GameObject currentTemplate;
    // Start is called before the first frame update
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
        gameObject.SetActive(false);
    }

    void ChooseMap()
    {
        Debug.Log(templates.Length);
        GameObject temp = Instantiate(templates[Random.Range(0, templates.Length)],new Vector3(0.13f, 0f, -3f),Quaternion.identity);
        RoomManager.instance.ChangeCurrentRoom(temp.GetComponent<Template>().StartRoom);
        currentTemplate = temp.transform.GetChild(0).gameObject;

        //animation de spawn de l'étage
        //showRoom.SetActive(false);

        //for(int i = 0; i <  templates.Length; i++)
        //{
        //    templates[i].SetActive(false);

        //}
        //currentTemplate.SetActive(true);
    }
    void Start()
    {
        ChooseMap();
    }

    public void SwitchView()
    {
        if(currentRoomInEdit.mana < 0)
        {

            return;
        }
        if (ready)
        {
            inMap = !inMap;
            if (!inMap)
            {
                StartCoroutine(IntoRoom());
            }
            else
            {
                StartCoroutine(ExitRoom());
            }
        }



    }

    IEnumerator IntoRoom()
    {
        ready = false;
        currentTemplate.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        //animation de salle qui s'affiche pour poser les cartes
        currentRoomInEdit.cardHolder.SetActive(true);
        //showRoom.SetActive(true);
        HandManager.instance.gameObject.SetActive(true);
        HandManager.instance.Init();
        ready = true;
    }
    IEnumerator ExitRoom()
    {
        ready = false;
        //showRoom.SetActive(false);

        currentRoomInEdit.cardHolder.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        //animation de salle qui s'affiche pour poser les cartes
        currentTemplate.SetActive(true);
        HandManager.instance.Close();
        HandManager.instance.gameObject.SetActive(false);
        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
