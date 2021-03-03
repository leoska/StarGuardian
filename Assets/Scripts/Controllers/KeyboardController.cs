using System;
using UnityEngine;

public class KeyboardController
{

    public Player Player;

    // UpdateKeyboard
    public void UpdateKeyboard()
    {
        // Vertical Movement
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);

        // Horizontal Movement
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveRight = Input.GetKey(KeyCode.D);

        bool fire = Input.GetKey(KeyCode.Space);

        bool shield = Input.GetKey(KeyCode.Z);

        bool rocket = Input.GetKey(KeyCode.X);

        bool dodge = Input.GetKey(KeyCode.C);
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

            if (shield)
            {
                Player.PlayerShield();
            }

            if (rocket)
            {
                Player.PlayerRocket();
            }

            if (dodge)
            {
                Player.PlayerDodge();
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
