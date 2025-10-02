using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToeTactTics_V2.classes
{
    public class GridManager
    {
        Grid? gameGrid = null;
        public Button[,] buttonGrid = new Button[0,0];
        RoutedEventHandler? callback = null;
        
        //Builds a square grid using 'rows' to determine the grid size.
        //The 'onClick' handler should be function that responds to a cell being clicked on.
        public void BuildGameGrid(Panel parent, int rows, int cellSize, RoutedEventHandler onClick)
        {
            if (gameGrid != null) { parent.Children.Remove(gameGrid); }

            callback = onClick;
            buttonGrid = new Button[rows, rows];
            gameGrid = new Grid();
            Grid.SetRow(gameGrid, 1);
            parent.Children.Add(gameGrid);

            for (int i = 0; i < rows; i++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                gameGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < rows; column++)
                {
                    Button btn = new Button();
                    btn.Width = cellSize;
                    btn.Height = cellSize;
                    btn.Content = " ";
                    btn.FontSize = 40;
                    btn.Click += callback;
                    btn.IsEnabled = false;
                    Grid.SetRow(btn, row);
                    Grid.SetColumn(btn, column);
                    buttonGrid[row, column] = btn;
                    gameGrid.Children.Add(btn);
                }
            }

            gameGrid.HorizontalAlignment = HorizontalAlignment.Center;
            gameGrid.VerticalAlignment = VerticalAlignment.Center;
        }

        //Checks the entire grid to see if any row, column, or corner angles are connected.
        //Returns true if found, false if not found.
        public bool CheckGrid(string value)
        {
            bool playerWon = false;
            int gridSize = buttonGrid.GetLength(1);
            if (CheckAxis(true, value, gridSize)) { playerWon = true; }
            if (CheckAxis(false, value, gridSize)) { playerWon = true; }
            if (CheckAngleOne(value, gridSize)) { playerWon = true; }
            if (CheckAngleTwo(value, gridSize)) { playerWon = true; }
            return playerWon;
        }

        //Loops over the 'buttonGrids' columns or rows based on 'isRows' and 'length'.
        //Returns whether the button element's content in the current cell is equal to 'letter'
        //The variable 'length' should be the total number of rows or columns.
        public bool CheckAxis(bool isRows, string letter, int length)
        {
            List<Button> connectedBtns = [];
            bool isConnected = false;
            for (int i = 0; i < length; i++)
            {
                connectedBtns.Clear();
                for (int j = 0; j < length; j++)
                {
                    Button target = (isRows) ? buttonGrid[i, j] : buttonGrid[j, i];
                    string value = target.Content.ToString();
                    if (value != letter) { break; }
                    else { connectedBtns.Add(target); }
                }
                if (connectedBtns.Count >= length)
                { 
                    HighlightLine(connectedBtns); 
                    isConnected = true;
                }
            } 
            return isConnected;
        }

        //Checks the buttonGrid top left to bottom right for a connection of 'letter'.
        //The variable 'length' should be the total number of rows or columns.
        public bool CheckAngleOne(string letter, int length)
        {
            List<Button> connectedBtns = [];
            bool isConnected = false;
            for (int i = 0; i < length; i++)
            {
                Button target = buttonGrid[i, i];
                if (target.Content.ToString() != letter) { break; }
                else { connectedBtns.Add(target); }
            }
            if (connectedBtns.Count >= length) 
            { 
                HighlightLine(connectedBtns); 
                isConnected = true;
            }
            return isConnected;
        }

        //Checks the buttonGrid top right to bottom left for a connection of 'letter'.
        //The variable 'length' should be the total number of rows or columns.
        public bool CheckAngleTwo(string letter, int length)
        {
            int g = length - 1;
            List<Button> connectedBtns = [];
            bool isConnected = false;
            for (int i = 0; i < length; i++)
            {
                Button target = buttonGrid[g, i];
                if (target.Content.ToString() != letter) { break; }
                else { connectedBtns.Add(target); }
                g--;
            }
            if (connectedBtns.Count >= length)
            {
                HighlightLine(connectedBtns);
                isConnected = true;
            }
            return isConnected;
        }

        //When a square is created and the event handler is added, this is added to avoid any memory leaks.
        private void OnSquareUnload(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.Click -= callback;
        }

        //Sets all buttons in the 'targets' list to have a gold foreground
        //Maybe ToDo: Add parameter for color.
        public void HighlightLine(List<Button> targets)
        {
            foreach (Button b in targets) { b.Foreground = Brushes.Gold; }
        }

        //Disables or Enables all buttons based on 'state'
        public void SetGridState(bool state)
        {
            foreach (Button btn in buttonGrid) { btn.IsEnabled = state; }
        }

        //Sets all Button's Content in buttonGrid to be empty and have a black foreground.
        public void ResetButtons()
        {
            foreach (Button button in buttonGrid) { button.Content = ""; button.Foreground = Brushes.Black; }
        }

        //Returns the total amount of buttons in the buttonGrid
        public int GetButtonCount() { return buttonGrid.Length; }
    }
}
