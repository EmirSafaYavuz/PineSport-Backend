using Core.Entities;

namespace Entities.Dtos;

public class SidebarMenuDto : IDto
{
    public string Title { get; set; }  // Menü başlığı
    public string Icon { get; set; }   // Menü simgesi
    public string Path { get; set; }   // Menü yolu (routerLink)
}