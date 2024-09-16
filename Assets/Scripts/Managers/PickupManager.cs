using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// I know I should've used an object pool, but I ran out of development time :P
/// </summary>
public class PickupManager : SingletonMonoBehaviour<PickupManager>, IEventListener
{
    [SerializeField] private List<Pickup> _pickupPrefabs;
    private Dictionary<int, Pickup> _pickupsByID;

    protected override void Awake()
    {
        base.Awake();
        _pickupsByID = new Dictionary<int, Pickup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.Subscribe(EventType.ItemPickedUp, this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time % 1 == 0)
        {
            Pickup newPickup = Instantiate(_pickupPrefabs[Random.Range(0, _pickupPrefabs.Count)]);
            newPickup.pickupID = IDProvider.GetID();
            _pickupsByID.Add(newPickup.pickupID, newPickup);
            newPickup.transform.position = new Vector2(Random.Range(-8f, 8f), Random.Range(-5f, 5f));
            newPickup.pickupType = (PickupType)Random.Range(0, 2);
            newPickup.OnPickedUp(newPickup.pickupType);
        } 
    }

    public void OnEventReceived(EventMessage eventMessage)
    {
        if (eventMessage._eventType == EventType.ItemPickedUp)
        {
            PickupEvent pickupEvent = (PickupEvent)eventMessage;
            if (_pickupsByID.ContainsKey(pickupEvent._pickupAble.pickupID))
            {
                _pickupsByID[pickupEvent._senderID].OnPickedUp(pickupEvent._pickupAble.pickupType);
                _pickupsByID.Remove(pickupEvent._senderID);
            }
        }
    }
}
