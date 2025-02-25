using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class Beaker : MonoBehaviour, IDropHandler
{
    private List<string> reactants = new List<string>();

    public Image beakerImage; // Assign in Inspector
    public Text productText; // Displays product name (Assign in Inspector)
    public Text reactionText; // Displays reaction description (Assign in Inspector)

    private Dictionary<string, Color> elementColors = new Dictionary<string, Color>()
    {
        {"Hydrogen", Color.white},
        {"Oxygen", Color.cyan},
        {"Sodium", Color.yellow},
        {"Chlorine", Color.green},
        {"Calcium", Color.gray},

        {"Hydrochloric Acid", Color.red},
        {"Sulfuric Acid", Color.magenta},

        {"Sodium Hydroxide", Color.blue},
        {"Calcium Hydroxide", new Color(0.5f, 0f, 0.5f)} // Custom purple
    };

    public void OnDrop(PointerEventData eventData)
    {
        DraggableElement draggedElement = eventData.pointerDrag.GetComponent<DraggableElement>();
        if (draggedElement != null)
        {
            reactants.Add(draggedElement.name);
            Destroy(draggedElement.gameObject); // Remove from scene

            if (reactants.Count == 2) // Process reaction after 2 reactants
            {
                CheckReaction();
                reactants.Clear(); // Reset reactants after mixing
            }
        }
    }

    void CheckReaction()
    {
        string productName = "No reaction";
        string productSymbol = "";
        string reactionDescription = "No significant reaction occurred.";
        Color newColor = Color.gray; // Default beaker color

        if (reactants.Contains("Hydrogen") && reactants.Contains("Oxygen"))
        {
            productName = "Water";
            productSymbol = "H₂O";
            reactionDescription = "Hydrogen and Oxygen combined to form Water.";
            newColor = Color.blue;
        }
        else if (reactants.Contains("Sodium") && reactants.Contains("Chlorine"))
        {
            productName = "Salt";
            productSymbol = "NaCl";
            reactionDescription = "Sodium and Chlorine combined to form Salt.";
            newColor = Color.white;
        }
        else if (reactants.Contains("Calcium") && reactants.Contains("Sulfuric Acid"))
        {
            productName = "Calcium Sulfate";
            productSymbol = "CaSO₄";
            reactionDescription = "Calcium reacted with Sulfuric Acid to form Calcium Sulfate.";
            newColor = Color.gray;
        }
        else if (reactants.Contains("Sodium Hydroxide") && reactants.Contains("Hydrochloric Acid"))
        {
            productName = "Salt Water";
            productSymbol = "NaCl + H₂O";
            reactionDescription = "Sodium Hydroxide and Hydrochloric Acid neutralized to form Salt Water.";
            newColor = Color.cyan;
        }

        DisplayProduct(productName, productSymbol, reactionDescription, newColor);
    }

    void DisplayProduct(string name, string symbol, string reaction, Color newColor)
    {
        productText.text = name + " (" + symbol + ")"; // Display Product name and symbol
        reactionText.text = reaction; // Show reaction description
        beakerImage.color = newColor; // Change beaker color
    }

    public void ClearReactants()
    {
        reactants.Clear();
        productText.text = "Beaker Reset!";
        reactionText.text = "";
        beakerImage.color = Color.gray;
        Debug.Log("Beaker reset!");
    }
}
