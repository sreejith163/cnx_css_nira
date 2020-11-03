﻿using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.ClientLOBGroup
{
    /// <summary>Validator for handling the validation of add client lob group object</summary>
    public class AddClientLOBGroupValidator : AbstractValidator<CreateClientLOBGroup>
    {
        /// <summary>Initializes a new instance of the <see cref="AddClientLOBGroupValidator" /> class.</summary>
        public AddClientLOBGroupValidator()
        {
            RuleFor(x => x.name).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}