using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinimapManager : SingletonMonoBehaviour<MinimapManager>
{
    

    private void Update()
    {
        GameManager.Instance.TryGetPlayerPosition(out Vector2 player);
        transform.position = new Vector3(player.x,player.y, - 10f);
    }
}