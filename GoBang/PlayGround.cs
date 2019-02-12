﻿using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GammaLibrary.Extensions;

namespace GoBang
{
    public enum Role
    {
        White,
        Black
    }
    public class PlayGround
    {
        private readonly Point[,] _chessBoard = new Point[15, 15];
        private string BlackChessman = "B";
        private string WhiteChessman = "W";
        private string Empty = "—";
        public Role role { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="black">棋盘上黑棋的字符</param>
        /// <param name="white">棋盘上白棋的字符</param>
        /// <param name="empty">棋盘上空位的字符</param>
        public PlayGround(string black, string white, string empty)
        {
            BlackChessman = black;
            WhiteChessman = white;
            Empty = empty;
            RandomizeFirst();
            for (var i = 0; i < 15; i++)
                for (var j = 0; j < 15; j++)
                    _chessBoard[i, j] = new Point(i, j, Chessman.Empty);
        }
        public PlayGround()
        {
            RandomizeFirst();
            for (var i = 0; i < 15; i++)
            for (var j = 0; j < 15; j++)
                _chessBoard[i, j] = new Point(i, j, Chessman.Empty);
        }

        private void RandomizeFirst()
        {
            var rnd = new Random().Next(2);
            role = rnd == 0 ? Role.White : Role.Black;
        }

        public Winner Check(Point coordinate)
        {
            var checker = new Checker(_chessBoard);
            return checker.Check(coordinate);
        }

        public string GetChessBoard()
        {
            var stringBuilder = new StringBuilder();
            for (var i = 14; i >= 0; i--)
            {
                for (var j = 0; j < 15; j++)
                    stringBuilder.Append(_chessBoard[j, i].Chessman == Chessman.Black
                        ? BlackChessman
                        : _chessBoard[j, i].Chessman == Chessman.White
                            ? WhiteChessman
                            : Empty);
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        public void WhiteGo(int x, int y)
        {
            role = Role.Black;
            _chessBoard[x, y] = new Point(x, y, Chessman.White);
        }

        public void BlackGo(int x, int y)
        {
            role = Role.White;
            _chessBoard[x, y] = new Point(x, y, Chessman.Black);
        }

        public bool CheckChessBoardIsFull()
        {
            for (var i = 0; i < 15; i++)
                for (var j = 0; j < 15; j++)
                    if (_chessBoard[i, j].Chessman == Chessman.Empty)
                        return false;
            return true;
        }

        public bool CheckPointIsRational(int x, int y)
        {
            return x <= 14 && y <= 14 && _chessBoard[x, y].Chessman == Chessman.Empty;
        }

        public string Go(int x, int y)
        {
            if (CheckPointIsRational(x, y))
            {
                return "诶呀,您落子的位置好像不太合理,再检查一次吧?";
            }
            switch (role)
            {
                case Role.White:
                    WhiteGo(x, y);
                    return "白方成功落子";
                case Role.Black:
                    BlackGo(x, y);
                    return "黑方成功落子";
                default:
                    throw new ArgumentOutOfRangeException();
            }           
        }

        public string IsWin(int x, int y)
        {
            switch (Check(_chessBoard.GetPoint(x, y)))
            {
                case Winner.Black:
                    return "黑方获胜!";
                case Winner.White:
                    return "白方获胜!";
                case Winner.None:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void Main(string[] args)
        {
            var playGround = new PlayGround();
            while (true)
            {
                Console.WriteLine(playGround.GetChessBoard());
                switch (playGround.role)
                {
                    case Role.Black:
                        Console.WriteLine("黑方走子");
                        break;
                    case Role.White:
                        Console.WriteLine("白方走子");
                        break;
                }

                Console.WriteLine("请输入X");
                var stringx = Console.ReadLine();
                Console.WriteLine("请输入Y");
                var stringy = Console.ReadLine();

                if (!stringx.IsInt() || !stringy.IsInt() || stringx == null || stringy == null)
                {
                    Console.WriteLine("输入格式非法");
                    continue;
                }
                var x = int.Parse(stringx);
                var y = int.Parse(stringy);
                if (playGround.CheckChessBoardIsFull())
                {
                    Console.WriteLine("棋盘上已经没有地方了,和棋!");
                    break;
                }
                Console.WriteLine(playGround.Go(x, y));
                var result = playGround.IsWin(x, y);
                if (result.IsNullOrEmpty())
                {
                    break;
                }
                Console.WriteLine(result);
            }
            Console.WriteLine("游戏结束");
            Console.ReadKey();
        }
    }
}
