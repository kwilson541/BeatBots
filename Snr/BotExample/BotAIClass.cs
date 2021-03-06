﻿using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
using System.Security.Cryptography;



namespace BotExample
{
    internal static class BotAIClass
    {
        // opponent info
        private static string _opponentName;
        private static string _lastOpponentsMove;
        private static int _opponentDynamiteRemaining;
        
        // game info
        private static int _pointstoWin;
        private static int _maxRounds;
        private static int _dynamite;
        private static bool _newGame;
        private static string _lastRoundResult;

        // weapon info
        private static string[] _weapons = new string[] {"ROCK", "PAPER", "SCISSORS", "DYNAMITE", "WATERBOMB"};
        private static readonly IDictionary<string, string[]> _winScenarios = new Dictionary<string, string[]>() {
            {"ROCK", new string[] {"SCISSORS", "WATERBOMB"}},
            {"PAPER", new string[] {"ROCK", "WATERBOMB"}},
            {"SCISSORS", new string[] {"PAPER", "WATERBOMB"}},
            {"DYNAMITE", new string[] {"ROCK", "PAPER", "SCISSORS"}},
            {"WATERBOMB", new string[] {"DYNAMITE"}}
        };
        private static int _dynamiteRemaining;

        // my moves
        private static string _myMove;
        private static string _myLastMove;


        /* Method called when start instruction is received
         *
         * POST http://<your_bot_url>/start
         *
         */
        internal static void SetStartValues(string opponentName, int pointstoWin, int maxRounds, int dynamite)
        {
            _dynamite = dynamite;
            _pointstoWin = pointstoWin;
            _maxRounds = maxRounds;

            ResetGameVariables();

            _opponentName = opponentName;
        }

        private static void ResetGameVariables()
        {
            if (_opponentName != null) {
                AddLastRoundToLog();
            }

            _newGame = true;
            _lastRoundResult = null;
            _myMove = null;
            _myLastMove = null;
            _lastOpponentsMove = null;
            _dynamiteRemaining = _dynamite;
            _opponentDynamiteRemaining = _dynamite;
        }

        /* Method called when move instruction is received instructing opponents move
         *
         * POST http://<your_bot_url>/move
         *
         */
        public static void SetLastOpponentsMove(string lastOpponentsMove)
        {
            if (lastOpponentsMove == "DYNAMITE") {
                _opponentDynamiteRemaining--;
            }
            _lastOpponentsMove = lastOpponentsMove;

            SetMyLastMove();
            GetLastRoundResult();
            AddLastRoundToLog();
        }


        /* Method called when move instruction is received requesting your move
         *
         * GET http://<your_bot_url>/move
         *
         */
        internal static string GetMove()
        {
            if (_newGame)
            {
                StartNewGameInLog();
                _newGame = false;
            }

            SetRandomMove();

            if (_myMove == "DYNAMITE") {
                _dynamiteRemaining--;
            }

            return _myMove;
        }

        private static void StartNewGameInLog()
        {
            string path = $@"C:\dev\BeatBots.Log\{_opponentName}.txt";

            TextWriter textWriter = new StreamWriter(path, true);

            textWriter.WriteLine("----------NEW GAME----------");
            textWriter.Close();    
        }

        private static void AddLastRoundToLog()
        {
            string path = $@"C:\dev\BeatBots.Log\{_opponentName}.txt";
            string newLine = $"Me: {_myLastMove}, {_opponentName}: {_lastOpponentsMove}, Result: {_lastRoundResult}";

            TextWriter textWriter = new StreamWriter(path, true);

            textWriter.WriteLine(newLine);
            textWriter.Close();
        }

        private static void SetMyLastMove()
        {
            _myLastMove = _myMove;
        }

        private static void GetLastRoundResult()
        {
            if (_winScenarios[_myLastMove].Contains(_lastOpponentsMove)) {
                _lastRoundResult = "WIN";
            }
            else if (_myLastMove == _lastOpponentsMove) {
                _lastRoundResult = "TIE";
            }
            else {
                _lastRoundResult = "LOSE";
            }
        }

        private static void SetRandomMove()
        {
            int randomSeed;

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[4];
                rng.GetBytes(randomNumber);
                randomSeed = BitConverter.ToInt32(randomNumber, 0);
            }

            Random random = new Random(randomSeed);

            _myMove = _weapons[random.Next(0,4)];
        }
    }        
}
