using UnityEngine;

public class NewsApp : MonoBehaviour
{

    [SerializeField] Transform bodyOfApp;

    void Awake()
    {
        GameObject newsToShow = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<TodaysNews>().todaysNews;

        RectTransform newsIns = Instantiate(newsToShow, bodyOfApp).GetComponent<RectTransform>();
        newsIns.anchoredPosition = Vector2.zero;
    }
}