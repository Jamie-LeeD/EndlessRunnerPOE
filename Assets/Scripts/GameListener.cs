using UnityEngine;

public interface IGameListener
{

    //Function to invoke events,
    //we also pass in an object incase we want any parameters
    void OnEvent(GameEvents eventType, Component sender, object param = null);

}

