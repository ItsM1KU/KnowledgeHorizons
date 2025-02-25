using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class Beaker : MonoBehaviour, IDropHandler
{
    private List<string> reactants = new List<string>();

    public Image beakerImage; // Beaker UI Image (assign in Inspector)
    public Text reactionText; // Reaction description text (assign in Inspector)
    public Transform productDisplayArea; // UI area for final product (assign in Inspector)

    public Dictionary<string, GameObject> productPrefabs; // Prefabs for final products

    private Dictionary<string, Color> elementColors = new Dictionary<string, Color>()
    {
        {"Hydrogen", Color.white},
        {"Oxygen", Color.cyan},
        {"Sodium", Color.yellow},
        {"Chlorine", Color.green},
        {"Hydrochloric Acid", Color.red},
        {"Sodium Hydroxide", Color.blue}
    };

    public void OnDrop(PointerEventData eventData)
    {
        DraggableElement draggedElement = eventData.pointerDrag.GetComponent<DraggableElement>();
        if (draggedElement != null)
        {
            reactants.Add(draggedElement.name);
            Destroy(draggedElement.gameObject); // Remove reactants from scene

            if (reactants.Count == 2) // Mix when two reactants are added
            {
                CheckReaction();
            }
        }
    }

    void CheckReaction()
    {
        string reactionResult = "No significant reaction.";
        GameObject product = null;
        Color newColor = Color.gray; // Default color

        if (reactants.Contains("Hydrogen") && reactants.Contains("Oxygen"))
        {
            reactionResult = "Water (H₂O) formed!";
            newColor = Color.blue;
            product = Instantiate(productPrefabs["Water"], productDisplayArea);
        }
        else if (reactants.Contains("Sodium") && reactants.Contains("Chlorine"))
        {
            reactionResult = "Sodium Chloride (NaCl) formed!";
            newColor = Color.white;
            product = Instantiate(productPrefabs["Salt"], productDisplayArea);
        }

        reactionText.text = reactionResult; // Update reaction description
        ChangeBeakerColor(newColor);
        reactants.Clear(); // Reset beaker contents
    }

    void ChangeBeakerColor(Color newColor)
    {
        beakerImage.color = newColor;
    }

    public void ClearReactants()
    {
        reactants.Clear();
        ChangeBeakerColor(Color.gray); // Reset color
        reactionText.text = "Beaker reset!"; // Clear reaction text

        // Destroy all products in the display area
        foreach (Transform child in productDisplayArea)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Beaker reset!");
    }
}
