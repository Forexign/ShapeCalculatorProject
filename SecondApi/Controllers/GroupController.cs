using DataAccessLayer.DbContext;
using DataAccessLayer.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Entities.ShapeModels;

namespace SecondApi.Controllers
{
    public class GroupController : ApiController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public IHttpActionResult GetXAmountShape([FromBody] List<Inputs> InputModel)
        {
            List<Shape> shapes = new List<Shape>();

            foreach (var shape in InputModel)
            {
                //starts logging
                var ApiLog = new ApiLog
                {
                    StartTime = DateTime.Now,
                    ApiId = db.Apis
                        .Where(i => i.Name == "SECOND_API")
                        .Select(i => i.Id)
                        .FirstOrDefault(),
                    InputsId = shape.Id,
                };

                Shape shapeToCalculate = ShapeFactory.CreateShape(shape);
                
                double area = shapeToCalculate.CalculateArea();
                double volume = shapeToCalculate.CalculateVolume();
                var MyShapeResult = new ShapeResult
                {
                    ShapeType = shapeToCalculate.ShapeType,
                    Area = area,
                    Volume = volume,
                    CreatedTime = DateTime.Now,
                    InputsId = shape.Id
                };

                // makes hesaplandiMi true
                var inputToUpdate = db.Inputs.Find(shape.Id);
                if (inputToUpdate != null)
                {
                    inputToUpdate.HesaplandiMi = true;
                    db.Inputs.AddOrUpdate(inputToUpdate);
                }

                // sets the result of the shape's userId
                MyShapeResult.UserId = shape.UserId;
                db.ShapeResults.AddOrUpdate(MyShapeResult);

                //finishing the logging
                ApiLog.EndTime = DateTime.Now;
                db.ApiLogs.Add(ApiLog);
            }
            db.SaveChanges();
            return Ok("Second API Successfully Completed");
        }
    }
}
