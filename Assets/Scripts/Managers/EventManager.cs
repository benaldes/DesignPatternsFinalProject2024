using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Interfaces;
using UnityEngine.Events;

public enum EventType
{
    ItemPickedUp,
    DamageEvent,
    EnemyDeathEvent,
    PlayerDeathEvent,
    PlayerSpawnEvent,
    EnemySpawnEvent,
    GameWon,
    GameLost
}

public class EventManager : PersistentSingletonMonoBehaviour<EventManager>
{
    private Dictionary<EventType, List<IEventListener>> _eventListeners;

    private List<EventMessage> _allEventsSentEver;

    private bool inited = false;
    
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        _eventListeners = new Dictionary<EventType, List<IEventListener>>();
        _allEventsSentEver = new List<EventMessage>();
        inited = true;
    }

    public void Subscribe(EventType type, IEventListener listener)
    {
        if (!inited) 
            Initialize();
        if (!_eventListeners.ContainsKey(type)) 
            _eventListeners.Add(type, new List<IEventListener>());
        _eventListeners[type].Add(listener);
    }
    
    public void Unsubscribe(EventType type, IEventListener listener)
    {
        if (!inited) 
            Initialize();
        if (!_eventListeners.ContainsKey(type)) 
            return;
        _eventListeners[type].Remove(listener);
        if (_eventListeners[type].Count == 0)
            _eventListeners.Remove(type);
    }

    public void RaiseEvent(EventMessage eventMessage)
    {
        foreach (var listener in _eventListeners[eventMessage._eventType])
        {
            listener.OnEventReceived(eventMessage);
        }
        _allEventsSentEver.Add(eventMessage);
        Debug.Log($"Raised event {eventMessage._eventType} from sender: {eventMessage._senderID} with {_eventListeners[eventMessage._eventType].Count} listeners");
    }

    private void OnApplicationQuit()
    {
        CacheEvents();
        return;

        //Cache all events sent during runtime
        void CacheEvents()
        {
            
        }
    }
}
