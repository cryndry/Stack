using UnityEngine;
using TMPro;
using System.Collections;

class ScoreManager : LazySingleton<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform scoreTextTransform;

    private const float maxAnimationTime = 0.4f;
    private const float maxTextScale = 2f;
    private const float minTextScale = 1f;

    private bool isAnimatingToMinScale = false;
    private bool isAnimatingToMaxScale = false;


    private int score;
    public int Score
    {
        get { return score; }
        private set
        {
            score = value;
            scoreText.text = value.ToString();
            isAnimatingToMaxScale = true;
            isAnimatingToMinScale = false;
        }
    }

    private void Update()
    {
        if (isAnimatingToMaxScale)
        {
            scoreText.transform.localScale += (maxTextScale - minTextScale) * (Time.deltaTime / maxAnimationTime) * Vector3.one;
            if (scoreText.transform.localScale.x >= maxTextScale)
            {
                isAnimatingToMaxScale = false;
                isAnimatingToMinScale = true;
                scoreTextTransform.localScale = Vector3.one * maxTextScale;
            }
        }
        else if (isAnimatingToMinScale)
        {
            scoreTextTransform.localScale -= (maxTextScale - minTextScale) * (Time.deltaTime / maxAnimationTime) * Vector3.one;
            if (scoreTextTransform.localScale.x <= minTextScale)
            {
                isAnimatingToMinScale = false;
                scoreTextTransform.localScale = Vector3.one * minTextScale;
            }
        }
    }

    private void OnEnable()
    {
        ResetScore();
        EventManager.Instance.OnBlockGenerated += IncrementScore;
        EventManager.Instance.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        ResetScore();
        EventManager.Instance.OnBlockGenerated -= IncrementScore;
        EventManager.Instance.OnGameOver -= OnGameOver;
    }

    private void IncrementScore()
    {
        Score++;
    }

    private void ResetScore()
    {
        Score = -1;
    }

    private void OnGameOver()
    {
        StartCoroutine(AnimateScoreTextToCenter());
    }

    private IEnumerator AnimateScoreTextToCenter()
    {
        Vector3 originalPosition = scoreTextTransform.localPosition;
        Vector3 targetPosition = Vector3.zero;

        float elapsedTime = 0f;
        float animationDuration = 1f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            scoreTextTransform.localPosition = Vector3.Lerp(originalPosition, targetPosition, t);
            scoreTextTransform.localScale = Vector3.Lerp(Vector3.one * minTextScale, Vector3.one * maxTextScale, t);
            
            yield return null;
        }

        scoreTextTransform.localPosition = targetPosition;
    }
}