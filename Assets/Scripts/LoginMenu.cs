using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour
{    
    public TMP_InputField usernamefield;
    public TMP_InputField passwordfield;
    public Button loginButton;    

    void Start()
    {
        loginButton.onClick.AddListener(AttemptLogin);
        usernamefield.text = "anugill147";
        passwordfield.text = "someone33";
    }

    private void AttemptLogin()
    {
        StartCoroutine(Login());
    }   

    private IEnumerator Login()
    {
        UnityWebRequest csrfRequest = UnityWebRequest.Get("http://127.0.0.1:8000/gettoken/");
        yield return csrfRequest.SendWebRequest();

        if (csrfRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to get CSRF token: " + csrfRequest.error);
            yield break;
        }

        string csrfToken = csrfRequest.GetResponseHeader("Set-Cookie").Split(';')[0].Split('=')[1];
        Debug.Log(csrfRequest.GetResponseHeader("Set-Cookie"));
        Debug.Log(csrfToken);

        string url = "http://127.0.0.1:8000/unitylogin/";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", usernamefield.text));
        formData.Add(new MultipartFormDataSection("password", passwordfield.text));
        formData.Add(new MultipartFormDataSection("X-CSRFToken", csrfToken));

        using (UnityWebRequest request = UnityWebRequest.Post(url, formData))
        {
            request.SetRequestHeader("User-Agent", "XRBioClient");
            request.downloadHandler = new DownloadHandlerBuffer();
            request.redirectLimit = 0;
            request.timeout = 60;

            for (int i=0; i<formData.Count; i++)
            {
                IMultipartFormSection valuepair = formData[i];
                request.SetRequestHeader(valuepair.sectionName, Encoding.UTF8.GetString(valuepair.sectionData));

            }
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Login failed:" + request.error);
            }

            else
            {
                if ((int)request.responseCode == 200)
                {
                    Debug.Log("Login successful!");

                    // Parse the JSON response and extract the authorization token
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log(jsonResponse);
                    SuccessfulLogin successfulLogin = JsonUtility.FromJson<SuccessfulLogin>(jsonResponse);
                    MainMenu.login_session = successfulLogin;
                    MainMenu.session_start_minute = int.Parse(successfulLogin.vrappsession_minute);
                    SceneManager.LoadScene(1);
                }

                else if((int)request.responseCode == 400)
                {
                    Debug.Log("Unsuccessful! Wrong Credentials");
                    string jsonResponse = request.downloadHandler.text;
                    UnSuccessfulLogin unSuccessfulLogin = JsonUtility.FromJson<UnSuccessfulLogin>(jsonResponse);
                    Debug.Log(jsonResponse);
                }
            }

        }
        
        
        /*
        if (request. || request.isHttpError)
        {
            Debug.LogError("Login failed: " + request.error);
        }
        else
        {
            Debug.Log("Login successful!");
            // Handle the response data here
        }

        */

    }


}

[System.Serializable]
public class SuccessfulLogin
{
    public bool success;
    public string username;
    public string vrappsession_minute;
    public string sessionID;
}

[System.Serializable]
public class UnSuccessfulLogin
{
    public bool success;
    public string error;
}


