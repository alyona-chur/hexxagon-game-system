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
using System.Net;
using System.Net.Sockets;
using System.IO;
using ConstantsLibrary;

namespace HexxagonVisualizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int click = 0;

        Board board = new Board();

        byte[] data = new byte[Constants.SIZE_OF_BYTES_NULL];
        ClientClass server = new ClientClass();
        int GameType =  Constants.GAME_TYPE_NULL;

        public MainWindow()
        {
            InitializeComponent();

            PrepareForFirstClick();
            DrawBoard();

            AgentUser.IsEnabled = false;
        }

        private void NexttButton_Click(object sender, RoutedEventArgs e)
        {
            #region First click
            if (click == 0) //Выбор типа игры
            {
                if (TwoAgents.IsChecked == true)
                {
                    GameType = Constants.GAME_TYPE_TWO_AGENTS;
                }
                else
                {
                    GameType = Constants.GAME_TYPE_AGENT_USER;
                }
                
                server.FirstConnection_ChangeMyId();
                AfterSecondClick();
            }
            #endregion
            #region Game
            else //Игра
            {
                NextButton.Content = "Next";

                click++;
                    server.SendConf();
                    data = server.GetStatus(); //Получение текущего статуса

                    if (GameOverCheck(data))
                        GameOver(data);
                    else
                        board.ChangeStatus(data);

                Player1Label.Content = "Player 1: " + board.GetPlayerScore(1).ToString();
                Player2Label.Content = "Player 2: " + board.GetPlayerScore(2).ToString();
                    DrawBoard();
            }
            click++;
            #endregion
        }

        #region Fuctions

        public void DrawBoard()
        {
            //Brushes
            SolidColorBrush boardBrush = new SolidColorBrush();
            boardBrush.Color = Colors.Black;
            SolidColorBrush emptyBrush = new SolidColorBrush();
            emptyBrush.Color = Color.FromRgb(145, 30, 66);
            SolidColorBrush player1Brush = new SolidColorBrush();
            player1Brush.Color = Colors.Red;
            SolidColorBrush player2Brush = new SolidColorBrush();
            player2Brush.Color = Colors.Blue;

            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    if (board.Cells[i, j].Type != Constants.CELL_NOT_EXIST || board.Cells[i, j].Type != Constants.CELL_EMPTY)
                    {
                        //Create
                        Polygon hex = new Polygon();
                        hex.Stroke = boardBrush;
                        hex.StrokeThickness = 3;

                        if (board.Cells[i, j].Type == Constants.CELL_EMPTY)
                            hex.Fill = emptyBrush;
                        if (board.Cells[i, j].Type == Constants.CELL_PLAYER1)
                            hex.Fill = player1Brush;
                        if (board.Cells[i, j].Type == Constants.CELL_PLAYER2)
                            hex.Fill = player2Brush;

                        //Set Points
                        hex.Points = board.Cells[i, j].PointsCollect;

                        //Add
                        Grid.SetRow(hex, 1);
                        Grid.SetColumn(hex, 0);
                        MainGrid.Children.Add(hex);
                    }
                }
            }
        }

        #region Control elements

        // Show|Hide elements
        public void PrepareForFirstClick()
        {
            NextButton.Content = "Next";
            quest1.Foreground = Brushes.Black;
            Player1Label.Visibility = Visibility.Hidden;
            Player2Label.Visibility = Visibility.Hidden;
        }
        public void AfterSecondClick()
        {
            NextButton.Content = "Start";

            quest1.Visibility = Visibility.Hidden;

            TwoAgents.Visibility = Visibility.Hidden;
            AgentUser.Visibility = Visibility.Hidden;
            Player1Label.Visibility = Visibility.Visible;
            Player2Label.Visibility = Visibility.Visible;
        }

        public void EndState()
        {
            NextButton.Visibility = Visibility.Hidden;
            //TODO: don't forget to close everything
        }

        #endregion

        // Проверка конца
        public bool GameOverCheck(byte[] gameStatus)
        {
            bool gameOver = true;
            for (int i = 0; i < 5; ++i)
            {
                if (gameStatus[i] != Constants.CMD_GAME_OVER[i])
                {
                    gameOver = false;
                    break;
                }
            }
            return gameOver;
        }
        // Конец игры
        public void GameOver(byte[] gameStatus)
        {
            EndState();
        }

        #endregion
    }
}
