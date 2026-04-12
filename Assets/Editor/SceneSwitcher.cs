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

    static int _index
    {
        get => EditorPrefs.GetInt("SceneIndex");
        set => EditorPrefs.SetInt("SceneIndex", value);
    }

    static bool _sceneFocus
    {
        get => EditorPrefs.GetBool("SceneFocus");
        set => EditorPrefs.SetBool("SceneFocus", value);
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
                if (_sceneFocus)
                {
                    var currentScene = EditorSceneManager.GetActiveScene().name;
                    for (int i = 0; i < _sceneNames.Length; i++)
                    {
                        if (_sceneNames[i] == currentScene)
                        {
                            _index = i;
                        }
                    }
                }
            }
            else if (obj == PlayModeStateChange.EnteredEditMode)
            {
                var target = _sceneAssets[_index];
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(target));
            }
        };
    }

    public static void ScenesDropdown()
    {
        GUILayout.FlexibleSpace();
        var newIndex = EditorGUILayout.Popup(_index, _sceneNames, ToolbarStyles.dropdownButtonStyle, GUILayout.Width(150f));
        if (newIndex != _index)
        {
            _index = newIndex;
            var target = _sceneAssets[_index];
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

    public static void FocusSceneBtn()
    {
        var tex = EditorGUIUtility.IconContent(@"UnityEditor.SceneView").image;
        GUI.changed = false;
        GUILayout.Toggle(_sceneFocus, new GUIContent(null, tex), "Command");
        if (GUI.changed)
        {
            _sceneFocus = !_sceneFocus;
        }
    }
}