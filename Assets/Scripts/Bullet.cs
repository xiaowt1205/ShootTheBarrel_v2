using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Weapon Weapon;
    private float MoveSpeed;
    private float Damage;
    private bool IsSettingBulletData;
    private GameObject BulletModel;

    void OnEnable()
    {
        IsSettingBulletData = true;

        if (Weapon == null) Weapon = FindObjectOfType<Weapon>();
        if (MoveSpeed != Weapon.MoveSpeed) MoveSpeed = Weapon.MoveSpeed;
        if (Damage != Weapon.Damage) Damage = Weapon.Damage;
        if (BulletModel != Weapon.BulletModel)
        {
            if (transform.childCount != 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            BulletModel = Weapon.BulletModel;
            Instantiate(BulletModel, transform);
        }

        IsSettingBulletData = false;
    }

    void Update()
    {
        if (IsSettingBulletData) return;
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hit"))
        {
            if (other.TryGetComponent<IHurtable>(out IHurtable target))
            {
                target.GetDamage(Damage);
            }
            ObjectPool<Bullet>.instance.Recycle(this);
        }
    }
}
