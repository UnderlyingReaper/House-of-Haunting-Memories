using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class RemoveHouseTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> enableGameObjectList;
    [SerializeField] private List<GameObject> disableGameObjectList;

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
        if(other.tag == "Player")
        {
            foreach(GameObject gameObject in enableGameObjectList)
            {
                gameObject.SetActive(true);
            }

            foreach(GameObject gameObject in disableGameObjectList)
            {
                gameObject.SetActive(false);
            }

            StartCoroutine(ChangeConfiner());
            enabled = false;
        }
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