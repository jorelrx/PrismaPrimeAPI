using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;
using PrismaPrimeInvest.Infra.Data.Contexts;

namespace PrismaPrimeInvest.Infra.Data.Repositories.Invest;

public class FundPaymentRepository(ApplicationDbContext context) : BaseRepository<FundPayment>(context), IFundPaymentRepository {}