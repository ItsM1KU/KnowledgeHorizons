using UnityEngine;

public class FB_Flag : MonoBehaviour
{
    public string flagCountry;
    private string flagCode;

    public void SetFlag(string country, string code)
    {
        flagCountry = country;
        flagCode = code;

        Debug.Log("Assigning Flag: " + flagCountry + " with Code: " + flagCode);

        // ✅ Load the sprite from the correct path inside Resources/
        Sprite flagSprite = Resources.Load<Sprite>("Flags/" + flagCode);
        if (flagSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = flagSprite;
        }
        else
        {
            Debug.LogWarning("Sprite for " + flagCode + " not found in Resources/Flags/");
        }
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(flagCode))
        {
            SetFlag(flagCountry, flagCode);
        }
    }
}
