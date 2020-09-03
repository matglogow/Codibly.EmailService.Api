﻿using System;
using AutoMapper;
using Codibly.EmailService.Api.Dtos.Dtos;
using Codibly.EmailService.Api.Dtos.Enums;
using EmailModel = Codibly.EmailService.Api.Models.Models.Email;
using EmailStateEnumModel = Codibly.EmailService.Api.Models.Models.Enums.EmailStateEnum;

namespace Codibly.EmailService.Api.Dtos
{
    public class MappingProfile : Profile
    {
        #region Construction

        public MappingProfile()
        {
            CreateEnumsMapping();
            CreateEmailHeaderDtoMapping();
            CreateEmailCreateableDtoMapping();
        }

        #endregion

        #region Private methods

        private void CreateEmailCreateableDtoMapping()
        {
            CreateMap<EmailCreateableDto, EmailModel>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Recipients, opt => opt.Ignore())
                .ForMember(m => m.State, opt => opt.MapFrom(o => EmailStateEnumModel.Pending))
                .ForMember(m => m.CreatedBy, opt => opt.MapFrom(o => o.Sender))
                .ForMember(m => m.CreatedOn, opt => opt.MapFrom(o => DateTimeOffset.UtcNow));
        }

        private void CreateEmailHeaderDtoMapping()
        {
            CreateMap<EmailModel, EmailHeaderDto>();
        }

        private void CreateEnumsMapping()
        {
            CreateMap<EmailStateEnumModel, EmailStateEnumDto>().ConvertUsing(s => (EmailStateEnumDto)s);
            CreateMap<EmailStateEnumDto, EmailStateEnumModel>().ConvertUsing(s => (EmailStateEnumModel)s);
        }

        #endregion
    }
}
