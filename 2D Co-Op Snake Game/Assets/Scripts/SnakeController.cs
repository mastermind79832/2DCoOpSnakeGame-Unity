using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnakeController : MonoBehaviour
{
    
    [Range(0,50)] public float speed;
    public Vector2 bounds;

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
            Vector3 pos = transform.position;
            pos += m_Direction;  
            CheckBoundary(ref pos);
            transform.position = pos;  
            m_MoveTimer = 0;
        }
        m_MoveTimer += Time.deltaTime;
    }
    private void CheckBoundary(ref Vector3 pos)
    {  
        if(Mathf.Abs(pos.x) >= bounds.x)
            pos.x *= -1;
        else if(Mathf.Abs(pos.y) >= bounds.y)
            pos.y *= -1;
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Fruit"))
        {
            FruitSpwanner.fruitInstance.SpawnNextFruit();
        }
    }
}
