using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MegaChallengeCasino
{
    public partial class Default : System.Web.UI.Page
    {
        string[] images = new string[] {"Bar.png", "Bell.png", "Cherry.png", "Clover.png", "Diamond.png", "HorseShoe.png", "Lemon.png", "Orange.png", "Plum.png", "Seven.png", "Strawberry.png", "Watermellong.png"};
        Random random = new Random();
        int cherriesCount = 0;
        int sevensCount = 0;
        int barCount = 0;
        int bet = 0;
        int winnings = 0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                getRandomImages();
                int playerMoney = 100;
                ViewState.Add("PlayerMoney", playerMoney);
                moneyLabel.Text = String.Format("Player Money: {0:C}", playerMoney);
            }
        }

        protected void playButton_Click(object sender, EventArgs e)
        {
            playRound();
        }

        public void playRound()
        {
            getRandomImages();
            takePlayerBet();
            checkRoll();
            scoreRound();
            displayResults();
        }

        private void getRandomImages()
        {
            Image1.ImageUrl = images[random.Next(1, 12) - 1];
            Image2.ImageUrl = images[random.Next(1, 12) - 1];
            Image3.ImageUrl = images[random.Next(1, 12) - 1];
        }

        private void evaluateImage(string imageURL)
        {
            switch (imageURL)
            {
                case "Bar.png":
                    barCount += 1;
                    break;
                case "Cherry.png":
                    cherriesCount += 1;
                    break;
                case "Seven.png":
                    sevensCount += 1;
                    break;
                default:
                    break;
            }
        }

        public void checkRoll()
        {
            evaluateImage(Image1.ImageUrl);
            evaluateImage(Image2.ImageUrl);
            evaluateImage(Image3.ImageUrl);
        }

        public void takePlayerBet()
        {
            if (!int.TryParse(betTextBox.Text, out bet))
                resultLabel.Text = "<p style='color: red'>Error: Please place a valid bet.</p>";
        }

        public void scoreRound()
        {
            int playerMoney = (int)ViewState["PlayerMoney"];
            if (barCount > 0)
                winnings -= bet;
            else if (cherriesCount == 1)
                winnings += bet * 2;
            else if (cherriesCount == 2)
                winnings += bet * 3;
            else if (cherriesCount == 3)
                winnings += bet * 4;
            else if (sevensCount == 3)
                winnings += bet * 100;
            else
                winnings -= bet;

            playerMoney += winnings;
            ViewState["PlayerMoney"] = playerMoney;
            
        }

        private void displayResults()
        {
            if (barCount == 0 && (cherriesCount > 0 || sevensCount == 3))
                resultLabel.Text = String.Format("You bet <b style='color:blue'>{0:C}</b> and won <b style='color:green'>{1:C}</b>!", bet, winnings);
            else
                resultLabel.Text = String.Format("You lost <b style='color:red'>{0:C}</b>. Better luck next time.", bet);
            int playerMoney = (int)ViewState["PlayerMoney"];
            moneyLabel.Text = String.Format("Player Money: {0:C}", playerMoney);
        }
    }
}