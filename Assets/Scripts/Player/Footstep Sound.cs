using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform checkPoint;
    [SerializeField] float checkRadius;
    [SerializeField] List<GroundAudioKeys> groundAudioKeysList;

    [System.Serializable]
    public struct GroundAudioKeys {
        public GroundMaterials material;
        public AudioClip audioClip;
    }

    GroundMaterials _currGroundMaterial;
    AudioClip _currAudioClip;
    AudioSource _audioSource;


    void Awake()
    {
        _currGroundMaterial = GroundMaterials.Concrete;
        _audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        Collider2D ground = Physics2D.OverlapCircle(checkPoint.position, checkRadius, groundMask);

        if(ground != null && ground.TryGetComponent(out GroundType groundType) && _currGroundMaterial != groundType.groundMaterial)
        {
            _currGroundMaterial = groundType.groundMaterial;

            foreach(GroundAudioKeys groundAudioKey in groundAudioKeysList)
            {
                if(groundAudioKey.material != _currGroundMaterial) continue;

                _currAudioClip = groundAudioKey.audioClip;
                break;
            }
        }
    }

    public void PlayFootStep()
    {
        _audioSource.volume = Random.Range(0.8f, 1.2f);
        _audioSource.pitch = Random.Range(0.85f, 1.2f);

        _audioSource.PlayOneShot(_currAudioClip);
    }
}
