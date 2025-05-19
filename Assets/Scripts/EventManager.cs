using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //Should make it a singleton for easy access
    public static EventManager Instance;


    //we can use a list, but dictionary works more efficiently
    private Dictionary<GameEvents, List<IGameListener>> listeners = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    /// <summary>
    /// Registers a new lister for a specific event
    /// </summary>
    /// <param name="eventType">The event to listen for</param>
    /// <param name="listener">The listener to register</param>
    public void AddListener(GameEvents eventType, IGameListener listener)
    {
        //basic null check
        if (listener == null) return;


        if (!listeners.TryGetValue(eventType, out var listenList)) //will try find something, will store found events in out var listn list
        {
            listenList = new List<IGameListener>();
            listeners[eventType] = listenList;
        }

        //check if there is not already a duplicate
        if (!listenList.Contains(listener))
        {
            listenList.Add(listener);
        }
    }

    /// <summary>
    ///  Post a notification to all listeners of the specified event type.
    /// </summary>
    /// <param name="eventType">Event to invoke</param>
    /// <param name="sender">The parent invoking the event</param>
    /// <param name="param">Optional event data</param>
    public void Invoke(GameEvents eventType, Component sender, object param = null) //Envokes it
    {

        //this is shorthand for both returning if there are no listeners, 
        // AND if there are store them in a variable called listenList
        if (!listeners.TryGetValue(eventType, out var listenList)) return;




        //when we invoke, we go through all the listeners (in reverse order incase we remove any along the way)
        for (int i = listenList.Count - 1; i >= 0; i--)
        {
            listenList[i]?.OnEvent(eventType, sender, param);
        }
    }

    /// <summary>
    /// Removes a listener from an event
    /// </summary>
    /// <param name="eventType">Event to stop listening too</param>
    /// <param name="listener">Listener to remove</param>
    public void RemoveListener(GameEvents eventType, IGameListener listener)
    {
        //again check if there are listeners, if there are, store in listenList variables
        if (listeners.TryGetValue(eventType, out var listenList))
        {
            listenList.Remove(listener);

            //if its the last listener, remove the event
            if (listenList.Count == 0)
            {
                listeners.Remove(eventType);
            }
        }
    }


    public void Clear()
    {
        //fresh start
        listeners.Clear();
    }

    
}
