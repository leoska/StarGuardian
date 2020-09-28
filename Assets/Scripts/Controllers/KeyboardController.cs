using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class KeyboardController : MonoBehaviour
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
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);

        if (Player != null)
        {

            if (moveUp || moveDown)
            {
                float xPosition = 0f;
                float yPosition = -Convert.ToInt32(moveDown) + Convert.ToInt32(moveUp);
                Vector3 Directional = new Vector3(xPosition, yPosition, 0);

                Player.PlayerMove(Directional);
            }
        }
        else
        {
            Player = FindObjectOfType<Player>();
        }
    }

    enum DirectionState
    {
        Up,
        Down,
        None,
    }

    enum MoveState
    {
        Idle,
        Walk,
    }
}
