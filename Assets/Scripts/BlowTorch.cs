using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlowTorch : MonoBehaviour
{
    public GameObject blowTorch;
    public ParticleSystem fireEffect;
    public Collider cremeBrulee;
       
    //When the mouse is held down, plays the fireeffect and rotates the blowtroch
    private void OnMouseDown() {
        fireEffect.Play();
        blowTorch.transform.Rotate(0, 0, -20f);
    }

    //When the mouse is released, the fire effects stop and the blowtorch is returned to its orginal position
    private void OnMouseUp() {
        fireEffect.Stop();
        blowTorch.transform.Rotate(0, 0, 20f);
    }
}
