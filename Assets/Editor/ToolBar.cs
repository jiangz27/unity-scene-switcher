using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace ToolBar
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;
		public static readonly GUIStyle dropdownButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 16,
				fixedWidth = 50,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};

			dropdownButtonStyle = new GUIStyle("Dropdown")
			{
				fontSize = 12,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};
		}
	}

	[InitializeOnLoad]
	public class ToolBarLeftButton
	{
		static ToolBarLeftButton()
		{
			ToolbarExtender.LeftToolbarGUI.Add(SceneSwitcher.ScenesDropdown);
			ToolbarExtender.LeftToolbarGUI.Add(SceneSwitcher.LockSceneToggle);
			ToolbarExtender.LeftToolbarGUI.Add(SceneSwitcher.StartSceneBtn);
		}
	}
}