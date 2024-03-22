using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

using UnityEngine.UIElements;
using System.Linq;

namespace WFC_Luyso{
    public class RuleTileManager : EditorWindow
    {   
        [SerializeField] private int m_SelectedIndex = -1;
        private VisualElement m_RightPane;

        RuleTile ruleTile;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Tools/Rule Tile Managers")]
        public static void ShowWindow()
        {
            EditorWindow wnd = GetWindow<RuleTileManager>();
            wnd.titleContent = new GUIContent("Rule Tile Manager");
              // Limit size of the window
            wnd.minSize = new Vector2(450, 200);
            wnd.maxSize = new Vector2(1920, 720);
        }

        public void OnEnable()
       {
           ruleTile = GameObject.FindObjectOfType<RuleTile>();
           if (ruleTile == null)
               return;

           var inspector = new InspectorElement(ruleTile);
           rootVisualElement.Add(inspector);
       }
        public void CreateGUI()
        {
            // Get a list of all sprites in the project
            var allObjectGuids = AssetDatabase.FindAssets("t:RuleTile");
            var allObjects = new List<RuleTile>();
            foreach (var guid in allObjectGuids)
            {
                allObjects.Add(AssetDatabase.LoadAssetAtPath<RuleTile>(AssetDatabase.GUIDToAssetPath(guid)));
            }

            // Create a two-pane view with the left pane being fixed with
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

            // Add the panel to the visual tree by adding it as a child to the root element
            rootVisualElement.Add(splitView);

            // A TwoPaneSplitView always needs exactly two child elements
            var leftPane = setupLeftPane(allObjects);
            splitView.Add(leftPane);
            m_RightPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            splitView.Add(m_RightPane);
        }

        private ListView setupLeftPane(List<RuleTile> allObjects){
            var leftPane = new ListView();
            // Initialize the list view with all sprites' names
            leftPane.makeItem = () => new Label();
            leftPane.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
            leftPane.itemsSource = allObjects;

            // React to the user's selection
            leftPane.onSelectionChange += OnRuleTileSelectionChange;

            // Restore the selection index from before the hot reload
            leftPane.selectedIndex = m_SelectedIndex;

            // Store the selection index when the selection changes
            leftPane.onSelectionChange += (items) => { m_SelectedIndex = leftPane.selectedIndex; };

            return leftPane;
        }


        private void OnRuleTileSelectionChange(IEnumerable<object> selectedItems)
        {
            // Clear all previous content from the pane
            m_RightPane.Clear();

            // Get the selected sprite
            var selectedRuleTile = selectedItems.First() as RuleTile;
            if (selectedRuleTile == null)
                return;

            // Add a new Image control and display the sprite
            var spriteImage = new Image();
            spriteImage.scaleMode = ScaleMode.ScaleToFit;
            spriteImage.sprite = selectedRuleTile.m_DefaultSprite;

            // Add the Image control to the right-hand pane
            m_RightPane.Add(spriteImage);
        }

        public void extractRulesFromRuleTile(RuleTile ruleTile){
            var rulesListRaw = ruleTile.m_TilingRules;
            var rulesList = JsonUtility.ToJson(rulesListRaw, true);
            rulesList.WriteToFile(ruleTile.name, "json");
        }

        private void applyRuleTilesToRuleTile(RuleTile ruleTile, List<Sprite> spriteList, string name){

            RuleTile rTile = ScriptableObject.CreateInstance("RuleTile") as RuleTile;

            if(ruleTile.m_TilingRules.Count == spriteList.Count){

                AssetDatabase.CreateAsset(rTile, "Assets/TileGen/Resources/ExportTilemap/" + name + "Ruletile.asset");
                String ruleTilePath = "Assets/TileGen/Resources/ExportTilemap/" + name + "Ruletile.asset";
                rTile.m_DefaultSprite = spriteList[0];

            }
        }


    }

    public static class Extensions{
        public static async void WriteToFile(this string data, string filename, string format = "txt")
        {
            byte[] bytesToWrite = Encoding.Unicode.GetBytes(data);

            string folderPath = @$"\\Assets\{format}";
            if(!Directory.Exists(folderPath)){
                Directory.CreateDirectory(folderPath);
            }
            string path = folderPath + $"\\{filename}";
            path = path.createUniqueNameFile(format);

            using (FileStream createdFile = File.Create(path, 4096, FileOptions.Asynchronous))
            {
                await createdFile.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
            }
        }

        public static string createUniqueNameFile(this string filePath, string format){
            int index=0;
            string finalPath = filePath + $".{format}";
            while(File.Exists(finalPath)){
                finalPath = filePath + index++ + $".{format}";
            }
            return finalPath;
        }
    }

    public class ExampleWindow : EditorWindow
    {
        [MenuItem("Tools/My Custom Editor")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<ExampleWindow>();
            wnd.titleContent = new GUIContent("My Custom Editor");
        }

        private void applyRuleTilesToRuleTile(RuleTile ruleTile, List<Sprite> spriteList, string name){

            RuleTile rTile = ScriptableObject.CreateInstance("RuleTile") as RuleTile;

            if(ruleTile.m_TilingRules.Count == spriteList.Count){

                AssetDatabase.CreateAsset(rTile, "Assets/TileGen/Resources/ExportTilemap/" + name + "Ruletile.asset");
                String ruleTilePath = "Assets/TileGen/Resources/ExportTilemap/" + name + "Ruletile.asset";
                rTile.m_DefaultSprite= spriteList[0];

            }
        }

        public void CreateGUI()
        {
            // Get a list of all sprites in the project
            var allObjectGuids = AssetDatabase.FindAssets("t:RuleTile");
            var allObjects = new List<RuleTile>();
            foreach (var guid in allObjectGuids)
            {
                allObjects.Add(AssetDatabase.LoadAssetAtPath<RuleTile>(AssetDatabase.GUIDToAssetPath(guid)));
            }
        }
    }



}