using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class Pickup : MonoBehaviour, IPickupAble
{
    [SerializeField]
    private PickupType _pickupType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int pickupID { get; set; }
    public PickupType pickupType { get; set; }

    public void OnPickedUp(PickupType type)
    {
        Debug.Log($"Alas, I, {gameObject.name} #{pickupID} was picked up as {type}. 'Tis the end of me. Adios.");
        Destroy(gameObject);
    }
}