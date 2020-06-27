using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public AudioSource _efxSource;

    public static AudioManager Instance => _instance;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }

    public void RandomClip(params AudioClip[] Clip)
    {
        int clipIndex = Random.Range(0, Clip.Length);
        _efxSource.clip = Clip[clipIndex];
        _efxSource.pitch = Random.Range(0.9f, 1.1f);
        _efxSource.Play();
    }

}
