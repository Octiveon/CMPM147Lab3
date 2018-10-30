using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMapV2 : MonoBehaviour {

    public GameObject currentBlockType;

    public float amp = 3f;
    public float frq = 12;
    public float seed = 99;

    // Use this for initialization
    void Start () {
        GenerateTerrain();
	}
	
	void GenerateTerrain()
    {
        int cols = 100;
        int rows = 100;

        Vector3 myPos = transform.position;

        for(int x = 0; x < cols; x++)
        {
            for(int z = 0; z < rows; z++)
            {
                float y = 0;
                y += Mathf.PerlinNoise(myPos.x + x/frq, myPos.z + z / frq) * amp;
                GameObject newBlock = Instantiate(currentBlockType,transform);

                newBlock.transform.position = new Vector3(myPos.x + x, y, myPos.z + z);

            }
        }

    }
}
