using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // Add this at the top

public class GameManagerr : MonoBehaviour
{
    public TextMeshProUGUI resultText; // ✅ Moved inside the class

    private Chemical selectedElement = null;
    private Chemical selectedReagent = null;

    [System.Serializable]
    public class Chemical
    {
        public string name;
        public string type; // Element, Acid, Base
    }

    // ✅ Updated method to work with Unity's OnClick()
    public void SelectChemical(string name)
    {
        // Determine if the chemical is an element or reagent
        if (name == "Fe" || name == "Zn" || name == "Al" || name == "Mg" || name == "Cu")
        {
            selectedElement = new Chemical { name = name, type = "Element" };
        }
        else if (name == "HCl" || name == "NaOH")
        {
            selectedReagent = new Chemical { name = name, type = (name == "HCl") ? "Acid" : "Base" };
        }
    }

    public void MixChemicals()
    {
        if (selectedElement == null || selectedReagent == null)
        {
            resultText.text = "Select an element and a reagent!";
            return;
        }

        string reaction = GetReaction(selectedElement, selectedReagent);
        resultText.text = reaction;

        // Clear selections
        selectedElement = null;
        selectedReagent = null;
    }

    private string GetReaction(Chemical element, Chemical reagent)
    {
        // Acid reactions
        if (reagent.name == "HCl")
        {
            switch (element.name)
            {
                case "Fe": return "Fe + 2HCl → FeCl2 + H2 ↑";
                case "Zn": return "Zn + 2HCl → ZnCl2 + H2 ↑";
                case "Al": return "2Al + 6HCl → 2AlCl3 + 3H2 ↑";
                case "Mg": return "Mg + 2HCl → MgCl2 + H2 ↑";
                case "Cu": return "No Reaction";
            }
        }

        // Base reactions
        if (reagent.name == "NaOH")
        {
            switch (element.name)
            {
                case "Al": return "Al2O3 + 2NaOH → 2NaAlO2 + H2O";
                case "Mg": return "MgO + 2NaOH → Na2MgO2 + H2O";
                default: return "No Reaction";
            }
        }

        return "No Reaction!";
    }
}
