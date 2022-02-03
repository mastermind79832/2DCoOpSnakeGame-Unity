using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpwanner : MonoBehaviour
{
    private static FruitSpwanner s_FruitInstance;
    public static FruitSpwanner fruitInstance {get {return s_FruitInstance;}}
    
    [Header("Fruit")]
    public Transform fruit;
    public int fruitValue;
    public float FruitSpawnInterval;

    [Header("Poison")]
    public Transform poisonPrefab;
    public int poisonValue;
    public float poisonSpawnInterval;
    public float poisonStayTime;

    [Header("Spawn Properties")]
    private float[] m_SpawnTimer = new float[2];
    private bool m_PosionEnable = false;

    void Awake()
    {
        s_FruitInstance = this;
        fruit = Instantiate(fruit.gameObject).transform;
        fruit.parent = transform;
    }
    
    void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (m_SpawnTimer[0] > FruitSpawnInterval)
        {
            SpawnNextFruit();
        }
        m_SpawnTimer[0] += Time.deltaTime;

        if(!m_PosionEnable)
            return;
        
        if (m_SpawnTimer[1] > poisonSpawnInterval)
        {
            SpawnNextPoison();
        }
        m_SpawnTimer[1] += Time.deltaTime;
    }

    public int SnakeAtePoison()
    {
        SpawnNextPoison();
        return poisonValue;
    }

    private void SpawnNextPoison()
    {
        Vector3 newPos = GetRandomPos();
        GameObject poisonInstance = Instantiate(poisonPrefab,newPos,Quaternion.identity).gameObject;
        poisonInstance.transform.parent = transform.parent;
        Destroy(poisonInstance,poisonStayTime);
        m_SpawnTimer[1] = 0;
    }

    public int SnakeAteFruit()
    {
        SpawnNextFruit();
        return fruitValue;
    }

    public void SpawnNextFruit()
    {
        Vector3 newPos = GetRandomPos();
        fruit.position = newPos;
        m_SpawnTimer[0] = 0;
    }

    public void PoisonActivation(bool value)
    {
        poisonPrefab.gameObject.SetActive(value);
        m_PosionEnable = value;
    }

    private Vector3 GetRandomPos()
    {
        //Bounds Bounds = GameManager.managerInstance.GetBounds();
        Vector3 pos;
        pos.x = Mathf.Round( Random.Range(Bounds.minX, Bounds.maxX));
        pos.y = Mathf.Round( Random.Range(Bounds.minY, Bounds.maxY));
        pos.z = 0;
        return pos;
    }
}
