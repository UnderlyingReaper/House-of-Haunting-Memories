using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class RemoveHouseTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> enableGameObjectList;
    [SerializeField] private List<GameObject> disableGameObjectList;
    [SerializeField] private AudioSource rain, fire;

    [SerializeField] private float initialDelay;
    [SerializeField] private float delay = 0;
    [SerializeField] private Collider2D colliderToAssign;
    


    private CinemachineCamera _currVirtualCam;
    private CinemachineConfiner2D _virtualCamConfinner;


    private void Start()
    {
        CinemachineCore.CameraActivatedEvent.AddListener(OnCameraActivated);
        GetVirtualCameraIfPossible();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!enabled) return;
        if(other.tag == "Player") StartCoroutine(StartTrigger());
    }

    private IEnumerator StartTrigger()
    {
        foreach(GameObject gameObject in disableGameObjectList)
        {
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        foreach(GameObject gameObject in enableGameObjectList)
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }        

        StartCoroutine(ChangeConfiner());
        enabled = false;

        float _orgRainVol = rain.volume;
        rain.volume = 0;
        rain.DOFade(_orgRainVol, 3);

        float _orgFireVol = fire.volume;
        fire.volume = 0;
        fire.DOFade(_orgFireVol, 3);
    }

    private IEnumerator ChangeConfiner()
    {
        float orgVal = _virtualCamConfinner.SlowingDistance;

        yield return new WaitForSeconds(initialDelay);

        _virtualCamConfinner.SlowingDistance = 0;
        _virtualCamConfinner.BoundingShape2D = colliderToAssign;
        _virtualCamConfinner.InvalidateBoundingShapeCache();

        yield return new WaitForSeconds(delay);

        DOVirtual.Float(_virtualCamConfinner.SlowingDistance, orgVal, 1, value => { _virtualCamConfinner.SlowingDistance = value; });
    }

    void GetVirtualCameraIfPossible()
    {
        _currVirtualCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineCamera;
        if(_currVirtualCam != null) _virtualCamConfinner = _currVirtualCam.GetComponent<CinemachineConfiner2D>();
    }

    private void OnCameraActivated(ICinemachineCamera.ActivationEventParams arg0)
    {
        _currVirtualCam = arg0.IncomingCamera as CinemachineCamera;
        _virtualCamConfinner = _currVirtualCam.GetComponent<CinemachineConfiner2D>();
    }
}