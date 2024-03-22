using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SnakeGame
{
    public sealed class SaveManager
    {
        TextAsset saveFile;
        private SaveManager() {
            }

        private List<Punctuation> punctuationList;
        private static SaveManager _instance;

        private static readonly string saveLocation = "/SnakeWFC.saveData.json";

        public static SaveManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SaveManager();
            }
            return _instance;
        }

        public void getHighestPunctuation()
        {

        }

        Punctuation[] GetAllPunctuations()
        {
            try 
            {
                string jsonText = File.ReadAllText(saveLocation);
                return JsonHelper.FromJson<Punctuation>(jsonText);
            } 
            catch (IOException e)
            {
                Debug.Log($"Couldnt Find the Save File due to a {e.GetType()}, creating new File...");
                createSaveLocation();
            }
            return null;
        }
        

        void createSaveLocation(){
            try{
                File.Create(saveLocation);
                
            }
            catch(Exception e)
            {
                Debug.Log($"Couldnt create the Save File due to a {e.GetType()}, progress will not be saved inbetween sessions");
            }
        }
        PlayerPrefs prefs;

    }

    class PlayerPrefsManager {

        private PlayerPrefsManager() { 

            keys = new Dictionary<string, Type>();

            keys.Add("Master Volume", typeof(int));
            keys.Add("Sound Effects Volume", typeof(int));
            keys.Add("Music Volume", typeof(int));
        }

        private List<Punctuation> punctuationList;
        private static PlayerPrefsManager _instance;

         public static PlayerPrefsManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PlayerPrefsManager();
            }
            return _instance;
        }
        private Dictionary<string, Type> keys;
    }


    [Serializable]
    public class Punctuation
    {
        int level = 0;
        int food = 0;
        float speed = 1;
        DateTime date;
        public string name;

        [NonSerialized]
        private static readonly int minNameLength = 3;
        [NonSerialized]
        private static readonly int maxNameLength = 10;
        [NonSerialized]
        private static readonly string regexText = "^[a-zA-Z0-9]{3,10}$";
        [NonSerialized]
        private System.Text.RegularExpressions.Regex regex;

        public Punctuation(int level, int food, float speed, string name = null){
            regex = new System.Text.RegularExpressions.Regex(regexText);
            this.level = level;
            this.food= food;
            this.speed = speed;
            this.name = regex.IsMatch(name)?name: throw new FormatException();
            date = DateTime.Now;
        }

        public Punctuation(int level, int food, float speed, DateTime date, string name)
        {
            this.level = level;
            this.food = food;
            this.speed = speed;
            this.date = date;
            this.name = name;
            regex = new System.Text.RegularExpressions.Regex(regexText);
        }
    }

    


    
}