using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool IsSlow { get { return Time.timeScale < 1 && Time.timeScale > 0; } }
    public static bool IsPaused { get { return Time.timeScale == 0; } }
}
