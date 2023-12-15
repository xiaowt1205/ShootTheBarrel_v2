using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Weapon : MonoBehaviour, IGetBuff
{
    [SerializeField] private WeaponDataSO CurrentWeapontData;
    [HideInInspector] public float MoveSpeed;
    [HideInInspector] public float Damage;
    [HideInInspector] public float ColdDown;
    public GameObject BulletPrefab;
    [HideInInspector] public GameObject BulletModel;
    private float MoveSpeedParam, DamageParam, ColdDownParam;
    private List<Transform> ShootPoints = new();
    private bool IsSetting;
    private float Timer;
    private GameDefined.CharactorType CurrentCharactorType;
    private ObjectPool<Bullet> BulletPool;

    void OnEnable()
    {
        PlayerController.OnCharactorUpdate += CharactorUpdate;
    }

    void OnDisable()
    {
        PlayerController.OnCharactorUpdate -= CharactorUpdate;
    }

    void Start()
    {
        ResetParam();
        SetWeaponData(GameDefined.CharactorType.Human);
        BulletPool = ObjectPool<Bullet>.instance;
        BulletPool.InitPool(BulletPrefab);
    }

    void Update()
    {
        if (IsSetting) return;
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Shoot();
            ResetTimer();
        }
    }

    void Shoot()
    {
        foreach (var point in ShootPoints)
        {
            if (point != null)
                BulletPool.Spawn(point);
        }
    }

    void SetWeaponData(GameDefined.CharactorType charactor)
    {
        CurrentCharactorType = charactor;

        CurrentWeapontData = GetWeaponDataFormSO(charactor);
        MoveSpeed = CurrentWeapontData.MoveSpeed;
        Damage = CurrentWeapontData.Damage;
        ColdDown = CurrentWeapontData.ColdDown;
        BulletModel = CurrentWeapontData.Model;

        SetWeaponWithParam();
        ResetTimer();
        UpdateShootPoints();
    }

    void SetWeaponWithParam()
    {
        MoveSpeed *= MoveSpeedParam;
        Damage *= DamageParam;
        ColdDown *= ColdDownParam;
    }

    WeaponDataSO GetWeaponDataFormSO(GameDefined.CharactorType charactor)
    {
        return Resources.Load<WeaponDataSO>($"{GameDefined.WEAPON_PATH}{charactor.GetDescriptionText()}");
    }

    void UpdateShootPoints()
    {
        ShootPoints.Clear();

        GameObject[] points = GameObject.FindGameObjectsWithTag("ShootPoint");

        foreach (var point in points)
        {
            ShootPoints.Add(point.transform);
        }
    }

    void CharactorUpdate(GameDefined.CharactorType type)
    {
        IsSetting = true;
        SetWeaponData(type);
        UpdateShootPoints();
        IsSetting = false;
    }

    void ResetTimer() => Timer = ColdDown;

    void ResetParam()
    {
        MoveSpeedParam = 1;
        DamageParam = 1;
        ColdDownParam = 1;
    }

    public void GetBuff(GameDefined.ItemType type)
    {
        switch (type.GetDescriptionText())
        {
            case "WeaponMoveSpeed":
                BuffWeaponMoveSpeed();
                break;
            case "WeaponDamage":
                BuffWeaponDamage();
                break;
            case "WeaponColdDown":
                BuffWeaponColdDown();
                break;
            default:
                break;
        }

        SetWeaponWithParam();
    }

    void BuffWeaponMoveSpeed()
    {
        MoveSpeedParam += 0.2f;
    }

    void BuffWeaponDamage()
    {
        DamageParam += 1;
    }

    void BuffWeaponColdDown()
    {
        ColdDownParam -= 0.2f;
    }
}
