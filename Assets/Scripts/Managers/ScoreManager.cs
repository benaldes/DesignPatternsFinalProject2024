using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IEventListener, IEventSender
{
    [SerializeField] private TMP_Text ScoreText;

    private int _score;
    // Start is called before the first frame update
    void Start()
    {
        SenderID = IDProvider.GetID();
        
        EventManager.Instance.Subscribe(EventType.ItemPickedUp, this);
        EventManager.Instance.Subscribe(EventType.EnemyDeathEvent, this);
    }

    public void OnEventReceived(EventMessage eventMessage)
    {
        switch (eventMessage._eventType)
        {
            case EventType.EnemyDeathEvent:
                _score += 50;
                break;
            case EventType.ItemPickedUp when ((PickupEvent)eventMessage)._pickupAble.pickupType == PickupType.ScoreBoost100:
                _score += 100;
                break;
        }
        
        ScoreText.text = _score.ToString();

        if (_score >= 1000)
        {
            SendEvent(new VictoryEvent(SenderID));
        }
    }

    public int SenderID { get; set; }

    public void SendEvent(EventMessage eventMessage)
    {
        EventManager.Instance.RaiseEvent(eventMessage);
    }
}
