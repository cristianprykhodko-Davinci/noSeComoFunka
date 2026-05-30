using UnityEngine;
using System;
using Unity.VisualScripting;


public class AudioManager : Singleton<AudioManager>
{
    public static Action<SoundStimulus> OnSoundGenerated;

    public void GenerateSound(Vector3 position, float radius, float intensity)

    {
        SoundStimulus stimulus = new SoundStimulus(position, radius, intensity);
        Debug.Log("AudioManager generó sonido en: " + position);
        OnSoundGenerated?.Invoke(stimulus);


    }
}
