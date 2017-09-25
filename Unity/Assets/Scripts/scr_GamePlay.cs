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



    #region Buttons & Sizzle

    public void OrangeButton() {
        options[0].picked = true;
    }

    public void BlueButton() {
        options[1].picked = true;
    }

    public void DiscardButton() {
        options[2].picked = true;
    }
    #endregion

    public void Start()
    {
        print("GameStart");
        codeToCheck = peopleToJudge[personBeingJudged].code;
        print("NEW PERSON: " + makeString(codeToCheck));
        codeText.text = makeString(codeToCheck);
    }

    public void Update()
    {
        foreach (cls_Option opt in options)
        {
            if (opt.picked)
            {
                print("Option " + opt.optionName + " WAS PICKED");
                if (personBeingJudged < peopleToJudge.Count)
                    CheckPerson(peopleToJudge[personBeingJudged]);
                else
                    print("Game Has Ended");
            }
            opt.picked = false;
        }
    }

    public void NextPerson()
    {
        if (personBeingJudged < peopleToJudge.Count)
        {
            personBeingJudged++;
            codeToCheck.Clear();
            codeToCheck = peopleToJudge[personBeingJudged].code;
            print("NEW PERSON: " + makeString(codeToCheck));
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
                NextPerson();
                break;
            }
            else
                print(opt.optionName + " was wrong");
        }
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
}

[System.Serializable]
public class cls_Person
{
    public List<int> code;
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