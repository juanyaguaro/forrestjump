using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    private string imageType;
    private int imageIndex;
    private bool change = false;
    private RawImage backgroundImage;

    //Update: se llama una vez por cada frame. Verifica si se debe cambiar la textura de la imagen en caso de que
    // change haya sido puesto en verdadero.
    private void Update() {
        if (change) {
            change = false;
            LoadImageTexture();
        }
    }

    //SetInfo: permite cambiar la textura modificando change a verdadero y asigna el tipo de imagen y el indice a los
    // atributos correspondientes. Esta informacion es vital para poder cargar la textura.
    public void SetInfo(string type, int index) {
        change = true;
        imageType = type;
        imageIndex = index;
    }

    //LoadImageTexture: utiliza los atributos de type e indice para cargar la imagen de Resources con dicho nombre
    // como textura para la imagen de fondo.
    void LoadImageTexture() {
        string imagePath = "noseloquetuquieras/" + imageType + imageIndex.ToString();

        backgroundImage = GetComponent<RawImage>();
        backgroundImage.texture = Resources.Load<Texture2D>(imagePath);
    }
}
