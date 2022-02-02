using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpwanner : MonoBehaviour
{
    private static FruitSpwanner m_FruitInstance;
    public static FruitSpwanner fruitInstance {get {return m_FruitInstance;}}

    public Transform fruit;
    public BoxCollider2D spawnArea;
    public float spawnInterval;
    private float m_SpawnTimer;

    void Awake()
    {
        m_FruitInstance = this;
    }
    
    void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (m_SpawnTimer > spawnInterval)
        {
            SpawnNextFruit();
        }
        m_SpawnTimer += Time.deltaTime;
    }

    public void SpawnNextFruit()
    {
        Vector3 newPos = GetRandomPos();
        fruit.position = newPos;
        m_SpawnTimer = 0;
    }

    private Vector3 GetRandomPos()
    {
        Vector2 area = spawnArea.size;
        Vector3 pos;
        pos.x = Mathf.Round( Random.Range(- area.x/2, area.x/2));
        pos.y = Mathf.Round( Random.Range(- area.y/2, area.y/2));
        pos.z = 0;
        return pos;
    }
}
