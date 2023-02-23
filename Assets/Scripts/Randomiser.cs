using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomiser : SerializedMonoBehaviour
{
    public Dictionary<GameObject, int> randomiserDictionary = new Dictionary<GameObject, int>();

    public List<GameObject> randomiserList = new List<GameObject>();

    public bool onAwake;

    void Start()
    {
        foreach (KeyValuePair<GameObject, int> item in randomiserDictionary)
        {
            for (int i = 0; i < item.Value; i++)
            {
                randomiserList.Add(item.Key);
            }
        }

        if (onAwake)
        {
            Randomise();
        }
    }

    public GameObject Randomise()
    {
        if(randomiserList.Count > 0)
        {
            Debug.Log("Randomising!");
            return randomiserList[Random.Range(0, randomiserList.Count)];
        }
        else
        {
            Debug.Log("Randomiser Error! List Not Populated!");
            return null;
        }
    }
}
