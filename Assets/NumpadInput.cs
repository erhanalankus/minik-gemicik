using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumpadInput : MonoBehaviour
{
    public float attackActivationTime;
    public bool[] redAttackIcons;
    public int attackCount = 0;

    List<int> attackInputList;
    float timeFromLastPress = 0;

    void Start()
    {
        attackInputList = new List<int>();
        redAttackIcons = new bool[3];
    }

    void Update()
    {
        CatchButtonPress();
        timeFromLastPress += Time.deltaTime;
        if (timeFromLastPress > attackActivationTime && attackInputList.Count > 0)
        {
            if (attackInputList.Count > 3)
            {
                attackInputList.RemoveRange(3, attackInputList.Count - 3);
            }
            PerformAttack(attackInputList);
            attackInputList.Clear();
        }
    }

    void PerformAttack(List<int> currentAttackList)
    {
        if (GetComponent<GMScript>().isGameOver)
        {
            return;
        }
        attackCount++;
        if (currentAttackList.Count == 1)
        {
            if (GetComponent<GMScript>().CanPerformAttack(2))
            {
                GetComponent<AttackCommunicatorScript>().ShowStormAttack(currentAttackList);
                GetComponent<GMScript>().SendStorm(currentAttackList);
            }
            else
            {
                StartCoroutine("Switch", 2);
            }
        }
        else if (currentAttackList.Count == 2)
        {
            if (GetComponent<GMScript>().CanPerformAttack(1))
            {
                GetComponent<AttackCommunicatorScript>().ShowWaveAttack(currentAttackList);
                GetComponent<GMScript>().SendWave(currentAttackList);
            }
            else
            {
                StartCoroutine("Switch", 1);
            }
        }
        else if (currentAttackList.Count == 3)
        {
            if (GetComponent<GMScript>().CanPerformAttack(0))
            {
                GetComponent<AttackCommunicatorScript>().ShowSharkAttack(currentAttackList);
                GetComponent<GMScript>().SendSharksTo(currentAttackList);
            }
            else
            {
                StartCoroutine("Switch", 0);
            }
        }
        else
        {
            Debug.LogError("Attack list is not proper!");
        }
    }
    public float GetNormalizedInputTimeleft()
    {
        return Mathf.Max(1f - timeFromLastPress / attackActivationTime, 0);
    }
    void CatchButtonPress()
    {

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            attackInputList.Add(1);
            PressedAButton();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            attackInputList.Add(2);
            PressedAButton();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            attackInputList.Add(3);
            PressedAButton();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            attackInputList.Add(4);
            PressedAButton();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            attackInputList.Add(5);
            PressedAButton();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            attackInputList.Add(6);
            PressedAButton();
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            attackInputList.Add(7);
            PressedAButton();
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            attackInputList.Add(8);
            PressedAButton();
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            attackInputList.Add(9);
            PressedAButton();
        }
    }

    private void PressedAButton()
    {
        ResetTimer();
    }

    private void ResetTimer()
    {
        timeFromLastPress = 0;
    }

    public bool MoreThanThree()
    {
        return attackInputList.Count > 3;
    }

    IEnumerator Switch(int i)
    {
        /*Color tem=GetComponent<Image> ().color;
		GetComponent<Image> ().color = Color.red;*/
        for (int j = 0; j < 3; j++)
        {
            redAttackIcons[i] = true;
            yield return new WaitForSeconds(0.05f);
            redAttackIcons[i] = false;
            yield return new WaitForSeconds(0.05f);
        }
        /*
		GetComponent<Image> ().color = tem;*/
    }
}
