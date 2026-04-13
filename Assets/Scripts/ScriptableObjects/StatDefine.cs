using UnityEngine;

[CreateAssetMenu(fileName = "StatDefine", menuName = "Scriptable Objects/StatDefine")]
public class StatDefine : ScriptableObject
{
    [Header("Identity")]
    public string statId;
    public string displayName;

    [Header("Value Scaling")]
    public float baseValuePerLevel = 10f;

    [Header("Upgrade Cost")]
    public int baseUpgradeCost = 50;
    public int costIncreasePerLevel = 25;
}