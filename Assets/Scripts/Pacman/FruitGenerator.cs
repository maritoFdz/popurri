using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType { Cherry, Strawberry, Orange, Apple, Melon, Galaxian, Bell, Key }

public class FruitGenerator : MonoBehaviour
{
    public static FruitGenerator instance;
    private bool fruitSpawned;
    private Dictionary<FruitType, (int score, Sprite sprite)> fruits;

    [SerializeField] private Fruit fruitPrefab;
    [SerializeField] private Vector3 spawnPoint;

    [Header("Fruits Sprites")]
    [SerializeField] private Sprite cherrySprite;
    [SerializeField] private Sprite strawbSprite;
    [SerializeField] private Sprite orangeSprite;
    [SerializeField] private Sprite appleSprite;
    [SerializeField] private Sprite melonSprite;
    [SerializeField] private Sprite bellSprite;
    [SerializeField] private Sprite galaxSprite;
    [SerializeField] private Sprite keySprite;

    private void Awake()
    {
        instance = this;
        fruits = new Dictionary<FruitType, (int, Sprite)>{
            { FruitType.Cherry, (100, cherrySprite) },
            { FruitType.Strawberry, (300, strawbSprite) },
            { FruitType.Orange, (500, orangeSprite) },
            { FruitType.Apple, (700, appleSprite) },
            { FruitType.Melon, (1000, melonSprite) },
            { FruitType.Galaxian, (2000, galaxSprite) },
            { FruitType.Bell, (3000, bellSprite) },
            { FruitType.Key, (5000, keySprite) }
        };
    }

    public void Generate(int level)
    {
        if (!fruitSpawned)
        {
            FruitType type = SelectType(level);
            (int score, Sprite sprite) = fruits[type];
            Fruit fruit = Instantiate(fruitPrefab, spawnPoint, Quaternion.identity);
            fruit.SetFruit(score, sprite);
            fruitSpawned = true;
            StartCoroutine(DestroyFruit(fruit));
        }
    }

    private FruitType SelectType(int level)
    {
        return level switch
        {
            1 => FruitType.Cherry,
            2 => FruitType.Strawberry,
            3 => FruitType.Orange,
            4 => FruitType.Apple,
            5 => FruitType.Melon,
            6 => FruitType.Galaxian,
            7 => FruitType.Bell,
            _ => FruitType.Key,
        };
    }

    private IEnumerator DestroyFruit(Fruit fruit)
    {
        yield return new WaitForSeconds(10);
        Destroy(fruit.gameObject);
        fruitSpawned = false;
    }
}
