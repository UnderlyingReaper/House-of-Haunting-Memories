using System.Collections.Generic;
using UnityEngine;

public class GameObjectsInitialize : MonoBehaviour, IObjectiveInitialize
{
    [SerializeField] private List<GameObject> activeGameObjectsList;
    [SerializeField] private List<GameObject> deactiveGameObjectsList;



    public void Initialize()
    {
        foreach(GameObject gameObject in activeGameObjectsList)
            gameObject.SetActive(true);

        foreach(GameObject gameObject in deactiveGameObjectsList)
            gameObject.SetActive(false);
    }
}