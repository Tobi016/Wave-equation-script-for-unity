using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavecontrol : MonoBehaviour
{
    public Material waveMaterial;
    public Texture2D waveTexture;
    float [][] waveN,waveNm1,WaveNp1; //informacion de estado
    float Lx =  10;//ancho
    float Ly =  10;//alto
    [SerializeField] float dx = 0.1f; //densidad del eje x
    float dy {  get => dx;} // densidad del eje y
    int nx,ny; // resolucion
    //variables para ecuacion de onda
    public float CFL =0.05f;
    public float c = 1;
    float dt; //cambio en el tiempo
    float t; //tiempo actual
    // Start is called before the first frame update
    void Start(){
    
        nx = Mathf.FloorToInt(Lx / dx);
        ny = Mathf.FloorToInt(Ly / dy);
        waveTexture = new Texture2D(nx,ny, TextureFormat.RGB32,false);
        waveN   = new float[nx][];
        waveNm1 = new float[nx][];
        waveNp1 = new float[nx][];
        //inicializacion de las variables de informacion de estado
        for(int i =0;i<nx;i++){
            waveN[i]    = new float [ny];
            waveNm1[i]  = new float [ny];
            waveNp1[i]  = new float [ny];
        }
        waveMaterial.SetTexture("_MainTex", waveTexture);//coloring texture
        waveMaterial.SetTexture("_Displacement",waveTexture);//displacement texture




    }
    void WaveStep{
        dt =CFL *dx/c; //recalculamos dt
        t +=dx; //incrementamos el tiempo
        //copia el contenido del estado de onda anterior al estado de onda actual, i es el estado anterior y j es el actual
        for(int i =0;i<nx;i++){
            for(int j=0;j<ny;j++){
                waveNm1[i][j] = waveN[i][j]; // copia el estado en N a N-1
                waveN[i][j]   = waveNp1[i][j];// copia el es estado N+1 A N 
            }

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
