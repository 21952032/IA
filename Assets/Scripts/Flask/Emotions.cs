using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Emotions : MonoBehaviour
{
    public TMP_Text debugText;
    private MeshRenderer elPibe;
    public Material[] materials;
    public Material defaultMaterial;
    private void Start()
    {
        elPibe = GetComponent<MeshRenderer>();
        elPibe.material = defaultMaterial;
    }
    public void NewEmotion(int code)
    {
        if (code >= 0 && code < 7)
        {
            elPibe.material = materials[code];
        }
        else
        {
            elPibe.material = defaultMaterial;
        }
        switch (code)
        {
            case 0:
                debugText.text = "El pibe está triste";
                break;
            case 1:
                debugText.text = "El pibe está alegre";
                break;
            case 2:
                debugText.text = "El pibe se ha enfadado";
                break;
            case 3:
                debugText.text = "Ni fu ni fa...";
                break;
            case 4:
                debugText.text = "El pibe se ha sorprendido";
                break;
            case 5:
                debugText.text = "El pibe tiene miedo";
                break;
            case 6:
                debugText.text = "Puaj...";
                break;
            default:
                debugText.text = "Parece que ha habido un problema detectando la emoción";
                break;
        }
    }
}
