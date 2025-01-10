using UnityEngine;

public class Balloon : MonoBehaviour
{
    public string number; 
    public bool isCorrect; 

    public void Pop()
    {
        if (isCorrect)
        {
            Debug.Log("Correct!");
            
        }
        else
        {
            Debug.Log("Wrong!");
            
        }

        Destroy(gameObject); 
    }

    private void OnMouseDown()
    {
        Pop();
    }
}
