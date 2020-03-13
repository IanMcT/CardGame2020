using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardGame2020
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //global variables
        int[] cards;
        Random r = new Random();
        int LocationInDeck;
        int[] playerHand;
        int[] dealerhand;
        TranslateTransform[] playerTranslateTransform;
        TranslateTransform[] dealerTranslateTransform;
        Rectangle[] rectanglePlayer;
        Rectangle[] rectangleDealer;
        ImageBrush[] playerSprite;
        ImageBrush[] dealerSprite;

        public MainWindow()
        {
            InitializeComponent();
            
            //instantiate
            cards = new int[52];
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = i;
            }

            //set default values
            playerSprite = new ImageBrush[4];
            dealerSprite = new ImageBrush[4];
            LocationInDeck = 0;
            rectanglePlayer = new Rectangle[4];
            rectangleDealer = new Rectangle[4];
            playerHand = new int[] {-1,-1,-1,-1 };
            dealerhand = new int[] { -1, -1, -1, -1 };
            playerTranslateTransform = new TranslateTransform[4];
            dealerTranslateTransform = new TranslateTransform[4];


            string output = "";
            for (int i = 0; i < cards.Length; i++)
            {
                output += cards[i].ToString() + Environment.NewLine;
            }
            MessageBox.Show(output);
            cards = Shuffle(cards);
            output = "";
            for (int i = 0; i < cards.Length; i++)
            {
                output += cards[i].ToString() + Environment.NewLine;
            }
            MessageBox.Show(output);

            playGame();
        }//end MainWindow

        public int[] Shuffle(int[] c)
        {
            for (int i = 0; i < c.Length; i++)
            {
                int temp = r.Next(c.Length);
                int tempValue = c[i];
                c[i] = c[temp];
                c[temp] = tempValue;
            }
            return c;
        }

        public int Deal()
        {
            int c = cards[LocationInDeck];
            LocationInDeck++;
            return c;
        }//end Deal method

        //PlayGame
        //Deal two cards to the player
        //deal two cards to the dealer
        //show both hands.
        public void playGame()
        {
            for (int i = 0; i < 2; i++)
            {
                playerHand[i] = Deal();
                dealerhand[i] = Deal();
            }

            string playerhandOutput = "Player cards: ";
            string dealerhandOutput = "Dealer cards: ";
            for (int i = 0; i < playerHand.Length; i++)
            {
                if (playerHand[i] >= 0)
                {
                    playerhandOutput += playerHand[i].ToString() + ", ";
                }
                if (dealerhand[i] >= 0)
                {
                    dealerhandOutput += dealerhand[i].ToString() + ", ";
                }
            }
            displayCards();
            MessageBox.Show(playerhandOutput + Environment.NewLine
                + dealerhandOutput);
        }
        public void displayCards()
        {
            canvas.Children.Clear();//clear the canvas to start fresh


            BitmapImage bitmapImage = new BitmapImage(new Uri("cards.png",UriKind.Relative));
            

            for (int i = 0; i < playerHand.Length; i++)
            {
                playerSprite[i] = new ImageBrush(bitmapImage);
                dealerSprite[i] = new ImageBrush(bitmapImage);
                playerTranslateTransform[i] = new TranslateTransform(0, 0);
                dealerTranslateTransform[i] = new TranslateTransform(0, 0);

                playerSprite[i].Stretch = Stretch.None;
                playerSprite[i].AlignmentX = AlignmentX.Left;
                playerSprite[i].AlignmentY = AlignmentY.Top;
                dealerSprite[i].Stretch = Stretch.None;
                dealerSprite[i].AlignmentX = AlignmentX.Left;
                dealerSprite[i].AlignmentY = AlignmentY.Top;

                playerSprite[i].Viewport = new Rect(0, 0, bitmapImage.Width / 13, bitmapImage.Height / 4);
                dealerSprite[i].Viewport = new Rect(0, 0, bitmapImage.Width / 13, bitmapImage.Height / 4);

                //set based on card
                playerTranslateTransform[i] =
                    new TranslateTransform(-(playerHand[i] % 13) * (bitmapImage.Width / 13),
                    -(playerHand[i] / 13) * (bitmapImage.Height / 4));
                dealerTranslateTransform[i] =
                    new TranslateTransform(-(dealerhand[i] % 13) * (bitmapImage.Width / 13),
                    -(dealerhand[i] / 13) * (bitmapImage.Height / 4));
                playerSprite[i].Transform = playerTranslateTransform[i];
                dealerSprite[i].Transform = dealerTranslateTransform[i];

                //make array for player and dealer
                rectanglePlayer[i] = new Rectangle();
                rectangleDealer[i] = new Rectangle();
                rectanglePlayer[i].Fill = playerSprite[i];
                rectangleDealer[i].Fill = dealerSprite[i];
                rectanglePlayer[i].Width = bitmapImage.Width / 13;
                rectangleDealer[i].Width = bitmapImage.Width / 13;
                rectanglePlayer[i].Height = bitmapImage.Height / 4;
                rectangleDealer[i].Height = bitmapImage.Height / 4;

                canvas.Children.Add(rectanglePlayer[i]);
                canvas.Children.Add(rectangleDealer[i]);
                //set based on position of hand
                Canvas.SetTop(rectanglePlayer[i], 10);
                Canvas.SetTop(rectangleDealer[i], 10 * 2 + bitmapImage.Height / 4);
                Canvas.SetLeft(rectanglePlayer[i], 10*i + (bitmapImage.Width / 13)*i);
                Canvas.SetLeft(rectangleDealer[i], 10 * i + (bitmapImage.Width / 13) * i);
            }//end for loop
        }//end displaycards method

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            /*1 = 2 of clubs
13 = ace of diamonds
loop through the array until the value is -1
	set the element at that index to dealcard
end loop
call display cards method*/
            for (int i = 0; i < playerHand.Length; i++)
            {
                if (playerHand[i] < 0)
                {
                    playerHand[i] = Deal();
                    break;
                }//end if
            }//end for loop
            displayCards();
            checkIfBust();
        }

        private void checkIfBust()
        {
            MessageBox.Show("do your cards add up to over 21?");
            //throw new NotImplementedException();
        }

        private void btnStay_Click(object sender, RoutedEventArgs e)
        {
            checkIfBust();
            updateDealer();
        }

        private void updateDealer()
        {           
            int handTotal = 0;
            for (int i = 0; i< dealerhand.Length; i++)
            {
                int CurrentCard = dealerhand[i] % 13 + 1;
                if (CurrentCard > 10)
                {
                    CurrentCard = 10;
                }
                if (CurrentCard == 1)
                {
                    CurrentCard = 11;
                }
                handTotal += CurrentCard;
            }
            if (handTotal == 21)
            {
                gameOver();
            }
            else if (handTotal > 16)
            {
                checkWhoWon();
            }
            else
            {
                hitDealer();
            }
            
        }//end updateDealer

        private void hitDealer()
        {
            for (int i = 0; i < dealerhand.Length; i++)
            {
                if (dealerhand[i] < 0)
                {
                    dealerhand[i] = Deal();
                    break;
                }//end if
            }//end for loop
            displayCards();
            updateDealer();
        }

        private void checkWhoWon()
        {
            MessageBox.Show("anyone won?");
            //throw new NotImplementedException();
        }

        private void gameOver()
        {
            //throw new NotImplementedException();
        }

        private void btnDealNewHand_Click(object sender, RoutedEventArgs e)
        {

        }
    }//end class
}//end namespace
