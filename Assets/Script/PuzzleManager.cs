using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class PuzzleManager : MonoBehaviour
{
    private List<Element> _SelectElements = new List<Element>();
    private string _SelectID = "";
    private int _Score = 0;
    private float _CurrentTime = 60;
    private bool _IsPlaying = true;
    public static PuzzleManager Instance { get; private set;}
    public GameObject[] ElementPrefabs;
    public LineRenderer LineRenderer;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimerText;
    public int ElementDestroyCount = 3;
    public float ElementConnectRange = 1.5f;
    public float PlayTime = 60;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        ElementSpawn(40);
        ScoreText.text = "0";
        _CurrentTime = PlayTime;
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
                ElementUp();
                BattleManager.Instance.PlayerDead();
            }
            TimerText.text = ((int)_CurrentTime).ToString();
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

    private void ElementSpawn(int count)
    {
        const float minX = -8f;
        const float maxX = 8f;
        const float startY = 17f;
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
            Instantiate(
                ElementPrefabs[Random.Range(0, ElementPrefabs.Length)],
                position,
                Quaternion.identity
            );
        }
    }

    public void ElementDown(Element element)
    {
        if(!_IsPlaying)return;
        _SelectElements.Add(element);
        element.SetIsSelect(true);

        _SelectID = element.ID;
    }

    public void ElementEnter(Element element)
    {
        if(_SelectID != element.ID) return;

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
            var Length = (_SelectElements[_SelectElements.Count - 1].transform.position - element.transform.position).magnitude;
            if(Length < ElementConnectRange)
            {
                _SelectElements.Add(element);
                element.SetIsSelect(true);
            }
        }
    }

    public void ElementUp()
    {
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
        foreach(var ElementItem in elements)
        {
            Destroy(ElementItem.gameObject);
        }
        ElementSpawn(elements.Count);
        AddScore(elements.Count);
        BattleManager.Instance.AttackEnemy(elements.Count);
    }
    
    private void AddScore(int elementCount)
    {
        _Score += (int)(elementCount * 100 * ( 1 + (elementCount - 3) * 0.1f ));
        ScoreText.text = _Score.ToString();
    }

    public void StopPuzzle()
    {
        _IsPlaying = false;
    }

    public int GetRemainTime()
    {
        return (int)_CurrentTime;
    }
}
