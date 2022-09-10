using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [SerializeField] private string authenticationEndpoint = "http://localhost:13756/account";
    
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    public static string userID;

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
            if(request.downloadHandler.text != "Invalid Credentials")
            {
                
                loginButton.interactable = false;
                GameAccount returnedAccount = JsonUtility.FromJson<GameAccount>(request.downloadHandler.text);
                alertText.text= "Welcome "+ returnedAccount.username + ((returnedAccount.adminFlag == 1) ? " Admin": "");
                
                userID=returnedAccount._id;
                SceneManager.LoadScene(1);

                }
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
