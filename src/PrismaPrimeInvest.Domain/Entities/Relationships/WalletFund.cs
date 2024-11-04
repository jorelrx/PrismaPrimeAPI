using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Entities.User;

namespace PrismaPrimeInvest.Domain.Entities.Relationships;

public class WalletFund : BaseEntity
{
    public Guid FundId { get; set; }
    public Guid WalletId { get; set; }

    public required Wallet Wallet { get; set; }
    public required Fund Fund { get; set; }
}

/*
    -- Quantidade de 


    -- WalletFundController.cs

    -- IWalletFundService.cs
    -- WalletFundService.cs

    -- IWalletFundRepository.cs
    -- WalletFundRepository.cs
*/

