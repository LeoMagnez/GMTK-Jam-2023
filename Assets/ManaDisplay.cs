using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] manaTokens;
    public static ManaDisplay instance;
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
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateDisplay(int value)
    {
        Debug.Log("MANACHANGe + " + value);
        if(value > 6)
        {
            for (int i = 0; i < manaTokens.Length; i++)
            {


                    manaTokens[i].GetComponent<Renderer>().material.color = Color.red;

            }
            //active tout
            //tout en rouge
        }
        else
        {
            for (int i = 0; i < manaTokens.Length; i++)
            {
                if(i < value)
                {

                    manaTokens[i].GetComponent<Renderer>().material.color = Color.blue;
                }
                else
                {

                    manaTokens[i].GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
    }
}
