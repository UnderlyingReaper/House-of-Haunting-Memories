using DG.Tweening;
using UnityEngine;

public class MenuMusicHandeler : MonoBehaviour
{
    public static MenuMusicHandeler Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            musicSource.DOFade(1, 3);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log($"Duplicate {this.name} Destroyed");
            Destroy(gameObject);
        }
    }

    public void GameStart(float time)
    {
        musicSource.DOFade(0, time).OnComplete(() => Destroy(gameObject));
    }
}