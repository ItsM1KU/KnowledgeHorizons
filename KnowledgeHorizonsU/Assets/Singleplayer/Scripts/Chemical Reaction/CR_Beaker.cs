using System.Collections.Generic;
using UnityEngine;

public class Beaker : MonoBehaviour
{
    private string targetProduct;
    private List<string> requiredReactants = new List<string>();
    private List<string> currentReactants = new List<string>();

    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>(); // Store original positions
    private List<GameObject> reactantObjects = new List<GameObject>(); // Track reactants added to the beaker

    void Start()
    {
        SaveReactantPositions(); // Save reactant positions when the game starts
    }

    public void SetTargetReaction(string product, List<string> reactants)
    {
        targetProduct = product;
        requiredReactants = new List<string>(reactants);
        currentReactants.Clear();
        reactantObjects.Clear(); // Clear the list of reactant objects
    }

    public void AddReactant(GameObject reactant)
    {
        string reactantName = reactant.name; // Get reactant name
        if (!currentReactants.Contains(reactantName))
        {
            currentReactants.Add(reactantName);
            reactantObjects.Add(reactant); // Track the reactant GameObject
        }
    }

    public void CheckReaction()
    {
        if (currentReactants.Count == requiredReactants.Count && new HashSet<string>(currentReactants).SetEquals(requiredReactants))
        {
            FindObjectOfType<GameManagerrrrrr>().CorrectReaction();
        }

        ResetReactants(); // Reset reactants after each guess
    }

    private void SaveReactantPositions()
    {
        GameObject[] reactants = GameObject.FindGameObjectsWithTag("Reactant"); // Find all reactants
        foreach (GameObject reactant in reactants)
        {
            if (!originalPositions.ContainsKey(reactant))
            {
                originalPositions[reactant] = reactant.transform.position; // Store original position
            }
        }
    }

    private void ResetReactants()
    {
        foreach (GameObject reactant in reactantObjects)
        {
            if (originalPositions.ContainsKey(reactant))
            {
                reactant.transform.position = originalPositions[reactant]; // Move back to original position
            }
        }

        currentReactants.Clear(); // Clear reactants in beaker
        reactantObjects.Clear(); // Clear reactant GameObjects
    }
}
