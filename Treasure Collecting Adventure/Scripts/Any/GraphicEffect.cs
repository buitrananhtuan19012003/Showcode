using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicEffect : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private List<MeshRenderer> listMeshRenderer;
    [SerializeField] private Color blinkColor = Color.white;
    [SerializeField] private float blinkIntensity = 1;
    [SerializeField] private float blinkDuration = 0.5f;

    private float blinkTimer;
    private bool isHit;

    public SkinnedMeshRenderer SkinnedMeshRenderer { get => this.skinnedMeshRenderer; set => this.skinnedMeshRenderer = value; }
    public List<MeshRenderer> ListMeshRenderer { get => listMeshRenderer; set => this.listMeshRenderer = value; }

    private void Update()
    {
        this.HitEffect();
    }

    public void PlayHitEffect()
    {
        this.blinkTimer = this.blinkDuration;
        this.isHit = true;
    }

    private void HitEffect()
    {
        if (this.isHit)
        {
            this.blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(this.blinkTimer / this.blinkDuration);
            float intensity = (lerp * this.blinkIntensity) + 1f;
            if (this.skinnedMeshRenderer != null)
            {
                this.skinnedMeshRenderer.material.color = this.blinkColor * intensity;
            }

            for (int i = 0; i < this.listMeshRenderer.Count; i++)
            {
                this.listMeshRenderer[i].material.color = this.blinkColor * intensity;
            }

            if (this.blinkTimer == 0)
                this.isHit = false;
        }
    }

}
