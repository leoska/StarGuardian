using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTouch : MonoBehaviour
{
    public Player Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = Player == null ? GetComponent<Player>() : Player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (App.Instance.gameController.state == GameController.GameState.Game)
            Player.PlayerAttack();
    }
}
