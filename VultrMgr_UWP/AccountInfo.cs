using Imagine.Uwp.Json.Attributes;

namespace VultrMgr
{
    public class AccountInfo
    {
        /// <summary>
        /// Account
        /// </summary>
        [Sign("balance")]
        public string Balance { get; set; }
        [Sign("pending_charges")]
        public string Pending { get; set; }
        [Sign("last_payment_date")]
        public string PayDate { get; set; }
        [Sign("last_payment_amount")]
        public string PayAmount { get; set; }
        /// <summary>
        /// User
        /// </summary>
        [Sign("name")]
        public string Name { get; set; }
        [Sign("email")]
        public string Email { get; set; }

        public AccountInfo()
        {

        }
    }
}