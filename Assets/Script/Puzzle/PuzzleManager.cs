using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    private enum ShufflePhase { Idle, Rising, Falling }

    private List<Element> _SelectElements = new List<Element>();
    private string _SelectID = "";
    private float _CurrentTime = 60;
    private bool _IsPlaying = true;
    private Vector2 _ShuffleBottomRestPosition;
    private ShufflePhase _ShufflePhase;
    private float _ShufflePhaseTime;
    private int _PendingShuffles;
    public static PuzzleManager Instance { get; private set;}
    public GameObject[] ElementPrefabs;
    public GameObject ExplosionPrefab;
    public LineRenderer LineRenderer;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI RoundText;
    public Button ShuffleButton;
    public Rigidbody2D ShuffleBottom;
    [Min(0.01f)] public float ShuffleLiftDistance = 0.8f;
    [Min(0.01f)] public float ShuffleRiseDuration = 0.06f;
    [Min(0.01f)] public float ShuffleReturnDuration = 0.12f;
    public int ElementDestroyCount = 3;
    public float PlayTime = 60;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        ElementSpawn(PlayerData.InitialElementCount);
        _CurrentTime = Mathf.Max(1f, PlayTime - (PlayerData.Round - 1) + PlayerData.TimeModifier);
        if (RoundText != null) RoundText.text = $"ラウンド {PlayerData.Round}/{PlayerData.LastRound}";
        if (ShuffleButton != null) ShuffleButton.onClick.AddListener(Shuffle);
        if (ShuffleBottom != null) _ShuffleBottomRestPosition = ShuffleBottom.position;
    }

    private void OnDestroy()
    {
        if (ShuffleButton != null) ShuffleButton.onClick.RemoveListener(Shuffle);
    }

    // Update is called once per frame
    void Update()
    {
        LineRendererUpdate();
        TimerUpdate();
    }

    private void FixedUpdate()
    {
        if (ShuffleBottom == null) return;

        if (_ShufflePhase == ShufflePhase.Idle)
        {
            if (_PendingShuffles == 0) return;
            _PendingShuffles--;
            _ShufflePhase = ShufflePhase.Rising;
            _ShufflePhaseTime = 0f;
        }

        _ShufflePhaseTime += Time.fixedDeltaTime;
        if (_ShufflePhase == ShufflePhase.Rising)
        {
            float progress = Mathf.Clamp01(_ShufflePhaseTime / ShuffleRiseDuration);
            ShuffleBottom.MovePosition(_ShuffleBottomRestPosition + Vector2.up * (ShuffleLiftDistance * progress));
            if (progress >= 1f)
            {
                _ShufflePhase = ShufflePhase.Falling;
                _ShufflePhaseTime = 0f;
            }
        }
        else
        {
            float progress = Mathf.Clamp01(_ShufflePhaseTime / ShuffleReturnDuration);
            ShuffleBottom.MovePosition(_ShuffleBottomRestPosition + Vector2.up * (ShuffleLiftDistance * (1f - progress)));
            if (progress >= 1f) _ShufflePhase = ShufflePhase.Idle;
        }
    }

    private void TimerUpdate()
    {
        if (_IsPlaying)
        {
            _CurrentTime -= Time.deltaTime;
            if(_CurrentTime <= 0)
            {
                StopPuzzle();
                CancelSelection();
                BattleManager.Instance.PlayerDead();
            }
            TimerText.text = GetRemainTime().ToString();
        }
    }

    private void LineRendererUpdate()
    {
        if(_SelectElements.Count >= 2)
        {
            LineRenderer.positionCount = _SelectElements.Count;
            LineRenderer.SetPositions(_SelectElements.Select(element => element.transform.position).ToArray());
            LineRenderer.gameObject.SetActive(true);
        }
        else LineRenderer.gameObject.SetActive(false);
    }

    private void ElementSpawn(int count, int guaranteedCritical = 0)
    {
        const float minX = -6.6f;
        const float maxX = 6.6f;
        const float startY = 2.5f;
        const float rowSpacing = 2f;

        float x = minX;
        float y = startY;

        for(int i = 0; i < count; i++)
        {
            float nextX = x + Random.Range(1f, 3f);

            if (nextX > maxX)
            {
                x = minX;
                y += rowSpacing;
            }
            else
            {
                x = nextX;
            }

            float spawnY = y + Random.Range(-1f, 1f);
            var position = new Vector3(x, spawnY, 0);
            bool explosion = i >= guaranteedCritical && ExplosionPrefab != null
                && Random.value < Mathf.Clamp01(PlayerData.ExplosionElementChance);
            var elementObject = Instantiate(
                explosion ? ExplosionPrefab : ChooseElementPrefab(),
                position,
                Quaternion.identity
            );
            var element = elementObject.GetComponent<Element>();
            if (!explosion)
            {
                ElementEffectAssigner.AssignRandomEffect(element);
                if (i < guaranteedCritical) element.SetEffect(element.Effect | ElementEffect.Critical);
            }
        }
    }

    private GameObject ChooseElementPrefab()
    {
        int totalWeight = 0;
        foreach (var prefab in ElementPrefabs)
        {
            var element = prefab.GetComponent<Element>();
            if (element != null) totalWeight += Mathf.Max(0, PlayerData.ElementSpawnWeights[(int)element.Attribute]);
        }

        if (totalWeight <= 0) return ElementPrefabs[Random.Range(0, ElementPrefabs.Length)];

        int pick = Random.Range(0, totalWeight);
        foreach (var prefab in ElementPrefabs)
        {
            var element = prefab.GetComponent<Element>();
            if (element == null) continue;
            pick -= Mathf.Max(0, PlayerData.ElementSpawnWeights[(int)element.Attribute]);
            if (pick < 0) return prefab;
        }
        return ElementPrefabs[ElementPrefabs.Length - 1];
    }

    public void ElementDown(Element element)
    {
        if(!_IsPlaying || element == null)return;
        _SelectElements.Add(element);
        element.SetIsSelect(true);

        _SelectID = element.ID;
    }

    public void Explode(Element center)
    {
        if (!_IsPlaying || center == null || !center.IsExplosion) return;
        CancelSelection();

        var all = FindObjectsByType<Element>(FindObjectsSortMode.None);
        DestroyElements(CollectExplosionElements(center, all), true);
    }

    private static List<Element> CollectExplosionElements(Element center, Element[] all)
    {
        var remaining = new Dictionary<Element, int> { [center] = Mathf.Max(1, PlayerData.ExplosionRange) };
        var queue = new Queue<Element>();
        queue.Enqueue(center);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int steps = current.IsExplosion ? Mathf.Max(1, PlayerData.ExplosionRange) : remaining[current];
            if (steps <= 0) continue;
            foreach (var candidate in all)
            {
                if (candidate == current || !AreTouching(current, candidate)) continue;
                int nextSteps = steps - 1;
                if (remaining.TryGetValue(candidate, out int previous) && previous >= nextSteps) continue;
                remaining[candidate] = nextSteps;
                queue.Enqueue(candidate);
            }
        }

        return remaining.Keys.ToList();
    }

    private static bool AreTouching(Element first, Element second)
    {
        var firstCollider = first.GetComponent<CircleCollider2D>();
        var secondCollider = second.GetComponent<CircleCollider2D>();
        if (firstCollider == null || secondCollider == null) return false;
        float radius = firstCollider.bounds.extents.x + secondCollider.bounds.extents.x;
        return Vector2.Distance(first.transform.position, second.transform.position) <= radius + 0.05f;
    }

    public void ElementEnter(Element element)
    {
        if(!_IsPlaying || _SelectElements.Count == 0 || _SelectID != element.ID) return;

        if (element.IsSelect)
        {
            if(_SelectElements.Count >= 2 && _SelectElements[_SelectElements.Count - 2] == element)
            {
                var RemoveElement = _SelectElements[_SelectElements.Count - 1];
                RemoveElement.SetIsSelect(false);
                _SelectElements.Remove(RemoveElement);
            }

        } 
        else
        {
            var previous = _SelectElements[_SelectElements.Count - 1];
            float centerDistance = Vector2.Distance(previous.transform.position, element.transform.position);
            var previousCollider = previous.GetComponent<CircleCollider2D>();
            var nextCollider = element.GetComponent<CircleCollider2D>();
            float previousRadius = previousCollider != null ? previousCollider.bounds.extents.x : 0f;
            float nextRadius = nextCollider != null ? nextCollider.bounds.extents.x : 0f;
            float gap = Mathf.Max(0f, centerDistance - previousRadius - nextRadius);
            if(gap <= PlayerData.ElementConnectRange)
            {
                _SelectElements.Add(element);
                element.SetIsSelect(true);
            }
        }
    }

    public void ElementUp()
    {
        if (!_IsPlaying) { CancelSelection(); return; }
        if(_SelectElements.Count >= ElementDestroyCount)
        {
            DestroyElements(_SelectElements);
        }
        else
        {
            foreach(var ElementItem in _SelectElements)
            {
                ElementItem.SetIsSelect(false);
            }
        }

        _SelectID = "";
        _SelectElements.Clear();
    }

    private void DestroyElements(List<Element> elements, bool explosion = false)
    {
        int AddMoney = 0;
        int criticalCount = 0;
        var attackGroups = explosion
            ? elements.Where(element => !element.IsExplosion && element.Attribute != ElementAttribute.None)
                .GroupBy(element => element.Attribute).OrderBy(group => group.Key).Select(group => group.ToList()).ToList()
            : null;
        foreach(var ElementItem in elements)
        {
            if((ElementItem.Effect & ElementEffect.Gold) != 0)
            {
                AddMoney++;
            }
            if ((ElementItem.Effect & ElementEffect.Critical) != 0) criticalCount++;
            Destroy(ElementItem.gameObject);
        }
        if (AddMoney > 0)
        {
            PlayerData.PiggyBankTmp += AddMoney;
        }
        if (explosion) BattleManager.Instance.AttackEnemyGroups(attackGroups);
        else BattleManager.Instance.AttackEnemy(elements);
        if (_IsPlaying) ElementSpawn(elements.Count, Mathf.Max(0, criticalCount - 1));
    }

    public void StopPuzzle()
    {
        _IsPlaying = false;
        _PendingShuffles = 0;
        CancelSelection();
    }

    public int GetRemainTime()
    {
        return Mathf.Max(0, Mathf.CeilToInt(_CurrentTime));
    }

    private void CancelSelection()
    {
        foreach (var element in _SelectElements)
            if (element != null) element.SetIsSelect(false);
        _SelectElements.Clear();
        _SelectID = "";
    }

    public void Shuffle()
    {
        if (!_IsPlaying || ShuffleBottom == null) return;
        _PendingShuffles++;
    }
}
