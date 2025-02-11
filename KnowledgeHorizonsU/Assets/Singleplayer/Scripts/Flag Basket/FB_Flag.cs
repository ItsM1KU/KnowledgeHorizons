using UnityEngine;

public class FB_Flag : MonoBehaviour
{
    public string flagCountry; // Holds the country name for this flag.
    private string flagCode;

    public float fallSpeed = 3f;
    public float fallThreshold = -5f;
    private bool isCollected = false; // Set true when caught.

    public void SetFlag(string country, string code)
    {
        flagCountry = country;
        flagCode = code;

        Debug.Log("Assigning Flag: " + flagCountry + " with Code: " + flagCode);

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

    // Called by basket when the flag is caught.
    public void MarkAsCollected()
    {
        isCollected = true;
    }

    void Update()
    {
        if (isCollected)
            return;

        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < fallThreshold)
        {
            FB_GameManager gm = FindObjectOfType<FB_GameManager>();
            if (gm != null)
            {
                gm.FlagMissed();
            }
            Destroy(gameObject);
        }
    }
}
