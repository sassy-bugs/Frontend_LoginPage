using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [SerializeField] private string authenticationEndpoint = "https://backend-server-9vsy.onrender.com/account";
    
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    public static string username1 = "Sample";
    public int num_of_logs =0;

   public void onLoginClick()
   {
    
        alertText.text = "Signing in...";
        loginButton.interactable = false;

        StartCoroutine(TryLogin());
   }

   private IEnumerator TryLogin()
   {
       
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length <3 || username.Length >24)
        {
            alertText.text = "Invalid username";
            loginButton.interactable = true;
            yield break;
        } 
        if (password.Length <3 || password.Length >24)
        {
            alertText.text = "Invalid password";
            loginButton.interactable = true;
            yield break;
        } 

        UnityWebRequest request = UnityWebRequest.Get($"{authenticationEndpoint}?rUsername={username}&rPassword={password}");
        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while(!handler.isDone)
        {
            startTime += Time.deltaTime;

            if(startTime > 10.0f)
            {
                break;
            }
            yield return null;
        }
        if(request.result == UnityWebRequest.Result.Success)
        {
            // if(request.downloadHandler.text == "Cannot login again")
            // {
            //     loginButton.interactable =false;
            //     alertText.text = "Cannot Login again";
            // }
            if(request.downloadHandler.text == "Invalid move")
            {
                 alertText.text= "Already logged in";
                loginButton.interactable = false;
            }
           else if(request.downloadHandler.text != "Invalid Credentials")
            {
                
                loginButton.interactable = false;
                GameAccount returnedAccount = JsonUtility.FromJson<GameAccount>(request.downloadHandler.text);
                alertText.text= "Welcome "+ returnedAccount.username + ((returnedAccount.adminFlag == 1) ? " Admin": "");
                
                username1=returnedAccount.username;
                //SceneManager.LoadScene(1);

                }
            // else if(request.downloadHandler.text == "Invalid move")
            // {
            //      alertText.text= "Already logged in";
            //     loginButton.interactable = false;
            // }
            else
            {
                alertText.text= "Invalid Credentials";
                loginButton.interactable = true;
            }
        }
        else
        {
            alertText.text = "Connection Lost...";
            loginButton.interactable = true;
            
        }

        Debug.Log($"{username} : {password}");

        yield return null;
   }
}
