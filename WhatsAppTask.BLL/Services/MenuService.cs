using Microsoft.EntityFrameworkCore;
using WhatsAppTask.DAL.DbContext;

namespace WhatsAppTask.BLL.Services
{
    public class MenuService : IMenuService
    {
        private readonly AppDbContext _context;

        public MenuService(AppDbContext context)
        {
            _context = context;
        }

        public Menu CreateMenu(int userId, CreateMenuDto dto)
        {
            var menu = new Menu
            {
                UserId = userId,
                Name = dto.Name,
                Message = dto.Message,
                IsRoot = dto.IsRoot
            };

            _context.Menus.Add(menu);
            _context.SaveChanges();

            return menu;
        }

        public List<Menu> GetMenus(int userId)
        {
            return _context.Menus
                .Include(m => m.Items)
                .Where(m => m.UserId == userId)
                .ToList();
        }

        public Menu GetMenu(int userId, int menuId)
        {
            var menu = _context.Menus
                .Include(m => m.Items)
                .FirstOrDefault(m => m.Id == menuId && m.UserId == userId);

            if (menu == null)
                throw new Exception("Menu not found");

            return menu;
        }

        public MenuItem AddItem(int menuId, CreateMenuItemDto dto)
        {
            if (dto.Type == MenuItemType.Message && string.IsNullOrEmpty(dto.ReplyMessage))
                throw new Exception("ReplyMessage is required");

            if (dto.Type == MenuItemType.Menu && dto.NextMenuId == null)
                throw new Exception("NextMenuId is required");

            var item = new MenuItem
            {
                MenuId = menuId,
                Title = dto.Title,
                Type = dto.Type,
                ReplyMessage = dto.ReplyMessage,
                NextMenuId = dto.NextMenuId
            };

            _context.MenuItems.Add(item);
            _context.SaveChanges();

            return item;
        }
    }

}
