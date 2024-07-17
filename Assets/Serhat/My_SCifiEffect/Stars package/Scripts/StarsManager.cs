using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class StarsManager : MonoBehaviour
{
    [Tooltip("Set to true if using with a skybox, so that the particles are as far away as possible")] 
    [SerializeField] bool m_usingWithSkybox = true;

    [Tooltip("Set to true if using with a day-night cycle")]
    [SerializeField] bool m_adjustStarsBrightnessByTimeOfDay = true;

    [Tooltip("-1 = midnight, +1 = midday, 0 = sunset/sunrise")]
    [SerializeField] AnimationCurve m_starsBrightness;

    [SerializeField] Light m_sun;
    [SerializeField] string m_starsColourProperty = "_BaseColor";

    // The renderers for the main stars particle system, plus the explosion sub emitter
    private ParticleSystemRenderer[] m_starsRenderers;
    private int m_starsColourId;

    private Camera m_mainCamera;


    private void Start()
    {
        m_mainCamera = Camera.main;
        transform.position = m_mainCamera.transform.position;
        transform.parent = m_mainCamera.transform;

        m_starsRenderers = GetComponentsInChildren<ParticleSystemRenderer>();
        m_starsColourId = Shader.PropertyToID(m_starsColourProperty);

        if (m_usingWithSkybox)
        {
            var scale = transform.localScale * m_mainCamera.farClipPlane * 0.09f;
            transform.localScale = scale;
        }
        else
        {
            for (int i = 0; i < m_starsRenderers.Length; i++)
                m_starsRenderers[i].material.renderQueue = (int) RenderQueue.Background;
        }
    }


    void Update()
    {
        if (m_sun != null)
            transform.rotation = m_sun.transform.rotation;
        else
            transform.rotation = Quaternion.identity;

        // Skip the next part if not using with a day-night cycle
        if (!m_adjustStarsBrightnessByTimeOfDay)
            return;

        // A proxy for how high the sun is in the sky
        // dot < 0 => sun is below the horizon
        // dot > 0 => sun is above the horizon
        float dot = Vector3.Dot(transform.forward, Vector3.down);

        // Adjust the brighness curve so that the stars become bright when the sun is below the horizon
        float intensity = m_starsBrightness.Evaluate(dot);

        for (int i = 0; i < m_starsRenderers.Length; i++)
        {
            var colour = m_starsRenderers[i].material.GetColor(m_starsColourId);
            colour.a = intensity;

            m_starsRenderers[i].material.SetColor(m_starsColourId, colour);
        }
    }
}