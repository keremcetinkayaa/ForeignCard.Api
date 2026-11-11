using System.Data;
using Dapper;
using ForeignCard.Common.Abstractions;
using ForeignCard.Common.Helpers;
using ForeignCard.Model.Transactions;
using MediatR;
using ForeignCard.Common.Constants;

namespace ForeignCard.Operation.Transactions;

public class SimulateTransactionsHandler : IRequestHandler<SimulateTransactions.SimulateTransactionsRequest, SimulateTransactions.SimulateTransactionsResponse>
{
    List<string> codes = CurrencyCode.AllNumericCodes.ToList();

    private readonly IDbConnectionFactory _factory;
    private readonly IMediator _mediator;

    public SimulateTransactionsHandler(IDbConnectionFactory factory, IMediator mediator)
    {
        _factory = factory;
        _mediator = mediator;
    }

    public async Task<SimulateTransactions.SimulateTransactionsResponse> Handle(SimulateTransactions.SimulateTransactionsRequest request, CancellationToken cancellationToken)
    {
        var response = new SimulateTransactions.SimulateTransactionsResponse { Requested = request.Count };

        List<string> cards;
        using (var conn = _factory.CreateConnection())
        {
            var rows = await conn.QueryAsync<string>(ProcedureConstants.SP_CardListForSimulation, commandType: CommandType.StoredProcedure);
            cards = rows.Select(c => c.Trim()).ToList();
        }

        if (cards.Count == 0)
        {
            response.Failed = request.Count;
            response.Errors.Add("Simulasyon icin uygun aktif kart bulunamadi");
            return response;
        }

        var random = new Random();

        for (int i = 0; i < request.Count; i++)
        {
            var command = new CreateTransaction.CreateTransactionRequest
            {
                CardNo = cards[random.Next(cards.Count)],
                Amount = Math.Round((decimal)(random.NextDouble() * 495 + 5), 2),
                CurrencyNumericCode = codes[random.Next(codes.Count)]
            };

            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                response.Transactions.Add(result);
                response.Succeeded++;
            }
            catch (Exception ex)
            {
                response.Failed++;
                response.Errors.Add(ex.Message);
            }
        }

        return response;
    }
}