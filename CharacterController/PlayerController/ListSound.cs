using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSound : MonoBehaviour, IPlayedRandomSound
{
    [SerializeField]
    private List<AudioClip> _soundClips;

    [SerializeField]
    private bool _listenSoundEverySecond;
    [SerializeField]
    private float _second;

    [SerializeField]
    [Range(0f, 1f)]
    private float _volume = 1f;

    private float _time;
    private bool _isPlaying;

    private float _soundLength;
    private AudioClip _lastPlayedClip;

    private AudioSource _audioSource;

[SerializeField]
    private List<int> _randomDigits = new List<int>();

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (!_isPlaying && _listenSoundEverySecond)
        {
            _time += Time.deltaTime;
            if (_time >= _second)
            {
                PlaySound();
                _time = 0;
            }
        }
    }

    public void PlaySound()
{
    if (_isPlaying)
    {
        return;
    }
    if (_soundClips.Count == 0)
    {
        Debug.LogWarning("No sound clips loaded.");
        return;
    }

    Vector2 randomDirection = Random.insideUnitCircle.normalized;
    int randomIndex = Mathf.RoundToInt(Mathf.Repeat(randomDirection.x, 1f) * _soundClips.Count);


    randomIndex = (randomIndex + _soundClips.Count) % _soundClips.Count;

    AudioClip clip = _soundClips[randomIndex];

   
    while (clip == _lastPlayedClip)
    {
        randomDirection = Random.insideUnitCircle.normalized;
        randomIndex = Mathf.RoundToInt(Mathf.Repeat(randomDirection.x, 1f) * _soundClips.Count);
        randomIndex = (randomIndex + _soundClips.Count) % _soundClips.Count;
        clip = _soundClips[randomIndex];
    }
    
    _randomDigits.Add(randomIndex);

    _audioSource.PlayOneShot(clip, _volume);
    _soundLength = clip.length;
    _lastPlayedClip = clip;

    StartCoroutine(WaitForSoundToFinish());
}


    IEnumerator WaitForSoundToFinish()
    {
        _isPlaying = true;
        yield return new WaitForSeconds(_soundLength + 1f);
        _isPlaying = false;
    }
}
