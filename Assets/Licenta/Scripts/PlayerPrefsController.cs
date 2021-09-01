using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    public static void setLevel(int level)
    {
        PlayerPrefs.SetInt("Level", level);
    }

    public static int getLevel()
    {
        return PlayerPrefs.GetInt("Level");
    }

    public static void setVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public static float getVolume()
    {
        return PlayerPrefs.GetFloat("Volume");
    }

}
