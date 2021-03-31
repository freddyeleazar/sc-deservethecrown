using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CellPhoneCinematicController : MonoBehaviour
{
    public EndType endType;
    public GameObject player;
    public float secondsPerImage = 2;

    public List<Sprite> getArrestedSprites;
    public List<Sprite> getSickSprites;
    private Image cellPhoneImage;

    public Image tryAgainImage;
    public Image exitImage;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovementController>().gameObject;
        cellPhoneImage = GetComponent<Image>();
        tryAgainImage = GetComponentsInChildren<Image>().ToList().Find(t => t.gameObject.name == "Try Again");
        exitImage = GetComponentsInChildren<Image>().ToList().Find(t => t.gameObject.name == "Exit");

        tryAgainImage.gameObject.SetActive(false);
        exitImage.gameObject.SetActive(false);
    }

    public IEnumerator PlayCinematic(EndType endType)
    {
        switch (endType)
        {
            case EndType.GetArrested:
                foreach (Sprite getArrestedSprite in getArrestedSprites)
                {
                    yield return new WaitForSecondsRealtime(secondsPerImage);
                    cellPhoneImage.sprite = getArrestedSprite;
                    cellPhoneImage.color = Color.white;
                    if(getArrestedSprites.IndexOf(getArrestedSprite) == getArrestedSprites.Count - 1)
                    {
                        tryAgainImage.gameObject.SetActive(true);
                        exitImage.gameObject.SetActive(true);
                    }
                }
                break;
            case EndType.GetSick:
                foreach (Sprite getSickSprite in getSickSprites)
                {
                    yield return new WaitForSecondsRealtime(secondsPerImage);
                    cellPhoneImage.sprite = getSickSprite;
                    cellPhoneImage.color = Color.white;
                    if (getSickSprites.IndexOf(getSickSprite) == getSickSprites.Count - 1)
                    {
                        tryAgainImage.gameObject.SetActive(true);
                        exitImage.gameObject.SetActive(true);
                    }
                }
                break;
            default:
                break;
        }
    }
}

public enum EndType
{
    GetArrested,
    GetSick
}
