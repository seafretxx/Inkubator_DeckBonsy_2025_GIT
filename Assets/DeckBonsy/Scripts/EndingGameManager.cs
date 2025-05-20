using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingSceneManager : MonoBehaviour
{
    public TextMeshProUGUI endingText;
    public TextMeshProUGUI tricepsText;
    public TextMeshProUGUI flintText;
    public TextMeshProUGUI fabiusText;
    public TextMeshProUGUI minervaText;

    void Start()
    {
        string ending = PlayerPrefs.GetString("ending", "neutral");

        switch (ending)
        {
            case "good":
                endingText.text = "My, niewolnicy, dotychczas zdominowani i poni�ani, postanowili�my walczy� o swoj� wolno��, ko�cz�c z systemem, kt�ry od lat nas uciska. Cho� s�absi liczebnie, zaskoczyli�my oprawc�w. D�wi�k wybuch�w i ogie� zamieni�y miasto w chaos, a stra�nicy, cho� przewy�szali nas liczebnie, nie byli przygotowani na tak szybki atak. Zyskali�my na sile, a zdesperowani �o�nierze wycofali si� na moment, by spr�bowa� ocali� swoje �ycie.  Mimo �e g��wna w�adza brutalnie t�umi powstanie, to jednak doprowadza ono do zmian. W miastach zaczyna powstawa� nowa rzeczywisto��. Cho� nie osi�gn�li�my ca�kowitej wolno�ci, zdobyli�my lepsze warunki �ycia, pojawiaj� si� pierwsze negocjacje dotycz�ce reformy systemu, zdobywamy prawo do g�osu. Wsp�lne protesty i manifestacje zacz�y by� dostrzegane przez rz�dy, kt�re powoli, cho� niech�tnie, musz� zareagowa� na narastaj�c� presj�. Powstanie, cho� nie zako�czy�o si� pe�nym zwyci�stwem, otworzy�o drzwi do nowych mo�liwo�ci i stawia pytanie o przysz�o�� systemu, kt�ry przez wieki trzyma� nas w niewoli. Szkoda, �e nie mo�esz tego zobaczy�, tato.";
                break;
            case "neutral":
                endingText.text = "Powstanie, mimo i� wybuch�o, nie mia�o ostatecznego prze�omu. By�o to zamieszanie, kt�re nic nie zmieni�o. Wybuchy i walki rozprzestrzeni�y si� po miastach, ale nie przynios�y decyduj�cego zwyci�stwa ani pora�ki. Przez chwil� mamy przewag�, ale nasze si�y s� zbyt s�abe, by cokolwiek zmieni�. Kapitol szybko odbiera nam kontrol�, a niewolnictwo trwa dalej. Wszyscy, kt�rzy prze�yli, musz� wr�ci� do swoich zada�. �adne zmiany nie nadchodz�. Stracili�my nadziej�, �e cokolwiek si� kiedykolwiek zmieni. Czy podj�am dobre decyzje? Czy jest szansa by walczy� dalej?";
                break;
            case "bad":
                endingText.text = "Rewolucja wybuch�a w spos�b, jakiego nikt si� nie spodziewa� � zbyt brutalnie, zbyt gwa�townie, bez zastanowienia nad konsekwencjami. Stra�nicy, widz�c nasz� nieust�pliwo��, szybko odpowiedzieli atakiem, a miasto zacz�o topnie� w chaosie. Cia�a poleg�ych niewolnik�w wype�nia�y ulice, a ich krzyki by�y nies�yszalne w obliczu ogromnej si�y wroga. Powstanie sta�o si� ju� tylko pr�b� ucieczki, a nie prawdziwego wyzwolenia. Za�lepieni gniewem i nadziej�, walczyli�my do ostatniego tchu, ale z ka�dym dniem liczba ofiar ros�a. Nasze marzenie o rewolucji przerodzi�o si� w koszmar, kt�ry nie ust�powa�. �adne ma�e zwyci�stwo nie przynios�o ulgi, a �wiat, kt�ry by� nasz� nadziej� na zmienienie losu, sta� si� jeszcze bardziej brutalny. Miasta, kt�re mia�y sta� si� symbolem wolno�ci, sta�y si� cmentarzyskami, a same w�adze jeszcze bardziej zdystansowa�y si� od niewolnik�w. Powstanie nie tylko nie przynios�o zamierzonych efekt�w, ale pog��bi�o przepa�� mi�dzy tymi, kt�rzy trzymali w�adz�, a tymi, kt�rzy nigdy nie mieli nadziei na lepsze �ycie.";
                break;
        }
        ShowNpcEnding("triceps", tricepsText);
        ShowNpcEnding("flint", flintText);
        ShowNpcEnding("fabius", fabiusText);
        ShowNpcEnding("minerva", minervaText);

    }
    private void ShowNpcEnding(string npcId, TextMeshProUGUI uiText)
    {
        string text = GameManager.gameManager.GetNpcEndingText(npcId);
        uiText.text = text;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
