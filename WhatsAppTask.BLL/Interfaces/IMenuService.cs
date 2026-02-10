public interface IMenuService
{
    Menu CreateMenu(int userId, CreateMenuDto dto);
    Menu GetMenu(int userId, int menuId);
    List<Menu> GetMenus(int userId);
    MenuItem AddItem(int menuId, CreateMenuItemDto dto);
}