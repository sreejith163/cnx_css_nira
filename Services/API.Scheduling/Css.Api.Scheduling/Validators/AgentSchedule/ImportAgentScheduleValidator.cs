using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.Enums;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for handling the validation of import agent schedule object
    /// </summary>
    public class ImportAgentScheduleValidator : AbstractValidator<ImportAgentSchedule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportAgentScheduleValidator"/> class.
        /// </summary>
        public ImportAgentScheduleValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.ModifiedBy).NotEmpty();
            RuleFor(x => x.AgentScheduleType).IsInEnum();
            RuleFor(x => x.AgentScheduleCharts).SetValidator(new AgentScheduleChartValidator<AgentScheduleChart>())
                .When(x => x.AgentScheduleType == AgentScheduleType.SchedulingTab);
            RuleFor(x => x.AgentScheduleManager).NotEmpty().When(x => x.AgentScheduleType == AgentScheduleType.SchedulingMangerTab);
            RuleForEach(x => x.AgentScheduleManager.AgentScheduleManagerCharts).SetValidator(new AgentScheduleChartValidator<AgentScheduleChart>())
                .When(x => x.AgentScheduleType == AgentScheduleType.SchedulingMangerTab);
        }
    }
}