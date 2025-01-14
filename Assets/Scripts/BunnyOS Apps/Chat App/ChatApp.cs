using NUnit.Framework;
using UnityEngine;

public class ChatApp : MonoBehaviour
{
    [SerializeField] private GameObject errorWindow;
    [SerializeField] private GameObject appWindow;

    [SerializeField] private GameObject userChats;
    [SerializeField] private GameObject noChatsDisplay;
    [SerializeField] private GameObject chatsHolder;

    void Awake()
    {
        ChatController chatController = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<ChatController>();

        errorWindow.SetActive(false);
        appWindow.SetActive(false);
        userChats.SetActive(false);
        noChatsDisplay.SetActive(false);

        if(chatController.isServerEnabled) 
        {
            appWindow.SetActive(true);

            if(!chatController.isChatEmpty)
            {
                userChats.SetActive(true);
                
                #if UNITY_EDITOR
                Assert.IsNotNull(chatController.unknownUserMessagesList);
                #endif

                if(chatController.unknownUserMessagesList.Count > 0)
                {
                    foreach(GameObject message in chatController.unknownUserMessagesList)
                    {
                        GameObject chat = Instantiate(message, chatsHolder.transform);
                        chat.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                }
            }
            else noChatsDisplay.SetActive(true);
        }
        else errorWindow.SetActive(true);
    }
}