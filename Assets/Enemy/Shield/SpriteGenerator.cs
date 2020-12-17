using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteGenerator : MonoBehaviour
{
    SpriteRenderer sr;

    public Texture2D pattern;
    Texture2D dest;
    Color[] colArray;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        colArray = pattern.GetPixels();
        dest = new Texture2D(60*9, pattern.height*15);
        //dest.SetPixels();

        //set base hex
        for(int j=0; j<15; j++)
        {
            for(int i=0; i<9; i++)
            {
                SetBlock(60*i,34*j);
            }
        }

        //set interval hex
        for(int j=0; j<15; j++)
        {
            for(int i=0; i<9; i++)
            {
                SetBlock(30+60*i, 17+34*j);
            }
        }
        dest.Apply();
        dest.wrapMode = TextureWrapMode.Clamp;
        SavePNG();

    }

    void SetBlock(int x, int y)
    {
        float t = Random.Range(0f,1.0f);
        MySetPixels(x+10,y, pattern.width, pattern.height, colArray, t);
    }

    void MySetPixels(int x, int y, int blockwidth, int blockheight, Color[] colors, float t)
    {
        for(int i=x; i<x+blockwidth; i++)
        {
            for(int j=y; j<y+blockheight; j++)
            {
                if(colors[i-x+(j-y)*blockwidth].a>0)
                {
                    Color o = colors[i-x+(j-y)*blockwidth]; //origin color
                    Color newCol = new Color(o.r*t, o.g*t, o.b*t, o.a);
                    dest.SetPixel(i,j, newCol);
                }
            }
        }
    }

    void SavePNG()
    {
        byte[] bytes = dest.EncodeToPNG();
        FileStream file= File.Open(Application.dataPath+ "/base.png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        Sprite newSprite = Sprite.Create(dest, 
                                        new Rect(0,0,dest.width,dest.height),
                                        Vector2.one*0.5f);
        sr.sprite = newSprite;
    }
}
