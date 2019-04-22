using DojoApp.Core.Contracts;
using DojoApp.Core.Entities;
using DojoApp.Web.DTOs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DojoApp.Web.Hubs
{
    public class LifeGameHub : Hub
    {
        private static ConcurrentDictionary<string, GenerationOfCellsDto> LastGenerationDTO = new ConcurrentDictionary<string, GenerationOfCellsDto>();

        private IGameOfLife _game;
        public LifeGameHub(IGameOfLife game) {
            _game = game;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ReceiveConnectionId", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (LastGenerationDTO.ContainsKey(Context.ConnectionId)){
                LastGenerationDTO.TryRemove(Context.ConnectionId, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendConnectionId() {
            await Clients.Caller.SendAsync("ReceiveConnectionId", Context.ConnectionId);
        }

        public async Task SendInitialGeneration(int percentLiveCels)
        {
            List<Coordinate2D> _newCellList;
            _newCellList = _game.RandomGeneration(percentLiveCels);

            GenerationOfCellsDto dto = new GenerationOfCellsDto() {
                liveCellList = _newCellList ,
                genNumber = 0
            };

            LastGenerationDTO.AddOrUpdate(Context.ConnectionId, dto, (key, existingVal) => {
                existingVal = dto;
                return existingVal;
            });

            await Clients.Caller.SendAsync("ReceiveGeneration", dto);
        }

        public async Task SendNextGeneration()
        {
            GenerationOfCellsDto oldGeneration;
            LastGenerationDTO.TryGetValue(Context.ConnectionId, out oldGeneration);

            List<Coordinate2D> oldCellList = oldGeneration.liveCellList;
            List<Coordinate2D> newCellList;

            newCellList = _game.NextGeneration(oldCellList);

            if (Enumerable.SequenceEqual(newCellList.OrderBy(HashCode => HashCode),
                                         oldCellList.OrderBy(HashCode => HashCode)))
            {
                await Clients.Caller.SendAsync("ReceiveGameOver");
            }
            else
            {
                GenerationOfCellsDto newGeneration = new GenerationOfCellsDto() {
                    liveCellList= newCellList,
                    genNumber = ++oldGeneration.genNumber 
                };

                LastGenerationDTO.AddOrUpdate(Context.ConnectionId, newGeneration, (key, existingVal) => {
                    existingVal = newGeneration;
                    return existingVal;
                });
                await Clients.Caller.SendAsync("ReceiveGeneration", newGeneration);
            }
        }
    }
}
