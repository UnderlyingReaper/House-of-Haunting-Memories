using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CrowFly : MonoBehaviour, CrowBehaviour
{
    [SerializeField] private Vector3 flyTo;
    [SerializeField] private float flySpeed = 1;
    [SerializeField] private float range = 1;

    public bool alerted;
    private Collider2D[] _colliders;


    private void Awake()
    {
        flyTo = new Vector3(transform.position.x + flyTo.x,
                            transform.position.y + flyTo.y,
                            transform.position.z);
    }
    public void UpdateFunc(Crow crow)
    {
        if(!enabled || alerted) return;

        _colliders = Physics2D.OverlapCircleAll(transform.position, range);

        foreach(Collider2D collider in _colliders)
        {
            if(collider.CompareTag("Player"))
            {
                alerted = true;
                StartCoroutine(Alert(crow));
                break;
            }
        }
    }

    public IEnumerator Alert(Crow crow)
    {
        enabled = false;
        alerted = true;

        yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));

        crow.animator.SetTrigger("Fly");

        crow.MakeCrowNoise();
        crow.StopCrowNoiseLoop();
        crow.CrowFlySound();

        foreach(Collider2D collider in _colliders)
        {
            if(collider.TryGetComponent(out CrowFly crowFly) && !crowFly.alerted)
                StartCoroutine(crowFly.Alert(crowFly.GetComponent<Crow>()));
        }

        if(transform.localScale.x == 1 && flyTo.x > transform.position.x) transform.localScale = new Vector3(-1, 1, 1);
        else if(transform.localScale.x == -1 && flyTo.x < transform.position.x) transform.localScale = new Vector3(1, 1, 1);

        transform.DOMove(flyTo, flySpeed).SetEase(Ease.InOutSine).OnComplete(() => Destroy(gameObject, 5));
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}