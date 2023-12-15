using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Game/Weapon/Create New WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    public GameObject Model;
    public float MoveSpeed;
    public float Damage;
    public float ColdDown;
}
