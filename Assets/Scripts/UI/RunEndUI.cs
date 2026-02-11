using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI that pops up at the end of a run
/// No matter if player loses or wins
/// Shows stats of the run
/// </summary>
public class RunEndUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI coinsEarnedText;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI timeSurvivedText;
    [SerializeField] private Button continueButton;

    private void Start()
    {
        continueButton?.onClick.AddListener(OnContinueClicked);
    }

    public void ShowResults(bool won, RunStats stats)
    {
        // win/loss?
        if (resultText != null)
        {
            resultText.text = won ? "VICTORY!" : "DEFEAT";
            resultText.color = won ? Color.green : Color.red;
        }

        // coins earned
        if (coinsEarnedText != null)
            coinsEarnedText.text = $"Coins Earned: {stats.coinsEarned}";

        // enemies killed
        if (enemiesKilledText != null)
            enemiesKilledText.text = $"Enemies Killed: {stats.enemiesKilled}";

        // how long player survived
        if (timeSurvivedText != null)
            timeSurvivedText.text = $"Time Survived: {Mathf.CeilToInt(stats.timeSurvived)}s";

        Debug.Log($"Run ended - Won: {won}, Coins: {stats.coinsEarned}, Enemies: {stats.enemiesKilled}");
    }

    //continue back to home base
    private void OnContinueClicked()
    {
        GameManager.Instance.ReturnToHomeBase();
    }
}