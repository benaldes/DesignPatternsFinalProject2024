using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageAble, IEventListener, IEventSender
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int hp = 1;
    [SerializeField] private int damageDealt = 1;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        SenderID = IDProvider.GetID();
        
        EventManager.Instance.Subscribe(EventType.DamageEvent, this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.TryGetPlayerPosition(out Vector2 directionToPlayer))
        {
            rb2d.velocity = directionToPlayer * moveSpeed;
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && TryGetComponent(typeof(IDamageAble), out var player))
        {
            SendEvent(new DamageEvent(SenderID, damageDealt, (IDamageAble)player, this));
        }
    }

    int IDamageAble.Health { get => hp; set => hp = value; }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void OnDamage(int damageTaken)
    {
        Debug.Log($"{gameObject.name} #{SenderID} was damaged.");
        return;
    }

    public void Die()
    {
        SendEvent(new EnemyDeathEvent(SenderID, this));
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe(EventType.DamageEvent, this);
    }
    
    public int SenderID { get; set; }

    public void SendEvent(EventMessage eventMessage)
    {
        EventManager.Instance.RaiseEvent(eventMessage);
    }

    public void OnEventReceived(EventMessage eventMessage)
    {
        switch (eventMessage._eventType)
        {
            case EventType.DamageEvent:
                DamageEvent damageEvent = (DamageEvent)eventMessage;
                if ((EnemyController)damageEvent._target == this)
                {
                    ((IDamageAble)this).TakeDamage(damageEvent._damage);
                }
                break;
        }
    }
}
