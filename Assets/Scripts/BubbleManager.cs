using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private GameObject _bonusBubblePrefab;
    [SerializeField] private RandomManager _randomManager;
    [SerializeField] private List<Vector2> _spawnPos;
    [SerializeField] private GameObject _thoughtScreen;
    [SerializeField] private Vector2 _dirDiffPos;

    private GameManager _gameManager;

    [Button]
    private void Test()
    {
        StartCoroutine(SpawnThemesCoroutine(30));
    }

    public void StartBubbles(float timer, GameManager gameManager)
    {
        _gameManager = gameManager;
        StartCoroutine(SpawnThemesCoroutine(timer));
        StartCoroutine(SpawnBonusCoroutine(timer));
    }

    private IEnumerator SpawnBonusCoroutine(float timer)
    {
        List<BonusType> bonusTypes = _randomManager.GetBonusList();
        float cooldown = (timer + 1) / bonusTypes.Count;
        foreach (BonusType bonusType in bonusTypes)
        {
            CreateBubble(bonusType);
            yield return new WaitForSeconds(cooldown - Random.Range(-cooldown/3,cooldown/3));
        }
    }

    private IEnumerator SpawnThemesCoroutine(float timer)
    {
        List<JokeThemeSO> jokeThemeSos = _randomManager.GetThemeList();
        float cooldown = (timer + 1) / jokeThemeSos.Count;
        foreach (JokeThemeSO jokeThemeSo in jokeThemeSos)
        {
            CreateBubble(jokeThemeSo);
            yield return new WaitForSeconds(cooldown);
        }
        _gameManager.StartPhase2();
    }
    
    private void CreateBubble(JokeThemeSO jokeThemeSo)
    {
        ThemeBubbleBehaviour bubble = Instantiate(_bubblePrefab, _spawnPos[Random.Range(0,_spawnPos.Count)], Quaternion.identity).GetComponent<ThemeBubbleBehaviour>();
        Vector2 dir = _thoughtScreen.transform.position - (bubble.transform.position + new Vector3(Random.Range(-_dirDiffPos.x, _dirDiffPos.x),Random.Range(-_dirDiffPos.y, _dirDiffPos.y)));
        bubble.SetupBubble(dir, jokeThemeSo);
    }
    
    private void CreateBubble(BonusType bonusType)
    {
        BonusBubbleBehaviour bubble = Instantiate(_bonusBubblePrefab, _spawnPos[Random.Range(0,_spawnPos.Count)], Quaternion.identity).GetComponent<BonusBubbleBehaviour>();
        Vector2 dir = _thoughtScreen.transform.position - (bubble.transform.position + new Vector3(Random.Range(-_dirDiffPos.x, _dirDiffPos.x),Random.Range(-_dirDiffPos.y, _dirDiffPos.y)));
        bubble.SetupBubble(dir, bonusType);
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
