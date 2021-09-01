using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialExit : MonoBehaviour
{

    float time;
    TextMeshProUGUI text;
    void Start()
    {
        time = Time.time;
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(WaitKeyPress());
    }

    IEnumerator WaitKeyPress()
    {
        while(!Input.anyKeyDown)
        {
            if(Time.time - time > 1)
            {
                time = Time.time;
                text.enabled = !text.enabled;
            }
            yield return null;
        }
        FindObjectOfType<SceneLoader>().LoadGame();
    }
}
