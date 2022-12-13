using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfigScriptableObject", order = 1)]
public class GameConfigScriptableObject : ScriptableObject
{
    public GameObject initialStructures;
    public float secondsInDay;
}
