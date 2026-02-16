using System.Collections;
using UnityEngine;

public class FruitGenerator : MonoBehaviour
{
    public static FruitGenerator instance;
    private bool fruitSpawned;

    [Header("Fruits Data")]
    [SerializeField] private Fruit fruitPrefab;
    [SerializeField] private FruitData cherry;
    [SerializeField] private FruitData strawberry;
    [SerializeField] private FruitData orange;
    [SerializeField] private FruitData apple;
    [SerializeField] private FruitData melon;
    [SerializeField] private FruitData galaxian;
    [SerializeField] private FruitData bell;
    [SerializeField] private FruitData key;
    [SerializeField] private Vector3 spawnPoint;

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        else instance = this;
    }

    public void Generate(int level)
    {
        if (fruitSpawned)
            return;
        FruitData data = level switch
        {
            1 => cherry,
            2 => strawberry,
            3 or 4 => orange,
            5 or 6 => apple,
            7 or 8 => melon,
            9 or 10 => galaxian,
            11 or 12 => bell,
            _ => key,
        };
        Fruit currentFruit = Instantiate(fruitPrefab, spawnPoint, Quaternion.identity);
        currentFruit.SetFruit(data);
        fruitSpawned = true;
        StartCoroutine(DestroyFruit(currentFruit));
    }

    private IEnumerator DestroyFruit(Fruit fruit)
    {
        yield return new WaitForSeconds(10);
        Destroy(fruit.gameObject);
        fruitSpawned = false;
    }
}
