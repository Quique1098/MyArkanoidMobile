using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // Singleton para facilitar el acceso 

    [Header("Static Sounds")]
    public AudioClip staticSound1; // Primer sonido estático
    public AudioClip staticSound2; // Segundo sonido estático

    [Header("Audio Clips")]
    public AudioClip releaseBallSound;
    public AudioClip ballCollisionSound;
    public AudioClip blockHitSound;
    public AudioClip blockDestroyedSound;
    public AudioClip powerUpPickupSound;
    public AudioClip lifeLostSound;
    public AudioClip ProyectilePowerUpSound;
    public AudioClip PaletPowerUpPowerUpSound;


    private AudioSource audioSource;

    public AudioSource musicAudioSource;
    public AudioSource soundEffectAudioSource;

    public float musicVolume = 1f; // 1f = volumen máximo
    public float soundEffectVolume = 1f; // 1f = volumen máximo

    // Sliders para ajustar el volumen
    public Slider musicVolumeSlider;
    public Slider soundEffectVolumeSlider;


    private void OnEnable()
    {
        GameEvents.OnReleaseBall += OnReleaseBall;
        GameEvents.OnBallCollision += OnBallCollision;
        GameEvents.OnBlockHit += OnBlockHit;
        GameEvents.OnBlockDestroyed += OnBlockDestroyed;
        GameEvents.OnPowerUpPickup += OnPowerUpPickup;
        GameEvents.OnLifeLost += OnLifeLost;
        GameEvents.OnShootProyectilePowerUp += OnShootProyectilePowerUp;
        GameEvents.OnPaletPowerUp += OnPaletPowerUp;

        
    }

    private void OnDisable()
    {
        GameEvents.OnReleaseBall -= OnReleaseBall;
        GameEvents.OnBallCollision -= OnBallCollision;
        GameEvents.OnBlockHit -= OnBlockHit;
        GameEvents.OnBlockDestroyed -= OnBlockDestroyed;
        GameEvents.OnPowerUpPickup -= OnPowerUpPickup;
        GameEvents.OnLifeLost -= OnLifeLost;
        GameEvents.OnShootProyectilePowerUp -= OnShootProyectilePowerUp;
        GameEvents.OnPaletPowerUp -= OnPaletPowerUp;

    }
    private void Awake()
    {
        // Implementación del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persistencia entre escenas

        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        //musicVolumeSlider.value = musicVolume;
        //soundEffectVolumeSlider.value = soundEffectVolume;

        // Asignar eventos para que el slider modifique el volumen
       // musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        //soundEffectVolumeSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
    }

    public void OnMusicVolumeChanged(float value)
    {
        musicVolume = value;
        musicAudioSource.volume = musicVolume;
    }

    // Cambiar volumen de los efectos de sonido
    public void OnSoundEffectVolumeChanged(float value)
    {
        soundEffectVolume = value;
        soundEffectAudioSource.volume = soundEffectVolume;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }



    // Métodos para suscribirse a los eventos
    public void OnReleaseBall() => PlaySound(releaseBallSound);
    public void OnBallCollision() => PlaySound(ballCollisionSound);
    public void OnBlockHit() => PlaySound(blockHitSound);
    public void OnBlockDestroyed() => PlaySound(blockDestroyedSound);
    public void OnPowerUpPickup() => PlaySound(powerUpPickupSound);
    public void OnLifeLost() => PlaySound(lifeLostSound);
    public void OnShootProyectilePowerUp() => PlaySound(ProyectilePowerUpSound);
    public void OnPaletPowerUp() => PlaySound(PaletPowerUpPowerUpSound);
    


}