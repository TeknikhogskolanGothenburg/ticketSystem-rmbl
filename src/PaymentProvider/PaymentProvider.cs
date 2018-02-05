using System;

namespace TicketSystem.PaymentProvider
{
    public class PaymentProvider : IPaymentProvider
    {
        /// <summary>
        /// Simulate a payment provider
        /// One out of 20 payments results in an unknown error
        /// Three out of 20 payments results in payment rejected
        /// </summary>
        /// <param name="amountToPay">The amount to pay</param>
        /// <param name="valuta">A three letter code (following ISO 4217) represending the valuta of the amount</param>
        /// <param name="orderReference">A Guid which is the internal reference of the payment system</param>
        /// <returns>A payment object, containging the status of the order</returns>
        public Payment Pay(decimal amountToPay, string valuta, string orderReference)
        {
            if (valuta.Length != 3)
            {
                // ISO 4217: https://en.wikipedia.org/wiki/ISO_4217
                throw new ArgumentException("The valua must be three charaters, the input '" + valuta + "' is not valid");
            }

            Random rnd = new Random();
            int randomPaymentStatus = rnd.Next(1, 21); // creates a number between 1 and 20

            PaymentStatus status = PaymentStatus.PaymentApproved;
            if (randomPaymentStatus >= 17 && randomPaymentStatus <= 19)
            {
                status = PaymentStatus.PaymentRejected;
            }
            if (randomPaymentStatus == 20)
            {
                status = PaymentStatus.UnknownError;
            }
            return new Payment()
            {
                Valuta = valuta,
                PaymentStatus = status,
                OrderReference = orderReference,
                TotalAmount = amountToPay,
                PaymentReference = Guid.NewGuid().ToString()
            };
        }
    }
}
