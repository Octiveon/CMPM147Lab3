using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionGenerate : MonoBehaviour {

    public VoxelMap[] maps;
    public GameObject[] Interactables;
    public GameObject parent;
    public int itemsPerMap = 4;

    int[][] map;

    // Use this for initialization
    void Start()
    {
        maps = GetComponentsInChildren<VoxelMap>();

        foreach (VoxelMap m in maps)
        {
            map = m.tileMap;

            for(int i = 0; i < itemsPerMap; i++)
            {
                int x = Random.Range(0, 49);
                int z = Random.Range(0, 49);

                int interact = Random.Range(0, Interactables.Length-1);

                Instantiate(Interactables[interact], m.gameObject.transform.position + new Vector3(x, 0, z), m.gameObject.transform.rotation, parent.transform);


            }
                                  
        }



    }
	
	
}
