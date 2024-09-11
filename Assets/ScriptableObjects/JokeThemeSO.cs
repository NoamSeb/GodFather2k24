using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/JokeTheme")]
    public class JokeThemeSO : ScriptableObject
    {
        [SerializeField] private string _theme;
        [SerializeField] private float _bubbleSpeedMulti = 1;
        [SerializeField] private int _quality;
        [SerializeField, TextArea] private List<string> _jokeList;

        public string Theme => _theme;
        public List<string> JokeList => _jokeList;
        public int Quality => _quality;
        public float BubbleSpeedMulti => _bubbleSpeedMulti;

        public string Joke(List<string> usedJokes)
        {
            List<string> jokeList = new List<string>();
            foreach (string joke in _jokeList)
            {
                jokeList.Add(joke);
            }
            foreach (string usedJoke in usedJokes)
            {
                if (jokeList.Contains(usedJoke))
                {
                    jokeList.Remove(usedJoke);
                }
            }
            if (jokeList.Count <= 0)
            {
                foreach (string joke in _jokeList)
                {
                    jokeList.Add(joke);
                }
            }
            return jokeList[Random.Range(0, jokeList.Count)];
        }
    }
}
