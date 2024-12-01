using EnsureThat;
using Maybe.App.Serivces;
using Maybe.Domain.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Maybe.Api.Controllers
{
    [Route("maybe/[controller]")]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private readonly IdeasService _ideasService;

        public IdeasController(IdeasService ideasService) 
        {
            EnsureArg.IsNotNull(ideasService, nameof(ideasService));

            _ideasService = ideasService;
        }

        [HttpGet("getAll")]
        public IActionResult GetAll(bool? publicated = null)
        {
            try
            {
                return Ok(_ideasService.GetAll(publicated));
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при получении списка идей: {ex.Message}");
            }
        }

        [HttpGet("get/{id:guid}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var idea = _ideasService.Get(id);

                return idea is null
                    ? NotFound($"Идея с id: {id} не найдена")
                    : Ok(idea);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при получении идеи: {ex.Message}");
            }
        }

        [HttpGet("count")]
        public IActionResult Count(bool? publicated = null)
        {
            try
            {
                return Ok(_ideasService.Count(publicated));
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при получении количества идей: {ex.Message}");
            }
        }

        [HttpPost("addFromFile")]
        public IActionResult AddFromFile(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("Необходимо прикрепить файл");
            }

            try
            {
                var ideas = new List<Idea>();
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var fileContent = stream.ReadToEnd();
                    var pattern = @"✨\s*(.*?)\s*(?=\n🗣️)(.*?)\s*💬";

                    var matches = Regex.Matches(fileContent, pattern, RegexOptions.Singleline);

                    foreach (Match match in matches)
                    {
                        var blockParts = match.Value.Split("\r\n");
                        ideas.Add(new Idea(blockParts[0], blockParts[2], blockParts[4]));
                    }
                }

                if(_ideasService.AddFromFile(ideas))
                    _ideasService.SaveChanges();

                return Ok($"Добавлено идей: {ideas.Count}");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove/{id:guid}")]
        public IActionResult Remove(Guid id)
        {
            try
            {
                _ideasService.Remove(id);
                _ideasService.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при удалении идеи: {ex.Message}");
            }
        }

        [HttpDelete("removeAll")]
        public IActionResult RemoveAll()
        {
            try
            {
                _ideasService.RemoveAll();
                _ideasService.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при удалении всех идей: {ex.Message}");
            }
        }
    }
}
