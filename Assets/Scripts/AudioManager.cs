using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource ambianceSource;
    [SerializeField] AudioSource playerSFXSource;
    [SerializeField] AudioSource generalSFXSource;
    [SerializeField] AudioSource bossSFXSource;

    public AudioClip ambiance;
    public AudioClip stepsSFX;
    public AudioClip jumpSFX;
    public AudioClip pickupSFX;
    public AudioClip btnGeneralSFX;
    public AudioClip btnExitSFX;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ambianceSource.clip = ambiance ;
        ambianceSource.Play();
    }

    public void PlayBtnGeneralSFX()
    {
        generalSFXSource.PlayOneShot(btnGeneralSFX);
    }

    public void PlayBtnExitSFX()
    {
        generalSFXSource.PlayOneShot(btnExitSFX);
    }

    public void PlaySteptsSFX()
    {
        playerSFXSource.clip = stepsSFX;
        playerSFXSource.Play();
    }

    public void StartJump()
    {
        playerSFXSource.Pause();
    }
    public void EndJump()
    {
        playerSFXSource.PlayOneShot(jumpSFX);
        playerSFXSource.clip = stepsSFX;
        playerSFXSource.Play();
    }

    public void PlayPickUp()
    {
        generalSFXSource.PlayOneShot(pickupSFX);
    }
}
