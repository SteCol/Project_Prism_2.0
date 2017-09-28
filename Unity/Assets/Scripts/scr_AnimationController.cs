using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_AnimationController : MonoBehaviour
{

    public GameObject container;
    public GameObject personPrefab;
    public List<GameObject> toAnimate;
    public List<Sprite> sprites;

    public int gapSize;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            PlaceNewPerson();
        }

        ReDraw();

        foreach (GameObject g in toAnimate)
            StartCoroutine(iSlideForward(g));
    }

    public void Pulse() {
        foreach (GameObject g in toAnimate)
            StartCoroutine(iSlideForward(g));
    }

    public IEnumerator iSlideForward(GameObject _g)
    {
        for (float i = 0.0f; i < 0.1f; i = i + 0.01f)
        {
            float xPos = Mathf.Lerp((float)_g.GetComponent<RectTransform>().position.x, (float)(_g.GetComponent<RectTransform>().position.x + gapSize), i);
            _g.GetComponent<RectTransform>().position = new Vector3(xPos, _g.GetComponent<RectTransform>().position.y, _g.GetComponent<RectTransform>().position.z);

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void ReDraw()
    {
        for (int i = 0; i < toAnimate.Count; i++)
        {
            Vector3 imagePos = toAnimate[i].GetComponent<RectTransform>().position;
            toAnimate[i].GetComponent<RectTransform>().position = new Vector3(imagePos.x - (i * gapSize), imagePos.y, imagePos.z);
        }
    }

    public void PlaceNewPerson()
    {
        GameObject person = Instantiate(personPrefab, container.transform);
        person.GetComponent<RectTransform>().position = this.gameObject.transform.position;
        SetSprite(person, sprites[0]);
        toAnimate.Add(person);
    }

    public void SetSprite(GameObject _person, Sprite _sprite)
    {
        _person.GetComponent<Image>().sprite = _sprite;
    }
}
