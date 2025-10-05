using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    public AudioSource songA;
    public AudioSource songB;

    public float fadeSpeed = 1f;
    private bool playingA = true;

    void Start()
    {
        songA.Play();
        songB.Play();

        songA.volume = 1f;
        songB.volume = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playingA = !playingA;
            StopAllCoroutines();
            StartCoroutine(FadeMusic());
        }
    }

    System.Collections.IEnumerator FadeMusic()
    {
        float t = 0f;
        float startA = songA.volume;
        float startB = songB.volume;

        float targetA = playingA ? 1f : 0f;
        float targetB = playingA ? 0f : 1f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            songA.volume = Mathf.Lerp(startA, targetA, t);
            songB.volume = Mathf.Lerp(startB, targetB, t);
            yield return null;
        }
    }
}
