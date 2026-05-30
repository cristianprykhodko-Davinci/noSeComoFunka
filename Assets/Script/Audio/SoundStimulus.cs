using UnityEngine;



public struct SoundStimulus
{
    public Vector3 Position;
    public float Radius;
    public float Intensity;

    public SoundStimulus(Vector3 position, float radius, float intensity)
    {

        Position = position;
        Radius = radius;
        Intensity = intensity;

    }
}
