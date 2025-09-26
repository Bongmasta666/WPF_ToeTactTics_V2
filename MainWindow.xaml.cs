using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/*
  Author: Michael Millar
  Date: 09-25-2025
  Description:
    A quick WPF Tic Tac Toe type game. 
    Keeps track of 2 users and their score, evaultes board state on each play, and alternates between players.
*/
namespace ToeTactTics_V2
{
    public partial class MainWindow : Window
    {
        string playerXName = "";
        string playerOName = "";
        int playerXWins = 0;
        int playerOWins = 0;
        bool isPlayerXTurn = true;
        bool playerWon = false;

        Button[] buttons = [];
        int buttonsRemaining = 9;

        SolidColorBrush buttonHighlight = Brushes.LightSalmon; 
        public MainWindow()
        {
            InitializeComponent();
            buttons = [button1, button2, button3, button4, button5, 
                         button6, button7, button8, button9];
        }

        public void OnNewGame(Object sender , RoutedEventArgs e) => ShowUsernameDialog();

        public void StartGame()
        {
            buttonsRemaining = 9;
            playerWon = false;
            SetRandomPlayer();
            ShowActivePlayer();
            ClearBoardContent();
            SetBoardState(true);
        }

        public void OnSquareSelected(Object sender, RoutedEventArgs e)
        {
            Button target = (Button)sender;
            target.Content = (isPlayerXTurn) ? "X" : "O";
            target.IsEnabled = false;
            buttonsRemaining--;

            EvaluateBoard();

            isPlayerXTurn = !isPlayerXTurn;
            ShowActivePlayer();
        }

        public void EvaluateBoard()
        {
            //Horizontal
            CheckLine(button1, button2, button3);
            CheckLine(button4, button5, button6);
            CheckLine(button7, button8, button9);
            //Vertical
            CheckLine(button1, button4, button7);
            CheckLine(button2, button5, button8);
            CheckLine(button3, button6, button9);
            //Diagonal
            CheckLine(button1, button5, button9);
            CheckLine(button3, button5, button7);

            if (playerWon)
            {
                string winnersName = "";
                if (isPlayerXTurn) 
                {
                    winnersName = playerXName;
                    playerXWins++; 
                }
                else 
                {
                    winnersName = playerOName;
                    playerOWins++;
                }
                UpdateUserInfo();
                MessageBox.Show(this, $"Congrats {winnersName}! You Win!");
                StartGame();
            }
            else if(buttonsRemaining <= 0) 
            {
                MessageBox.Show(this, "Neither Player Wins. Draw Game!");
                StartGame();
            }
        }

        public bool CheckLine(Button first, Button second, Button third)
        {
            bool isConnected = first.Content == second.Content &&
                second.Content == third.Content && first.Content.ToString() != "";
            if (isConnected)
            {
                playerWon = true;
                first.Foreground = buttonHighlight;
                second.Foreground = buttonHighlight;
                third.Foreground = buttonHighlight;
            }
            return isConnected;
        }

        public void SetBoardState(bool isActive)
        {
            foreach (Button button in buttons) { button.IsEnabled = isActive; }
        }

        public void ClearBoardContent()
        {
            foreach (Button button in buttons) { button.Content = ""; button.Foreground = Brushes.Black; }
        }

        public void ShowUsernameDialog()
        {
            CustomPopUp usernamePop = new CustomPopUp();
            usernamePop.Title = "Username Selection";
            usernamePop.Owner = this;
            usernamePop.ShowDialog();

            if (usernamePop.DialogResult == true)
            {
                playerXName = usernamePop.textBoxPlayerX.Text;
                playerOName = usernamePop.textBoxPlayerO.Text;
                UpdateUserInfo();
                StartGame();
            }
        }

        public void SetRandomPlayer()
        {
            Random random = new Random();
            int number = random.Next(1, 101);
            isPlayerXTurn = number % 2 == 0; 
        }

        public void ShowActivePlayer()
        {
            labelPlayerTurn.Content = (isPlayerXTurn) ? $"{playerXName}'s Turn" : $"{playerOName}'s Turn";
        }

        public void UpdateUserInfo()
        {
            labelPlayerXInfo.Content = $"Player (X)  {playerXName}  : Wins - {playerXWins}";
            labelPlayerOInfo.Content = $"Player (O)  {playerOName}  : Wins - {playerOWins}";
        }

        public void OnQuitGame(Object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}