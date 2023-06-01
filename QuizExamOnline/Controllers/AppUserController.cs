﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Core.Authentication;
using Nest;
using QuizExamOnline.Entities.AppUsers;
using QuizExamOnline.Models;
using QuizExamOnline.Repositories;
using QuizExamOnline.Responses;
using QuizExamOnline.Services.AppUsers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizExamOnline.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserService _appUserService;

        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AppUserDto>> Register([FromBody] CreateAppUserDto createAppUserDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var appUser = await _appUserService.CreateUser(createAppUserDto);
                //return new OkObjectResult(new ResponseUser<AppUserDto>("Success", appUser));
                return StatusCode(StatusCodes.Status201Created, new ResponseUser<AppUserDto>("Success", appUser));

            } catch(AppUserExeption ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

        //[HttpGet("getuser")]
        //public async Task<ActionResult<AppUserDto>> GetUser()
        //{
           
        //    try
        //    {
        //        var appuser = await _appUserService.Get();
        //        return StatusCode(StatusCodes.Status200OK, new ResponseUser<AppUserDto>("Success", appuser));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
        //    }
        //}

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<AppUserDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var appuser = await _appUserService.RefreshToken(refreshTokenDto.RefreshToken);
                //return new OkObjectResult(new ResponseUser<AppUserDto>("Success", appuser));
                return StatusCode(StatusCodes.Status200OK, new ResponseUser<AppUserDto>("Success", appuser));
            }
            catch (AppUserExeption ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AppUserDto>> Login([FromBody] UserLoginDto userlogin)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse("Invalid Input"));
            try
            {
                var appuser = await _appUserService.Login(userlogin);
                return StatusCode(StatusCodes.Status200OK, new ResponseUser<AppUserDto>("Success", appuser));
            }
            catch (AppUserExeption ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new BaseResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse(ex.Message));
            }
        }

    }
}
