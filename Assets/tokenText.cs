using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tokenText : MonoBehaviour
{
    TextMeshProUGUI textrender;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Showtext(int index)
    {
        HandManager.instance.ShowToken(index);
    }
    public void HideText() 
    {
        textrender.SetText("");
    }
}
