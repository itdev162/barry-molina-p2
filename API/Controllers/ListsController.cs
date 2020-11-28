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
        /// GET api/lists
        /// </summary>
        /// <returns>All lists associated with the authorized user</returns>
        [Authorize]
        [HttpGet]
        public ActionResult<List<List>> GetLists()
        {
            var user = (User)HttpContext.Items["User"];
            return _context.Lists.Where(l => l.User == user.Id).ToList();
        }

        /// <summary>
        /// POST api/lists
        /// </summary>
        /// <param name="list">The list to create</param>
        /// <returns>A new list</returns>
        [Authorize]
        [HttpPost]
        public ActionResult CreateList([FromBody] List list)
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
        public ActionResult CreateItem([FromRoute] Guid id, [FromBody] ListItem item)
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
        /// PUT api/lists/{id}
        /// </summary>
        /// <param name="id">The list to update</param>
        /// <param name="newList">The new list info</param>
        /// <returns>An updated list</returns>
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<List> UpdateList([FromRoute] Guid id, [FromBody] List newList)
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

            list.Title = newList.Title != null ? newList.Title : list.Title;

            var success = _context.SaveChanges() > 0;

            if (success)
            {
                return Ok(list);
            }

            throw new Exception("Error updating list");
        }

        /// <summary>
        /// PUT api/lists/{listId}/{itemId}
        /// </summary>
        /// <param name="listId">The containing list</param>
        /// <param name="itemId">The item to update</param>
        /// <param name="newItem">The new item info</param>
        /// <returns>An updated item</returns>
        [Authorize]
        [HttpPut("{listId}/{itemId}")]
        public ActionResult<ListItem> UpdateItem([FromRoute] Guid listId, [FromRoute] Guid itemId, [FromBody] ListItem newItem)
        {
            var user = (User)HttpContext.Items["User"];
            var list = _context.Lists.Find(listId);

            if (list == null)
            {
                throw new Exception("Could not find list");
            }

            if (list.User != user.Id )
            {
                throw new Exception("User not authorized");
            }

            var item = list.Items.FirstOrDefault(i => i._id == itemId);

            if (item == null)
            {
                throw new Exception("Could not find item");
            }

            item.Desc = newItem.Desc != null ? newItem.Desc : item.Desc;

            var success = _context.SaveChanges() > 0;

            if (success)
            {
                return Ok(item);
            }

            throw new Exception("Error updating list");
        }

        /// <summary>
        /// DELETE api/lists/{id}
        /// </summary>
        /// <param name="id">The list to delete</param>
        /// <returns>True if deletion was successful</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteList(Guid id)
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

        /// <summary>
        /// DELETE api/lists/{listId}/{itemId}
        /// </summary>
        /// <param name="listId">The list to delete from</param>
        /// <param name="itemId">The item to delete</param>
        /// <returns>True if deletion was successful</returns>
        [Authorize]
        [HttpDelete("{listId}/{itemId}")]
        public ActionResult<bool> DeleteItem(Guid listId, Guid itemId)
        {
            var user = (User)HttpContext.Items["User"];
            var list = _context.Lists.Find(listId);

            if (list == null)
            {
                throw new Exception("Could not find list");
            }

            if (list.User != user.Id )
            {
                throw new Exception("User not authorized");
            }

            var item = list.Items.FirstOrDefault(i => i._id == itemId);

            if (item == null)
            {
                throw new Exception("Could not find item");
            }

            _context.Remove(item);

            var success = _context.SaveChanges() > 0;

            if (success)
            {
                return true;
            }

            throw new Exception("Error deleting list");
        }
    }
}