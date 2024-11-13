using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip gameOverMusic;
    public AudioClip mainMenuMusic;
    public static AudioManager instance;
    private AudioSource audioSource; // Background music
    private AudioSource collectibleAudioSource; // For collectible sounds

    public List<AudioClip> collectibleSounds; // Sequential sounds
    public AudioClip wrongCollectibleSound;    // Sound for wrong pickups
    private int collectibleSoundIndex = 0;     // Tracks the current sound index

    // Play next collectible sound using the second AudioSource
    public void PlayNextCollectibleSound()
    {
        if (collectibleSounds.Count == 0) return;

        collectibleAudioSource.clip = collectibleSounds[collectibleSoundIndex];
        collectibleAudioSource.Play();

        // Advance to the next sound, loop back to the start if needed
        collectibleSoundIndex = (collectibleSoundIndex + 1) % collectibleSounds.Count;
    }

    // Play the wrong collectible sound and reset sequence using the second AudioSource
    public void PlayWrongCollectibleSound()
    {
        collectibleAudioSource.clip = wrongCollectibleSound;
        collectibleAudioSource.Play();
        collectibleSoundIndex = 0; // Reset sequence
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;              // Exit to avoid setting audioSource for a destroyed object
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on AudioManager GameObject.");
        }

        // Add a second AudioSource for collectible sounds
        collectibleAudioSource = gameObject.AddComponent<AudioSource>();
        if (collectibleAudioSource == null)
        {
            Debug.LogError("Failed to create collectibleAudioSource.");
        }
    }

    void Start()
    {
        // Play the music based on the initial scene
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    void OnEnable()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the music whenever a new scene is loaded
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        // Stop any music currently playing with a fade out
        FadeOutAndPlayNewMusic(GetMusicClipForScene(sceneName), 1f);
    }

    AudioClip GetMusicClipForScene(string sceneName)
    {
        // Return the music clip based on the scene name
        switch (sceneName)
        {
            case "Level1":
                return level1Music;
            case "Level2":
                return level2Music;
            case "GameOverScene":
                return gameOverMusic;
            case "MainMenu":
                return mainMenuMusic;
            default:
                return null;  // No music for this scene
        }
    }

    // Fade out the current music, then play new music
    public void FadeOutAndPlayNewMusic(AudioClip newClip, float fadeDuration = 1f)
    {
        if (newClip == null)
            return;

        StartCoroutine(FadeOutCoroutine(newClip, fadeDuration));
    }

    private IEnumerator FadeOutCoroutine(AudioClip newClip, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        // Fade out the current music
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;

        // Change the clip and fade it in
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in the new music
        while (audioSource.volume < 1)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
