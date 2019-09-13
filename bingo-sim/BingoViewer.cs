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
            
                                              
        }

        // Spin Event Variables
        private int SPIN_EVENT;
        private string ENGINE_ID;
        private string GAME_ID;
        private int CREDITS_BET;
        private int BET_LEVEL;
        private int[] BALL_DRAW;
        private int[] BASE_CARD;
        private string[] BASE_DAUB;
        private string[] COVER_DAUB;
        private int BONUS_TYPE;
        private int[] BONUS_CARD;
        private string[] BONUS_DAUB;
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

        // TODO Implement into message handlers
        byte[] byteInboundBuffer = new Byte[256];
        byte[] byteOutboundBuffer;


        // Dummy Data Bingo Card Numbers
        int[] cardSpotNums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };



        private void AddBingoNumToCard()
        {
            int i = 0;
            foreach (Control control in BingoCard.Controls)
            {
                Label BingoCardLabel = control as Label;
                if (BingoCardLabel != null)
                {
                    int bingonum = BASE_CARD[i];
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
                    int bingonum = BONUS_CARD[i];
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

        public void ReceiveMessage(TcpClient client)
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
                int index = Array.IndexOf(items, item);
                //var field = item;
                //Console.WriteLine($"field:{field}");
                string ItemVal = items[index + 1];
                switch (item)
                {
                    case "SPIN_EVENT":                        
                        //Console.WriteLine(ItemVal);
                        SPIN_EVENT = Int32.Parse(ItemVal);
                        //Console.WriteLine("index of:" + $"{item} " + index);
                        //Console.WriteLine($"{item}");
                        break;
                    case "ENGINE_ID":                        
                        //Console.WriteLine(ItemVal);
                        ENGINE_ID = ItemVal;
                        break;
                    case "GAME_ID":
                        //Console.WriteLine(ItemVal);
                        GAME_ID = ItemVal;
                        break;
                    case "CREDITS_BET":
                        //Console.WriteLine(ItemVal);
                        CREDITS_BET = Int32.Parse(ItemVal);
                        break;
                    case "BET_LEVEL":
                        //Console.WriteLine(ItemVal);
                        BET_LEVEL = Int32.Parse(ItemVal);
                        break;
                    case "BALL_DRAW":
                        //Console.WriteLine(ItemVal.GetType());
                        string[] ballArray = ItemVal.Split(',');
                        BALL_DRAW = Array.ConvertAll(ballArray, int.Parse);
                        break;
                    case "BASE_CARD":
                        //Console.WriteLine(ItemVal.GetType());
                        string[] basecard = ItemVal.Split(',');
                        BASE_CARD = Array.ConvertAll(basecard, int.Parse);
                        break;
                    case "BASE_DAUB":
                        //Console.WriteLine(ItemVal.GetType());
                        //string[] basedaub = ItemVal.Split(',');
                        BASE_DAUB = ItemVal.Split(',');
                        //TODO convert to int Array --- Array.ConvertAll(basedaub, int.Parse);
                        break;
                    case "COVER_DAUB":
                        //Console.WriteLine(ItemVal.GetType());
                        //string[] coverdaub = ItemVal.Split(',');
                        COVER_DAUB = ItemVal.Split(',');
                        //TODO convert to int Array --- Array.ConvertAll(basedaub, int.Parse);
                        break;
                    case "BONUS_TYPE":
                        //Console.WriteLine(ItemVal);
                        BONUS_TYPE = Int32.Parse(ItemVal);
                        break;
                    case "BONUS_CARD":
                        //Console.WriteLine(ItemVal.GetType());
                        string[] bonuscard = ItemVal.Split(',');
                        BONUS_CARD = Array.ConvertAll(bonuscard, int.Parse);
                        break;
                    case "BONUS_DAUB":
                        //Console.WriteLine(ItemVal.GetType());
                        //string[] basedaub = ItemVal.Split(',');
                        BONUS_DAUB = ItemVal.Split(',');
                        //TODO convert to int Array --- Array.ConvertAll(basedaub, int.Parse);
                        break;
                    case "BASE_WIN":
                        //Console.WriteLine(ItemVal);
                        BASE_WIN = Int32.Parse(ItemVal);
                        break;
                    case "COVER_WIN":
                        //Console.WriteLine(ItemVal);
                        COVER_WIN = Int32.Parse(ItemVal);
                        break;
                    case "BONUS_WIN":
                        //Console.WriteLine(ItemVal);
                        BONUS_WIN = Int32.Parse(ItemVal);
                        break;
                    case "BASE_NET":
                        //Console.WriteLine(ItemVal);
                        BASE_NET = Int32.Parse(ItemVal);
                        break;
                    case "BONUS_NET":
                        //Console.WriteLine(ItemVal);
                        BONUS_NET = Int32.Parse(ItemVal);
                        break;
                    case "TOTAL_NET":
                        //Console.WriteLine(ItemVal);
                        TOTAL_NET = Int32.Parse(ItemVal);
                        break;

                }

            }
            Console.WriteLine("spin event: " + SPIN_EVENT);
            Console.WriteLine("engine id : " + ENGINE_ID);
            Console.WriteLine("game id : " + GAME_ID);
            Console.WriteLine("credits bet : " + CREDITS_BET);
            Console.WriteLine("bet level : " + BET_LEVEL);
            // log out Ball Draw Arrary
            if(BALL_DRAW != null)
            {
                Console.Write("\nball draw: ");
                for (var i = 0; i < BALL_DRAW.Length; i++)
                {
                    Console.Write("{0}  ", BALL_DRAW[i]);
                }
                Console.Write("\n");
            }
            // End Ball Draw log
            // log out Base Daub Arrary
            if (BASE_DAUB != null)
            {
                Console.Write("\nbase daub: ");
                for (var i = 0; i < BASE_DAUB.Length; i++)
                {
                    Console.Write("{0}  ", BASE_DAUB[i]);
                }
                Console.Write("\n");
            }
            // End Base Daub log
            // log out Cover Daub Arrary
            if (COVER_DAUB != null)
            {
                Console.Write("\ncover daub: ");
                for (var i = 0; i < COVER_DAUB.Length; i++)
                {
                    Console.Write("{0}  ", COVER_DAUB[i]);
                }
                Console.Write("\n");
            }
            // End Cover Daub log
            Console.WriteLine("bonus type : " + BONUS_TYPE);
            // log out Bonus Daub Arrary
            if (BONUS_DAUB != null)
            {
                Console.Write("\nbonus daub: ");
                for (var i = 0; i < BONUS_DAUB.Length; i++)
                {
                    Console.Write("{0}  ", BONUS_DAUB[i]);
                }
                Console.Write("\n");
            }
            // End Bonus Daub log
            Console.WriteLine("base win : " + BASE_WIN);
            Console.WriteLine("cover win : " + COVER_WIN);
            Console.WriteLine("bonus win : " + BONUS_WIN);
            Console.WriteLine("base net : " + BASE_NET);
            Console.WriteLine("bonus net : " + BONUS_NET);
            Console.WriteLine("total net : " + TOTAL_NET);

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
            string SpinRequest = "|NEW_SPIN|" + SubNumber + "|ENGINE_ID|" + ENGINE_ID + "|CREDITS_BET|" + CREDITS_BET + "|BET_LEVEL|" + BET_LEVEL + "|";
            OutBoundMsg = SpinRequest;
            SendMessage(OutBoundMsg, client);
            //Console.WriteLine(SpinRequest);
            ReceiveMessage(client);
            AddBingoNumToCard();
            AddBonusNumToCard();
        }

        private void Preview_btn_Click(object sender, EventArgs e)
        {
            // Send Preview Card Request
            // Preview Card Request Message format |PREVIEW_CARD|<SubNumber>|ENGINE_ID|<EngineID>|
            // Response Message format |PREVIEW_CARD|<SubNumber>|ENGINE_ID|<EngineID>|GAME_ID|<GameID>|BASE_CARD|<BaseCardSpots>|

            // Build Preview Card Request String
            string PreviewRequest = "|PREVIEW_CARD|" + SubNumber + "|ENGINE_ID|" + ENGINE_ID + "|";
            OutBoundMsg = PreviewRequest;
            SendMessage(OutBoundMsg, client);
            //Console.WriteLine(PreviewRequest);

            ReceiveMessage(client);
            AddBingoNumToCard();
            BONUS_CARD = cardSpotNums;
            AddBonusNumToCard();
        }

        private void Play_preview_btn_Click(object sender, EventArgs e)
        {
            // Play Preview Card 
            // Play Preview Card Request Message format |PLAY_PREVIEW|<SubNumber>|ENGINE_ID|<EngineID>|CREDITS_BET|<CreditQuantity>|BET_LEVEL|<Multiplier>|
            // Play Preview Response Message format |SPIN_EVENT|<SubNumber>|ENGINE_ID|<EngineID>|GAME_ID|<GameID>|CREDITS_BET|<CreditQuantity>|BET_LEVEL|<Multiplier>|BALL_DRAW|
            //  <BallDraw>|BASE_CARD|<BaseCardSpots>|BASE_DAUB|<BaseDaubs>|COVER_DAUB|<CoverDaubs>|BONUS_TYPE|<BonusID>|BONUS_CARD|<BonusCardSpots>|BONUS_DAUB|<BonusDaubs>|
            //  BASE_WIN|<BaseWinValue>|COVER_WIN|<CoverWinValue>|BONUS_WIN|<BonusWinValue>|BASE_NET|<BaseCreditsNet>|BONUS_NET|<BonusCreditsNet>|TOTAL_NET|<TotalCreditsNet>|

            // Build Play Preview Request String
            string PlayPreviewRequest = "|PLAY_PREVIEW|" + SubNumber + "|ENGINE_ID|" + ENGINE_ID + "|CREDITS_BET|" + CREDITS_BET + "|BET_LEVEL|" + BET_LEVEL + "|";
            OutBoundMsg = PlayPreviewRequest;
            SendMessage(OutBoundMsg, client);
            //Console.WriteLine(PlayPreviewRequest);

            ReceiveMessage(client);
            AddBingoNumToCard();
            AddBonusNumToCard();
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
            //Console.WriteLine(BET_LEVEL);
        }

        private void CreditsBet_ValueChanged(object sender, EventArgs e)
        {
            int credit = Convert.ToInt32(CreditsBet.Value);
            CREDITS_BET = credit;
            //Console.WriteLine(CreditsBet.Value);
        }

    }


}
