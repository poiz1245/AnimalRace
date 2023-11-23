using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Kart Definition", menuName = "Scriptable Object/Kart Definition")]
public class KartDefinition : ScriptableObject
{
    public const int Max_Stat = 5;

    public KartMove prefab;

    [SerializeField, Range(1, Max_Stat)] private int speedStat;
    [SerializeField, Range(1, Max_Stat)] private int accelStat;
    [SerializeField, Range(1, Max_Stat)] private int turnStat;

    public float SpeedStat => (float)speedStat / Max_Stat;
    public float AccelStat => (float)accelStat / Max_Stat;
    public float TurnStat => (float)turnStat / Max_Stat;
}
