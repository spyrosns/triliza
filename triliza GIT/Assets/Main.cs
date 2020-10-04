using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    Timer t = new Timer();
    private System.Random rnd = new System.Random();
    public GameObject[] butts;
    public GameObject ttimeL;
    public GameObject CoverButts;
    public GameObject linewin;
    public GameObject canvmenu;
    public Text ttime;
    public Text tscore;
    public Sprite Ximg;
    public Sprite Oimg;
    private int[] typedok = new int[9];//0: Keno, 1: Me, 2: AI
    private int remaintime = 60;
    private int timesplayed = 0;
    private int winner = 0;//0: Playing, 1: Draw, 2: AI, 3: Me
    private int posai = 0;  //Grafei thn epilegmeni thesi ap' to AI
    private int level;
    private int timetoscore = 10;
    private int score = 0;
    private bool[] synth = new bool[8];
    private bool onLine = false;
    private bool givebonus = false;
    private bool playerturn = true;// false: AI, true: Mine
    private bool xoturn = false;// false: X, true: O (MY TURN)

    // Start is called before the first frame update
    void Start()
    {
        t.Elapsed += new ElapsedEventHandler(OnTimer);
        t.Interval = 1000;
        t.Start();
        level = PlayerPrefs.GetInt("LevelSaved");
        SetRemainingTime();

        for (int i = 0; i < typedok.Length; i++)
            typedok[i] = 0;

    }

    // Update is called once per frame
    void Update()
    {
        tscore.text = "" + score;
        if (remaintime == 0)    //Otan teleiwsei o xronos o paikths xanei
        {
            t.Stop();
            winner = 2;
            playerturn = true;
            ttime.text = "You LOSE!";
            ttime.color = Color.red;
            ttime.fontStyle = FontStyle.Bold;
            ttimeL.SetActive(false);    //  Kryvei to keimeno "Time"
            CoverButts.SetActive(true); //Kalyptontai ta koumpia gia na mhn mporoun na patithoun afou exei teleiwsei to game
        }
        if (timesplayed >= 9 || winner != 0)
        {
            if ((winner == 1 || winner == 2) && score > 0)
                score--;
            if (score == 0)
                tscore.text = "0!";

            if (playerturn == false)
                GameOver(3);
            else
                GameOver(2);

            t.Stop();
            return;
        }
        // XRWMA & TEXT XRONOU
        if (remaintime <= 10)
            ttime.color = Color.red;
        else
            ttime.color = Color.white;
        if (playerturn)
            ttime.text = "" + remaintime;
        else
            ttime.text = "O's Turn";
    }

    void OnTimer(object source, ElapsedEventArgs e)
    {
        remaintime--;
        // Afksisi score analoga me xrono 
        if (timetoscore > 1)
            timetoscore--;
    }

    public void TypeXO(int id) //Seira mou
    {
        typedok[id] = 1;

        butts[id].GetComponent<Image>().color = Color.white;
        if (xoturn == false)                                    //Grafei "X" 'H "O" sto meros pou pathse o paikths
            butts[id].GetComponent<Image>().sprite = Ximg;
        else
            butts[id].GetComponent<Image>().sprite = Oimg;


        remaintime = 20;        //  Tyxaios arithmos megalytero tou 10 apla gia na mhn einai kokkino to keimeno
        score += timetoscore;   //  Ayksisi score analoga me to xrono
        timetoscore = 10;       //  Epanafora metavlitis gia to score

        ttimeL.SetActive(false);//  Kryvei to keimeno "Time"
        playerturn = false;     //  Allagi seiras
        CoverButts.SetActive(true); //Kalyptontai ta koumpia gia na mhn mporoun na patithoun afou einai seira toy AI
        timesplayed++;          //  Poses fores exoun paixtei synolika
        GameOver(3);            //  Elegxos gia telos paixnidiou

        if (timesplayed < 9 && winner == 0)
            Invoke("ChangeTurnAI", 1f);
    }
    
    private void ChangeTurnAI() //Seira AI
    {
        posai = 0;
        if (level == 0)
            posai = AIeasy();
        else if (level == 1)
            posai = AImid();
        else
            posai = AIdiff();

        typedok[posai] = 2;

        if (xoturn == false)
            butts[posai].GetComponent<Image>().sprite = Oimg;
        else
            butts[posai].GetComponent<Image>().sprite = Ximg;

        butts[posai].GetComponent<Image>().color = Color.white;
        butts[posai].GetComponent<Button>().interactable = false;


        SetRemainingTime();

        ttimeL.SetActive(true);
        playerturn = true;
        CoverButts.SetActive(false);
        timesplayed++;
        GameOver(2);            //  Elegxos gia telos paixnidiou
    }

    private void SetRemainingTime()
    {
        if (level == 0)
            remaintime = 50;
        else if (level == 1)
            remaintime = 35;
        else
            remaintime = 20;
    }
    // Levels
    private int AIeasy()
    {
        int i = 0;
        do
        {
            i = rnd.Next(0, 9);
        } while (typedok[i] == 1 || typedok[i] == 2);
        return i;
    }
    private int AImid()
    {
        return FindBestPos(2);  //Elegxei pou mporei na kanei triada to AI
    }
    private int AIdiff()
    {
        onLine = false;
        int k = FindBestPos(2); //Elegxei pou mporei na kanei triada to AI
        if (onLine == true)
        {
            k = FindBestPos(1); //Elegxei pou mporei na empodisei triada mou to AI
        }
        return k;
    }
    //Find best position to play
    private int FindBestPos(int turn)
    {
        int i = 0;
        // ORIZONTIA
        if (typedok[0] == 0 && typedok[1] == turn && typedok[2] == turn)
            i = 0;
        else if (typedok[0] == turn && typedok[1] == 0 && typedok[2] == turn)
            i = 1;
        else if (typedok[0] == turn && typedok[1] == turn && typedok[2] == 0)
            i = 2;
        else if (typedok[3] == 0 && typedok[4] == turn && typedok[5] == turn)
            i = 3;
        else if (typedok[3] == turn && typedok[4] == 0 && typedok[5] == turn)
            i = 4;
        else if (typedok[3] == turn && typedok[4] == turn && typedok[5] == 0)
            i = 5;
        else if (typedok[6] == 0 && typedok[7] == turn && typedok[8] == turn)
            i = 6;
        else if (typedok[6] == turn && typedok[7] == 0 && typedok[8] == turn)
            i = 7;
        else if (typedok[6] == turn && typedok[7] == turn && typedok[8] == 0)
            i = 8;
        // KATHETA
        else if (typedok[0] == 0 && typedok[3] == turn && typedok[6] == turn)
            i = 0;
        else if (typedok[0] == turn && typedok[3] == 0 && typedok[6] == turn)
            i = 3;
        else if (typedok[0] == turn && typedok[3] == turn && typedok[6] == 0)
            i = 6;
        else if (typedok[1] == 0 && typedok[4] == turn && typedok[7] == turn)
            i = 1;
        else if (typedok[1] == turn && typedok[4] == 0 && typedok[7] == turn)
            i = 4;
        else if (typedok[1] == turn && typedok[4] == turn && typedok[7] == 0)
            i = 7;
        else if (typedok[2] == 0 && typedok[5] == turn && typedok[8] == turn)
            i = 2;
        else if (typedok[2] == turn && typedok[5] == 0 && typedok[8] == turn)
            i = 5;
        else if (typedok[2] == turn && typedok[5] == turn && typedok[8] == 0)
            i = 8;
        // DIAGWNIA
        else if (typedok[0] == 0 && typedok[4] == turn && typedok[8] == turn)
            i = 0;
        else if (typedok[0] == turn && typedok[4] == 0 && typedok[8] == turn)
            i = 4;
        else if (typedok[0] == turn && typedok[4] == turn && typedok[8] == 0)
            i = 8;
        else if (typedok[2] == 0 && typedok[4] == turn && typedok[6] == turn)
            i = 2;
        else if (typedok[2] == turn && typedok[4] == 0 && typedok[6] == turn)
            i = 4;
        else if (typedok[2] == turn && typedok[4] == turn && typedok[6] == 0)
            i = 6;
        else //DEN EXEI FTEIAXTEI KAMIA DIADA, ara stin epomeni kinisi de tha ginei kamia triada
        {
            onLine = true;  //Paizei rolo sto Pro level "AIdiff"
            i = AIeasy();
        }
        return i;
    }

    private void GameOver(int id)
    {
        synth[0] = typedok[0] == typedok[1] && typedok[0] == typedok[2] && typedok[0] != 0; // Synthikes Nikhs
        synth[1] = typedok[3] == typedok[4] && typedok[3] == typedok[5] && typedok[3] != 0;
        synth[2] = typedok[6] == typedok[7] && typedok[6] == typedok[8] && typedok[6] != 0;
        
        synth[3] = typedok[0] == typedok[4] && typedok[0] == typedok[8] && typedok[0] != 0;
        synth[4] = typedok[2] == typedok[4] && typedok[2] == typedok[6] && typedok[2] != 0;

        synth[5] = typedok[0] == typedok[3] && typedok[0] == typedok[6] && typedok[0] != 0;
        synth[6] = typedok[1] == typedok[4] && typedok[1] == typedok[7] && typedok[1] != 0;
        synth[7] = typedok[2] == typedok[5] && typedok[2] == typedok[8] && typedok[2] != 0;

        

        if (synth[0] || synth[1] || synth[2] || synth[3] || synth[4] || synth[5] || synth[6] || synth[7]) 
        {
            Debug.Log("WIIIN");
            winner = id;
            if (winner == 3 && givebonus == false)
            {
                score += 15;        //Dinei bonus score otan kerdisw
                ttime.text = "You WIN!";
                ttime.color = Color.green;
                givebonus = true;   //Bool metavliti gia na dwsei mono MIA fora bonus score
                
                if (score > PlayerPrefs.GetInt("ScoreSaved"))
                    PlayerPrefs.SetInt("ScoreSaved", score);   // Save HIGHSCORE
            }
            else if (winner == 2)
            {
                ttime.text = "You LOSE!";
                ttime.color = Color.red;
            }
            ttime.fontStyle = FontStyle.Bold;
            ttimeL.SetActive(false);
            DrawLineWin();  //Sxediazei grammi sthn triada pou fteiaxtike
        }
        else if (timesplayed >= 9)  //An ginoun 9 kiniseis (gemisei to tamplo) kai den exei kerdisei kaneis, bgainei isopalia
        {
            winner = 1;
            ttime.text = "DRAW!";
            ttime.color = Color.blue;
            ttime.fontStyle = FontStyle.Bold;
        }
    }

    private void DrawLineWin()
    {
        linewin.SetActive(true);
        if (synth[0])
        {
            linewin.transform.localPosition = new Vector3(0, 340, 0);
            linewin.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (synth[1])
        {
            linewin.transform.localPosition = new Vector3(0, 0, 0);
            linewin.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (synth[2])
        {
            linewin.transform.localPosition = new Vector3(0, -340, 0);
            linewin.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (synth[3])
        {
            linewin.transform.localPosition = new Vector3(0, 0, 0);
            linewin.transform.localEulerAngles = new Vector3(0, 0, -45);
        }
        else if (synth[4])
        {
            linewin.transform.localPosition = new Vector3(0, 0, 0);
            linewin.transform.localEulerAngles = new Vector3(0, 0, 45);
        }
        else if (synth[5])
        {
            linewin.transform.localPosition = new Vector3(-340, 0, 0);
            linewin.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (synth[6])
        {
            linewin.transform.localPosition = new Vector3(0, 0, 0);
            linewin.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (synth[7])
        {
            linewin.transform.localPosition = new Vector3(340, 0, 0);
            linewin.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        if (winner == 2)
            linewin.GetComponent<Image>().color = Color.red;
        else if (winner == 3)
            linewin.GetComponent<Image>().color = Color.green;
        CoverButts.SetActive(true);
    }

    public void GoToHome()
    {
        SceneManager.LoadScene(0); // Load Menu Scene
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1); // Load Play Scene
    }
}
