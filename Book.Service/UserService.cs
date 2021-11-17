using Book.IRepository;
using Book.IService;
using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Book.Extensions.ExpressionExtensions;

namespace Book.Service
{
   public class UserService:BaseService<User>,IUserService
    {
        private readonly IUserRepository repository;
        private readonly CreatExprssions expression;
        public UserService(IUserRepository _repository,CreatExprssions _expression) {
            repository = _repository;
            expression = _expression;
            base._repository = _repository;
        }

        public async Task<User> GetUserByid(int id) {
            if(id>0)
              return await repository.FindAsync(id);
            return null;
        }

        public Task<List<User>> UserQueryAsync()
        {
            return  repository.QueryAsync();
        }

        public Task<List<User>> GetUsers(User user)
        {
            var lamada = expression.CatchModel(user); 
            return base.QueryAsync(lamada);
        }
    }
}
