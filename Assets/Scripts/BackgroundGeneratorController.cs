using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGeneratorController : MonoBehaviour
{
    private int backgroundLayerQuantity = 9;
    private int platformLayerQuantity = 2;
    public GameObject backgroundPrefab;

    //StartBG: crea las instancias de las capas del background y del platform.
    // CAMBIAR, LIMITES NO DEBEN SER HARDCODE.
    private void StartBG() {
        for (int i = 0; i < backgroundLayerQuantity; i++)
            CreateLayer("bg", i);
        for (int i = 0; i < platformLayerQuantity; i++)
            CreateLayer("pf", i, false);
    }

    //CreateLayer: obtiene el nombre y el indice de la imagen, crea una instancia del prefab de background y
    // asigna a dicha instancia los parametros recibidos y su tag correspondiente.
    //CAMBIAR, SOLUCION NO ELEGANTE RESPECTO AL BOOLEANO.
    private void CreateLayer(string type, int index, bool background = true) {
        var newLayer = Instantiate(backgroundPrefab, transform.position, Quaternion.identity);

        newLayer.GetComponent<BackgroundController>().SetInfo(type, index);
        newLayer.transform.SetParent(GameObject.Find("Background").transform, false);
        newLayer.transform.gameObject.tag = (background)? "Background" : "Platform";
    }
}
