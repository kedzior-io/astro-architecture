//namespace AstroArchitecture.Handlers.Countries;

//public static class GetCountries
//{
//    public sealed class Query : IQuery<IHandlerResponse<Response>>
//    {
//    }

//    public sealed class Response
//    {
//        public IReadOnlyCollection<string> Countries { get; set; } = Array.Empty<string>();
//    }

//    public sealed class Handler : QueryHandler<Query, Response>
//    {
//        public Handler(IHandlerContext context) : base(context)
//        {
//        }

//        public override Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
//        {
//            var someNonAsyncStuff = new Response() { Countries = new List<string> { "Spain", "United Stated of America" } };

//            return Success(someNonAsyncStuff);
//        }
//    }
//}