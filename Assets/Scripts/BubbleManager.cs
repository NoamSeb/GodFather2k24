using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private RandomManager _randomManager;
    [SerializeField] private List<Vector2> _spawnPos;
    [SerializeField] private GameObject _thoughtScreen;
    [SerializeField] private Vector2 _dirDiffPos;


    [Button]
    private void Test()
    {
        StartCoroutine(LaunchGame(30));
    }
    private IEnumerator LaunchGame(float timer)
    {
        List<JokeThemeSO> jokeThemeSos = _randomManager.GetThemeList();
        float cooldown = (timer + 1) / jokeThemeSos.Count;
        foreach (JokeThemeSO jokeThemeSo in jokeThemeSos)
        {
            CreateBubble(jokeThemeSo);
            yield return new WaitForSeconds(cooldown);
        }
    }
    
    private void CreateBubble(JokeThemeSO jokeThemeSo)
    {
        ThemeBubbleBehaviour bubble = Instantiate(_bubblePrefab, _spawnPos[Random.Range(0,_spawnPos.Count)], Quaternion.identity).GetComponent<ThemeBubbleBehaviour>();
        Vector2 dir = _thoughtScreen.transform.position - (bubble.transform.position + new Vector3(Random.Range(-_dirDiffPos.x, _dirDiffPos.x),Random.Range(-_dirDiffPos.y, _dirDiffPos.y)));
        bubble.SetupBubble(dir, jokeThemeSo);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_thoughtScreen.transform.position, _thoughtScreen.transform.localScale);
        Gizmos.color = Color.green;
        foreach (Vector2 spawnPos in _spawnPos)
        {
            Gizmos.DrawWireSphere(spawnPos, 0.5f);
        }
    }
}
