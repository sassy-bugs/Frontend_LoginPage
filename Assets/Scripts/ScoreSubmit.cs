using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class ScoreSubmit : MonoBehaviour
{
    

    public int score;
    public TextMeshProUGUI scoreText;
    public Button submitScore;
    public string username ;
    

    [SerializeField] private string scoreEndpoint = "https://backend-server-9vsy.onrender.com/score";
    // public void start()
    // {
        
    // }
    void Update()
    {
        scoreText.text = score.ToString();
    }
    public void addScore()
    {
      score++;
    }
    public void onSubmit()
    {
        submitScore.interactable = false;
        username = Login.username1;
        Debug.Log(username);

        StartCoroutine(TrySubmit());
    }
    private IEnumerator TrySubmit()
    {
        UnityWebRequest request = UnityWebRequest.Get($"{scoreEndpoint}?rUserName={username}&rScore={score}");
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
            
            Debug.Log($"{username} : {score}");
        }

    }
}
