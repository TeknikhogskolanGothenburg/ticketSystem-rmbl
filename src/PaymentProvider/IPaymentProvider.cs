namespace TicketSystem.PaymentProvider
{
    public interface IPaymentProvider
    {
        /// <summary>
        /// Pay an amount
        /// </summary>
        /// <param name="amountToPay">The amount to pay</param>
        /// <param name="valuta">A three letter code (following ISO 4217) represending the valuta of the amount</param>
        /// <param name="orderReference">A string representing the order</param>
        /// <returns>A payment object, containging the status of the order</returns>
        Payment Pay(decimal amountToPay, string valuta, string orderReference);
    }
}
