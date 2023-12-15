using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject BarrelPrefab;
    private ObjectPool<Barrel> BarrelPool;
    private readonly List<Vector3> SpawnPosition = new() { new(-3.5f, 0, 32f), new(0, 0, 32f), new(3.5f, 0, 32f) };

    [SerializeField] private float SpawnCD;
    private float Timer;

    void Start()
    {
        BarrelPool = ObjectPool<Barrel>.instance;
        BarrelPool.InitPool(BarrelPrefab);
        ResetTimer();
    }

    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            SpawnBarrel();
            ResetTimer();
        }
    }

    void SpawnBarrel()
    {
        transform.position = SpawnPosition[Random.Range(0, SpawnPosition.Count)];
        Barrel barrel = BarrelPool.Spawn(transform);
        int itemType = Random.Range(0, GameDefined.GetItemTypeCount());
        int hp = (GameManager.Instance.GetGameLevel * 10) + Random.Range(0, 9);
        barrel.SetBarrel((GameDefined.ItemType)itemType, hp);
    }

    void ResetTimer() => Timer = SpawnCD;

}
