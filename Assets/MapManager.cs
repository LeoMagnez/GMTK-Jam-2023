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

    [SerializeField] public GameObject currentTemplate;
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
        GameObject temp = Instantiate(templates[Random.Range(0, templates.Length)],new Vector3(0, 0f, -3f),Quaternion.identity);
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
        showRoom.SetActive(true);
        //currentTemplate.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        //animation de salle qui s'affiche pour poser les cartes
        currentRoomInEdit.cardHolder.SetActive(true);
        
        HandManager.instance.gameObject.SetActive(true);
        HandManager.instance.Init();
        ready = true;
    }
    IEnumerator ExitRoom()
    {
        ready = false;
        //showRoom.SetActive(false);

        currentRoomInEdit.cardHolder.SetActive(false);
        HandManager.instance.Close();
        HandManager.instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        //animation de salle qui s'affiche pour poser les cartes
        showRoom.SetActive(false);

        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inMap)
        {
            currentTemplate.transform.localPosition = Vector3.Lerp(currentTemplate.transform.localPosition, new Vector3(0, 0, 0),Time.deltaTime*10);
            showRoom.transform.localPosition = Vector3.Lerp(showRoom.transform.localPosition, new Vector3(0, 10, 0), Time.deltaTime*10);

        }
        else
        {
            currentTemplate.transform.localPosition = Vector3.Lerp(currentTemplate.transform.localPosition, new Vector3(0, -2, 0), Time.deltaTime*10);
            showRoom.transform.localPosition = Vector3.Lerp(showRoom.transform.localPosition, new Vector3(0, 0.033f, 0), Time.deltaTime*10);

        }
    }
}
