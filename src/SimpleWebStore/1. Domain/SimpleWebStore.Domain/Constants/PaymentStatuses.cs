namespace SimpleWebStore.Domain.Constants
{
    public static class PaymentStatuses
	{
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "Approved for delayed payment";
        public const string PaymentStatusRejected = "Rejected";
    }
}
