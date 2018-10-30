using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]


public class VoxelMap : MonoBehaviour {

    [Header("Genreal Map Generation Properties")]
    public GameObject[] blocks;
    public Vector3 voxelSize = new Vector3(1, 1, 1);

    public int xVoxels = 4;
    public int zVoxels = 4;


    public float amp = 3f;
    public float frq = 12;
    public float seed = 99;

    public MeshFilter myMeshFilter;

    [Header("Stone Generation Properties")]
    public int stoneRepeatMin = 2;
    public int stoneRepeatMax = 4;

    public int stoneMinimumSides = 2;
    public int stoneMinimumConnected = 6;
    public int stoneMaximumConnected = 25;

    [Header("Sand Generation Properties")]
    public int sandRepeatMin = 2;
    public int sandRepeatMax = 4;
    public int sandMinimumSides = 2;
    public int sandMinimumConnected = 6;
    public int sandMaximumConnected = 25;



    public int[][] tileMap;
    private int water = 0;
    private int grass = 1;
    private int stone = 2;
    private int sand = 3;

    // Use this for initialization
    void Start () {
        myMeshFilter = GetComponent<MeshFilter>();

        GenerateGrid();

    }

    void GenerateGrid()
    {
        GameObject[] voxels = new GameObject[xVoxels * zVoxels];

        Vector3 oPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        GenerateTiles();

        int i = -1;

        for(int x = 0; x < xVoxels; x++)
        {
            for(int z = 0; z < zVoxels; z++)
            {
                i++;
                voxels[i] = Instantiate(blocks[tileMap[x][z]],transform);

                oPos = this.transform.position;
                oPos.y = 0f;
                oPos.x -= xVoxels / 2 * voxelSize.x;
                oPos.z -= zVoxels / 2 * voxelSize.z;

                oPos.x += x * voxelSize.x;
                oPos.z += z * voxelSize.z;

                if(tileMap[x][z] == water)//If water
                {
                    oPos.y += Mathf.PerlinNoise(((this.transform.position.x + oPos.x)) / frq,
                  ((this.transform.position.z + oPos.z)) / frq) * -amp;
                }
                else
                {
                   oPos.y += Mathf.PerlinNoise(((this.transform.position.x + oPos.x)) / frq,
                   ((this.transform.position.z + oPos.z)) / frq) * amp;
                }
               

                voxels[i].transform.position = oPos;
                voxels[i].transform.localScale = voxelSize;
                voxels[i].transform.parent = transform;

            }
        }

        CombineMeshes();

        foreach(GameObject voxel in voxels)
        {
            Destroy(voxel);
        }
    }

    void GenerateGrassTiles()
    {

        tileMap = new int[50][];

        //Create 2d array map of tile indexs
        // 0 is Water
        // 1 is Grass
        // 2 is Stone
        // 3 is Sand

        for(int i = 0; i < xVoxels; i++)
        {
            tileMap[i] = new int[xVoxels];
            //Fill map with grass tiles
            for(int j = 0; j < xVoxels; j++)
            {
                tileMap[i][j] = 1;
            }
        }

        //Generate Stone

        /*
         * Conditions for stones
         * C1: Must be connected on 2 sides by stone.
         * C2: Must be connected to at least 6 tiles and at most 10
         */

        int x = Random.Range(0, xVoxels);
        int y = Random.Range(0, xVoxels);

    }

    void GenerateStoneTiles()
    {

        int[] xChoices;
        int[] yChoices;
        int[] connections;

        int index = 0;


        //Generate Stone

        /*
         * Conditions for stones
         * C1: Must be connected on 2 sides by stone.
         * C2: Must be connected to at least 6 tiles and at most 10
         */


        int repeat = Random.Range(stoneRepeatMin,stoneRepeatMax); 

        for(int i = 0; i < repeat; i++)
        {
            index = 0;
            xChoices = new int[Random.Range(stoneMinimumConnected, stoneMaximumConnected)];
            yChoices = new int[xChoices.Length];
            connections = new int[xChoices.Length];

            int x = Random.Range(2, xVoxels - 2);
            int y = Random.Range(2, xVoxels - 2);
            int maxConnection = Random.Range(1, stoneMaximumConnected);

            // Select Random Starting point within outside ring
            xChoices[index] = x;
            yChoices[index] = y;

            index++;
            //Set that point to stone
            tileMap[x][y] = stone;

            for (int j =0; j < xChoices.Length-1; j++)
            {
                int trys = 0;
                //Select one of current tile choices
                int choice = Random.Range(0, index-1);
               
                x = xChoices[choice];
                y = yChoices[choice];

                if(connections[choice] >= maxConnection)
                {
                    while(connections[choice] >= maxConnection)
                    {
                        trys++;
                        choice = Random.Range(0, index - 1);

                        x = xChoices[choice];
                        y = yChoices[choice];

                        if (trys > xChoices.Length * 3)
                        {
                            break;
                        }
                    }
                }

                //Choose a direction 0 up, 1 down, 2 left, 3 right;

                int xdirection = Random.Range(-1, 1);
                int ydirection = Random.Range(-1, 1);

               
                int newX = x + xdirection;
                int newY = y + ydirection;

                if(newX < 0) { newX += 1; }
                else if(newX >=50)  { newX -= 1; }

                if(newY < 0){ newY += 1;}
                else if (newY >=50)  { newY -= 1; }


                if (tileMap[newX][newY] == stone)
                {
                    trys = 0;
                    while (tileMap[newX][newY] == stone)
                    {
                        xdirection = Random.Range(-1, 1);
                        ydirection = Random.Range(-1, 1);

                    

                        newX = x + xdirection;
                        newY = y + ydirection;

                        if (newX < 0) { newX += 1; }
                        else if (newX >= 50) { newX -= 1; }

                        if (newY < 0) { newY += 1; }
                        else if (newY >= 50) { newY -= 1; }

                        if (trys > 16)
                        {
                            break;
                        }
                        trys++;
                    }
                }


                tileMap[newX][newY] = stone;


                xChoices[index] = newX;
                yChoices[index] = newY;

                connections[choice] += 1;
                index++;

            }
        }
    }

    void GenerateSandTiles()
    {

        int[] xChoices;
        int[] yChoices;
        int[] connections;

        int index = 0;


        //Generate Sand

       


        int repeat = Random.Range(sandRepeatMin, sandRepeatMax);

        for (int i = 0; i < repeat; i++)
        {
            index = 0;
            xChoices = new int[Random.Range(sandMinimumConnected, sandMaximumConnected)];
            yChoices = new int[xChoices.Length];
            connections = new int[xChoices.Length];

            int x = Random.Range(2, xVoxels - 2);
            int y = Random.Range(2, xVoxels - 2);
            int maxConnection = Random.Range(1, sandMaximumConnected);

            // Select Random Starting point within outside ring
            xChoices[index] = x;
            yChoices[index] = y;

            index++;
            //Set that point to stone
            tileMap[x][y] = sand;

            for (int j = 0; j < xChoices.Length - 1; j++)
            {
                int trys = 0;
                //Select one of current tile choices
                int choice = Random.Range(0, index - 1);

                x = xChoices[choice];
                y = yChoices[choice];

                if (connections[choice] >= maxConnection)
                {
                    while (connections[choice] >= maxConnection)
                    {
                        trys++;
                        choice = Random.Range(0, index - 1);

                        x = xChoices[choice];
                        y = yChoices[choice];



                        if (trys > xChoices.Length * 3)
                        {
                            break;
                        }
                    }
                }

                //Choose a direction 0 up, 1 down, 2 left, 3 right;

                int xdirection = Random.Range(-1, 1);
                int ydirection = Random.Range(-1, 1);


                int newX = x + xdirection;
                int newY = y + ydirection;

                if (newX < 0) { newX += 1; }
                else if (newX >= 50) { newX -= 1; }

                if (newY < 0) { newY += 1; }
                else if (newY >= 50) { newY -= 1; }


                if (tileMap[newX][newY] == sand)
                {
                    trys = 0;
                    while (tileMap[newX][newY] == sand)
                    {
                        xdirection = Random.Range(-1, 1);
                        ydirection = Random.Range(-1, 1);



                        newX = x + xdirection;
                        newY = y + ydirection;

                        if (newX < 0) { newX += 1; }
                        else if (newX >= 50) { newX -= 1; }

                        if (newY < 0) { newY += 1; }
                        else if (newY >= 50) { newY -= 1; }

                        if (trys > 16)
                        {
                            break;
                        }
                        trys++;
                    }
                }


                tileMap[newX][newY] = sand;


                xChoices[index] = newX;
                yChoices[index] = newY;

                connections[choice] += 1;
                index++;

            }
        }
    }

    void GenerateTiles()
    {
        GenerateGrassTiles();
        GenerateStoneTiles();
        GenerateSandTiles();
        Debug.Log("Finished Generation");

    }

    void CombineMeshes()
    {
        /* Multiple texture code used from
         * https://www.youtube.com/watch?v=6APzUgckV7U by Craig Perko
         */

        Vector3 oldPos = transform.position;
        Quaternion oldQuat = transform.rotation;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>(false);

        List<Material> materials = new List<Material>();
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(false);

        foreach(MeshRenderer renderer in renderers)
        {
            if(renderer.transform == transform) { continue; }

            Material[] localMats = renderer.sharedMaterials;
            foreach (Material localMat in localMats)
            {
                if (!materials.Contains(localMat)) { materials.Add(localMat); }
            }
        }

        List<Mesh> submeshes = new List<Mesh>();

        foreach(Material material in materials)
        {
            List<CombineInstance> combiners = new List<CombineInstance>();
            int count = 0;

            foreach (MeshFilter filter in filters)
            {
                if (filter.transform == transform) { continue; }


                MeshRenderer renderer = filter.GetComponent<MeshRenderer>();
                if(renderer == null)
                {
                    Debug.LogError(filter.name + " has no MeshRenderer");
                    continue;
                }

                Material[] localMaterials = renderer.sharedMaterials;



                for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++)
                {
                    if(localMaterials[materialIndex] != material) { count++; continue; }

                    CombineInstance ci = new CombineInstance();

                    ci.mesh = filter.sharedMesh;
                    ci.subMeshIndex = materialIndex;
                    ci.transform = filters[count++].transform.localToWorldMatrix;
                  
                    combiners.Add(ci);
                }
            }
            //Flatten into single mesh.
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combiners.ToArray(), true);

            submeshes.Add(mesh);
        }

        List<CombineInstance> finalCombiners = new List<CombineInstance>();

        for (int meshIndex = 0; meshIndex < submeshes.Count; meshIndex++)
        {
            CombineInstance ci = new CombineInstance
            {
                mesh = submeshes[meshIndex],
                subMeshIndex = 0,
                transform = Matrix4x4.identity
            };


            finalCombiners.Add(ci);
        }


        //Mesh finalMesh = new Mesh();
        //finalMesh.CombineMeshes(finalCombiners.ToArray(), false);
        MeshFilter msh = transform.GetComponent<MeshFilter>();

        msh.mesh = new Mesh();
        msh.mesh.CombineMeshes(finalCombiners.ToArray(), false);
        msh.mesh.RecalculateBounds();
        msh.mesh.RecalculateNormals();

       // GetComponent<MeshFilter>().sharedMesh = finalMesh;

        transform.rotation = oldQuat;
        transform.position = oldPos;

        Debug.Log("Final Mesh has " + submeshes.Count + "materials.");
        gameObject.AddComponent<MeshCollider>();

        /*CombineInstance[] combined = new CombineInstance[filters.Length];



        for(int i = filters.Length - 1; i >= 0; i--)
        {
            combined[i].mesh = filters[i].sharedMesh;
            combined[i].transform = filters[i].transform.localToWorldMatrix;
            filters[i].gameObject.SetActive(false);
        }

        MeshFilter msh = transform.GetComponent<MeshFilter>();

        msh.mesh = new Mesh();
        msh.mesh.CombineMeshes(combined, false);
        msh.mesh.RecalculateBounds();
        msh.mesh.RecalculateNormals();

        gameObject.AddComponent<MeshCollider>();
        */
    }


}
