using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static PlayerScore Instance { get; private set; }

    public string FinalScore { get; private set; }
    public int TotalPenalty { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetFinalScore(string score)
    {
        FinalScore = score;
        
    }

    public void SetPenalty(int penalty)
    {
        TotalPenalty = penalty;
    }
}
