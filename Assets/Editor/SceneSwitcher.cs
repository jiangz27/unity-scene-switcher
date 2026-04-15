using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using ToolBar;

public class SceneSwitcher
{
    const string CONFIG_PATH = "Assets/Configs/ScenesConfig.asset";
    static string[] _sceneNames = new string[] { "empty" };
    static SceneAsset[] _sceneAssets;
    static SceneAsset _startScene;

    static int _sceneIndex
    {
        get => EditorPrefs.GetInt("SceneIndex");
        set => EditorPrefs.SetInt("SceneIndex", value);
    }

    static bool _sceneLock
    {
        get => EditorPrefs.GetBool("SceneLock");
        set => EditorPrefs.SetBool("SceneLock", value);
    }

    [InitializeOnLoadMethod]
    public static void Init()
    {
        ScenesConfig config = null;

        if (File.Exists(CONFIG_PATH))
        {
            config = AssetDatabase.LoadAssetAtPath<ScenesConfig>(CONFIG_PATH);
            if (config == null) return;

            _startScene = config.startScene;

            _sceneAssets = config.scenes;
            if (_sceneAssets.Length == 0) return;

            _sceneNames = new string[_sceneAssets.Length];

            for (int i = 0; i < _sceneAssets.Length; i++)
            {
                _sceneNames[i] = _sceneAssets[i].name;
            }
        }

        EditorApplication.playModeStateChanged += (obj) =>
        {
            if (obj == PlayModeStateChange.ExitingPlayMode)
            {
                if (_sceneLock)
                {
                    var currentScene = EditorSceneManager.GetActiveScene().name;
                    for (int i = 0; i < _sceneNames.Length; i++)
                    {
                        if (_sceneNames[i] == currentScene)
                        {
                            _sceneIndex = i;
                        }
                    }
                }
            }
            else if (obj == PlayModeStateChange.EnteredEditMode)
            {
                var target = _sceneAssets[_sceneIndex];
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(target));
            }
        };
    }

    public static void ScenesDropdown()
    {
        GUILayout.FlexibleSpace();
        var newIndex = EditorGUILayout.Popup(_sceneIndex, _sceneNames, ToolbarStyles.dropdownButtonStyle, GUILayout.Width(150f));
        if (newIndex != _sceneIndex)
        {
            _sceneIndex = newIndex;
            var target = _sceneAssets[_sceneIndex];
            if (EditorSceneManager.GetActiveScene().name != target.name)
            {
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(target));
            }
        }
    }

    public static void StartSceneBtn()
    {
        if (GUILayout.Button(new GUIContent("Start"), ToolbarStyles.commandButtonStyle))
        {
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(_startScene));
            EditorApplication.isPlaying = true;
        }
    }

    public static void LockSceneToggle()
    {
        GUI.changed = false;
        GUILayout.Toggle(_sceneLock, new GUIContent("Lock"), ToolbarStyles.commandButtonStyle);
        if (GUI.changed)
        {
            _sceneLock = !_sceneLock;
        }
    }
}