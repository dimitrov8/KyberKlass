namespace KyberKlass.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Web.ViewModels.Admin.User;

    public interface IUserService
    {
        Task<List<UserViewModel>> AllAsync();

        Task<UserDetailsViewModel?> GetUserDetailsAsync(string id);
    }
}
