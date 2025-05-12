using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public Slider hungerBar;
    public Slider happinessBar;

    [Header("Background")]
    public SpriteRenderer backgroundRenderer;
    public Sprite defaultBackground;
    public Sprite illusionBackground;

    [Header("Game Over")]
    public string gameOverSceneName = "GameOverScene";

    [Header("Stats")]
    private float hunger = 100f;
    private float happiness = 100f;
    public float hungerDecayRate = 1f;
    public float happinessDecayRate = 0.5f;
    private bool gameOver = false;

    [Header("Illusion Mode")]
    public bool isIllusionMode = false;
    public float illusionChance = 0.3f; 
    public float illusionDuration = 10f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InvokeRepeating(nameof(DrainStats), 1f, 1f);
        InvokeRepeating(nameof(CheckIllusionMode), 15f, 15f); 

        UpdateUI();
    }

    void DrainStats()
    {
        if (gameOver) return;

        hunger -= hungerDecayRate;
        happiness -= happinessDecayRate;

        UpdateUI();
        CheckGameOver();
    }

    public void HandleItemEffect(FallingItem.ItemType type)
    {
        if (gameOver) return;

        if (!isIllusionMode)
        {
            switch (type)
            {
                case FallingItem.ItemType.Fruit:
                    hunger += 10f;
                    break;
                case FallingItem.ItemType.RottenFruit:
                    hunger -= 25f;
                    break;
                case FallingItem.ItemType.HappyMuffin:
                    happiness += 15f;
                    break;
                case FallingItem.ItemType.Barrel:
                    happiness -= 25f;
                    break;
            }
        }
        else // Illusion
        {
            switch (type)
            {
                case FallingItem.ItemType.Fruit:
                    hunger -= 20f;
                    break;
                case FallingItem.ItemType.RottenFruit:
                    hunger += 15f;
                    break;
                case FallingItem.ItemType.HappyMuffin:
                    happiness -= 20f;
                    break;
                case FallingItem.ItemType.Barrel:
                    happiness += 15f;
                    break;
            }
        }

        hunger = Mathf.Clamp(hunger, 0f, 100f);
        happiness = Mathf.Clamp(happiness, 0f, 100f);
        UpdateUI();
        CheckGameOver();
    }

    void UpdateUI()
    {
        hungerBar.value = hunger;
        happinessBar.value = happiness;
    }

    void CheckGameOver()
    {
        if (hunger <= 0 || happiness <= 0)
        {
            gameOver = true;
            Debug.Log("Game Over!");
            SceneManager.LoadScene(gameOverSceneName); 
        }
    }

    void CheckIllusionMode()
    {
        if (!isIllusionMode && Random.value < illusionChance)
        {
            ActivateIllusionMode();
        }
    }

    void ActivateIllusionMode()
    {
        isIllusionMode = true;
        Debug.Log("Illusion activated!");

        if (backgroundRenderer != null && illusionBackground != null)
            backgroundRenderer.sprite = illusionBackground;

        Invoke(nameof(DeactivateIllusionMode), illusionDuration);
    }

    void DeactivateIllusionMode()
    {
        isIllusionMode = false;
        Debug.Log("Illusion end");

        if (backgroundRenderer != null && defaultBackground != null)
            backgroundRenderer.sprite = defaultBackground;
    }
    void ResizeBackgroundToSprite(Sprite sprite)
    {
        if (sprite == null || backgroundRenderer == null) return;

        Vector2 size = sprite.bounds.size;
        backgroundRenderer.transform.localScale = new Vector3(
            10f / size.x, 10f / size.y, 1f
        );
    }
}
