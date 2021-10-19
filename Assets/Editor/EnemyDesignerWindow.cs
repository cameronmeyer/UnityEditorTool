using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class EnemyDesignerWindow : EditorWindow
{
    Texture2D headerSectionTexture;
    Texture2D mageSectionTexture;
    Texture2D warriorSectionTexture;
    Texture2D rogueSectionTexture;

    Color headerSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);
    Color mageSectionColor = new Color(0f, 174f / 255f, 211f / 255f, 1f);
    Color warriorSectionColor = new Color(242f / 255f, 137f / 255f, 0f, 1f);
    Color rogueSectionColor = new Color(0f, 153f / 255f, 18f / 255f, 1f);

    Rect headerSection;
    Rect mageSection;
    Rect warriorSection;
    Rect rogueSection;

    static MageData mageData;
    static WarriorData warriorData;
    static RogueData rogueData;

    public static MageData MageInfo { get { return mageData; } }
    public static WarriorData WarriorInfo { get { return warriorData; } }
    public static RogueData RogueInfo { get { return rogueData; } }

    [MenuItem("Window/Enemy Designer")]
    static void OpenWindow()
    {
        EnemyDesignerWindow window = (EnemyDesignerWindow) GetWindow(typeof(EnemyDesignerWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    void OnEnable()
    {
        InitTextures();
        InitData();
    }

    public static void InitData()
    {
        mageData = (MageData) ScriptableObject.CreateInstance(typeof(MageData));
        warriorData = (WarriorData) ScriptableObject.CreateInstance(typeof(WarriorData));
        rogueData = (RogueData) ScriptableObject.CreateInstance(typeof(RogueData));
    }

    void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();

        mageSectionTexture = new Texture2D(1, 1);
        mageSectionTexture.SetPixel(0, 0, mageSectionColor);
        mageSectionTexture.Apply();

        warriorSectionTexture = new Texture2D(1, 1);
        warriorSectionTexture.SetPixel(0, 0, warriorSectionColor);
        warriorSectionTexture.Apply();

        rogueSectionTexture = new Texture2D(1, 1);
        rogueSectionTexture.SetPixel(0, 0, rogueSectionColor);
        rogueSectionTexture.Apply();
    }

    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawMageSettings();
        DrawWarriorSettings();
        DrawRogueSettings();
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
        mageSection.width = Screen.width / 3f;
        mageSection.height = Screen.height - 50;

        warriorSection.x = Screen.width / 3f;
        warriorSection.y = 50;
        warriorSection.width = Screen.width / 3f;
        warriorSection.height = Screen.height - 50;

        rogueSection.x = 2 * Screen.width / 3f;
        rogueSection.y = 50;
        rogueSection.width = Screen.width / 3f;
        rogueSection.height = Screen.height - 50;

        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(mageSection, mageSectionTexture);
        GUI.DrawTexture(warriorSection, warriorSectionTexture);
        GUI.DrawTexture(rogueSection, rogueSectionTexture);
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);

        GUILayout.Label("Enemy Designer");

        GUILayout.EndArea();
    }

    void DrawMageSettings()
    {
        GUILayout.BeginArea(mageSection);

        GUILayout.Label("Mage");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Damage");
        mageData.dmgType = (MageDmgType) EditorGUILayout.EnumPopup(mageData.dmgType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Weapon");
        mageData.wpnType = (MageWpnType)EditorGUILayout.EnumPopup(mageData.wpnType);
        EditorGUILayout.EndHorizontal();

        // defines a button and what code will execute if clicked
        if(GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.MAGE);
        }

        GUILayout.EndArea();
    }

    void DrawWarriorSettings()
    {
        GUILayout.BeginArea(warriorSection);

        GUILayout.Label("Warrior");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Class");
        warriorData.classType = (WarriorClassType)EditorGUILayout.EnumPopup(warriorData.classType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Weapon");
        warriorData.wpnType = (WarriorWpnType)EditorGUILayout.EnumPopup(warriorData.wpnType);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.WARRIOR);
        }

        GUILayout.EndArea();
    }

    void DrawRogueSettings()
    {
        GUILayout.BeginArea(rogueSection);

        GUILayout.Label("Rogue");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Strategy");
        rogueData.strategyType = (RogueStrategyType)EditorGUILayout.EnumPopup(rogueData.strategyType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Weapon");
        rogueData.wpnType = (RogueWpnType)EditorGUILayout.EnumPopup(rogueData.wpnType);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.ROGUE);
        }

        GUILayout.EndArea();
    }
}

public class GeneralSettings : EditorWindow
{
    public enum SettingsType
    {
        MAGE,
        WARRIOR,
        ROGUE
    }
    static SettingsType dataSetting;
    static GeneralSettings window;

    public static void OpenWindow(SettingsType setting)
    {
        dataSetting = setting;
        window = (GeneralSettings)GetWindow(typeof(GeneralSettings));
        window.minSize = new Vector2(250, 200);
        window.Show();
    }

    private void OnGUI()
    {
        switch (dataSetting)
        {
            case SettingsType.MAGE:
                DrawSettings((CharacterData) EnemyDesignerWindow.MageInfo);
                break;
            case SettingsType.WARRIOR:
                DrawSettings((CharacterData) EnemyDesignerWindow.WarriorInfo);
                break;
            case SettingsType.ROGUE:
                DrawSettings((CharacterData) EnemyDesignerWindow.RogueInfo);
                break;
        }
    }

    void DrawSettings(CharacterData charData)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Prefab");
        charData.prefab = (GameObject) EditorGUILayout.ObjectField(charData.prefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Max Health");
        charData.maxHealth = EditorGUILayout.FloatField(charData.maxHealth);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Max Energy");
        charData.maxEnergy = EditorGUILayout.FloatField(charData.maxEnergy);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Power");
        charData.power = EditorGUILayout.Slider(charData.power, 0, 100);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("% Crit Chance");
        charData.critChance = EditorGUILayout.Slider(charData.critChance, 0, charData.power);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        charData.name = EditorGUILayout.TextField(charData.name);
        EditorGUILayout.EndHorizontal();

        if(charData.prefab == null)
        {
            EditorGUILayout.HelpBox("This enemy needs a [Prefab] before it can be created.", MessageType.Warning);
        }
        else if (charData.name == null || charData.name.Length < 1)
        {
            EditorGUILayout.HelpBox("This enemy needs a [Name] before it can be created.", MessageType.Warning);
        }
        else if (GUILayout.Button("Finish and Save", GUILayout.Height(30)));
        {
            SaveCharacterData();
            window.Close();
        }
    }

    void SaveCharacterData()
    {
        string prefabPath; // path to the base prefab
        string newPrefabPath = "Assets/Prefabs/Characters/";
        string dataPath = "Assets/Resources/CharacterData/Data/";

        switch(dataSetting)
        {
            case SettingsType.MAGE:
                // create the .asset file
                //MageData mageInfo = Instantiate(EnemyDesignerWindow.MageInfo);
                dataPath += "Mage/" + EnemyDesignerWindow.MageInfo.name + ".asset";
                AssetDatabase.CreateAsset(EnemyDesignerWindow.MageInfo, dataPath); // creates a new asset from the mageinfo scriptable object at the specified path

                newPrefabPath += "Mage/" + EnemyDesignerWindow.MageInfo.name + ".prefab";

                // get prefab path
                prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.MageInfo.prefab);
                AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GameObject magePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));
                if (!magePrefab.GetComponent<Mage>())
                {
                    magePrefab.AddComponent(typeof(Mage));
                }

                magePrefab.GetComponent<Mage>().mageData = EnemyDesignerWindow.MageInfo;
                break;

            case SettingsType.WARRIOR:

                break;

            case SettingsType.ROGUE:

                break;
        }
    }
}
