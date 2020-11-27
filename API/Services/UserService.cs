using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using API.Helpers;
using API.Models;
using Domain;
using Persistence;

namespace API.Services
{
    public interface IUserService
    {
        AuthenticateResponse Login(AuthenticateRequest req);
        IEnumerable<User> GetAll();
    
        User GetById(Guid id);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public UserService(DataContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Login(AuthenticateRequest req)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == req.Email);

            // check if email exists
            if  (user == null)
                return null;

            // check if password is correct
            // add password hash matching here
            if (user.Password != req.Password)
            {
                return null;
            }
            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(token);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}