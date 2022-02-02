using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnakeController : MonoBehaviour
{
    
    [Range(0,50)] public float speed;

    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.A;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightkey = KeyCode.D;

    private Vector3 m_Direction = Vector3.right;
    private Rigidbody2D m_rigidBody;
    private float m_MoveTimer = 0;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {     
        GetSnakeDirection();
        MoveSnake();
    }
    void FixedUpdate()
    {
    }

    private void GetSnakeDirection()
    {
        if(Input.GetKeyDown(upKey))
            m_Direction = Vector3.up;
        else if(Input.GetKeyDown(leftKey))
            m_Direction = Vector3.left;
        else  if(Input.GetKeyDown(rightkey))
            m_Direction = Vector3.right;
        else if(Input.GetKeyDown(downKey))
            m_Direction = Vector3.down;
    }

    private void MoveSnake()
    {
        if(m_MoveTimer > 1/speed)
        {
            transform.position += m_Direction;  
            m_MoveTimer = 0;
        }
        m_MoveTimer += Time.deltaTime;
    }
}
