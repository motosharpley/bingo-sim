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
        private string GameName = "Redfire";
        private string EngineID = "Redfire_AlphaTest";
        private string IP_ADDRESS = "127.0.0.1";
        private int SubNumber = 9;

        private string OutBoundMsg;


        byte[] byteInboundBuffer = new Byte[256];
        byte[] byteOutboundBuffer;


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

        public TcpClient client = new TcpClient();
        

        public void Subscribe(TcpClient client)
        {

            try
            {
                // Connect to server
                client.Connect("127.0.0.1", 4779);

                string SubRequest = "|SUBSCRIBE|" + GameName + "|IP_ADDRESS|" + IP_ADDRESS + "|";
                OutBoundMsg = SubRequest;
                
                // Send Subcription message
                SendMessage(SubRequest, client);
                ReceiveMessage(client);
                
               // StreamReader reader = new StreamReader(client.GetStream());
                //String server_string = reader.ReadLine();
                //Console.WriteLine(server_string);

                //reader.Close();
                //writer.Close();
                //client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SendMessage(string msg, TcpClient client)
        {
            
            Byte[] data = Encoding.ASCII.GetBytes(msg);
            // TODO ERRR Handle No Connection 
            NetworkStream writer = client.GetStream();
            // writer.WriteLine(msg);
            writer.Write(data, 0 , data.Length);
            writer.Flush();
            //writer.Close();
        }

        public static void ReceiveMessage(TcpClient client)
        {
            Byte[] data = new Byte[1024];
            // String to store the response ASCII representation.
            String responseData = String.Empty;
            NetworkStream reader = client.GetStream();
            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = reader.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            string[] items = responseData.Split('|');
            foreach (var item in items)
            {
                Console.WriteLine($"{item}");
            }
            //reader.Close();
            reader.Flush();
        }


        private void Connect_btn_Click(object sender, EventArgs e)
        {
            //Connect to bingo server
            // Connect Message format |SUBSCRIBE|Redfire_92|IP_ADDRESS|10.7.3.87|
            // Response Message format |SUBSCRIBE|<SubNumber|ENGINE_ID|<EngineID>|
            // Build Subscribe Request String
            //string SubRequest = "|SUBSCRIBE|" + GameName + "|IP_ADDRESS|" + IP_ADDRESS + "|";
            //Instantiate client connection
            Subscribe(client);
            // Send subscription message
            //SendMessage(SubRequest, client);

            
            //Console.WriteLine(SubRequest);
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
            OutBoundMsg = SpinRequest;
            SendMessage(OutBoundMsg, client);
            //Console.WriteLine(SpinRequest);
            ReceiveMessage(client);
            
        }

        private void Preview_btn_Click(object sender, EventArgs e)
        {
            // Send Preview Card Request
            // Preview Card Request Message format |PREVIEW_CARD|<SubNumber>|ENGINE_ID|<EngineID>|
            // Response Message format |PREVIEW_CARD|<SubNumber>|ENGINE_ID|<EngineID>|GAME_ID|<GameID>|BASE_CARD|<BaseCardSpots>|

            // Build Preview Card Request String
            string PreviewRequest = "|PREVIEW_CARD|" + SubNumber + "|ENGINE_ID|" + EngineID + "|";
            OutBoundMsg = PreviewRequest;
            SendMessage(OutBoundMsg, client);
            //Console.WriteLine(PreviewRequest);

            ReceiveMessage(client);
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
            OutBoundMsg = PlayPreviewRequest;
            SendMessage(OutBoundMsg, client);
            //Console.WriteLine(PlayPreviewRequest);

            ReceiveMessage(client);
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
