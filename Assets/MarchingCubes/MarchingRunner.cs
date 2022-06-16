using UnityEngine;
using System.Collections.Generic;

using ProceduralNoiseProject;

namespace MarchingCubesProject
{

    public enum MARCHING_MODE {  CUBES, TETRAHEDRON };

    public class MarchingRunner : MonoBehaviour
    {

        public Material m_material;

        public MARCHING_MODE mode = MARCHING_MODE.CUBES;

        public int seed = 0;
        int width = 40;
        int height = 40;
        int length = 40;
        List<Vector3> particles = new List<Vector3>();
        List<float> charges = new List<float>();
        float constant = 8.9875517923f;

        List<GameObject> meshes = new List<GameObject>();

        float getforce(float x, float y,  float z)
        {
            int counter = 0;
            float[] charge_arr = charges.ToArray();
            List<Vector3> vecs = new List<Vector3>();
            foreach (Vector3 p in particles)
            {
                Vector3 dir = new Vector3(x - p.x, y - p.y, z - p.z);
                float dist = Mathf.Sqrt(((p.x - x) * (p.x - x)) + ((p.y - y) * (p.y - y)) + ((p.z-z) * (p.z-z)));
                float weight = (1.6f*1.6f*constant)/(dist*dist);
                //dir.Normalize();
                dir.x *= weight;
                dir.y *= weight;
                dir.z *= weight;
                if (charge_arr[counter] < 0)
                    vecs.Add(-dir);
                else
                    vecs.Add(dir);
                counter++;
            }

            Vector3 final = new Vector3(0,0,0);

            foreach (Vector3 vec in vecs)
            {
                final += vec;
            }

            return final.magnitude;
        }

        // float gf(float x, float y,  float z) {
        //     int counter = 0;
        //     float[] charge_arr = charges.ToArray();
        //     List<Vector3> vecs = new List<Vector3>();
        //     foreach (Vector3 p in particles)
        //     {
        //         Vector3 dir = new Vector3(x - p.x, y - p.y, z - p.z);
        //         float dist = Mathf.Sqrt(((p.x - x) * (p.x - x)) + ((p.y - y) * (p.y - y)) + ((p.z-z) * (p.z-z)));
        //         float weight = (1.6f*1.6f*constant)/(dist*dist);
        //         //dir.Normalize();
        //         dir.x *= weight;
        //         dir.y *= weight;
        //         dir.z *= weight;
        //         if (charge_arr[counter] < 0)
        //             vecs.Add(-dir);
        //         else
        //             vecs.Add(dir);
        //         counter++;
        //     }

        //     Vector3 final = new Vector3(0,0,0);

        //     foreach (Vector3 vec in vecs)
        //     {
        //         final += vec;
        //     }

        //     return final.magnitude;
        // }

        void Start()
        {
            // Vector3 ptest = new Vector3(0.6f,0.4f,0.5f);
            
            // particles.Add(ptest);
            // charges.Add(-1.0f);

            // Vector3 ptest2 = new Vector3(0.6f,0.6f,0.5f);
            // particles.Add(ptest2);
            // charges.Add(-1.0f);           

            // Vector3 ptest3 = new Vector3(0.4f,0.4f,0.5f);
            // particles.Add(ptest3);
            // charges.Add(1.0f);  


            // INoise perlin = new PerlinNoise(seed, 2.0f);
            // FractalNoise fractal = new FractalNoise(perlin, 3, 1.0f);

            // //Set the mode used to create the mesh.
            // //Cubes is faster and creates less verts, tetrahedrons is slower and creates more verts but better represents the mesh surface.
            // Marching marching = null;
            // if(mode == MARCHING_MODE.TETRAHEDRON)
            //     marching = new MarchingTertrahedron();
            // else
            //     marching = new MarchingCubes();

            // //Surface is the value that represents the surface of mesh
            // //For example the perlin noise has a range of -1 to 1 so the mid point is where we want the surface to cut through.
            // //The target value does not have to be the mid point it can be any value with in the range.
            // marching.Surface = 10f;

            // //The size of voxel array.
            // int width = 80;
            // int height = 80;
            // int length = 80;

            // float[] voxels = new float[width * height * length];


            // //Fill voxels with values. Im using perlin noise but any method to create voxels will work.
            // for (int x = 0; x < width; x++)
            // {
            //     for (int y = 0; y < height; y++)
            //     {
            //         for (int z = 0; z < length; z++)
            //         {
            //             float fx = x/ (width - 1.0f);
            //             float fy = y/ (height - 1.0f);
            //             float fz = z/ (length - 1.0f);

            //             int idx = x + y * width + z * width * height;

            //             //voxels[idx] = fractal.Sample3D(fx, fy, fz);
            //             voxels[idx] = getforce(fx, fy, fz);
            //             //Debug.Log(voxels[idx]);
            //         }
            //     }
            // }

            // //Debug.Log(voxels.Length);

            // List<Vector3> verts = new List<Vector3>();
            // List<int> indices = new List<int>();

            // //The mesh produced is not optimal. There is one vert for each index.
            // //Would need to weld vertices for better quality mesh.
            // marching.Generate(voxels, width, height, length, verts, indices);

            // //A mesh in unity can only be made up of 65000 verts.
            // //Need to split the verts between multiple meshes.

            // int maxVertsPerMesh = 30000; //must be divisible by 3, ie 3 verts == 1 triangle
            // int numMeshes = verts.Count / maxVertsPerMesh + 1;

            // for (int i = 0; i < numMeshes; i++)
            // {

            //     List<Vector3> splitVerts = new List<Vector3>();
            //     List<int> splitIndices = new List<int>();

            //     for (int j = 0; j < maxVertsPerMesh; j++)
            //     {
            //         int idx = i * maxVertsPerMesh + j;

            //         if (idx < verts.Count)
            //         {
            //             splitVerts.Add(verts[idx]);
            //             splitIndices.Add(j);
            //         }
            //     }

            //     if (splitVerts.Count == 0) continue;

            //     Mesh mesh = new Mesh();
            //     mesh.SetVertices(splitVerts);
            //     mesh.SetTriangles(splitIndices, 0);
            //     mesh.RecalculateBounds();
            //     mesh.RecalculateNormals();

            //     GameObject go = new GameObject("Mesh");
            //     go.transform.parent = transform;
            //     go.AddComponent<MeshFilter>();
            //     go.AddComponent<MeshRenderer>();
            //     go.GetComponent<Renderer>().material = m_material;
            //     go.GetComponent<MeshFilter>().mesh = mesh;
            //     go.transform.localPosition = new Vector3(-width / 2, -height / 2, -length / 2);

            //     meshes.Add(go);
            //     go.transform.localScale = new Vector3(.1f,.1f,.1f);
            //     go.transform.position = new Vector3(0,0,0);
            // }

        }


        void NormalizePList()
        {
            
            float mix = 999f;
            float miy = 999f;
            float miz = 999f;
            float max = -999f;
            float may = -999f;
            float maz = -999f;

            foreach (Vector3 item in particles)
            {
                max = item.x > max ? item.x : max;
                mix = item.x < mix ? item.x : mix;
                may = item.y > may ? item.y : may;
                miy = item.y < miy ? item.y : miy;
                maz = item.z > maz ? item.z : maz;
                miz = item.z < miz ? item.z : miz;
            }

            List<Vector3> newvecs = new List<Vector3>();
            
            foreach (var item in particles)
            {
                float x = (item.x - mix) / (max - mix);
                float y = (item.y - miy) / (may - miy);
                float z = (item.z - miz) / (maz - miz);

                newvecs.Add(new Vector3(x, y, z));
            }

            particles = newvecs;
        }
      
        public List<Mesh> RunSim(List<Vector3> prtles, List<float> chrgs, int size, float thresh)
        {
            particles = prtles;
            // foreach (Vector3 item in particles)
            // {
            //     Debug.Log(item);
            // }
            //NormalizePList();

            List<Vector3> p_pos = new List<Vector3>();
            foreach (var item in particles)
            {
                float x = item.x + .5f;
                float y = item.y + .5f;
                float z = item.z + .5f;
                Vector3 vv = new Vector3(x,y,z);
                //p_pos.Add(item.gameObject.transform.localPosition * 3/2);
                p_pos.Add(vv);
            }

            particles = p_pos;

            // foreach (Vector3 item in particles)
            // {
            //     Debug.Log(item);
            // }
            charges = chrgs;
            // Vector3 ptest = new Vector3(0.6f,0.4f,0.5f);
            
            // particles.Add(ptest);
            // charges.Add(1.0f);

            // Vector3 ptest2 = new Vector3(0.6f,0.6f,0.5f);
            // particles.Add(ptest2);
            // charges.Add(-1.0f);           

            // Vector3 ptest3 = new Vector3(0.4f,0.4f,0.5f);
            // particles.Add(ptest3);
            // charges.Add(-1.0f);  
            INoise perlin = new PerlinNoise(seed, 2.0f);
            FractalNoise fractal = new FractalNoise(perlin, 3, 1.0f);

            //Set the mode used to create the mesh.
            //Cubes is faster and creates less verts, tetrahedrons is slower and creates more verts but better represents the mesh surface.
            Marching marching = null;
            if(mode == MARCHING_MODE.TETRAHEDRON)
                marching = new MarchingTertrahedron();
            else
                marching = new MarchingCubes();

            //Surface is the value that represents the surface of mesh
            //For example the perlin noise has a range of -1 to 1 so the mid point is where we want the surface to cut through.
            //The target value does not have to be the mid point it can be any value with in the range.
            marching.Surface = thresh;

            //The size of voxel array.
            width = size;
            height = size;
            length = size;

            float[] voxels = new float[width * height * length];


            //Fill voxels with values. Im using perlin noise but any method to create voxels will work.
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        float fx = x/ (width - 1.0f);
                        float fy = y/ (height - 1.0f);
                        float fz = z/ (length - 1.0f);

                        int idx = x + y * width + z * width * height;

                        //voxels[idx] = fractal.Sample3D(fx, fy, fz);
                        voxels[idx] = getforce(fx, fy, fz);
                        //Debug.Log(voxels[idx]);
                    }
                }
            }

            //Debug.Log(voxels.Length);

            List<Vector3> verts = new List<Vector3>();
            List<int> indices = new List<int>();


            //The mesh produced is not optimal. There is one vert for each index.
            //Would need to weld vertices for better quality mesh.
            marching.Generate(voxels, width, height, length, verts, indices);

            //A mesh in unity can only be made up of 65000 verts.
            //Need to split the verts between multiple meshes.

            int maxVertsPerMesh = 30000; //must be divisible by 3, ie 3 verts == 1 triangle
            int numMeshes = verts.Count / maxVertsPerMesh + 1;

            List<Mesh> meshi = new List<Mesh>();

            for (int i = 0; i < numMeshes; i++)
            {

                List<Vector3> splitVerts = new List<Vector3>();
                List<int> splitIndices = new List<int>();

                for (int j = 0; j < maxVertsPerMesh; j++)
                {
                    int idx = i * maxVertsPerMesh + j;

                    if (idx < verts.Count)
                    {

                        //edit vertices
                        Vector3 vert  = verts[idx];
                        vert.x = -0.5f + vert.x * (1/(float)width);
                        vert.y = -0.5f + vert.y * (1/(float)height);
                        vert.z = -0.5f + vert.z * (1/(float)length);
                        splitVerts.Add(vert);
                        splitIndices.Add(j);
                    }
                }

                if (splitVerts.Count == 0) continue;
                Mesh mesh = new Mesh();
                mesh.SetVertices(splitVerts);
                mesh.SetTriangles(splitIndices, 0);
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                meshi.Add(mesh);

                // GameObject go = new GameObject("Mesh");
                // go.transform.parent = transform;
                // go.AddComponent<MeshFilter>();
                // go.AddComponent<MeshRenderer>();
                // go.GetComponent<Renderer>().material = m_material;
                // go.GetComponent<MeshFilter>().mesh = mesh;
                // go.transform.localPosition = new Vector3(-width / 2, -height / 2, -length / 2);

                // meshes.Add(go);
            }

            return meshi;
        }
        void Update()
        {
            //transform.Rotate(Vector3.up, 10.0f * Time.deltaTime);
        }

        public void ToVTKFile(List<Vector3> prtles, List<float> chrgs, int size)
        {
            List<Vector3> p_pos = new List<Vector3>();
            foreach (var item in particles)
            {
                float x = item.x + .5f;
                float y = item.y + .5f;
                float z = item.z + .5f;
                Vector3 vv = new Vector3(x,y,z);
                p_pos.Add(vv);
            }

            particles = p_pos;

            charges = chrgs;

            width = size;
            height = size;
            length = size;

            float[] voxels = new float[width * height * length];

            string file_string = "# vtk Force datafile 1.0\nThis file contains force data genereated between particles\nASCII\nDATASET STRUCTURED_GRID\nDIMENTIONS "+size+" "+size+" "+size+" "+size+"\nPOINTS "+size*size*size+" float\n";


            //Fill voxels with values. Im using perlin noise but any method to create voxels will work.
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        float fx = x/ (width - 1.0f);
                        float fy = y/ (height - 1.0f);
                        float fz = z/ (length - 1.0f);

                        int idx = x + y * width + z * width * height;

                        //append point to file
                        file_string += fx + " " + fy+ " " + fz+ " " + getforce(fx, fy, fz)+ "\n";
                    }
                }
            }

            
            System.IO.File.WriteAllText("force.vtk", file_string);
        }

    }

}
