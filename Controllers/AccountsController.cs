using AutoMapper;
using MeterReaderAPI.Entities;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MeterReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repository;
        
        private IMapper _mapper;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserRepository repository,
            IConfiguration configuration,
            IMapper mapper,
            ILogger<AccountsController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;   
            _configuration = configuration;            
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<ActionResult<AuthenticationResponse>> Create([FromBody] UserCredentials userCredentials)
        {
            try
            {
                var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
                var result = await _repository.Create(user, userCredentials);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Creating ${userCredentials.Email} as new year");
                    return Ok();
                }
                else
                {
                    _logger.LogError(result.Errors.ToString());
                    if(result.Errors.Count() == 0)
                    {
                        List<string> customErrorsArr = new List<string>();
                        customErrorsArr.Add("Email: Email allready exsist");
                        return BadRequest(customErrorsArr);
                    }

                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
        {
            try
            {
                _logger.LogInformation($"Creating ${userCredentials.Email} as new year");
                var result = await _repository.Login(userCredentials.Email, userCredentials.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"${userCredentials.Email} is logged in");
                    return await BuildToken(userCredentials);
                }
                else
                {
                    return BadRequest("Incorrect Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        private async Task<AuthenticationResponse> BuildToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email",userCredentials.Email)
            };

            var user = await _userManager.FindByNameAsync(userCredentials.Email);
            var claimsDB = await _userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: creds);

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
