using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ZombiePrefab;
    private List<ZombieDataSO> ZombieData = new();
    private List<ZombieDataSO> Probability = new();
    private bool IsDataReady;
    [SerializeField] private float SpawnCD;
    private float Timer;
    private ObjectPool<Zombie> ZombiePool;


    void Awake() => StartCoroutine(LoadZombieData());

    void Start()
    {
        ResetTimer();
        ZombiePool = ObjectPool<Zombie>.instance;
        ZombiePool.InitPool(ZombiePrefab);
    }

    void Update()
    {
        if (!IsDataReady) return;
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            SpawnZombie();
            ResetTimer();
        }
    }

    void SpawnZombie()
    {
        transform.position = new Vector3(Random.Range(-4.2f, 4.2f), transform.position.y, transform.position.z);
        Zombie zombie = ZombiePool.Spawn(transform);
        zombie.SetDataSO(Probability[Random.Range(0, Probability.Count)]);
    }

    void ResetTimer() => Timer = SpawnCD;

    IEnumerator LoadZombieData()
    {
        IsDataReady = false;
        ZombieDataSO[] resources = Resources.LoadAll<ZombieDataSO>($"{GameDefined.ZOMBIE_PATH}");
        ZombieData = new List<ZombieDataSO>(resources);
        yield return SetProbability();
    }

    IEnumerator SetProbability()
    {
        foreach (var data in ZombieData)
        {
            int probability = data.Probability;

            for (int i = 0; i < probability; i++)
            {
                Probability.Add(data);
            }
        }
        IsDataReady = true;
        yield return null;
    }
}
