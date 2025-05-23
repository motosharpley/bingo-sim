﻿using System;
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
        //private int SPIN_EVENT; Response Messages recieved use this key to echo back the SUB_NUMBER passed in the request 
        private string ENGINE_ID;
        private string GAME_ID;
        private int CREDITS_BET;
        private int BET_LEVEL;
        private int[] BALL_DRAW;
        private int[] BASE_CARD;
        private int[] BASE_DAUB;
        private int[] COVER_DAUB;
        private int BONUS_TYPE;
        private int[] BONUS_CARD;
        private int[] BONUS_DAUB;
        private int BASE_WIN;
        private int COVER_WIN;
        private int BONUS_WIN;
        private int BASE_NET;
        private int BONUS_NET;
        private int TOTAL_NET;



        // Subscription Event Variables
        private string GameName = "Redfire"; // this field is used on start-up subscription only and server assigned GAME_ID is in reponse message for use in all subsequent requests
        private string IP_ADDRESS = "127.0.0.1"; // this should be populated with the EGM IP Address
        private int SUB_NUMBER;
        private int MACHINE_ID = 12345;
        private string SERVER = "10.7.3.2";// 10.7.3.2 for production server

        private bool preview = false;
        private bool connected = false;


        // Dummy Data Bingo Card Index Numbers
        int[] cardSpotNums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
        int[] blankDaubs = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


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

        private void DaubBaseCard()
        {
            int i = 0;
            foreach (Control control in BingoCard.Controls)
            {
                Label BingoCardLabel = control as Label;
                if (BASE_DAUB[i] > 0)
                {
                    BingoCardLabel.BackColor = Color.FromArgb(204, 0, 0);                    
                } else
                {
                    BingoCardLabel.BackColor = Color.FromArgb(255, 255, 255);
                }
                i++;
            }
        }

        private void DaubBonusCard()
        {
            int i = 0;
            foreach (Control control in BonusCard.Controls)
            {
                Label BingoCardLabel = control as Label;
                if (BONUS_DAUB[i] >0)
                {
                    BingoCardLabel.BackColor = Color.FromArgb(204, 0, 0);
                }
                else
                {
                    BingoCardLabel.BackColor = Color.FromArgb(255, 255, 255);
                }
                i++;
            }
        }

        public void Connect(String msg)
        {
            OutGoingMSG.Text = msg;
            try
            {
                string server = SERVER;
                Int32 port = 4779;
                TcpClient client = new TcpClient(server, port);
                Console.WriteLine(msg);
                Byte[] data = Encoding.ASCII.GetBytes(msg);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);


                data = new Byte[1024];
                // String to store the response ASCII representation.
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                IncomingMSG.Text = responseData;

                string[] items = responseData.Split('|');

                foreach (var item in items)
                {
                    int index = Array.IndexOf(items, item);
                    //var field = item;
                    //Console.WriteLine($"field:{field}");
                    string ItemVal = items[index + 1];
                    switch (item)
                    {
                        case "SUBSCRIBE":
                            //Console.WriteLine(ItemVal);
                            SUB_NUMBER = Int32.Parse(ItemVal);
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
                            string[] basedaub = ItemVal.Split(',');
                            BASE_DAUB = Array.ConvertAll(basedaub, int.Parse);
                            //TODO convert to int Array --- Array.ConvertAll(basedaub, int.Parse);
                            break;
                        case "COVER_DAUB":
                            //Console.WriteLine(ItemVal.GetType());
                            string[] coverdaub = ItemVal.Split(',');
                            COVER_DAUB = Array.ConvertAll(coverdaub, int.Parse);
                            //TODO convert to int Array --- Array.ConvertAll(coverdaub, int.Parse);
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
                            string[] bonusdaub = ItemVal.Split(',');
                            BONUS_DAUB = Array.ConvertAll(bonusdaub, int.Parse);
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
                Console.WriteLine("subNumber: " + SUB_NUMBER);
                Console.WriteLine("engine id : " + ENGINE_ID);
                Console.WriteLine("game id : " + GAME_ID);
                Console.WriteLine("credits bet : " + CREDITS_BET);
                Console.WriteLine("bet level : " + BET_LEVEL);
                // log out Ball Draw Arrary
                if (BALL_DRAW != null)
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

               // Console.WriteLine(responseData);
                stream.Close();
                client.Close();
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
            // Response Message format |SUBSCRIBE|<SubNumber>|ENGINE_ID|<EngineID>|

            // Send Subcription message
            string SubRequest = "|SUBSCRIBE|" + GameName + "|MACHINE_ID|" + MACHINE_ID + "|";
            if (!connected)
            {
                Connect(SubRequest);
                connected = true;
            }
           
        }

        private void Spin_btn_Click(object sender, EventArgs e)
        {
            // Send Spin Request
            // New Spin Request Message format |NEW_SPIN|<SubNumber>|ENGINE_ID|<EngineID>|CREDITS_BET|<CreditQuantity>|BET_LEVEL|<Multiplier>|
            // Spin Response Message format |SPIN_EVENT|<SubNumber>|ENGINE_ID|<EngineID>|GAME_ID|<GameID>|CREDITS_BET|<CreditQuantity>|BET_LEVEL|<Multiplier>|BALL_DRAW|
            //  <BallDraw>|BASE_CARD|<BaseCardSpots>|BASE_DAUB|<BaseDaubs>|COVER_DAUB|<CoverDaubs>|BONUS_TYPE|<BonusID>|BONUS_CARD|<BonusCardSpots>|BONUS_DAUB|<BonusDaubs>|
            //  BASE_WIN|<BaseWinValue>|COVER_WIN|<CoverWinValue>|BONUS_WIN|<BonusWinValue>|BASE_NET|<BaseCreditsNet>|BONUS_NET|<BonusCreditsNet>|TOTAL_NET|<TotalCreditsNet>|

            // Build and Send Spin Request String
            string SpinRequest = "|NEW_SPIN|" + SUB_NUMBER + "|ENGINE_ID|" + ENGINE_ID + "|CREDITS_BET|" + CREDITS_BET + "|BET_LEVEL|" + BET_LEVEL + "|";
            Connect(SpinRequest);

            // Display Bingo Cards
            AddBingoNumToCard();
            AddBonusNumToCard();
            DaubBaseCard();
            DaubBonusCard();
            // Display play results
            BaseWin.Text = BASE_WIN.ToString();
            BonusWin.Text = BONUS_WIN.ToString();
            NetWin.Text = TOTAL_NET.ToString();
            // array to string for ball draw display purpose only
            string balls = String.Join(",", BALL_DRAW.Select(p => p.ToString()).ToArray());
            BallDraw.Text = balls;
            preview = false;
        }

        private void Preview_btn_Click(object sender, EventArgs e)
        {
            // Send Preview Card Request
            // Preview Card Request Message format |PREVIEW_CARD|<SubNumber>|ENGINE_ID|<EngineID>|
            // Response Message format |PREVIEW_CARD|<SubNumber>|ENGINE_ID|<EngineID>|GAME_ID|<GameID>|BASE_CARD|<BaseCardSpots>|

            // Build Preview Card Request String
            string PreviewRequest = "|PREVIEW_CARD|" + SUB_NUMBER + "|ENGINE_ID|" + ENGINE_ID + "|";
            Connect(PreviewRequest);


            // Display Card Preview Set bonus card to spot indexes
            AddBingoNumToCard();
            BONUS_CARD = blankDaubs; // set bonus card to spot indexes
            AddBonusNumToCard();
            BASE_DAUB = blankDaubs;
            BONUS_DAUB = blankDaubs;
            DaubBaseCard();
            DaubBonusCard();
            BallDraw.Text = "";
            preview = true;

        }

        private void Play_preview_btn_Click(object sender, EventArgs e)
        {
            // Play Preview Card 
            // Play Preview Card Request Message format |PLAY_PREVIEW|<SubNumber>|ENGINE_ID|<EngineID>|CREDITS_BET|<CreditQuantity>|BET_LEVEL|<Multiplier>|
            // Play Preview Response Message format |SPIN_EVENT|<SubNumber>|ENGINE_ID|<EngineID>|GAME_ID|<GameID>|CREDITS_BET|<CreditQuantity>|BET_LEVEL|<Multiplier>|BALL_DRAW|
            //  <BallDraw>|BASE_CARD|<BaseCardSpots>|BASE_DAUB|<BaseDaubs>|COVER_DAUB|<CoverDaubs>|BONUS_TYPE|<BonusID>|BONUS_CARD|<BonusCardSpots>|BONUS_DAUB|<BonusDaubs>|
            //  BASE_WIN|<BaseWinValue>|COVER_WIN|<CoverWinValue>|BONUS_WIN|<BonusWinValue>|BASE_NET|<BaseCreditsNet>|BONUS_NET|<BonusCreditsNet>|TOTAL_NET|<TotalCreditsNet>|

            if (preview)
            {
                // Build Play Preview Request String
                string PlayPreviewRequest = "|PLAY_PREVIEW|" + SUB_NUMBER + "|ENGINE_ID|" + ENGINE_ID + "|CREDITS_BET|" + CREDITS_BET + "|BET_LEVEL|" + BET_LEVEL + "|";
                Connect(PlayPreviewRequest);

                // Display Bingo Cards
                AddBingoNumToCard();
                AddBonusNumToCard();
                DaubBaseCard();
                DaubBonusCard();
                // Display play results
                BaseWin.Text = BASE_WIN.ToString();
                BonusWin.Text = BONUS_WIN.ToString();
                NetWin.Text = TOTAL_NET.ToString();
                // array to string for ball draw display purpose only
                string balls = String.Join(",", BALL_DRAW.Select(p => p.ToString()).ToArray());
                BallDraw.Text = balls;
                preview = false;
            }

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

        private void GameID_TextChanged(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(MachineID.Text);
            MACHINE_ID = ID;
        }


    }


}
