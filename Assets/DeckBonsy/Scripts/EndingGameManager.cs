using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;


public class EndingSceneManager : MonoBehaviour
{
    public TextMeshProUGUI endingText;
    public ScrollRect scrollRect;
    [TextArea(10, 20)] public string fullText; // pe�ny tekst ko�cowy (g��wna + npc)

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
                fullText = "ZAKO�CZENIE DOBRE\nMy, niewolnicy, dotychczas zdominowani i poni�ani, postanowili�my walczy� o swoj� wolno��, ko�cz�c z systemem, kt�ry od lat nas uciska. Cho� s�absi liczebnie, zaskoczyli�my oprawc�w. D�wi�k wybuch�w i ogie� zamieni�y miasto w chaos, a stra�nicy, cho� przewy�szali nas liczebnie, nie byli przygotowani na tak szybki atak. Zyskali�my na sile, a zdesperowani �o�nierze wycofali si� na moment, by spr�bowa� ocali� swoje �ycie.  Mimo �e g��wna w�adza brutalnie t�umi powstanie, to jednak doprowadza ono do zmian. W miastach zaczyna powstawa� nowa rzeczywisto��. Cho� nie osi�gn�li�my ca�kowitej wolno�ci, zdobyli�my lepsze warunki �ycia, pojawiaj� si� pierwsze negocjacje dotycz�ce reformy systemu, zdobywamy prawo do g�osu. Wsp�lne protesty i manifestacje zacz�y by� dostrzegane przez rz�dy, kt�re powoli, cho� niech�tnie, musz� zareagowa� na narastaj�c� presj�. Ta zakapturzona posta�, kt�ra to wszystko zacz�a okaza�a si� Senatorem Kaeso. To jedyny senator, kt�ry chce pom�c niewolnikom. To on pozna� mnie ze wszystkimi i wspiera� nas jak m�g� z g�ry. Odegra� kluczow� rol� w przekonaniu rz�du do rozpocz�cia proces�w legislacyjnych, kt�re prowadz� do uznania praw niewolnik�w. Po�rednio jego wp�yw w administracji umo�liwia wprowadzenie pierwszych ustaw, kt�re zaczynaj� zmienia� status niewolnik�w w systemie, otwieraj�c drog� do dalszych reform. Cho� zmiany te s� tylko pocz�tkiem, to stanowi� realny krok ku lepszemu �wiatu. Powstanie, cho� nie zako�czy�o si� pe�nym zwyci�stwem, otworzy�o drzwi do nowych mo�liwo�ci i stawia pytanie o przysz�o�� systemu, kt�ry przez wieki trzyma� nas w niewoli. Szkoda, �e nie mo�esz tego zobaczy�, tato.";
                break;
            case "neutral":
                fullText = "ZAKO�CZENIE NEUTRALNE\nPowstanie, mimo i� wybuch�o, nie mia�o ostatecznego prze�omu. By�o to zamieszanie, kt�re nic nie zmieni�o. Wybuchy i walki rozprzestrzeni�y si� po miastach, ale nie przynios�y decyduj�cego zwyci�stwa ani pora�ki. Przez chwil� mamy przewag�, ale nasze si�y s� zbyt s�abe, by cokolwiek zmieni�. Kapitol szybko odbiera nam kontrol�, a niewolnictwo trwa dalej.Ta zakapturzona posta�, kt�ra to wszystko zacz�a okaza�a si� Senatorem Kaeso. To jedyny senator, kt�ry chcia� pom�c niewolnikom. To on pozna� mnie ze wszystkimi i wspiera� nas jak m�g� z g�ry. W obawie o swoj� pozycj� polityczn� nie wtr�ca si� ju� w sprawy niewolnik�w, zupe�nie si� od nas odci��. Wszyscy, kt�rzy prze�yli, musz� wr�ci� do swoich zada�. �adne zmiany nie nadchodz�. Stracili�my nadziej�, �e cokolwiek si� kiedykolwiek zmieni. Czy podj�am dobre decyzje? Czy jest szansa by walczy� dalej?";
                break;
            case "bad":
                fullText = "ZAKO�CZENIE Z�E\nRewolucja wybuch�a w spos�b, jakiego nikt si� nie spodziewa� � zbyt brutalnie, zbyt gwa�townie, bez zastanowienia nad konsekwencjami. Stra�nicy, widz�c nasz� nieust�pliwo��, szybko odpowiedzieli atakiem, a miasto zacz�o topnie� w chaosie. Cia�a poleg�ych niewolnik�w wype�nia�y ulice, a ich krzyki by�y nies�yszalne w obliczu ogromnej si�y wroga. Powstanie sta�o si� ju� tylko pr�b� ucieczki, a nie prawdziwego wyzwolenia. Za�lepieni gniewem i nadziej�, walczyli�my do ostatniego tchu, ale z ka�dym dniem liczba ofiar ros�a. Nasze marzenie o rewolucji przerodzi�o si� w koszmar, kt�ry nie ust�powa�. �adne ma�e zwyci�stwo nie przynios�o ulgi, a �wiat, kt�ry by� nasz� nadziej� na zmienienie losu, sta� si� jeszcze bardziej brutalny. Miasta, kt�re mia�y sta� si� symbolem wolno�ci, sta�y si� cmentarzyskami, a same w�adze jeszcze bardziej zdystansowa�y si� od niewolnik�w. Ta zakapturzona posta�, kt�ra to wszystko zacz�a okaza�a si� Senatorem Kaeso. To jedyny senator, kt�ry chcia� pom�c niewolnikom. To on pozna� mnie ze wszystkimi i wspiera� nas jak m�g� z g�ry.  Po naszej pora�ce ucieka i ukrywa si� na innej planecie. W�adze chyba dowiedzia�y si� o jego udziale w powstaniu i jest poszukiwany. Powstanie nie tylko nie przynios�o zamierzonych efekt�w, ale pog��bi�o przepa�� mi�dzy tymi, kt�rzy trzymali w�adz�, a tymi, kt�rzy nigdy nie mieli nadziei na lepsze �ycie. Bunt zostaje rozbity, a w�adze zaostrzaj� kontrol� nad niewolnikami. Wi�kszo�� powsta�c�w ginie, a ci, kt�rzy prze�yli, s� poddawani torturom i niewolnictwu w jeszcze gorszych warunkach. Kapitol og�asza, �e niewolnicy s� barbarzy�cami, co prowadzi do jeszcze wi�kszej dyskryminacji. Co my�my zrobili? Czy da si� jeszcze wydosta� z tego koszmaru?";
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
            scrollRect.verticalNormalizedPosition = 0f; // autoscroll na d�
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
