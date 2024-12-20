﻿using AppyNox.Services.Sso.Application.DTOs.ClaimDtos.Models;
using AutoMapper;
using System.Security.Claims;

namespace AppyNox.Services.Sso.Application.DTOs.ClaimDtos.Mappings
{
    /// <summary>
    /// Represents the mapping configuration for Claim and ClaimDto objects. 
    /// This class is responsible for defining the AutoMapper mappings between these two types.
    /// </summary>
    public class ClaimMapping : Profile
    {
        #region [ Public Constructors ]

        public ClaimMapping()
        {
            CreateMap<ClaimDto, Claim>().ReverseMap();
        }

        #endregion
    }
}