using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDescriptorData", menuName = "ScriptableObjects/CardDescriptor", order = 1)]
public class CardDescriptor : ScriptableObject
{
    public Sprite cardImage;
    public string description;
}
