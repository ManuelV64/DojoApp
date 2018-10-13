using DojoApp.Core.Contracts;
using DojoApp.Core.Entities;
using DojoApp.Core.Katas;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DojoApp.Web.Hubs
{
    public class GameOfLifeHub : Hub
    {
        private IGameOfLife _game;
        private static List<string> _playerIdList = new List<string>();

        public GameOfLifeHub(IGameOfLife game)
        {
            _game = game;
        }
        public async Task SendInitialGeneration(int percentLiveCels)
        {
            List<Coordinate2D> _newCellList;
            _newCellList = _game.RandomGeneration(percentLiveCels);
            await Clients.Caller.SendAsync("ReceiveInitialGeneration", _newCellList);
        }

        public async Task SendStartGame(List<Coordinate2D> liveCellCoordinateList)
        {
            List<Coordinate2D> _newCellList;
            List<Coordinate2D> _oldCellList= liveCellCoordinateList;
            bool _gameOver = false;

            string playerId = Guid.NewGuid().ToString("N");
            _playerIdList.Add(playerId);
            await Clients.Caller.SendAsync("ReceivePlayerId", playerId);

            int cnt = 0;
            while (_playerIdList.Contains(playerId))
            {
                _newCellList = _game.NextGeneration(_oldCellList);

                if (!Enumerable.SequenceEqual(_newCellList.OrderBy(HashCode => HashCode),
                                             _oldCellList.OrderBy(HashCode => HashCode)))
                {
                    System.Threading.Thread.Sleep(500);
                    await Clients.Caller.SendAsync("ReceiveNextGeneration", _newCellList);
                    _oldCellList = _newCellList;
                }
                else
                {
                    _gameOver = true;
                    SendStopGame(playerId);
                }
            }

            if (_gameOver)
                await Clients.Caller.SendAsync("ReceiveGameOver", " El juego ha terminado.");
            else
                await Clients.Caller.SendAsync("ReceiveGameStopped", " El juego se ha detenido.");
        }

        public void SendStopGame(string playerId)
        {
            _playerIdList.Remove(playerId);
        }
    }
}
