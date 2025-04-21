using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WormholeFXBuilder : MonoBehaviour
{
    [MenuItem("Tools/Create Wormhole Particle Prefab")]
    static void CreateWormholePrefab()
    {
        GameObject root = new GameObject("WormholeFX");

        // --- Star Streaks ---
        GameObject starGO = new GameObject("StarStreaks");
        starGO.transform.parent = root.transform;
        var starPS = starGO.AddComponent<ParticleSystem>();
        var main = starPS.main;
        main.duration = 5;
        main.loop = true;
        main.startLifetime = 1f;
        main.startSpeed = 40f;
        main.startSize = 0.1f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 1000;

        var emission = starPS.emission;
        emission.rateOverTime = 200;

        var shape = starPS.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 5;
        shape.radius = 0.5f;

        var renderer = starGO.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Stretch;
        renderer.velocityScale = 0;
        renderer.lengthScale = 4;
        renderer.material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Particle.mat");

        // --- Tunnel Spiral ---
        GameObject spiralGO = new GameObject("TunnelSpiral");
        spiralGO.transform.parent = root.transform;
        var spiralPS = spiralGO.AddComponent<ParticleSystem>();
        var spiralMain = spiralPS.main;
        spiralMain.duration = 5;
        spiralMain.loop = true;
        spiralMain.startLifetime = 2f;
        spiralMain.startSpeed = 0f;
        spiralMain.startSize = 1f;
        spiralMain.simulationSpace = ParticleSystemSimulationSpace.Local;
        spiralMain.maxParticles = 300;

        var spiralEmission = spiralPS.emission;
        spiralEmission.rateOverTime = 30;

        var spiralShape = spiralPS.shape;
        spiralShape.shapeType = ParticleSystemShapeType.Donut;
        spiralShape.radius = 2f;
        spiralShape.donutRadius = 0.5f;

        var rotOverLifetime = spiralPS.rotationOverLifetime;
        rotOverLifetime.enabled = true;
        rotOverLifetime.z = new ParticleSystem.MinMaxCurve(360f);

        var sizeOverLifetime = spiralPS.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 1f);
        curve.AddKey(1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);

        var spiralRenderer = spiralGO.GetComponent<ParticleSystemRenderer>();
        spiralRenderer.material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Particle.mat");

        // Save as Prefab
        string path = "Assets/WormholeFX.prefab";
        PrefabUtility.SaveAsPrefabAsset(root, path);
        Debug.Log("Wormhole prefab saved at " + path);
        DestroyImmediate(root);
    }
}