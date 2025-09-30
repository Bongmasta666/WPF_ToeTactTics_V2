using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ToeTactTics_V2.classes;

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
        Player playerOne, playerTwo;

        bool isPlayerOneTurn = true;
        bool playerWon = false;
        int drawGames = 0;

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

        public void OnNewGame(Object sender, RoutedEventArgs e) => ShowUsernameDialog();

        public void StartGame()
        {
            buttonsRemaining = 9;
            playerWon = false;
            ResetBoard();
            SetRandomPlayer();
            ShowActivePlayer();
        }

        public void OnSquareSelected(Object sender, RoutedEventArgs e)
        {
            Button target = (Button)sender;
            target.Content = (isPlayerOneTurn) ? playerOne.symbol : playerTwo.symbol;
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

            if (playerWon) { OnPlayerWon(); }
            else if (buttonsRemaining <= 0) { OnDrawGame(); }
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

        public void ShowUsernameDialog()
        {
            CustomPopUp usernamePop = new CustomPopUp();
            usernamePop.Title = "Username Selection";
            usernamePop.Owner = this;
            usernamePop.ShowDialog();
            if (usernamePop.DialogResult == true) { OnDialogSuccess(usernamePop); }
        }

        private void OnDialogSuccess(CustomPopUp dialog)
        {
            playerOne.username = dialog.textBoxPlayerX.Text;
            playerTwo.username = dialog.textBoxPlayerO.Text;
            if (dialog.checkboxIntials.IsChecked == true)
            {
                playerOne.symbol = playerOne.username[0].ToString();
                playerTwo.symbol = playerTwo.username[0].ToString();
            }
            else
            {
                playerOne.symbol = "X";
                playerTwo.symbol = "O";
            }
            playerOne.wins = 0;
            playerTwo.wins = 0;
            drawGames = 0;
            labelDrawGames.Content = $"Draw Games: {drawGames}";
            UpdateUserInfo();
            StartGame();
        }

        public void OnPlayerWon()
        {
            string winnersName = "";
            if (isPlayerOneTurn)
            {
                winnersName = playerOne.username;
                playerOne.wins++;
            }
            else
            {
                winnersName = playerTwo.username;
                playerTwo.wins++;
            }
            UpdateUserInfo();
            ShowEndgameDialog($"Congrats {winnersName}! You Win!");
        }

        public void OnDrawGame()
        {
            drawGames++;
            labelDrawGames.Content = $"Draw Games: {drawGames}";
            ShowEndgameDialog("Neither Player Wins. Draw Game!");
        }

        public void ShowEndgameDialog(string message)
        {
            WinDrawPopUp pop = new WinDrawPopUp(this);
            pop.Owner = this;
            pop.ShowText(message);
            pop.ShowDialog();
            if (pop.DialogResult == false) { StartGame(); }
        }

        public void SetRandomPlayer()
        {
            Random random = new Random();
            int number = random.Next(1, 101);
            isPlayerOneTurn = number % 2 == 0; 
        }

        public void SetBoardState(bool isActive)
        {
            foreach (Button button in buttons) { button.IsEnabled = isActive; }
        }

        public void ClearBoardContent()
        {
            foreach (Button button in buttons) { button.Content = ""; button.Foreground = Brushes.Black; }
        }

        public void ShowActivePlayer()
        {
            labelPlayerTurn.Content = (isPlayerOneTurn) ? $"{playerOne.username}'s Turn" : $"{playerTwo.username}'s Turn";
        }

        public void UpdateUserInfo()
        {
            labelPlayerXInfo.Content = $"Player ({playerOne.symbol})  {playerOne.username}  : Wins - {playerOne.wins}";
            labelPlayerOInfo.Content = $"Player ({playerTwo.symbol})  {playerTwo.username}  : Wins - {playerTwo.wins}";
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