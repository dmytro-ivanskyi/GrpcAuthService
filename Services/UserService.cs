using Data.Models;
using Data.RepoInterfaces;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Threading.Tasks;

namespace GrpcAuthService.Services
{
    public class UserService : Register.RegisterBase
    {
        private readonly IUserRepo _repo;
        public UserService(IUserRepo repo)
        {
            _repo = repo;
        }

        public override async Task<UserReply> RegisterUser(UserRequest request, ServerCallContext context)
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email
            };

            var created = await _repo.CreateUserAsync(user);
            if (!created)
                return null;

            var reply = await _repo.GetUserByIdAsync(user.Id);
            return new UserReply { Id = reply.Id, Name = reply.Name, Email = reply.Email };
        }

        public override async Task<AllUsersReply> GetAllUsers(Empty request, ServerCallContext context)
        {
            var users = await _repo.GetUsersAsync();

            var reply = new AllUsersReply();
            foreach (var u in users)
            {

                reply.Users.Add(new UserReply
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                });
            }
            
            return reply;
        }
    }
}
