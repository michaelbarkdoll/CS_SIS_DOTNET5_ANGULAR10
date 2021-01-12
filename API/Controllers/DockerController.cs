using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DockerController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly DataContext context;
        public DockerController(IUnitOfWork unitOfWork, IMapper mapper, DataContext context)
        {
            this.context = context;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("status")]
        public async Task<ActionResult<MemberContainerDto>> Status() {
            var sourceUserId = User.GetUserId();

            return await this.unitOfWork.DockerRepository.GetMemberContainersAsync(sourceUserId);
        }

        [HttpPost("create-container")]
        public async Task<ActionResult<MemberContainerDto>> CreateContainer(UserContainerDto userContainerDto) {
            var sourceUserId = User.GetUserId();
            var user = await unitOfWork.UserRepository.GetUserByUsernameUserContainersAsync(User.GetUsername());


            // var addContainer2 = new UserContainer {
            //     // Id = 1,
            //     ContainerId = "1",
            //     Image = "test",
            //     Command = "test",
            //     InternalPort = 1,
            //     ExternalPort = 1,
            //     JobOwner = "Test"

            // };
            var addContainer = new UserContainer {
                // Id = 1, // Index not set by us.. set by DB
                ContainerId = userContainerDto.ContainerId,
                Image = userContainerDto.Image,
                Command = userContainerDto.Command,
                InternalPort = userContainerDto.InternalPort,
                ExternalPort = userContainerDto.ExternalPort,
                JobOwner = userContainerDto.JobOwner
            };

            if( user.UserContainers == null) {
                user.UserContainers = new List<UserContainer>();
            }

            if( user.UserContainers != null) {
                user.UserContainers.Add(addContainer);
            }
                

            if (await unitOfWork.Complete()) {
                return Ok(this.mapper.Map<MemberContainerDto>(user));
            }

            return BadRequest("Failed to add container");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-user-container")]
        public async Task<ActionResult<MemberContainerDto>> CreateUserContainer(UserContainerDto userContainerDto) {
            
            var user = await unitOfWork.UserRepository.GetUserByUsernameUserContainersAsync(userContainerDto.JobOwner);


            // var addContainer2 = new UserContainer {
            //     // Id = 1,
            //     ContainerId = "1",
            //     Image = "test",
            //     Command = "test",
            //     InternalPort = 1,
            //     ExternalPort = 1,
            //     JobOwner = "Test"

            // };
            var addContainer = new UserContainer {
                // Id = 1, // Index not set by us.. set by DB
                ContainerId = userContainerDto.ContainerId,
                Image = userContainerDto.Image,
                Command = userContainerDto.Command,
                InternalPort = userContainerDto.InternalPort,
                ExternalPort = userContainerDto.ExternalPort,
                JobOwner = userContainerDto.JobOwner
            };

            if( user.UserContainers == null) {
                user.UserContainers = new List<UserContainer>();
            }

            if( user.UserContainers != null) {
                user.UserContainers.Add(addContainer);
            }
                

            if (await unitOfWork.Complete()) {
                return Ok(this.mapper.Map<MemberContainerDto>(user));
            }

            return BadRequest("Failed to add container");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-container/{id}", Name = "DeleteContainer")]
        public async Task<ActionResult<MemberContainerDto>> DeleteContainer(int id)
        {
            var username = User.GetUsername();

            var container = await this.unitOfWork.DockerRepository.GetContainerAsync(id);

            if(container != null) {
                this.unitOfWork.DockerRepository.DeleteContainer(container);
            }

            if (await this.unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed to delete container");
        }
    }
}