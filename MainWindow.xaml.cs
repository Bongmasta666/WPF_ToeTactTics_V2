using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToeTactTics_V2.classes;

/*
  Author: Michael Millar
  Date: 10-01-2025
  Description:
    A quick WPF Tic Tac Toe type game. 
    Keeps track of 2 users and their score, evaultes board state on each play, and alternates between players.
*/
namespace ToeTactTics_V2
{
    public partial class MainWindow : Window
    {
        GridManager gridManager = new();
        Player playerOne, playerTwo;

        bool isPlayerOneTurn = true;
        int drawGames = 0;

        int buttonsRemaining = 9;

        public MainWindow()
        {
            InitializeComponent();

            RoutedCommand newGameCommand = new();
            CommandBindings.Add(new CommandBinding(newGameCommand, OnNewGame));
            InputBindings.Add(new KeyBinding(newGameCommand, Key.N, ModifierKeys.Control));

            RoutedCommand quitGameCommand = new();
            CommandBindings.Add(new CommandBinding(quitGameCommand, OnQuitGame));
            InputBindings.Add(new KeyBinding(quitGameCommand, Key.Q, ModifierKeys.Control));

            gridManager.BuildGameGrid(gameGridCon, 3, 60, OnSquareSelected);
        }

        public void OnNewGame(Object sender, RoutedEventArgs e) => ShowUsernameDialog();

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
            ToggleStart();
        }

        public void ToggleStart()
        {   
            btnStart.IsEnabled = !btnStart.IsEnabled;
            if (btnStart.IsEnabled)
            {
                btnStart.Visibility = Visibility.Visible;
                btnStart.Focus();
            }
            else { btnStart.Visibility = Visibility.Hidden;}
        }

        public void OnStartGame(Object sender, RoutedEventArgs e) => StartGame();

        public void StartGame()
        {
            ResetBoard();
            SetRandomPlayer();
            ShowActivePlayer();
            ToggleStart();
            btnOptions.IsEnabled = false;
        }

        public void OnSquareSelected(Object sender, RoutedEventArgs e)
        {
            Button target = (Button)sender;
            string stamp = (isPlayerOneTurn) ? playerOne.symbol : playerTwo.symbol;
            target.Content = stamp;
            target.IsEnabled = false;
            buttonsRemaining--;

            if (gridManager.CheckGrid(stamp)) { OnPlayerWon(); }
            else if (buttonsRemaining <= 0) { OnDrawGame(); }
            else { SwapPlayer(); }
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
            btnOptions.IsEnabled = true;
            gridManager.SetGridState(false);
            labelPlayerTurn.Content = "Game Over";
            WinDrawPopUp pop = new WinDrawPopUp(this);
            pop.Owner = this;
            pop.ShowText(message);
            pop.ShowDialog();
            ToggleStart();
        }

        public void SetRandomPlayer()
        {
            Random random = new Random();
            int number = random.Next(1, 101);
            isPlayerOneTurn = number % 2 == 0; 
        }

        public void SwapPlayer()
        {
            isPlayerOneTurn = !isPlayerOneTurn;
            ShowActivePlayer();
        }

        public void ResetBoard()
        {
            gridManager.ResetButtons();
            gridManager.SetGridState(true);
            buttonsRemaining = gridManager.GetButtonCount();
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

        public void OnChangeSize(object sender, RoutedEventArgs e)
        {
            RadioButton obj = (RadioButton)sender;
            char c = obj.Content.ToString()[0];
            int gridSize = int.Parse(c.ToString());
            gridManager.BuildGameGrid(gameGridCon, gridSize, 60, OnSquareSelected);
        }

        public void OnQuitGame(Object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}