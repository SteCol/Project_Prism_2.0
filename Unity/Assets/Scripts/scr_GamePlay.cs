using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_GamePlay : MonoBehaviour
{
    [Header("Gameplay Calculation Stuff")]
    public List<cls_Option> options;
    public int personBeingJudged;
    public List<int> codeToCheck;
    public List<cls_Person> peopleToJudge;

    [Header("UI Stuff")]
    public Text codeText;
    public Text resultText;
    public Text scoreText;
    public int score;

    #region Buttons & Sizzle

    public void OrangeButton()
    {
        options[1].picked = true;
        CheckPerson(peopleToJudge[personBeingJudged]);

    }

    public void BlueButton()
    {
        options[0].picked = true;
        CheckPerson(peopleToJudge[personBeingJudged]);

    }

    public void DiscardButton()
    {
        options[2].picked = true;
        CheckPerson(peopleToJudge[personBeingJudged]);

    }
    #endregion

    public void Awake() {
        ClearPeople();
        GeneratePeople();
    }

    public void Start()
    {
        print("GameStart");
        codeToCheck = peopleToJudge[personBeingJudged].code;
        print("NEW PERSON: " + makeString(codeToCheck));
        DrawCode(codeToCheck);
        codeText.text = makeString(codeToCheck);
        scoreText.text = score.ToString();
    }

    public void NextPerson()
    {
        if (personBeingJudged < peopleToJudge.Count)
        {
            personBeingJudged++;
            codeToCheck.Clear();
            codeToCheck = peopleToJudge[personBeingJudged].code;
            print("NEW PERSON: " + makeString(codeToCheck));
            DrawCode(codeToCheck);
            codeText.text = makeString(codeToCheck);
        }
        else
            print("Game End");
    }

    public void CheckPerson(cls_Person _person)
    {
        foreach (cls_Option opt in options)
        {
            print("Checking If " + opt.optionName + " is Correct");

            if (compareLists(opt.mustContain, codeToCheck) == opt.mustContain.Count && compareLists(opt.cantContain, codeToCheck) == 0)
            {
                print("Right Answer was " + opt.optionName);
                resultText.text = "The correct answer was " + opt.optionName;
                if (opt.picked)
                {
                    score++;
                    opt.picked = false;
                }
                print("Score is now  " + score);
                scoreText.text = score.ToString();

                NextPerson();
                break;
            }
            else
                print(opt.optionName + " was wrong");

        }
        scoreText.text = score.ToString();
    }

    public int compareLists(List<int> _listA, List<int> _listB)
    {
        int match = 0;

        print("Compairing " + makeString(_listA) + " to " + makeString(_listB));

        foreach (int i in _listA)
            foreach (int j in _listB)
                if (i == j)
                    match++;

        print("Matches" + match);
        return match;
    }

    public string makeString(List<int> _ints)
    {
        string sum = "";

        foreach (int i in _ints)
            sum = sum + "." + i.ToString();

        return sum;
    }

    public void ClearPeople() {
        peopleToJudge.Clear();
    }

    public void GeneratePeople() {
        for (int i = 0; i < 50; i++) {
            List<int> tempCode = new List<int>();

            for (int j = 0; j < 3; j++)
                tempCode.Add(Random.Range(1, 9));

            peopleToJudge.Add(new cls_Person(tempCode));
        }
    }

    public void DrawCode(List<int> _code) {
        foreach (Transform t in _Storage.Storage().symbolPanel.GetComponentInChildren<Transform>()) {
            Destroy(t.gameObject);
        }

        foreach (int i in _code) {
            GameObject symbolClone = Instantiate(_Storage.Storage().imagePrefab, _Storage.Storage().symbolPanel.transform);
            symbolClone.GetComponentInChildren<Image>().sprite = _Storage.GetSymbols(i);

        }
    }
}

[System.Serializable]
public class cls_Person
{
    public List<int> code;

    public cls_Person(List<int> _code) {
        code = _code;
    }
}

[System.Serializable]
public class cls_Option
{
    public string optionName;
    public bool picked;
    public List<int> mustContain;
    public List<int> canContain;
    public List<int> cantContain;
}