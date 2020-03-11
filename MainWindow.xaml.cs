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
            LocationInDeck = 0;
            playerHand = new int[] {-1,-1,-1,-1 };
            dealerhand = new int[] { -1, -1, -1, -1 };


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
            displayCard(13);
            MessageBox.Show(playerhandOutput + Environment.NewLine
                + dealerhandOutput);
        }
        public void displayCard(int c)
        {
           
            BitmapImage bitmapImage = new BitmapImage(new Uri("cards.png",UriKind.Relative));
            ImageBrush sprite = new ImageBrush(bitmapImage);
            TranslateTransform translateTransform;
            translateTransform = new TranslateTransform(0, 0);

            sprite.Stretch = Stretch.None;
            sprite.AlignmentX = AlignmentX.Left;
            sprite.AlignmentY = AlignmentY.Top;
            //(c%13)*(bitmapImage.Width/13)
            sprite.Viewport = new Rect(0, 0, bitmapImage.Width/13, bitmapImage.Height/4);
            translateTransform = 
                new TranslateTransform(-(c % 13) * (bitmapImage.Width / 13), 
                -(c/13)*(bitmapImage.Height/4));
            sprite.Transform = translateTransform;

            Rectangle card1 = new Rectangle();
            card1.Fill = sprite;
            card1.Width = bitmapImage.Width/13;
            card1.Height = bitmapImage.Height / 4;

            canvas.Children.Add(card1);
            Canvas.SetTop(card1, 50);
            Canvas.SetLeft(card1, 100);
            // MessageBox.Show(bitmapImage.Width.ToString());
          //  MessageBox.Show(bitmapImage.Height.ToString());
        }
    }
}
