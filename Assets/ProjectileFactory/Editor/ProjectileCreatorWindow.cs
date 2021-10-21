using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ProjectileCreatorWindow : EditorWindow
{
    Texture2D headerSectionTexture;
    Texture2D projectileSectionTexture;

    Color headerSectionColor = new Color(13f / 255f, 25f / 255f, 35f / 255f, 1f);
    Color projectileSectionColor = new Color(34f / 255f, 62f / 255f, 86f / 255f, 1f);

    Rect headerSection;
    Rect projectileSection;

    GUISkin skin;

    static ProjectileData projectileData;

    public static ProjectileData ProjectileInfo { get { return projectileData; } }

    [MenuItem("Window/Projectile Factory")]
    static void OpenWindow()
    {
        ProjectileCreatorWindow window = (ProjectileCreatorWindow) GetWindow(typeof(ProjectileCreatorWindow));
        window.minSize = new Vector2(350, 300);
        window.titleContent.text = "Projectile Factory";
        window.Show();
    }

    static void CloseWindow()
    {
        ProjectileCreatorWindow window = (ProjectileCreatorWindow) GetWindow(typeof(ProjectileCreatorWindow));
        window.Close();
    }

    void OnEnable()
    {
        InitTextures();
        InitData();
        skin = Resources.Load<GUISkin>("GUISkin/ProjectileFactorySkin");
    }

    public static void InitData()
    {
        projectileData = (ProjectileData) ScriptableObject.CreateInstance(typeof(ProjectileData));
    }

    void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();

        projectileSectionTexture = new Texture2D(1, 1);
        projectileSectionTexture.SetPixel(0, 0, projectileSectionColor);
        projectileSectionTexture.Apply();
    }

    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawProjectileSettings();
    }

    void DrawLayouts()
    {
        // Always draw header in the top left of the window
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;

        projectileSection.x = 0;
        projectileSection.y = 50;
        projectileSection.width = Screen.width;
        projectileSection.height = Screen.height - 50;

        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(projectileSection, projectileSectionTexture);
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);

        GUILayout.Label("Projectile Factory", skin.GetStyle("Header1"));

        GUILayout.EndArea();
    }

    void DrawProjectileSettings()
    {
        GUILayout.BeginArea(projectileSection);

        GUILayout.Label("Projectile", skin.GetStyle("Header2"));

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name", skin.GetStyle("Body"));
        GUILayout.FlexibleSpace();
        projectileData.name = EditorGUILayout.TextField(projectileData.name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Projectile Model", skin.GetStyle("Body"));
        GUILayout.FlexibleSpace();
        projectileData.model = (ProjectileData.projectileModel) EditorGUILayout.EnumPopup(projectileData.model);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Add Particles?", skin.GetStyle("Body"));
        GUILayout.FlexibleSpace();
        projectileData.containsParticles = EditorGUILayout.Toggle(" ", projectileData.containsParticles);
        EditorGUILayout.EndHorizontal();

        if (projectileData.containsParticles)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Particle Material", skin.GetStyle("Body"));
            GUILayout.FlexibleSpace();
            projectileData.particleMaterial = (Material)EditorGUILayout.ObjectField(projectileData.particleMaterial, typeof(Material), false);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Add Trail?", skin.GetStyle("Body"));
        GUILayout.FlexibleSpace();
        projectileData.containsTrail = EditorGUILayout.Toggle(" ", projectileData.containsTrail);
        EditorGUILayout.EndHorizontal();

        if (projectileData.containsTrail)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Trail Material", skin.GetStyle("Body"));
            GUILayout.FlexibleSpace();
            projectileData.trailMaterial = (Material)EditorGUILayout.ObjectField(projectileData.trailMaterial, typeof(Material), false);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Add Impact Particles?", skin.GetStyle("Body"));
        GUILayout.FlexibleSpace();
        projectileData.containsImpactParticles = EditorGUILayout.Toggle(" ", projectileData.containsImpactParticles);
        EditorGUILayout.EndHorizontal();

        if (projectileData.containsImpactParticles)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Impact Particle Material", skin.GetStyle("Body"));
            GUILayout.FlexibleSpace();
            projectileData.impactParticleMaterial = (Material)EditorGUILayout.ObjectField(projectileData.impactParticleMaterial, typeof(Material), false);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Projectile Speed", skin.GetStyle("Body"));
        GUILayout.FlexibleSpace();
        projectileData.speed = EditorGUILayout.FloatField(projectileData.speed);
        EditorGUILayout.EndHorizontal();

        if (projectileData.name == null || projectileData.name.Length < 1)
        {
            EditorGUILayout.HelpBox("This projectile needs a [Name] before it can be created.", MessageType.Warning);
        }
        else if (projectileData.containsParticles && projectileData.particleMaterial == null)
        {
            EditorGUILayout.HelpBox("This projectile needs a [Particle Material] before it can be created.", MessageType.Warning);
        }
        else if (projectileData.containsImpactParticles && projectileData.impactParticleMaterial == null)
        {
            EditorGUILayout.HelpBox("This projectile needs a [Impact Particle Material] before it can be created.", MessageType.Warning);
        }
        // defines a button and what code will execute if clicked
        else if (GUILayout.Button("Create!", skin.GetStyle("Button"), GUILayout.Height(40)))
        {
            SaveProjectile();
            CloseWindow();
        }

        GUILayout.EndArea();
    }

    void SaveProjectile()
    {
        string prefabPath = "Assets/ProjectileFactory/Prefabs/Projectiles/";
        string dataPath = "Assets/ProjectileFactory/Resources/ProjectileData/Data/";

        // create folders if necessary
        if (!Directory.Exists("Assets/ProjectileFactory/Prefabs/"))
        {
            AssetDatabase.CreateFolder("Assets/ProjectileFactory", "Prefabs");
        }

        if (!Directory.Exists(prefabPath))
        {
            AssetDatabase.CreateFolder("Assets/ProjectileFactory/Prefabs", "Projectiles");
        }

        if(!Directory.Exists(dataPath))
        {
            AssetDatabase.CreateFolder("Assets/ProjectileFactory/Resources/ProjectileData", "Data");
        }

        // create the .asset file
        dataPath += ProjectileInfo.name + ".asset";
        AssetDatabase.CreateAsset(ProjectileInfo, dataPath); // creates a new projectile scriptable object asset at the specified path

        prefabPath += ProjectileInfo.name + ".prefab";
        AssetDatabase.GenerateUniqueAssetPath(prefabPath);
        GameObject projectile = ProjectileBuilder();
        PrefabUtility.SaveAsPrefabAssetAndConnect(projectile, prefabPath, InteractionMode.UserAction);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        DestroyImmediate(projectile);
    }

    GameObject ProjectileBuilder()
    {
        // Create projectile object with required components
        GameObject projectile = new GameObject();
        GameObject visuals = null;

        projectile.AddComponent<Rigidbody>();
        projectile.AddComponent<ProjectileController>();

        // Name
        projectile.name = ProjectileInfo.name;

        // Model
        switch (ProjectileInfo.model)
        {
            case ProjectileData.projectileModel.Sphere:
                visuals = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                DestroyImmediate(visuals.GetComponent<SphereCollider>());
                projectile.AddComponent<SphereCollider>();
                break;
            case ProjectileData.projectileModel.Capsule:
                visuals = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                visuals.transform.rotation = Quaternion.Euler(90, 0, 0); // Align with Z-Axis (looks more like a bullet with a rotated mesh)
                DestroyImmediate(visuals.GetComponent<CapsuleCollider>());
                projectile.AddComponent<CapsuleCollider>();
                projectile.GetComponent<CapsuleCollider>().direction = 2; // Align with Z-Axis (align collider with mesh rotation)
                break;
            case ProjectileData.projectileModel.Cube:
                visuals = GameObject.CreatePrimitive(PrimitiveType.Cube);
                DestroyImmediate(visuals.GetComponent<BoxCollider>());
                projectile.AddComponent<SphereCollider>();
                break;
            case ProjectileData.projectileModel.None:
                visuals = new GameObject();
                visuals.AddComponent<MeshFilter>();
                visuals.AddComponent<MeshRenderer>();
                projectile.AddComponent<SphereCollider>();
                break;
        }

        visuals.name = "Visuals";
        visuals.transform.parent = projectile.transform;

        // Particles
        if (ProjectileInfo.containsParticles)
        {
            visuals.AddComponent<ParticleSystem>();

            ParticleSystem.ShapeModule sm = visuals.GetComponent<ParticleSystem>().shape;
            sm.rotation = new Vector3(180, 0, 0);

            ParticleSystem.MainModule mm = visuals.GetComponent<ParticleSystem>().main;
            mm.simulationSpace = ParticleSystemSimulationSpace.World;

            visuals.GetComponent<ParticleSystemRenderer>().material = projectileData.particleMaterial;
            projectile.GetComponent<ProjectileController>()._projectileParticles = visuals.GetComponent<ParticleSystem>();
        }

        // Trail
        if (ProjectileInfo.containsTrail)
        {
            visuals.AddComponent<TrailRenderer>();

            TrailRenderer tr = visuals.GetComponent<TrailRenderer>();
            tr.material = projectileData.trailMaterial;
            tr.startWidth = 0.5f;
        }

        // Impact Particles
        if (ProjectileInfo.containsImpactParticles)
        {
            GameObject impactParticles = new GameObject();
            impactParticles.AddComponent<ParticleSystem>();
            impactParticles.name = "Impact Particles";
            impactParticles.transform.parent = visuals.transform;

            ParticleSystem.MainModule mm = impactParticles.GetComponent<ParticleSystem>().main;
            mm.playOnAwake = false;
            mm.duration = 1f;
            mm.loop = false;
            mm.startLifetime = 0.5f;

            impactParticles.GetComponent<ParticleSystemRenderer>().material = projectileData.impactParticleMaterial;
            projectile.GetComponent<ProjectileController>()._impactParticles = impactParticles.GetComponent<ParticleSystem>();
        }

        // Speed
        // Set speed value in controller script component
        projectile.GetComponent<ProjectileController>().Speed = ProjectileInfo.speed;

        return projectile;
    }
}