using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private Dictionary<GameEvents, List<IGameListener>> listeners = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        // This component shares a GameObject with the scene managers. DontDestroyOnLoad
        // on that object would keep their old scene references alive after a level change.
        bool attachedToSceneManagers = GetComponent<GameManager>() != null
            || GetComponent<PickUpManager>() != null
            || GetComponent<BossManager>() != null;

        if (attachedToSceneManagers)
        {
            GameObject host = new GameObject("EventManager");
            host.AddComponent<EventManager>();
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    /// <summary>
    /// Registers a new lister for a specific event
    /// </summary>
    /// <param name="eventType">The event to listen for</param>
    /// <param name="listener">The listener to register</param>
    public void AddListener(GameEvents eventType, IGameListener listener)
    {
        if (!IsAlive(listener)) return;

        if (!listeners.TryGetValue(eventType, out var listenList))
        {
            listenList = new List<IGameListener>();
            listeners[eventType] = listenList;
        }

        if (!listenList.Contains(listener))
            listenList.Add(listener);
    }

    /// <summary>
    ///  Post a notification to all listeners of the specified event type.
    /// </summary>
    /// <param name="eventType">Event to invoke</param>
    /// <param name="sender">The parent invoking the event</param>
    /// <param name="param">Optional event data</param>
    public void Invoke(GameEvents eventType, Component sender, object param = null)
    {
        if (!listeners.TryGetValue(eventType, out var listenList)) return;

        for (int i = listenList.Count - 1; i >= 0; i--)
        {
            IGameListener listener = listenList[i];
            if (!IsAlive(listener))
            {
                listenList.RemoveAt(i);
                continue;
            }

            listener.OnEvent(eventType, sender, param);
        }
    }

    /// <summary>
    /// Removes a listener from an event
    /// </summary>
    /// <param name="eventType">Event to stop listening too</param>
    /// <param name="listener">Listener to remove</param>
    public void RemoveListener(GameEvents eventType, IGameListener listener)
    {
        if (listeners.TryGetValue(eventType, out var listenList))
        {
            listenList.Remove(listener);

            if (listenList.Count == 0)
                listeners.Remove(eventType);
        }
    }

    public void Clear()
    {
        listeners.Clear();
    }

    private static bool IsAlive(IGameListener listener)
    {
        if (listener == null)
            return false;

        // A destroyed Unity object stored as an interface is not C# null.
        if (listener is Object unityObject && unityObject == null)
            return false;

        return true;
    }
}
