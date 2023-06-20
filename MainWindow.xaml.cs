using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Krestiki_noliki
{
    public partial class MainWindow : Window
    {
        private enum Player { None, 口, 人 }

        private Player currentPlayer = Player.口;
        private int scoreX = 0;
        private int scoreO = 0;


        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Content == null)
            {
                button.Content = currentPlayer.ToString();
                button.IsEnabled = false;

                if (CheckForWin())
                {
                    if (currentPlayer == Player.口)
                        scoreX++;
                    else
                        scoreO++;

                    scoreLabel.Content = $"X: {scoreX}  O: {scoreO}";
                    SaveGame();
                    ResetGame();

                }
                else if (CheckForDraw())
                {
                    scoreLabel.Content = "Нічия!";
                    ResetGame();
                    SaveGame();

                }
                else
                {
                    currentPlayer = currentPlayer == Player.口 ? Player.人 : Player.口;
                }
            }
        }

        private bool CheckForWin()
        {
            string[] moves = new string[9];

            for (int i = 0; i < 9; i++)
            {
                Button button = (Button)gameGrid.Children[i];
                moves[i] = button.Content as string;
            }

            // Check rows
            for (int i = 0; i < 3; i++)
            {
                if (moves[i * 3] == moves[i * 3 + 1] && moves[i * 3] == moves[i * 3 + 2] && !string.IsNullOrEmpty(moves[i * 3]))
                    return true;
            }

            // Check columns
            for (int i = 0; i < 3; i++)
            {
                if (moves[i] == moves[i + 3] && moves[i] == moves[i + 6] && !string.IsNullOrEmpty(moves[i]))
                    return true;
            }

            // Check diagonals
            if ((moves[0] == moves[4] && moves[0] == moves[8] && !string.IsNullOrEmpty(moves[0])) ||
                (moves[2] == moves[4] && moves[2] == moves[6] && !string.IsNullOrEmpty(moves[2])))
                return true;

            return false;


        }

        private bool CheckForDraw()
        {
            foreach (Button button in gameGrid.Children)
            {
                if (button.Content == null)
                    return false;
            }

            return !CheckForWin();
        }

        private async void SaveGame()
        {
            MessageBox.Show("Результати були збережені до файлу 'Result_Krestiki_noliki'");
            string path = @"C:\Users\nasty\OneDrive\Робочий стіл\Программирование\Project\NET.CORE\LAB_1\Krestiki_noliki\Result_Krestiki_noliki.txt";
            string result_info = scoreLabel.Content.ToString();
            using (StreamWriter saving = File.AppendText(path))
            {
                await saving.WriteLineAsync(result_info);
            }
        }
        private void ResetGame()
        {
            foreach (Button button in gameGrid.Children)
            {
                button.Content = null;
                button.IsEnabled = true;
            }

            currentPlayer = Player.口;
        }
    }
}