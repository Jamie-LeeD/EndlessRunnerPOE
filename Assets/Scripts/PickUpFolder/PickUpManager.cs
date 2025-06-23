using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager Instance;

    [SerializeField]
    GameObject ghostSheild;
    [SerializeField]
    Animator animator;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    public bool isGhost {  get; set; }

    [Serializable]
    class Effect
    {
        public PickUpEffects type;
        public float duration;
        public float timeRemaining;
    }
    [SerializeField] List<Effect> activeEffects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGhost = false;
    }

    // Update is called once per frame
    void Update()
    {
        List<PickUpEffects> toBeRemoved = new List<PickUpEffects>();

        foreach (var statusEffect in activeEffects)
        {
            statusEffect.duration -= Time.deltaTime;
            statusEffect.timeRemaining -= Time.deltaTime;

            if (statusEffect.timeRemaining <= 0)
            {
                //apply status effect
                switch (statusEffect.type)
                {
                    case PickUpEffects.TORCH:
                        RenderSettings.fogDensity = 0.05f;
                        break;
                    case PickUpEffects.FUEL:
                        playerController.runSpeed = 15f;
                        animator.SetBool("IsSprint", true);
                        break;
                    case PickUpEffects.GHOST:
                        isGhost = true;
                        ghostSheild.SetActive(true);
                        break;
                    default:
                        break;
                }
            }

            if (statusEffect.duration <= 0)
            {
                switch (statusEffect.type)
                {
                    case PickUpEffects.TORCH:
                        RenderSettings.fogDensity = 0.10f;
                        break;
                    case PickUpEffects.FUEL:
                        playerController.runSpeed = 10f;
                        animator.SetBool("IsSprint", false);
                        break;
                    case PickUpEffects.GHOST:
                        ghostSheild.SetActive(false);
                        isGhost = false;
                        break;
                    default:
                        break;
                }
                toBeRemoved.Add(statusEffect.type);
            }
        }

        foreach (var statusType in toBeRemoved)
        {
            RemoveEffect(statusType);
        }
        toBeRemoved.Clear();
    }

    public void AddEffect(PickUpEffects type, float duration)
    {
        Effect newEffect = new Effect();
        newEffect.type = type;
        newEffect.duration = duration;
        newEffect.timeRemaining = 0;

        activeEffects.Add(newEffect);

        EventManager.Instance.Invoke(GameEvents.PICK_UP_ADDED, this, newEffect.type);
    }

    public void RemoveEffect(PickUpEffects type)
    {
        Effect toBeRemoved = null;

        foreach (Effect effect in activeEffects)
        {
            if (effect.type == type)
            {
                toBeRemoved = effect;
            }
        }
        if (toBeRemoved != null)
        {
            activeEffects.Remove(toBeRemoved);
        }
    }

}
