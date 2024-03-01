using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private int maxVolume;
    [SerializeField] private int minVolume;
    private FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;

    public enum HubTracks
    {
        DEFAULT,
        EMBRODYLE,
        MSPRETTY,
        PEARCE
    }
    private HubTracks currentHubCharacterTrack;

    private void Awake()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
    }

    public void PlaySceneTrack()
    {
        instance.start();
        instance.setParameterByName("Embrodyle Volume", minVolume);
        instance.setParameterByName("Pearce Volume", minVolume);
        instance.setParameterByName("MsPretty Volume", minVolume);
        instance.setParameterByName("Default Volume", maxVolume);
    }

    public IEnumerator ChangeHubCharacterTrack(HubTracks track)
    {
        string outTrackParameter = currentHubCharacterTrack.ToString() + " Volume";
        string inTrackParameter = track.ToString() + " Volume";
        int volumeIn = 1;
        int volumeOut = maxVolume - 2;
        while (volumeIn <= maxVolume)
        {
            if (volumeIn >= maxVolume / 2)
            {
                instance.setParameterByName(outTrackParameter, volumeOut);
                volumeOut--;
            }
            instance.setParameterByName(inTrackParameter, volumeIn);
            volumeIn++;

            yield return new WaitForSeconds(0.05f);
        }
        currentHubCharacterTrack = track;
    }

}