using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace bingo_sim
{
    public partial class BingoViewer : Form
    {
        public BingoViewer()
        {
            InitializeComponent();
            AddBingoNumToCard();
            AddBonusNumToCard();
        }

        // Spin Event Variables
        private int SPIN_EVENT;
        private string ENGINE_ID;
        private string GAME_ID;
        private int CREDITS_BET;
        private int BET_LEVEL;
        private int[] BALL_DRAW;
        private int[] BASE_CARD;
        private int BASE_DAUB;
        private int COVER_DAUB;
        private int BONUS_TYPE;
        private int[] BONUS_CARD;
        private int BONUS_DAUB;
        private int BASE_WIN;
        private int COVER_WIN;
        private int BONUS_WIN;
        private int BASE_NET;
        private int BONUS_NET;
        private int TOTAL_NET;

        // Subscription Event Variables
        private string EngineID = "Redfire_92";
        private string IP_ADDRESS = "127.0.0.1";
        private int SubNumber = 1;



        // Dummy Data Bingo Card Numbers
        int[] bingoNums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };

        private void AddBingoNumToCard()
        {
            int i = 0;
            foreach (Control control in BingoCard.Controls)
            {
                Label BingoCardLabel = control as Label;
                if (BingoCardLabel != null)
                {
                    int bingonum = bingoNums[i];
                    BingoCardLabel.Text = bingonum.ToString();
                    i++;
                }
            }
        }

        private void AddBonusNumToCard()
        {
            int i = 0;
            foreach (Control control in BonusCard.Controls)
            {
                Label BingoCardLabel = control as Label;
                if (BingoCardLabel != null)
                {
                    int bingonum = bingoNums[i];
                    BingoCardLabel.Text = bingonum.ToString();
                    i++;
                }
            }
        }

        public static void Subscribe()
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", port: 3000);
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                String s = "Hello Bingo";
                writer.WriteLine(s);
                writer.Flush();
                String server_string = reader.ReadLine();
                Console.WriteLine(server_string);

                //reader.Close();
                //writer.Close();
                //client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private void Connect_btn_Click(object sender, EventArgs e)
        {
            //Connect to bingo server
            // Connect Message format |SUBSCRIBE|Redfire_92|IP_ADDRESS|10.7.3.87|
            // Response Message format |SUBSCRIBE|<SubNumber|ENGINE_ID|<EngineID>|
            Subscribe();

            // Build Subscribe Request String
            string SubRequest = "|SUBSCRIBE|" + EngineID + "|IP_ADDRESS|" + IP_ADDRESS + "|";
            Console.WriteLine(SubRequest);
        }

        private void Spin_btn_Click(object sender, EventArgs e)
        {
            // Send Spin Request
            // New Spin Request Message format |NEW_SPIN|<SubNumber>|ENGINE_ID|<EngineID>|CREDITS_BET|<CreditQuantity>|BET_LEVEL|<Multiplier>|
            // Spin Response Message format |SPIN_EVENT|<SubNumber>|ENGINE_ID|<EngineID>|GAME_ID|<GameID>|CREDITS_BET|<CreditQuantity>|BET_LEVEL|<Multiplier>|BALL_DRAW|
            //  <BallDraw>|BASE_CARD|<BaseCardSpots>|BASE_DAUB|<BaseDaubs>|COVER_DAUB|<CoverDaubs>|BONUS_TYPE|<BonusID>|BONUS_CARD|<BonusCardSpots>|BONUS_DAUB|<BonusDaubs>|
            //  BASE_WIN|<BaseWinValue>|COVER_WIN|<CoverWinValue>|BONUS_WIN|<BonusWinValue>|BASE_NET|<BaseCreditsNet>|BONUS_NET|<BonusCreditsNet>|TOTAL_NET|<TotalCreditsNet>|

            // Build Spin Request String
            string SpinRequest = "|NEW_SPIN|" + SubNumber + "|ENGINE_ID|" + EngineID + "|CREDITS_BET|" + CREDITS_BET + "|BET_LEVEL|" + BET_LEVEL + "|";
            Console.WriteLine(SpinRequest);
        }

        private void Preview_btn_Click(object sender, EventArgs e)
        {
            // Send Preview Card Request
            // Preview Card Request Message format |PREVIEW_CARD|<SubNumber>|ENGINE_ID|<EngineID>|
            // Response Message format |PREVIEW_CARD|<SubNumber>|ENGINE_ID|<EngineID>|GAME_ID|<GameID>|BASE_CARD|<BaseCardSpots>|

            // Build Preview Card Request String
            string PreviewRequest = "|PREVIEW_CARD|" + SubNumber + "|ENGINE_ID|" + EngineID + "|";
            Console.WriteLine(PreviewRequest);
        }

        private void Play_preview_btn_Click(object sender, EventArgs e)
        {
            // Play Preview Card 
            // Play Preview Card Request Message format |PLAY_PREVIEW|<SubNumber>|ENGINE_ID|<EngineID>|CREDITS_BET|<CreditQuantity>|BET_LEVEL|<Multiplier>|
            // Play Preview Response Message format |SPIN_EVENT|<SubNumber>|ENGINE_ID|<EngineID>|GAME_ID|<GameID>|CREDITS_BET|<CreditQuantity>|BET_LEVEL|<Multiplier>|BALL_DRAW|
            //  <BallDraw>|BASE_CARD|<BaseCardSpots>|BASE_DAUB|<BaseDaubs>|COVER_DAUB|<CoverDaubs>|BONUS_TYPE|<BonusID>|BONUS_CARD|<BonusCardSpots>|BONUS_DAUB|<BonusDaubs>|
            //  BASE_WIN|<BaseWinValue>|COVER_WIN|<CoverWinValue>|BONUS_WIN|<BonusWinValue>|BASE_NET|<BaseCreditsNet>|BONUS_NET|<BonusCreditsNet>|TOTAL_NET|<TotalCreditsNet>|

            // Build Play Preview Request String
            string PlayPreviewRequest = "|PLAY_PREVIEW|" + SubNumber + "|ENGINE_ID|" + EngineID + "|CREDITS_BET|" + CREDITS_BET + "|BET_LEVEL|" + BET_LEVEL + "|";
            Console.WriteLine(PlayPreviewRequest);
        }

        private void Interim_Daub_btn_CheckedChanged(object sender, EventArgs e)
        {
            // Display Interim Card Daub
            // Interim Daub Data BASE_DAUB
        }

        private void Coverall_Daub_btn_CheckedChanged(object sender, EventArgs e)
        {
            // Display Coverall Card Daub
            // Coverall Daub Data COVER_DAUB
        }

        private void Messages_out_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Display outgoing message
            // Outgoing messages SUBSCRIBE, NEW_SPIN, PREVIEW_CARD, PLAY_PREVIEW
        }

        private void Messages_in_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Display incoming message
            // Incoming messages SUBSCRIBE, SPIN_EVENT, PREVIEW_CARD, (PLAY_PREVIEW)SPIN_EVENT
        }

        private void BetLevel_ValueChanged(object sender, EventArgs e)
        {
            int bet = Convert.ToInt32(BetLevel.Value);
            BET_LEVEL = bet;
            Console.WriteLine(BET_LEVEL);
        }

        private void CreditsBet_ValueChanged(object sender, EventArgs e)
        {
            int credit = Convert.ToInt32(CreditsBet.Value);
            CREDITS_BET = credit;
            Console.WriteLine(CreditsBet.Value);
        }
    }


}
