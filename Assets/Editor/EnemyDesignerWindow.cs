using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    [MenuItem("Window/Enemy Designer")]
    static void OpenWindow()
    {
        EnemyDesignerWindow window = (EnemyDesignerWindow) GetWindow(typeof(EnemyDesignerWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        InitTextures();
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



        GUILayout.EndArea();
    }

    void DrawMageSettings()
    {
        GUILayout.BeginArea(mageSection);



        GUILayout.EndArea();
    }

    void DrawWarriorSettings()
    {
        GUILayout.BeginArea(warriorSection);



        GUILayout.EndArea();
    }

    void DrawRogueSettings()
    {
        GUILayout.BeginArea(rogueSection);



        GUILayout.EndArea();
    }
}
