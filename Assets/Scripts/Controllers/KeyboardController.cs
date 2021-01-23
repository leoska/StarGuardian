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
        // Vertical Movement
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);

        // Horizontal Movement
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveRight = Input.GetKey(KeyCode.D);

        bool fire = Input.GetKey(KeyCode.Space);

        if (Player != null)
        {

            if (moveUp || moveDown || moveLeft || moveRight)
            {
                float xPosition = -Convert.ToInt32(moveLeft) + Convert.ToInt32(moveRight);
                float yPosition = -Convert.ToInt32(moveDown) + Convert.ToInt32(moveUp);
                Vector3 Directional = new Vector3(xPosition, yPosition, 0);

                Player.PlayerMove(Directional);
            }

            if (fire)
            {
                Player.PlayerAttack();
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
