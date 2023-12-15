using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour, IGetBuff
{
    [SerializeField] private float MoveSpeed;
    private float MoveHorizontal;
    private List<GameObject> CharactorList = new();
    private List<Animator> AnimatorList = new();
    private readonly List<Vector3> CharactorPosition = new() { Vector3.zero, new(-0.56f, 0, -0.65f), new(0.56f, 0, -0.65f) };
    public int CharactorLevel = 0;
    public GameDefined.CharactorType Test;
    public GameDefined.CharactorType CurrentCharactor;
    public static Action<GameDefined.CharactorType> OnCharactorUpdate;
    private bool IsSetting;

    void Start()
    {
        AddCharactor();
    }

    void Update()
    {
        if (IsSetting) return;
        if (Input.GetKeyDown(KeyCode.J))
        {
            // SetCharactor(Test);
            CharactorLevelUp();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            AddCharactor();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            RemoveCharactor();
        }
    }

    void FixedUpdate()
    {
        if (IsSetting) return;
        MoveHorizontal = Input.GetAxis("Horizontal");

        gameObject.transform.Translate(MoveHorizontal * MoveSpeed * Time.deltaTime, 0, 0);
        gameObject.transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.0f, 4.0f), 0, 0);

        if (MoveHorizontal == 0)
        {
            return;
        }
        PlayCharactorMoveAnimation();
    }

    void CharactorLevelUp()
    {
        if (CharactorLevel >= GameDefined.GetItemTypeCount()) return;
        CharactorLevel++;
        SetCharactor((GameDefined.CharactorType)CharactorLevel);
    }

    void SetCharactor(GameDefined.CharactorType type)
    {
        if (CurrentCharactor == type) return;
        CurrentCharactor = type;
        ReCreateCharactor();
    }

    public void AddCharactor()
    {
        if (CharactorList.Count >= 3) return;

        GameObject go = Resources.Load<GameObject>($"{GameDefined.CHARACTOR_PATH}{CurrentCharactor.GetDescriptionText()}");
        Instantiate(go, transform);
        UpdateCharactor();
    }

    public void RemoveCharactor()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("GAME OVER");
            return;
        }

        Destroy(CharactorList.Last());

        Utilities.ActionDelay(() =>
        {
            UpdateCharactor();
        }, 0.1f);
    }

    void ReCreateCharactor()
    {
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        CharactorList.Clear();
        AnimatorList.Clear();

        Utilities.ActionDelay(() =>
        {
            for (int i = 0; i < count; i++)
            {
                AddCharactor();
            }
        }, 0.1f);
    }

    public void UpdateCharactor()
    {
        IsSetting = true;
        OnCharactorUpdate?.Invoke(CurrentCharactor);

        CharactorList.Clear();
        AnimatorList.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            CharactorList.Add(transform.GetChild(i).gameObject);
            CharactorList[i].transform.localPosition = CharactorPosition[i];

            Animator animator = transform.GetChild(i).GetComponent<Animator>();
            AnimatorList.Add(animator);
            animator.enabled = false;
        }

        foreach (var animator in AnimatorList)
        {
            animator.enabled = true;
        }
        IsSetting = false;
    }

    void PlayCharactorMoveAnimation()
    {
        if (AnimatorList.Count <= 0)
        {
            Debug.Log("Can't Find Charactor's Animator");
            return;
        }

        foreach (var animator in AnimatorList)
        {
            if (animator != null)
                animator.SetFloat("MovementH", MoveHorizontal);
        }
    }

    public void GetBuff(GameDefined.ItemType type)
    {
        switch (type.GetDescriptionText())
        {
            case "AddPeople":
                AddCharactor();
                break;
            case "LevelUp":
                CharactorLevelUp();
                break;
            default:
                break;
        }
    }
}
