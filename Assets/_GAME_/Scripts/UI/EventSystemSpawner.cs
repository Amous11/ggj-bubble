using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ
{
    public class EventSystemSpawner : MonoBehaviour
    {
        void OnEnable()
        {
            EventSystem sceneEventSystem = FindObjectOfType<EventSystem>();
            if (sceneEventSystem == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");

                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
        }
    }
}