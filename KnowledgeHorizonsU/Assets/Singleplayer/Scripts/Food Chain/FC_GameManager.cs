using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class FC_GameManager : MonoBehaviour
{
    public static FC_GameManager instance;

    [SerializeField] GameObject[] slotsA;
    [SerializeField] GameObject[] slotsB;
    [SerializeField] GameObject[] slotsC;
    [SerializeField] GameObject[] slotsD;
    [SerializeField] GameObject[] slotsE;

    [SerializeField] GameObject[] animalPrefabs;

    [SerializeField]
    Transform[] _spawnPoints;

    [SerializeField] Canvas Canvas;

    private int[] AnswerSet;

    int currentPlace;
    //private bool canSubmit = false;

    [SerializeField] public List<GameObject[]> slotsPerRound = new List<GameObject[]>();


    private int currentRound = 0;
    private int score;
    
    private List<GameObject> spawnedAnimals = new List<GameObject>();


    [SerializeField] GameObject responsePanel;
    [SerializeField] TMP_Text responseText;
    [SerializeField] TMP_Text scoreText;


    [SerializeField] GameObject submitButton;
    [SerializeField] AudioSource chompSound;

    [SerializeField] Text finalScoreText;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject menuManager;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentRound = 0;
        score = 0;

        slotsPerRound.Add(slotsA);
        slotsPerRound.Add(slotsB);
        slotsPerRound.Add(slotsC);
        slotsPerRound.Add(slotsD);
        slotsPerRound.Add(slotsE);
    }

    private void Start()
    {
        SpawnFirstSet();
        //ActivateSlots();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.LoadScene("Islands");
        }
    }

    private void PickRound()
    {
        switch (currentRound)
        {
            case 0:
                SpawnFirstSet();
                break;
            case 1:
                StartCoroutine(SecondRound());
                break;
            case 2:
                StartCoroutine(ThirdRound());
                break;
            case 3:
                StartCoroutine(FourthRound());
                break;
            case 4:
                StartCoroutine(FifthRound());
                break;
            case 5:
                StartCoroutine(endgame());
                break;
            default:
                Debug.Log("Game Over");
                break;
        }
    }
    private void SpawnFirstSet()
    {
        ActivateSlots(slotsA);
        DeactivateSlots(slotsB);
        DeactivateSlots(slotsC);
        DeactivateSlots(slotsD);
        DeactivateSlots(slotsE);


        AnswerSet = FC_AnimalPicker.instance.PickFirstSet();
        int[] selectedset = AnswerSet;

        List<int> spawnIndices = new List<int> { 0, 1, 2 };
        shuffleList(spawnIndices);
        for (int i = 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length)
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[spawnIndices[i]].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
                spawnedAnimals.Add(go);
            }
        }
    }

    private void SpawnSecondSet()
    {
        DeactivateSlots(slotsA);
        ActivateSlots(slotsB);
        DeactivateSlots(slotsC);
        DeactivateSlots(slotsD);
        DeactivateSlots(slotsE);

        DestroyAnimals(spawnedAnimals);

        AnswerSet = FC_AnimalPicker.instance.PickSecondSet();
        int[] selectedset = AnswerSet;

        List<int> spawnIndices = new List<int> { 0, 1, 2};
        shuffleList(spawnIndices);
        for (int i = 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length)
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[spawnIndices[i]].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
                spawnedAnimals.Add(go);
            }
        }
        submitButton.SetActive(true);
    }

    private void SpawnThirdSet()
    {
        DeactivateSlots(slotsA);
        ActivateSlots(slotsC);
        DeactivateSlots(slotsB);
        DeactivateSlots(slotsD);
        DeactivateSlots(slotsE);

        DestroyAnimals(spawnedAnimals);

        AnswerSet = FC_AnimalPicker.instance.PickThirdSet();
        int[] selectedset = AnswerSet;

        List<int> spawnIndices = new List<int> { 0, 1, 2, 3};
        shuffleList(spawnIndices);
        for (int i = 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length)
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[spawnIndices[i]].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
                spawnedAnimals.Add(go);
            }
        }
        submitButton.SetActive(true);
    }

    private void SpawnFourthSet()
    {
        DeactivateSlots(slotsA);
        ActivateSlots(slotsD);
        DeactivateSlots(slotsC);
        DeactivateSlots(slotsB);
        DeactivateSlots(slotsE);

        DestroyAnimals(spawnedAnimals);

        AnswerSet = FC_AnimalPicker.instance.PickFourthSet();
        int[] selectedset = AnswerSet;

        List<int> spawnIndices = new List<int> { 0, 1, 2, 3};
        shuffleList(spawnIndices);
        for (int i = 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length)
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[spawnIndices[i]].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
                spawnedAnimals.Add(go);
            }
        }
        submitButton.SetActive(true);
    }

    private void SpawnFifthSet()
    {
        DeactivateSlots(slotsA);
        ActivateSlots(slotsE);
        DeactivateSlots(slotsC);
        DeactivateSlots(slotsD);
        DeactivateSlots(slotsB);

        DestroyAnimals(spawnedAnimals);

        AnswerSet = FC_AnimalPicker.instance.PickFifthSet();
        int[] selectedset = AnswerSet;

        List<int> spawnIndices = new List<int> { 0, 1, 2, 3, 4};
        shuffleList(spawnIndices);
        for (int i = 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length)
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[spawnIndices[i]].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
                spawnedAnimals.Add(go);
            }
        }
        submitButton.SetActive(true);
    }



    public void submit()
    {
        
        if (currentRound > 5)
        {
            StartCoroutine(endgame());
        }

        GameObject[] currentSlots = slotsPerRound[currentRound];

        bool areOccupied = true;
        foreach (GameObject slot in currentSlots)
        {
            if (!slot.GetComponent<FC_SlotHolder>().isoccupied)
            {
                areOccupied = false;
                break;
            }
        }

        if (areOccupied)
        {
            submitButton.SetActive(false);
            currentPlace = 0;
            List<GameObject> placedAnimals = new List<GameObject>();
            for (int i = 0; i < AnswerSet.Length; i++)
            {
                GameObject placedAnimal = currentSlots[i].transform.GetChild(0).gameObject;
                placedAnimals.Add(placedAnimal);
                if (currentSlots[i].transform.GetChild(0).GetComponent<FC_AnimalInfo>().AnimalID == AnswerSet[i])
                {
                    Debug.Log("Correct");
                    
                    currentPlace++;
                }
                else
                {
                    Debug.Log("Incorrect");
                    
                }
            }
            if (currentPlace >= AnswerSet.Length)
            {
                Debug.Log("You have completed the food chain");
                score += AnswerSet.Length * 10;
                scoreText.text = score.ToString();
                StartCoroutine(AnimalSubmitAnimation(placedAnimals));

            }
            else
            {

                Debug.Log("You have " + currentPlace + " Correct");
                //submitButton.SetActive(true);
                int wrongAnswers = AnswerSet.Length - currentPlace;
                score -= (wrongAnswers * 5);
                scoreText.text = score.ToString();
                StartCoroutine(showResponse("You have " + currentPlace + " Correct"));

            }
        }
        else
        {
           StartCoroutine(showResponse("Please fill all the slots."));
        }
    }

    public void test()
    {
        Debug.Log(AnswerSet.Length);
    }

    public IEnumerator SecondRound()
    {
        yield return new WaitForSeconds(1);
        SpawnSecondSet();
    }

    public IEnumerator ThirdRound()
    {
        yield return new WaitForSeconds(1);
        SpawnThirdSet();
    }

    public IEnumerator FourthRound()
    {
        yield return new WaitForSeconds(1);
        SpawnFourthSet();
    }

    public IEnumerator FifthRound()
    {
        yield return new WaitForSeconds(1);
        SpawnFifthSet();
    }


    private void shuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void ActivateSlots(GameObject[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(true);
        }
    }

    private void DeactivateSlots(GameObject[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(false);
        }
    }

    private void DestroyAnimals(List<GameObject> animals)
    {
        foreach (GameObject animal in animals)
        {
            Destroy(animal);
        }
    }

    public IEnumerator showResponse(string response)
    {
        
        responsePanel.SetActive(true);
        responseText.text = response;
        yield return new WaitForSeconds(2);
        responsePanel.SetActive(false);

        yield return new WaitForSeconds(4f);
        submitButton.SetActive(true);
    }

    public IEnumerator startNextRound()
    {
        responsePanel.SetActive(true);
        responseText.text = "You have completed the food chain";
        yield return new WaitForSeconds(1);
        responsePanel.SetActive(false);
        //yield return new WaitForSeconds(1);
        currentRound++;
        if(currentRound >= 6)
        {
            Debug.Log("Game Over"); 
        }
        PickRound();
        
    }


    private IEnumerator AnimalSubmitAnimation(List<GameObject> placedAnimals)
    {
        for (int i = 0; i < placedAnimals.Count - 1; i++) 
        {
            animalAnimation(placedAnimals[i], placedAnimals[i+1]);
            yield return new WaitForSeconds(2.5f);
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(startNextRound());
    }

    private void animalAnimation(GameObject prey, GameObject predator)
    {
        Image preyImage = prey.GetComponent<Image>();
        Image predatorImage = predator.GetComponent<Image>();

        var sequence = DOTween.Sequence();

        sequence.Append(prey.transform.DOShakePosition(0.5f, 5, 10, 90, false, true));
        sequence.Append(predator.transform.DOMove(prey.transform.position, 1f).SetEase(Ease.InOutQuad));
        sequence.Join(predator.transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 1f));
        sequence.Append(prey.transform.DOScale(Vector3.zero, 0.5f));
        sequence.Join(preyImage.DOFade(0, 0.5f));
        //chompSound.Play();
        sequence.Append(predator.transform.DOScale(new Vector3(2f, 2f, 2f), 0.5f).SetEase(Ease.OutBounce));
        sequence.Join(predatorImage.DOColor(Color.red, 0.1f));
        sequence.Append(predatorImage.DOColor(Color.white, 0.1f));
    }

    private IEnumerator endgame()
    {
        menuManager.SetActive(false);
        responsePanel.SetActive(true);
        responseText.text = "You have completed all the given food chains";
        yield return new WaitForSeconds(1);
        responsePanel.SetActive(false);
        finalScoreText.text = "Final Score: " + score;
        gameOverMenu.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene("FC_gamescene");
    }

    public void Islands()
    {
        SceneManager.LoadScene("Islands");
    }
}
