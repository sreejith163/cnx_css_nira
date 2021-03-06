﻿using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using FluentValidation;

namespace Css.Api.Setup.Validators.ClientLOBGroup
{
    /// <summary>Validator for handling the validation of update client lob group object</summary>
    public class UpdateClientLOBGroupValidator : AbstractValidator<UpdateClientLOBGroup>
    {
        /// <summary>Initializes a new instance of the <see cref="UpdateClientLOBGroupValidator" /> class.</summary>
        public UpdateClientLOBGroupValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.TimezoneId).NotEmpty();
            RuleFor(x => x.FirstDayOfWeek).NotNull();
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}
