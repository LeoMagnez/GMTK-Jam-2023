using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonsize : MonoBehaviour
{
    // Start is called before the first frame update
    bool mouseover;
    Vector3 originalsize;

    void Start()
    {
        originalsize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(mouseover)
        {
            transform.localScale = originalsize * 1.2f;
        }
        else
        {
            transform.localScale = originalsize;

        }
    }
    public void ChangeMouseOver(bool newstate)
    {
        mouseover = newstate;
    }
}
