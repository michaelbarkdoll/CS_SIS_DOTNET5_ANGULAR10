using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class CoursesController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;
        private readonly IFileRepoService fileRepoService;
        private readonly DataContext context;
        
        public CoursesController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService, IFileRepoService fileRepoService, DataContext context)
        {
            this.context = context;
            this.fileRepoService = fileRepoService;
            this.photoService = photoService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-semesters-paged", Name = "GetSemestersPaged")]
        public async Task<ActionResult<IEnumerable<Semester>>> GetSemestersPaged([FromQuery]UserParams userParams) 
        {
            userParams.CurrentUserName = User.GetUsername();    // From Token

            var semesters = await this.unitOfWork.CoursesRepository.GetSemestersPagedAsync(userParams);
            
            Response.AddPaginationHeader(semesters.CurrentPage, semesters.PageSize, semesters.TotalCount, semesters.TotalPages);

            return Ok(semesters);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-courses-paged", Name = "GetCoursesPaged")]
        // public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses([FromQuery]UserParams userParams) 
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesPaged([FromQuery]UserParams userParams) 
        {
            userParams.CurrentUserName = User.GetUsername();    // From Token

            var courses = await this.unitOfWork.CoursesRepository.GetCoursesPagedAsync(userParams);
            
            Response.AddPaginationHeader(courses.CurrentPage, courses.PageSize, courses.TotalCount, courses.TotalPages);

            return Ok(courses);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("create-semester", Name = "CreateSemester")]
        // public async Task<ActionResult<MessageDto>> CreateCourse(CreateCourseDto createCourseDto)
        // public async Task<ActionResult<Semester>> CreateSemester(Semester createSemesterDto)
        public async Task<ActionResult<Semester>> CreateSemester(SemesterDto createSemesterDto)
        {
            var username = User.GetUsername();

            


            var semester = new Semester {
                Year = createSemesterDto.Year,
                Term = createSemesterDto.Term
                // Year = 2021,
                // Term = "Spring"
            };


            // this.unitOfWork.MessageRepository.AddMessage(message);
            this.unitOfWork.CoursesRepository.AddSemester(semester);

            if (await this.unitOfWork.Complete())
                return Ok(this.mapper.Map<Semester>(semester));

            return BadRequest("Failed to create semester");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-semester/{id}", Name = "DeleteSemester")]
        // public async Task<ActionResult<MessageDto>> CreateCourse(CreateCourseDto createCourseDto)
        public async Task<ActionResult<Semester>> DeleteSemester(int id)
        {
            var username = User.GetUsername();

            var semester = await this.unitOfWork.CoursesRepository.GetSemesterAsync(id);

            this.unitOfWork.CoursesRepository.DeleteSemester(semester);

            if (await this.unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed to delete semester");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-course", Name = "CreateCourse")]
        // public async Task<ActionResult<MessageDto>> CreateCourse(CreateCourseDto createCourseDto)
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseDto createCourseDto)
        {
            var username = User.GetUsername();

            // if (username == createCourseDto.RecipientUsername.ToLower())
            //     return BadRequest("You cannot send messages to yourself");

            // var sender = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            // var recipient = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            // if (recipient == null)
            //     return NotFound();

            var semester = await this.unitOfWork.CoursesRepository.GetSemesterAsync(1);
            
            var course = new Course {
                TA = "Sample TA",
                FullCourseDescription = "CS200B 001",
                Semester = semester,
                SemesterId = semester.Id
                /*
                public int Id { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime EndDate { get; set; }
                public string TA { get; set; }
                public string FullCourseDescription { get; set; }
                public Semester Semester { get; set; }
                public string CourseTitle { get; set; }
                public string College { get; set; }
                public string Department { get; set; }
                public string CRN { get; set; }
                public string Building { get; set; }
                public string ClassFormat { get; set; }
                public string ClassMeetingTimes { get; set; }
                public int BeginTime1 { get; set; }
                public int EndTime1 { get; set; }
                public int BeginTime2 { get; set; }
                public int EndTime2 { get; set; }
                public int BeginTime3 { get; set; }
                public int EndTime3 { get; set; }
                public string CourseStatus { get; set; }
                public int CourseCredits { get; set; }
                public DateTime WithdrawlDropDate { get; set; }
                public string Prerequisites { get; set; }
                public int RequiredSeqNum { get; set; }
                public string Site { get; set; }
                public ICollection<CourseStudent> CourseStudents { get; set; }
                public ICollection<Instructor> Instructors { get; set; }
                */
            };

            


            // var message = new Message
            // {
            //     Sender = sender,
            //     Recipient = recipient,
            //     SenderUsername = sender.UserName,
            //     RecipientUsername = recipient.UserName,
            //     Content = createCourseDto.Content
            // };

            // this.unitOfWork.MessageRepository.AddMessage(message);
            this.unitOfWork.CoursesRepository.AddCourse(course);

            // semester.Courses.Add(course)

            if (await this.unitOfWork.Complete())
                return Ok(this.mapper.Map<CourseDto>(course));

            return BadRequest("Failed to create course");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-course/{id}", Name = "DeleteCourse")]
        // public async Task<ActionResult<MessageDto>> CreateCourse(CreateCourseDto createCourseDto)
        public async Task<ActionResult<Semester>> DeleteCourse(int id)
        {
            var username = User.GetUsername();

            var course = await this.unitOfWork.CoursesRepository.GetCourseAsync(id);

            if(course != null) {
                this.unitOfWork.CoursesRepository.DeleteCourse(course);
            }

            if (await this.unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed to delete course");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-semesters", Name = "GetSemesters")]
        public async Task<ActionResult<IEnumerable<Semester>>> GetSemesters() 
        {
            // userParams.CurrentUserName = User.GetUsername();    // From Token

            var courses = await this.unitOfWork.CoursesRepository.GetSemestersAsync();

            return Ok(courses);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-courses", Name = "GetCourses")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses() 
        {
            // userParams.CurrentUserName = User.GetUsername();    // From Token

            var courses = await this.unitOfWork.CoursesRepository.GetCoursesAsync();

            return Ok(courses);
        }

    }
}