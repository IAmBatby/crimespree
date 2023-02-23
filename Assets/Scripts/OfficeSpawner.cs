using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeSpawner : MonoBehaviour
{
    public Randomiser randomiser;


    void Start()
    {
        GameObject office = randomiser.Randomise();
        GameObject officeObject = Instantiate(office, transform.position, office.transform.rotation);
        Debug.Log("Created" + officeObject);
        //officeObject.transform.SetParent(transform);
        officeObject.transform.position = transform.position;
    }
}
