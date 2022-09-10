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
    public string userID = Login.userID;
    [SerializeField] private string scoreEndpoint = "http://localhost:13756/score";
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

        StartCoroutine(TrySubmit());
    }
    private IEnumerator TrySubmit()
    {
        UnityWebRequest request = UnityWebRequest.Get($"{scoreEndpoint}?rUserID={userID}&rScore={score}");
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
            Debug.Log($"{userID} : {score}");
        }

    }
}
