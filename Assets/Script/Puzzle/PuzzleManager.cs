using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    private List<Element> _SelectElements = new List<Element>();
    private string _SelectID = "";
    private float _CurrentTime = 60;
    private bool _IsPlaying = true;
    public static PuzzleManager Instance { get; private set;}
    public GameObject[] ElementPrefabs;
    public LineRenderer LineRenderer;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI RoundText;
    public Button ShuffleButton;
    public int ElementDestroyCount = 3;
    public float PlayTime = 60;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        ElementSpawn(30);
        _CurrentTime = Mathf.Max(1f, PlayTime - (PlayerData.Round - 1));
        if (RoundText != null) RoundText.text = $"ラウンド {PlayerData.Round}/{PlayerData.LastRound}";
        if (ShuffleButton != null) ShuffleButton.onClick.AddListener(Shuffle);
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
            var elementObject = Instantiate(
                ElementPrefabs[Random.Range(0, ElementPrefabs.Length)],
                position,
                Quaternion.identity
            );
            var element = elementObject.GetComponent<Element>();
            ElementEffectAssigner.AssignRandomEffect(element);
            if (i < guaranteedCritical) element.SetEffect(element.Effect | ElementEffect.Critical);
        }
    }

    public void ElementDown(Element element)
    {
        if(!_IsPlaying || element == null)return;
        _SelectElements.Add(element);
        element.SetIsSelect(true);

        _SelectID = element.ID;
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

    private void DestroyElements(List<Element> elements)
    {
        int AddMoney = 0;
        int criticalCount = 0;
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
        BattleManager.Instance.AttackEnemy(elements);
        if (_IsPlaying) ElementSpawn(elements.Count, Mathf.Max(0, criticalCount - 1));
    }

    public void StopPuzzle()
    {
        _IsPlaying = false;
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
        if (!_IsPlaying) return;
        Vector2 origin = new Vector2(0f, -15f);
        foreach (var element in FindObjectsByType<Element>())
        {
            var body = element.GetComponent<Rigidbody2D>();
            if (body == null) continue;
            Vector2 direction = ((Vector2)element.transform.position - origin).normalized;
            direction.x += Random.Range(-0.3f, 0.3f);
            body.AddForce(direction.normalized * Random.Range(5f, 9f), ForceMode2D.Impulse);
        }
    }
}
