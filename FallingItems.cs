using UnityEngine;

public class FallingItem : MonoBehaviour
{
    public enum ItemType { Fruit, RottenFruit, HappyMuffin, Barrel }
    public ItemType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.HandleItemEffect(type);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Bottom"))
        {
            Destroy(gameObject);
        }
    }
}
