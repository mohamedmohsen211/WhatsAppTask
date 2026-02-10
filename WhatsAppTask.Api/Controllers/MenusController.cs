using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsAppTask.DTO;

namespace WhatsAppTask.Api.Controllers
{
    [ApiController]
    [Route("api/menus")]
    [Authorize]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        private int UserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public IActionResult Create(CreateMenuDto dto)
        {
            var menu = _menuService.CreateMenu(UserId, dto);

            return Ok(new { menu.Id });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var menus = _menuService.GetMenus(UserId)
                .Select(m => new MenuResponseDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Message = m.Message,
                    IsRoot = m.IsRoot,
                    Items = m.Items.Select(i => new MenuItemResponseDto
                    {
                        Id = i.Id,
                        Title = i.Title,
                        Type = i.Type,
                        ReplyMessage = i.ReplyMessage,
                        NextMenuId = i.NextMenuId
                    }).ToList()
                });

            return Ok(menus);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var m = _menuService.GetMenu(UserId, id);

            return Ok(new MenuResponseDto
            {
                Id = m.Id,
                Name = m.Name,
                Message = m.Message,
                IsRoot = m.IsRoot,
                Items = m.Items.Select(i => new MenuItemResponseDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Type = i.Type,
                    ReplyMessage = i.ReplyMessage,
                    NextMenuId = i.NextMenuId
                }).ToList()
            });
        }

        [HttpPost("{menuId}/items")]
        public IActionResult AddItem(int menuId, CreateMenuItemDto dto)
        {
            var item = _menuService.AddItem(menuId, dto);
            return Ok(new { item.Id });
        }
    }

}
