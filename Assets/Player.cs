using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] Character selectedCharacter;
    [SerializeField] Transform attRef;
    [SerializeField] bool isBot;
    [SerializeField] List<Character> characterlist;
    [SerializeField] UnityEvent onTakeDamage;

    public Character SelectedCharacter { get => selectedCharacter; }
    public List<Character> Characterlist { get => characterlist; }

    private void Start() 
    {
        if (isBot)
        {
            foreach (var character in characterlist)
            {
                character.Button.interactable = false;
            }
        }
        
    }

    public void Prepare()
    {
        selectedCharacter = null;
    }
    
    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
    }

    public void SetPlay(bool value)
    {
        if(isBot)
        {
            List<Character> lotteryList = new List<Character>();
            foreach (var character in characterlist)
            {
                int ticket =Mathf.CeilToInt(f: ((float) character.CurrentHp / (float) character.MaxHP) * 10);
                for (int i = 0; i < ticket; i++)
                {
                    lotteryList.Add(item: character);
                }
            }

            int index = Random.Range(0,maxExclusive: lotteryList.Count);
            
            selectedCharacter = lotteryList[index];
        }
        else
        {
            foreach (var character in characterlist)
            {
                character.Button.interactable = value;
            }
        }
        
    }
    public void Attack()
    {
        selectedCharacter.transform.DOMove(endValue: attRef.position, 0.5f);
    }

    public bool isAttacking()
    {
        if(selectedCharacter == null)
            return false;
        return DOTween.IsTweening(selectedCharacter.transform);
    }

    public void TakeDamage(int damageValue)
    {
        selectedCharacter.ChangeHP(amount: -damageValue);
        var spriteRend = selectedCharacter.GetComponent<SpriteRenderer>();
        spriteRend.DOColor(endValue: Color.red,0.1f).SetLoops(6, loopType: LoopType.Yoyo);
        onTakeDamage.Invoke();
    }
    public bool IsDamaging()
    {
        if(selectedCharacter == null)
            return false;
        var spriteRend = selectedCharacter.GetComponent<SpriteRenderer>();
        return DOTween.IsTweening(targetOrId: spriteRend);
    }

    public void Remove(Character character)
    {
        if(characterlist.Contains(item: character)==false)
            return;
        
        if(selectedCharacter == character)
            selectedCharacter = null;
    
        character.Button.interactable = false;
        character.gameObject.SetActive(false);
        characterlist.Remove(item: character);
    }

    public void Return()
    {
        selectedCharacter.transform.DOMove(endValue: selectedCharacter.InitialPosition, 0.5f);
    }


    public bool IsReturning()
    {
        if(selectedCharacter == null)
            return false;

        return DOTween.IsTweening(targetOrId: selectedCharacter.transform);
    }
}
