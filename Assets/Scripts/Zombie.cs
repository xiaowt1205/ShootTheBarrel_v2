using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour, IHurtable
{
    [HideInInspector] public ZombieDataSO Data;
    private int HealthPoint;
    private float MoveSpeed;
    private GameObject Model;
    private int LevelParam;
    private bool IsReady, IsDeath;
    [SerializeField] private ParticleSystem BloodVFX;

    public void SetDataSO(ZombieDataSO dataSO)
    {
        Data = dataSO;
        GenerateZombie();
    }

    void GenerateZombie()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject != BloodVFX.gameObject)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        LevelParam = GameManager.Instance.GetGameLevel;
        HealthPoint = Data.HealthPoint * LevelParam;
        MoveSpeed = Data.MoveSpeed * LevelParam;
        Model = Instantiate(Data.Model, transform);
        IsReady = true;
    }

    public void GetDamage(float damage)
    {
        HealthPoint -= (int)damage;
        PlayBloodVFX();

        if (HealthPoint <= 0)
        {
            IsDeath = true;
            Model.GetComponent<Animator>().SetBool("Death", true);
            Utilities.ActionDelay(() =>
            {
                ObjectPool<Zombie>.instance.Recycle(this);
            }, 1f);
        }
    }

    void PlayBloodVFX() => BloodVFX.Play();

    void Update()
    {
        if (!IsReady || IsDeath) return;
        transform.Translate(MoveSpeed * Time.deltaTime * Vector3.forward);
    }

    void OnDisable()
    {
        IsReady = false;
        IsDeath = false;
    }
}
