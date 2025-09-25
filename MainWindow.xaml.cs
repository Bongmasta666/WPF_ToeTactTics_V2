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

        Button[] buttons = [];
        int buttonsRemaining = 9;
        public MainWindow()
        {
            InitializeComponent();
            buttons = [button1, button2, button3, button4, button5, 
                         button6, button7, button8, button9];
        }

        public void OnNewGame(Object sender , RoutedEventArgs e) => NewGame();
        public void NewGame()
        {
            ShowUsernameDialog();
        }

        public void StartGame()
        {
            SetRandomPlayer();
            ShowActivePlayer();
            SetBoardState(true);
        }

        public void OnSquareSelected(Object sender, RoutedEventArgs e)
        {
            Button target = (Button)sender;
            target.Content = (isPlayerXTurn) ? "X" : "O";
            target.IsEnabled = false;

            isPlayerXTurn = !isPlayerXTurn;
            ShowActivePlayer();
        }

        public bool CheckLine(Button first, Button second, Button third)
        {
            return first.Content == second.Content && second.Content == third.Content && first.Content.ToString() != "";
        }

        public void SetBoardState(bool isActive)
        {
            foreach (Button button in buttons) { button.IsEnabled = isActive; }
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