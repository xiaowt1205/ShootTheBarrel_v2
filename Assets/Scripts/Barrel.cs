using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Barrel : MonoBehaviour, IHurtable
{
    [SerializeField] private GameDefined.ItemType ItemType;
    [SerializeField] private TMP_Text HPText;
    private int Hp;
    [SerializeField] private float MoveSpeed;
    private bool IsSetting;

    public void SetBarrel(GameDefined.ItemType type, int hp)
    {
        IsSetting = true;
        ItemType = type;
        Hp = hp;
        UpdateHpText();
        IsSetting = false;
    }

    void Update()
    {
        if (IsSetting) return;
        transform.Translate(MoveSpeed * Time.deltaTime * Vector3.forward);

    }

    void UpdateHpText() => HPText.text = Hp.ToString();

    public void GetDamage(float damage)
    {
        Hp -= (int)damage;
        UpdateHpText();

        if (Hp <= 0)
        {
            SendBuff();
            ObjectPool<Barrel>.instance.Recycle(this);
        }
    }

    void SendBuff()
    {
        switch (ItemType.GetEnumTooltip())
        {
            case "PlayerController":
                PlayerController playController = FindObjectOfType<PlayerController>();
                playController.GetBuff(ItemType);
                break;

            case "Weapon":
                Weapon weapon = FindObjectOfType<Weapon>();
                weapon.GetBuff(ItemType);
                break;

            default:
                break;
        }
    }

}
