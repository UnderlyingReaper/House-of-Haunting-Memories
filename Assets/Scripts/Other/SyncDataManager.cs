using UnityEngine;

public class SyncDataManager : MonoBehaviour
{
    public static SyncDataManager Instance;

    [SerializeField] private bool didSeeNews;
    [SerializeField] private bool hasRope;
    [SerializeField] private bool didSeeSophiaDie;

    public bool DidSeeNews {
        get { return didSeeNews; }
        set { didSeeNews = value; }
    }

    public bool HasRope {
        get { return hasRope; }
        set { hasRope = value; }
    }

    public bool DidSeeSophiaDie {
        get { return didSeeSophiaDie; }
        set { didSeeSophiaDie = value; }
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log($"Duplicate {gameObject.name} Destroyed");
        }
    }
}