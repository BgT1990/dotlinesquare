using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class IPfinder : MonoBehaviour
{
    public GameObject ThisObject;
    private string RawIpInfo;
    public string IP = "#";

    private void OnEnable()
    {
        StartCoroutine(GetIpFromGoogle());
    }

    /*
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
    */

    /*
function CheckIP()
{
    myExtIPWWW = WWW("http://checkip.dyndns.org");
    if (myExtIPWWW == null) return;
    yield myExtIPWWW;
    myExtIP = myExtIPWWW.data;
    myExtIP = myExtIP.Substring(myExtIP.IndexOf(":") + 1);
    myExtIP = myExtIP.Substring(0, myExtIP.IndexOf("<"));
    // print(myExtIP);
}
*/

    IEnumerator GetIpFromGoogle()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://checkip.dyndns.com/");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            RawIpInfo = www.downloadHandler.text;
            Debug.Log(www.downloadHandler.text);
            //Debug.Log(www.downloadHandler.data);

            //Text looks like this...
            //<html><head><title>Current IP Check</title></head><body>Current IP Address: 81.105.234.2</body></html>

            //Moved from onEnable()
            string TextWithIp = ThisObject.GetComponent<Text>().text.Replace("#", FormatIp());

            string TextWithLAN = TextWithIp.Replace("@", LocalIPAddress());

            ThisObject.GetComponent<Text>().text = TextWithLAN;

        }
    }

    public string FormatIp()
    {
        if (RawIpInfo.Length > 16)
        {
            IP = RawIpInfo.Remove(0, 76);
            IP = IP.Remove(IP.Length - 16);
        }
        return IP;
    }

    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    //Network.player.ipAddress
}
