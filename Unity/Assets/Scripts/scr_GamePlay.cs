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
    public scr_AnimationController animCont;


    #region Buttons & Sizzle

    public void OrangeButton()
    {
        if (animCont.cont)
            Action(1, 1);
        //options[1].picked = true;
        ////scr_AnimationController animCont = GameObject.Find("Panel_LineOfPeople").GetComponent<scr_AnimationController>();
        //animCont.SetSprite(6, animCont.sprites[1]);
        //CheckPerson(peopleToJudge[personBeingJudged]);
    }

    public void BlueButton()
    {
        if (animCont.cont)
            Action(0, 2);
    }

    public void DiscardButton()
    {
        if (animCont.cont)
        {

            //options[2].picked = true;
            resultText.text = "DISCARDED";

            //scr_AnimationController animCont = GameObject.Find("Panel_LineOfPeople").GetComponent<scr_AnimationController>();
            animCont.SetSprite(6, animCont.sprites[3], 2.0f);
            CheckPerson(peopleToJudge[personBeingJudged]);
        }
    }

    public void Action(int _optionToSet, int _spriteToShow)
    {
        options[_optionToSet].picked = true;
        animCont.SetSprite(6, animCont.sprites[_spriteToShow]);
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

        ClearPeople();
        GeneratePeople();
    }

    public void Start()
    {

        scr_AnimationController animCont = GameObject.Find("Panel_LineOfPeople").GetComponent<scr_AnimationController>();

        //Starting the first loop.
        /*
        scr_AnimationController animCont = GameObject.Find("Panel_LineOfPeople").GetComponent<scr_AnimationController>();

        print("GameStart");
        codeToCheck = peopleToJudge[personBeingJudged].code;
        print("NEW PERSON: " + makeString(codeToCheck));
        DrawCode(codeToCheck);
        codeText.text = makeString(codeToCheck);
        scoreText.text = score.ToString();
        */

        StartCoroutine(iGameLoop());
    }

    //The main gameplay loop.
    IEnumerator iGameLoop()
    {
        resultText.text = "";

        GameObject.Find("Panel_LineOfPeople").GetComponent<scr_AnimationController>().Pulse();
        yield return new WaitForEndOfFrame();

        DrawCode(new List<int>());

        yield return new WaitUntil(() => GameObject.Find("Panel_LineOfPeople").GetComponent<scr_AnimationController>().cont == true);

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

    //public void NextPerson()
    //{
    //    if (personBeingJudged < peopleToJudge.Count)
    //    {
    //        personBeingJudged++;
    //        codeToCheck.Clear();
    //        codeToCheck = peopleToJudge[personBeingJudged].code;
    //        print("NEW PERSON: " + makeString(codeToCheck));
    //        DrawCode(codeToCheck);
    //        codeText.text = makeString(codeToCheck);
    //    }
    //    else
    //        print("Game End");
    //}

    public void CheckPerson(cls_Person _person)
    {
        scr_AnimationController animCont = GameObject.Find("Panel_LineOfPeople").GetComponent<scr_AnimationController>();

        resultText.text = "";

        foreach (cls_Option opt in options)
        {
            //animCont.SetSprite(6, animCont.sprites[3]);

            print("Checking If " + opt.optionName + " is Correct");
            if (compareLists(opt.mustContain, codeToCheck) == opt.mustContain.Count && compareLists(opt.cantContain, codeToCheck) == 0)
            {
                print("Right Answer was " + opt.optionName);
                resultText.text = "The correct answer was " + opt.optionName;

                if (opt.picked)
                {
                    score++;
                    opt.picked = false;
                    //animCont.SetSprite(6, opt.sprite);
                }

                break;
            }
            else
            {
                opt.picked = false;
                print(opt.optionName + " was wrong");
            }
        }

        print("Score is now  " + score);
        scoreText.text = score.ToString();
        cont = true;
        //NextPerson();
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
            int answer = pseudoRandom.Next(0, 2);
            print("Generated " + i + " " + answer + ".");
            List<int> tempCode = new List<int>();

            for (int j = 0; j < 6; j++)
            {
                if (answer == 0)
                    tempCode.Add(pseudoRandom.Next(1, 6));

                if (answer == 1)
                    tempCode.Add(pseudoRandom.Next(4, 10));
            }

            peopleToJudge.Add(new cls_Person(tempCode));
        }
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
public class cls_Option
{
    public string optionName;
    public Sprite sprite;
    public bool picked;
    public List<cls_Symbol> mustContain;
    //public List<cls_Symbol> canContain;
    public List<cls_Symbol> cantContain;
}

[System.Serializable]
public class cls_Symbol
{
    public int num;
    public bool check;
}