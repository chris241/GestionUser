﻿using GestionUserBack.Business.Repository;
using GestionUserBack.Entity;
using GestionUserBack.Entity.Services;
using GestionUserBack.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GestionUserBack.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private EntityRepository<User> _userRepository = null;
        private DataTableRepository<User> _userTableRepository = null;
        public UserController(EntityRepository<User> UserRepository,
            DataTableRepository<User> UserTableRepository)
            {
                this._userRepository = UserRepository;
                this._userTableRepository = UserTableRepository;
            }

            [HttpPost]
            [Route("all")]
            public  HttpResponseMessage GetAllUsers(GetDataTableRequest request)
            {
                try
                {
                DataTable<User> user = this._userTableRepository.GetAll(request);
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
                }
            }
        [HttpGet]
        [Route("user")]
        public async Task<HttpResponseMessage> GetAllUsers([FromUriAttribute] Guid id)
        {
            try
            {
               User user = await this._userRepository.FindById(id);
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "user introuvable.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        [HttpPost]
            [Route("add")]
            public HttpResponseMessage AddUser([FromBody]CreateUserReq userReq)
            {
                try
                {
                    if (string.IsNullOrEmpty(userReq.Nom)|| string.IsNullOrEmpty(userReq.Email)|| string.IsNullOrEmpty(userReq.Contact))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "vide.");
                    }
                    User user = new User()
                    {
                        Nom = userReq.Nom,
                        Contact = userReq.Contact,
                        Email = userReq.Email,
                        DateCreate = DateTime.Now
                    };
                    this._userRepository.SaveOrUpdate(user);
                    return Request.CreateResponse(HttpStatusCode.OK, "Utilisateur enregistré avec succès.");
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
                }
            }
            [HttpDelete]
            [Route("delete")]
            public async Task<HttpResponseMessage> DeleteUser(Guid id)
            {
                try
                {
                User user = await this._userRepository.FindById(id);
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "user introuvable.");
                }
                await this._userRepository.Delete(user);
                return Request.CreateResponse(HttpStatusCode.OK, "Utilisateur supprimé avec succès.");

                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e);

                }
            }

            [HttpPut]
            [Route("update")]
            public async Task<HttpResponseMessage> UpdateUser(CreateUserReq userReq)
            {
                try
                {
                    User user = await this._userRepository.FindById(userReq.Id);
                    if (user == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "user introuvable.");
                    }
                    user.Email = userReq.Email;
                    user.Nom = userReq.Nom;
                    user.Contact = userReq.Contact;
                    user.DateModify = DateTime.Now;
                    this._userRepository.SaveOrUpdate(user);
                    return Request.CreateResponse(HttpStatusCode.OK, "modification enregistrée avec succès.");
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
                }
            }
        }
    }
