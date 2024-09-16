using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public abstract class EventMessage
{
    public EventType _eventType { get; private set; }
    public int _senderID { get; private set; }

    protected EventMessage(EventType type, int senderID)
    {
        _eventType = type;
        _senderID = senderID;
    }
}

public class PickupEvent : EventMessage
{
    public IPickupAble _pickupAble { get; private set; }

    public PickupEvent(int senderID, IPickupAble pickupAble) : base(EventType.ItemPickedUp, senderID)
    {
        _pickupAble = pickupAble;
    }
}

public class DamageEvent : EventMessage
{
    public IEventSender _killer { get; private set; }
    public int _damage { get; private set; }
    public IDamageAble _target { get; private set; }

    public DamageEvent(int senderID, int damage, IDamageAble target, IEventSender killer) : base(EventType.DamageEvent, senderID)
    {
        _damage = damage;
        _target = target;
        _killer = killer;
    }
}

public abstract class DeathEvent : EventMessage
{
    public IEventSender _killer { get; private set; }
    public IKillAble _killedEntity { get; private set; }
    public DeathEvent(int senderID, EventType eventType, IKillAble killedEntity) : base(eventType, senderID)
    {
        _killedEntity = killedEntity;
    }
}

public class PlayerDeathEvent : DeathEvent
{
    public PlayerDeathEvent(int senderID, IKillAble killedEntity) : base(senderID, EventType.PlayerDeathEvent, killedEntity)
    {
    }
}

public class EnemyDeathEvent : DeathEvent
{
    public EnemyDeathEvent(int senderID, IKillAble killedEnemy) : base(senderID, EventType.EnemyDeathEvent, killedEnemy)
    {
    }
}

public class PlayerSpawnedEvent : EventMessage
{
    public PlayerController _playerController;
    public Vector3 _spawnPosition;

    public PlayerSpawnedEvent(int senderID, PlayerController playerController, Vector3 spawnPosition) : base(EventType.PlayerSpawnEvent, senderID)
    {
        _playerController = playerController;
        _spawnPosition = spawnPosition;
    }
}

public class VictoryEvent : EventMessage
{
    public VictoryEvent(int senderID) : base(EventType.GameWon, senderID)
    {
        
    }
}

