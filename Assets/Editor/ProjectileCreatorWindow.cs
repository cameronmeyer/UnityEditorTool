using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProjectileCreatorWindow : EditorWindow
{
    Texture2D headerSectionTexture;
    Texture2D mageSectionTexture;

    Color headerSectionColor = new Color(13f / 255f, 25f / 255f, 35f / 255f, 1f);
    Color projectileSectionColor = new Color(34f / 255f, 62f / 255f, 86f / 255f, 1f);

    Rect headerSection;
    Rect mageSection;

    static ProjectileData projectileData;

    public static ProjectileData ProjectileInfo { get { return projectileData; } }

    [MenuItem("Window/Projectile Creator")]
    static void OpenWindow()
    {
        ProjectileCreatorWindow window = (ProjectileCreatorWindow) GetWindow(typeof(ProjectileCreatorWindow));
        window.minSize = new Vector2(300, 300);
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

        mageSectionTexture = new Texture2D(1, 1);
        mageSectionTexture.SetPixel(0, 0, projectileSectionColor);
        mageSectionTexture.Apply();
    }

    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawProjectileSettings();
    }

    // Defines Rect values and paints textures based on Rects
    void DrawLayouts()
    {
        // Always draw header in the top left of the window
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;

        mageSection.x = 0;
        mageSection.y = 50;
        mageSection.width = Screen.width;
        mageSection.height = Screen.height - 50;

        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(mageSection, mageSectionTexture);
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);

        GUILayout.Label("Projectile Creator");

        GUILayout.EndArea();
    }

    void DrawProjectileSettings()
    {
        GUILayout.BeginArea(mageSection);

        GUILayout.Label("Projectile");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        GUILayout.FlexibleSpace();
        projectileData.name = EditorGUILayout.TextField(projectileData.name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Projectile Model");
        GUILayout.FlexibleSpace();
        projectileData.model = (ProjectileData.projectileModel) EditorGUILayout.EnumPopup(projectileData.model);
        EditorGUILayout.EndHorizontal();

        /*EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Prefab");
        projectileData.prefab = (GameObject)EditorGUILayout.ObjectField(projectileData.prefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();*/

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Add Particles?");
        GUILayout.FlexibleSpace();
        projectileData.containsParticles = EditorGUILayout.Toggle(" ", projectileData.containsParticles);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Add Trail?");
        GUILayout.FlexibleSpace();
        projectileData.containsTrail = EditorGUILayout.Toggle(" ", projectileData.containsTrail);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Add Impact Particles?");
        GUILayout.FlexibleSpace();
        projectileData.containsImpactParticles = EditorGUILayout.Toggle(" ", projectileData.containsImpactParticles);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Projectile Speed");
        GUILayout.FlexibleSpace();
        projectileData.speed = EditorGUILayout.FloatField(projectileData.speed);
        EditorGUILayout.EndHorizontal();

        if (projectileData.name == null || projectileData.name.Length < 1)
        {
            EditorGUILayout.HelpBox("This projectile needs a [Name] before it can be created.", MessageType.Warning);
        }
        // defines a button and what code will execute if clicked
        else if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            SaveProjectile();
            CloseWindow();
        }

        GUILayout.EndArea();
    }

    void SaveProjectile()
    {
        //string prefabPath; // path to the base prefab
        string prefabPath = "Assets/Prefabs/Projectiles/";
        string dataPath = "Assets/Resources/ProjectileData/Data/";

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
        projectile.AddComponent<Rigidbody>();
        projectile.AddComponent<ProjectileController>();

        // Name
        projectile.name = ProjectileInfo.name;

        // Model
        switch(ProjectileInfo.model)
        {
            case ProjectileData.projectileModel.Sphere:
                break;
            case ProjectileData.projectileModel.Cube:
                break;
            case ProjectileData.projectileModel.None:
                break;
        }

        // Particles

        // Trail

        // Impact Particles

        // Speed
        // Set speed value in controller script component
        projectile.GetComponent<ProjectileController>().Speed = ProjectileInfo.speed;
        Debug.Log("ProjectileInfo Speed: " + ProjectileInfo.speed + "  Projectile Controller Speed: " + projectile.GetComponent<ProjectileController>().Speed);

        // Damage?
        // Set damage value in controller script component

        return projectile;
    }
}