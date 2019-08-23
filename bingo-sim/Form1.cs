using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bingo_sim
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AddBingoNumToCard();
        }

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


        private void Connect_btn_Click(object sender, EventArgs e)
        {
            //Connect to bing server
        }

        private void Spin_btn_Click(object sender, EventArgs e)
        {
            // Send Play Request
        }

        private void Preview_btn_Click(object sender, EventArgs e)
        {
            // Send Preview Card Request
        }

        private void Play_preview_btn_Click(object sender, EventArgs e)
        {
            // Play Preview Card Request
        }

        private void Interim_Daub_btn_CheckedChanged(object sender, EventArgs e)
        {
            // Display Interim Card Daub 
        }

        private void Coverall_Daub_btn_CheckedChanged(object sender, EventArgs e)
        {
            // Display Coverall Card Daub
        }

        private void Messages_out_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Display outgoing message
        }

        private void Messages_in_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Display incoming message
        }
    }


}
