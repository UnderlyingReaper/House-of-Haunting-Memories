using UnityEngine;

public class ArrestJoshCondition : MonoBehaviour, IObjectiveCondition
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector2 boxSize;
    private Transform _player;



    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool IsConditionMet()
    {
        if (Physics2D.OverlapBox(CalculateBoxPosition(), boxSize, 0f, LayerMask.GetMask("Josh")))
        {
            return true;
        }
        else return false;
    }

    private Vector3 CalculateBoxPosition()
    {
        if(_player.localScale.x == 1)
            return _player.position + offset;
        else
        {
            return _player.position + new Vector3(-offset.x, offset.y, offset.z);
        }
    }

    private void OnDrawGizmos()
    {
        if (_player == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(CalculateBoxPosition(), new Vector3(boxSize.x, boxSize.y, 0));
    }
}