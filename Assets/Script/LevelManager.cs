using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject[] ElementPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ElementSpawn(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ElementSpawn(int count)
    {
        float StartX = -8;
        float StartY = 17;
        float X = StartX;
        float Y = StartY;
        float MaxX = 9;

        for(int i = 0; i < count; i++)
        {
            float rndX = Random.Range(1f,3f);
            float rndY = Random.Range(-1f,1f);
            X += rndX;
            Y += rndY;
            
            var Position = new Vector3(X, Y, 0);
            Instantiate(ElementPrefabs[Random.Range(0,ElementPrefabs.Length)],Position,Quaternion.identity);

            if (X >= MaxX)
            {
                X = StartX;
                Y += 2;
            }
        }
    }
}
