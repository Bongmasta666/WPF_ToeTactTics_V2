using System.Data.Common;
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
        string playerOneName = "";
        string playerTwoName = "";

        int playerOneWins = 0;
        int playerTwoWins = 0;

        string playerOneSymbol = "X";
        string playerTwoSymbol = "O";

        bool isPlayerOneTurn = true;
        bool playerWon = false;

        Button[] buttons = [];
        int buttonsRemaining = 9;

        SolidColorBrush buttonHighlight = Brushes.LightSalmon; 
        public MainWindow()
        {
            InitializeComponent();
            buttons = [button1, button2, button3, button4, button5, 
                         button6, button7, button8, button9];

            RoutedCommand newGameCommand = new();
            CommandBindings.Add(new CommandBinding(newGameCommand, OnNewGame));
            InputBindings.Add(new KeyBinding(newGameCommand, Key.N, ModifierKeys.Control));

            RoutedCommand quitGameCommand = new();
            CommandBindings.Add(new CommandBinding(quitGameCommand, OnQuitGame));
            InputBindings.Add(new KeyBinding(quitGameCommand, Key.Q, ModifierKeys.Control));
        }

        public void OnNewGame(Object sender , RoutedEventArgs e) => ShowUsernameDialog();

        public void StartGame()
        {
            buttonsRemaining = 9;
            playerWon = false;
            SetRandomPlayer();
            ShowActivePlayer();
            ResetBoard();
        }

        public void OnSquareSelected(Object sender, RoutedEventArgs e)
        {
            Button target = (Button)sender;
            target.Content = (isPlayerOneTurn) ? playerOneSymbol : playerTwoName;
            target.IsEnabled = false;
            buttonsRemaining--;

            EvaluateBoard();

            isPlayerOneTurn = !isPlayerOneTurn;
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
                if (isPlayerOneTurn) 
                {
                    winnersName = playerOneName;
                    playerOneWins++; 
                }
                else 
                {
                    winnersName = playerTwoName;
                    playerTwoWins++;
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
                playerOneName = usernamePop.textBoxPlayerX.Text;
                playerTwoName = usernamePop.textBoxPlayerO.Text;
                UpdateUserInfo();
                StartGame();
            }
        }

        public void SetRandomPlayer()
        {
            Random random = new Random();
            int number = random.Next(1, 101);
            isPlayerOneTurn = number % 2 == 0; 
        }

        public void ShowActivePlayer()
        {
            labelPlayerTurn.Content = (isPlayerOneTurn) ? $"{playerOneName}'s Turn" : $"{playerTwoName}'s Turn";
        }

        public void UpdateUserInfo()
        {
            labelPlayerXInfo.Content = $"Player ({playerOneSymbol})  {playerOneName}  : Wins - {playerOneWins}";
            labelPlayerOInfo.Content = $"Player ({playerTwoSymbol})  {playerTwoName}  : Wins - {playerTwoWins}";
        }

        public void ResetBoard()
        {
            ClearBoardContent();
            SetBoardState(true);
        }

        public void OnQuitGame(Object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}