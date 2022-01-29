using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayParameters", menuName = "Data/Gameplay Parameters")]
public class GameplayParameters : ScriptableObject
{
    [Tooltip("Cooldown time required between two consecutive switches (done from any button), to avoid epileptic flashes")]
    [Range(0f, 2f)]
    public float switchCooldownDuration = 1f;
}
