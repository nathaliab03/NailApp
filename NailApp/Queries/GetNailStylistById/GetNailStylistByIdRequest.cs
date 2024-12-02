using MediatR;

namespace NailApp.Queries.GetNailStylistById
{
    public class GetNailStylistByIdRequest : IRequest<GetNailStylistByIdResponse>
    {
        public int Id { get; set; } 
    }
}
