using Application.Service;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
  [Route("consumer")]
  [Authorize]
  [ApiController]
  public class ConsumerController : Controller
  {
    private readonly IConsumerService _service;
    private readonly ILogger _logger;

    public ConsumerController(IConsumerService service, ILogger logger) {
      _service = service;
      _logger = logger;
    }

    /// <summary>
    /// Endpoint responsável por criar um perfil de consumidor com os dados do usuário.
    /// </summary>
    /// <returns>Valida os dados passados para criação de perfil de consumidor e retorna o perfil cadastrado.</returns>
    [AllowAnonymous]
    [HttpPost("create")]
    [ProducesResponseType(typeof(Response<CreateConsumerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<CreateConsumerResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<CreateConsumerResponse>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Response<CreateConsumerResponse>), StatusCodes.Status500InternalServerError)]
    public ActionResult<Response<CreateConsumerResponse>> CreateConsumer([FromBody] CreateConsumerRequest createConsumerRequest)
    {
      try
      {
        var response = _service.CreateConsumerService(createConsumerRequest);
        return StatusCode(StatusCodes.Status200OK, new Response<CreateConsumerResponse>() { Status = 200, Message = $"Perfil de Consumidor registrado com sucesso.", Data = response, Success = true });
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Exception while creating new consumer!");
        switch (ex.Message)
        {
          case "consumerExistsToUser":
            return StatusCode(StatusCodes.Status409Conflict, new Response<CreateConsumerResponse>() { Status = 409, Message = $"Perfil de consumidor já registrado para esse usuário.", Success = false });
          case "consumerNotCreated":
            return StatusCode(StatusCodes.Status304NotModified, new Response<CreateConsumerResponse>() { Status = 304, Message = $"Perfil de consumidor não registrado.", Success = false });
          default:
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<CreateConsumerResponse>() { Status = 500, Message = $"Internal server error! Exception Detail: {ex.Message}", Success = false });
        }   
      }
    }

    /// <summary>
    /// Endpoint responsável por atualizar os dados do perfil consumidor.
    /// </summary>
    /// <returns>Valida os dados passados e atualiza o perfil de consumidor, retornando o perfil com os dados atualizados.</returns>
    [HttpPut("update")]
    [ProducesResponseType(typeof(Response<UpdateConsumerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<UpdateConsumerResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<UpdateConsumerResponse>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Response<UpdateConsumerResponse>), StatusCodes.Status500InternalServerError)]
    public ActionResult<Response<UpdateConsumerResponse>> UpdateConsumerService([FromBody] UpdateConsumerRequest updateConsumerRequest)
    {
      try
      {
        var response = _service.UpdateConsumerService(updateConsumerRequest);
        return StatusCode(StatusCodes.Status200OK, new Response<UpdateConsumerResponse>() { Status = 200, Message = $"Perfil de Consumidor atualizado com sucesso.", Data = response, Success = true });
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Exception while updating consumer!");
        switch (ex.Message)
        {
          case "consumerNotExists":
            return StatusCode(StatusCodes.Status404NotFound, new Response<UpdateConsumerResponse>() { Status = 404, Message = $"Perfil de consumidor não existe com o id informado.", Success = false });
          case "consumerNotUpdated":
            return StatusCode(StatusCodes.Status304NotModified, new Response<UpdateConsumerResponse>() { Status = 304, Message = $"Perfil de consumidor não atualizado.", Success = false });
          default:
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<UpdateConsumerResponse>() { Status = 500, Message = $"Internal server error! Exception Detail: {ex.Message}", Success = false });
        }   
      }
    }

    /// <summary>
    /// Endpoint responsável por retornar todos os perfis de consumidor registrados.
    /// </summary>
    /// <returns>Retorna uma lista de perfis de consumidor registrados.</returns>
    [HttpGet("all")]
    [ProducesResponseType(typeof(Response<List<Consumer>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<List<Consumer>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<List<Consumer>>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Response<List<Consumer>>), StatusCodes.Status500InternalServerError)]
    public ActionResult<Response<List<Consumer>>> GetConsumers()
    {
      try
      {
        var response = _service.GetConsumers();
        return StatusCode(StatusCodes.Status200OK, new Response<List<Consumer>>() { Status = 200, Message = $"Consumidores retornados com sucesso.", Data = response, Success = true });
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Exception while retrieve consumers!");
        switch (ex.Message)
        {
          default:
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<List<Consumer>>() { Status = 500, Message = $"Internal server error! Exception Detail: {ex.Message}", Success = false });
        }   
      }
    }

    /// <summary>
    /// Endpoint responsável por retornar um perfil de usuario por id.
    /// </summary>
    /// <returns>Valida os dados passados e retorna os dados do perfil de consumidor.</returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(Response<Consumer>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<Consumer>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<Consumer>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Response<Consumer>), StatusCodes.Status500InternalServerError)]
    public ActionResult<Response<Consumer>> GetConsumerByUserId([FromQuery] Guid user_id)
    {
      try
      {
        var response = _service.GetConsumerByUserId(user_id);
        return StatusCode(StatusCodes.Status200OK, new Response<Consumer>() { Status = 200, Message = $"Perfil de consumidor retornado com sucesso.", Data = response, Success = true });
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Exception while creating new session!");
        switch (ex.Message)
        {
          default:
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<Consumer>() { Status = 500, Message = $"Internal server error! Exception Detail: {ex.Message}", Success = false });
        }   
      }
    }
  }
}
