using Data.Models;
using Data.RepoInterfaces;
using Grpc.Core;
using System.Threading.Tasks;

namespace GrpcAuthService.Services
{
    public class RegisterService: Register.RegisterBase
    {
        private readonly IUserRepo _repo;
        public RegisterService(IUserRepo repo)
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

        //        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        //        {
        //            return Task.FromResult(new HelloReply
        //            {
        //                Message = "Hello " + request.Name
        //            });
        //        }
    }
}
