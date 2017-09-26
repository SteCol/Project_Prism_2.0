using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    public List<Sprite> symbols;
    public GameObject imagePrefab;
    public GameObject symbolPanel;
}

public class _Storage
{
    public static Storage Storage()
    {
        return GameObject.FindGameObjectWithTag("GameController").GetComponent<Storage>();
    }

    public static Sprite GetSymbols(int symbolToGet)
    {
        return Storage().symbols[symbolToGet];
    }
}

