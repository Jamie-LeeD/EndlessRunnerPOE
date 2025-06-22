using UnityEngine;
//requires unity authentication
using Unity.Services.Core;
using Unity.Services.Authentication;


public class UGSInitializer : MonoBehaviour
{
    async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}

