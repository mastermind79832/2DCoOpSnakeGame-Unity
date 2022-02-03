using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class SnakeController : MonoBehaviour
{ 
    [Range(0,50)] public float speed;

    [Header("Body Config")]
    public GameObject bodyPart;
    public int initialBodyCount;

    [Header("Key Config")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.A;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightkey = KeyCode.D;

    private Vector3 m_Direction;
    private Rigidbody2D m_rigidBody;
    private List<Transform> m_body;
    private float m_MoveTimer = 0;
    private bool m_IsVertical, m_IsDead, m_Immunity;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_rigidBody.bodyType = RigidbodyType2D.Kinematic;
        m_IsDead = false;
        InitializeBody();
    }

    private void InitializeBody()
    {
        transform.position = transform.parent.position;
        m_Direction = Vector3.right;
        m_body = new List<Transform>();
        StartCoroutine(SetImmunity(0.5f));
        m_body.Add(this.transform);
        for (int i = 0; i < initialBodyCount; i++)
        {
            AddNewBodyPart();
        }
    }
    private IEnumerator SetImmunity(float timer)
    {
        m_Immunity = true;
        yield return new WaitForSeconds(timer);
        m_Immunity = false;
    }

    private void Update() 
    { 
        if(m_IsDead)
            return;    
        GetSnakeDirection();
        MoveSnake();  
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
        //Bounds Bounds = GameManager.managerInstance.GetBounds(); 
        if(pos.x > Bounds.maxX || pos.x < Bounds.minX)
            pos.x =((pos.x > 0)?Bounds.minX:Bounds.maxX);
        else if(pos.y > Bounds.maxY || pos.y < Bounds.minY)
            pos.y =((pos.y > 0)?Bounds.minY:Bounds.maxY);
    }

    private void AddNewBodyPart()
    {
        int bodyCount =m_body.Count;
        Vector3 lastPart ;
        lastPart = m_body[bodyCount-1].position; //- Vector3.one; 
        GameObject newPart = Instantiate<GameObject>(bodyPart,lastPart,Quaternion.identity);
        newPart.name = string.Format("body {0}",bodyCount);
        m_body.Add(newPart.transform);
        newPart.transform.parent = transform.parent;
        SetScale();
    }

    private void SetScale()
    {
        int bodyCount = m_body.Count;
        float decrement = 0.3f;
        for (int i = 1; i < 5; i++)
        {  
            if(bodyCount-i == 1)
                return;
            Vector3 scale = m_body[bodyCount - i].localScale;
            scale.x = 0.7f - decrement;
            scale.y = 0.7f - decrement;
            m_body[bodyCount - i].localScale = scale;
            decrement -= 0.1f;
        }
    }

    private void PlayAgain()
    {
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        m_IsDead = true;
        float waitTime = 0.1f;
        for (int i = m_body.Count-1; i > 0; i--)
        {
            Destroy(m_body[i].gameObject,waitTime);
            waitTime += 0.05f;
        }
        yield return new WaitForSeconds(waitTime);
        m_body.Clear();
        InitializeBody();
        m_IsDead = false;
    }
    private void DestoryLastBody()
    {
        Destroy(m_body[m_body.Count- 1].gameObject);
        m_body.RemoveAt(m_body.Count- 1);
        SetScale();
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
            if(m_body.Count > 3)
                FruitSpwanner.fruitInstance.PoisonActivation(true);
        }

        if(other.CompareTag("Poison"))
        {
            Destroy(other.gameObject);
            int count = FruitSpwanner.fruitInstance.SnakeAtePoison();
            for (int i = 0; i < count; i++)
            {
                DestoryLastBody();
            }
            if(m_body.Count < 3)
                FruitSpwanner.fruitInstance.PoisonActivation(false);
        }

        if(other.CompareTag("Body") && !m_Immunity)
        {
            Debug.Log("Player Dead");
            PlayAgain();
            FruitSpwanner.fruitInstance.PoisonActivation(false);
        }
    }

}
