using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public float movementSpeed = 1f;

    private void Update()
    {
        if(!gameObject.activeSelf) return;

        float horizontal = DevTools.instance.devControls.DevTools.Movement.ReadValue<Vector2>().x;
        float vertical = DevTools.instance.devControls.DevTools.Movement.ReadValue<Vector2>().y;

        Vector3 direction = new Vector3(horizontal, vertical, 0);

        transform.Translate(direction * movementSpeed * Time.deltaTime);
    }
}