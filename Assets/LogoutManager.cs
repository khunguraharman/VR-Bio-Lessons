using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class LogoutManager : MonoBehaviour
{
    
    private void OnApplicationQuit()
    {
        StartCoroutine(SendLogoutRequest());
    }


    public void Logout()
    {
        StartCoroutine(SendLogoutRequest());
    }

    private IEnumerator SendLogoutRequest()
    {
        UnityWebRequest csrfRequest = UnityWebRequest.Get("http://127.0.0.1:8000/gettoken/");
        yield return csrfRequest.SendWebRequest();

        if (csrfRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to get CSRF token: " + csrfRequest.error);
            yield break;
        }

        string csrfToken = csrfRequest.GetResponseHeader("Set-Cookie").Split(';')[0].Split('=')[1];
        //Debug.Log(csrfRequest.GetResponseHeader("Set-Cookie"));
        Debug.Log("Token for logout:" + csrfToken);

        string url = "http://127.0.0.1:8000/unitylogout/";
        
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("X-CSRFToken", csrfToken));
        formData.Add(new MultipartFormDataSection("session",  MainMenu.login_session.sessionID));

        using (UnityWebRequest request = UnityWebRequest.Post(url, formData))
        {
            request.SetRequestHeader("User-Agent", "XRBioClient");
            request.downloadHandler = new DownloadHandlerBuffer();
            request.redirectLimit = 0;
            request.timeout = 60;

            for (int i = 0; i < formData.Count; i++)
            {
                IMultipartFormSection valuepair = formData[i];
                request.SetRequestHeader(valuepair.sectionName, Encoding.UTF8.GetString(valuepair.sectionData));
            }

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Logout request error: " + request.error);
            }
            else
            {
                Debug.Log("Logout request successful");
            }
        }
    }
}
