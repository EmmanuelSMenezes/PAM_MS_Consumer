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
    [Route("address")]
    [ApiController]
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IAddressService _service;
        private readonly ILogger _logger;

        public AddressController(IAddressService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint responsável por criar uma endereço vinculado a um usuário do tipo consumidor.
        /// </summary>
        /// <returns>Valida os dados passados para criação do endereço e retorna os dados do endereço.</returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(Response<CreateAddressResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<CreateAddressResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<CreateAddressResponse>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<CreateAddressResponse>), StatusCodes.Status500InternalServerError)]
        public ActionResult<Response<CreateAddressResponse>> CreateAddress([FromBody] CreateAddressRequest createAddressRequest)
        {
            try
            {
                var response = _service.CreateAddressService(createAddressRequest);
                return StatusCode(StatusCodes.Status200OK, new Response<CreateAddressResponse>() { Status = 200, Message = $"Endereço cadastrado com sucesso", Data = response, Success = true });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while creating new address!");
                switch (ex.Message)
                {
                    case "consumerNotExists":
                        return StatusCode(StatusCodes.Status412PreconditionFailed, new Response<CreateAddressResponse>() { Status = 412, Message = $"Não foi possível criar o endereço. Não existe consumidor para o consumer_id informado.", Success = false, Error = ex.Message });
                    case "addressNotCreated":
                        return StatusCode(StatusCodes.Status304NotModified, new Response<CreateAddressResponse>() { Status = 412, Message = $"Não foi possível criar o endereço. Tente Novamente.", Success = false, Error = ex.Message });
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response<CreateAddressResponse>() { Status = 500, Message = $"Internal server error! Exception Detail: {ex.Message}", Success = false, Error = ex.Message });
                }
            }
        }

        /// <summary>
        /// Endpoint responsável por buscar todos os endereços de um usuário do tipo consumidor.
        /// </summary>
        /// <returns>Valida os dados passados e retorna os endereços do consumidor solicitado.</returns>
        [HttpGet("all")]
        [ProducesResponseType(typeof(Response<List<Address>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<List<Address>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<List<Address>>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<List<Address>>), StatusCodes.Status500InternalServerError)]
        public ActionResult<Response<List<Address>>> GetAdressesByConsumerId([FromQuery] Guid consumer_id)
        {
            try
            {
                var response = _service.GetAdressesByConsumerId(consumer_id);
                return StatusCode(StatusCodes.Status200OK, new Response<List<Address>>() { Status = 200, Message = $"Endereços retornados com sucesso", Data = response, Success = true });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while retrieve adresses!");
                switch (ex.Message)
                {
                    case "consumerNotExists":
                        return StatusCode(StatusCodes.Status412PreconditionFailed, new Response<List<Address>>() { Status = 412, Message = $"Não foi possível listar endereços. Não existe usuário do tipo consumidor com o id informado.", Success = false });
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response<List<Address>>() { Status = 500, Message = $"Internal server error! Exception Detail: {ex.Message}", Success = false });
                }
            }
        }

        /// <summary>
        /// Endpoint responsável por buscar um endereço pelo seu id.
        /// </summary>
        /// <returns>Valida os dados passados e busca um endereço por id.</returns>
        [HttpGet("get")]
        [ProducesResponseType(typeof(Response<Address>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<Address>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<Address>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<Address>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<Address>), StatusCodes.Status500InternalServerError)]
        public ActionResult<Response<Address>> GetAddressByAddressId([FromQuery] Guid address_id)
        {
            try
            {
                var response = _service.GetAddressByAddressId(address_id);
                return StatusCode(StatusCodes.Status200OK, new Response<Address>() { Status = 200, Message = $"Endereço retornado com sucesso.", Data = response, Success = true });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while retrieve address!");
                switch (ex.Message)
                {
                    case "addressNotExists":
                        return StatusCode(StatusCodes.Status404NotFound, new Response<Address>() { Status = 404, Message = $"Endereço não encontrado para o id informado.", Success = false });
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response<Address>() { Status = 500, Message = $"Internal server error! Exception Detail: {ex.Message}", Success = false });
                }
            }
        }

        /// <summary>
        /// Endpoint responsável por deletar um ou mais endereços.
        /// </summary>
        /// <returns>Valida os dados passados para deleçao dos endereços e retorna um status de ok se obter sucesso na exclusão.</returns>
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status500InternalServerError)]
        public ActionResult<Response<bool>> DeleteAddress([FromBody] List<Guid> ids)
        {
            try
            {
                var response = _service.DeleteAddress(ids);
                return StatusCode(StatusCodes.Status200OK, new Response<bool>() { Status = 200, Message = $"Endereços deletados com sucesso.", Data = response, Success = true });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while delete address!");
                switch (ex.Message)
                {
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>() { Status = 500, Message = $"Internal server error! Exception Detail: {ex.Message}", Success = false });
                }
            }
        }

        /// <summary>
        /// Endpoint responsável por atualizar um endereço.
        /// </summary>
        /// <returns>Valida os dados passados para atualização do endereço e retorna os dados do endereço atualizado.</returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(Response<UpdateAddressResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<UpdateAddressResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<UpdateAddressResponse>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<UpdateAddressResponse>), StatusCodes.Status500InternalServerError)]
        public ActionResult<Response<UpdateAddressResponse>> UpdateAddress([FromBody] UpdateAddressRequest updateAddressRequest)
        {
            try
            {
                var response = _service.UpdateAddress(updateAddressRequest);
                return StatusCode(StatusCodes.Status200OK, new Response<UpdateAddressResponse>() { Status = 200, Message = $"Endereço Atualizado com Sucesso.", Data = response, Success = true });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while updating address!");
                switch (ex.Message)
                {
                    case "addressNotExists":
                        return StatusCode(StatusCodes.Status404NotFound, new Response<UpdateAddressResponse>() { Status = 403, Message = $"Não foi possível atualizar o endereço. Endereço com o email informado não existe.", Success = false });
                    case "addressNotUpdated":
                        return StatusCode(StatusCodes.Status304NotModified, new Response<UpdateAddressResponse>() { Status = 403, Message = $"Endereço não atualizado. Tente novamente.", Success = false });
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response<UpdateAddressResponse>() { Status = 500, Message = $"Internal server error! Exception Detail: {ex.Message}", Success = false });
                }
            }
        }
    }
}
