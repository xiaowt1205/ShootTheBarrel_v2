
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieDataSO", menuName = "Game/Zombie/Create New ZombieDataSO")]
public class ZombieDataSO : ScriptableObject
{
    public int HealthPoint;
    public float MoveSpeed;
    public GameObject Model;
    public int Probability;
}
