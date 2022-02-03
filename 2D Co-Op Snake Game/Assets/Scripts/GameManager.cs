using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_ManagerInstance;
    public static GameManager managerInstance {get { return s_ManagerInstance; }}

    private BoxCollider2D m_collider;

    // private Bounds s_bounds;

    void Awake()
    {
        s_ManagerInstance = this;
        m_collider = GetComponent<BoxCollider2D>();
        SetBounds();
        displayBounds();
    }

    private void displayBounds()
    {
        Debug.Log("Bounds maxX : "+Bounds.maxX);
        Debug.Log("Bounds minX : "+Bounds.minX);
        Debug.Log("Bounds maxY : "+Bounds.maxY);
        Debug.Log("Bounds minY : "+Bounds.minY);
    }

    private void SetBounds()
    {
        Vector2 offset = m_collider.offset;
        Vector2 size = m_collider.size;
        Vector2 pos = transform.position;

        Bounds.maxX = pos.x + (size.x/2) + offset.x;
        Bounds.minX = pos.x - (size.x/2) + offset.x;

        Bounds.maxY = pos.y + (size.y/2) + offset.y;
        Bounds.minY = pos.y - (size.y/2) + offset.y;
    }

    // public Bounds GetBounds()
    // {
    //     return s_bounds;
    // }
}
