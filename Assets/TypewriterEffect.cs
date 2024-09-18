using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float typingSpeed = 0.05f;
    public float dotAddingInterval = 0.5f;
    public float dotEffectDuration = 3.0f; // Kaç saniye boyunca . .. ... efekti olacak

    private string[] sentences = {
        "Anna, seni göremez",
        "Anna, seni duyamaz",
        "Anna, dondurmanýn tadýný bilmiyor",
        "Anna, hiç kuþ görmedi",
        "Anna, yaðmurdan sonra topraðý koklayamaz",
        "Anna, peluþ oyuncaklarýnýn nasýl hissettirdiðini bilmiyor",
        "Anna, müziðin nasýl bir ses olduðunu bilmiyor",
        "Anna, denizin kokusunu hiç alamadý",
        "Anna, çiçeklerin renklerini hayal edemez",
        "Anna, sýcaklýðý ve soðukluðu hissedemez",
        "Anna, hiç bir elmanýn tadýna bakmadý",
        "Anna, rüzgarýn tenine nasýl dokunduðunu bilmiyor",
        "Anna, parmaklarýnýn arasýndan akan suyun hissini bilemez",
        "Anna, hiçbir kitabýn sayfalarýný çevirmedi",
        "Anna, güneþin nasýl parladýðýný göremez",
        "Anna, çimenlerin üzerinde yürürken nasýl hissettirdiðini bilmiyor",
        "Anna, hiç tatlý bir yiyeceði koklayamadý",
        "Anna, kýþýn karýn nasýl göründüðünü bilmiyor",
        "Anna, yaz rüzgarlarýnýn sýcaklýðýný hissedemez",
        "Anna, aðaçlarýn yapraklarýnýn hýþýrtýsýný duyamaz",
        "Anna, renklerin canlýlýðýný asla deneyimleyemez",
        "Anna, þekerin tatlýlýðýný bilmez",
        "Anna, tuzun dilindeki acýmsý tadýný hissedemez",
        "Anna, ateþin nasýl ýsýndýðýný bilemez",
        "Anna, soðuk suyun cildinde nasýl hissettirdiðini anlayamaz",
        "Anna, bir köpeðin tüylerinin yumuþaklýðýný hissedemez",
        "Anna, yaðmur damlalarýnýn pencerede nasýl ses çýkardýðýný duyamaz",
        "Anna, sabah kahvesinin kokusunu almaz",
        "Anna, gökyüzünün maviliðini göremez",
        "Anna, hiç meyve toplamadý",
        "Anna, bir güvercinin kanat çýrpýþýný duyamaz",
        "Anna, çikolatanýn tatlý lezzetini bilmez",
        "Anna, gök gürültüsünün gücünü iþitemez",
        "Anna, günbatýmýný göremez",
        "Anna, sabahýn serinliðini hissetmez",
        "Anna, taze piþmiþ ekmeðin kokusunu alamaz",
        "Anna, karýn soðukluðunu hissedemez",
        "Anna, yaz meyvelerinin tadýný bilmez",
        "Anna, bir kedinin mýrlamasýný duyamaz",
        "Anna, fýrtýnanýn nasýl uðultu yaptýðýný duyamaz",
        "Anna, okyanusun dalgalarýný göremez",
        "Anna, bir kitabýn kokusunu asla bilemez",
        "Anna, ormanda yürürken dallarýn çýtýrdamasýný duyamaz",
        "Anna, hiç rüzgarýn saçlarýný nasýl savurduðunu hissetmedi",
        "Anna, yýldýzlarýn parlaklýðýný göremez",
        "Anna, çikolatalý kekin nasýl koktuðunu bilmez",
        "Anna, gökkuþaðýnýn renklerini göremez",
        "Anna, sevdiði birinin sesini duyamaz",
        "Anna, nehrin akýþýný izleyemez",
        "Anna, sýcak bir çayýn tadýný çýkaramaz"
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
            // 1. Cümlenin geri kalanýný sýrayla yazma ("Anna," sabit kalacak)
            string currentSentence = sentences[currentSentenceIndex];
            string staticPart = "Anna,"; // Sabit kalan kýsým
            string dynamicPart = currentSentence.Substring(staticPart.Length); // Deðiþecek kýsým

            textMeshPro.text = staticPart;
            for (int i = 0; i < dynamicPart.Length; i++)
            {
                textMeshPro.text += dynamicPart[i];
                yield return new WaitForSeconds(typingSpeed);
            }

            // 2. "Loading" noktalarýnýn animasyonu (. .. ...)
            yield return StartCoroutine(AnimateDots(textMeshPro.text));

            // 3. Yazýyý silme (sadece dinamik kýsmý sil)
            for (int i = dynamicPart.Length - 1; i >= 0; i--)
            {
                textMeshPro.text = staticPart + dynamicPart.Substring(0, i);
                yield return new WaitForSeconds(typingSpeed);
            }

            // 4. Bir sonraki cümleye geç
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

            // Noktalarý sil
            textMeshPro.text = baseText;
            yield return new WaitForSeconds(dotAddingInterval);

            // Süreyi güncelle
            elapsedTime += dotAddingInterval * 4; // . .. ... ve silme için toplam süre
        }
    }
}
