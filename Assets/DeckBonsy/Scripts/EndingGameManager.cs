using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;


public class EndingSceneManager : MonoBehaviour
{
    public TextMeshProUGUI endingText;
    public ScrollRect scrollRect;
    [TextArea(10, 20)] public string fullText; // pe³ny tekst koñcowy (g³ówna + npc)

    public float typingSpeed = 0.02f;

    private void Start()
    {
        PrepareFullEnding();
        StartCoroutine(TypeText());
    }

    private void PrepareFullEnding()
    {
        string ending = PlayerPrefs.GetString("ending", "neutral");

        string triceps = GameManager.gameManager.GetNpcEndingText("triceps");
        string flint = GameManager.gameManager.GetNpcEndingText("flint");
        string fabius = GameManager.gameManager.GetNpcEndingText("fabius");
        string minerva = GameManager.gameManager.GetNpcEndingText("minerva");

        string npcEndings = $"\n\n<b><color=#00FFFF>Triceps:</color></b> {triceps}\n" +
                    $"<b><color=#00FFFF>Flint:</color></b> {flint}\n" +
                    $"<b><color=#00FFFF>Fabius:</color></b> {fabius}\n" +
                    $"<b><color=#00FFFF>Minerva:</color></b> {minerva}";

        switch (ending)
        {
            case "good":
                fullText = "ZAKOÑCZENIE DOBRE\nMy, niewolnicy, dotychczas zdominowani i poni¿ani, postanowiliœmy walczyæ o swoj¹ wolnoœæ, koñcz¹c z systemem, który od lat nas uciska. Choæ s³absi liczebnie, zaskoczyliœmy oprawców. DŸwiêk wybuchów i ogieñ zamieni³y miasto w chaos, a stra¿nicy, choæ przewy¿szali nas liczebnie, nie byli przygotowani na tak szybki atak. Zyskaliœmy na sile, a zdesperowani ¿o³nierze wycofali siê na moment, by spróbowaæ ocaliæ swoje ¿ycie.  Mimo ¿e g³ówna w³adza brutalnie t³umi powstanie, to jednak doprowadza ono do zmian. W miastach zaczyna powstawaæ nowa rzeczywistoœæ. Choæ nie osi¹gnêliœmy ca³kowitej wolnoœci, zdobyliœmy lepsze warunki ¿ycia, pojawiaj¹ siê pierwsze negocjacje dotycz¹ce reformy systemu, zdobywamy prawo do g³osu. Wspólne protesty i manifestacje zaczê³y byæ dostrzegane przez rz¹dy, które powoli, choæ niechêtnie, musz¹ zareagowaæ na narastaj¹c¹ presjê. Powstanie, choæ nie zakoñczy³o siê pe³nym zwyciêstwem, otworzy³o drzwi do nowych mo¿liwoœci i stawia pytanie o przysz³oœæ systemu, który przez wieki trzyma³ nas w niewoli. Szkoda, ¿e nie mo¿esz tego zobaczyæ, tato.";
                break;
            case "neutral":
                fullText = "ZAKOÑCZENIE NEUTRALNE\nPowstanie, mimo i¿ wybuch³o, nie mia³o ostatecznego prze³omu. By³o to zamieszanie, które nic nie zmieni³o. Wybuchy i walki rozprzestrzeni³y siê po miastach, ale nie przynios³y decyduj¹cego zwyciêstwa ani pora¿ki. Przez chwilê mamy przewagê, ale nasze si³y s¹ zbyt s³abe, by cokolwiek zmieniæ. Kapitol szybko odbiera nam kontrolê, a niewolnictwo trwa dalej. Wszyscy, którzy prze¿yli, musz¹ wróciæ do swoich zadañ. ¯adne zmiany nie nadchodz¹. Straciliœmy nadziejê, ¿e cokolwiek siê kiedykolwiek zmieni. Czy podjê³am dobre decyzje? Czy jest szansa by walczyæ dalej?";
                break;
            case "bad":
                fullText = "ZAKOÑCZENIE Z£E\nRewolucja wybuch³a w sposób, jakiego nikt siê nie spodziewa³ – zbyt brutalnie, zbyt gwa³townie, bez zastanowienia nad konsekwencjami. Stra¿nicy, widz¹c nasz¹ nieustêpliwoœæ, szybko odpowiedzieli atakiem, a miasto zaczê³o topnieæ w chaosie. Cia³a poleg³ych niewolników wype³nia³y ulice, a ich krzyki by³y nies³yszalne w obliczu ogromnej si³y wroga. Powstanie sta³o siê ju¿ tylko prób¹ ucieczki, a nie prawdziwego wyzwolenia. Zaœlepieni gniewem i nadziej¹, walczyliœmy do ostatniego tchu, ale z ka¿dym dniem liczba ofiar ros³a. Nasze marzenie o rewolucji przerodzi³o siê w koszmar, który nie ustêpowa³. ¯adne ma³e zwyciêstwo nie przynios³o ulgi, a œwiat, który by³ nasz¹ nadziej¹ na zmienienie losu, sta³ siê jeszcze bardziej brutalny. Miasta, które mia³y staæ siê symbolem wolnoœci, sta³y siê cmentarzyskami, a same w³adze jeszcze bardziej zdystansowa³y siê od niewolników. Powstanie nie tylko nie przynios³o zamierzonych efektów, ale pog³êbi³o przepaœæ miêdzy tymi, którzy trzymali w³adzê, a tymi, którzy nigdy nie mieli nadziei na lepsze ¿ycie.";
                break;
        }
        fullText += "\n\n<b><color=#00FFFF>Triceps:</color></b> " + triceps;
        fullText += "\n<b><color=#00FFFF>Flint:</color></b> " + flint;  //cyjan
        fullText += "\n<b><color=#00FFFF>Fabius:</color></b> " + fabius;
        fullText += "\n<b><color=#00FFFF>Minerva:</color></b> " + minerva;


    }

    private IEnumerator TypeText()
    {
        endingText.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            endingText.text += fullText[i];
            yield return new WaitForSeconds(typingSpeed);

            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f; // autoscroll na dó³
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
