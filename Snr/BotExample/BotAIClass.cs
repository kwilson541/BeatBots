using System;
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
        private static IDictionary<string, string[]> _winScenarios = new Dictionary<string, string[]>() {
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
            _opponentName = opponentName;
            _pointstoWin = pointstoWin;
            _maxRounds = maxRounds;
            _dynamite = dynamite;
            
            resetGameVariables();
        }

        internal static void resetGameVariables()
        {
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
        }


        /* Method called when move instruction is received requesting your move
         *
         * GET http://<your_bot_url>/move
         *
         */
        internal static string GetMove()
        {
            GetLastRoundResult();
            return "ROCK";

            if (_myMove == "DYNAMITE") {
                _dynamiteRemaining--;
            }
        }

        internal static void GetLastRoundResult()
        {

        }
    }
        
}
