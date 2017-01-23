using UnityEngine;
using UnityEngine.UI;

public class InputVisualizer : MonoBehaviour
{
    public NumpadInput NI;

    void Update()
    {
        float size = NI.GetNormalizedInputTimeleft();
        transform.localScale = new Vector3(size, size, 1f);
        if (NI.MoreThanThree())
            GetComponent<Image>().color = Color.red;
        else
            GetComponent<Image>().color = Color.green;
    }
}
