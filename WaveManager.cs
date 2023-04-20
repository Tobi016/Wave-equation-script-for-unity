using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Material waveMaterial;
    public Texture2D waveTexture;
    float[][] waveN, waveNm1, waveNp1; //informacion de estado
    float Lx = 10;//ancho
    float Ly = 10;//alto
    [SerializeField] float dx = 0.1f; //densidad del eje x
    float dy { get => dx; } // densidad del eje y
    int nx, ny; // resolucion
    //variables para ecuacion de onda
    public float CFL = 0.05f;
    public float c = 1;
    float dt; //cambio en el tiempo
    float t; //tiempo actual
    // Start is called before the first frame update
    void Start()
    {

        nx = Mathf.FloorToInt(Lx / dx);
        ny = Mathf.FloorToInt(Ly / dy);
        waveTexture = new Texture2D(nx, ny, TextureFormat.RGBA32, false);
        waveN = new float[nx][];
        waveNm1 = new float[nx][];
        waveNp1 = new float[nx][];
        //inicializacion de las variables de informacion de estado
        for (int i = 0; i < nx; i++)
        {
            waveN[i] = new float[ny];
            waveNm1[i] = new float[ny];
            waveNp1[i] = new float[ny];
        }
        waveMaterial.SetTexture("_MainTex", waveTexture);//coloring texture
        waveMaterial.SetTexture("_Displacement", waveTexture);//displacement texture




    }
    void WaveStep()
    {

        dt = CFL * dx / c; //recalculamos dt
        t += dx; //incrementamos el tiempo
        //copia el contenido del estado de onda anterior al estado de onda actual, i es el estado anterior y j es el actual
        for (int i = 0; i < nx; i++)
        {
            for (int j = 0; j < ny; j++)
            {
                waveNm1[i][j] = waveN[i][j]; // copia el estado en N a N-1
                waveN[i][j] = waveNp1[i][j];// copia el es estado N+1 A N 
            }

        }
        for (int i = 1; i < nx - 1; i++)
        { //no procesa las esquinas de la textura    
            for (int j = 1; j < ny - 1; j++)
            {
                float n_ij = waveN[i][j];
                float n_ip1j = waveN[i + 1][j];
                float n_im1j = waveN[i - 1][j];
                float n_ijp1 = waveN[i][j + 1];
                float n_ijm1 = waveN[i][j - 1];
                float nm1_ij = waveNm1[i][j];
                waveNp1[i][j] = 2f * n_ij - nm1_ij + CFL * CFL * (n_ijm1 + n_ijp1 + n_im1j + n_ip1j - 4f * n_ij);//ecuacion de onda

            }

        }
    }
    void ApplyMatrixToTexture(float[][] state, ref Texture2D tex)
    {
        for (int i = 0; i < nx; i++)
        {
            for (int j = 0; j < ny; j++)
            {
                float val = state[i][j];
                tex.SetPixel(i, j, new Color(val+0.5f, val + 0.5f, val + 0.5f, 1f)); //pinta en escala de grises 

            }
            tex.Apply();
        }
    }
    // Update is called once per frame
    void Update()
    {
        WaveStep();
        ApplyMatrixToTexture(waveN, ref waveTexture);
    }
}

