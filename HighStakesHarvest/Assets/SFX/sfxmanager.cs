using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class sfxmanager : MonoBehaviour
{
    Button sfxButton;
    AudioSource audioData;
    public AudioClip cliplist;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         sfxButton = GetComponent<Button>();
        audioData = GetComponent<AudioSource>();
        sfxButton.onClick.AddListener(audioData.Play);
    }

    // Update is called once per frame
    void PlayRoundRobin()
    {   
        audioData.pitch = Random.Range(0.8f, 1.3f);
        audioData.PlayOneShot(cliplist);
    }
}
