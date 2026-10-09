using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager Instance;

    [SerializeField]
    GameObject ghostSheild;
    [SerializeField]
    Animator animator;
    [SerializeField]
    PlayerController playerController;
    public bool isGhost { get; set; }

    [System.Serializable]
    class Effect
    {
        public PickUpEffects type;
        public float duration;
        public float timeRemaining;
    }
    [SerializeField] List<Effect> activeEffects = new List<Effect>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            PickUpManager previous = Instance;
            Instance = this;
            if (previous.gameObject != gameObject)
                Destroy(previous.gameObject);
        }
        else
        {
            Instance = this;
        }

        if (activeEffects == null)
            activeEffects = new List<Effect>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void Start()
    {
        isGhost = false;
    }

    void Update()
    {
        if (activeEffects == null)
            return;

        List<Effect> expired = new List<Effect>();

        foreach (var statusEffect in activeEffects)
        {
            statusEffect.duration -= Time.deltaTime;
            statusEffect.timeRemaining -= Time.deltaTime;

            if (statusEffect.timeRemaining <= 0)
                ApplyEffect(statusEffect.type);

            if (statusEffect.duration <= 0)
                expired.Add(statusEffect);
        }

        foreach (Effect statusEffect in expired)
        {
            activeEffects.Remove(statusEffect);
            if (!HasEffect(statusEffect.type))
                EndEffect(statusEffect.type);
        }
    }

    public void AddEffect(PickUpEffects type, float duration)
    {
        if (activeEffects == null)
            activeEffects = new List<Effect>();

        Effect newEffect = new Effect();
        newEffect.type = type;
        newEffect.duration = duration;
        newEffect.timeRemaining = 0;
        activeEffects.Add(newEffect);

        if (EventManager.Instance != null)
            EventManager.Instance.Invoke(GameEvents.PICK_UP_ADDED, this, newEffect.type);
    }

    public void RemoveEffect(PickUpEffects type)
    {
        if (activeEffects == null)
            return;

        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (activeEffects[i].type == type)
            {
                activeEffects.RemoveAt(i);
                return;
            }
        }
    }

    private bool HasEffect(PickUpEffects type)
    {
        foreach (Effect effect in activeEffects)
        {
            if (effect.type == type)
                return true;
        }
        return false;
    }

    private void ApplyEffect(PickUpEffects type)
    {
        switch (type)
        {
            case PickUpEffects.TORCH:
                RenderSettings.fogDensity = 0.05f;
                break;
            case PickUpEffects.FUEL:
                if (playerController != null)
                    playerController.runSpeed = 15f;
                if (animator != null)
                    animator.SetBool("IsSprint", true);
                break;
            case PickUpEffects.GHOST:
                isGhost = true;
                if (ghostSheild != null)
                    ghostSheild.SetActive(true);
                break;
        }
    }

    private void EndEffect(PickUpEffects type)
    {
        switch (type)
        {
            case PickUpEffects.TORCH:
                RenderSettings.fogDensity = 0.10f;
                break;
            case PickUpEffects.FUEL:
                if (playerController != null)
                    playerController.runSpeed = 10f;
                if (animator != null)
                    animator.SetBool("IsSprint", false);
                break;
            case PickUpEffects.GHOST:
                if (ghostSheild != null)
                    ghostSheild.SetActive(false);
                isGhost = false;
                break;
        }
    }
}
