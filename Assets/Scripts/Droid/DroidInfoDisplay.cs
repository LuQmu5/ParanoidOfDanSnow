using TMPro;
using UnityEngine;

public class DroidInfoDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _distanceText;
    [SerializeField] private TMP_Text _scanLevelText;
    [SerializeField] private TMP_Text _paranoidLevelText;

    public void SetDistance(float value)
    {
        if (value > 50)
        {
            _distanceText.text = "Losing signal...\n\n\n\n\n\n\n\n\n\n\n";
            _distanceText.color = Color.red;
        }
        else if (value < 10)
        {
            _distanceText.text = "Dangerous ";
            _distanceText.color = Color.red;
        }
        else if (value < 20)
        {
            _distanceText.text = "Carefull ";
            _distanceText.color = Color.yellow;
        }
        else
        {
            _distanceText.text = "Normal ";
            _distanceText.color = Color.green;
        }

        _distanceText.text += "Distance:\n" + value.ToString("00.00");
    }

    public void SetScanLevel(float value)
    {
        _scanLevelText.text = "Scan progress:\n" + value.ToString("00.00") + "%";
    }

    public void SetParanoidLevel(float value)
    {
        _paranoidLevelText.text = "Paranoid level:\n" + value.ToString("00.00") + "%";
    }
}
