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

namespace rgr_krestiki_noliki
{
    public partial class MainWindow : Window
    {
        private char[] board = new char[9];
        private bool isPlayerTurn = true;

        public MainWindow()
        {
            InitializeComponent();
            Array.Fill(board, ' ');
        }
        int countComputerWins = 0;
        int countDraws = 0;
        int countPlayerWins = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int index = int.Parse(button.Name.Substring(1)) - 1;

            if (board[index] == ' ') 
            {
                if (isPlayerTurn) 
                {
                    board[index] = 'O'; 
                    button.Content = "O"; 
                    UpdateBoard(); 

                    if (checkWin(board, 'O')) 
                    {
                        countPlayerWins++;
                        MessageBox.Show("Ты сделал невозможное!");
                        CountResults();
                        return;
                    }
                    else if (!IsMovesEnd(board)) 
                    {
                        countDraws++;
                        MessageBox.Show("Ничья!");
                        CountResults();
                        return;
                    }

                    isPlayerTurn = !isPlayerTurn;
                    doBestMove(board); 
                    UpdateBoard(); 
                    
                    if (checkWin(board, 'X')) 
                    {   
                        countComputerWins++;
                        MessageBox.Show("Компьютер победил!");
                        CountResults();
                        return;
                    }
                    else
                    {
                        isPlayerTurn = !isPlayerTurn; 
                    }
                }
            }
        }

        private void UpdateBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                ((Button)this.FindName($"B{i + 1}")).Content = board[i] == ' ' ? string.Empty : board[i].ToString();
            }
        }

        private void ResetGame()
        {
            Array.Fill(board, ' ');
            UpdateBoard();
            isPlayerTurn = true;
        }
        
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void doBestMove(char[] board)
        {
            int bestVal = int.MinValue; 
            int bestMove = -1;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == ' ')
                {
                    board[i] = 'X';
                    int moveVal = miniMaxArlgoritm(board, 0, false);
                    board[i] = ' ';
                    if (moveVal > bestVal)
                    {
                        bestVal = moveVal;
                        bestMove = i;
                    }
                }
            }
            board[bestMove] = 'X';
        }

        private int miniMaxArlgoritm(char[] board, int depth, bool isMax)
        {
            int score = ValueLift(board);
            if (score == 10)
            {
                return score - depth;
            }
            if (score == -10)
            {
                return score + depth;
            }
            if (!IsMovesEnd(board))
            {
                return 0;
            }
            if (isMax)
            {
                int best = int.MinValue;
                for (int i = 0; i < 9; i++)
                {
                    if (board[i] == ' ')
                    {
                        board[i] = 'X';
                        best = Math.Max(best, miniMaxArlgoritm(board, depth + 1, !isMax));
                        board[i] = ' ';
                    }
                }
                return best;
            }
            else
            {
                int best = int.MaxValue;
                for (int i = 0; i < 9; i++)
                {
                    if (board[i] == ' ')
                    {
                        board[i] = 'O';
                        best = Math.Min(best, miniMaxArlgoritm(board, depth + 1, !isMax));
                        board[i] = ' ';
                    }
                }
                return best;
            }
        }

        private bool checkWin(char[] board, char player)
        {
            if ((board[0] == player && board[1] == player && board[2] == player) ||
                (board[3] == player && board[4] == player && board[5] == player) ||
                (board[6] == player && board[7] == player && board[8] == player) ||
                (board[0] == player && board[3] == player && board[6] == player) ||
                (board[1] == player && board[4] == player && board[7] == player) ||
                (board[2] == player && board[5] == player && board[8] == player) ||
                (board[0] == player && board[4] == player && board[8] == player) ||
                (board[2] == player && board[4] == player && board[6] == player))
            {
                return true;
            }
            return false;
        }

        private bool IsMovesEnd(char[] board)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == ' ')
                {
                    return true;
                }
            }
            return false;
        }

        private int ValueLift(char[] board)
        {
            if (checkWin(board, 'X'))
            {
                return 10;
            }
            else if (checkWin(board, 'O'))
            {
                return -10;
            }
            return 0;
        }
        private void CountResults()
        {
            Wins.Content = $"Побед: {countPlayerWins}";
            Draws.Content = $"Ничей: {countDraws}";
            Defeats.Content = $"Поражений: {countComputerWins}";
        }
    }
}