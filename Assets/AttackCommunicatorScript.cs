using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackCommunicatorScript : MonoBehaviour
{
    public Text[] KeysForSharkAttack;
    public Text[] KeysForWaveAttack;
    public Text KeyForStormAttack;

    internal void ShowSharkAttack(List<int> currentAttackList)
    {
        int i = 0;
        foreach (int key in currentAttackList)
        {
            KeysForSharkAttack[i].text = key.ToString();
            i++;
        }
    }

    internal void ShowWaveAttack(List<int> currentAttackList)
    {
        int i = 0;
        foreach (int key in currentAttackList)
        {
            KeysForWaveAttack[i].text = key.ToString();
            i++;
        }
    }

    internal void ShowStormAttack(List<int> currentAttackList)
    {
        KeyForStormAttack.text = currentAttackList[0].ToString();
    }

    public void ClearSharkAttackIndicators()
    {
        foreach (var item in KeysForSharkAttack)
        {
            item.text = "";
        }
    }

    public void ClearWaveAttackIndicators()
    {
        foreach (var item in KeysForWaveAttack)
        {
            item.text = "";
        }
    }

    public void ClearStormAttackIndicator()
    {
        KeyForStormAttack.text = "";
    }
}
