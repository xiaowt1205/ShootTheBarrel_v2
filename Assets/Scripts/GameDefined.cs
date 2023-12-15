using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class GameDefined
{
    public enum GameProcess
    {
        GameSet, GamePlaying, GamePause, GameOver
    }

    public enum CharactorType
    {
        [Description("Human")] Human,
        [Description("Archer")] Archer,
        [Description("Swat")] Swat
    };

    public enum ZombieType
    {
        [Description("ZombieM")] ZombieM,
        [Description("ZombieF")] ZombieF,
        [Description("Runner")] Runner
    };

    public enum ItemType
    {
        [Tooltip("PlayerController")][Description("AddPeople")] AddPeople,
        [Tooltip("PlayerController")][Description("LevelUp")] LevelUp,
        [Tooltip("Weapon")][Description("WeaponMoveSpeed")] WeaponMoveSpeed,
        [Tooltip("Weapon")][Description("WeaponDamage")] WeaponDamage,
        [Tooltip("Weapon")][Description("WeaponColdDown")] WeaponColdDown,
    }

    public static int GetItemTypeCount()
    {
        return Enum.GetNames(typeof(CharactorType)).Length - 1;
    }

    // Resources Path
    public const string CHARACTOR_PATH = "Charactors/";
    public const string WEAPON_PATH = "Weapon/";
    public const string ZOMBIE_PATH = "Zombie/";

}
