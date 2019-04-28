using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DojoApp.Core.Contracts;
using DojoApp.Core.Entities;
using DojoApp.Web.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DojoApp.Web.Controllers
{
 // [Route("api/[controller]")]
 // [Route("[controller]/[Action]")]
    [Route("LifeGame")]
    [ApiController]

    public class LifeGameController : ControllerBase
    {
        private IGameOfLife _game;
        private static long gameId = 0;
        private static ConcurrentDictionary<long, GenerationOfCellsDto> LastGenerationDTO = new ConcurrentDictionary<long, GenerationOfCellsDto>();

        public LifeGameController(IGameOfLife game) {
            _game = game;
        }

        // GET: api/LifeGame
        [HttpGet("GameId")]
        public ActionResult<long> GetGameId() {
            return Ok(++gameId);
        }

        // GET: LifeGame/InitialGeneration/5,17
        [HttpGet("InitialGeneration/{gameId},{percentLiveCels}")]
        public ActionResult<GenerationOfCellsDto> GetInitialGeneration(long gameId, int percentLiveCels)
        {
            string errMessage = "";
            if (gameId.Equals(null)) {
                errMessage = "gameId no encontrado";
            };

            if (percentLiveCels < 1 || percentLiveCels > 99) {
                errMessage = "El valor de % de celulas vivas es incorrecto.";
            }

            if (errMessage != "") {
                return BadRequest(new AlertDto(
                    status: (int)HttpStatusCode.BadRequest,
                    header: "Life Game.",
                    message: errMessage));
            }
            //return StatusCode((int)HttpStatusCode.NoContent, errMessage);                

            List<Coordinate2D> _newCellList;
            _newCellList = _game.RandomGeneration(percentLiveCels);

            GenerationOfCellsDto dto = new GenerationOfCellsDto()
            {
                liveCellList = _newCellList,
                genNumber = 0
            };

            LastGenerationDTO.AddOrUpdate(gameId, dto, (key, existingVal) =>
            {
                existingVal = dto;
                return existingVal;
            });
            return Ok(dto);
        }

        // GET: LifeGame/InitialGeneration/5
        [HttpGet("NextGeneration/{gameId}")]
        public async Task<ActionResult<GenerationOfCellsDto>> GetNextGeneration(long gameId)
        {
            //string gameId = HttpContext.Request?.Headers["gameId"];
            if (gameId.Equals(null)) {
                return BadRequest(new AlertDto(
                    status: (int)HttpStatusCode.BadRequest,
                    header: "Life Game.",
                    message: "gameId no encontrado"));
            }

            GenerationOfCellsDto oldGeneration;
            LastGenerationDTO.TryGetValue(gameId, out oldGeneration);

            List<Coordinate2D> oldCellList = oldGeneration.liveCellList;
            List<Coordinate2D> newCellList;

            newCellList = await Task.Run(() => _game.NextGeneration(oldCellList));


            if (Enumerable.SequenceEqual(newCellList.OrderBy(HashCode => HashCode),
                                         oldCellList.OrderBy(HashCode => HashCode)))
            {
                return Ok(new GenerationOfCellsDto() {
                    genNumber = oldGeneration.genNumber,
                    liveCellList = oldCellList,
                    gameOver = true
                });
            }
            else
            {
                GenerationOfCellsDto newGeneration = new GenerationOfCellsDto() {
                    liveCellList = newCellList,
                    genNumber = ++oldGeneration.genNumber
                };

                LastGenerationDTO.AddOrUpdate(gameId, newGeneration, (key, existingVal) => {
                    existingVal = newGeneration;
                    return existingVal;
                });
                return Ok(newGeneration);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("DeleteGame/{gameId}")]
        public IActionResult DeleteGame(long gameId)
        {
            if (LastGenerationDTO.ContainsKey(gameId)) {
                LastGenerationDTO.TryRemove(gameId, out _);
            }
            else {
                return BadRequest(new AlertDto(
                    status: (int)HttpStatusCode.BadRequest,
                    header: "Life Game.",
                    message: "gameId no encontrado"));
            }
            return NoContent();
        }
    }
}
