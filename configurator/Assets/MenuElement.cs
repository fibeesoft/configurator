using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuElement:MonoBehaviour
{
    public string ElementName { get; }
    public string Description { get; }
    public float Price { get;}

    public MenuElement(string _description, float _price)
    {
        Description = _description;
        Price = _price;
    }

    public MenuElement(string _elementName)
    {
        ElementName = _elementName;
    }
}
