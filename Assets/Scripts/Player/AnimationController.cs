using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static AnimationController Instance { get; private set; }
    
    public Animator animator;
    [SerializeField] float transitionDuration;
    [SerializeField] float walkThreshold;

    [Header("States")]
    public bool isGrabbingBox;
    
    // Layer 0: Body Layer
    [SerializeField] static readonly int bodyIdleClip = Animator.StringToHash("BODY Idle");
    [SerializeField] static readonly int bodyWalkClip = Animator.StringToHash("BODY Walk");

    [SerializeField] static readonly int bodyGrabBoxClip = Animator.StringToHash("BODY Grab Box");
    [SerializeField] static readonly int bodyDropBoxClip = Animator.StringToHash("BODY Drop Box");

    // Layer 1: Legs Layer
    [SerializeField] static readonly int legIdleClip = Animator.StringToHash("LEGS Idle");
    [SerializeField] static readonly int legWalkClip = Animator.StringToHash("LEGS Walk");

    [SerializeField] static readonly int legGrabBoxClip = Animator.StringToHash("LEGS Grab Box");
    [SerializeField] static readonly int legDropBoxClip = Animator.StringToHash("LEGS Drop Box");


    Vector3 _prevPos;
    int _currBodyState;
    int _currLegState;
    float _lockedTill;
    float _distance;
    bool _override = false;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    void Start()
    {
        _prevPos = transform.position;
    }

    void Update()
    {
        if(_override) return;

        _distance = Mathf.Abs(transform.position.x - _prevPos.x);


        int bodyState = GetBodyState();
        if (bodyState != _currBodyState)
        {
            animator.CrossFade(bodyState, transitionDuration, 0);
            _currBodyState = bodyState;
        }

        int legState = GetLegState();
        if (legState != _currLegState)
        {
            animator.CrossFade(legState, transitionDuration, 1);
            _currLegState = legState;
        }
        
        
        _prevPos = transform.position;
    }

    int GetBodyState() {
        if (Time.time < _lockedTill) return _currBodyState;

        // Priorities
        if(isGrabbingBox) return bodyGrabBoxClip;
        else if(_distance >= walkThreshold)return bodyWalkClip;
        else return bodyIdleClip;
    }
    int GetLegState() {
        if (Time.time < _lockedTill) return _currLegState;

        // Priorities
        if (_distance >= walkThreshold) return legWalkClip;
        else return legIdleClip;
    }

    int LockState(int s, float t)
    {
        _lockedTill = Time.time + t;
        return s;
    }

    public IEnumerator GrabBoxOverride()
    {
        _override = true;
        animator.CrossFade(bodyGrabBoxClip, transitionDuration, 0);
        animator.CrossFade(legGrabBoxClip, transitionDuration, 1);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + 0.5f);

        isGrabbingBox = true;
        _override = false;
    }
    public IEnumerator DropBoxOverride()
    {
        _override = true;
        animator.CrossFade(bodyDropBoxClip, transitionDuration, 0);
        animator.CrossFade(legDropBoxClip, transitionDuration, 1);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + 0.5f);

        isGrabbingBox = false;
        _override = false;
    }
}
