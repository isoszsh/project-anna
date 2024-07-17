using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushController : MonoBehaviour
{

    public Color currentColor = Color.white;

    public Material brushMaterial;

    private void Start()
    {
        brushMaterial.color = Color.white;
    }

    public void SetColor(Color newColor)
    {
        Color purple = new Color(0.5f, 0.0f, 0.5f);
        Color brown = new Color(0.6f, 0.4f, 0.2f);
        Color orange = new Color(1.0f, 0.5f, 0.0f);
        Color pink = new Color(1.0f, 0.4f, 0.7f);
        Color darkGreen = new Color(0.0f, 0.5f, 0.0f);
        Color lightGreen = new Color(0.5f, 1.0f, 0.5f);
        Color darkBlue = new Color(0.0f, 0.0f, 0.5f);
        Color lightBlue = new Color(0.5f, 0.5f, 1.0f);

        if (newColor == currentColor) return;

        if (newColor == Color.white)
        {
            currentColor = Color.white;
            brushMaterial.color = currentColor;
            return;
        }
        if (currentColor == Color.white)
        {
            currentColor = newColor;
            brushMaterial.color = currentColor;
        }
        else
        {
            // Renk kar���mlar�
            if ((currentColor == Color.yellow && newColor == Color.green) ||
                (currentColor == Color.green && newColor == Color.yellow))
            {
                currentColor = orange; // Sar� + Ye�il = Turuncu
            }
            else if ((currentColor == Color.blue && newColor == Color.red) ||
                     (currentColor == Color.red && newColor == Color.blue))
            {
                currentColor = purple; // Mavi + K�rm�z� = Mor
            }
            else if ((currentColor == Color.blue && newColor == Color.green) ||
                     (currentColor == Color.green && newColor == Color.blue))
            {
                currentColor = Color.cyan; // Mavi + Ye�il = Turkuaz
            }
            else if ((currentColor == Color.red && newColor == Color.green) ||
                     (currentColor == Color.green && newColor == Color.red))
            {
                currentColor = brown; // K�rm�z� + Ye�il = Kahverengi
            }
            else if ((currentColor == Color.yellow && newColor == Color.blue) ||
                     (currentColor == Color.blue && newColor == Color.yellow))
            {
                currentColor = Color.green; // Sar� + Mavi = Ye�il
            }
            else if ((currentColor == Color.yellow && newColor == Color.red) ||
                     (currentColor == Color.red && newColor == Color.yellow))
            {
                currentColor = pink; // Sar� + K�rm�z� = Pembe
            }
            else if ((currentColor == Color.yellow && newColor == Color.cyan) ||
                     (currentColor == Color.cyan && newColor == Color.yellow))
            {
                currentColor = lightGreen; // Sar� + Turkuaz = A��k Ye�il
            }
            else if ((currentColor == Color.red && newColor == Color.cyan) ||
                     (currentColor == Color.cyan && newColor == Color.red))
            {
                currentColor = darkGreen; // K�rm�z� + Turkuaz = Koyu Ye�il
            }
            else if ((currentColor == Color.green && newColor == purple) ||
                     (currentColor == purple && newColor == Color.green))
            {
                currentColor = darkBlue; // Ye�il + Mor = Koyu Mavi
            }
            else if ((currentColor == Color.blue && newColor == purple) ||
                     (currentColor == purple && newColor == Color.blue))
            {
                currentColor = lightBlue; // Mavi + Mor = A��k Mavi
            }

            brushMaterial.color = currentColor;
        }
    }

}
