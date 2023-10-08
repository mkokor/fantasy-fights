using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using FantasyFights.BLL.DTOs.User;
using FantasyFights.BLL.Utilities;
using FantasyFights.DAL.Entities;
using FantasyFights.DAL.Other.Email;
using FantasyFights.DAL.Repositories.UnitOfWork;

namespace FantasyFights.BLL.Services.AuthenticationService
{
    public partial class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}