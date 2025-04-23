using UnityEngine;

public class TB_CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public float yOffset = 2f; // How much buffer to leave above the tower
    private float initialY;

    void Start()
    {
        initialY = transform.position.y;
    }

    void Update()
    {
        if (!TB_CrateSpawner.cameraShouldFollow) return;

        float highestCrateY = GetHighestCrateY();
        float cameraTopY = transform.position.y + Camera.main.orthographicSize;

        // Move camera up only when a crate rises above the current top view
        if (highestCrateY > cameraTopY - yOffset)
        {
            float targetY = highestCrateY - Camera.main.orthographicSize + yOffset;
            Vector3 newPos = new Vector3(transform.position.x, targetY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }



    float GetHighestCrateY()
    {
        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
        if (crates.Length == 0) return float.MinValue;

        float highestY = float.MinValue;
        foreach (GameObject crate in crates)
        {
            if (crate.transform.position.y > highestY)
                highestY = crate.transform.position.y;
        }

        return highestY;
    }
}
