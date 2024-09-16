using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>, IEventListener
{
    public PlayerController currentPlayer { get; private set; }

    public bool TryGetPlayerPosition(out Vector2 playerPosition)
    {
        playerPosition = Vector2.zero;
        if (currentPlayer == null) 
            return false;
        
        playerPosition = currentPlayer.transform.position;
        return true;
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.Subscribe(EventType.GameWon, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void OnEventReceived(EventMessage eventMessage)
    {
        switch (eventMessage._eventType)
        {
            case EventType.PlayerSpawnEvent:
                break;
            case EventType.GameWon:
                RestartGame();
                break;
            case EventType.GameLost:
                Application.Quit(); //It's a hardcore game
                break;
        }
    }
}
