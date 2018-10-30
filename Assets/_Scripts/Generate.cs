using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

    public GameObject[] blocks;

    public float[] chanceOfSpawning;
    public float chanceIncrease;


    public int[][] map;
    public int mapSize = 5;
    public bool generating = false;

    private float[] startingChances;


    private void Start()
    {
        startingChances = chanceOfSpawning;
        map = new int[mapSize][];
        for(int i =0; i < mapSize; i++)
        {
            map[i] = new int[mapSize];
        }

        GenerateBlocks();
    }

    private void GenerateBlocks()
    {
        for(int i = 0; i < mapSize; i++)
        {
            for(int j =0; j < mapSize; j++)
            {
                int num = Random.Range(0, blocks.Length - 1);
                float percent = Random.Range(0, 100);

                while(percent >= chanceOfSpawning[num])
                {
                    Debug.Log("Increasing Chances");
                    chanceOfSpawning[num] += chanceIncrease;
                    num = Random.Range(0, blocks.Length - 1);
                    percent = Random.Range(0, 100);
                }
                chanceOfSpawning[num] = startingChances[num];


                map[i][j] = num;
            }
        }

        InstantiateBlocks();
    }

    private void InstantiateBlocks()
    {
        for (int i = -mapSize / 2; i < mapSize/2; i++)
        {
            for (int j = -mapSize / 2; j < mapSize/2; j++)
            {
                Instantiate(blocks[map[(i + mapSize/2)][(j + mapSize / 2 )]], new Vector3(2 * i, 0.1f, 2 * j), new Quaternion(0,0,0,0),transform);
            }
        }
    }

}
