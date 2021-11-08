using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Refactoring.LegacyService;
using Refactoring.LegacyService.Interfaces;
using System;
using Refactoring.JuniorApi.Models;
using Microsoft.AspNetCore.Http;
using Refactoring.LegacyService.Exceptions;

namespace Refactoring.JuniorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CandidatesController : ControllerBase
    {
        private readonly ILogger<CandidatesController> _logger;
        private readonly ICandidateService _candidateService;
        private readonly ICandidateDataAccess _dataAccessService;
        private readonly IPositionRepository _positionService;
        private readonly ICandidateCreditService _creditService;

        public CandidatesController(
            ILogger<CandidatesController> logger,
            ICandidateService candidateService,
            ICandidateDataAccess dataAccessService,
            IPositionRepository positionService,
            ICandidateCreditService creditService)
        {
            _logger = logger;
            _dataAccessService = dataAccessService;
            _positionService = positionService;
            _creditService = creditService;
            _candidateService = candidateService;

            _logger.LogInformation($"Initializing {nameof(CandidatesController)}");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(nameof(CandidatesController));
        }

        [HttpPost]
        public IActionResult Add(CandidateModel request)
        {
            try
            {
                _logger.LogInformation($"Adding candidate: {request}");
                _candidateService.AddCandidate(
                        request.Firstname,
                        request.Surname,
                        request.PositionId,
                        request.DateOfBirth,
                        request.EmailAddress);
                return Accepted();
            }
            catch (CreditCheckFailedException ex)
            {
                _logger.LogError($"Failed to add Candidate: {request}");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to add Candidate: {request}");
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
    }
}