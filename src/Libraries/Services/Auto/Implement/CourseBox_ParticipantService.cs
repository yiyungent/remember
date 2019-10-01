// Code generated by a template
// Project: Remember
// LastUpadteTime: 2019-10-01 11:35:54
using Domain.Entities;
using Repositories.Interface;
using Services.Core;
using Services.Interface;

namespace Services.Implement
{
    public partial class CourseBox_ParticipantService : BaseService<CourseBox_Participant>, ICourseBox_ParticipantService
    {
        private readonly ICourseBox_ParticipantRepository _repository;
        public CourseBox_ParticipantService(ICourseBox_ParticipantRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}