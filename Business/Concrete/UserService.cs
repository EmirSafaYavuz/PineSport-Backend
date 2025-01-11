using System.Security.Claims;
using AutoMapper;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IHttpContextAccessor httpContextAccessor, 
                           IUserRepository userRepository, 
                           IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IDataResult<List<SidebarMenuDto>> GetSidebarMenu()
        {
            try
            {
                // Kullanıcının rolüne göre menüleri belirle
                var sidebarMenus = new List<SidebarMenuDto>();

                if (IsUserInRole("Admin"))
                {
                    sidebarMenus = GetAdminMenus();
                }
                else if (IsUserInRole("School"))
                {
                    sidebarMenus = GetSchoolMenus();
                }
                else if (IsUserInRole("Student"))
                {
                    sidebarMenus = GetStudentMenus();
                }
                else if (IsUserInRole("Parent"))
                {
                    sidebarMenus = GetParentMenus();
                }
                else if (IsUserInRole("Trainer"))
                {
                    sidebarMenus = GetTrainerMenus();
                }
                else
                {
                    throw new UnauthorizedAccessException("Geçersiz rol.");
                }

                return new SuccessDataResult<List<SidebarMenuDto>>(sidebarMenus);
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ErrorDataResult<List<SidebarMenuDto>>(ex.Message);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<SidebarMenuDto>>("Beklenmeyen bir hata oluştu: " + ex.Message);
            }
        }

        private bool IsUserInRole(string role)
        {
            // Kullanıcıyı bul, claim listesinden "role" tipindeki claim'i al
            var userRole = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(cl => cl.Type == ClaimTypes.Role 
                                      || cl.Type.EndsWith("role", StringComparison.OrdinalIgnoreCase))
                ?.Value;

            // Elde ettiğimiz kullanıcı rolü, parametre olarak gönderilen role ile eşleşiyor mu?
            return !string.IsNullOrEmpty(userRole) && userRole.Equals(role, StringComparison.OrdinalIgnoreCase);
        }

        private List<SidebarMenuDto> GetAdminMenus()
        {
            return new List<SidebarMenuDto>
            {
                new SidebarMenuDto { Title = "Ana Sayfa", Icon = "admin_panel_settings", Path = "/dashboard/admin" },
                new SidebarMenuDto { Title = "Okullar", Icon = "school", Path = "/schools" },
                new SidebarMenuDto { Title = "Şubeler", Icon = "location_city", Path = "/branches" },
                new SidebarMenuDto { Title = "Eğitmenler", Icon = "people", Path = "/trainers" },
                new SidebarMenuDto { Title = "Öğrenciler", Icon = "people", Path = "/students" },
                new SidebarMenuDto { Title = "Veliler", Icon = "people", Path = "/parents" },
                new SidebarMenuDto { Title = "Ödemeler", Icon = "credit_card", Path = "/payments" },
                new SidebarMenuDto { Title = "Raporlar", Icon = "bar_chart", Path = "/reports" },
                new SidebarMenuDto { Title = "Gelişimler", Icon = "trending_up", Path = "/progress" }
            };
        }

        private List<SidebarMenuDto> GetSchoolMenus()
        {
            return new List<SidebarMenuDto>
            {
                new SidebarMenuDto { Title = "School Dashboard", Icon = "dashboard", Path = "/app/dashboard/school" },
                new SidebarMenuDto { Title = "Branches", Icon = "location_city", Path = "/app/branches" }
            };
        }

        private List<SidebarMenuDto> GetStudentMenus()
        {
            return new List<SidebarMenuDto>
            {
                new SidebarMenuDto { Title = "Student Dashboard", Icon = "dashboard", Path = "/app/student-dashboard" },
                new SidebarMenuDto { Title = "My Courses", Icon = "menu_book", Path = "/app/my-courses" }
            };
        }

        private List<SidebarMenuDto> GetParentMenus()
        {
            return new List<SidebarMenuDto>
            {
                new SidebarMenuDto { Title = "Parent Dashboard", Icon = "family_restroom", Path = "/app/parent-dashboard" },
                new SidebarMenuDto { Title = "Children Info", Icon = "child_care", Path = "/app/children-info" }
            };
        }

        private List<SidebarMenuDto> GetTrainerMenus()
        {
            return new List<SidebarMenuDto>
            {
                new SidebarMenuDto { Title = "Trainer Dashboard", Icon = "dashboard", Path = "/app/trainer-dashboard" },
                new SidebarMenuDto { Title = "My Classes", Icon = "class", Path = "/app/my-classes" }
            };
        }
    }
}