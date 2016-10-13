﻿using System;
using System.Linq;
using System.Web.Http;
using BusinessLogic;
using Web.Models.EntityModels;

namespace Web.Controllers
{
    public class AuthorController : BaseApiController<AuthorModelFactory>
    {
        public AuthorController(IDataManager dataManager) : base(dataManager)
        { }

        public IHttpActionResult Get()
        {
            try
            {
                var model = DataManager.AuthorRepository.Get();
                if (model != null && model.Count > 0)
                    return Ok(model);

                return InternalServerError(new Exception("Database is empty."));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Post([FromBody] AuthorModel model)
        {
            try
            {
                var entity = ModelFactory.Create(model);
                var createdEntity = DataManager.AuthorRepository.Create(entity);

                if (createdEntity != null)
                {
                    var entityModel = ModelFactory.Create(createdEntity);
                    return Created($"http:/localhost/api/author/{entityModel.Id}", entityModel);
                }

                return InternalServerError(new Exception("Recording has not created"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Put([FromBody] AuthorModel model)
        {
            try
            {
                var entity = ModelFactory.Create(model);
                var updatedEntity = DataManager.AuthorRepository.Update(entity);

                if (updatedEntity != null)
                {
                    var entityModel = ModelFactory.Create(updatedEntity);
                    return Ok(entityModel);
                }

                return InternalServerError(new Exception("Recording has not created"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Delete(int id)
        {
            try
            {
                var entity = DataManager.AuthorRepository.Get().Where(x=>x.Id == id).ToList()[0];
                if (entity != null)
                    DataManager.AuthorRepository.Delete(entity);
                else
                    return NotFound();
                
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}