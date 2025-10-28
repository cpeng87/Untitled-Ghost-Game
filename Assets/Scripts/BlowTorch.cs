using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlowTorch : MonoBehaviour
{
    public GameObject blowTorch;
    public ParticleSystem fireEffect;
    public Collider cremeBrulee;
    public RingScript ringScript;

    public CremeBrulee slider;

    public TMP_Text text;

    [SerializeField] private Color green;
    [SerializeField] private Color yellow;
    [SerializeField] private Color red;

    private void Start()
    {
        fireEffect.Stop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SprayFire());
        }
    }
    

    private IEnumerator SprayFire()
    {
        if (!fireEffect.isPlaying)
        {
            fireEffect.Play();
            blowTorch.transform.Rotate(0, 0, -20f);

            int currScore = ringScript.GetScore();
            float scoreIncrement = RingScript.ScoreToSliderIncrement(currScore);

            text.color = (scoreIncrement == 10) ? green : (scoreIncrement > 0) ? yellow : red;
            string toPlay = (scoreIncrement == 10) ? "Great" : (scoreIncrement > 0) ? "Okay" : "Bad";
            AudioManager.Instance.PlaySound(toPlay);

            text.text = RingScript.ScoreToString(currScore);
            slider.IncreaseSlider(scoreIncrement);
            yield return new WaitForSeconds(0.3f);



            fireEffect.Stop();
            blowTorch.transform.Rotate(0, 0, 20f);
            text.text = "";
            AudioManager.Instance.StopSound();
        }
    }
}
