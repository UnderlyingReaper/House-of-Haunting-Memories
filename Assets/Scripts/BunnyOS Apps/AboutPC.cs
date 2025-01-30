using TMPro;
using UnityEngine;
using System;

public class AboutPC : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deviceName;
    [SerializeField] TextMeshProUGUI processorName;
    [SerializeField] TextMeshProUGUI ramName;
    [SerializeField] TextMeshProUGUI deviceID;



    void Awake()
    {
        deviceName.text += SystemInfo.deviceName;
        processorName.text += SystemInfo.processorType;
        ramName.text += ((Mathf.FloorToInt(SystemInfo.systemMemorySize) / 1024) + 1).ToString() + "GB";
        deviceID.text += SystemInfo.deviceUniqueIdentifier.ToString();
    }
}