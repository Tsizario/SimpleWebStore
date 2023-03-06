namespace SimpleWebStore.Domain.Constants
{
    public static class PaymentStatuses
	{
        public const string PaymentStatusPending = "Payment pending";
        public const string PaymentStatusApproved = "Payment approved";
        public const string PaymentStatusDelayedPayment = "Approved for delayed payment";
        public const string PaymentStatusCancelled = "Cancelled";        
        public const string PaymentStatusRefunded = "Refunded";        
    }    
}
