using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageAble, IPickupConsumer, IEventListener, IEventSender
{
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private int _health;

    private Rigidbody2D rb2d;
    private float damageCooldown = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        SenderID = IDProvider.GetID();
        
        EventManager.Instance.Subscribe(EventType.DamageEvent, this);
    }

    private void Update()
    {
        if (damageCooldown > 0)
        {
            damageCooldown -= Time.deltaTime;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = new Vector2(0, 0);
        if (Input.GetAxis("Vertical") != 0f)
        {
            movement += (Vector2.up * (Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime));
        }
        if (Input.GetAxis("Horizontal") != 0f)
        {
            movement += (Vector2.right * (Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime));
        }

        rb2d.velocity = movement;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(typeof(IPickupAble), out var pickup))
        {
            ConsumePickup((IPickupAble)pickup);
        }
    }
    
    public void ConsumePickup(IPickupAble pickupConsumed)
    {
        SendEvent(new PickupEvent(SenderID, pickupConsumed));
        ApplyPickup(pickupConsumed.pickupType);
    }

    private void ApplyPickup(PickupType pickupType)
    {
        switch (pickupType)
        {
            case PickupType.SpeedBoost:
                movementSpeed *= 1.2f;
                break;
        }
    }

    public int Health
    {
        get => _health;
        set => _health = value;
    }

    public void TakeDamage(int damage)
    {
        if (damageCooldown <= 0f)
        {
            Health -= damage;
            damageCooldown = 1f;
        }

        if (Health <= 0)
        {
            Die();
        }
    }

    public void OnDamage(int damageTaken)
    {
        Debug.Log($"{gameObject.name} #{SenderID} was damaged.");
    }

    public void Die()
    {
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
                if ((PlayerController)damageEvent._target == this)
                {
                    ((IDamageAble)this).TakeDamage(damageEvent._damage);
                }
                break;
        }
    }
}