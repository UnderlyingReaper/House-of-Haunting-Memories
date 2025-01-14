using UnityEngine;

public class ChatIcon : MonoBehaviour, IIconBehaviour
{
    [SerializeField] private GameObject chatHistroy;

    public void Run()
    {
        chatHistroy.SetActive(true);
    }
}