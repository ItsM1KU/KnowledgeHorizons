using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerrrrrr : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    public Beaker beaker;

    private int score = 0;
    private float timeLeft = 30f;
    private int currentRound = 0;
    private int totalRounds = 5;

    private List<string> selectedProducts = new List<string>();
    private Dictionary<string, List<string>> possibleReactions = new Dictionary<string, List<string>>();

    void Start()
    {
        InitializeReactions();
        SelectRandomProducts();
        StartNewRound();
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
        }
        else
        {
            EndGame();
        }
    }

    void InitializeReactions()
    {
        possibleReactions.Clear(); // Clear previous data

        // Only reactions that use the available elements, acid, and base
        possibleReactions.Add("NaCl", new List<string> { "Na", "Cl2" });
        possibleReactions.Add("ZnO", new List<string> { "Zn", "O2" });
        possibleReactions.Add("H2O", new List<string> { "H2", "O2" });
        possibleReactions.Add("HCl", new List<string> { "H2", "Cl2" });
        possibleReactions.Add("NaOH", new List<string> { "Na", "H2O" });
        possibleReactions.Add("ZnCl2", new List<string> { "Zn", "Cl2" });
        possibleReactions.Add("H2O2", new List<string> { "H2", "O2", "O2" });
        possibleReactions.Add("Na2O", new List<string> { "Na", "O2" });
        possibleReactions.Add("H2ZnO2", new List<string> { "Zn", "H2O2" });
        possibleReactions.Add("Na2ZnO2", new List<string> { "Na", "ZnO" });
    }

    void SelectRandomProducts()
    {
        selectedProducts.Clear();
        List<string> allProducts = new List<string>(possibleReactions.Keys);

        while (selectedProducts.Count < totalRounds)
        {
            string randomProduct = allProducts[Random.Range(0, allProducts.Count)];
            if (!selectedProducts.Contains(randomProduct))
            {
                selectedProducts.Add(randomProduct);
            }
        }
    }

    void StartNewRound()
    {
        if (currentRound < totalRounds)
        {
            promptText.text = "Mix the reactants to create: " + selectedProducts[currentRound];
            beaker.SetTargetReaction(selectedProducts[currentRound], possibleReactions[selectedProducts[currentRound]]);
        }
        else
        {
            EndGame();
        }
    }

    public void CorrectReaction()
    {
        score += 10;
        scoreText.text = "Score: " + score;
        currentRound++;
        StartNewRound();
    }

    void EndGame()
    {
        promptText.text = "Game Over! Final Score: " + score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
