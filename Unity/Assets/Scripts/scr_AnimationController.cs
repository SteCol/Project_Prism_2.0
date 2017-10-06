using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_AnimationController : MonoBehaviour
{
    public GameObject container;
    public GameObject personPrefab;
    public GameObject machineHead;
    public List<GameObject> toAnimate;
    public List<Sprite> sprites;

    public bool cont;
    public int gapSize;

    void Start()
    {
        print(Screen.width);
        gapSize = (Screen.width / 834) * 100;
        cont = true;

        for (int i = 0; i < 15; i++)
            PlaceNewPerson(-6);
    }

    public void Pulse()
    {
        PlaceNewPerson(-6);
        GameObject tempObj = toAnimate[0];
        toAnimate.RemoveAt(0);
        //Destroy(tempObj);

        foreach (GameObject g in toAnimate)
            StartCoroutine(iSlideForward(g));
    }

    public IEnumerator iSlideForward(GameObject _g)
    {
        cont = false;
        float animTime = 0.5f;

        Vector3 headStartPos = machineHead.transform.position;

        //Move Machinehead Up
        //for (float i = 0.0f; i < 1.0f; i+= animTime * Time.deltaTime) {
        //    float yPos = Mathf.Lerp(headStartPos.y, headStartPos.y + (Screen.height/3), i);
        //    machineHead.GetComponent<RectTransform>().position = new Vector3(headStartPos.x, yPos, headStartPos.z);
        //    yield return new WaitForEndOfFrame();
        //}

        //Move Conveyor belt
        Vector3 startPos = _g.GetComponent<RectTransform>().position;
        for (float i = 0.0f; i < 1.0f; i += animTime * Time.deltaTime)
        {
            float xPos = Mathf.Lerp(startPos.x, (float)(startPos.x + gapSize), i);
            _g.GetComponent<RectTransform>().position = new Vector3(xPos, _g.GetComponent<RectTransform>().position.y, _g.GetComponent<RectTransform>().position.z);

            yield return new WaitForEndOfFrame();
        }

        //Move Machinehead Down
        //for (float i = 0.0f; i < 1.0f; i += animTime * Time.deltaTime)
        //{
        //    float yPos = Mathf.Lerp(headStartPos.y + (Screen.height / 3), headStartPos.y, i);
        //    machineHead.GetComponent<RectTransform>().position = new Vector3(headStartPos.x, yPos, headStartPos.z);
        //    yield return new WaitForEndOfFrame();

        //}

        cont = true;
        yield return null;
    }

    public void ReDraw()
    {
        for (int i = 0; i < toAnimate.Count; i++)
        {
            Vector3 imagePos = toAnimate[i].GetComponent<RectTransform>().position;
            toAnimate[i].GetComponent<RectTransform>().position = new Vector3((imagePos.x - (i * gapSize)), imagePos.y, imagePos.z);
        }
    }

    public void PlaceNewPerson(float _offset)
    {
        GameObject person = Instantiate(personPrefab, container.transform);
        person.GetComponent<RectTransform>().position = new Vector3((this.gameObject.transform.position.x - (toAnimate.Count + _offset) * gapSize), this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        person.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        SetSprite(person, sprites[0]);
        toAnimate.Add(person);
    }

    public void SetSprite(GameObject _person, Sprite _sprite)
    {
        _person.GetComponent<Image>().sprite = _sprite;
    }

    public void SetSprite(int _i, Sprite _sprite)
    {
        toAnimate[_i].GetComponent<Image>().sprite = _sprite;
    }
}
