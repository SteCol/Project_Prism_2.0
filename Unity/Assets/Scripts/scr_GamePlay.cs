/*
    NOTE:

    Make is way easier and work backwards: pick a correct button, then generate a code based on that button, and only check if the player picked the right button.
    - Updated to the better system.
*/

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
    public List<cls_PersonB> peopleToJudge;
    public string seed;
    public System.Random pseudoRandom;

    [Header("UI Stuff")]
    public Text codeText;
    public Text resultText;
    public Text scoreText;
    public int score;

    [Header("Gameplay Stuff")]
    public float waitBetweenSteps = 0.0f;
    public bool cont;
    public bool busy;
    public scr_AnimationController animationController;


    #region Buttons & Sizzle

    public void OrangeButton()
    {
        if (animationController.cont)
            Action(1, 1);
    }

    public void BlueButton()
    {
        if (animationController.cont)
            Action(0, 2);
    }

    public void DiscardButton()
    {
        if (animationController.cont)
        {
            resultText.text = "DISCARDED";
            animationController.SetSprite(6, animationController.sprites[3], 2.0f);
            CheckPerson(peopleToJudge[personBeingJudged]);
        }
    }

    public void Action(int _optionToSet, int _spriteToShow)
    {
        options[_optionToSet].picked = true;
        animationController.SetSprite(6, animationController.sprites[_spriteToShow]);
        CheckPerson(peopleToJudge[personBeingJudged]);
    }
    #endregion

    public void Awake()
    {
        pseudoRandom = new System.Random(seed.GetHashCode());

        if (seed == "")
            seed = "ArgelblargbarkBOOM";

        if (waitBetweenSteps == 0.0f)
            waitBetweenSteps = 1.0f;

        cont = false;

        //Empty the list to make random people, or manually set codes in the inspector.
        if (peopleToJudge.Count == 0)
        {
            ClearPeople();
            GeneratePeople();
        }
    }

    public void Start()
    {
        animationController = GameObject.Find("Panel_LineOfPeople").GetComponent<scr_AnimationController>();

        //Starting the first loop.
        StartCoroutine(iGameLoop());
    }

    //The main gameplay loop.
    IEnumerator iGameLoop()
    {
        resultText.text = "";

        animationController.Pulse();
        yield return new WaitForEndOfFrame();

        DrawCode(new List<int>());

        yield return new WaitUntil(() => animationController.cont == true);

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

        yield return new WaitUntil(() => cont == true);

        //yield return new WaitForSeconds(waitBetweenSteps);

        cont = false;

        yield return new WaitForSeconds(waitBetweenSteps);
        StartCoroutine(iGameLoop());

        yield return null;
    }

    public void CheckPerson(cls_PersonB _person)
    {
        resultText.text = "";

        foreach (cls_Option opt in options)
        {

            if (_person.option == opt.option)
            {
                print("Right Answer was " + opt.optionName);
                resultText.text = "The correct answer was " + opt.optionName;

                if (opt.picked)
                {
                    score++;
                    opt.picked = false;
                }
            }
        }

        print("Score is now  " + score);
        scoreText.text = score.ToString();
        cont = true;
    }

    public int compareLists(List<cls_Symbol> _listA, List<int> _listB)
    {
        int match = 0;
        List<int> tempList = new List<int>();

        foreach (cls_Symbol s in _listA)
            tempList.Add(s.num);

        print("Compairing " + makeString(tempList) + " to " + makeString(_listB));

        for (int i = 0; i < _listA.Count; i++)
            for (int j = 0; j < _listB.Count; j++)
                if (tempList[i] == _listB[j])
                {
                    print("FOUND " + tempList[i]);
                    _listA[i].check = true;
                }


        foreach (cls_Symbol s in _listA)
            if (s.check)
            {
                match++;
                s.check = false;
            }
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

    public void ClearPeople()
    {
        peopleToJudge.Clear();
    }

    public void GeneratePeople()
    {
        for (int i = 0; i < 50; i++)
        {
            enum_Options answer = (enum_Options)pseudoRandom.Next(0, 3);
            print("Generated " + i + " " + answer + ".");

            //Generate a code
            List<int> tempCode = new List<int>();

            //Amount of symbols per person.
            //for (int j = 0; j < 6; j++)
            //{
            //    if (answer == 0)
            //        tempCode.Add(pseudoRandom.Next(1, 6));

            //    if (answer == 1)
            //        tempCode.Add(pseudoRandom.Next(4, 10));
            //}

            int amountOfSymbols = 6;

            //Add Random Codes
            for (int j = 0; j < amountOfSymbols/2; j++)
                tempCode.Add(pseudoRandom.Next(4, 6));

            //Add Color Codes
            if (answer == enum_Options.Blue)
                for (int j = 1; j <= 3; j++)
                    tempCode.Add(j);

            if (answer == enum_Options.Orange)
                for (int j = 7; j <= 9; j++)
                    tempCode.Add(j);

            if (answer == enum_Options.Discard) {
                tempCode.Add(pseudoRandom.Next(0,3));
                tempCode.Add(pseudoRandom.Next(0,9));
                tempCode.Add(pseudoRandom.Next(7,9));
            }

            //Scramble the code
            print("TEMPCODE" +  makeString(tempCode));

            for (int j = 0; j < amountOfSymbols; j++)
                ChangeListPlace(tempCode, pseudoRandom.Next(0, tempCode.Count), pseudoRandom.Next(0, tempCode.Count));

            peopleToJudge.Add(new cls_PersonB(answer, tempCode));
        }
    }

    void ChangeListPlace(List<int> _list, int _indexToMove, int _indexToBeMoved)
    {
        int valueA = _list[_indexToMove];
        int valueB = _list[_indexToBeMoved];

        _list[_indexToBeMoved] = valueA;
        _list[_indexToMove] = valueB;
    }

    public void DrawCode(List<int> _code)
    {
        foreach (Transform t in _Storage.Storage().symbolPanel.GetComponentInChildren<Transform>())
            Destroy(t.gameObject);

        foreach (int i in _code)
        {
            GameObject symbolClone = Instantiate(_Storage.Storage().imagePrefab, _Storage.Storage().symbolPanel.transform);
            symbolClone.GetComponentInChildren<Image>().sprite = _Storage.GetSymbols(i - 1);
        }
    }
}

[System.Serializable]
public class cls_Person
{
    public List<int> code;

    public cls_Person(List<int> _code)
    {
        code = _code;
    }
}

[System.Serializable]
public class cls_PersonB
{
    public enum_Options option;
    public List<int> code;

    public cls_PersonB(enum_Options _option, List<int> _code)
    {
        code = _code;
        option = _option;
    }
}

public enum enum_Options
{
    Orange = 1,
    Blue = 0,
    Discard = 2
}

[System.Serializable]
public class cls_Option
{
    public string optionName;
    public enum_Options option;

    //public Sprite sprite;
    public bool picked;
    //public List<cls_Symbol> mustContain;
    //public List<cls_Symbol> canContain;
    //public List<cls_Symbol> cantContain;
}

[System.Serializable]
public class cls_Symbol
{
    public int num;
    public bool check;
}