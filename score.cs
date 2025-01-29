using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class score : MonoBehaviour
{

    [Header("Time")]
    [SerializeField] TextMeshProUGUI FinalScore;


    private Links links;

    // Start is called before the first frame update
    void Start()
    {
        string baseFinalScore = PlayerScore.Instance?.FinalScore ?? "00:00";
        int totalPenalty = PlayerScore.Instance?.TotalPenalty ?? 0;

        // Parse the base final score into minutes and seconds
        string[] timeParts = baseFinalScore.Split(':');
        int minutes = int.Parse(timeParts[0]);
        int seconds = int.Parse(timeParts[1]);

        // Convert the base final score to total seconds
        int totalSeconds = (minutes * 60) + seconds;

        // Add 10 seconds for each penalty
        int penaltyTime = totalPenalty * 10;
        totalSeconds += penaltyTime;

        // Convert the total seconds back to minutes and seconds
        minutes = totalSeconds / 60;
        seconds = totalSeconds % 60;

        // Format the adjusted final score
        string adjustedFinalScore = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Display the adjusted final score
        FinalScore.text = adjustedFinalScore;
    }
}
