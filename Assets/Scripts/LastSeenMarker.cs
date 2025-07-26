using UnityEngine;

public class LastSeenMarker : MonoBehaviour
{
    public float fadeDuration = 2f;
    private SpriteRenderer sr;
    private float timer;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / fadeDuration;
        if (t >= 1f)
            Destroy(gameObject);
        else
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;
        }
    }
}