using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnakeController : MonoBehaviour
{
    
    [Range(0,50)] public float speed;
    public Vector2 bounds;
    public GameObject bodyPart;
    public int initialBodyCount;

    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.A;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightkey = KeyCode.D;

    private Vector3 m_Direction = Vector3.right;
    private Rigidbody2D m_rigidBody;
    private List<Transform> m_body;
    private float m_MoveTimer = 0;
    private bool m_IsVertical;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_body = new List<Transform>();
        m_body.Add(this.transform);
        InitializeBody();
    }

    private void InitializeBody()
    {
        for (int i = 0; i < initialBodyCount; i++)
        {
            AddNewBodyPart();
        }
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
        if(Input.GetKeyDown(upKey) && !m_IsVertical)
        {
            m_Direction = Vector3.up;
            m_IsVertical = true;
        }
        else if(Input.GetKeyDown(leftKey) && m_IsVertical)
        {   
             m_Direction = Vector3.left;
            m_IsVertical = false;
        }
        else  if(Input.GetKeyDown(rightkey) && m_IsVertical)
        {   
            m_Direction = Vector3.right;
            m_IsVertical = false;
        }
        else if(Input.GetKeyDown(downKey) && !m_IsVertical)
        {   
            m_Direction = Vector3.down;  
            m_IsVertical = true; 
        }
    }

    private void MoveSnake()
    {
        if (m_MoveTimer > 1 / speed)
        {
            MoveBody();
            MoveHead();
        }
        m_MoveTimer += Time.deltaTime;
    }

    private void MoveBody()
    {
        int bodyCount = m_body.Count;
        for (int i = bodyCount - 1; i > 0; i--)
        {
            m_body[i].position = m_body[i-1].position;
        }
    }

    private void MoveHead()
    {
        Vector3 pos = transform.position;
        pos += m_Direction;
        CheckBoundary(ref pos);
        transform.position = pos;
        m_MoveTimer = 0;
    }

    private void CheckBoundary(ref Vector3 pos)
    {  
        if(Mathf.Abs(pos.x) >= bounds.x)
            pos.x =(pos.x *-1) + ((pos.x < 0)?-1:1);
        else if(Mathf.Abs(pos.y) >= bounds.y)
            pos.y = (pos.y *-1) + ((pos.y < 0)?-1:1);
    }

    private void AddNewBodyPart()
    {
        int bodyCount =m_body.Count;
        Vector3 lastPart ;
        lastPart = m_body[bodyCount-1].position - Vector3.one; 
        GameObject newPart = Instantiate<GameObject>(bodyPart,lastPart,Quaternion.identity);
        newPart.name = string.Format("body {0}",bodyCount);
        m_body.Add(newPart.transform);
        newPart.transform.parent = transform.parent;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Fruit"))
        {
            int count = FruitSpwanner.fruitInstance.SnakeAteFruit();
            for (int i = 0; i < count; i++)
            {
                AddNewBodyPart();
            }
        }
        if(other.CompareTag("Body"))
        {
            Debug.Log("Player Dead");
        }
    }

}
