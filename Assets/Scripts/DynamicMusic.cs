using UnityEngine;

public class DynamicMusic : MonoBehaviour
{
    public AudioClip peacefulMusic;
    public AudioClip combatMusic;

    [Range(0f, 1f)] public float maxVolume = 0.5f;
    public float combatDuration = 15f;
    public float crossfadeSpeed = 2f;

    private AudioSource peacefulSource;
    private AudioSource combatSource;
    private float combatTimer = 0f;

    public static DynamicMusic instance;

    void Awake()
    {
        instance = this;

        peacefulSource = gameObject.AddComponent<AudioSource>();
        combatSource = gameObject.AddComponent<AudioSource>();

        peacefulSource.loop = true;
        combatSource.loop = true;

        peacefulSource.clip = peacefulMusic;
        combatSource.clip = combatMusic;

        peacefulSource.volume = maxVolume;
        combatSource.volume = 0f;

        peacefulSource.Play();
        combatSource.Play();
    }

    void Update()
    {
        if (combatTimer > 0)
        {
            combatTimer -= Time.deltaTime;
        }

        float targetCombatVol = (combatTimer > 0) ? maxVolume : 0f;
        float targetPeacefulVol = (combatTimer > 0) ? 0f : maxVolume;

        combatSource.volume = Mathf.Lerp(combatSource.volume, targetCombatVol, Time.deltaTime * crossfadeSpeed);
        peacefulSource.volume = Mathf.Lerp(peacefulSource.volume, targetPeacefulVol, Time.deltaTime * crossfadeSpeed);
    }

    public void TriggerCombatMusic()
    {
        combatTimer = combatDuration; // Reset timer to 15s
    }
}