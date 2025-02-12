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
    private bool _wasLegGrabbingBox;

    public bool equipGun;
    
    // Layer 0: Body Layer
    [SerializeField] static readonly int bodyIdleClip = Animator.StringToHash("BODY Idle");
    [SerializeField] static readonly int bodyWalkClip = Animator.StringToHash("BODY Walk");

    [SerializeField] static readonly int bodyGrabBoxClip = Animator.StringToHash("BODY Grab Box");
    [SerializeField] static readonly int bodyDropBoxClip = Animator.StringToHash("BODY Drop Box");

    [SerializeField] static readonly int bodyEquipGun = Animator.StringToHash("BODY Take Gun Out");
    [SerializeField] static readonly int bodyUnequipGun = Animator.StringToHash("Body Gun Put Back");

    // Layer 1: Legs Layer
    [SerializeField] static readonly int legIdleClip = Animator.StringToHash("LEGS Idle");
    [SerializeField] static readonly int legWalkClip = Animator.StringToHash("LEGS Walk");

    [SerializeField] static readonly int legGrabBoxClip = Animator.StringToHash("LEGS Grab Box");
    [SerializeField] static readonly int legDropBoxClip = Animator.StringToHash("LEGS Drop Box");


    Vector3 _prevPos;
    int _currBodyState;
    int _currLegState;
    float _bodyLockedTill;
    float _legLockedTill;
    float _distance;

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
        if (Time.time < _bodyLockedTill) return _currBodyState;

        // Priorities
        if(equipGun) return bodyEquipGun;
        else if(!equipGun && _currBodyState == bodyEquipGun) return LockState(bodyUnequipGun, 1);

        else if(isGrabbingBox) return bodyGrabBoxClip;
        else if(!isGrabbingBox && _currBodyState == bodyGrabBoxClip) return LockState(bodyDropBoxClip, 1.33f);

        else if(_distance >= walkThreshold)return bodyWalkClip;
        else return bodyIdleClip;

        int LockState(int s, float t)
        {
            _bodyLockedTill = Time.time + t;
            return s;
        }
    }
    int GetLegState() {
        if (Time.time < _legLockedTill) return _currLegState;

        // Priorities
        if(isGrabbingBox && !_wasLegGrabbingBox)
        {
            _wasLegGrabbingBox = true;
            return LockState(legGrabBoxClip, 1.33f);
        }
        else if(!isGrabbingBox && _wasLegGrabbingBox)
        {
            _wasLegGrabbingBox = false;
            return LockState(legDropBoxClip, 1.33f);
        }

        else if (_distance >= walkThreshold) return legWalkClip;
        else return legIdleClip;

        int LockState(int s, float t)
        {
            _legLockedTill = Time.time + t;
            return s;
        }
    }
}
