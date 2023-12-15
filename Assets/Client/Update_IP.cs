using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Update_IP : MonoBehaviour
{
    //private const string PlayerPrefs_IP = "127.000.000.000"; // Key for PlayerPrefs
    private const string PlayerPrefs_IP = "192.168.105.145"; // Key for PlayerPrefs
    private string TargetIP = "127.0.0.1";


    NetworkClient ntwkClient;

    private void Awake()
    {
        ntwkClient = GameObject.Find("Client").GetComponent<NetworkClient>();
        //FindObjectOfType<NetworkClient>();

    }

    public void UpdateIP()
    {
        Debug.Log("Update IP : " + TargetIP);

        // Store the updated TargetIP in PlayerPrefs
        PlayerPrefs.SetString(PlayerPrefs_IP, TargetIP);
        PlayerPrefs.Save();

        ntwkClient.UpdateServerIP(TargetIP);
    }

    public void Set_IP(string input_ip)
    {
        if (IsIPAddress(input_ip) || true) // TODO: remove true
        {
            TargetIP = input_ip;
        }
        else
        {
            Debug.Log("The input is not an IP");
        }
       
    }


    public static bool IsIPAddress(string input)
    {
        // Regular expression to check if the string is a valid IP address
        string pattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";



        // Check if the string matches the pattern
        Regex regex = new Regex(pattern);
        return regex.IsMatch(input);
    }
}
