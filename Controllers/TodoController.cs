using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        //CONSTRUCTOR
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;
        }

        //Get All Todo’s
        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllItems()
        {
            var result = await _context.TodoItems.ToListAsync();
            return new JsonResult(new {success = true,StatusCode = 200,data =  result})
            {
                StatusCode = 200
            };
        }

        //Get Specific Todo
        [HttpGet("get-by-name/{name}")]
        public async Task<ActionResult<TodoItem>> GetByName(string name)
        {
            var todoItem = await _context.TodoItems.Where(x => x.Title.ToUpper().Contains(name.ToUpper())).ToListAsync();
            if (todoItem == null)
            {
                Response r = new Response();
                r.success = "false";
                return NotFound(r);
            }
            //return Ok();
            return new JsonResult(new {success = true,StatusCode = 200,data =  todoItem})
            {
                StatusCode = 200
            };
        }
        
        /* Get Incoming ToDo (for today/next day/current week)
           ID =>
           1 = Today
           2 = tomorrow
           3 = current week
        */  
        [HttpGet("incoming/{id}")]
        public async Task<ActionResult<TodoItem>>  GetIncomingTodo(int id)
        {
            String today = DateTime.Now.ToString("yyyy-MM-dd");
            String tomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            DayOfWeek currentDay = DateTime.Now.DayOfWeek;  
            int daysTillCurrentDay = currentDay - DayOfWeek.Sunday;  
            DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
            DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

            List<TodoItem> todoItem = null;
             
            if (id  ==  1){
                todoItem = await _context.TodoItems.Where(x => x.ExpirateDate == today).ToListAsync();
            }
            else if (id == 2){
                todoItem = await _context.TodoItems.Where(x => x.ExpirateDate == tomorrow).ToListAsync();
            }else if (id == 3){
                todoItem = await _context.TodoItems.Where(x => Convert.ToDateTime(x.ExpirateDate) >= currentWeekStartDate && Convert.ToDateTime(x.ExpirateDate) <= currentWeekEndDate).ToListAsync();
            }
            else{
                return new JsonResult(new {success = true,StatusCode = 202,message="id not found!"})
                {
                    StatusCode = 202
                };    
            }


            if (todoItem == null)
            {
                Response r = new Response();
                r.success = "false";
                return NotFound(r);
            }
            //return Ok();
            return new JsonResult(new {success = true,StatusCode = 200,data =  todoItem})
            {
                StatusCode = 200
            };  
        }


        //Create Todo
        [HttpPost()]
        public JsonResult AddTodo([FromBody] createTodoDto model)
        {
            _context.TodoItems.Add(new TodoItem { 
                                                    Title = model.Title,
                                                    ExpirateDate = model.ExpirateDate, //yyyy-MM-dd
                                                    Description = model.Description,
                                                    PercentageComplete = model.PercentageComplete
                                                });
            _context.SaveChanges();

            return new JsonResult(new {success = true,StatusCode = 201,message="create success!"})
            {
                StatusCode = 201
            };
        }

        //Update Todo
        [HttpPut("{id}")]
        public JsonResult UpdateTodo(int id, [FromBody] createTodoDto model)
        {
            var todo = _context.TodoItems.Where(x => x.Id == id).SingleOrDefault();
            
            if (todo != null){
                todo.Title = model.Title;
                todo.ExpirateDate = model.ExpirateDate;
                todo.Description = model.Description;
                todo.PercentageComplete = model.PercentageComplete;            
                _context.SaveChanges();

                return new JsonResult(new {success = true,StatusCode = 200,message="update success!"})
                {
                    StatusCode = 200
                };
            }else{
                return new JsonResult(new {success = true,StatusCode = 202,message="id not found!"})
                {
                    StatusCode = 202
                };
            }
        }

        //Set Todo percent complete
        [HttpGet("set-percentage/{id}/{percentage}")]
        public JsonResult SetTodoPercentage(int id, int percentage)
        {
            var todo = _context.TodoItems.Where(x => x.Id == id).SingleOrDefault();
            
            if (todo != null){
                todo.PercentageComplete = percentage;
                _context.SaveChanges();
                return new JsonResult(new {success = true,StatusCode = 200,message="set percentage success!"})
                {
                    StatusCode = 200
                };
            }else{
                return new JsonResult(new {success = true,StatusCode = 202,message="id not found!"})
                {
                    StatusCode = 202
                };
            }
        }

        //Delete Todo
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var todo = _context.TodoItems.Where(x => x.Id == id).SingleOrDefault();
            if (todo != null){
                _context.TodoItems.Remove(todo);
                _context.SaveChanges();
                return new JsonResult(new {success = true,StatusCode = 200,message="delete success!"})
                {
                    StatusCode = 200
                };
            }else{
                return new JsonResult(new {success = true,StatusCode = 202,message="id not found!"})
                {
                    StatusCode = 202
                };
            }
        }

        //Mark Todo as Done
        [HttpGet("mark-as-done/{id}")]
        public JsonResult MarkAsDone(int id)
        {
            var todo = _context.TodoItems.Where(x => x.Id == id).SingleOrDefault();
            
            if (todo != null){
                todo.PercentageComplete = 100;
                _context.SaveChanges();
                return new JsonResult(new {success = true,StatusCode = 200,message="mark as done success!"})
                {
                    StatusCode = 200
                };
            }else{
                return new JsonResult(new {success = true,StatusCode = 202,message="id not found!"})
                {
                    StatusCode = 202
                };
            }
        }
    }
}
