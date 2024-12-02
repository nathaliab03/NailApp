using MediatR;

namespace NailApp.Commands.CreateAccount
{
    public class CreateAccountRequest : IRequest<CreateAccountResponse>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }

        public bool RememberMe { get; set; }
    }
}
