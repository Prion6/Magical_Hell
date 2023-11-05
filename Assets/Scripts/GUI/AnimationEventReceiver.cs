using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public void CallChangeSceneEvent(string sceneName)
    {
        // Llama al evento est�tico de GameManager aqu�.
        GameManager.Instance.ChangeScene(sceneName);
    }
}
