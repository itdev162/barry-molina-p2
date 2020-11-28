using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Services;
using API.Models;
using Persistence;
using Domain;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListsController : ControllerBase
    {
        private IUserService _userService;
        private DataContext _context;

        public ListsController(IUserService userService, DataContext context)
        {
            _userService = userService;
            _context = context;
        }

        /// <summary>
        /// POST api/lists
        /// </summary>
        /// <param name="list">The list to create</param>
        /// <returns>A new list</returns>
        [Authorize]
        [HttpPost]
        public ActionResult Create([FromBody] List list)
        {
            var user = (User)HttpContext.Items["User"];
            var newList = new List
            {
                Title = list.Title,
                User = user.Id
            };

            _context.Lists.Add(newList);
            var success = _context.SaveChanges() > 0;

            if (success)
            {
                return Ok(newList);
            }

            throw new Exception("Error creating list");
        }

        /// <summary>
        /// POST api/lists/{id}
        /// </summary>
        /// <param name="id">The list item will be added to</param>
        /// <param name="item">The list item to add</param>
        /// <returns>A new list item</returns>
        [Authorize]
        [HttpPost("{id}")]
        public ActionResult Create([FromRoute] Guid id, [FromBody] ListItem item)
        {
            var user = (User)HttpContext.Items["User"];
            var list = _context.Lists.Find(id);

            if (list.User != user.Id )
            {
                throw new Exception("User not authorized");
            }
            var newItem = new ListItem
            {
                Desc = item.Desc,
            };

            list.Items.Add(newItem);
            var success = _context.SaveChanges() > 0;

            if (success)
            {
                return Ok(newItem);
            }
            throw new Exception("Error creating list item");
        }

        /// <summary>
        /// GET api/lists
        /// </summary>
        /// <returns>A list of lists</returns>
        [Authorize]
        [HttpGet]
        public ActionResult<List<List>> GetLists()
        {
            var user = (User)HttpContext.Items["User"];
            return _context.Lists.Where(l => l.User == user.Id).ToList();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(Guid id)
        {
            var user = (User)HttpContext.Items["User"];
            var list = _context.Lists.Find(id);

            if (list == null)
            {
                throw new Exception("Could not find list");
            }

            if (list.User != user.Id )
            {
                throw new Exception("User not authorized");
            }

            _context.Remove(list);

            var success = _context.SaveChanges() > 0;

            if (success)
            {
                return true;
            }

            throw new Exception("Error deleting list");
        }
    }
}