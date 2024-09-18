using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float typingSpeed = 0.05f;
    public float dotAddingInterval = 0.5f;
    public float dotEffectDuration = 3.0f; // Ka� saniye boyunca . .. ... efekti olacak

    private string[] sentences = {
        "Anna, seni g�remez",
        "Anna, seni duyamaz",
        "Anna, dondurman�n tad�n� bilmiyor",
        "Anna, hi� ku� g�rmedi",
        "Anna, ya�murdan sonra topra�� koklayamaz",
        "Anna, pelu� oyuncaklar�n�n nas�l hissettirdi�ini bilmiyor",
        "Anna, m�zi�in nas�l bir ses oldu�unu bilmiyor",
        "Anna, denizin kokusunu hi� alamad�",
        "Anna, �i�eklerin renklerini hayal edemez",
        "Anna, s�cakl��� ve so�uklu�u hissedemez",
        "Anna, hi� bir elman�n tad�na bakmad�",
        "Anna, r�zgar�n tenine nas�l dokundu�unu bilmiyor",
        "Anna, parmaklar�n�n aras�ndan akan suyun hissini bilemez",
        "Anna, hi�bir kitab�n sayfalar�n� �evirmedi",
        "Anna, g�ne�in nas�l parlad���n� g�remez",
        "Anna, �imenlerin �zerinde y�r�rken nas�l hissettirdi�ini bilmiyor",
        "Anna, hi� tatl� bir yiyece�i koklayamad�",
        "Anna, k���n kar�n nas�l g�r�nd���n� bilmiyor",
        "Anna, yaz r�zgarlar�n�n s�cakl���n� hissedemez",
        "Anna, a�a�lar�n yapraklar�n�n h���rt�s�n� duyamaz",
        "Anna, renklerin canl�l���n� asla deneyimleyemez",
        "Anna, �ekerin tatl�l���n� bilmez",
        "Anna, tuzun dilindeki ac�ms� tad�n� hissedemez",
        "Anna, ate�in nas�l �s�nd���n� bilemez",
        "Anna, so�uk suyun cildinde nas�l hissettirdi�ini anlayamaz",
        "Anna, bir k�pe�in t�ylerinin yumu�akl���n� hissedemez",
        "Anna, ya�mur damlalar�n�n pencerede nas�l ses ��kard���n� duyamaz",
        "Anna, sabah kahvesinin kokusunu almaz",
        "Anna, g�ky�z�n�n mavili�ini g�remez",
        "Anna, hi� meyve toplamad�",
        "Anna, bir g�vercinin kanat ��rp���n� duyamaz",
        "Anna, �ikolatan�n tatl� lezzetini bilmez",
        "Anna, g�k g�r�lt�s�n�n g�c�n� i�itemez",
        "Anna, g�nbat�m�n� g�remez",
        "Anna, sabah�n serinli�ini hissetmez",
        "Anna, taze pi�mi� ekme�in kokusunu alamaz",
        "Anna, kar�n so�uklu�unu hissedemez",
        "Anna, yaz meyvelerinin tad�n� bilmez",
        "Anna, bir kedinin m�rlamas�n� duyamaz",
        "Anna, f�rt�nan�n nas�l u�ultu yapt���n� duyamaz",
        "Anna, okyanusun dalgalar�n� g�remez",
        "Anna, bir kitab�n kokusunu asla bilemez",
        "Anna, ormanda y�r�rken dallar�n ��t�rdamas�n� duyamaz",
        "Anna, hi� r�zgar�n sa�lar�n� nas�l savurdu�unu hissetmedi",
        "Anna, y�ld�zlar�n parlakl���n� g�remez",
        "Anna, �ikolatal� kekin nas�l koktu�unu bilmez",
        "Anna, g�kku�a��n�n renklerini g�remez",
        "Anna, sevdi�i birinin sesini duyamaz",
        "Anna, nehrin ak���n� izleyemez",
        "Anna, s�cak bir �ay�n tad�n� ��karamaz"
    };

    private int currentSentenceIndex = 0;

    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        while (true)
        {
            // 1. C�mlenin geri kalan�n� s�rayla yazma ("Anna," sabit kalacak)
            string currentSentence = sentences[currentSentenceIndex];
            string staticPart = "Anna,"; // Sabit kalan k�s�m
            string dynamicPart = currentSentence.Substring(staticPart.Length); // De�i�ecek k�s�m

            textMeshPro.text = staticPart;
            for (int i = 0; i < dynamicPart.Length; i++)
            {
                textMeshPro.text += dynamicPart[i];
                yield return new WaitForSeconds(typingSpeed);
            }

            // 2. "Loading" noktalar�n�n animasyonu (. .. ...)
            yield return StartCoroutine(AnimateDots(textMeshPro.text));

            // 3. Yaz�y� silme (sadece dinamik k�sm� sil)
            for (int i = dynamicPart.Length - 1; i >= 0; i--)
            {
                textMeshPro.text = staticPart + dynamicPart.Substring(0, i);
                yield return new WaitForSeconds(typingSpeed);
            }

            // 4. Bir sonraki c�mleye ge�
            currentSentenceIndex = (currentSentenceIndex + 1) % sentences.Length;
        }
    }

    IEnumerator AnimateDots(string baseText)
    {
        float elapsedTime = 0f;
        while (elapsedTime < dotEffectDuration)
        {
            // . ekle
            textMeshPro.text = baseText + ".";
            yield return new WaitForSeconds(dotAddingInterval);

            // .. ekle
            textMeshPro.text = baseText + "..";
            yield return new WaitForSeconds(dotAddingInterval);

            // ... ekle
            textMeshPro.text = baseText + "...";
            yield return new WaitForSeconds(dotAddingInterval);

            // Noktalar� sil
            textMeshPro.text = baseText;
            yield return new WaitForSeconds(dotAddingInterval);

            // S�reyi g�ncelle
            elapsedTime += dotAddingInterval * 4; // . .. ... ve silme i�in toplam s�re
        }
    }
}
